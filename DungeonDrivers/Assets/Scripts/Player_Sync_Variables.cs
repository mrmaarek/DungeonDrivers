using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class Player_Sync_Variables
{
	[SyncVar]
	public int playerID, currentTurn, health, maxMoves, currentPhaseId;
	[SyncVar]
	public int currentMaxMoves = 3;
	[SyncVar]
	public bool turnDone, ready, turnStarted;
	[SyncVar]
	public Vector3 nextPosition, currentPosition;
	[SyncVar]
	public GameObject CurrentGridBlock, NextGridBlock;

	[SyncVar]
	public Game_Manager_Script.Phase phase;

	[SyncVar]
	public int currentBlockId, previousBlockId;

	[SyncVar]
	public Card_Script Card_Script;

	[SyncVar]
	public int cardId;

	[SyncVar]
	public string playerClass;

}
