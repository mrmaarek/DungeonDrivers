using UnityEngine;
using System.Collections;

public class Light_On_At_StartUp : MonoBehaviour 
{

	public Light Light;
	float timer = 0;
	
	void Update () 
	{
		timer += Time.deltaTime;

		if(timer > 0.2f)
		{
			Light.enabled = true;
			Destroy(this);
		}
	}
}
