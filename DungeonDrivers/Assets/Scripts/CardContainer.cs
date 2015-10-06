using System.IO;

using UnityEngine;

//using System;
//using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;


[XmlRoot("CardCollection")]
public class CardContainer 
{
	[XmlArray("Cards")]
	[XmlArrayItem("Card")]
	public List<Card> cards = new List<Card>();

	public static CardContainer Load(string path)
	{
		TextAsset myXml = Resources.Load<TextAsset>(path);

		XmlSerializer mySerialzer = new XmlSerializer(typeof(CardContainer));


		StringReader reader = new StringReader(myXml.text);

		CardContainer cards = mySerialzer.Deserialize(reader) as CardContainer;

        reader.Close();

		return cards;
	}
}