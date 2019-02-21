using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour {

    #region[instance]
    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [SerializeField]
    [Range(0f, 5f)]
    public int timeToStartWhenReady;
    [Header("Timer")]
    [Range(0f, 10f)]
    public int Minutes;
    [Range(0f,59f)]
    public int Seconds;
    [Space]
    [Header("Game References")]
    [Range(10f, 100f)]
    public int SpawnFullProbabilty = 50;
    public BarMode BM;
    //float SpawnFullProbabilty;
    public List<GameObject> CustomerPrefabs = new List<GameObject>();
    public List<Chair> Stools = new List<Chair>();
   // public List<CharacterTemplate> characterTemplates = new List<CharacterTemplate>();
    [Space]
    [Header("UI References")]
    public Text timerTxt;
    

    int currCountdownSeconds;
    int currCountdownMinutes;

    private void Start()
    {
        //SpawnFullProbabilty = ()
        //Debug.Log("p" + (SpawnFullProbabilty / 100.0f));
        CallMoreCustomers();
        //StartTheGame();
        //Timer
        timerTxt.text = Minutes.ToString("00") + ":" + Seconds.ToString("00"); //init the  timer text UI
    }

    void CallMoreCustomers() {
        //check for available stools
        List<Chair> availableStools = new List<Chair>();
        foreach (Chair ch in FindObjectsOfType<Chair>()) {
            if (ch.type == Chair.Type.Stool && ch.IsAvailable) {
                availableStools.Add(ch);
            }
        }

        #region[if there's no stool available then return]
        if (availableStools.Count == 0) {Debug.Log("All stools are occupied :/");return;}
        #endregion

        //will find a random number between the 74% and 100% of the available stools
        int rnd = Random.Range((int)(availableStools.Count * (float)(SpawnFullProbabilty / 100.0f)), availableStools.Count);
        //Debug.Log("Stools Available: " + availableStools.Count);
        //Debug.Log("Random number: " + rnd);
        Queue<GameObject> customersOutside = new Queue<GameObject>();
        for (int i = 0; i < rnd; i++)
        {
            customersOutside.Enqueue(CustomerPrefabs[Random.Range(0,CustomerPrefabs.Count - 1)]);
        }
        //Debug.Log("There are " + customersOutside.Count + "customer outside!");
        //spawn customer at the start of the level
        StartCoroutine(CustomerSpawner.instance.Spawn(customersOutside,availableStools));
    }

    //Timer coroutine that takes place every second
    public IEnumerator StartCountdown(int ISeconds, int IMinutes)
    {
        currCountdownSeconds = ISeconds; //temp variable to store the given seconds
        currCountdownMinutes = Minutes;  //temp variable to store the given minutes
        //int i = 0;
        int TotalSeconds = ISeconds + (Minutes * 60); //maximun amount of seconds that will last this loop
        string time = ""; // init the string for the UI
        while (TotalSeconds > 0)
        {
            yield return new WaitForSeconds(1.0f);
            TotalSeconds--;
            currCountdownSeconds--;
            time = currCountdownMinutes.ToString("00") + ":" + currCountdownSeconds.ToString("00");
            if (currCountdownSeconds == 0) { currCountdownSeconds = 60; }
            if (currCountdownSeconds == 60 && TotalSeconds > 0) { currCountdownMinutes--; }
            
            timerTxt.text = time;

        }
        Debug.Log("Time out");
    }

    /// <summary>
    /// Call this function when the game is ready to start
    /// </summary>
    public void StartTheGame() {
        StartCoroutine(StartCountdown(Seconds, Minutes)); // calls the timer coroutine
        BM.FindMatchs();
    }

    //plus means right , false mean left
    public bool CheckCharacterSide(int indexStool, bool plus) {

        if (indexStool == 0 && !plus) { return false; }
        if (indexStool == (Stools.Count - 1) && plus) { return false; }
        int nextStool = indexStool + 1;
        int prevStool = indexStool - 1;

        if (plus){
            if (Stools[nextStool].convosysAttached == null) { return false; } else {
                return Stools[nextStool].convosysAttached.chara.isAvailable;
            }
        }else {
            if (Stools[prevStool].convosysAttached == null) { return false; } else {
                return Stools[prevStool].convosysAttached.chara.isAvailable;
            }
            
        }

    }

     

    /*//instantiatea new conversation 
    void StartNewConversation(ConvoSystem CharacterA, ConvoSystem CharacterB) {
        CharacterA.chara.isAvailable = false;
        CharacterB.chara.isAvailable = false;
        CharacterA.OnFoundPartner(CharacterB);
        CharacterB.OnFoundPartner(CharacterA);
    }*/
}
