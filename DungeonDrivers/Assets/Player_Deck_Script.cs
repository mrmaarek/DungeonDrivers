using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Deck_Script : MonoBehaviour 
{
	public PhaseWalker Player;

	public GameObject[] allCards2;
	public List<GameObject> allCards = new List<GameObject>();

	public List<GameObject> deck, hand, discardPile = new List<GameObject>();
	public GameObject[] tempDeck;
	public GameObject DeckObject, HandObject, DiscardPile, CardToPlay, CardCurrentlyPlayed;

	public LayerMask cardSelectLayerMask;

	void Start () 
	{
		//LoadDeck();
		//FillHand();
		LoadAllCards();
	}

	void Update () 
	{
		//SelectCard();
	}

	void LoadAllCards()
	{
		foreach(GameObject classId in Resources.LoadAll<GameObject>("Classes/"))
		{
			allCards2 = Resources.LoadAll<GameObject>("Cards/" + classId.name);

			for(int i = 0; i < allCards2.Length; i++)
			{
				allCards.Add(allCards2[i]);
			}
		}
	}

	public void LoadDeck()
	{
		if(Player != null)
		{
			tempDeck = Resources.LoadAll<GameObject>("Cards/" + Player.Player_Sync_Variables.playerClass);
		}
		else 
		{
			Debug.Log("No Player script (PhaseWalker) attached in the player slot.");
		}
		for(int i = 0; i < tempDeck.Length; i++)
		{
			deck.Add(tempDeck[i]);
		}
		for(int i = 0; i < tempDeck.Length; i++)
		{
			deck.Add(tempDeck[i]);
		}

		ShuffleDeck();
	}

	void SpawnDeck()
	{

	}

	public void ShuffleDeck()
	{
		for (int i = 0; i < deck.Count; i++)
		{
			// For each card make a temporary copy.
			GameObject tempCard = deck[i];
			
			// Grab a random number, where to place it.
			int randomIndex = UnityEngine.Random.Range(0, deck.Count);

			deck[i] = deck[randomIndex];
			deck[randomIndex] = tempCard;
		}
	}

	public void FillHand()
	{

		for (int i = 0; i < 3; i++)
		{
			DrawCard();

		}

	}

	public void DrawCard()
	{
		if(hand.Count < 4)
		{
			GameObject drawedCard = Instantiate(deck[0]) as GameObject;
			hand.Add(drawedCard);
			drawedCard.transform.SetParent(HandObject.transform);

			OrderCards();

			drawedCard.GetComponent<BoxCollider>().enabled = true;
			
			deck.Remove(deck[0]);
		}
	}

	public void SelectCard()
	{
		GameObject CardHoveredOver;

		RaycastHit vHit = new RaycastHit();
		
		Ray vRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(vRay, out vHit, 1000, cardSelectLayerMask)) 
		{
			CardHoveredOver = vHit.collider.gameObject;
		}
		else
		{
			CardHoveredOver = null;
		}

		foreach(GameObject cardHit in hand)
		{
			if(CardHoveredOver == cardHit)
			{
				cardHit.transform.localPosition = new Vector3(cardHit.transform.localPosition.x, 110, cardHit.transform.localPosition.z);
				if(Input.GetMouseButtonDown(0))
				{
					if(CardCurrentlyPlayed == cardHit)
					{
						PutCardBack(cardHit);
					}
					else if(CardCurrentlyPlayed == null)
					{
						CardCurrentlyPlayed = cardHit;
					}
				}
			}
			else
			{
				cardHit.transform.localPosition = new Vector3(cardHit.transform.localPosition.x, -80, cardHit.transform.localPosition.z);
			}

			if(CardCurrentlyPlayed == cardHit)
			{
				cardHit.transform.localPosition = new Vector3(cardHit.transform.localPosition.x, 130, cardHit.transform.localPosition.z);
				cardHit.transform.localScale = new Vector3(500, 500, 500);
			}
		}

	}

	void PutCardBack(GameObject CardHit)
	{
		CardHit.transform.localPosition = new Vector3(CardHit.transform.localPosition.x, -80, CardHit.transform.localPosition.z);
		CardHit.transform.localScale = new Vector3(400, 400, 400);
		CardCurrentlyPlayed = null;
	}

	void OrderCards()
	{
		for(int i = 0; i < hand.Count; i++)
		{
			hand[i].transform.localPosition = new Vector3(125 + i * 250, -80, 0);
			hand[i].transform.localScale = new Vector3(400, 400, 400);
			hand[i].transform.localRotation = Quaternion.Euler(new Vector3(270, 0));
			hand[i].GetComponent<BoxCollider>().enabled = true;
		}
	}

	public void RefilDeck()
	{
		for(int i = 0; i < discardPile.Count; i++)
		{
			deck.Add(discardPile[i]);
		}
		ShuffleDeck();
	}
}
