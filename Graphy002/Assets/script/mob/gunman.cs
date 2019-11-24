using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class gunman : NetworkBehaviour
{
    private float sensitivityY = 6f;



    //上下的最大视角

    private float minY = -60f;

    private float maxY = 60f;

    float rotationY = 0f;

    public GameObject bulletpr;
    public bool isr;
    private float timer = 0;
    private float reloadtimer = 0;
    private float ltimer = 0;
    public float sensitivityX = 6f;
    float rotationX = 0f;
    public GameObject ui;
    public float v;
    private bool bigb=false;
    public GameObject explodepr;
    private float bigtimer;
    private float biglast;
    public GameObject shotpoint;
    public GameObject camer;
    private Vector3 ang;
    public float health;
    public float rtimer;
    public int bulletnum;
    private float okspeed =6;
    public GameObject gun;
    private CharacterController cc;
    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        health = 1000;
        bulletnum = 30;
        isr = false;
    }
    // Update is called once per frame	

    void gunrot()
    {
        //获取鼠标左右旋转的角度

        //获取鼠标上下旋转的角度

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;



        //角度限制，如果rorationY小于min返回min，大于max返回max

        rotationY = Clam(rotationY, minY, maxY);



        //设置摄像机的角度

        gun.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);

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
    [Command]
    void Cmdshot()
    {
        GameObject bullet = Instantiate(bulletpr, shotpoint.transform.position, shotpoint.transform.rotation) as GameObject;
        NetworkServer.Spawn(bullet);
    }

    private void bulldam()
    {
        Debug.Log("damage");
        health -= 150;
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
            }else
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
        else if(bigtimer >= 10) ui.transform.Rotate(new Vector3(0, 0, 5));
        if (bigtimer<=60)bigtimer += Time.deltaTime;
        if (Input.GetKey("q")&&biglast==0)
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
    }
    private void FixedUpdate()
        
    {
        v = GetComponent<Rigidbody>().velocity.magnitude;
        if (timer <= 1) timer += Time.deltaTime;
        if (!isLocalPlayer)
        {
            return;
        }
        gunrot();
        die();
        rot();
        big();
        move();
        attack();
    }


}
