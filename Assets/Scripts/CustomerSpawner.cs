using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour {

    public static CustomerSpawner instance;
    [Range(1f,10f)]
    public int maxTimeToSpawn = 1;
    int spawnCount;
    Queue<GameObject> Customers = new Queue<GameObject>();
    private void Awake()
    {
        instance = this;
    }

    //Spawn New customers
    public IEnumerator Spawn(Queue<GameObject> NewCustomers,List<Chair> availableStools) {
        Customers = NewCustomers;
        Queue<Chair> stools = new Queue<Chair>();
        List<int> usedChairs = new List<int>();
        //int maxNumberOfTemplates = LevelManager.instance.characterTemplates.Count - 1;
        int possibleindex;

        for (int i = 0; i < Customers.Count; i++)
        {
            do
            {
                possibleindex = Random.Range(0, availableStools.Count);
            } while (usedChairs.Contains(possibleindex));
            usedChairs.Add(possibleindex);
            stools.Enqueue(availableStools[possibleindex]);
        }
        yield return new WaitForEndOfFrame();
        while (Customers.Count > 0)
        {
            GameObject newCustomer =  Instantiate(Customers.Dequeue(), this.transform.position, Quaternion.identity);
            Vector3 stoolPosition = stools.Dequeue().PosA.position;
            newCustomer.GetComponent<CustomerController>().GoTo(stoolPosition);
            //CharacterTemplate template = LevelManager.instance.characterTemplates[Random.Range(0, maxNumberOfTemplates)];
            //newCustomer.GetComponent<ConvoSystem>().chara = new Character(template);
            newCustomer.name = Customers.Count.ToString();
            yield return new WaitForSeconds(Random.Range(1,maxTimeToSpawn));
        }

        StartCoroutine(ReadyToStart());
        
    }

    IEnumerator ReadyToStart() {
        yield return new WaitForSeconds(LevelManager.instance.timeToStartWhenReady);
        LevelManager.instance.StartTheGame();
    }
	
}
