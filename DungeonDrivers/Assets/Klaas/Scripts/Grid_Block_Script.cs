using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid_Block_Script : MonoBehaviour
{
	public GameObject CurrentPlayer;
	//public List<bool> allPlayersInside = new List<bool>();
	public bool[] playersInSide = new bool[4];
	public SpriteRenderer SpriteRenderer;
	public bool isActive, isSelected, isMovePos, isCardPos;
	//public GameObject Top, Right, Bot, Left;
	//public GameObject TopLeft, TopRight, BotLeft, BotRight;
	//public List<GameObject> surroundingGridBlocks = new List<GameObject>();

	public int xLane, yLane;

	public Sprite active, unActive, ActiveSelected, UnactiveSelected, MovePos, CardPos;

	void FixedUpdate()
	{
		if(isCardPos)
		{
			SpriteRenderer.sprite = CardPos;
		}
		else if(isMovePos)
		{
			SpriteRenderer.sprite = MovePos;
		}
		else if(isActive)
		{
			if(isSelected)
			{
				SpriteRenderer.sprite = ActiveSelected;
			}
			else
			{
				SpriteRenderer.sprite = active;
			}
		}
		else
		{
			if(isSelected)
			{
				SpriteRenderer.sprite = UnactiveSelected;
			}
			else
			{
				SpriteRenderer.sprite = unActive;
			}
		}
	}

}
