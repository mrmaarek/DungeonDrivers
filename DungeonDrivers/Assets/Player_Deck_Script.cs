using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Deck_Script : MonoBehaviour 
{

	public List<GameObject> deck, hand, discardPile = new List<GameObject>();
	public GameObject[] tempDeck;
	public GameObject DeckObject, HandObject, DiscardPile, CardToPlay, CardCurrentlyPlayed;

	public LayerMask cardSelectLayerMask;

	void Start () 
	{
		LoadDeck();
		FillHand();
	}

	void Update () 
	{
		SelectCard();
	}

	void LoadDeck()
	{
		tempDeck = Resources.LoadAll<GameObject>("Cards/Warrior");
		for(int i = 0; i < tempDeck.Length; i++)
		{
			deck.Add(tempDeck[i]);
		}
	}

	void SpawnDeck()
	{

	}

	void ShuffleDeck()
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

	void FillHand()
	{

		for (int i = 0; i < 3; i++)
		{
			DrawCard();

			deck.Remove(deck[i]);
		}

	}

	void DrawCard()
	{
		GameObject drawedCard = Instantiate(deck[0]) as GameObject;
		hand.Add(drawedCard);
		drawedCard.transform.SetParent(HandObject.transform);

		OrderCards();

		drawedCard.GetComponent<BoxCollider>().enabled = true;
		
		deck.Remove(deck[0]);
	}

	void SelectCard()
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
					if(CardCurrentlyPlayed == null)
					{
						CardCurrentlyPlayed = cardHit;

					}
				}
			}
			else
			{
				cardHit.transform.localPosition = new Vector3(cardHit.transform.localPosition.x, -80, cardHit.transform.localPosition.z);
			}
		}

	}

	void PutCardBack()
	{

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
}
