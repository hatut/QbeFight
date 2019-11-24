using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour {

    public static GameCanvas instan;
	void Awake () {
        instan = this;
    }
}
