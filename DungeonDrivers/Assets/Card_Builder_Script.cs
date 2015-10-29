using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class Card_Builder_Script : MonoBehaviour 
{

	Grid_Spawner_Script Grid_Spawner_Script;

	string cardLocation = "Cards/";

	public List<GameObject> cards = new List<GameObject>();

	public GameObject[] cards2;
	
	public bool isNewCard;

	public GameObject CurrentCard;

	public GameObject NewCard;

	public GameObject EmptyCard;

	public GameObject NewPrefab;

	public Card_Script Card_Script;

	public GameObject ChooseStats, TargetSelect;
	private bool chooseStats = true;
	private bool targetSelect = false;

	public List<GameObject> selecteGridBlocks = new List<GameObject>();

	void Start () 
	{
		Grid_Spawner_Script = GetComponent<Grid_Spawner_Script>();
		LoadCardList();
	}

	void Update () 
	{
		if(targetSelect)
		{
			Grid_Spawner_Script.SelectWithMouse();
		}
	}

	public void SwitchInterface()
	{
		chooseStats = !chooseStats;
		targetSelect = !targetSelect;

		TargetSelect.SetActive(targetSelect);

		NewCard.SetActive(chooseStats);
	}

	public void SetName(InputField InputField)
	{
		Card_Script.cardName = InputField.text;
		Card_Script.SetTexts();
	}

	public void SetInitiative(int initiative)
	{
		Card_Script.initiative = initiative;
		Card_Script.SetTexts();
	}

	public void SetDamage(InputField InputField)
	{
		Card_Script.damage = int.Parse(InputField.text);
		Card_Script.SetTexts();
	}

	public void SetDescription(InputField InputField)
	{
		Card_Script.description = InputField.text + " This attack deals " + Card_Script.damage + " damage.";
		Card_Script.SetTexts();
	}

	void LoadCardList()
	{
		cards2 = Resources.LoadAll<GameObject>(cardLocation);
		cards.Clear();
		for(int i = 0; i < cards2.Length; i++)
		{
			cards.Add(cards2[i]);
		}
	}


	public void SetTargetingSelf()
	{
		Card_Script.targeting = Card_Script.Targeting.SelfCast;
	}

	public void SetTargetingFree()
	{
		Card_Script.targeting = Card_Script.Targeting.FreeSelect;
	}

	public void SetTargetingLocked()
	{
		Card_Script.targeting = Card_Script.Targeting.Locked;
	}

	public void CreateNewCard()
	{
		if(NewCard != null)
		{
			Destroy(NewCard);
		}

		NewCard = Instantiate(EmptyCard);
		Card_Script = NewCard.GetComponent<Card_Script>();
	}

	public void SafeGridSelection()
	{

	}

	public void SaveCard()
	{
		if(isNewCard)
		{
			NewPrefab = PrefabUtility.CreatePrefab("Assets/Resources/Cards/" + Card_Script.cardName + ".prefab", NewCard);
			AssetDatabase.Refresh();
			LoadCardList();
		}
		else
		{

			AssetDatabase.Refresh();
		}
	}


}
