using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletsp : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider a)
    {
        if (a.tag == "Player")
            {
                a.SendMessage("bulldam");
            Destroy(gameObject);
            }
    }
    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward * 50 * Time.deltaTime);
	}
}
