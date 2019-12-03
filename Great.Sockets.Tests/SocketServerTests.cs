using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Xunit;

namespace Great.Sockets.Tests
{
    public class SocketServerTests
    {
        [Fact]
        public void BasicTests()
        {
            var s = new SocketServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234));

            s.NewConnection += (args) =>
            {
                Debug.WriteLine($"New Connection from {args.NewConnection.RemoteEndPoint}");
            };

            s.ServerStopped += (args) =>
            {
                Debug.WriteLine("Server Stopped!");
            };

            s.Start();
            Thread.Sleep(20000);
            s.Stop();
        }

    }
}
