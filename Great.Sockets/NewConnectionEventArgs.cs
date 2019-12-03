using System;
using System.Net.Sockets;

namespace Great.Sockets
{
    public class NewConnectionEventArgs : EventArgs
    {
        public Socket NewConnection { get; set; }

        public NewConnectionEventArgs(Socket connection)
        {
            NewConnection = connection;
        }
    }

    public delegate void NewConnectionHandler(NewConnectionEventArgs args);
}