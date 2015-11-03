using UnityEngine;
using System.Collections;


public class Card_Script_Mark : MonoBehaviour
{

    [SerializeField]
    private Animator Card_Animator_Mark;

    public void flipCard()
    {
        // Call the animator and set HeadsUp to true to play the animation of the card.
        Card_Animator_Mark.SetBool("Headsup", true);
    }
}
