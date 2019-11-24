using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCtrl : NetworkBehaviour {


    void Start ()
    {

	}

	void Update ()
    {
        if (!isLocalPlayer)
            return;

        Move();


    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal") * 2f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 5f;

        Vector3 v = new Vector3(0, 0, z);

        v = transform.TransformDirection(v);

        if (GetComponent<CharacterController>().isGrounded)
        {
            v.y = 0;
        }
        v.y -= 9.8f * Time.deltaTime;

        CmdMove(x, v);
    }
    [Command]
    void CmdMove(float x, Vector3 v)
    {
        transform.Rotate(0, x, 0);

        GetComponent<CharacterController>().Move(v);
    }


    //当自己的角色被创建的时候
    public override void OnStartLocalPlayer()
    {
        //关闭所有UI显示
        LobbyUI.instan.AllUIFalse();
        //标记自己为蓝色
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
