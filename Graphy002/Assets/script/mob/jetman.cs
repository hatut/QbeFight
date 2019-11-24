using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class jetman : NetworkBehaviour {



    private float sensitivityY = 6f;
    private float sensitivityX = 6f;

    //上下的最大视角

    private float minY = -60f;

    private float maxY = 60f;

    float rotationY = 0f;
    float rotationX = 0f;

    [SyncVar]
    public string id;
    private float flycoldp;
    private float timer=0;
    public GameObject arm;
    public GameObject myarm;
    public GameObject ui;
    public float v;
    private bool bigb=false;
    public GameObject explodepr;
    private float bigtimer;
    private float biglast;
    public GameObject camer;
    private Vector3 ang;
    public float health;
    public float flytimer;
    private float flyspeed = 10;
    private float okspeed =6;
    private CharacterController cc;
    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        health = 1500;
        flytimer = 0;
        flycoldp = 1;
        transform.GetChild(1).name = GetComponent<NetworkIdentity>().netId.ToString() + "armjet";
    }
    // Update is called once per frame	

    void rotarm()
    {
        //获取鼠标左右旋转的角度

        //获取鼠标上下旋转的角度

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        //角度限制，如果rorationY小于min返回min，大于max返回max

        rotationY = Clam(rotationY, minY, maxY);

        //设置摄像机的角度

        myarm.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);

    }
    
    public float Clam(float value, float min, float max)

    {

        if (value < min) { return min; }

        if (value > max) { return max; }

        return value;

    }
    private void die()
    {
        if (health <= 0)
        {
            Debug.Log("die");
            NetworkServer.Destroy(gameObject);
        }
    }
    [Command]
    void Cmdexp()
    {
        Debug.Log("cmd");
        GameObject explode = Instantiate(explodepr, transform.position, transform.rotation) as GameObject;
        NetworkServer.Spawn(explode);

    }
    private void damage()
    {
        Debug.Log("damage");
        health -= 300;
    }
    private void bulldam()
    {
        Debug.Log("damage");
        health -= 150;
    }
    private void OnCollisionEnter(Collision a)
    {
        if (!isLocalPlayer) return;
        if (a.collider.tag == "brick")
        {
            if (v >= 23)
            if(timer>=1)
            {
                Debug.Log(v);
                Cmdexp();
                timer = 0;
                }
        }
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
                flycoldp = 1;
                biglast = 0;
                bigtimer = 0;
                bigb = false;
                myarm.SendMessage("des");
            }
        }
        if (bigtimer >= 60) ui.transform.Rotate(new Vector3(0, 0, 20));
        else if (bigtimer >= 30) ui.transform.Rotate(new Vector3(0, 0, 10));
        else if(bigtimer >= 10) ui.transform.Rotate(new Vector3(0, 0, 5));
        if (bigtimer<=60)bigtimer += Time.deltaTime;
        if (Input.GetKey("q")&&biglast==0)
        {

            if (bigtimer >= 60)
            {
                Debug.Log("big");
                bigb = true;
                flycoldp = 2;
                Cmdbig2(id);
            }
            else if (bigtimer >= 30)
            {
                flycoldp = 2;
                bigb = true;
                Cmdbig1(id);
            }
            else if (bigtimer >= 10)
            {
                flycoldp = 1.5f;
                bigb = true;
            }
        }
    }
    [Command]
    private void Cmdbig1(string aid)
    {
        myarm.transform.eulerAngles = new Vector3(0, 0, 0);
        Transform ar = GameObject.Find(aid).transform.GetChild(1).transform;
        GameObject arm1 = Instantiate(arm, ar.position, Quaternion.Euler(ar.eulerAngles.x, ar.eulerAngles.y + 180, ar.eulerAngles.z), ar) as GameObject;
        arm1.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm1.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm1.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        NetworkServer.Spawn(arm1);
    }
    [Command]
    private void Cmdbig2(string aid)
    {
        myarm.transform.eulerAngles = new Vector3(0, 0, 0);
        Transform ar = GameObject.Find(aid).transform.GetChild(1).transform;
        GameObject arm1 = Instantiate(arm, ar.position, Quaternion.Euler(ar.eulerAngles.x, ar.eulerAngles.y + 90, ar.eulerAngles.z), ar)as GameObject;
        GameObject arm2 = Instantiate(arm, ar.position, Quaternion.Euler(ar.eulerAngles.x, ar.eulerAngles.y + 180, ar.eulerAngles.z), ar)as GameObject;
        GameObject arm3 = Instantiate(arm, ar.position, Quaternion.Euler(ar.eulerAngles.x, ar.eulerAngles.y + 270, ar.eulerAngles.z), ar)as GameObject;
        arm1.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm2.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm3.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm1.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm2.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm3.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm1.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm2.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        arm3.transform.GetChild(0).GetComponent<arm_jet>().id = aid;
        NetworkServer.Spawn(arm1); NetworkServer.Spawn(arm2); NetworkServer.Spawn(arm3);
    }
    private void fly()
    {
        if (flytimer >= 2.5f)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (flyspeed<=25)flyspeed += Time.deltaTime*10;
                ang = camer.transform.forward;
            }else 
            if (ang!=new Vector3(0,0,0))
            {
                gameObject.GetComponent<Rigidbody>().AddForce(ang * flyspeed*50, ForceMode.Impulse);
                flytimer = 0;
                flyspeed = 10;
                ang = new Vector3(0, 0, 0);

            }
        }
        else { flytimer += Time.deltaTime*flycoldp;  }
      
    }
    private void move()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(transform.forward * okspeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(transform.forward * -okspeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(transform.right * -okspeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(transform.right * okspeed * Time.deltaTime, Space.World);
        }
            if (Input.GetKey(KeyCode.Space))
            if(Physics.Raycast(transform.position,-transform.up,0.8f))
            {
                gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,1,0)*50, ForceMode.Impulse);
            }
    }
    private void rot()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        transform.localEulerAngles = new Vector3(0, rotationX, 0);
    }
    public override void OnStartLocalPlayer()
    {
        camer.SetActive(true);
        id = GetComponent<NetworkIdentity>().netId.ToString();
    }
    private void FixedUpdate()
        
    {
        v = GetComponent<Rigidbody>().velocity.magnitude;
        if (timer <= 1) timer += Time.deltaTime;
        if (!isLocalPlayer)
        {
            return;
        }
        rotarm();
        die();
        rot();
        big();
        fly();
        move();
    }


}
