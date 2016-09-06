using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class GameController : NetworkBehaviour {
    public static float counter = 10;
    public static GameObject[] PlayersList;
    public static bool SwitchOffLight = false;

    // Use this for initialization
    void Start ()
    {
        SwitchOffLight = false;

        SetLookingPlayer();
    }
    //[Server]
    void Update()
    {
        SwitchOffLights();
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

    void SwitchOffLights()
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

}
