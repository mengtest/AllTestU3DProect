using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class Server : MonoBehaviour {
	public GameObject player;
	NetworkManager netWorkManage;
	public Text msg;
	string ip = "localhost";
	int maxConnections = 10;
	int port = 2333;

	void Update () {

	}
	void Start () {
		netWorkManage = gameObject.GetComponent<NetworkManager> ();
		//netWorkManage.playerPrefab = player;
	}

	/// <summary>
	/// 当服务器启动成功
	/// </summary>
	void OnServerInitialized () {
		Log ("创建成功");
	}

	void Log (object _o) {
		if (msg != null) {
			msg.text = _o.ToString ();
		}
		Debug.Log (_o);
	}
	string tempPort = "";
	void OnGUI () {
		GUILayout.BeginVertical ();
		if (!netWorkManage.isNetworkActive) {
			netWorkManage.networkAddress = GUILayout.TextField (netWorkManage.networkAddress);
			tempPort = netWorkManage.networkPort.ToString ();

			tempPort = GUILayout.TextField (tempPort);
			int prot;
			if (int.TryParse (tempPort, out prot)) {
				netWorkManage.networkPort = prot;
			}
			string portStr = GUILayout.TextArea ("Port..", 5);
			if (GUILayout.Button ("创建服务器")) {
				netWorkManage.StartServer ();
			}
			if (GUILayout.Button ("创建主机")) {
				netWorkManage.networkAddress = ip;
				netWorkManage.networkPort = port;
				netWorkManage.StartHost ();
			}
			if (GUILayout.Button ("进入主机")) {
				netWorkManage.networkAddress = ip;
				netWorkManage.networkPort = 7777;
				netWorkManage.StartClient ();
			}
		}
		//服务器是否已启动
		if (NetworkServer.active) {
			string text = "Server: port=" + netWorkManage.networkPort;
			GUILayout.Label ("服务器： " + text);
		}
		//客户端是否连接
		if (netWorkManage.IsClientConnected ()) {
			GUILayout.Label ("客户端:");
			GUILayout.Label ("      address=" + netWorkManage.networkAddress);
			GUILayout.Label ("      port=" + netWorkManage.networkPort);

			if (GUILayout.Button ("退出主机")) {
				netWorkManage.StopHost ();
			}
		}

		GUILayout.EndVertical ();
	}
}