using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
// using AdvancedInspector;
using ProtoBuf;
using UnityEngine;

namespace Game {
	[SerializeField, ProtoContract]
	public class MessageReceiveData {
		[ProtoMember (1)]
		public GIPEndPoint receivePoint;
		[ProtoMember (2)]
		public byte[] receiveBytes;
	}

	[SerializeField, ProtoContract]
	public class GVector3 {
		[ProtoMember (1)]
		public double x = 0;
		[ProtoMember (2)]
		public double y = 0;
		[ProtoMember (3)]
		public double z = 0;
		public GVector3 () {

		}
		public GVector3 (double _x, double _y, double _z) {
			x = _x;
			y = _y;
			z = _z;
		}
		public override string ToString () {
			return string.Format ("{0},{1},{2}", x, y, z);
		}
		public static implicit operator Vector3 (GVector3 payment) {
			return new Vector3 () {

				x = (float) payment.x,
					y = (float) payment.y,
					z = (float) payment.z,
			};
		}
		public static implicit operator GVector3 (Vector3 payment) {
			return new GVector3 () {
				x = payment.x,
					y = payment.y,
					z = payment.z,
			};
		}
	}

	[SerializeField, ProtoContract]
	public class GIPEndPoint {
		[ProtoMember (1)]
		public string ip = "127.0.0.1";
		[ProtoMember (2)]
		public int port = 1234;
		public GIPEndPoint () { }
		public static implicit operator IPEndPoint (GIPEndPoint payment) {
			return new IPEndPoint (IPAddress.Parse (payment.ip), payment.port);
		}
		public static implicit operator EndPoint (GIPEndPoint payment) {
			return new IPEndPoint (IPAddress.Parse (payment.ip), payment.port) as EndPoint;
		}
		public static implicit operator GIPEndPoint (IPEndPoint payment) {
			return new GIPEndPoint () {
				ip = payment.Address.ToString (),
					port = payment.Port,
			};
		}
		public static implicit operator GIPEndPoint (EndPoint payment) {
			IPEndPoint point = (IPEndPoint) payment;
			return new GIPEndPoint () {
				ip = point.Address.ToString (),
					port = point.Port,
			};
		}
		public override string ToString () {
			return string.Format ("{0}:{1}", ip, port);
		}
		public override bool Equals (object obj) {
			if (obj.GetType () == typeof (EndPoint) || obj.GetType () == typeof (IPEndPoint)) {
				IPEndPoint point = obj as IPEndPoint;
				return point.Address.ToString () == ip && port == point.Port;
			} else if (obj.GetType () == typeof (GIPEndPoint)) {
				GIPEndPoint point = obj as GIPEndPoint;
				return point.ip == ip && port == point.port;
			}
			return false;
		}
		public override int GetHashCode () {
			return ip.GetHashCode () + port.GetHashCode ();
		}
		public static bool operator == (GIPEndPoint left, GIPEndPoint right) {
			return left.ip == right.ip && left.port == right.port;
		}
		public static bool operator != (GIPEndPoint left, GIPEndPoint right) {
			return !(left == right);
		}
	}

	[SerializeField, ProtoContract]
	public class MessageData<T> where T : new () {
		[ProtoMember (1)]
		public HeadMsg head = new HeadMsg ();
		[ProtoMember (2)]
		public T body = new T ();
		public MessageData () { }
	}

	[SerializeField, ProtoContract]
	public class HeadMsg {
		[ProtoMember (1)]
		public int flgHead = 0;
		[ProtoMember (2)]
		public int cmd = 0;
		[ProtoMember (3)]
		public int scmd = 0;
		[ProtoMember (4)]
		public int msgLen = 0;
		[ProtoMember (5)]
		public int msgOrder = 0;
		[ProtoMember (6)]
		public int msgUid = 0;
		[ProtoMember (7)]
		public int msgToken = 0;
		[ProtoMember (8)]
		public int flgEnd = 0;
		public HeadMsg () { }
	}

	[SerializeField, ProtoContract]
	[ProtoInclude (100, typeof (MessageData_1_1))]
	[ProtoInclude (200, typeof (MessageData_1_2))]
	[ProtoInclude (300, typeof (MessageData_1_10))]
	[ProtoInclude (400, typeof (MessageData_1_11))]
	public class BaseMessageData {
		[ProtoMember (1)]
		public GIPEndPoint _ip = new GIPEndPoint ();
		[ProtoMember (2)]
		public int dealFlg = 0;
		public BaseMessageData () {

		}
	}

	/// <summary>
	/// 登陆
	/// </summary>
	[SerializeField, ProtoContract]
	public class MessageData_1_1 : BaseMessageData {
		[ProtoMember (1)]
		public List<PlayerInfo> players = new List<PlayerInfo> ();
		public MessageData_1_1 () {
			players = new List<PlayerInfo> ();
		}
		public PlayerInfo InvokeConstructor () {
			return new PlayerInfo ();
		}
		public override string ToString () {
			return "登陆";
		}

		[SerializeField, ProtoContract]
		public class PlayerInfo {
			[ProtoMember (1)]
			public GIPEndPoint ip = new GIPEndPoint ();
			[ProtoMember (2)]
			public GVector3 pos = new GVector3 ();
			[ProtoMember (3)]
			public GVector3 velocity = new GVector3 ();
			[ProtoMember (4)]
			public GVector3 rot = new GVector3 ();
			[ProtoMember (5)]
			public double aimValue = 1;
			public PlayerInfo () { }
		}
	}

	/// <summary>
	/// 离开
	/// </summary>

	[SerializeField, ProtoContract]
	public class MessageData_1_2 : BaseMessageData {
		public override string ToString () {
			return "离开";
		}
	}

	/// <summary>
	/// 说话
	/// </summary>

	[SerializeField, ProtoContract]
	public class MessageData_1_10 : BaseMessageData {
		[ProtoMember (1)]
		public string talkStr = "";
		public override string ToString () {
			return "说话";
		}
	}

	/// <summary>
	/// 操作
	/// </summary>

	[SerializeField, ProtoContract]
	public class MessageData_1_11 : BaseMessageData {
		[ProtoMember (1)]
		public Dictionary<OperType, List<GVector3>> oper = new Dictionary<OperType, List<GVector3>> ();
		[SerializeField, ProtoContract]
		public enum OperType {
			nil,
			wantMove,
			move,
			aim,
			attack,
			jump,
		}
		public override string ToString () {
			return "操作";
		}
	}
}