using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guncon : MonoBehaviour {

    //方向灵敏度


    private float sensitivityY = 6f;



    //上下的最大视角

    private float minY = -60f;

    private float maxY = 60f;

    float rotationY = 0f;




    // Use this for initialization

    void Start()
    {

    }



    // Update is called once per frame

    void Update()
    {
        //获取鼠标左右旋转的角度

        //获取鼠标上下旋转的角度

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;



        //角度限制，如果rorationY小于min返回min，大于max返回max

        rotationY = Clam(rotationY, minY, maxY);



        //设置摄像机的角度

        transform.localEulerAngles = new Vector3(-rotationY, 0, 0);

    }



    public float Clam(float value, float min, float max)

    {

        if (value < min) { return min; }

        if (value > max) { return max; }

        return value;

    }
}
