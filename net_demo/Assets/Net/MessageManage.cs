using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Game
{
    public class MessageManage
    {
        private static MessageManage _self;
        public static MessageManage Self
        {
            get
            {
                if (_self == null)
                {
                   _self = new MessageManage();
                }
                return _self;
            }
        }

        private MessageManage() {

        }
        public void Init()
        {

        }
        public void OnSocketConnect(Socket socket, GIPEndPoint point)
        {

        }

        public void DealMsg(ReceiveMsgData _data)
        {
            if (_data == null) return;
            byte[] _dataBytes = _data.receiveBytes;
            IPEndPoint receivePoint = _data.receivePoint;
            ByteBuffer _buff = new ByteBuffer(_dataBytes);
            int length = _buff.ReadInt32();         //真实长度
            byte[] _bytes = _buff.ReadBytes(length);
            MessageData data = ProtoBufTools.DeSerialize<MessageData>(_bytes);
            data.receivePoint = receivePoint;
            DealMsgSwitch(data);
        }



        public void DealMsgSwitch(MessageData data)
        {
            switch (data.head.cmd)
            {
                case 1:
                    switch (data.head.scmd)
                    {
                        case 1:  Deal_1_1(data);  break;
                        case 2:  Deal_1_2(data);  break;
                        case 10: Deal_1_10(data); break;
                        case 11: Deal_1_11(data); break;
                        case 12: Deal_1_12(data); break;
                        default:break;
                    }
                    break;
                default:
                    break;
            }
        }

        public void Send_1_1()
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            Client.SendMsg(1, 1, buff);
        }

        public void Deal_1_1(MessageData data)
        {
            ByteBuffer buff = new ByteBuffer(data.data);
            Debug.Log(buff.ReadString());
        }

        public void Send_1_2()
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            Client.SendMsg(1, 2, buff);
        }

        public void Deal_1_2(MessageData data)
        {

        }

        public void Deal_1_10(MessageData data)
        {

        }


        public void Send_1_10(string _str)
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            Client.SendMsg(1, 10, buff);
        }

        public void Deal_1_12(MessageData data)
        {

        }

        public void Send_1_12()
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            Client.SendMsg(1, 12, buff);
        }

        public void Deal_1_11(MessageData data)
        {
        }

        public void Send_1_11()
        {
            ByteBuffer buff = new ByteBuffer();
            buff.WriteString("ok");
            Client.SendMsg(1, 11, buff);
        }
    }
}
