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

	void OnMouseEnter()
	{
        // Need to be added:
        // IF THE CARD IS IN THE HAND, THEN YOU CAN TRANSFORM ITS Y++
        
        transform.localPosition = new Vector3(transform.localPosition.x, 110, transform.localPosition.z);
	}

	void OnMouseExit()
	{
		transform.localPosition = new Vector3(transform.localPosition.x, -120, transform.localPosition.z);
	}

}
