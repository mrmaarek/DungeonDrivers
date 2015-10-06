using System.Xml;
using System.Xml.Serialization;

using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour 
{
	[XmlAttribute("Cardname")]
	public string cardName;

}
