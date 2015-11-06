using UnityEngine;

using System.Collections;


public class Card_Script_Mark : MonoBehaviour
{
    float distance = 1f;

    [SerializeField]
    private Camera testCam;

    [SerializeField]
    //private Animator Card_Animator_Mark;

    public void flipCard()
    {
        // Call the animator and set HeadsUp to true to play the animation of the card.
       // Card_Animator_Mark.SetBool("Headsup", true);
    }

    public void OnMouseDrag()
    {

        Debug.Log(this);
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = testCam.ScreenToWorldPoint(mousePosition);

        this.transform.position = objPosition;
    }
}
