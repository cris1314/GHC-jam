/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barStool : MonoBehaviour
{
    public Character myClient;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Client")
        {
            myClient = other.GetComponent<Character>();
            myClient.isSitting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Client")
        {
            myClient = other.GetComponent<Character>();
            myClient.isSitting = false;
        }
    }
}*/
