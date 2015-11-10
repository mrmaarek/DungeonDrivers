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
    private GameObject myDiscardPile;

    [SerializeField]
    private Camera testCam;

    [SerializeField]
    //private Animator Card_Animator_Mark;

    public void Start()
    {
        myDiscardPile = GameObject.Find("MyDiscardPile");
        myPlayerHand = GameObject.Find("MyHand");
        cardToPlay = GameObject.Find("MyCardToPlay");
        testCam = GameObject.Find("InWorldUICamera (1)").GetComponent<Camera>();
    }

    public void flipCard()
    {
        // Call the animator and set HeadsUp to true to play the animation of the card.
       // Card_Animator_Mark.SetBool("Headsup", true);
    }

    public void OnMouseDrag()
    {

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
            ReOrganizeHand();

            /*
            // WANNEER EEN KAART GESPEELD IS, ZET DE ANDERE KAARTEN UIT ZODAT ER NIET MEER GESPEELD KUNNEN WORDEN.
            for (int i = 0; i < myPlayerHand.GetComponent<Player_Hand>().myTempHand.Count; i++)
            {
                myPlayerHand.GetComponent<Player_Hand>().myTempHand[i].GetComponent<Card_Script_Mark>().enabled = false;
                myPlayerHand.GetComponent<Player_Hand>().myTempHand[i].GetComponent<Card_Script>().enabled = false; 
            }
            */
            
            // Card to discard pile if he really is played.
        }

    }
    //When the mouse is released? 
    // So this will be the end of the drag?
    public void OnMouseUp()
    {
        // Add the card to the list of the discard pile.
        myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards.Add(this.gameObject);
        this.transform.SetParent(myDiscardPile.transform);
    }

    private void ReOrganizeHand()
    {
        // 
        for (int i = 0; i < myPlayerHand.GetComponent<Player_Hand>().myTempHand.Count; i++)
        {
            // Voor elk object in de array.
            myPlayerHand.GetComponent<Player_Hand>().myTempHand[i].transform.localPosition = new Vector3(125 + i * 250, -80, 0);
        }
    }
}
