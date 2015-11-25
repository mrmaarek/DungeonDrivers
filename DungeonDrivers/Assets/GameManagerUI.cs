using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManagerUI : MonoBehaviour
{
    public List<Player_Sync_Variables> players = new List<Player_Sync_Variables>();

    //public Prefab  

    // Use this for initialization
    void Start ()
    {
        // At the start, the players current health is based on their maxhealth.
        for (int i = 0; i < players.Count; i++)
        {
            players[i].currentHealth = players[i].maxHealth;
        }

       
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
