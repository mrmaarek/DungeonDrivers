using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;


//myDeck = Resources.LoadAll("Resources/Cards/Warrior/", typeof(GameObject));

 
public class playerDeck : MonoBehaviour
{
    string deckLocation;
    public GameObject[] myDeck;

    [SerializeField]
    private GameObject mySelectedClass;

    [SerializeField]
    private Transform myDeckPanel;

   


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

    }

    // Update is called once per frame
    void Update ()
    {
        
    }

    void generateDeck()
    {
        myDeck = Resources.LoadAll<GameObject>("Cards/Warior");
        //myDeck = Resources.LoadAll<GameObject>(deckLocation);
        Debug.Log(myDeck);
    }

    void spawnDeck()
    {
       
        foreach (GameObject card in myDeck)
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
        Debug.Log("There currently are... " + myDeck.Length + " cards in your deck." );

        for (int i = 0; i < myDeck.Length; i++)
        {
            // For each card make a temporary copy.
            GameObject tempCard = myDeck[i];
            int randomIndex = UnityEngine.Random.Range(0, myDeck.Length);
            myDeck[i] = myDeck[randomIndex];
            myDeck[randomIndex] = tempCard;

            //Destroy(tempCard);
        }

    }
}
