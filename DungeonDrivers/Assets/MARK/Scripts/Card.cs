using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    public void flipCard()
    {
        this.transform.Rotate(new Vector3(0, 180, 0));
    }
}
