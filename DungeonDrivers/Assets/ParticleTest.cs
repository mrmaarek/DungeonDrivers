using UnityEngine;
using System.Collections;

public class ParticleTest : MonoBehaviour
{
    public GameObject particleObject;
    public ParticleSystem test;

    public PhaseWalker myPlayer;

	// Use this for initialization
	void Start ()
    {
        //particleObject = GameObject.Find("Arcane Bolt");
        //myPlayer = particleObject.transform.GetComponentInParent<PhaseWalker>();
        //particleObject.particleSystem.isPlaying
        //test = particleObject.GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        /*
        if (myPlayer.Game_Manager_Script.phase != Game_Manager_Script.Phase.IsMovingResovle)
        {
            // WAIT
        }
        else
        {
            Debug.Log(test.isPlaying);
            test.transform.parent = myPlayer.FreeSelectGridBlock.transform;
            //test.transform.
        }
        */
    }
}
