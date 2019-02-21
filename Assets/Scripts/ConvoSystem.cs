using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoSystem : MonoBehaviour{

    [HideInInspector]
    public CharacterTemplate chara;
    [HideInInspector]
    public ConvoSystem partner;
    public Chair currentStool;
    // public CustomerController cusctrl;
    private void Start()
    {
        chara = GetComponent<CharacterTemplate>();
    }
    //public int TimeToLookAgain;

    public void lookforConversation(Chair c) {
        
        LevelManager lvlmng = LevelManager.instance;
        currentStool = c;
        lvlmng.BM.WaitingList(this);


    }

    public void OnFoundPartner(ConvoSystem p) {
        partner = p;
        Debug.Log(this.name + ": my partner is " + partner.name);
    }


}

