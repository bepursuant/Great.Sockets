using System;
using System.Net;
using System.Net.Sockets;

namespace Great.Sockets
{
    public class SocketServer
    {
        private readonly TcpListener _server;

        public SocketServer(IPEndPoint endpoint)
        {
            _server = new TcpListener(endpoint);
        }

        public void Start()
        {
            _server.Start();
            _waitForConnection();
        }

        public void Stop()
        {
            _server.Stop();
            ServerStopped(new ServerStoppedEventArgs());
        }


        private async void _waitForConnection()
        {
            try
            {
                var connection = await _server.AcceptSocketAsync();
                NewConnection(new NewConnectionEventArgs(connection));
                _waitForConnection();
            }
            catch (Exception)
            {
                Stop();
            }
        }

        public event NewConnectionHandler NewConnection;
        public event ServerStoppedHandler ServerStopped;
    }
}
