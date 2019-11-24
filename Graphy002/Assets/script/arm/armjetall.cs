using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armjetall : MonoBehaviour
{
    

    private void des()
    {
        for (int i = 4; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {
    }
    
}
