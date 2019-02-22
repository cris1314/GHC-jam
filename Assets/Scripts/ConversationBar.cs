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
    public Transform cameraNewPos;
    public Transform cameraNewLookAt;
    bool canSelect = false;

    List<Likes> chALikes = new List<Likes>(); 
    List<Likes> chBLikes = new List<Likes>();
    //int chatTimer;

    int turn;
    //private void Start()
    //{
      //  positiveFeedback = positiveMatchFeedback.GetComponent<Animator>();
        //negativeFeedback = negativeMatchFeedback.GetComponent<Animator>();
    //}

    public  void SetConversationBar(ConvoSystem a, ConvoSystem b) {
        CharacterA = a;
        CharacterB = b;
        canSelect = true;
        for (int i = 0; i < CharacterA.chara.likes.Count; i++)
        {
            if (CharacterA.chara.likes[i].like)
            {
                if (CharacterB.chara.likes[i].like) {
                    CharacterB.chara.Goods++;
                }
                    chALikes.Add(CharacterA.chara.likes[i]);
            }
        }

        for (int j = 0; j < CharacterB.chara.likes.Count; j++)
        {
            if (CharacterB.chara.likes[j].like)
            {
                if (CharacterA.chara.likes[j].like)
                {
                    CharacterA.chara.Goods++;
                }
                chBLikes.Add(CharacterB.chara.likes[j]);
            }
        }
        
        turn = Random.Range(0,2);
        StartCoroutine(Conversation());
    }

    IEnumerator Conversation() {
        Character chA;
        Character chB;
        //if (turn > 0)
        //{
            chA = CharacterA.chara;
            chB = CharacterB.chara;
            float chatTimerA = Random.Range(chA.opinionRate.x, chA.opinionRate.y);
            float chatTimerB = Random.Range(chB.opinionRate.x, chB.opinionRate.y);

            int idA = Random.Range(0, chALikes.Count);
            chA.CommentSomething(chA.likes.IndexOf(chALikes[idA]));
            yield return new WaitForSeconds(1);
            chB.GiveOpinion(chA.likes.IndexOf(chALikes[idA]));
            yield return new WaitForSeconds(chatTimerB);

            int idB = Random.Range(0, chBLikes.Count);
            //Debug.Log("");
            chB.CommentSomething(chB.likes.IndexOf(chBLikes[idB]));
            yield return new WaitForSeconds(1);
            chA.GiveOpinion(chB.likes.IndexOf(chBLikes[idB]));

           


        yield return new WaitForSeconds(chatTimerA);
            StartCoroutine(Conversation());
       /* }
        else {
            chA = CharacterB.chara;
            chB = CharacterA.chara;
            float chatTimerA = Random.Range(chA.opinionRate.x, chA.opinionRate.y);
            float chatTimerB = Random.Range(chB.opinionRate.x, chB.opinionRate.y);
            int idA = Random.Range(0, chBLikes.Count);
            chA.CommentSomething(chA.likes.IndexOf(chBLikes[idA]));
            yield return new WaitForSeconds(1);
            chB.GiveOpinion(chA.likes.IndexOf(chBLikes[idA]));
            yield return new WaitForSeconds(chatTimerB);

            int idB = Random.Range(0, chALikes.Count);

            chB.CommentSomething(chB.likes.IndexOf(chALikes[idB]));
            yield return new WaitForSeconds(1);
            chA.GiveOpinion(chB.likes.IndexOf(chALikes[idB]));

            yield return new WaitForSeconds(chatTimerA);
            StartCoroutine(Conversation());
        }*/


        //Debug.Log(chALikes.Count + " " + chA.name);
       
    }

    public void SelectConversation() {
        if (!canSelect) { return; }
        //Debug.Log("Selected " + this.name);
        LevelManager.instance.cam.GetComponent<CameraController>().MoveToPoint(cameraNewPos.position,cameraNewLookAt);
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
        //int ABads = CharacterA.chara.Bads;
        int BGoods = CharacterB.chara.Goods;
        //int BBads = CharacterB.chara.Bads;
        /*if ((AGoods + ABads) < 2 || (BGoods + BBads) < 2)
        {
            Match(false);
            Debug.Log("They Cannot know more each other");
            yield break;
        } */
        
        //int Apleased = (AGoods * 100) / (AGoods + ABads);
        //int Bpleased = (BGoods * 100) / (BGoods + BBads);
        //Debug.Log("A : " + Apleased);
        //Debug.Log("B : " + Bpleased);
        int minimumPlease = LevelManager.instance.BM.minimunAcceptancetoMatch;
        yield return new WaitForSeconds(2);
        
        if (AGoods >= minimumPlease && BGoods >= minimumPlease)
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
        if (result)
        {
            CharacterA.gameObject.GetComponent<CustomerController>().GoAway();
            CharacterB.gameObject.GetComponent<CustomerController>().GoAway();
            
        }
        else {
            CharacterA.gameObject.GetComponent<CustomerController>().KeepLooking();
            CharacterB.gameObject.GetComponent<CustomerController>().KeepLooking();
           

        }
        canSelect = false;
        this.GetComponent<BoxCollider>().enabled = false;
        Invoke("Unable", 2.0f);

    }

    void Unable() {
        this.gameObject.SetActive(false);
    }

}
