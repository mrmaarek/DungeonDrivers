using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player_UI_Phases : MonoBehaviour 
{
	
	private PhaseWalker Player;

	public List<Image> phaseBlocks = new List<Image>();

	public Color active, nonActive;

	public int currentPhase;

	public Text MovesLeftText;

	void Start()
	{
		Player = GetComponentInParent<PhaseWalker>();
		StartCoroutine("UpdateUIEnum");
	}

	IEnumerator UpdateUIEnum()
	{
		while(true)
		{
			UpdateUI();
			yield return new WaitForSeconds(0.25f);
		}
	}

	void UpdateUI()
	{
		currentPhase = (int)Player.Player_Sync_Variables.phase;

		int i = 0;
		foreach(Image PhaseBlock in phaseBlocks)
		{
			if(i == currentPhase)
			{
				PhaseBlock.color = active;
			}
			else
			{
				PhaseBlock.color = nonActive;
			}
			i++;
		}

		//MovesLeftText.text = "" + Player.movesLeft;
	}

}
