using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class Server_And_Client : MonoBehaviour
{
    public bool isAtStartUp = true;
    NetworkClient myClient;
    public string serverIP;
    public InputField addressInput;
    public server myServer;
       

	// Update is called once per frame
	void Update ()
    {
        
        //if (addressInput.IsActive())
        //serverIP = addressInput.text;



        //While we are still starting up the client or server.
        if (isAtStartUp)
        {

        }
        else
        {
            myServer.CmdLoadLevel("MarkInGame");
            DestroyObject(addressInput);
        }
	}
    
    
    
    public void onClickedCreateGame()
    {
        createServer();
        createLocalClient();
    }
    public void onClickedJoinGame()
    {
        createClient(serverIP);
    }
 

    public void createServer()
    {
        NetworkServer.Listen(7777);
        isAtStartUp = false;
    }

    public void createLocalClient()
    {
        
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        isAtStartUp = false;
    }

    public void createClient(string ipaddress)
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect(ipaddress, 7777);
        isAtStartUp = false;
    }

    

    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }

    public void OnInvalidIP(NetworkMessage netMsg)
    {
        Debug.Log("Failed to connect " + serverIP);
    }
}
