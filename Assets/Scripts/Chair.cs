using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour {
    public enum Type {Stool,Chair};
    public Type type;
    public Transform PosA;
    public Transform PosB;
    public Transform DefaultPointToLook;
    public ConvoSystem convosysAttached;
    
   // public int timeToAvailable;
    //CustomerController currentCustomer;
    private bool isAvailable = true;

    public bool IsAvailable
    {
        get
        {
            return this.isAvailable;
        }
        set {
            isAvailable = value;
        }
        
    }

    public void Occupied(ConvoSystem cvs) {
        convosysAttached = cvs;
        isAvailable = false;
        PosA.GetComponent<SphereCollider>().enabled = false;
    }

    public IEnumerator CustomerStandUp() {
        isAvailable = true;
        yield return new WaitForSeconds(2f);
        Debug.Log("EMpty");
        PosA.GetComponent<SphereCollider>().enabled = true;

    }

}
