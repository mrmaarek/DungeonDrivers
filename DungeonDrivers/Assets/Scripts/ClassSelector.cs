using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClassSelector : MonoBehaviour
{
    public Button readyToStart;
    [SerializeField]
    private GameObject gridWithPlayer;
    [SerializeField]
    private Text chosenCharacter;

    [SerializeField]
    Game_Manager_Script gmScript;

    [SerializeField]
    private CanvasGroup notNeededUI;

    [SerializeField]
    private CanvasGroup developerPhases;


    public enum Classes
    {
        None_Selected,
        Sandmage,
        Warrior
    }

    public Classes playerClass;

    void Start()
    {
		/*
        // To make the class selection screen clearer, i'm having a canvas group which alpha will be set to 0.
        notNeededUI = GameObject.Find("inGameUIElements").GetComponent<CanvasGroup>();
        notNeededUI.alpha = 0;

        developerPhases = GameObject.Find("PhasesPanel").GetComponent<CanvasGroup>();
        developerPhases.alpha = 0;
        */
    }

    void Update()
    {
        Debug.Log(Camera.current);

        if (this.playerClass == Classes.None_Selected)
        {
            readyToStart.enabled = false;
            readyToStart.image.color = Color.red;
            //gridWithPlayer.SetActive(false);
        }
        // WHEN PLAYER CLASS IS NOT NONE SELECTED.
        if (playerClass != Classes.None_Selected)
        {
            // MAKE THE READY BUTTON ACTIVE.
            readyToStart.image.color = Color.white;
            readyToStart.enabled = true;

            //chosenCharacter = playerClass;
        }


        // Changing the text of the character you play.
        if (playerClass == Classes.None_Selected)
        {
            chosenCharacter.text = "You haven't chosen a character";
        }
        else if (playerClass == Classes.Sandmage)
        {
            chosenCharacter.text = "Playing as Kaylessa";
        }
        else if (playerClass == Classes.Warrior)
        {
            chosenCharacter.text = "Playing as Grimmet";
        }


        //Debug.Log(gmScript.playersReady);
        /*
        if (gmScript.playersReady < 4)
        {
            chosenCharacter.text = "Waiting for other players.";
        }
        */
    }

    public void ClassSelect(int pClass)
    {
        this.playerClass = (Classes)pClass;
    }

    
}
