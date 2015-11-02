using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;


//myDeck = Resources.LoadAll("Resources/Cards/Warrior/", typeof(GameObject));

 
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
    private List<GameObject> myTempHand = new List<GameObject>();

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
            deckLocation = "Cards/Warior";
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
        myDeck = Resources.LoadAll<GameObject>("Cards/Warior");
      
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
            newCard.transform.position = new Vector3(0, 0, 0);
            newCard.transform.localPosition = new Vector3(0, 0, 0);
            newCard.transform.Rotate(new Vector3(90, 180, 0));
            newCard.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            
        }
    }

    void shuffleDeck()
    {
        Debug.Log("There currently are... " + myTempDeck.Count + " cards in your deck." );

        for (int i = 0; i < myTempDeck.Count; i++)
        {
            // For each card make a temporary copy.
            GameObject tempCard = myTempDeck[i];
            int randomIndex = UnityEngine.Random.Range(0, myDeck.Length);
            myTempDeck[i] = myTempDeck[randomIndex];
            myTempDeck[randomIndex] = tempCard;

            Debug.Log(myTempDeck[i]);
            Debug.Log(myTempDeck[randomIndex]);

            //Destroy(tempCard);
        }

    }

    void drawHand()
    {
        int cardsToDraw = 3;
        for (int i = 0; i < cardsToDraw; i++)
        {
            myTempHand.Add(myTempDeck[i]);
        }
    }
}
