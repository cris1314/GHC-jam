using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour {

    public float timeToDestroy;
	// Use this for initialization
	void Start () {
        Invoke("Erase",timeToDestroy);
    }


    void Erase() {
        Destroy(gameObject);
    }
	
}
