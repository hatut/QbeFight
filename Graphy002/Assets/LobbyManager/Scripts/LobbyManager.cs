using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager {

    public static LobbyManager instan;


    private void Start()
    {
        if(instan == null)
        {
            instan = this;
        }
    }

}
