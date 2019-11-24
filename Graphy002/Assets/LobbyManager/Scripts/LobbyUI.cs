using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {

    //单例
    public static LobbyUI instan;
    private void Awake()
    {
        //如果instan为空 才赋值
        if(instan == null)
        {
            instan = this;
        }
    }
    //Player父对象
    public Transform playerList;

    //进入大厅后的UI
    public GameObject joinDone;

    //提示框UI
    public GameObject infoUI;

    //设置提示框UI
    public void SetInfoUI(string content, string button ,UnityAction action)
    {
        infoUI.SetActive(true);
        infoUI.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = content;
        infoUI.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = button;
        //提示框Button事件
        infoUI.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        infoUI.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(action);
    }


    //身份信息Text
    public Text statusInfo;

    //服务器IP信息Text
    public Text ipInfo;

    public void SetStatusIP(string status, string ip)
    {
        statusInfo.text = status;
        ipInfo.text = ip;
    }

    //返回Button
    public Button back;

    //返回Button
    public Button backQuit;

    //创建主机Button
    public Button createHost;

    //输入框UI
    public InputField input;

    //加入Button
    public Button joinButton;

    //用于接收创建的客户端
    public NetworkClient client;

    public Button play;

    //服务器列表
    public Transform serverList;

    //服务器列表
    public List<GameObject> servers = new List<GameObject>();

    //扫描服务器Button
    public Button saoMiaoBtn;
    //服务器预设
    public GameObject prefabServer;


    private void Start()
    {
        //创建Button事件
        createHost.onClick.AddListener(CreateHost);
        //返回Button事件
        back.onClick.AddListener(Back);
        back.onClick.AddListener(StopUDP);

        //加入Button事件
        joinButton.onClick.AddListener(JoinClient);


        input.onEndEdit.AddListener(InputListener);

        //backQuit.onClick.AddListener(Quit);

        saoMiaoBtn.onClick.AddListener(RecUDP);

    }
    //发送广播
    void SendUDP()
    {
        //先停止接收
        StopUDP();

        GetComponent<LobbyDiscover>().Initialize();

        //发送
        GetComponent<LobbyDiscover>().StartAsServer();
        print("开始发送广播！");
    }

    //接收广播
    void RecUDP()
    {
        if (!GetComponent<LobbyDiscover>().isClient)
        {
            for (int i = 0; i < serverList.childCount; i++)
            {
                Destroy(serverList.GetChild(i).gameObject);
            }
            servers.Clear();

            GetComponent<LobbyDiscover>().Initialize();

            GetComponent<LobbyDiscover>().StartAsClient();

            Invoke("IsServer", 1f);

            print("开始扫描服务器！");
        }
    }
    //停止广播
    void StopUDP()
    {
        if (GetComponent<LobbyDiscover>().isClient || GetComponent<LobbyDiscover>().isServer)
        {
            GetComponent<LobbyDiscover>().StopBroadcast();
        }
    }
    //找到服务器
    void IsServer()
    {
        if (GetComponent<LobbyDiscover>().running)
        {
            //服务器列表是否大于0
            if (GetComponent<LobbyDiscover>().broadcastsReceived.Count > 0)
            {
                List<string> ip = new List<string>();
                //遍历服务器列表
                foreach (var key in GetComponent<LobbyDiscover>().broadcastsReceived.Keys)
                {
                    NetworkBroadcastResult value;
                    if (GetComponent<LobbyDiscover>().broadcastsReceived.TryGetValue(key, out value))
                    {
                        //添加服务器预设到列表
                        servers.Add(Instantiate(prefabServer, serverList));
                        ip.Add(value.serverAddress);
                    }
                }
                for (int i = 0; i < servers.Count; i++)
                {
                    servers[i].GetComponent<ServerIP>().id = i;
                    servers[i].GetComponent<ServerIP>().ip = ip[i];
                    servers[i].transform.GetChild(0).GetComponent<Text>().text = "服务器" + i + "(" + ip[i] + ")";
                }
            }
            else
            {
                print("没有扫描到服务器！");
            }
        }
        
        print("扫描完毕！扫描到" + GetComponent<LobbyDiscover>().broadcastsReceived.Count + "个服务器！");
        StopUDP();
    }

    public void JoinServer(string ip)
    {
        //获取地址
        GetComponent<LobbyManager>().networkAddress = ip;

        //开始连接
        client = GetComponent<LobbyManager>().StartClient();

        //实时判断是否连接上服务器
        InvokeRepeating("PanDuan", 0, 0.05f);

        //设置超时时间
        Invoke("TimeChao", 10f);

        //设置连接中UI
        SetInfoUI("连接中...", "取消", Back);
    }

    //void Quit()
    //{
    //    ViewManager.instan.main001.SetActive(false);
    //    ViewManager.instan.main002.SetActive(true);
    //    Destroy(gameObject);
    //}

    //隐藏所有UI
    public void AllUIFalse()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //输入框事件
    void InputListener(string n)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            JoinClient();
        }
    }


    //创建主机方法
    void CreateHost()
    {
        //创建服务器
        client = GetComponent<LobbyManager>().StartHost();

        if (client == null)
        {
            //设置连接中UI
            SetInfoUI("创建失败", "确定", Back);
        }
        else
        {
            //创建成功 设置UI显示
            SetUIActive(true);
            SendUDP();
        }
    }


    //加入方法
    void JoinClient()
    {
        //获取地址
        GetComponent<LobbyManager>().networkAddress = input.text;

        //开始连接
        client = GetComponent<LobbyManager>().StartClient();

        //实时判断是否连接上服务器
        InvokeRepeating("PanDuan", 0, 0.05f);

        //设置超时时间
        Invoke("TimeChao", 10f);

        //设置连接中UI
        SetInfoUI("连接中...", "取消" , Back);
    }

    //返回Button 断开连接
    void Back()
    {
        //停止服务器
        GetComponent<LobbyManager>().StopHost();
        GetComponent<LobbyManager>().StopClient();

        //UI界面关闭
        SetUIActive(false);
        //提示框关闭
        infoUI.SetActive(false);


        //关闭定时器
        CancelInvoke("PanDuan");
        CancelInvoke("TimeChao");
    }



    //判断是否连接上服务器
    void PanDuan()
    {
        //是否连接到服务器
        if (client.isConnected)
        {
            //已连接上 打开UI显示
            SetUIActive(true);

            //提示框关闭
            infoUI.SetActive(false);

            //关闭两个定时器
            CancelInvoke("PanDuan");
            CancelInvoke("TimeChao");
        }
    }
    //超时方法
    void TimeChao()
    {
        //先把连接停止
        GetComponent<LobbyManager>().StopClient();

        //显示连接超时
        SetInfoUI("连接超时！", "确定", Back);

        //关闭定时器
        CancelInvoke("PanDuan");
    }

    //设置连接成功需要显示的UI
    void SetUIActive(bool i)
    {
        joinDone.SetActive(i);
        back.gameObject.SetActive(i);
        backQuit.gameObject.SetActive(!i);
    }

}
