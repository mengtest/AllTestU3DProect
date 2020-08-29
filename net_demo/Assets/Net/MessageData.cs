using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Game
{
    public class TalkMsg
    {
        public GIPEndPoint send;
        public string str;
    }
    public class GlobalData
    {
        
    }


    [ProtoContract]
    public class ReceiveMsgData
    {
        [ProtoMember(1)]
        public GIPEndPoint receivePoint;
        [ProtoMember(2)]
        public Socket socket;
        [ProtoMember(3)]
        public byte[] receiveBytes;
    }

    [ProtoContract]
    public class GIPEndPoint
    {
        [ProtoMember(1)]
        public string ip = "127.0.0.1";
        [ProtoMember(2)]
        public int port = 1234;
        public GIPEndPoint() { }
        public static implicit operator IPEndPoint(GIPEndPoint payment)
        {
            return new IPEndPoint(IPAddress.Parse(payment.ip), payment.port);
        }
        public static implicit operator EndPoint(GIPEndPoint payment)
        {
            return new IPEndPoint(IPAddress.Parse(payment.ip), payment.port) as EndPoint;
        }
        public static implicit operator GIPEndPoint(IPEndPoint payment)
        {
            return new GIPEndPoint()
            {
                ip = payment.Address.ToString(),
                port = payment.Port,
            };
        }
        public static implicit operator GIPEndPoint(EndPoint payment)
        {
            IPEndPoint point = (IPEndPoint)payment;
            return new GIPEndPoint()
            {
                ip = point.Address.ToString(),
                port = point.Port,
            };
        }
        public override string ToString()
        {
            return string.Format("{0}:{1}", ip, port);
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(EndPoint) || obj.GetType() == typeof(IPEndPoint))
            {
                IPEndPoint point = obj as IPEndPoint;
                return point.Address.ToString() == ip && port == point.Port;
            }
            else if (obj.GetType() == typeof(GIPEndPoint))
            {
                GIPEndPoint point = obj as GIPEndPoint;
                return point.ip == ip && port == point.port;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return ip.GetHashCode() + port.GetHashCode();
        }
        public static bool operator ==(GIPEndPoint left, GIPEndPoint right)
        {
            return left.ip == right.ip && left.port == right.port;
        }
        public static bool operator !=(GIPEndPoint left, GIPEndPoint right)
        {
            return !(left == right);
        }
    }

    [ProtoContract]
    public class MessageData
    {
        [ProtoMember(1)]
        public HeadMsg head = new HeadMsg();
        [ProtoMember(2)]
        public GIPEndPoint receivePoint = new GIPEndPoint();
        [ProtoMember(3)]
        public byte[] data = null;
        public MessageData()
        {

        }
        public MessageData(byte[] _data)
        {
            data = _data;
        }
    }

    [ProtoContract]
    public class HeadMsg
    {
        [ProtoMember(1)]
        public int flgHead = 0;
        [ProtoMember(2)]
        public int cmd = 0;
        [ProtoMember(3)]
        public int scmd = 0;
        [ProtoMember(4)]
        public int msgLen = 0;
        [ProtoMember(5)]
        public int msgOrder = 0;
        [ProtoMember(6)]
        public int msgUid = 0;
        [ProtoMember(7)]
        public int msgToken = 0;
        [ProtoMember(8)]
        public int flgEnd = 0;
        public HeadMsg() { }
    }
}