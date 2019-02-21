using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
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

            GameObject character = Instantiate(characterPrefab, transform, false);
            Character ch = character.GetComponent<Character>();

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
        }
    }
}
