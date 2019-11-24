using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guncontroller : MonoBehaviour {

    //方向灵敏度
    private float sensitivityX = 6f;
    private float rotationX = 0f;
    private float sensitivityY = 6f;
    
    //上下的最大视角

    private float minY = -70f;

    private float maxY = 70f;

    private float rotationY = 0f;

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

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        transform.localEulerAngles = new Vector3(0, rotationX, 0);

    }

    private float Clam(float value, float min, float max)

    {
        if (value < min) { return min; }

        if (value > max) { return max; }

        return value;
    }
}
