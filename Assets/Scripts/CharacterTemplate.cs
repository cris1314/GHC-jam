/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Character Template", menuName = "GHC/Character Template")]
public class CharacterTemplate : MonoBehaviour {
    // actual list of topics to add to the character
    public enum Topics
    {
        Movies, Games, Sports, Work, Study,
        Travel, Romance, Books, Science, Foods, Fashion
    };
    //a list with likes of the character based on true or false
    public List<Likes> likes = new List<Likes>(new Likes[] {
    //declaring topics for an empty template
    new Likes(Topics.Movies),
    new Likes(Topics.Games),
    new Likes(Topics.Sports),
    new Likes(Topics.Work),
    new Likes(Topics.Study),
    new Likes(Topics.Travel),
    new Likes(Topics.Romance),
    new Likes(Topics.Books),
    new Likes(Topics.Science),
    new Likes(Topics.Foods),
    new Likes(Topics.Fashion),
    });
    //is it available to talk?
    public bool isAvailable = true;

    //#region[Getters and Setters]
    //public bool IsAvailable {get { return IsAvailable; } set { IsAvailable = value; }}
    //#endregion



}




/*[System.Serializable]
public class Likes
{
    
    public CharacterTemplate.Topics topic; // variable to store the topics chossen for this like
    public bool like = false; // does like it or not?

    public Likes(CharacterTemplate.Topics t) {
        topic = t;
    }


}*/
