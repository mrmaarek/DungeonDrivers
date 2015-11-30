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
		IsUtility = 8,
		IsUtilityResolve = 9,
		isCCing = 10,
		IsCCingResolve = 11,
		IsAttacking = 12,
		IsAttackingResolve = 13,
		EndTurn = 14,
		EndTurnResolve = 15
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
			NextPhase(Phase.IsAttacking);
			break;
		case Phase.IsUtility:
			NextPhase(Phase.IsUtilityResolve);
			break;
		case Phase.IsUtilityResolve:
			NextPhase(Phase.isCCing);
			break;
		case Phase.isCCing:
			NextPhase(Phase.IsCCingResolve);
			break;
		case Phase.IsCCingResolve:
			NextPhase(Phase.IsAttacking);
			break;
		case Phase.IsAttacking:
			NextPhase(Phase.IsAttackingResolve);
			break;
		case Phase.IsAttackingResolve:
			//NextPhase(Phase.EndTurn);
			NextPhase(Phase.ChooseMovePosition);
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
