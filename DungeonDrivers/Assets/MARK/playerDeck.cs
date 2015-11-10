using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;
//using UnityEditor;
using System.Collections;

public class playerDeck : MonoBehaviour
{
    string deckLocation;
    
    [SerializeField]
    private GameObject mySelectedClass;

    [SerializeField]
    private Transform myDeckPanel;

    public GameObject[] myDeck;                                         //Array of Deck
    [SerializeField]
    private List<GameObject> myTempDeck = new List<GameObject>();       //List of Deck

    [SerializeField]
    private List<GameObject> mySpawnedDeck = new List<GameObject>();

    [SerializeField]
    private Player_Hand myPlayerHand;

    [SerializeField]
    private Transform myPlayerHandT;

    [SerializeField]
    private GameObject gmObj;

    public int currentTurnID, previousTurnID;

    [SerializeField]
    private GameObject myDiscardPile;

    // Use this for initialization
    void Start ()
    {
        myDiscardPile = GameObject.Find("MyDiscardPile");
        
        //Locate the gamemanger
        gmObj = GameObject.Find("Game Manager");
        
        previousTurnID = 0;


        //Check which Class is selected.
        // And load the corresponding cards.
        if (mySelectedClass.GetComponent<ClassSelector>().playerClass == ClassSelector.Classes.Sandmage)
        {
            deckLocation = "Cards/SandMage";
        }
        if (mySelectedClass.GetComponent<ClassSelector>().playerClass == ClassSelector.Classes.Warrior)
        {
            deckLocation = "Cards/Warrior";
        }

        generateDeck();
        spawnDeck();
        shuffleDeck();
        drawHand();
        

       
    }

    // Update is called once per frame
    void Update ()
    {
        // Holds the previous and the current turn ID.
        // Here I can check if it's increasing? 
        currentTurnID = gmObj.GetComponent<Game_Manager_Script>().players[0].currentTurn;

        // If it's increasing i know im in a new round, so i need to draw a card. 
        if (currentTurnID > previousTurnID)
        {
            //Debug.Log("A new round has started.");
            // Check if there are still cards in the deck.
            if (mySpawnedDeck.Count != 0)
            {
                drawCard();
            }
            // If there aren't anymore cards to draw...
            if (mySpawnedDeck.Count == 0)
            {
                    int playedCards = myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards.Count;
                    Debug.Log(playedCards);

                    for (int i = 0; i < playedCards; i++)
                    {

                        //Debug.Log("Kaart # " + i);
                        //Debug.Log(myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards[i]);
                        // Add the card again to the spawned deck.
                        mySpawnedDeck.Add(myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards[i]);
                        // Set it's parent
                        myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards[i].transform.SetParent(myDeckPanel);

                        myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards[i].transform.localPosition = new Vector3(115, 0, 0);
                        myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards[i].transform.localRotation = Quaternion.Euler(new Vector3(90, 0));
                        myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards[i].transform.localScale = new Vector3(380, 380, 380);

                    // Remove the card from the discard pile
                    //myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards.Remove(myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards[i]);

                }
                
                myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards.Clear();
                shuffleDeck();
              
            }
                previousTurnID = currentTurnID;
        }
    }

    void generateDeck()
    {
        // The Deck is loaded in a array.
        myDeck = Resources.LoadAll<GameObject>("Cards/Warrior");
        
        // The Deck is loaded to a list instead of an array.
        for(int i = 0; i < myDeck.Length; i++)
        {
            // Add every element of the array to the list.
            myTempDeck.Add(myDeck[i]);
        }
    }

    void spawnDeck()
    {
       
        foreach (GameObject card in myTempDeck)
        {
            GameObject newCard = Instantiate(card) as GameObject;
            newCard.transform.SetParent(myDeckPanel);

            newCard.transform.localPosition = new Vector3(115, 0, 0);
            newCard.transform.localRotation = Quaternion.Euler(new Vector3(90, 0));

            newCard.transform.localScale = new Vector3(380, 380, 380);
            mySpawnedDeck.Add(newCard);
        }
    }

    void shuffleDeck()
    {
        Debug.Log("There currently are... " + mySpawnedDeck.Count + " cards in your deck." );

        for (int i = 0; i < mySpawnedDeck.Count; i++)
        {
            // For each card make a temporary copy.
            GameObject tempCard = mySpawnedDeck[i];
            Debug.Log("Tempcard created > " + mySpawnedDeck[i]);

            // Grab a random number, where to place it.
            int randomIndex = UnityEngine.Random.Range(0, mySpawnedDeck.Count);


            mySpawnedDeck[i] = mySpawnedDeck[randomIndex];
            mySpawnedDeck[randomIndex] = tempCard;

        }

    }

    void drawHand()
    {
        int cardsToDraw = 3;
        for (int i = 0; i < cardsToDraw; i++)
        {

			GameObject newCard = mySpawnedDeck[i];
			newCard.transform.SetParent(myPlayerHandT);
			myPlayerHand.myTempHand.Add(newCard);
			newCard.transform.localPosition = new Vector3(125 + i * 250,-80,0);
			newCard.transform.localScale = new Vector3(400, 400, 400);
			newCard.transform.localRotation = Quaternion.Euler(new Vector3(270, 0));
			newCard.GetComponent<BoxCollider>().enabled = true;

            mySpawnedDeck.Remove(mySpawnedDeck[i]);
            
            /*
            //mySpawnedDeck[i].transform.localPosition = new Vector3(0, 0, 0);
            //mySpawnedDeck[i].AddComponent<LayoutElement>();
            //mySpawnedDeck[i].GetComponent<LayoutElement>().preferredWidth = 120;
            //mySpawnedDeck[i].GetComponent<LayoutElement>().preferredHeight = 80;
            // myTempHand.Add(myTempDeck[i]);
            */
        }
    }

    void drawCard()
    {
       
            GameObject drawedCard = mySpawnedDeck[0];
            drawedCard.transform.SetParent(myPlayerHandT);
            myPlayerHand.myTempHand.Add(drawedCard);

            drawedCard.transform.localPosition = new Vector3(125 + 3 * 250, -80, 0);
            drawedCard.transform.localScale = new Vector3(400, 400, 400);
            drawedCard.transform.localRotation = Quaternion.Euler(new Vector3(270, 0));
            drawedCard.GetComponent<BoxCollider>().enabled = true;

            mySpawnedDeck.Remove(mySpawnedDeck[0]);
    }

    void PlacePlayedCardsBackInDeck()
    {
        int discardedCards = myDiscardPile.GetComponent<Discard_Pile_Mark>().myPlayedCards.Count;
        for (int i = 0; i < discardedCards; i++)
        {
            // Do something
        }
    }
}
