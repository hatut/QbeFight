
using UnityEngine;

using UnityEngine.Networking;

using UnityEngine.SceneManagement;

using UnityEngine.UI;





public class NetworkUI : MonoBehaviour

{



    #region Variables

    public Button hostButton;

    public Button clientButton;

    public InputField ipInput;

    #endregion



    void Awake()

    {

        CheckRegisted();

    }



    void Start()

    {

        print("start");

        OfflineSet();

    }



    /// <summary>

    /// 绑定当前按钮事件

    /// </summary>

    public void OfflineSet()

    {

        hostButton.onClick.AddListener(StartHost);

        clientButton.onClick.AddListener(StartClient);

        print("OfflineSet");

    }

    /// <summary>

    /// 检查是否有空引用

    /// </summary>

    public void CheckRegisted()

    {

        if (hostButton == null)

        {

            hostButton = GameObject.Find("HostButton").GetComponent<Button>();

        }

        if (ipInput == null)

        {

            ipInput = GameObject.Find("IPInput").GetComponent<InputField>();

        }

        if (clientButton == null)

        {

            clientButton = GameObject.Find("ClientButton").GetComponent<Button>();

        }

    }

    /// <summary>

    /// HostButton绑定的事件

    /// </summary>

    public void StartHost()

    {

        print("StartHost");

        NetworkManager.singleton.StartHost();
        NetworkDiscovery.print(ipInput.text);

    }



    /// <summary>

    /// ClientButton绑定的事件

    /// </summary>

    public void StartClient()

    {

        //先获得当前主机的IP，我在InputField里放了个默认值127.0.0.1，这是一个特殊IP，直接代表此计算机本身

        NetworkManager.singleton.networkAddress = ipInput.text;

        NetworkManager.singleton.StartClient();

    }





}
