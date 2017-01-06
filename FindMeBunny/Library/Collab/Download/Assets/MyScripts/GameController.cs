using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GameController : NetworkBehaviour {
    public static float counter = 10;
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

        SetLookingPlayer();
    }
    //[Server]
    void Update()
    {
        RpcSwitchOffLights();
        //print(PlayersList.Length);
    }
    void SetLookingPlayer()
    {
        PlayersList = GameObject.FindGameObjectsWithTag("Player");

        if (PlayersList.Length > 0)
        {
            PlayersList[0].layer = LayerMask.NameToLayer("Ignore Raycast");
            PlayersList[0].GetComponent<PlayerControll>().isLooking = true;
        }
        Debug.Log("PlayersList.Length: " + PlayersList.Length);
    }

    
    public static bool AllPlayersWereCatched()
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
        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        if (counter < 0 && SwitchOffLight == false)
        {
            GameObject.FindGameObjectWithTag("MainLight").SetActive(false);
            //GameObject.FindGameObjectWithTag("roof").SetActive(true);
            SwitchOffLight = true;
        }
    }

    [RPC]
    public void RpcReturnToLobby()
    {
        StartCoroutine(ServerCountdownCoroutine());
        if (Called == false)
        {
            //GameController.counter = 10;
            //GameController.SwitchOffLight = false;
            //RpcReturnToLobby();
            GameObject.FindGameObjectWithTag("Lobby").GetComponent<Prototype.NetworkLobby.LobbyManager>().SendReturnToLobby();
            Called = true;
        }
    }
            
    public IEnumerator ServerCountdownCoroutine()
    {
        float remainingTime = 5.0f;
        int floorTime = Mathf.FloorToInt(remainingTime);

        while (remainingTime > 0)
        {
            yield return null;

            remainingTime -= Time.deltaTime;
            int newFloorTime = Mathf.FloorToInt(remainingTime);

            if (newFloorTime != floorTime)
            {//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                floorTime = newFloorTime;

                for (int i = 0; i < PlayersList.Length; ++i)
                {
                    if (PlayersList[i] != null)
                    {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                        //(PlayersList[i] as GameObject).RpcDisplay
                    }
                }
            }
        }
        
    }

    
}
