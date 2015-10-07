using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketChallenge
{
    public class Listener : TcpListener
    {
        public Action<Socket> ClientConnected;

        public Listener()
            : base(IPAddress.Any, 0)
        {
            Start(10);
            BeginAcceptSocket(OnClient, null);
        }

        public void OnClient(IAsyncResult result)
        {
            Socket client = EndAcceptSocket(result);
            if (ClientConnected != null)
                ClientConnected(client);
            BeginAcceptSocket(OnClient, null);
        }
    }
}
