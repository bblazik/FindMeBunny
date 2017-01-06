using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerControll : NetworkBehaviour
{
    [SyncVar]
    public bool isLooking; 
    [SyncVar]
    private float counter;
    public Text DisplayText;
    public Camera c;
    [SyncVar]
    public GameObject Flashlight;
    [SyncVar]
    private float SleepOff = 5f;
    


    void Start()
    {
        //Initialize
        isLooking = false;
        // IF I'M THE PLAYER, STOP HERE (DON'T TURN MY OWN CAMERA OFF)
        if (isLocalPlayer) return;
        // DISABLE CAMERA AND CONTROLS HERE (BECAUSE THEY ARE NOT ME)
        c.enabled = false;
        //GetComponent<PlayerMovement>().enabled = false;
        
    }

    void Update()
    {
        counter = GameController.counter;

        Movement();
        CheckIfLooking();
        DisplayCounter();
    }
    
     
    void Movement() {
        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        

        //transform.Rotate(0, x, 0);s
            transform.Translate(x, 0, z);
    }
    
    void CheckIfLooking() {
        if (isLooking)
        {
            c.fieldOfView = 1;
        }

        if (isLooking && counter < 0)
        {
            c.fieldOfView = 60;
            Flashlight.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        else {
            Flashlight.SetActive(false);
        }
    }

    void DisplayCounter()
    {
        if (counter > 0)
        {
            DisplayText.enabled = true;
            DisplayText.text = "time to hide: " + counter.ToString();
        }
        else DisplayText.enabled = false;

        if (GameController.AllPlayersWereCatched() && counter < 0)
        {
            DisplayText.enabled = true;
            DisplayText.text = "All players were catched";

            //Initialize

            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().RpcReturnToLobby();
        }
    }
}