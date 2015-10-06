using UnityEngine;
using System.Collections;

public class CardLoader : MonoBehaviour {

	public const string path = "DungeonDrivers/Assets/XML/cards"; 

	void Start()
	{
		CardContainer cc = CardContainer.Load(path);

		foreach(Card card in cc.cards)
		{
			print(card.cardName);
		}
	}
}
