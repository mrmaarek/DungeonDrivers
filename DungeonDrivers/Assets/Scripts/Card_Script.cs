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

	public List<Vector3> affectedGridBlocks = new List<Vector3>();


	public enum Targeting
	{
		SelfCast,
		FreeSelect,
		Locked
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
}
