using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class CustomerController : MonoBehaviour {

    Camera cam;
    NavMeshAgent agent;
    ConvoSystem convsys;
    Character charactergb;
    bool isOnStool = false;
    Chair currentChair;
    [HideInInspector]
    public Collider characterCollider;
    Vector3 lookPoint;
    Vector3 initialLook;


    // Use this for initialization
    void Start() {
        cam = FindObjectOfType<Camera>();
        agent = this.GetComponent<NavMeshAgent>();
        convsys = this.GetComponent<ConvoSystem>();
        characterCollider = this.GetComponent<CapsuleCollider>();
        charactergb = this.GetComponent<Character>();
        initialLook = transform.forward;
    }

    // Update is called once per frame
    void Update() {
        if (agent.velocity != Vector3.zero) {
            charactergb.isWalking = true;
            if (Vector3.Distance(this.transform.position, lookPoint) > 0.5f) {

                transform.LookAt(lookPoint);

            }

        } else {
            charactergb.isWalking = false;
        }


    }

    public void GoTo(Transform point)
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(point.position);
        lookPoint = point.position;
        //charactergb.isAvailable = true;
    }

    public void GoAway() {
        transform.position = currentChair.PosA.position;
        charactergb.isSitting = false;

        agent.enabled = true;
        GoTo(LevelManager.instance.Exit);
    }

    public void KeepLooking()
    {
        transform.position = currentChair.PosA.position;
        //charactergb.isSitting = false;
        charactergb.isAvailable = true;
        transform.position = currentChair.PosB.position;
        transform.LookAt(currentChair.DefaultPointToLook);
        LevelManager.instance.BM.AloneList.Add(convsys);
        StartCoroutine(Pascience());
    }

    IEnumerator Pascience() {
        yield return new WaitForSeconds(convsys.pascienceTime);
        if (charactergb.isAvailable)
        {
            charactergb.isAvailable = false;
            LevelManager.instance.BM.AloneList.Remove(convsys);
            GoAway();
        }
        

        
    } 

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag) {
            case "SittingPoint":
                Chair ch = other.transform.parent.GetComponent<Chair>();
                if (ch.IsAvailable) {
                    currentChair = ch;
                    agent.enabled = false;
                    transform.position = currentChair.PosB.position;
                    //isOnStool = true;
                    charactergb.isSitting = true;
                    currentChair.Occupied(convsys);
                    convsys.lookforConversation(currentChair);
                    transform.LookAt(currentChair.DefaultPointToLook);
                    
                }
                break;
            case "Exit":
                Debug.Log("BYE");
                foreach (ConvoSystem cs in convsys.partners) {
                    cs.partners.Remove(convsys);
                }
                currentChair.IsAvailable = true;
                currentChair.PosA.GetComponent<SphereCollider>().enabled = true;
                Destroy(gameObject);
                break;

        }



    
    }


    #region[FMOD]
    public void FootSteeps(string path)
    {

       
            FMODUnity.RuntimeManager.PlayOneShot(path, charactergb.r_foot.gameObject.transform.position);
        
           //FMODUnity.RuntimeManager.PlayOneShot(path, charactergb.l_foot.gameObject.transform.position);
        
    }
    #endregion
}
