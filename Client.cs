using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketChallenge
{
    public class Client : BaseClient
    {
        //BEGIN_CHALLENGE
        private Socket ClientSocket { get; set; }
        private byte[] Buffer = new byte[1024];

        public override void Connect(string ip, int port)
        {
            ClientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            ClientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), OnConnect, null);
        }

        public void OnConnect(IAsyncResult iar)
        {
            ClientSocket.EndConnect(iar);
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.SetBuffer(Buffer, 0, Buffer.Length);
            args.Completed += OnReceive;
            ClientSocket.ReceiveAsync(args);

            Send(Encoding.ASCII.GetBytes("Hello World!"));
        }

        public void OnReceive(object sender, SocketAsyncEventArgs args)
        {
            int size = args.BytesTransferred;
            if (size == 0)
                return;

            byte[] packet = new byte[size];
            Array.Copy(Buffer, packet, size);

            if (Encoding.ASCII.GetString(packet).Equals("Marco"))
                Send(Encoding.ASCII.GetBytes("Polo"));

            ClientSocket.ReceiveAsync(args);
        }

        public override void Send(byte[] data)
        {
            ClientSocket.Send(data);
        }
        //END_CHALLENGE
    }
}
