
using UnityEngine;
using UnityEngine.AI;
public class CustomerController : MonoBehaviour {

    Camera cam;
    NavMeshAgent agent;
    ConvoSystem convsys;
    bool isOnStool = false;
    Chair currentChair;
    [HideInInspector]
    public Collider characterCollider;
	// Use this for initialization
	void Start () {
        cam = FindObjectOfType<Camera>();
        agent = this.GetComponent<NavMeshAgent>();
        convsys = this.GetComponent<ConvoSystem>();
        characterCollider = this.GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	/*void Update () {
        /*if (Input.GetMouseButtonDown(0)) {
            if (isOnChair) {
                transform.position = currentChair.PosA.position;
                agent.enabled = true;
                isOnChair = false;
                StartCoroutine(currentChair.CustomerStandUp());
            }else{
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    agent.SetDestination(hit.point);
                }
            }
           
        }
	}*/

    public void GoTo(Vector3 point)
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(point);
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
                    isOnStool = true;

                    currentChair.Occupied(convsys);
                    convsys.lookforConversation(currentChair);
                }
                break;
        }
    }
}
