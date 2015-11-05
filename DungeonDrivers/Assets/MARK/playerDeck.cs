using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEditor;
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

    // Use this for initialization
    void Start ()
    {
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
        //shuffleDeck();
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

            newCard.transform.localPosition = new Vector3(75, 0, 0);
            newCard.transform.localRotation = Quaternion.Euler(new Vector3(90, 0));

            newCard.transform.localScale = new Vector3(200, 200, 200);
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
            int randomIndex = UnityEngine.Random.Range(0, myDeck.Length);
            mySpawnedDeck[i] = mySpawnedDeck[randomIndex];
            mySpawnedDeck[randomIndex] = tempCard;

            Debug.Log(mySpawnedDeck[i]);
            Debug.Log(mySpawnedDeck[randomIndex]);

            //Destroy(tempCard);
        }

    }

    void drawHand()
    {
        int cardsToDraw = 3;
        for (int i = 0; i < cardsToDraw; i++)
        {
            mySpawnedDeck[i].transform.SetParent(myPlayerHandT);
            myPlayerHand.myTempHand.Add(mySpawnedDeck[i]);
            //mySpawnedDeck[i].transform.localPosition = new Vector3(0, 0, 0);
            mySpawnedDeck[i].AddComponent<LayoutElement>();
            mySpawnedDeck[i].GetComponent<LayoutElement>().preferredWidth = 120;
            mySpawnedDeck[i].GetComponent<LayoutElement>().preferredHeight = 80;
            // myTempHand.Add(myTempDeck[i]);
        }
    }
}
