using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Color skinColor;
    public Color hairColor;
    public Color clothingColor;
    public Color shoeColor;
    public Color glassesColor;
    public GameObject hairStyle;
    public GameObject glasses;
    public Texture clothing;
    public Texture faceFeature;
    public Texture shoeStyle;

    public Transform head;
    public MeshRenderer body;
    public MeshRenderer face;
    public MeshRenderer r_hand;
    public MeshRenderer l_hand;
    public MeshRenderer r_foot;
    public MeshRenderer l_foot;
    public MeshRenderer drink;

    public GameObject opinionPrefab;
    public GameObject currentDrink;
    public Texture preferedDrink;
    public Animator anim;

    public SpriteRenderer opinion;
    public Sprite[] positiveReact;
    public Sprite[] negativeReact;

    public bool isAvailable = false;
    public bool isSitting = false;
    public bool isWalking = false;

    public float t;
    public float i;
    public int drinkTimer;
    public float chatTimer;
    public float chatCycleOffset;
    public int opinionLifetime;
    public Vector2 opinionRate;

    private FMODUnity.StudioEventEmitter eventEmitterRef;

    public int Goods;
    public int Bads;
    [HideInInspector]
    public bool canTalk = true;
    // Start is called before the first frame update

    private void Awake()
    {
        eventEmitterRef = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    void Start()
    {
        if (hairStyle != null)
        {
           GameObject hair = Instantiate(hairStyle, head, false);
           hair.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", hairColor);
        }

        if (glasses != null)
        {
            GameObject glss = Instantiate(glasses, head, false);
            glasses.transform.position = new Vector3(-0.1f, 0, 0.6f);
            glss.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", glassesColor);
        }

        body.material.SetColor("_Color", clothingColor);
        face.material.SetColor("_Color", skinColor);

        r_hand.material.SetColor("_Color", skinColor);
        l_hand.material.SetColor("_Color", skinColor);

        r_foot.material.SetColor("_Color", shoeColor);
        l_foot.material.SetColor("_Color", shoeColor);

        face.material.mainTexture = faceFeature;
        body.material.mainTexture = clothing;
        r_foot.material.mainTexture = shoeStyle;
        l_foot.material.mainTexture = shoeStyle;

        drink.material.mainTexture = preferedDrink;
        drinkTimer = Random.Range(1, 3);
        chatTimer = Random.Range(opinionRate.x, opinionRate.y); //SOMETHING HERE THAT MAY CHANGE

        chatCycleOffset = Random.Range(0f, 1f);
        anim.speed = Random.Range(0.5f, 2f);
        anim.SetFloat("chatCycleOffset", chatCycleOffset);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isAvailable", isAvailable);
        anim.SetBool("isSitting", isSitting);
        anim.SetBool("isWalking", isWalking);
        
        if (isSitting)
        {
            if (isAvailable)
            { 
                t += Time.deltaTime;

                currentDrink.SetActive(true);

                if (t >= drinkTimer)
                {
                    anim.SetTrigger("Drink");
                    drinkTimer = Random.Range(8, 16);
                    t = 0;
                }

            }
            else
            { 
                i += Time.deltaTime;

                if(i >= chatTimer)
                {
                    GiveOpinion();
                    i = 0;
                }
            }
            
        }
        else
        {
            currentDrink.SetActive(false);
        }
    }

    void GiveOpinion()
    {
        //if (!canTalk) { return; }
        GameObject op = Instantiate(opinionPrefab, head.transform.position, Quaternion.identity);
        opinion = op.GetComponentsInChildren<SpriteRenderer>()[1];

        int r = Random.Range(0, 2);

        if(r == 0)
        {
            int p = Random.Range(0, positiveReact.Length);
            opinion.sprite = positiveReact[p];
            Goods++;
        }
        else
        {
            int n = Random.Range(0, positiveReact.Length);
            opinion.sprite = negativeReact[n];
            Bads++;
        }
        eventEmitterRef.Play();
        Destroy(op, opinionLifetime);
    }

}
