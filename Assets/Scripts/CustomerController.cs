
using UnityEngine;
using UnityEngine.AI;
public class CustomerController : MonoBehaviour {

    Camera cam;
    NavMeshAgent agent;
	// Use this for initialization
	void Start () {
        cam = FindObjectOfType<Camera>();
        agent = this.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
           Ray ray = cam.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                agent.SetDestination(hit.point);
            }
        }
	}
}
