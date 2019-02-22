using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoSystem : MonoBehaviour {

    [HideInInspector]
    public Character chara;

    [HideInInspector]
    public int pascienceTime;
    //public ConvoSystem partner;
    [HideInInspector]
    public List<ConvoSystem> partners = new List<ConvoSystem>();
    public Chair currentStool;
    
    // public CustomerController cusctrl;
    private void Start()
    {
        chara = GetComponent<Character>();
    }
    //public int TimeToLookAgain;

    public void lookforConversation(Chair c) {
        
        LevelManager lvlmng = LevelManager.instance;
        currentStool = c;
        lvlmng.BM.WaitingList(this);


    }

    public void OnFoundPartner(ConvoSystem p) {
        partners.Add(p);
        transform.LookAt(p.transform);
        chara.isAvailable = false;
    }


}

