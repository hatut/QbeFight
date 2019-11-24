using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brick : MonoBehaviour
{
    public bool rig;
    // Use this for initialization
    void Start ()
    {
        rig = false;
    }
    private void rigid()
    {
     if (!rig)
        {
            Debug.Log("rig");
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().freezeRotation = true;
            rig = true;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
