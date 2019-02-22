using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public static Randomizer instance;
    /// <summary>
    /// Gabs Variables :)
    /// </summary>
    public GameObject characterPrefab;
    public GameObject[] hairPrefabs;
    public GameObject[] glassesPrefabs;
    public Color[] skinTones;
    public Color[] hairTones;
    public Color[] shoeColors;
    public Color[] glassesColors;
    public Texture[] faces;
    public Texture[] bodies;
    public Texture[] shoes;
    public Texture[] drinks;

    //-----------------
    [Range(1f, 10f)]
    public int maxTimeToSpawn = 1;
    int spawnCount;
    int CustomersQuantity;
    private void Awake()
    {
        instance = this;
    }

    //Spawn New customers
    public IEnumerator Spawn(int NewCustomersInComing, List<Chair> availableStools)
    {
        CustomersQuantity = NewCustomersInComing;
        Queue<Chair> stools = new Queue<Chair>();
        List<int> usedChairs = new List<int>();
        //int maxNumberOfTemplates = LevelManager.instance.characterTemplates.Count - 1;
        int possibleindex;

        for (int i = 0; i < CustomersQuantity; i++)
        {
            do
            {
                possibleindex = Random.Range(0, availableStools.Count);
            } while (usedChairs.Contains(possibleindex));
            usedChairs.Add(possibleindex);
            stools.Enqueue(availableStools[possibleindex]);
        }
        yield return new WaitForEndOfFrame();
        while (CustomersQuantity > 0)
        {

            //GameObject newCustomer =  Instantiate(Customers.Dequeue(), this.transform.position, Quaternion.identity);
            //--------
            int s = Random.Range(0, skinTones.Length);
            int f = Random.Range(0, faces.Length);
            int b = Random.Range(0, bodies.Length);
            int h = Random.Range(0, hairPrefabs.Length);
            int t = Random.Range(0, hairTones.Length);
            int x = Random.Range(0, shoeColors.Length);
            int y = Random.Range(0, shoes.Length);
            int g = Random.Range(0, glassesPrefabs.Length);
            int l = Random.Range(0, glassesColors.Length);
            int d = Random.Range(0, drinks.Length);

            Vector3 c = new Vector3(Random.Range(0.25f, 1f), Random.Range(0.25f, 1f), Random.Range(0.25f, 1f));
            Color clothes = new Color(c.x, c.y, c.z, 1f);

            GameObject newCustomer = Instantiate(characterPrefab, this.transform.position, Quaternion.identity);
            Character ch = newCustomer.GetComponent<Character>();

            ch.skinColor = skinTones[s];
            ch.hairColor = hairTones[t];
            ch.clothingColor = clothes;
            ch.clothing = bodies[b];
            ch.faceFeature = faces[f];
            ch.hairStyle = hairPrefabs[h];
            ch.shoeStyle = shoes[y];
            ch.shoeColor = shoeColors[x];
            ch.glasses = glassesPrefabs[g];
            ch.glassesColor = glassesColors[l];
            ch.preferedDrink = drinks[d];
            //---------
            Transform stoolPosition = stools.Dequeue().transform;
            newCustomer.GetComponent<CustomerController>().GoTo(stoolPosition);
            //CharacterTemplate template = LevelManager.instance.characterTemplates[Random.Range(0, maxNumberOfTemplates)];
            //newCustomer.GetComponent<ConvoSystem>().chara = new Character(template);
            newCustomer.name = CustomersQuantity.ToString();
            CustomersQuantity--;
            yield return new WaitForSeconds(Random.Range(1, maxTimeToSpawn));
        }

        StartCoroutine(ReadyToStart());

    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(LevelManager.instance.timeToStartWhenReady);
        LevelManager.instance.StartTheGame();
    }
}
