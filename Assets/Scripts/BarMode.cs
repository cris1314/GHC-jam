using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarMode : MonoBehaviour {


    [Range(10.0f, 100.0f)]
    public int minimunAcceptancetoMatch = 50;

    public int PointsTowin;
    public int currentPoints;

    public Slider ScoreBar;
    public List<ConvoSystem> AloneList = new List<ConvoSystem>();
    public List<ConversationBar> CurrentConversations = new List<ConversationBar>();
    public GameObject inviteAdrinkPanel;
    private FMODUnity.StudioEventEmitter eventEmitterRef;
    public FMODUnity.StudioEventEmitter yesSFX;
    public FMODUnity.StudioEventEmitter noSFX;
    [HideInInspector]
    public int selectedConversation;
    [HideInInspector]

    private void Awake()
    {
        eventEmitterRef = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public bool OnSelection = false;

    public void WaitingList(ConvoSystem cs) {
        AloneList.Add(cs);

    }


    public void FindMatchs() {
        Debug.Log(AloneList.Count);
        Debug.Log("Finding Match...");
        eventEmitterRef.Play();
        Queue<ConvoSystem> WaitingCustomers = new Queue<ConvoSystem>();
        int possibleindex;
        int timesForLoop = AloneList.Count;

        for (int i = 0; i < timesForLoop; i++)
        {
            do
            {
                possibleindex = Random.Range(0, timesForLoop);
            } while (WaitingCustomers.Contains(AloneList[possibleindex]));
          
            WaitingCustomers.Enqueue(AloneList[possibleindex]);
        }

        while (WaitingCustomers.Count > 0) {
       // foreach (ConvoSystem currentCS in AloneList) { 
            int i = Random.Range(0, WaitingCustomers.Count - 1);
            ConvoSystem currentCS = WaitingCustomers.Dequeue();
            if (currentCS.chara.isAvailable)
            {

                int stoolIndex = LevelManager.instance.Stools.IndexOf(currentCS.currentStool);
                bool right = LevelManager.instance.CheckCharacterSide(stoolIndex, true);
                bool left = LevelManager.instance.CheckCharacterSide(stoolIndex, false);
                //if both true
                //Debug.Log("available begin" + currentCS.name);
                if (right && left)
                {
                    //Debug.Log("Both Sides" + currentCS.name);
                    int choosenSide = Random.Range(0, 1);
                    switch (choosenSide)
                    {
                        case 0:
                            left = true;
                            right = false;
                            break;
                        case 1:
                            left = false;
                            right = true;
                            break;
                    }
                }

                if (left)
                {
                    // Debug.Log("left Sides" + currentCS.name);
                    ConvoSystem matchPartner = LevelManager.instance.Stools[stoolIndex - 1].convosysAttached;
                    if (!currentCS.partners.Contains(matchPartner)) {
                        StartNewConversation(currentCS, LevelManager.instance.Stools[stoolIndex - 1].convosysAttached);
                    }
                }
                else if (right)
                {
                    //Debug.Log("right Sides" + currentCS.name);
                    ConvoSystem matchPartner = LevelManager.instance.Stools[stoolIndex + 1].convosysAttached;
                    if (!currentCS.partners.Contains(matchPartner))
                    {
                        StartNewConversation(currentCS, LevelManager.instance.Stools[stoolIndex + 1].convosysAttached);

                    }
                }

            }

        }
    }

    void StartNewConversation(ConvoSystem CharacterA, ConvoSystem CharacterB)
    {
      
        AloneList.Remove(CharacterA);
        AloneList.Remove(CharacterB);
        int idA = LevelManager.instance.Stools.IndexOf(CharacterA.currentStool);
        int idB = LevelManager.instance.Stools.IndexOf(CharacterB.currentStool);

        ConversationBar CB;
        if (idA < idB) {
            CB = CurrentConversations[idA];
        } else {
            CB = CurrentConversations[idB];
        }
        CB.gameObject.SetActive(true);
        CB.SetConversationBar(CharacterA,CharacterB);
        CharacterA.OnFoundPartner(CharacterB);
        CharacterB.OnFoundPartner(CharacterA);
        
    }

    public void RequestSelection(ConversationBar cbtemp)
    {
        selectedConversation = CurrentConversations.IndexOf(cbtemp);
        inviteAdrinkPanel.SetActive(true);
        OnSelection = true;
    }


    public void InviteADrink() {
        yesSFX.Play();
        inviteAdrinkPanel.SetActive(false);
        CurrentConversations[selectedConversation].SelectionLight.enabled = false;
        LevelManager.instance.turnLights(true);
        StartCoroutine(CurrentConversations[selectedConversation].DetermineMatch());
        //selectedConversation = null;
        OnSelection = false;
        LevelManager.instance.cam.GetComponent<CameraController>().RestorePosition();
        //selectedConversation = 0;
    }

    

    public void DontInviteADrink()
    {
        noSFX.Play();
        inviteAdrinkPanel.SetActive(false);
        CurrentConversations[selectedConversation].SelectionLight.enabled = false;
        LevelManager.instance.turnLights(true);
        OnSelection = false;
        LevelManager.instance.cam.GetComponent<CameraController>().RestorePosition();

    }

    public void AddPoints() {
        currentPoints++;
        calculateProgressBar();
        if (currentPoints >= PointsTowin) {
            LevelManager.instance.GameWon();
        }
    }

    public void RestPoints() {
        if (currentPoints > 0) {
            currentPoints--;
            calculateProgressBar();
        }
    }

    void calculateProgressBar() {
        float barValue = ((currentPoints * 100) / (PointsTowin)) / 100.0f;
        ScoreBar.value = barValue;
    }
}
