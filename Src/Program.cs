using System;

namespace GameServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = $"Game Server";
            var server = new Server();
            server.Start(50, 26950);
            Console.ReadKey();
        }
    }
}