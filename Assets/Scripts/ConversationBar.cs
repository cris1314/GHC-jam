using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationBar : MonoBehaviour {

    [HideInInspector]
    public ConvoSystem CharacterA;
    [HideInInspector]
    public ConvoSystem CharacterB;

    public GameObject positiveMatchFeedback;
    public GameObject negativeMatchFeedback;
    public Light SelectionLight;
    //Animator positiveFeedback;
    //Animator negativeFeedback;
    
    //int turn;
    //private void Start()
    //{
      //  positiveFeedback = positiveMatchFeedback.GetComponent<Animator>();
        //negativeFeedback = negativeMatchFeedback.GetComponent<Animator>();
    //}

    public  void SetConversationBar(ConvoSystem a, ConvoSystem b) {
        CharacterA = a;
        CharacterB = b;

        
        //turn = Random.Range(0,1);
        //StartCoroutine(Conversation());
    }

    public void SelectConversation() {
        //Debug.Log("Selected " + this.name);
        LevelManager.instance.turnLights(false);
        turnLight();
        LevelManager.instance.BM.RequestSelection(this);
        //LevelManager.instance.BM.inviteAdrinkPanel.SetActive(true);
       // LevelManager.instance.BM.selectedConversation = this;
    }

    public void turnLight() {
        foreach (ConversationBar cb in FindObjectsOfType<ConversationBar>()) {
            if (cb != this) {
                cb.SelectionLight.enabled = false;
            }
           
        }
        SelectionLight.enabled = true;
    }

    public IEnumerator DetermineMatch() {
        CharacterA.chara.canTalk = false;
        CharacterB.chara.canTalk = false;

        int AGoods = CharacterA.chara.Goods;
        int ABads = CharacterA.chara.Bads;
        int BGoods = CharacterB.chara.Goods;
        int BBads = CharacterB.chara.Bads;
        if ((AGoods + ABads) < 2 || (BGoods + BBads) < 2)
        {
            Match(false);
            Debug.Log("They Cannot know more each other");
            yield break;
        } 
        
        int Apleased = (AGoods * 100) / (AGoods + ABads);
        int Bpleased = (BGoods * 100) / (BGoods + BBads);
        Debug.Log("A : " + Apleased);
        Debug.Log("B : " + Bpleased);
        int minimumPlease = LevelManager.instance.BM.minimunAcceptancetoMatch;
        yield return new WaitForSeconds(2);
        
        if (Apleased >= minimumPlease && Bpleased >= minimumPlease)
        {
            Match(true);
        }else {
            Match(false);
        }

    }
    private void OnEnable()
    {
        this.GetComponent<BoxCollider>().enabled = true;

    }
    void Match(bool result) {
        if (result)
        {
            GameObject gFeedback = Instantiate(positiveMatchFeedback, this.transform);
            //gFeedback.transform.SetParent(gFeedback.transform.parent.parent);
            LevelManager.instance.BM.AddPoints();
        }else {
            GameObject gFeedback = Instantiate(negativeMatchFeedback, this.transform);
            //gFeedback.transform.SetParent(gFeedback.transform.parent.parent);

            LevelManager.instance.BM.RestPoints();
        }

        CharacterA.chara.isAvailable = false;
        CharacterB.chara.isAvailable = false;

        CharacterA.gameObject.GetComponent<CustomerController>().GoAway();
        CharacterB.gameObject.GetComponent<CustomerController>().GoAway();
        this.GetComponent<BoxCollider>().enabled = false;
        Invoke("Unable", 2.0f);
    }

    void Unable() {
        this.gameObject.SetActive(false);
    }

}
