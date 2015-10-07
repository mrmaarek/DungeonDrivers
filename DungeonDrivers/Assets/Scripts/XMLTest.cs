using UnityEngine;
using UnityEngine.UI;
using System;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class XMLTest : MonoBehaviour
{
    XmlDocument xmlDoc = new XmlDocument();
    string myPath;

    public GameObject CardTest;
    public Text CardTekst;
    //public Prefab testpref;

    void Awake()
    {
        myPath = Application.dataPath + "/XML/cards.xml";
        Debug.Log(myPath);
        xmlDoc.Load(myPath);



        CardTekst = CardTest.GetComponent<Text>();
    }
    // Use this for initialization
    void Start ()
    {
        CardTekst.text = "MWAH";
        //Debug.Log(testText);
        //
        Debug.Log(xmlDoc);

        XmlNodeList cardName = xmlDoc.GetElementsByTagName("name");
        XmlNodeList cardDescription = xmlDoc.GetElementsByTagName("description");

        for (int i = 0; i < cardName.Count; i++)
        {
            Instantiate(CardTest);


            //testText.text = cardName[i].InnerXml;

            /*
            Debug.Log(cardName[i].InnerXml);
            Debug.Log(cardDescription[i].InnerXml);
            */
        }

        //xmlDoc.Close();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
