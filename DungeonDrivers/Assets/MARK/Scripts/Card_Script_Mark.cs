using UnityEngine;

using System.Collections;


public class Card_Script_Mark : MonoBehaviour
{
    float distance = 1f;

    [SerializeField]
    private GameObject myPlayerHand;


    [SerializeField]
    GameObject cardToPlay;

    [SerializeField]
    private Camera testCam;

    [SerializeField]
    //private Animator Card_Animator_Mark;

    public void Start()
    {
        myPlayerHand = GameObject.Find("MyHand");
    }

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


        

        // The card which will be played, will transform into a 'X' Mark
        // So you can play your card easily on the grid.
        if (Input.mousePosition.y > 200)
        {
            this.transform.SetParent(cardToPlay.transform);
            myPlayerHand.GetComponent<Player_Hand>().myTempHand.Remove(this.gameObject);

            // RE ORGANIZE THE PLAYER HAND WITHOUT THE PLAYED CARD.
        }

        //
    }

    private void ReOrganizeHand()
    {
        // 
    }
}
