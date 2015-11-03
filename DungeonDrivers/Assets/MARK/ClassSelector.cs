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


    public enum Classes
    {
        None_Selected,
        Sandmage,
        Warrior
    }

    public Classes playerClass;

    void Start()
    {
        this.playerClass = Classes.None_Selected;
        readyToStart.enabled = false;
        readyToStart.image.color = Color.red;
        gridWithPlayer.SetActive(false);

    }

    void Update()
    {
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
            chosenCharacter.text = "Playing as Sandmage";
        }
        else if (playerClass == Classes.Warrior)
        {
            chosenCharacter.text = "Playing as Warrior";
        }

        // When i don't press any of the class 'play' buttons i need to reset the playerClass to NoneSelected.

    }

    public void ClassSelect(int pClass)
    {
        this.playerClass = (Classes)pClass;
    }
}
