using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class gunmanc : NetworkBehaviour
{
    //方向灵敏度
    private float sensitivityX = 6f;
    private float rotationX = 0f;
    private float sensitivityY = 6f;

    //上下的最大视角

    private float minY = -70f;

    private float maxY = 70f;

    private float rotationY = 0f;
    public int myid;
    public GameObject body;
    public GameObject bulletpr;
    public bool isr;
    private float reloadtimer = 0;
    private float ltimer = 0;
    public GameObject ui;
    private bool bigb = false;
    private float bigtimer;
    private float biglast;
    public GameObject shotpoint;
    public GameObject camer;
    private Vector3 ang;
    public float health;
    public float rtimer;
    public int bulletnum;
    // Use this for initialization
    void Awake()
    {

        bulletnum = 30;
        isr = false;
        Instantiate(body, transform.position, transform.rotation);
    }
    // Update is called once per frame	
    [Command]
    void Cmdspawn()
    {
        GameObject gunmanbody = Instantiate(body, transform.position, transform.rotation);
        NetworkServer.Spawn(gunmanbody);
        gunmanbody.GetComponent<gunwalk>().id = myid;
    } 
    private float Clam(float value, float min, float max)

    {
        if (value < min) { return min; }

        if (value > max) { return max; }

        return value;
    }
    [Command]
    void Cmdshot()
    {
        GameObject bullet = Instantiate(bulletpr, shotpoint.transform.position, shotpoint.transform.rotation) as GameObject;
        NetworkServer.Spawn(bullet);
    }
    
    private void attack()
    {
        if (ltimer >= 0.07f)
        {
            if (!isr)
            {
                if (Input.GetKey(KeyCode.R))
                    isr = true;
                if (bulletnum > 0)
                {

                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        Debug.Log(bulletnum);
                        bulletnum--;
                        Cmdshot();
                        ltimer = 0;
                    }
                }
                else isr = true;
            }
            else
            {
                reload();
            }
        }
        else ltimer += Time.deltaTime;
    }
    private void reload()
    {
        if (reloadtimer >= 0.07f) { bulletnum++; reloadtimer = 0; } else reloadtimer += Time.deltaTime;
        if (bulletnum == 35) isr = false;
    }
    private void big()
    {
        if (bigb)
        {
            if (biglast <= 10)
            {
                biglast += Time.deltaTime;
            }
            else
            {
                biglast = 0;
                bigtimer = 0;
                bigb = false;
                for (int i = 3; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
        if (bigtimer >= 60) ui.transform.Rotate(new Vector3(0, 0, 20));
        else if (bigtimer >= 30) ui.transform.Rotate(new Vector3(0, 0, 10));
        else if (bigtimer >= 10) ui.transform.Rotate(new Vector3(0, 0, 5));
        if (bigtimer <= 60) bigtimer += Time.deltaTime;
        if (Input.GetKey("q") && biglast == 0)
        {

            if (bigtimer >= 60)
            {
                Debug.Log("big");
            }
            else if (bigtimer >= 30)
            {

            }
            else if (bigtimer >= 10)
            {

            }
        }
    }
    private void rot()
    {

        //获取鼠标左右旋转的角度

        //获取鼠标上下旋转的角度

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        //角度限制，如果rorationY小于min返回min，大于max返回max

        rotationY = Clam(rotationY, minY, maxY);
        //设置摄像机的角度
        

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
    public override void OnStartLocalPlayer()
    {
        myid=GetComponent<NetworkConnection>().connectionId;
        camer.SetActive(true);
        Cursor.visible = false;
    }
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        transform.position = body.transform.position;
        rot();
        big();
        attack();
    }


}
