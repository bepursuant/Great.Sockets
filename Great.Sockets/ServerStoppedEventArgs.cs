using System;

namespace Great.Sockets
{
    public class ServerStoppedEventArgs : EventArgs
    {

        public ServerStoppedEventArgs()
        {

        }
    }

    public delegate void ServerStoppedHandler(ServerStoppedEventArgs args);
}