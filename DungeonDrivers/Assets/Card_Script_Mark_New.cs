using UnityEngine;
using System.Collections;

public class Card_Script_Mark_New : MonoBehaviour
{

    [SerializeField]
    GameObject cardToPlay;
    [SerializeField]
    GameObject myPlayerHand;
    [SerializeField]
    GameObject myDiscardPile;

    void Start()
    {
        myDiscardPile = GameObject.Find("MyDiscardPile");
        myPlayerHand = GameObject.Find("MyHand");
        cardToPlay = GameObject.Find("MyCardToPlay");
    }

    // Update is called once per frame
    void Update ()
    {
        if (GetComponent<Card_Script>().cardIsInPlay)
        {
            setCardToPlay();
        }
        else
        {
            //setBackToHand();
        }
	}

    void setCardToPlay()
    {
        transform.SetParent(cardToPlay.transform);

        if (GetComponent<Card_Script>().targeting == Card_Script.Targeting.FreeSelect)
        {
            Debug.Log("I'm a free select card");


        }
    }
    void setBackToHand()
    {
        transform.SetParent(myPlayerHand.transform);
    }
}
