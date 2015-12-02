using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Game_Manager_Script :  NetworkBehaviour
{



    public List<Player_Sync_Variables> players = new List<Player_Sync_Variables>();
	 
	void Start()
	{
		StartCoroutine("GameFlowActivateEnum");

       
    }



	IEnumerator GameFlowActivateEnum()
	{
		while(true)
			{
			GameFlow();
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	//------------------------------------------------------------------------------------------------------
	// Game Flow
	//------------------------------------------------------------------------------------------------------
	public MapSpawnerScript MapSpawnerScript;
	public bool allPlayersReady;
	public bool allPlayersDone;
	public Phase phase;
	public enum Phase
	{
		PlayerInSetup = 0,
		SpawnPlayers = 1,
		ChooseMovePosition = 2,
		ChooseMovementPositionResolve = 3,
		PlayCards = 4,
		PlayCardsResolve = 5,
		IsMoving = 6,
		IsMovingResovle = 7,
		IsCard1 = 8,
		IsCard1Resolve = 9,
		IsCard2 = 10,
		IsCard2Resolve = 11,
		IsCard3 = 12,
		IsCard3Resolve = 13,
		IsCard4 = 14,
		IsCard4Resolve = 15,
		EndTurn = 16,
		EndTurnResolve = 17
	}


	// isMoving direction and choose movement resolve direction are changed to increase testing speed
	void GameFlow()
	{
		switch(phase)
		{
		case Phase.PlayerInSetup:
			BeginPhases();
			break;
		case Phase.SpawnPlayers:
			NextPhase(Phase.ChooseMovePosition);
			break;
		case Phase.ChooseMovePosition:
			NextPhase(Phase.ChooseMovementPositionResolve);
			break;
		case Phase.ChooseMovementPositionResolve:
			NextPhase(Phase.PlayCards);
			//NextPhase(Phase.IsMoving);
			break;
		case Phase.PlayCards:
			NextPhase(Phase.PlayCardsResolve);
			break;
		case Phase.PlayCardsResolve:
			NextPhase(Phase.IsMoving);
			break;
		case Phase.IsMoving:
			NextPhase(Phase.IsMovingResovle);
			break;
		case Phase.IsMovingResovle:
			//NextPhase(Phase.IsUtility);
			NextPhase(Phase.IsCard1);
			break;
		case Phase.IsCard1:
			NextPhase(Phase.IsCard1Resolve);
			break;
		case Phase.IsCard1Resolve:
			NextPhase(Phase.IsCard2);
			break;
		case Phase.IsCard2:
			NextPhase(Phase.IsCard2Resolve);
			break;
		case Phase.IsCard2Resolve:
			NextPhase(Phase.IsCard3);
			break;
		case Phase.IsCard3:
			NextPhase(Phase.IsCard3Resolve);
			break;
		case Phase.IsCard3Resolve:
			//NextPhase(Phase.EndTurn);
			NextPhase(Phase.IsCard4);
			break;
		case Phase.IsCard4:
			NextPhase(Phase.IsCard4Resolve);
			break;
		case Phase.IsCard4Resolve:
			//NextPhase(Phase.EndTurn);
			NextPhase(Phase.EndTurn);
			break;
		case Phase.EndTurn:
			NextPhase(Phase.EndTurnResolve);
			break;
		case Phase.EndTurnResolve:
			NextPhase(Phase.ChooseMovePosition);
			break;
		}
	}

    public int playersReady;

	public void ReadyCheck()
	{
        playersReady = 0;
		foreach(Player_Sync_Variables playerS in players)
		{
            playersReady++;
			if(!playerS.ready)
			{
				allPlayersReady = false;
				break;
			}
			else
			{
				allPlayersReady = true;
			}
		}
	}

	public void DoneCheck()
	{
		foreach(Player_Sync_Variables playerS in players)
		{
			if(!playerS.turnDone)
			{
				allPlayersDone = false;
				break;
			}
			else
			{
				allPlayersDone = true;
			}
		}
	}
	

	void BeginPhases()
	{

		if(allPlayersReady)
		{

			phase = Phase.SpawnPlayers;

			foreach(Player_Sync_Variables player in players)
			{
				player.phase = Phase.SpawnPlayers;

				player.currentPhaseId = (int)phase;

               
			}

			
			MapSpawnerScript.startSpawning = true;
		}
	}

	void NextPhase(Phase nextPhase)
	{
		if(allPlayersDone)
		{
			
			phase = nextPhase;

			foreach(Player_Sync_Variables player in players)
			{
				player.phase = nextPhase;

				player.currentPhaseId = (int)phase;

				player.turnDone = false;
				player.turnStarted = false;
				player.phase = nextPhase;
			}


			allPlayersDone = false;
		}
	}
}
