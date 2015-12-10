using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health_Bars_Script : MonoBehaviour 
{
	private Game_Manager_Script GM;
	public Text[] HealthTexts;

    private Player_Sync_Variables PSV;
    private Sprite playerPortrait;

    public Sprite kaylessaPortrait;
    public Sprite grimmetPortrait; 

    

	void Start () 
	{
		GM = GetComponentInParent<PhaseWalker>().Game_Manager_Script;
        PSV = GetComponentInParent<PhaseWalker>().Player_Sync_Variables;
        

		for(int i = 0; i < GM.players.Count; i++)
		{
			HealthTexts[i].transform.parent.gameObject.SetActive(true);


            // Check which class the player is.
            if (GM.players[i].playerClass == "Sandmage")
            {
                HealthTexts[i].GetComponentInParent<Image>().sprite = kaylessaPortrait;
            }
            else
            {
                HealthTexts[i].GetComponentInParent<Image>().sprite = grimmetPortrait;
            }
		}

		StartCoroutine("UpdateHealthBars");
	}

	IEnumerator UpdateHealthBars()
	{
		while(true)
		{
			UpdateHealthObjects();

			yield return new WaitForSeconds(0.25f);
		}
	}

	void UpdateHealthObjects()
	{
		for(int i = 0; i < GM.players.Count; i++)
		{
			HealthTexts[i].text = "" + GM.players[i].currentHealth;

		}
	}

    void Update()
    {
        if (AreThereMultiplePlayers(GM.players.Count))
        {
            // Yes, we are currently handling the end of a turn.        
            if (GM.phase == Game_Manager_Script.Phase.EndTurn)
            {
                checkIfPlayerDied();
            }
        }
    }

    /// <summary>
    /// This function checks if there are more then one player playing (in case of testing)
    /// </summary>
    /// <param name="numberOfPlayers">Parameter value to pass.</param>
    /// <returns>Returns an boolean based on the passed value.</returns>
    private bool AreThereMultiplePlayers(int numberOfPlayers)
    {
       /// multiplePlayers = 
        if (GM.players.Count > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// This function checks each players health. And debugs currently if there is a player with less then 1 HP.
    /// </summary>
    void checkIfPlayerDied()
    {
        // For each Player
        for (int i = 0; i < GM.players.Count; i++)
        {
            if (GM.players[i].currentHealth < 1)
            {
                Debug.Log("Psst, i found someone with < 1 HP.");

                //GM.players[i].currentPosition.z = GM.players[i].currentPosition.z - 50 * Time.deltaTime;
            }
            else
            {
                Debug.Log(GM.players[i].currentHealth);
            }
        }
    }  
}
