using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.TCP
{
    class ServerObject
    {
        public ServerObject(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();
        }

        public void Run()
        {
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Connect to " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                ClientObject clientObject = new ClientObject(client);
                Thread clientThread = new Thread(new ThreadStart(clientObject.GetCommand));
                clientThread.Start();
            }
        }

        public string ip { get; }
        public int port { get; }
        private readonly TcpListener listener;
    }
}
