using UnityEngine;
using System.Collections;

public class Display_Done_Button : MonoBehaviour 
{

	PhaseWalker PhaseWalker;
	public GameObject DoneButton, ReadyButton, UndoButton;

	void Start () 
	{
		PhaseWalker = GetComponentInParent<PhaseWalker>();
		StartCoroutine("SetButtonsEnum");
	}

	IEnumerator SetButtonsEnum()
	{
		while(true)
		{
			SetButtons();
			yield return new WaitForSeconds(0.1f);
		}
	}

	void SetButtons()
	{
		if(PhaseWalker.Player_Sync_Variables.phase == Game_Manager_Script.Phase.PlayerInSetup)
		{
			ReadyButton.SetActive(true);
		}
		else
		{
			ReadyButton.SetActive(false);
		}
		
		if(PhaseWalker.currentPhase.phaseType == Phase.PhaseType.ManualDone)
		{
			DoneButton.SetActive(true);
			UndoButton.SetActive(true);
		}
		else
		{
			DoneButton.SetActive(false);
			UndoButton.SetActive(false);
		}
		/*
		if(Player.Player_Sync_Variables.phase == Game_Manager_Script.Phase.PlayerInSetup)
		{
			ReadyButton.SetActive(true);
		}
		else
		{
			ReadyButton.SetActive(false);
		}
		
		if(Player.Player_Sync_Variables.phase == Game_Manager_Script.Phase.ChooseMovePosition || Player.Player_Sync_Variables.phase == Game_Manager_Script.Phase.PlayCards)
		{
			DoneButton.SetActive(true);
			UndoButton.SetActive(true);
		}
		else
		{
			DoneButton.SetActive(false);
			UndoButton.SetActive(false);
		}
		*/
	}
}
