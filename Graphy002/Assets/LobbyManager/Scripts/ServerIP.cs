using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerIP : MonoBehaviour {

    public int id;

    public string ip;

    public void JoinClient()
    {
        LobbyUI.instan.JoinServer(ip);
    }

}
