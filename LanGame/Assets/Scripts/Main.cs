using System;
using System.Collections;
using UnityEngine;
//引入库
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;

namespace Game {
    public class Main : MonoBehaviour {
        private static Main _self = null;
        public static Main Self {
            get {
                return _self;
            }
        }
        public ClientType clientType = ClientType.NULL;
        public UdpServerr server = null;
        public UdpClientt client = null;
        public Player curPlayer = null;
        public Queue<MessageReceiveData> messageQueue = new Queue<MessageReceiveData> ();
        public delegate void MsgCallBack (MessageReceiveData _data);
        public event MsgCallBack msgCallBack = null;
        public void CallEventMsg (MessageReceiveData _data) {
            if (msgCallBack != null) {
                msgCallBack (_data);
            }
        }
        void Start () {
            _self = this;
            msgCallBack += MessageManage.Self.DealMsg;
            // Debug.logger.logEnabled = false;
            StartCoroutine (DealQueueMessage ());
        }
        
        IEnumerator DealQueueMessage () {
            while (true) {
                while (messageQueue.Count > 0) {
                    CallEventMsg (messageQueue.Dequeue ());
                }
                yield return 1;
            }
        }

        void Update () {
            if (curPlayer != null) {
            }
            if (clientType == ClientType.server) {
                server.CheckCallSend ();
            }
        }

        [HideInInspector]
        public Dictionary<GIPEndPoint, Player> playerList = new Dictionary<GIPEndPoint, Player> ();
        private string editString = "";

        private void OnGUI () {
            if (clientType == ClientType.NULL) {
                if (GUI.Button (new Rect (0, 50, 100, 50), "建立主机")) {
                    clientType = ClientType.server;
                    server = new UdpServerr ();
                    server.InitSocket ();
                }
                if (GUI.Button (new Rect (0, 0, 100, 50), "连接 服务器")) {
                    clientType = ClientType.client;
                    client = new UdpClientt ();
                    client.InitSocket ();
                }
            } else if (clientType == ClientType.client) {
                editString = GUI.TextField (new Rect (100, 0, 100, 50), editString);
                if (GUI.Button (new Rect (200, 0, 100, 50), "send")) {
                    MessageManage.Self.Send_1_10 (editString);
                }
                if (GUI.Button (new Rect (300, 0, 100, 50), "断开")) {
                    MessageManage.Self.Send_1_2 ();
                    clientType = ClientType.NULL;
                    foreach (Player player in Main.Self.playerList.Values) {
                        player.Quit ();
                    }
                    Main.Self.playerList.Clear ();
                }
            }
            if (GUI.Button (new Rect (200, 50, 100, 50), "ok")) {
                Action addPlayer = () => {
                    string[] strs = new string[] {
                    "23123212",
                    "12312313131313131313",
                    "4564564426427544564456464264262462456246314535",
                    "45645644264275445644564642642624624213123131313313131135",
                    };
                    IPEndPoint iPEndPoint = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 6666);
                    System.Random ran = new System.Random ();
                    TalkManage.Self.SetTalk (iPEndPoint, strs[UnityEngine.Random.Range (0, strs.Length)]);
                };
                addPlayer ();
            }
        }

        public void SetHand (Image img) {
            img.sprite = spr[UnityEngine.Random.Range (0, spr.Length)];
        }

        public Sprite[] spr = null;

        private void OnApplicationQuit () {
            if (client != null) {
                MessageManage.Self.Send_1_2 ();
                client.SocketQuit ();
            }
            if (server != null) {
                server.SocketQuit ();
            }
        }
    }

    public enum ClientType {
        NULL,
        client,
        server,
    }
}