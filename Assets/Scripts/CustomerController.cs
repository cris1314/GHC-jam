
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
	void Start () {
        cam = FindObjectOfType<Camera>();
        agent = this.GetComponent<NavMeshAgent>();
        convsys = this.GetComponent<ConvoSystem>();
        characterCollider = this.GetComponent<CapsuleCollider>();
        charactergb = this.GetComponent<Character>();
        initialLook = transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
        if (agent.velocity != Vector3.zero){
            charactergb.isWalking = true;
            if (Vector3.Distance(this.transform.position, lookPoint) > 0.5f) {
                
                transform.LookAt(lookPoint);

            }

        }else {
            charactergb.isWalking = false;
        }

        
	}

    public void GoTo(Transform point)
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(point.position);
        lookPoint = point.position;
        
    }

    public void GoAway() {
        transform.position = currentChair.PosA.position;
        charactergb.isSitting = false;
        charactergb.isAvailable = true;
        agent.enabled = true;
        GoTo(LevelManager.instance.Exit);
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
                currentChair.IsAvailable = true;
                currentChair.PosA.GetComponent<SphereCollider>().enabled = true;
                Destroy(gameObject);
                break;

        }
    }
}
