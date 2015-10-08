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


    public GameObject Card;
    [SerializeField]
    private GameObject cardName;
    [SerializeField]
    private GameObject cardDescription;

    private Text CardTekst;
    //public Prefab testpref;

    void Awake()
    {
        myPath = Application.dataPath + "/XML/cards.xml";
        //Debug.Log(myPath);
        xmlDoc.Load(myPath);
 


        CardTekst = Card.GetComponent<Text>();
    }
    // Use this for initialization
    void Start ()
    {        
        //ca
        XmlNodeList cardNames = xmlDoc.GetElementsByTagName("name");
        int totalCards = cardNames.Count;

        //Debug.Log(totalCards);

        XmlNodeList cardDescription = xmlDoc.GetElementsByTagName("description");

        for (int cardID = 0; cardID < totalCards; cardID++)
        {
            //CreateCard(cardNames[cardID].InnerXml, cardID);
           
            /*
            Card.transform.SetParent(myHand.transform);
            Card.name = cardNames[cardID].InnerXml;
            Card.GetComponent<Text>().text = cardNames[cardID].InnerXml;
            Card.SetActive(true);
            */
            //cardName = cardNames[cardID].InnerXml;
            // Debug.Log(cardNames[cardID] + " " + cardNames[cardID].InnerXml);
            //Debug.Log(cardNames[cardID].InnerXml);


            GameObject newCard = Instantiate(Card) as GameObject;
            newCard.transform.SetParent(myHand.transform);
            newCard.name = cardNames[cardID].InnerXml;


            newCard.transform.GetChild(1).GetComponent<Text>().text = cardNames[cardID].InnerXml;
            newCard.transform.GetChild(2).GetComponent<Text>().text = cardDescription[cardID].InnerXml;
            //cardName = newCard.transform.GetChild(1);
            //newCard.
            //cardName.GetComponent<Text>().text = cardNames[cardID].InnerXml;
            //newCard.GetComponentInChildren<Text>().text = cardName;

            //newCard.GetComponentInChildren<Text>().text = cardNames[cardID].InnerXml;
            //newCard.GetComponent<Text>().text = cardNames[cardID].InnerXml;


            newCard.SetActive(true);
            
            newCard.transform.localScale = new Vector3(1,1,1);
          
            newCard.transform.localPosition = new Vector3(0, 0, 0);
           
        }
    }
}
