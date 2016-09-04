using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FlashlightColider : NetworkBehaviour {

    Ray landingRay = new Ray(transform.position, Vector3.forward);
    public float deployment;
    RaycastHit hit; 

    void OnTriggerEnter(Collider other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("Player"))
        {
            Physics.Raycast(landingRay, out hit, deployment);
            //other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            other.gameObject.GetComponent<PlayerControll>().isLooking = true;
            Debug.DrawLine(ray.origin, hit.point);
        }
        //print("I see object");

    }
}
