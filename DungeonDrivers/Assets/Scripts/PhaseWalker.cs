using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PhaseWalker : NetworkBehaviour 
{
	// Setup vars
	public Game_Manager_Script Game_Manager_Script;
	public Player_Sync_Variables Player_Sync_Variables; // The players syncing variables
	public Grid_Spawner_Script Grid_Spawner_Script;

	public int playerID;

	public List<Phase> phases = new List<Phase>();	// Contains all phases a player can go trough.

	public int currentPhaseId;
	public Phase currentPhase;
	
	private GameObject PlayerCamera;
	public GameObject ObjectsToTurnOn;	// Will be turned on when te player spawns.

	private float doneTimer;

	public List<string> phaseFuncions = new List<string>(); // Functions that belong to the phases.
	
	public int[] playerStartIDs; // The ID's of the gridblokcs used as spawnpoints.

	
	private List<GameObject> PlayerStartGridBlocks = new List<GameObject>(); // The player spawnpoints


	// Movement vars
	public GameObject CurrentGridBlock, NextGridBlock, PreviousGridBlock;
	public Vector3 currentPlayerPos, nextPlayerPos, previousPlayerPos;

	// All the in wordl players.
	public List<GameObject> spawnedPlayers = new List<GameObject>();
	public GameObject PlayerBlockObject, PlayerContainer;

	private bool canMove = true; // Used to block movement.
	
	public LayerMask layerMask; // Used for gridblock detection


	/*
	public List<LaneEvent> laneEvents = new List<LaneEvent>();
	public GameObject LaneWarning;
	public List<GameObject> laneWarnings = new List<GameObject>();
	*/

	// Add the player to the list of objects moving along with the camera, set the starting vars.
	void Awake()
	{
		GameObject.Find("Map Spawner").GetComponent<MapSpawnerScript>().ObjectsMovingAlong.Add(this.gameObject);
		Grid_Spawner_Script = GetComponent<Grid_Spawner_Script>();
		Game_Manager_Script = GameObject.Find("Game Manager").GetComponent<Game_Manager_Script>();
		Game_Manager_Script.players.Add(new Player_Sync_Variables());

		playerID = Game_Manager_Script.players.Count - 1; // Give the player his id.
	}

	// Activate the startvars
	void Start()
	{
		if(isLocalPlayer)
		{
			SetStartVariables();
		}
	}
		
	void Update () 
	{
		GameFlow();
		if(isLocalPlayer)
		{
			CmdSetStartVariables();
		}
	}

	// Setup for the player at the start of the game.
	void SetStartVariables()
	{
		CmdSetStartVariables();


		GameObject.Find("Scene Camera").SetActive(false);
		
		foreach(Camera camera in GetComponentsInChildren<Camera>(true))
		{
			if(camera.tag == "PlayerCamera")
			{
				PlayerCamera = camera.gameObject;
			}
		}
		ObjectsToTurnOn.SetActive(true);

		Grid_Spawner_Script.SetupGrid();
		
		for(int i = 0; i < playerStartIDs.Length; i++)
		{
			PlayerStartGridBlocks.Add(Grid_Spawner_Script.gridBlocks[playerStartIDs[i]]);
		}
		
		CurrentGridBlock = PlayerStartGridBlocks[playerID];
		currentPlayerPos = CurrentGridBlock.transform.localPosition;
		CmdSendPosition(currentPlayerPos);

	}

	// Set the syncing vars.
	[Command]
	void CmdSetStartVariables()
	{
		RpcSetStartVariables();
	}
	
	[ClientRpc]
	void RpcSetStartVariables()
	{
		Player_Sync_Variables = Game_Manager_Script.players[playerID];
		Player_Sync_Variables.playerID = playerID;
	}

	// Used to activate the main gameplay mechanics.
	void GameFlow()
	{
		// Searching for the current phase and activates the funcion.
		currentPhaseId = Player_Sync_Variables.currentPhaseId;
		currentPhase = phases[currentPhaseId];

		for(int k = 0; k < phaseFuncions.Count; k++)
		{
			if(k == currentPhaseId)
			{
				if(phaseFuncions[k] != "")
				{
					Invoke(phaseFuncions[k], 0);
				}
			}
		}

		/*
		switch(currentPhaseId)
		{
		case 0:

			break;
		case 1:
			SpawnPlayers();
			break;
		case 2:
			PlayerIsChoosingMovement();
			break;
		case 3:
			PlayerIsChoosingMovementResolve();
			break;
		case 4:
			PlayerIsChoosingCard();
			break;
		case 5:
			PlayerIsChoosingCardResolve();
			break;
		case 6:
			PlayerIsMoving();
			break;
		case 7:
			PlayerIsMovingResolve();
			break;
		case 8:
			PlayerIsUtility();
			break;
		case 9:
			PlayerIsUtilityResolve();
			break;
		case 10:
			PlayerIsCC();
			break;
		case 11:
			PlayerIsCCResolve();
			break;
		case 12:
			PlayerIsAttack();
			break;
		case 13:
			PlayerIsAttackResolve();
			break;
		case 14:
			PlayerIsEndTurn();
			break;
		case 15:
			PlayerIsEndTurnResolve();
			break;
		}
		*/

		// Completes a turn for the player.
		if(isLocalPlayer)
		{
			if(currentPhase.phaseType == Phase.PhaseType.AutoDone)
			{
				if(!currentPhase.turnEnded)
				{
					doneTimer += Time.deltaTime;

					if(doneTimer >= currentPhase.doneDelay)
					{
						CmdTurnDone();
						currentPhase.turnEnded = true;
						doneTimer = 0;
					}
				}
			}
		}
		/*
		switch(Player_Sync_Variables.phase)
		{
		case Game_Manager_Script.Phase.SpawnPlayers:
			SpawnPlayers();
			break;
		case Game_Manager_Script.Phase.ChooseMovePosition:
			PlayerIsChoosingMovement();
			break;
		case Game_Manager_Script.Phase.PlayCards:
			PlayerIsPlayingCards();
			break;
		case Game_Manager_Script.Phase.IsMoving:
			PlayerIsMoving();
			break;
		case Game_Manager_Script.Phase.IsUtility:
			PlayerIsUtility();
			break;
		case Game_Manager_Script.Phase.IsAttacking:
			
			break;
		case Game_Manager_Script.Phase.isCCing:
			
			break;
		}
*/
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Spawn Players
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	

	// Spawns the in-world/visible players.
	public void SpawnPlayers()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();

				for(int i = 0; i < Game_Manager_Script.players.Count; i++)
				{
					GameObject NewPlayerBlock = Instantiate(PlayerBlockObject, Vector3.zero, Quaternion.identity) as GameObject;
					spawnedPlayers.Add(NewPlayerBlock);
					NewPlayerBlock.transform.SetParent(PlayerContainer.transform);
					NewPlayerBlock.transform.localPosition = new Vector3(PlayerStartGridBlocks[i].transform.localPosition.x, PlayerStartGridBlocks[i].transform.localPosition.z, PlayerStartGridBlocks[i].transform.localPosition.y);
				}
			}
		}
	}
	
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Choose Movement
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Choose the location the player will move towards.


	bool stopDeselecting;
	public List<GameObject> moveBlocks = new List<GameObject>(); // All avaidable gridblocks for movement.

	public void PlayerIsChoosingMovement()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();

				CmdSetTurn();

				/*
				for(int i = laneEvents.Count - 1; i >= 0; i--)
				{
					if(laneEvents[i].turnsLeft <= 0)
					{
						foreach(GameObject laneW in laneEvents[i].laneWarnings)
						{
							Destroy(laneW);
						}
						laneEvents.RemoveAt(i);
					}
				}

				foreach(LaneEvent laneEvent in laneEvents)
				{


					if(laneEvent.turnsLeft == 1)
					{
						foreach(GameObject gridBlockW in Grid_Spawner_Script.gridBlocks)
						{
							if(gridBlockW.GetComponent<Grid_Block_Script>().xLane == laneEvent.xLane)
							{
								
								GameObject laneWarning = Instantiate(LaneWarning) as GameObject;
								laneWarning.transform.SetParent(gridBlockW.transform);
								laneWarning.transform.localPosition = Vector3.zero;
								laneEvent.laneWarnings.Add(laneWarning);
							}
						}
					}

					laneEvent.turnsLeft--;
				}
				*/
			}
			/*
			if(Input.GetKeyDown(KeyCode.Q))
			{
				int randomLane = Random.Range(1,6);
				int randomTurnsLeft = Random.Range(1,4);
				laneEvents.Add(new LaneEvent(laneEvents.Count ,randomTurnsLeft, 1, randomLane));
			}
			*/

			if(canMove)
			{
				PlayerMovementSelector();
				SelectWithMouse();
				stopDeselecting = false;
			}
			else
			{
				if(!stopDeselecting)
				{
					stopDeselecting = true;
					DeselectAllGridBlocks();
				}
			}
		}
	}

	// Acivate all reachable gridblocks.
	void PlayerMovementSelector()
	{
		moveBlocks.Clear(); // Refresh the list of avaidable gridblocks.


		// Select the gridblocks.
		if(Player_Sync_Variables.currentMaxMoves > 0)
		{
			SelectSurroundingBlocks(CurrentGridBlock.transform.position);
		}

		int bCount;
		for(int i = Player_Sync_Variables.currentMaxMoves; i > 1; i--)
		{
			bCount = moveBlocks.Count;
			for(int j = 0; j < bCount; j++)
			{
				SelectSurroundingBlocks(moveBlocks[j].transform.position);
			}
		}

		// Activate them
		foreach(GameObject moveBlockDef in moveBlocks)
		{
			ActivateGridBlock(moveBlockDef);
		}
	}

	// Select the 4 surrounding gridblocks.
	void SelectSurroundingBlocks(Vector3 selectPos)
	{
		float tileSize = Grid_Spawner_Script.tileSize;

		for(int i = 0; i < 4; i++)
		{
			GameObject selected2 = null;
			switch(i)
			{
			case 0:
				selected2 = SelectMoveBlock(new Vector3(selectPos.x + tileSize, selectPos.y, selectPos.z));
				break;
			case 1:
				selected2 = SelectMoveBlock(new Vector3(selectPos.x - tileSize, selectPos.y, selectPos.z));
				break;
			case 2:
				selected2 = SelectMoveBlock(new Vector3(selectPos.x, selectPos.y, selectPos.z + tileSize));
				break;
			case 3:
				selected2 = SelectMoveBlock(new Vector3(selectPos.x , selectPos.y, selectPos.z - tileSize));
				break;
			}
			if(selected2 != null)
			{
				moveBlocks.Add(selected2);
			}
		}
		/*
		GameObject selected2 = SelectMoveBlock(new Vector3(selectPos.x + tileSize, selectPos.y, selectPos.z));
		if(selected2 != null)
		{
			moveBlocks.Add(selected2);
		}
		GameObject selected3 = SelectMoveBlock(new Vector3(selectPos.x - tileSize, selectPos.y, selectPos.z));
		if(selected3 != null)
		{
			moveBlocks.Add(selected3);
		}
		GameObject selected4 = SelectMoveBlock(new Vector3(selectPos.x, selectPos.y, selectPos.z + tileSize));
		if(selected4 != null)
		{
			moveBlocks.Add(selected4);
		}
		GameObject selected5 = SelectMoveBlock(new Vector3(selectPos.x , selectPos.y, selectPos.z - tileSize));
		if(selected5 != null)
		{
			moveBlocks.Add(selected5);
		}
		*/
	}

	// Select a gridblock.
	GameObject SelectMoveBlock(Vector3 blockPos)
	{
		RaycastHit vHit = new RaycastHit();
		
		if(Physics.Linecast(PlayerCamera.transform.position, blockPos, out vHit, layerMask))
		{
			foreach(GameObject hitObject in moveBlocks)
			{
				if(hitObject == vHit.collider.gameObject)
				{
					return null;
				}
			}
			return vHit.collider.gameObject;
		}
		else
		{
			return null;
		}
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Choose Movement Resolve
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


	public void PlayerIsChoosingMovementResolve()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();

				canMove = true;

				int i = 0;
				foreach(GameObject gridB in Grid_Spawner_Script.gridBlocks)
				{
					if(gridB == CurrentGridBlock)
					{
						CmdSetCurrentPosition(i);
						break;
					}
					i++;
				}

				DeselectAllGridBlocks();
				DeactivateAllGridBlocks();
				PreviousGridBlock = null;
			}
		}
	}

	


	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Choose Card
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsChoosingCard()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Choose Card Resolve
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsChoosingCardResolve()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Move
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsMoving()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}

			int i = 0;
			foreach(GameObject playerObject in spawnedPlayers)
			{
				
				GameObject playerGridBlock = Grid_Spawner_Script.gridBlocks[Game_Manager_Script.players[i].currentBlockId];
				Grid_Block_Script Grid_Block_Script = playerGridBlock.GetComponent<Grid_Block_Script>();
				Grid_Block_Script.playersInSide[i] = true;
				
				GameObject playerPreviousGridBlock = Grid_Spawner_Script.gridBlocks[Game_Manager_Script.players[i].previousBlockId];
				Grid_Block_Script Grid_Block_Script2 = playerPreviousGridBlock.GetComponent<Grid_Block_Script>();
				Grid_Block_Script2.playersInSide[i] = false;
				
				playerObject.transform.localPosition = Vector3.MoveTowards(playerObject.transform.localPosition, new Vector3(Game_Manager_Script.players[i].currentPosition.x, Game_Manager_Script.players[i].currentPosition.z, Game_Manager_Script.players[i].currentPosition.y), 3 * Time.deltaTime);

				i++;
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Move Resolve
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsMovingResolve()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Utility
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsUtility()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}
	
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Utility Resolve
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsUtilityResolve()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// CC
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsCC()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}
	
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// CC Resolve
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsCCResolve()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Attack
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsAttack()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}
	
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Attack Resolve
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsAttackResolve()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// End Turn
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsEndTurn()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();

				/*
				foreach(LaneEvent laneEvent in laneEvents)
				{
					if(laneEvent.turnsLeft == 0)
					{
						foreach(GameObject gridBlockW in Grid_Spawner_Script.gridBlocks)
						{
							Grid_Block_Script G_Script = gridBlockW.GetComponent<Grid_Block_Script>();
							if(G_Script.xLane == laneEvent.xLane)
							{
								for(int i = 0; i < G_Script.playersInSide.Length; i++)
								{
									if(G_Script.playersInSide[i])
									{
										for(int j = 0; j < Game_Manager_Script.players.Count; j++)
										{
											CmdDoDamage(laneEvent.damage, i);
										}

									}
								}
							}
						}
					}
				}
				*/
			}
		}
	}
	
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// End Turn Resolve
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void PlayerIsEndTurnResolve()
	{
		if(isLocalPlayer)
		{
			if(!currentPhase.turnStarted)
			{
				ResetPhases();
			}
		}
	}



	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	// Turn Done Check
	//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	[Command]
	public void CmdTurnDone()
	{
		RpcTurnDone();
	}
	
	[ClientRpc]
	public void RpcTurnDone()
	{
		Player_Sync_Variables.turnDone = !Player_Sync_Variables.turnDone;
		
		Game_Manager_Script.DoneCheck();
	}

	//------------------------------------------------------------------------------------------------------
	// Ready Check
	//------------------------------------------------------------------------------------------------------
	
	[Command]
	public void CmdReady()
	{
		RpcReady();
	}
	
	[ClientRpc]
	public void RpcReady()
	{
		Player_Sync_Variables.ready = !Player_Sync_Variables.ready;
		
		Game_Manager_Script.ReadyCheck();
	}

	//------------------------------------------------------------------------------------------------------
	// Grid Selection
	//------------------------------------------------------------------------------------------------------

	public void SelectGridBlock(Vector3 gridBlockPos)
	{
		RaycastHit vHit = new RaycastHit();
		
		if(Physics.Linecast(PlayerCamera.transform.position, gridBlockPos, out vHit, layerMask))
		{
			ActivateGridBlock(vHit.collider.gameObject);
		}
	}
	
	void ActivateGridBlock(GameObject GridBlockSelected)
	{
		GameObject HitGridBlock = GridBlockSelected;
		Grid_Block_Script Temp_Grid_Block_Script = HitGridBlock.GetComponent<Grid_Block_Script>();
		Temp_Grid_Block_Script.isActive = true;
	}
	
	public void SelectWithMouse()
	{
		RaycastHit vHit = new RaycastHit();
		
		Ray vRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(vRay, out vHit, 1000, layerMask)) 
		{
			SelectGridBlockWithMouse(vHit.collider.gameObject);
		}
	}
	
	void SelectGridBlockWithMouse(GameObject gridBlockSelected)
	{
		DeselectAllGridBlocks();
		GameObject HitGridBlock = gridBlockSelected;
		Grid_Block_Script Temp_Grid_Block_Script = HitGridBlock.GetComponent<Grid_Block_Script>();
		Temp_Grid_Block_Script.isSelected = true;
		
		if(Input.GetMouseButtonUp(0))
		{
			if(gridBlockSelected != null)
			{
				if(Temp_Grid_Block_Script.isActive)
				{
					MoveToPosition(gridBlockSelected);
				}
			}
		}
	}

	void MoveToPosition(GameObject gridBlockSelected)
	{
		PreviousGridBlock = CurrentGridBlock;
		int i = 0;
		foreach(GameObject gridB in Grid_Spawner_Script.gridBlocks)
		{
			if(gridB == PreviousGridBlock)
			{
				CmdSendPreviousPosition(i);
				break;
			}
			i++;
		}	
		CurrentGridBlock.GetComponent<Grid_Block_Script>().isMovePos = false;
		CurrentGridBlock = gridBlockSelected;
		currentPlayerPos = gridBlockSelected.transform.localPosition;
		CmdSendPosition(currentPlayerPos);		
		canMove = false;
		DeactivateAllGridBlocks();
		CurrentGridBlock.GetComponent<Grid_Block_Script>().isMovePos = true;
	}

	void DeactivateAllGridBlocks()
	{
		for(int i = 0; i < Grid_Spawner_Script.gridBlocks.Count; i++)
		{
			Grid_Spawner_Script.gridBlocks[i].GetComponent<Grid_Block_Script>().isActive = false;
		}
	}
	void DeselectAllGridBlocks()
	{
		for(int i = 0; i < Grid_Spawner_Script.gridBlocks.Count; i++)
		{
			Grid_Spawner_Script.gridBlocks[i].GetComponent<Grid_Block_Script>().isSelected = false;
		}
	}
	
	public void BackToPreviousGridBlock()
	{
		if(!canMove)
		{
			canMove = true;
			CurrentGridBlock.GetComponent<Grid_Block_Script>().isMovePos = false;
			CurrentGridBlock = PreviousGridBlock;
			CurrentGridBlock.GetComponent<Grid_Block_Script>().isMovePos = true;
			DeactivateAllGridBlocks();
			CmdSendPosition(CurrentGridBlock.transform.localPosition);	
		}
	}

	//------------------------------------------------------------------------------------------------------
	// 
	//------------------------------------------------------------------------------------------------------




	void ResetPhases()
	{
		foreach(Phase phase in phases)
		{
			phase.turnStarted = false;
			phase.turnEnded = false;
		}
		currentPhase.turnStarted = true;
	}

	[Command]
	void CmdSendPosition(Vector3 currentPlayerP)
	{
		RpcSendPosition(currentPlayerP);
	}
	
	[ClientRpc]
	void RpcSendPosition(Vector3 currentPlayerP)
	{
		Player_Sync_Variables.nextPosition = currentPlayerP;
	}

	[Command]
	void CmdSetCurrentPosition(int blockId)
	{
		RpcSetCurrentPosition(blockId);
	}
	
	[ClientRpc]
	void RpcSetCurrentPosition(int blockId)
	{
		currentPlayerPos = nextPlayerPos;
		Player_Sync_Variables.currentBlockId = blockId;
		Player_Sync_Variables.currentPosition = Player_Sync_Variables.nextPosition;
	}

	[Command]
	void CmdSendPreviousPosition(int blockId)
	{
		RpcSendPreviousPosition(blockId);
	}
	
	[ClientRpc]
	void RpcSendPreviousPosition(int blockId)
	{
		Player_Sync_Variables.previousBlockId = blockId;
	}

	[Command]
	void CmdSetTurn()
	{
		RpcSetTurn();
	}
	
	[ClientRpc]
	void RpcSetTurn()
	{
		Player_Sync_Variables.currentTurn++;
	}

	[Command]
	void CmdDoDamage(int damage, int pId)
	{
		RpcDoDamage(damage, pId);
	}
	
	[ClientRpc]
	void RpcDoDamage(int damage, int pId)
	{
		Game_Manager_Script.players[pId].health -= damage;
	}

}



//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class Phase
{
	public string name;

	public bool turnStarted, turnEnded;

	public PhaseType phaseType;
	public enum PhaseType
	{
		AutoDone,
		ManualDone
	}

	public float doneDelay;

	public Phase(string tName)
	{
		name = tName;
	}
}
