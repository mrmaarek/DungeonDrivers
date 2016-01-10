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

	public int currentLoadClass, currentLoadCard;
	public GameObject[] currentLoadCardArray;
	public List<GameObject> currentLoadCardList = new List<GameObject>();

	public Text LoadClassName;

	public GameObject LoadCardInterface;

	public Card_Script LoadCardScript;




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
		Card_Script.description = InputField.text;
		Card_Script.SetTexts();
	}

	public void SetSelfMovementModifier(InputField InputField)
	{
		Card_Script.selfMovementModifier = int.Parse(InputField.text);
	}

	public void SetEnemyMovementModifier(InputField InputField)
	{
			Card_Script.enemyMovementModifier = int.Parse(InputField.text);
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
		isNewCard = true;
		ClearCardInformation();
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
			GameObject prefabToReplace = Resources.Load<GameObject>("Cards/" + Card_Script.cardClass + "/" + Card_Script.cardName);
			Debug.Log(prefabToReplace.name);
			PrefabUtility.ReplacePrefab(NewCard, prefabToReplace);
			AssetDatabase.Refresh();
			currentLoadCardArray = Resources.LoadAll<GameObject>("Cards/" + classes[currentLoadClass].name + "/");
			currentLoadCardList.Clear();
			for(int i = 0; i < currentLoadCardArray.Length; i++)
			{
				currentLoadCardList.Add(currentLoadCardArray[i]);
			}

		}
	}

	public void OpenLoadInterface(bool open)
	{
		LoadCardInterface.SetActive(open);
	}

	public void SwitchLoadClass(bool next)
	{
		if(next)
		{
			if(currentLoadClass < classes.Count - 1)
			{
				currentLoadClass++;
			}
			else
			{
				currentLoadClass = 0;
			}
		}
		else
		{
			if(currentLoadClass > 0)
			{
				currentLoadClass--;
			}
			else
			{
				currentLoadClass = classes.Count - 1;
			}
		}

		currentLoadCardArray = Resources.LoadAll<GameObject>("Cards/" + classes[currentLoadClass].name + "/");
		currentLoadCardList.Clear();
		for(int i = 0; i < currentLoadCardArray.Length; i++)
		{
			currentLoadCardList.Add(currentLoadCardArray[i]);
		}

		LoadClassName.text = classes[currentLoadClass].name;
	}

	public void SwitchLoadCard(bool next)
	{
		if(next)
		{
			if(currentLoadCard < currentLoadCardList.Count - 1)
			{
				currentLoadCard++;
			}
			else
			{
				currentLoadCard = 0;
			}
		}
		else
		{
			if(currentLoadCard > 0)
			{
				currentLoadCard--;
			}
			else
			{
				currentLoadCard = currentLoadCardList.Count - 1;
			}
		}

		SetLoadInformation();
	}

	public InputField NameText, DamageText, DescriptionText, SelfMovementModifierText, EnemyMovementModifier;
	public Image ImageImage;
	public Text ClassText;

	public void SetCardInformation()
	{
		NameText.text = Card_Script.cardName;
		DamageText.text = Card_Script.damage + "";
		DescriptionText.text = Card_Script.description;
		ImageImage.sprite = Card_Script.cardImage;
		ClassText.text = Card_Script.cardClass;
		SelfMovementModifierText.text = Card_Script.selfMovementModifier + "";
		EnemyMovementModifier.text = Card_Script.enemyMovementModifier + "";
	}

	public void ClearCardInformation()
	{
		NameText.text = "";
		DamageText.text = "";
		DescriptionText.text = "";
		ImageImage.sprite = null;
		ClassText.text = "";
		SelfMovementModifierText.text = "";
		EnemyMovementModifier.text = "";

	}


	public Text LoadNameText, LoadTargetingText, LoadDamageText, LoadInitiativeText, LoadDescriptionText, LoadSelfMovementModifier, LoadEnemyMovementModifier;
	public Image LoadImage;

	public void SetLoadInformation()
	{
		LoadCardScript = currentLoadCardList[currentLoadCard].GetComponent<Card_Script>();

		LoadNameText.text = LoadCardScript.cardName;
		LoadTargetingText.text = LoadCardScript.targeting.ToString();
		LoadDamageText.text = LoadCardScript.damage + "";
		LoadInitiativeText.text = LoadCardScript.initiative + "";
		LoadDescriptionText.text = LoadCardScript.description;
		LoadImage.sprite = LoadCardScript.cardImage;
		LoadSelfMovementModifier.text = LoadCardScript.selfMovementModifier + "";
		LoadEnemyMovementModifier.text = LoadCardScript.enemyMovementModifier + "";
	}

	public void LoadCardFunction()
	{
		if(NewCard != null)
		{
			Destroy(NewCard);
		}
		
		NewCard = Instantiate(currentLoadCardList[currentLoadCard]);
		Card_Script = NewCard.GetComponent<Card_Script>();
		isNewCard = false;
		SetCardInformation();
	}
}
