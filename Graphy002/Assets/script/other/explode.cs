using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour {
    public LayerMask mask;
    // Use this for initialization
    void Awake () {
    }
	// Update is called once per frame
	void Update () {
        Collider [] cs = Physics.OverlapSphere(transform.position,1,mask);
        Rigidbody rig = GetComponent<Rigidbody>();
        if (cs.Length != 0)
        {
            foreach (Collider cscell in cs)
            {
                Debug.Log("NO");
                cscell.SendMessage("rigid");
            }
            rig.AddExplosionForce(50, transform.position,1);
        }
        Destroy(gameObject, 1);
        Debug.Log("NO");
    }

}
