using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Custom_Network_Manager_UI : MonoBehaviour 
{
	public NetworkManager Manager;


	public void StartServer()
	{
		Manager.StartHost();
	}

	public void JoinServer()
	{
		Manager.StartClient();
	}
}
