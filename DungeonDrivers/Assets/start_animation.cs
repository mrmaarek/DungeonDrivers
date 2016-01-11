using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class start_animation : MonoBehaviour 
{
     Canvas myCanvas;
            
    
    public Animator cameraAnim;

   
    void Start()
    {
        myCanvas = GameObject.FindGameObjectWithTag("myCanvasje").GetComponent<Canvas>();
        myCanvas.enabled = false;
        cameraAnim = GameObject.Find("CharacterSelectionCamera").GetComponent<Animator>();
        //charCanvas = 
        cameraAnim.SetBool("isFinished", true);
    }

	// Update is called once per frame
	void Update () 
    {
        if (cameraAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !cameraAnim.IsInTransition(0))
        {
            Debug.Log("Finished");
            myCanvas.enabled = true;
            //myCanvas = GameObject.FindWith("Canvascharacterselection").GetComponent<Canvas>();
//            myCanvas.enabled = true;
        }
	}
}
