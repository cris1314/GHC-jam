using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationBar : MonoBehaviour {

    public ConvoSystem CharacterA;
    public ConvoSystem CharacterB;

    int turn;

    public ConversationBar(ConvoSystem a, ConvoSystem b) {
        CharacterA = a;
        CharacterB = b;
        turn = Random.Range(0,1);
        StartCoroutine(Conversation());
    }

    IEnumerator Conversation() {

        yield return null;

    }

}
