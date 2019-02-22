using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    [Range(5f, 20f)]
    public int minTimeForNextWave;
    [Range(5f, 20f)]
    public int maxTimeForNextWave;

    [Range(5f, 20f)]
    public int minPascienceTime;
    [Range(5f, 20f)]
    public int maxPascienceTime;
    [Header("Timer")]
    [Range(0f, 10f)]
    public int Minutes;
    [Range(0f,59f)]
    public int Seconds;
    [Space]
    [Header("Game References")]
    public Camera cam;
    [Range(10f, 100f)]
    public int SpawnFullProbabilty = 50;
    public BarMode BM;
    //float SpawnFullProbabilty;
    public List<GameObject> CustomerPrefabs = new List<GameObject>();
    public List<Chair> Stools = new List<Chair>();
    public List<Light> Lamps = new List<Light>();

    public FMODUnity.StudioEventEmitter JukeBoxSFX;
    
    // public List<CharacterTemplate> characterTemplates = new List<CharacterTemplate>();
    public Transform Exit;
    [Space]
    [Header("UI References")]
    public Text timerTxt;

    bool gamehasstarted = false;
    int currCountdownSeconds;
    int currCountdownMinutes;

    private void Start()
    {
        
        CallMoreCustomers();
        
        timerTxt.text = Minutes.ToString("00") + ":" + Seconds.ToString("00"); //init the  timer text UI
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "ConversationSelect" && !BM.OnSelection)
                {
                    hit.transform.gameObject.GetComponent<ConversationBar>().SelectConversation();
                }
                //Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
    }

    void CallMoreCustomers() {
        //check for available stools
        List<Chair> availableStools = new List<Chair>();
        foreach (Chair ch in FindObjectsOfType<Chair>()) {
            if (ch.type == Chair.Type.Stool && ch.IsAvailable) {
                //Debug.Log("Available!");
             
                availableStools.Add(ch);
            }
        }

        #region[if there's no stool available then return]
        if (availableStools.Count == 0) {Debug.Log("All stools are occupied :/");return;}
        #endregion

        //will find a random number between the 74% and 100% of the available stools
        //int rnd = Random.Range((int)(availableStools.Count * (float)(SpawnFullProbabilty / 100.0f)), availableStools.Count);
        int rnd = Random.Range(1, availableStools.Count);
        //Debug.Log("Stools Available: " + availableStools.Count);
        //Debug.Log("Random number: " + rnd);
        /*Queue<GameObject> customersOutside = new Queue<GameObject>();
        for (int i = 0; i < rnd; i++)
        {
            customersOutside.Enqueue(CustomerPrefabs[Random.Range(0,CustomerPrefabs.Count - 1)]);
        }*/
        //Debug.Log("There are " + customersOutside.Count + "customer outside!");
        //spawn customer at the start of the level
        StartCoroutine(Randomizer.instance.Spawn(rnd,availableStools));
    }

    //Timer coroutine that takes place every second
    public IEnumerator StartCountdown(int ISeconds, int IMinutes)
    {
        JukeBoxSFX.Play();
        yield return new WaitForSeconds(1);
        cam.GetComponent<FMODUnity.StudioEventEmitter>().Play();

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

    IEnumerator Waves() {
        yield return new WaitForSeconds(Random.Range(minTimeForNextWave,maxTimeForNextWave));
        CallMoreCustomers();
        StartCoroutine(Waves());
    }

    /// <summary>
    /// Call this function when the game is ready to start
    /// </summary>
    public void StartTheGame() {
        if (!gamehasstarted)
        {
            gamehasstarted = true;
            StartCoroutine(StartCountdown(Seconds, Minutes)); // calls the timer coroutine
            StartCoroutine(Waves());
        }
        
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

    public void turnLights(bool OnOff)
    {
        foreach (Light l in Lamps) {
            l.enabled = OnOff;
        }

    }


    public void GameWon() {
        SceneManager.LoadScene(0);

    }


}
