using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Custom_Network_Manager_UI : MonoBehaviour 
{
	public NetworkManager Manager;
	public InputField IpBar;


	public void StartServer()
	{
		Manager.StartHost();
	}

	public void JoinServer()
	{
		Manager.StartClient();
	}

	public void SetHostIp()
	{
		Manager.networkAddress = IpBar.text.ToString();
	}

    public void QuitGame()
    {
        Application.Quit();
    }
}
