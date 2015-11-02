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
        }
      



        /*
        foreach (GameObject card in myDeck)
        {
            //Instantiate(card) as GameObject;

            GameObject test = Instantiate(card, new Vector3(0,0,0), Quaternion.identity) as GameObject; 
            card.transform.SetParent(myDeckPanel);
        }
        */
    }
}
