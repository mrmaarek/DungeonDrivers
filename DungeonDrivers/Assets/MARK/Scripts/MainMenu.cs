using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Animator Anim_AreYouSure;
    public Text selectedClassInText;

    // Referentie naar Player Script.
    [SerializeField]
    private Player myPlayer;

    //public Player.Classes playerClass;
    public bool playerConfirmation;
	
	// Update is called once per frame
	void Update ()
    {
        if (myPlayer.playerClass != Player.Classes.NoneSelected)
        {
            selectedClassInText.GetComponent<Text>().text = myPlayer.playerClass.ToString();
            Anim_AreYouSure.SetBool("ClassSelected", true);
        }
    }

    public void AreYouSure(bool AmISure)
    {
        playerConfirmation = AmISure;
        if (playerConfirmation)
        {
            Application.LoadLevel(1);
        }
        else
        {
            Anim_AreYouSure.SetBool("ClassSelected", false);
            myPlayer.playerClass = Player.Classes.NoneSelected;
        }
    }

    public void ClassSelect(int pClass)
    {
        myPlayer.playerClass = (Player.Classes)pClass;
    }
}
