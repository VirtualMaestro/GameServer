using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class Server
    {
        private readonly Dictionary<int, Client> _clients = new Dictionary<int, Client>();
        private TcpListener _tcpListener;

        public int MaxPlayers { get; private set; }
        public int Port { get; private set; }

        public void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;

            _InitializeServerData();
            Console.WriteLine($"Starting server...");

            _tcpListener = new TcpListener(IPAddress.Any, Port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(_OnTCPConnectCallback, null);

            Console.WriteLine($"Server started on {Port}");
        }

        private void _InitializeServerData()
        {
            for (var i = 1; i <= MaxPlayers; i++)
            {
                _clients.Add(i, new Client(i));
            }
        }

        private void _OnTCPConnectCallback(IAsyncResult result)
        {
            var client = _tcpListener.EndAcceptTcpClient(result);
            _tcpListener.BeginAcceptTcpClient(_OnTCPConnectCallback, null);

            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (var i = 1; i <= MaxPlayers; i++)
            {
                if (_clients[i].Tcp.Socket != null) continue;

                _clients[i].Tcp.Connect(client);
                return;
            }

            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }
    }
}