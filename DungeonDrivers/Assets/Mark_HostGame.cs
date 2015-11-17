using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Mark_HostGame : MonoBehaviour
{
    NetworkClient myClient;
    bool isAtStartUp = true;

    // Use this for initialization
    void Update()
    {
        if (isAtStartUp)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SetupServer();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                SetupClient();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                SetupServer();
                SetupLocalClient();
            }
        }
    }

    public void SetupServer()
    {
        NetworkServer.Listen(7777);
        isAtStartUp = false;

        SetupLocalClient();
    }

    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect("141.252.227.63", 7777);
        isAtStartUp = false;


    }
    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        isAtStartUp = false;
    }
    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
}
