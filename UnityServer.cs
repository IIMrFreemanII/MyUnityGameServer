using System;
using System.Net;
using System.Net.Sockets;

namespace MyUnityGameServer
{
    public static class UnityServer
    {
        private static IPAddress _ipAddress = IPAddress.Any;
        private static int _port = 8000;
        
        private static TcpListener serverSocket = new TcpListener(_ipAddress, 8000);

        public static void InitializeNetwork()
        {
            Console.WriteLine("Initializing Packets...");
            ServerHandleData.InitializePackets();
            
            serverSocket.Start();
            
            Console.WriteLine($"Server IP: {_ipAddress}");
            Console.WriteLine($"Server started on port: {_port}");
            
            serverSocket.BeginAcceptTcpClient(OnClientConnect, null);
        }

        private static void OnClientConnect(IAsyncResult result)
        {
            TcpClient client = serverSocket.EndAcceptTcpClient(result);
            serverSocket.BeginAcceptTcpClient(OnClientConnect, null);
            
            ClientsManager.CreateNewConnection(client);
        }
    }
}