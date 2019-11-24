using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
public class gunwalk : NetworkBehaviour
{
    public int id;
    private float sensitivityX = 6f;
    private float rotationX = 0f;
    public float health;
    private float okspeed = 6;
    private float timer = 0;
    public GameObject explodepr;
    public float v;
    // Use this for initialization
    void Start () {
        health = 1000;
    }

    private void die()
    {
        if (health <= 0)
        {
            Debug.Log("die");
            NetworkServer.Destroy(gameObject);
        }
    }
    private void damage()
    {
        health -= 300;
    }

    private void bulldam()
    {
        health -= 150;
    }
    [Command]
    void Cmdexp()
    {
        Debug.Log("cmd");
        GameObject explode = Instantiate(explodepr, transform.position, transform.rotation) as GameObject;
        NetworkServer.Spawn(explode);
    }
    private void OnCollisionEnter(Collision a)
    {
        if (!isLocalPlayer) return;
        if (a.collider.tag == "brick")
        {
            if (v >= 23)
                if (timer >= 1) ;
                {
                    Debug.Log(v);
                    Cmdexp();
                    timer = 0;
                }
        }
    }
    private void move()
    {

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        transform.localEulerAngles = new Vector3(0, rotationX, 0);
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
            if (Physics.Raycast(transform.position, -transform.up, 0.8f))
            {
                gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 1, 0) * 50, ForceMode.Impulse);
            }
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (id!= GetComponent<NetworkConnection>().connectionId) return;
        v = GetComponent<Rigidbody>().velocity.magnitude;
        if (timer <= 1) timer += Time.deltaTime;
        die();
        move();
    }
}
