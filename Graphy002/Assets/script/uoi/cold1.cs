using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class cold1 :MonoBehaviour {
    public GameObject go;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Slider>().value = go.GetComponent<jetman>().flytimer;
	}
}
