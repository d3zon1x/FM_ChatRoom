using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

namespace ServerConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            ChatServer.Instance.Start();
            Console.ReadLine();
        }
    }
}
