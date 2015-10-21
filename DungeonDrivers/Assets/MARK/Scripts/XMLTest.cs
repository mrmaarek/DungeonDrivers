using UnityEngine;
using UnityEngine.UI;
using System;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class XMLTest : MonoBehaviour
{
    XmlDocument xmlDoc = new XmlDocument();
    string myPath;

    [SerializeField]
    private GameObject thePlayer;

    [SerializeField]
    private GameObject myHand;
    [SerializeField]
    private GameObject myDeck;

    public int totalCards;

    public GameObject Card;

    public List<GameObject> playerDeck = new List<GameObject>();
    public List<GameObject> playerHand = new List<GameObject>();


    void Awake()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player");
  

        myPath = Application.dataPath + "/XML/Warrior.xml";
        
        /*
         *  Currently Disabled for testing purposes
         * 
        if (thePlayer.GetComponent<Player>().playerClass == Player.Classes.SandMage)
        {
            myPath = Application.dataPath + "/XML/SandMage.xml";
        }
        else if (thePlayer.GetComponent<Player>().playerClass == Player.Classes.Warrior)
        {
            myPath = Application.dataPath + "/XML/Warrior.xml";
        }
        
        */
        xmlDoc.Load(myPath);
        //CardTekst = Card.GetComponent<Text>();
    }
    // Use this for initialization
    void Start ()
    {        
        
        XmlNodeList cardNames = xmlDoc.GetElementsByTagName("name");
        totalCards = cardNames.Count;
        XmlNodeList cardDescriptions = xmlDoc.GetElementsByTagName("description");
        Debug.Log(totalCards);

        //Voor elke kaart... maak een nieuw gameobject en vul de gegevens in.
        for (int cardID = 0; cardID < totalCards; cardID++)
        {
            // Instantieren van een nieuw gameobject.
            GameObject newCard = Instantiate(Card) as GameObject;
            Debug.Log(cardID + cardNames[cardID].InnerXml + cardDescriptions[cardID].InnerXml);

 
            //Give the new GameObject a name and set myDeck to it's parent.
            newCard.name = cardNames[cardID].InnerXml;
            newCard.transform.SetParent(myDeck.transform);

            /* Replace the beneath code */
            //Getting the transform of the card.
            Transform cardTitle;
            Transform cardDescript;

            //Getting the Text component of Card_Title and set it's name. 
            cardTitle = newCard.gameObject.transform.GetChild(0).GetChild(1);
            cardTitle.GetComponent<Text>().text = cardNames[cardID].InnerXml;
            //Getting the Text component of Card_Description and set it's name. 
            cardDescript = newCard.gameObject.transform.GetChild(0).GetChild(2);
            cardDescript.GetComponent<Text>().text = cardDescriptions[cardID].InnerXml;
            /* To prevend a different Child Order of the Parent !!! */

            newCard.SetActive(true);                  

            newCard.transform.localScale = new Vector3(1,1,1);
            newCard.transform.localPosition = new Vector3(0, 0, 0);
            newCard.transform.rotation = new Quaternion(0, 0, 0, 0);
            //newCard.transform.Rotate(0, -90, 90, Space.Self );                //Quaternion.x();// = 0; //new Vector3(0, -90, 90);
            playerDeck.Add(newCard);
            //Debug.Log(newCard);
            //Debug.Log(playerDeck[cardID]);
        }

        pickTopCard();
        pickTopCard();
        pickTopCard();
        

    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < playerDeck.Count; i++)
        {
            GameObject temp = playerDeck[i];
            int randomIndex = UnityEngine.Random.Range(0, totalCards);
            playerDeck[i] = playerDeck[randomIndex];
            playerDeck[randomIndex] = temp;
            //Destroy(temp);

            //Debug.Log(playerDeck[i]);
        }
    }

    public void pickTopCard()
    {
        // Set it's position to the hand.
        playerDeck[0].transform.position = myHand.transform.position;
        


        playerDeck[0].transform.SetParent(myHand.transform);
        //flipCard();

        playerHand.Add(playerDeck[0]);
        playerDeck.RemoveAt(0);

        playerHand[0].transform.Rotate(0, 180, 0);
        
        //playerDeck.RemoveAt(0);
        //Debug.Log(playerDeck[0]);
        //myCard.flipCard();
        //playerHand[0].transform.SetParent(myHand.transform);
    }

    public void flipCard()
    {
        //playerHand[0].transform.Rotate(new Vector3(0, 180, 0));
    }

    void Update()
    {
        
    }
}
