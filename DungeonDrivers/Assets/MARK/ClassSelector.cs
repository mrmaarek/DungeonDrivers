using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClassSelector : MonoBehaviour
{
    public Button readyToStart;


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
    }

    void Update()
    {
        // WHEN PLAYER CLASS IS NOT NONE SELECTED.
        if (playerClass != Classes.None_Selected)
        {
            // MAKE THE READY BUTTON ACTIVE.
            readyToStart.image.color = Color.white;
            readyToStart.enabled = true;
        }

        // When i don't press any of the class 'play' buttons i need to reset the playerClass to NoneSelected.

    }

    public void ClassSelect(int pClass)
    {
        this.playerClass = (Classes)pClass;
    }
}
