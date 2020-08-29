using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Game
{
    public class Client
    {
        public static Thread receiveThread;
        public static Thread dealThread;
        public static Socket socket;
        public static Queue<ReceiveMsgData> queue = new Queue<ReceiveMsgData>();

        public static void Start()
        {
            //设定服务器IP地址
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8885)); //配置服务器IP与端口
                receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(socket);
            }
            catch
            {
                return;
            }
        }

        private static void DealMessage(object o)
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    ReceiveMsgData data = queue.Dequeue();
                    MessageManage.Self.DealMsg(data);
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                byte[] result = new byte[1024];
                int receiveLength = myClientSocket.Receive(result);
                Console.WriteLine(receiveLength);
                if (receiveLength > 0)
                {
                    ReceiveMsgData data = new ReceiveMsgData();
                    data.receivePoint = myClientSocket.RemoteEndPoint;
                    data.receiveBytes = result;

                    Debug.Log("ReceiveMessage " + data.receivePoint.ToString());
                    MessageManage.Self.DealMsg(data);
                }
            }
        }

        public static void SendMsg(int cmd, int scmd, ByteBuffer buffer)
        {
            MessageData sendMsg = new MessageData();
            HeadMsg head = new HeadMsg();
            head.cmd = cmd;
            head.scmd = scmd;
            sendMsg.head = head;
            sendMsg.data = buffer.ToBytes();
            byte[] bytes = ProtoBufTools.Serialize(sendMsg);
            ByteBuffer sendBuffer = new ByteBuffer();
            sendBuffer.WriteInt32(bytes.Length);
            sendBuffer.WriteBytes(bytes);
            bytes = sendBuffer.ToBytes();
            socket.Send(bytes);
        }

        public static void Close()
        {
            if (receiveThread != null)
            {
                receiveThread.Interrupt();
                receiveThread.Abort();
            }
            if (socket != null)
            {
                socket.Close();
            }
        }
    }
}