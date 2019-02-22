using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Writer : MonoBehaviour {

    Text txt;
    string WriteText;
    public float timeBetweenLetter = 0.125f;
    void Awake()
    {
        txt = GetComponent<Text>();
        WriteText = txt.text;
        

        // TODO: add optional delay when to start
        
    }

    private void OnEnable()
    {
        txt.text = "";
        StartCoroutine("PlayText");
    }
    IEnumerator PlayText()
    {
        foreach (char c in WriteText)
        {
            txt.text += c;
            yield return new WaitForSeconds(timeBetweenLetter);
        }
    }
}
