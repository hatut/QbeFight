using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyDiscover : NetworkDiscovery {

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        //print(fromAddress);
        //foreach (var key in broadcastsReceived.Keys)
        //{
        //    NetworkBroadcastResult value;
        //    if(broadcastsReceived.TryGetValue(key,out value))
        //    {
        //        print(value.serverAddress);
        //    }
        //}
    }
}
