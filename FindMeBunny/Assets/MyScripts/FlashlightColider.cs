using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FlashlightColider : NetworkBehaviour {

    Ray landingRay;
    RaycastHit hit;
    public float deployment;


    void update()
    {
        
    }

void OnTriggerEnter(Collider other)
    {

        landingRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));

        debug();
        

        if (other.gameObject.CompareTag("Player"))
        {
            if (Physics.Raycast(landingRay, out hit, deployment))
            {
                if (hit.collider.tag == "Player" )
                {   
                    other.gameObject.GetComponent<PlayerControll>().isLooking = true;
                    Debug.DrawLine(hit.point, hit.point + Vector3.up * 5, Color.green);
                }
            }
        }
    }

    void debug()
    {
        if (Physics.Raycast(landingRay, out hit, deployment))
        {
            Debug.DrawLine(hit.point, hit.point + Vector3.up * 5, Color.red);
            Debug.Log("WTF IS THAT ");
            Debug.Log("  " + hit.collider.gameObject.name);
        }
    }
}
