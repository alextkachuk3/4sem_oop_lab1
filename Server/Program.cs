using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip;
            int port;
            ip = Console.ReadLine();
            if (ip == "default")
            {
                ip = "192.168.0.85";
                port = 25565;
            }
            else
            {
                port = int.Parse(Console.ReadLine());
            }

            TCP.ServerObject serverObject = new TCP.ServerObject(ip, port);
            serverObject.Run();
        }
    }
}
