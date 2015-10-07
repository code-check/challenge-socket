using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketChallenge
{
    [TestFixture]
    public class Tests
    {
        const int MaxWait = 5000;
        const int Wait = 1000;

        public Listener Server { get; set; }
        public List<Socket> Clients = new List<Socket>();

        public Client Client1 = new Client();
        public Client Client2 = new Client();

        [TestFixtureSetUp]
        public void Setup()
        {
            Server = new Listener();
            Server.ClientConnected += (socket) => Clients.Add(socket);

            int port = ((IPEndPoint)Server.LocalEndpoint).Port;
            Client1.Connect("127.0.0.1", port);
            Client2.Connect("127.0.0.1", port);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Server.Stop();
            Clients.ForEach(x => x.Dispose());
        }


        [Test]
        public void Step1()
        {
            Assert.IsTrue(Task.Run(async () =>
            {
                await Task.Delay(Wait);
                Assert.AreEqual(2, Clients.Count, "Clients failed to connect within 1000ms");
            }).Wait(MaxWait), "Timeout expired");
        }

        [Test]
        public void Step2()
        {
            Assert.AreEqual(true, Clients.TrueForAll(x => x.Connected), "One or more clients appear to be disconnected");

            Assert.IsTrue(Task.Run(() =>
            {
                Clients.ForEach(x =>
                {
                    byte[] packet = new byte[12];
                    x.Receive(packet);
                    string hello = Encoding.ASCII.GetString(packet);
                    Assert.IsTrue(hello.Equals("Hello World!", StringComparison.CurrentCultureIgnoreCase), "Excepted the packet to contain \"Hello World!\"");
                });
            }).Wait(MaxWait), "Timeout expired");
        }

        [Test]
        public void Step3()
        {
            Assert.AreEqual(true, Clients.TrueForAll(x => x.Connected), "One or more clients appear to be disconnected");
            Assert.AreEqual(0, Clients.Sum(x => x.Available), "Received data on the server, but was not expecting any");

            Assert.IsTrue(Task.Run(async () =>
            {
                Clients.ForEach(x => x.Send(Encoding.ASCII.GetBytes("Ping")));
                await Task.Delay(Wait);
                Assert.AreEqual(0, Clients.Sum(x => x.Available), "Received data on the server, but was not expecting any");

                Clients.ForEach(x => x.Send(Encoding.ASCII.GetBytes("Marco")));
                Clients.ForEach(x =>
                {
                    byte[] packet = new byte[12];
                    x.Receive(packet);
                    string hello = Encoding.ASCII.GetString(packet);
                    Assert.IsTrue(hello.StartsWith("Polo"), "Excepted the packet to contain \"Polo\"");
                });
            }).Wait(MaxWait), "Timeout expired, no data received within 5 seconds.");
        }
    }
}
