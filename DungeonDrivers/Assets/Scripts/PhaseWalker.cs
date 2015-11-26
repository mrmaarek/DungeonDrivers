﻿using UnityEngine;
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

	public GameObject PlayerDeckObject;

	public GameObject ClassSelector, ClassSelectorUI;

	public GameObject  PlayerHand, CardPos;
	//public List<GameObject> cardsInHand = new List<GameObject>();

	//private Player_Deck_Script Player_Deck_Script;


	// Add the player to the list of objects moving along with the camera, set the starting vars.
	void Awake()
	{
		GameObject.Find("Map Spawner").GetComponent<MapSpawnerScript>().ObjectsMovingAlong.Add(this.gameObject);
		//GetComponent<Player_Deck_Script>().enabled = true;
		Grid_Spawner_Script = GetComponent<Grid_Spawner_Script>();
		Game_Manager_Script = GameObject.Find("Game Manager").GetComponent<Game_Manager_Script>();
		Game_Manager_Script.players.Add(new Player_Sync_Variables());
		//Player_Deck_Script = GetComponent<Player_Deck_Script>();

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

		//Grid_Spawner_Script.SetupGrid();
		


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

				Player_Sync_Variables.playerClass = ClassSelector.GetComponent<ClassSelector>().playerClass.ToString();
				Destroy(ClassSelector);
				Destroy(ClassSelectorUI);

				Grid_Spawner_Script.SetupGrid();
				LoadDeck();
				FillHand();

				//PlayerDeckObject.GetComponent<playerDeck>().enabled = true;
				
				LoadAllCards();

				for(int i = 0; i < playerStartIDs.Length; i++)
				{
					PlayerStartGridBlocks.Add(Grid_Spawner_Script.gridBlocks[playerStartIDs[i]]);
				}
				
				CurrentGridBlock = PlayerStartGridBlocks[playerID];
				currentPlayerPos = CurrentGridBlock.transform.localPosition;
				CmdSendPosition(currentPlayerPos);

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


				DrawCard();
			}

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
                


				/*
				cardsInHand = PlayerHand.GetComponent<Player_Hand>().myTempHand;
				foreach(GameObject cardInHand in cardsInHand)
				{
					cardInHand.GetComponent<Card_Script>().PhaseWalker = this;
				}
				*/

                
            }
			
			SelectCard();
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


				CmdSetCard();


				for(int i = 0; i < hand.Count; i++)
				{
					if(hand[i] == CardCurrentlyPlayed)
					{
						discardPile.Add(hand[i]);
						hand.RemoveAt(i);
					}
				}
				RefilDeck();
				//Destroy(Player_Deck_Script.CardCurrentlyPlayed);
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
				/*
				int i = 0;

				foreach(GameObject playerObject in spawnedPlayers)
				{

					Player_Sync_Variables Temp_Player_Sync_Variables = playerObject.GetComponent<PhaseWalker>().Player_Sync_Variables;
					if(Temp_Player_Sync_Variables.Card_Script.initiative == 4)
					{
						Card_Script Temp_Card_Script = Player_Sync_Variables.Card_Script;



					}

					i++;
				}
				*/

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
        // Deze heb ik nodig.
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

	[Command]
	void CmdSetCard()
	{
		RpcSetCard();
	}



	[ClientRpc]
	void RpcSetCard()
	{
		Player_Sync_Variables.cardId = currentCardId;
		Debug.Log(currentCardId);



		/*
		for(int i = 0; i < Player_Deck_Script.tempDeck.Length; i++)
		{
			if(Player_Deck_Script.tempDeck[i].GetComponent<Card_Script>().cardName == Player_Deck_Script.CardCurrentlyPlayed.GetComponent<Card_Script>().cardName)
			{
				Player_Sync_Variables.Card_Script = Player_Deck_Script.tempDeck[i].GetComponent<Card_Script>();
			}
		}
		*/
	}

	
	public GameObject[] allCards2;
	public List<GameObject> allCards = new List<GameObject>();
	
	public List<GameObject> deck, hand, discardPile = new List<GameObject>();
	public GameObject[] tempDeck;
	public GameObject DeckObject, HandObject, DiscardPile, CardToPlay, CardCurrentlyPlayed;
	
	public LayerMask cardSelectLayerMask;
	
	public int currentCardId;
	

	
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

		tempDeck = Resources.LoadAll<GameObject>("Cards/" + Player_Sync_Variables.playerClass);

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
						
						for(int i = 0; i < allCards.Count; i++)
						{
							if(allCards[i].name + "(Clone)" == cardHit.name)
							{
								currentCardId = i;
							}
						}
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
