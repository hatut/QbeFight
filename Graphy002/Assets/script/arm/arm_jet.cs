using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class arm_jet : NetworkBehaviour
{
    

    private float timer = 0;
    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
        if (timer <= 0.5) timer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider a)
    {
        if (a.tag == "Player")
                if (timer>=0.5)
        {
            a.SendMessage("damage");
            timer = 0;
        }
    }

}
