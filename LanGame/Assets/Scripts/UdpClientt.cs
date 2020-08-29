using System.Collections;
using UnityEngine;
//引入库
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Game {
	[Serializable]
	public class UdpClientt : IClient {
		//以下默认都是私有的成员
		Socket socket; //目标socket
		EndPoint serverEnd; //收到的服务端
		IPEndPoint ipEnd; //服务端端口
		int recvLen; //接收的数据长度
		Thread connectThread; //连接线程
		//初始化
		public void InitSocket () {
			//定义连接的服务器ip和端口，可以是本机ip，局域网，互联网
			ipEnd = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 9999);
			//定义套接字类型,在主线程中定义
			socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			//建立初始连接，这句非常重要，第一次连接初始化了serverEnd后面才能收到消息
			MessageManage.Self.Send_1_1 ();

			//开启一个线程连接，必须的，否则主线程卡死
			connectThread = new Thread (new ThreadStart (SocketReceive));
			connectThread.Start ();
		}

		public void Send (byte[] data) {
			//发送给指定服务端
			socket.SendTo (data, data.Length, SocketFlags.None, ipEnd);
		}

		//服务器接收
		public void SocketReceive () {
			//定义服务端
			IPEndPoint sender = new IPEndPoint (IPAddress.Any, 0);
			serverEnd = (EndPoint) sender;
			//进入接收循环
			while (true) {
				//对data清零
				byte[] recvData = new byte[1024];
				//获取客户端，获取服务端端数据，用引用给服务端赋值，实际上服务端已经定义好并不需要赋值
				recvLen = socket.ReceiveFrom (recvData, ref serverEnd);
				if (recvLen > 0) {
					MessageReceiveData data = new MessageReceiveData ();
					data.receivePoint = serverEnd;
					data.receiveBytes = recvData;
					Main.Self.messageQueue.Enqueue (data);
				}
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