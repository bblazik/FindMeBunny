using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FlashlightColider : NetworkBehaviour {

    Ray landingRay;
    RaycastHit hit;
    public float deployment;


    void update()
    {
        
        //landingRay = new Ray(transform.position, Vector3.forward);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * deployment);
       // Debug.DrawLine(hit.point, hit.point + Vector3.up * 5, Color.red);
    }

void OnTriggerEnter(Collider other)
    {

        landingRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        if (Physics.Raycast(landingRay, out hit, deployment))
        {
            Debug.DrawLine(hit.point, hit.point + Vector3.up * 5, Color.red);
        }
            //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("Player"))
        {
            if (Physics.Raycast(landingRay, out hit, deployment))
            {
                if(hit.collider.tag == "Player" && hit.collider.tag != "wall")
                {   
                    other.gameObject.GetComponent<PlayerControll>().isLooking = true;
                    Debug.DrawLine(hit.point, hit.point + Vector3.up * 5, Color.green);
                }
            }
            //Debug.Log("  " + hit.collider.gameObject.name);
            //other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            //sother.gameObject.GetComponent<PlayerControll>().isLooking = true;

            // Debug.DrawRay(transform.position, Vector3.forward * deployment);
        }
        //print("I see object");

    }
}
