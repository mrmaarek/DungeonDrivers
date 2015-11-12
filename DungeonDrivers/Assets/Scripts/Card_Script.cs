using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Card_Script : MonoBehaviour 
{

	public int id;
	public string cardName;
	public int initiative;
	public int damage;
	public string description;
	public Sprite cardImage;
	public Targeting targeting;
	public CardType cardType;
	public string cardClass;

	public List<Vector3> affectedGridBlocks = new List<Vector3>();

	public Image cardImageObject;

    public bool cardIsInPlay = false;

	public PhaseWalker PhaseWalker;

	public Vector3 cardPos;

	public enum Targeting
	{
		SelfCast,
		FreeSelect,
		Locked,
		Directional
	}

	public enum CardType
	{
		Utility,
		CC,
		Attack
	}


	public GameObject Prefab;


	public Text NameObject, InitiativeObject, DescriptionObject;

	public void SetTexts()
	{
		NameObject.text = cardName;
		InitiativeObject.text = "Initiative: " + initiative;
		DescriptionObject.text = description;
	}
	/*
	void OnMouseEnter()
	{
        // Need to be added:
        // IF THE CARD IS IN THE HAND, THEN YOU CAN TRANSFORM ITS Y++
        if (!cardIsInPlay)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 110, transform.localPosition.z);
        }
	}

	void OnMouseExit()
	{
        if (!cardIsInPlay)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -80, transform.localPosition.z);
        }
	}
*/
    void OnMouseDown()
    {
        
    }

    void OnMouseUp()
    {
		/*
        // The card is NOT in play, and will be transformed to 'playable'
        if (!cardIsInPlay)
        {
            cardIsInPlay = true;
            transform.localScale = new Vector3(450, 450, 450);
			PhaseWalker.CardToPlay = this.gameObject;
			transform.position = PhaseWalker.CardPos.transform.position;
            Debug.Log("You made the card to IN-Play");
        }
        // The card is already in play, set it back.
        else
        {
            cardIsInPlay = false;
			transform.localScale = new Vector3(400, 400, 400);
			transform.localPosition = cardPos;
			
			Debug.Log("I Want the card back to normal.");
        }
        */
    }
    // ONMOUSECLICK()
    // 

}
