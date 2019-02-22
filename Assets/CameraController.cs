using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector3 initialPos;
    public Transform originalLookAt;
    Transform currentLookAt;

    Vector3 startPos;
    Vector3 endPos;

    public float speed = 1.0f;
    private float startTime;
    private float journeyLength;
    bool canMove = false;
   // bool wantToLookFront =false;
    

    void Start()
    {
        initialPos = this.transform.position;
        transform.LookAt(originalLookAt);
    }

    public void MoveToPoint(Vector3 destination,Transform Lookobj) {
        endPos = destination;
        startTime = Time.time;
        startPos = transform.position;
        currentLookAt = Lookobj;
        journeyLength = Vector3.Distance(startPos, endPos);
        canMove = true;
    }

    public void RestorePosition() {
        MoveToPoint(initialPos,originalLookAt);
        //wantToLookFront = true;
    }

    void Update()
    {
        // Distance moved = time * speed.
        if (canMove) {
            if (this.transform.position != endPos)
            {
                float distCovered = (Time.time - startTime) * speed;

                // Fraction of journey completed = current distance divided by total distance.
                float fracJourney = distCovered / journeyLength;
                transform.LookAt(currentLookAt);
                // Set our position as a fraction of the distance between the markers.
                transform.position = Vector3.Lerp(startPos, endPos, fracJourney);

            }
            
        }
        
        
    }
   
}
