using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using Prototype.NetworkLobby;
using UnityEngine.Networking.Match;


public class GameController : NetworkBehaviour {
    public static float counter = 10;
    public static float counterToFinish = 5;
    public static GameObject[] PlayersList;
    public static bool SwitchOffLight = false;
    private static bool Called = false;
    //public Text DisplayText;
    // Use this for initialization
    void Start ()
    {
        Called = false;
        counter = 10;
        SwitchOffLight = false;
    }
    
    void Update()
    {
        //Game is created already in queue so I need to check if game really started or not.
        if (!GameObject.FindGameObjectWithTag("Lobby")
                .GetComponent<Prototype.NetworkLobby.LobbyManager>()
                .IsClientConnected()) return;

        PlayersList = GameObject.FindGameObjectsWithTag("Player");
        RpcSwitchOffLights();
        RpcDisplayCounter(ref counter, "Time to hide: ");
        RpcSetLookingPlayer(
        GameObject.FindGameObjectsWithTag("Player")
            .OrderBy(go => go.GetComponent<NetworkIdentity>().netId.Value)
            .ToArray());

        if (AllPlayersWereCatched() && counter < 0)
        {
            RpcDisplayCounter(ref counterToFinish, "Reset at: ");
            if(calculateCounter(ref counterToFinish) < 0)
                CmdReturnToLobby();
        }
    }
    [Command] 
    void CmdSetLookingPlayer()
    {
        PlayersList = GameObject.FindGameObjectsWithTag("Player").OrderBy(go => go.GetComponent<NetworkIdentity>().netId).ToArray();
        RpcSetLookingPlayer(PlayersList);
        //Debug.Log("Server: " + PlayersList[0].GetComponent<NetworkIdentity>().netId + " : " + PlayersList[1].GetComponent<NetworkIdentity>().netId);
    }

    //[ClientRpc]
    void RpcSetLookingPlayer(GameObject [] List)
    {
        PlayersList = List;
        foreach(GameObject p in PlayersList) {
            Debug.Log("UnetId: " +p.GetComponent<NetworkIdentity>().netId);
        }

        if (PlayersList.Length > 0)
        {
            PlayersList[0].layer = LayerMask.NameToLayer("Ignore Raycast");
            PlayersList[0].GetComponent<PlayerControll>().isLooking = true;
        }
    }

    
    public  bool AllPlayersWereCatched()
    {
        PlayersList = GameObject.FindGameObjectsWithTag("Player");
        if (PlayersList.Length > 0)
        {      
            foreach (GameObject p in PlayersList)
            {
                if (p.GetComponent<PlayerControll>().isLooking == false)
                    return false;
            }
        }
        
        return true;
    }
    //Dodanie RPC skutkuje tym ze jest mniejsza roznica w counterach na klientach
    [RPC]
    void RpcSwitchOffLights()
    {
        calculateCounter(ref counter);
        if (counter < 0 && SwitchOffLight == false)
        {
            GameObject.FindGameObjectWithTag("MainLight").SetActive(false);
            //GameObject.FindGameObjectWithTag("roof").SetActive(true);
            SwitchOffLight = true;
        }
    }
    [Command]
    void CmdReturnToLobby()
    {
        RpcReturnToLobby();
    }

    float calculateCounter(ref float counter )
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        return counter;
    }

    [RPC]
    public void RpcReturnToLobby()
    {
        if (Called == false && isServer)
        {
            GameObject.FindGameObjectWithTag("Lobby").GetComponent<Prototype.NetworkLobby.LobbyManager>().SendReturnToLobby();
            Called = true;
        }
    }
    
    [RPC]
    void RpcDisplayCounter(ref float counter, String text)
    { 
        
        if (PlayersList.Length != 0 && Called == false)
            foreach (GameObject p in PlayersList)
            {
                if (counter > 0)
                {
                    p.GetComponent<PlayerControll>().DisplayText.enabled = true;
                    p.GetComponent<PlayerControll>().DisplayText.text = text + counter.ToString();
                }
                else p.GetComponent<PlayerControll>().DisplayText.enabled = false;

            }
    }//EndDisplayCounter

    private static int SortByName(GameObject o1, GameObject o2)
    {
        return o1.name.CompareTo(o2.name);
    }
}
