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
	public int selfMovementModifier;
	public int enemyMovementModifier;

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
}
