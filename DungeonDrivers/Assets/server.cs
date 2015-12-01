using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class server : NetworkBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }


    [Command]
    public void CmdLoadLevel(string sceneName)
    {
        NetworkManager.singleton.ServerChangeScene(sceneName);
    }
}
