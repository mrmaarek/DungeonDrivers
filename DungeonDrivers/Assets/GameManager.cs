using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Grid_Spawner_Script Grid_Spawner_Script;

	// Use this for initialization
	void Start ()
    {
        Grid_Spawner_Script.SetupGrid();
	}
	
	
}
