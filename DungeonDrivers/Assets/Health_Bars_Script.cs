using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health_Bars_Script : MonoBehaviour 
{
	private Game_Manager_Script GM;

	public Text[] HealthTexts;

	void Start () 
	{
		GM = GetComponentInParent<PhaseWalker>().Game_Manager_Script;

		for(int i = 0; i < GM.players.Count; i++)
		{
			HealthTexts[i].transform.parent.gameObject.SetActive(true);
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
}
