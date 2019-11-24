using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Networking;
public class playernetid : NetworkBehaviour {
    public string id;
    public Text t;
    public override void OnStartLocalPlayer()
    {
        id = GetComponent<NetworkIdentity>().netId.ToString();
        transform.name = id;
    }
    // Use this for initialization
    void Start () {
        id = GetComponent<NetworkIdentity>().netId.ToString();
        transform.name = id;
        t.text = id;
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
