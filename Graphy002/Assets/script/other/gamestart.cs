using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gamestart : MonoBehaviour {
    private float timer = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(timer>=4)
        SceneManager.LoadScene("lobby");
        timer += Time.deltaTime;
    }
}
