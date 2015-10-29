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
	public List<GameObject> classes = new List<GameObject>();
	public List<Sprite> images = new List<Sprite>();

	public GameObject[] classes2;

	public GameObject[] cards2;

	public Sprite[] images2;
	
	public bool isNewCard;

	public GameObject CurrentCard;

	public GameObject NewCard;

	public GameObject EmptyCard;

	public GameObject NewPrefab;

	public GameObject NewClass;

	public Card_Script Card_Script;

	public GameObject ChooseStats, TargetSelect;
	private bool chooseStats = true;
	private bool targetSelect = false;

	public List<GameObject> selecteGridBlocks = new List<GameObject>();

	public int currentClass;

	public int currentImage;

	public Text ClassName;

	public Text cardImage;

	void Start () 
	{
		Grid_Spawner_Script = GetComponent<Grid_Spawner_Script>();
		LoadCardList();
		LoadClassList();
		LoadImageList();
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

	public void SwitchImage(bool nextImage)
	{
		if(nextImage)
		{
			if(currentImage < images.Count - 1)
			{
				currentImage++;
			}
			else
			{
				currentImage = 0;
			}
		}
		else
		{
			if(currentImage > 0)
			{
				currentImage--;
			}
			else
			{
				currentImage = images.Count - 1;
			}
		}
		Card_Script.cardImage = images[currentImage];
		Card_Script.cardImageObject.sprite = images[currentImage];
		cardImage.text = images[currentImage].name;
	}

	public void LoadImageList()
	{
		images2 = Resources.LoadAll<Sprite>("Card Images/");
		images.Clear();
		
		for(int i = 0; i < images2.Length; i++)
		{
			images.Add(images2[i]);
		}
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

	public void LoadClassList()
	{
		classes2 = Resources.LoadAll<GameObject>("Classes/");
		classes.Clear();

		for(int i = 0; i < classes2.Length; i++)
		{
			classes.Add(classes2[i]);
		}
	}

	public void SwitchClass(bool nextClass)
	{
		if(nextClass)
		{
			if(currentClass < classes.Count - 1)
			{
				currentClass++;
			}
			else
			{
				currentClass = 0;
			}
		}
		else
		{
			if(currentClass > 0)
			{
				currentClass--;
			}
			else
			{
				currentClass = classes.Count - 1;
			}
		}
		Card_Script.cardClass = classes[currentClass].name;
		ClassName.text = classes[currentClass].name;
	}

	public void CreateNewClass(InputField InputField)
	{
		AssetDatabase.CreateFolder("Assets/Resources/Cards", InputField.text);
		NewClass = PrefabUtility.CreatePrefab("Assets/Resources/Classes/" + InputField.text + ".prefab", NewClass);
		AssetDatabase.Refresh();
		LoadCardList();
	}

	public void SetTargeting(int i)
	{
		Card_Script.targeting = (Card_Script.Targeting)i;
	}
	/*
	switch(i)
	{
	case 0:
		Card_Script.targeting = Card_Script.Targeting.SelfCast;
		break;
	case 1:
		Card_Script.targeting = Card_Script.Targeting.FreeSelect;
		break;
	case 2:
		Card_Script.targeting = Card_Script.Targeting.Locked;
		break;
	case 3:
		Card_Script.targeting = Card_Script.Targeting.Directional;
		break;
	}
	*/
	/*
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

	public void SetTargetingDirectional()
	{
		Card_Script.targeting = Card_Script.Targeting.Locked;
	}
*/
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
		foreach(GameObject gB in Grid_Spawner_Script.selectedGridBlocks)
		{
			Card_Script.affectedGridBlocks.Add(gB.transform.localPosition);
		}
	}

	public void SaveCard()
	{
		if(isNewCard)
		{

			NewPrefab = PrefabUtility.CreatePrefab("Assets/Resources/Cards/"+ Card_Script.cardClass + "/" + Card_Script.cardName + ".prefab", NewCard);
			AssetDatabase.Refresh();
			LoadCardList();
		}
		else
		{

			AssetDatabase.Refresh();
		}
	}


}
