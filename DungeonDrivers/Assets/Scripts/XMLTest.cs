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
    private GameObject myHand;

    public int totalCards;

    public GameObject Card;
    [SerializeField]
    private GameObject cardName;
    [SerializeField]
    private GameObject cardDescription;

    private List<GameObject> playerDeck = new List<GameObject>();

    //private Text CardTekst;
    //public Prefab testpref;

    void Awake()
    {
        //Load the XML document with his path
        myPath = Application.dataPath + "/XML/cards.xml";
        xmlDoc.Load(myPath);

        //CardTekst = Card.GetComponent<Text>();
    }
    // Use this for initialization
    void Start ()
    {        
        
        XmlNodeList cardNames = xmlDoc.GetElementsByTagName("name");
        totalCards = cardNames.Count;

        XmlNodeList cardDescription = xmlDoc.GetElementsByTagName("description");

        for (int cardID = 0; cardID < totalCards; cardID++)
        {
            GameObject newCard = Instantiate(Card) as GameObject;
            newCard.transform.SetParent(myHand.transform);
            newCard.name = cardNames[cardID].InnerXml;

            newCard.SetActive(true);

            /* Replace the beneath code */
            newCard.transform.GetChild(1).GetComponent<Text>().text = cardNames[cardID].InnerXml;
            newCard.transform.GetChild(2).GetComponent<Text>().text = cardDescription[cardID].InnerXml;
            /* To prevend a different Child Order of the Parent !!! */



            newCard.transform.localScale = new Vector3(1,1,1);
            newCard.transform.localPosition = new Vector3(0, 0, 0);
            //newCard.transform.Rotate(0, -90, 90, Space.Self );                //Quaternion.x();// = 0; //new Vector3(0, -90, 90);
            playerDeck.Add(newCard);
        }

        /*
        Debug.Log(playerDeck[0]);

        ShuffleDeck();

        Debug.Log(playerDeck[0]);
        Debug.Log(playerDeck[1]);
        Debug.Log(playerDeck[2]);
        //Debug.Log(playerDeck[3]);
        */

    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < playerDeck.Count; i++)
        {
            GameObject temp = playerDeck[i];
            int randomIndex = UnityEngine.Random.Range(0, totalCards);
            playerDeck[i] = playerDeck[randomIndex];
            playerDeck[randomIndex] = temp;
            Destroy(temp);
        }
    }

}
