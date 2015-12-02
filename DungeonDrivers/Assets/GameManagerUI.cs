using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameManagerUI : MonoBehaviour
{


    public Game_Manager_Script gmScript;
    //public Prefab  

    // Use this for initialization
    void Start ()
    {
        gmScript = GameObject.Find("Game Manager").GetComponent<Game_Manager_Script>();

        // At the start, the players current health is based on their maxhealth.
        

       
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
