using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {


    //是否已准备
    [SyncVar]
    public bool isReady = false;

	void Start ()
    {
        //设置自己的父对象为PlayerList
        transform.SetParent(LobbyUI.instan.playerList);
        transform.localScale = Vector3.one;

        //准备按钮事件绑定
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Ready);

        if (isLocalPlayer)
        {
            transform.GetChild(0).GetComponent<Text>().text = "我";

            //关闭准备字样Text
            transform.GetChild(2).gameObject.SetActive(false);

            //准备按钮显示
            transform.GetChild(1).gameObject.SetActive(true);
            
        }
        else
        {
            transform.GetChild(0).GetComponent<Text>().text = "敌人";

            //打开准备字样Text
            transform.GetChild(2).gameObject.SetActive(true);

            transform.GetChild(1).gameObject.SetActive(false);

            InitReady();
        }
    }


    //只会在自己的角色被创建的时候才会调用
    public override void OnStartLocalPlayer()
    {
        //判断自己是否是服务器
        if (isServer)
        {
            //当前自己的身份状态、IP
            LobbyUI.instan.SetStatusIP("主机", Network.player.ipAddress);
            //Play按钮可控
            LobbyUI.instan.play.gameObject.SetActive(true);
            //绑定游戏开始方法
            LobbyUI.instan.play.onClick.AddListener(Play);
        }
        else
        {
            //当前自己的身份状态、服务器的IP
            LobbyUI.instan.SetStatusIP("客户端", LobbyUI.instan.client.serverIp);
            //Play按钮不可控
            LobbyUI.instan.play.gameObject.SetActive(false);
        }
        //标识自己
        GetComponent<Image>().color = Color.blue;
    }

    //准备
    void Ready()
    {
        CmdReady();
    }
    
    //在服务器执行
    [Command]
    void CmdReady()
    {
        //通知客户端执行
        RpcReady(!readyToBegin);
    }

    //在客户端执行
    [ClientRpc]
    void RpcReady(bool r)
    {
        //准备状态切换
        readyToBegin = r;

        //同步变量，用于记录准备状态，因为readyToBegin在初始被创建时值并不会同步为服务器的值
        isReady = r;

        //更新UI显示
        InitReady();
    }

    //初始化准备UI
    void InitReady()
    {
        if (isReady)
        {
            //Button字体
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "取消";
            transform.GetChild(2).GetComponent<Text>().text = "已准备";
            transform.GetChild(2).GetComponent<Text>().color = Color.black;
        }
        else
        {
            //Button字体
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "准备";
            transform.GetChild(2).GetComponent<Text>().text = "未准备";
            transform.GetChild(2).GetComponent<Text>().color = Color.white;
        }
    }



    void Play()
    {
        int k = 0;
        for (int i = 0; i < LobbyUI.instan.playerList.childCount; i++)
        {
            //判断玩家是否准备好
            if (LobbyUI.instan.playerList.GetChild(i).GetComponent<LobbyPlayer>().readyToBegin)
            {
                k++;
            }
        }
        //如果准备好的玩家数等于服务器的总玩家数
        if(k == LobbyManager.instan.numPlayers)
        {

            //游戏开始 加载场景
            // LobbyManager.instan.ServerChangeScene(LobbyManager.instan.playScene);
             SendReadyToBeginMessage();
        }
        else
        {
            //打开提示框
            LobbyUI.instan.SetInfoUI("有玩家未准备！", "确定", Guanbi);
        }
    }
    //关闭UI显示
    void Guanbi()
    {
        LobbyUI.instan.infoUI.SetActive(false);
    }

}
