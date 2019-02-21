using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMode : MonoBehaviour {
    
    public List<ConvoSystem> AloneList = new List<ConvoSystem>();
    public List<ConversationBar> CurrentConversations = new List<ConversationBar>();
    public void WaitingList(ConvoSystem cs) {
        AloneList.Add(cs);

    }


    public void FindMatchs() {
        Debug.Log(AloneList.Count);
        Debug.Log("Finding Match...");
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
                    StartNewConversation(currentCS, LevelManager.instance.Stools[stoolIndex - 1].convosysAttached);
                }
                else if (right)
                {
                    //Debug.Log("right Sides" + currentCS.name);
                    StartNewConversation(currentCS, LevelManager.instance.Stools[stoolIndex + 1].convosysAttached);
                }

            }
            else {
                //Debug.Log("not available" + currentCS.name);
            }

        }
    }

    void StartNewConversation(ConvoSystem CharacterA, ConvoSystem CharacterB)
    {
       
        AloneList.Remove(CharacterA);
        AloneList.Remove(CharacterB);
        CharacterA.chara.isAvailable = false;
        CharacterB.chara.isAvailable = false;



        //CharacterA.OnFoundPartner(CharacterB);
        //CharacterB.OnFoundPartner(CharacterA);
    }

}
