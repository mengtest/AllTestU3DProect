using System.Collections;
using UnityEngine;
//引入库
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Game {
	[Serializable]
	public class UdpServerr : IClient {
		//以下默认都是私有的成员
		Socket socket; //目标socket
		public List<EndPoint> clientsEnd = new List<EndPoint> (); //客户端
		public IPEndPoint ipEnd; //侦听端口
		int recvLen; //接收的数据长度
		Thread connectThread; //连接线程
		public Queue<byte[]> sendMessageToAllQueue = new Queue<byte[]> ();
		//初始化
		public void InitSocket () {
			//定义侦听端口,侦听任何IP
			ipEnd = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 9999);
			//定义套接字类型,在主线程中定义
			socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			//服务端需要绑定ip
			socket.Bind (ipEnd);

			//开启一个线程连接，必须的，否则主线程卡死
			connectThread = new Thread (new ThreadStart (SocketReceive));
			connectThread.Start ();

			clientsEnd.Add (ipEnd);
			MessageManage.Self.Send_1_1 ();
		}

		//分发给所有玩家
		public void Send (byte[] data) {
			socket.SendTo (data, data.Length, SocketFlags.None, ipEnd);
		}

		public void CheckCallSend () {
			while (sendMessageToAllQueue.Count > 0) {
				byte[] bytes = sendMessageToAllQueue.Dequeue ();
				foreach (EndPoint client in clientsEnd) {
					IPEndPoint point = (IPEndPoint) client;
					socket.SendTo (bytes, bytes.Length, SocketFlags.None, client);
				}
			}
		}

		//分发给所有玩家
		public void AddSendQueue (byte[] data) {
			sendMessageToAllQueue.Enqueue (data);
		}

		//服务器接收
		public void SocketReceive () {
			//定义客户端
			IPEndPoint sender = new IPEndPoint (IPAddress.Any, 0);
			EndPoint endPoint = (EndPoint) sender;
			//进入接收循环
			while (true) {
				//对data清零
				byte[] recvData = new byte[1024];
				//获取客户端，获取客户端数据，用引用给客户端赋值
				recvLen = socket.ReceiveFrom (recvData, ref endPoint);
				if (recvLen > 0) {
					MessageReceiveData data = new MessageReceiveData ();
					data.receivePoint = endPoint;
					data.receiveBytes = recvData;
					Main.Self.messageQueue.Enqueue (data);
				}
			}
		}

		public void AddPlayer (EndPoint _endPoint) {
			if (!clientsEnd.Contains (_endPoint)) {
				clientsEnd.Add (_endPoint);
			}
		}

		public void RemovePlayer (EndPoint _endPoint) {
			if (clientsEnd.Contains (_endPoint)) {
				clientsEnd.Remove (_endPoint);
			}
		}

		//连接关闭
		public void SocketQuit () {
			//关闭线程
			if (connectThread != null) {
				connectThread.Interrupt ();
				connectThread.Abort ();
			}
			//最后关闭socket
			if (socket != null) {
				socket.Close ();
			}
		}
	}
}