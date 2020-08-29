using System;

namespace Game {
    public interface IClient {
        void InitSocket ();
        void Send (byte[] _data);
        void SocketQuit ();
        void SocketReceive ();
    }
}