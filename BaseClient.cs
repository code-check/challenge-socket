using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketChallenge
{
    public abstract class BaseClient
    {
        public abstract void Connect(string ip, int port);

        public abstract void Send(byte[] data);
    }
}
