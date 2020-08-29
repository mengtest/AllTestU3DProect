using System.Collections.Generic;
using System.Net;
using ProtoBuf;
using UnityEngine;

namespace Game {
    public class MessageManage {
        public byte[] ServerDealMsg (MessageData<BaseMessageData> data, IPEndPoint ip) {
            switch (data.head.cmd) {
                case 1:
                    switch (data.head.scmd) {
                        case 1:
                            MessageData_1_1 messageData = new MessageData_1_1 ();
                            foreach (EndPoint _point in Main.Self.server.clientsEnd) {
                                Vector3 _pos = Vector3.zero;
                                Vector3 _rot = Vector3.zero;
                                double _aimValue = 1;
                                if (Main.Self.playerList.ContainsKey (_point)) {
                                    Player player = Main.Self.playerList[_point];
                                    _pos = player.trans.localPosition;
                                    _rot = player.trans.localEulerAngles;
                                    _aimValue = player.aimValue;
                                }
                                messageData.players.Add (new MessageData_1_1.PlayerInfo () {
                                    ip = _point,
                                        pos = _pos,
                                        rot = _rot,
                                        aimValue = _aimValue,
                                });
                            }
                            data.body = messageData;
                            break;
                        case 2:
                            break;
                        case 10:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            data.body.dealFlg = 1;
            data.body._ip = ip;
            return data.ToBytes ();
        }

        public void DealMsg (MessageReceiveData _data) {
            if (_data == null) {
                return;
            }
            byte[] _dataBytes = _data.receiveBytes;
            IPEndPoint receivePoint = _data.receivePoint;
            ByteBuffer _buff = new ByteBuffer (_dataBytes);
            int length = _buff.ReadInt32 ();
            byte[] _bytes = _buff.ReadBytes (length);
            MessageData<BaseMessageData> data = ProtoBufTools.DeSerialize<MessageData<BaseMessageData>> (_bytes);
            if (data.body.dealFlg == 0) {
                if (Main.Self.clientType == ClientType.server) {
                    if (data.head.cmd == 1 && data.head.scmd == 1) {
                        Main.Self.server.AddPlayer (receivePoint);
                    } else if (data.head.cmd == 1 && data.head.scmd == 2) {
                        Main.Self.server.RemovePlayer (receivePoint);
                    }
                    byte[] _serverDealBytes = ServerDealMsg (data, receivePoint);
                    ByteBuffer _serverDealBuff = new ByteBuffer ();
                    _serverDealBuff.WriteInt32 (_serverDealBytes.Length);
                    _serverDealBuff.WriteBytes (_serverDealBytes);
                    Main.Self.server.AddSendQueue (_serverDealBuff.ToBytes ());
                }
            } else {
                if (Main.Self.clientType == ClientType.server) {

                } else {

                }
                SwitchMsg (data);
            }
        }

        public void SendMsg (int cmd, int scmd, BaseMessageData body) {
            MessageData<BaseMessageData> data = new MessageData<BaseMessageData> ();
            HeadMsg head = new HeadMsg ();
            head.cmd = cmd;
            head.scmd = scmd;
            data.head = head;
            data.body = body;
            ByteBuffer _buff = new ByteBuffer ();
            byte[] bytes = data.ToBytes ();
            _buff.WriteInt32 (bytes.Length);
            _buff.WriteBytes (bytes);
            bytes = _buff.ToBytes ();
            switch (Main.Self.clientType) {
                case ClientType.client:
                    Main.Self.client.Send (bytes);
                    break;
                case ClientType.server:
                    Main.Self.server.Send (bytes);
                    break;
                case ClientType.NULL:
                    break;
                default:
                    break;
            }
        }

        private static MessageManage _self = new MessageManage ();
        public static MessageManage Self {
            get {
                return _self;
            }
        }
        private MessageManage () { }

        public void SwitchMsg (MessageData<BaseMessageData> data) {
            switch (data.head.cmd) {
                case 1:
                    switch (data.head.scmd) {
                        case 1:
                            Deal_1_1 (data);
                            break;
                        case 2:
                            Deal_1_2 (data);
                            break;
                        case 10:
                            Deal_1_10 (data);
                            break;
                        case 11:
                            Deal_1_11 (data);
                            break;
                        case 12:
                            Deal_1_12 (data);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        //加入服务器
        public void Send_1_1 () {
            BaseMessageData body = new BaseMessageData ();
            SendMsg (1, 1, body);
        }
        public void Deal_1_1 (MessageData<BaseMessageData> data) {
            MessageData_1_1 messageData = data.body as MessageData_1_1;
            for (int i = 0; i < messageData.players.Count; i++) {
                MessageData_1_1.PlayerInfo playerInfo = messageData.players[i];
                GIPEndPoint playerIp = playerInfo.ip;
                if (!Main.Self.playerList.ContainsKey (playerIp)) {
                    GameObject go = GameObject.Instantiate (Resources.Load ("Player") as GameObject);
                    Player player = go.AddComponent<Player> ();
                    Vector3 pos = playerInfo.pos;
                    Vector3 rot = playerInfo.rot;
                    double aimValue = playerInfo.aimValue;
                    player.StartCallBack += () => {
                        player.curIp = playerIp;
                        player.SetName (playerIp.ToString ());
                        player.SetTrans (new List<GVector3> () { pos, rot });
                        player.aimValue = (float) aimValue;
                    };
                    Main.Self.playerList.Add (playerIp, player);
                    if (Main.Self.curPlayer == null && data.body._ip.Equals (playerIp)) {
                        Main.Self.curPlayer = player;
                        player.isLocalPlayer = true;
                        player.StartCallBack += () => {
                            player.SetColor ();
                        };
                    }
                }
            }
            Debug.Log ("欢迎 " + data.body._ip.ToString ());
        }
        //离开服务器
        public void Send_1_2 () {
            BaseMessageData body = new BaseMessageData ();
            SendMsg (1, 2, body);
        }
        public void Deal_1_2 (MessageData<BaseMessageData> data) {
            if (Main.Self.playerList.ContainsKey (data.body._ip)) {
                Debug.Log (data.body._ip.ToString () + "离开了");
                Main.Self.playerList[data.body._ip].Quit ();
                Main.Self.playerList.Remove (data.body._ip);
            }
        }
        public void Deal_1_10 (MessageData<BaseMessageData> data) {
            MessageData_1_10 messageData = data.body as MessageData_1_10;
            string str = messageData.talkStr;
            Debug.Log (messageData._ip + "说" + str);
        }
        //发送消息
        public void Send_1_10 (string _str) {
            MessageData_1_10 body = new MessageData_1_10 ();
            body.talkStr = _str;
            SendMsg (1, 10, body);
        }
        public void Deal_1_12 (MessageData<BaseMessageData> data) {
            if (Main.Self.playerList.ContainsKey (data.body._ip)) {
                Main.Self.playerList[data.body._ip].Attack ();
            }
        }
        //发送消息
        public void Send_1_12 () {
            MessageData_1_11 messageData = new MessageData_1_11 ();
            BaseMessageData body = new BaseMessageData ();
            SendMsg (1, 12, body);
        }
        public void Deal_1_11 (MessageData<BaseMessageData> data) {
            MessageData_1_11 messageData = data.body as MessageData_1_11;
            if (Main.Self.playerList.ContainsKey (data.body._ip)) {
                Player player = Main.Self.playerList[data.body._ip];
                foreach (MessageData_1_11.OperType item in messageData.oper.Keys) {
                    List<GVector3> vet = messageData.oper[item];
                    switch (item) {
                        case MessageData_1_11.OperType.nil:
                            break;
                        case MessageData_1_11.OperType.wantMove:
                            player.Move (vet);
                            break;
                        case MessageData_1_11.OperType.move:
                            if (!player.isLocalPlayer) {
                                player.SetTrans (vet);
                            }
                            break;
                        case MessageData_1_11.OperType.aim:
                            player.SetAimValue (vet);
                            break;
                        case MessageData_1_11.OperType.attack:
                            player.Attack ();
                            break;
                        case MessageData_1_11.OperType.jump:
                            player.Jump (vet);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //发送消息
        public void Send_1_11 (Dictionary<MessageData_1_11.OperType, List<GVector3>> _oper) {
            MessageData_1_11 data = new MessageData_1_11 ();
            data.oper = _oper;
            SendMsg (1, 11, data);
        }
    }
}