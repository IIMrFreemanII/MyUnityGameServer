using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace MyUnityGameServer
{
    public static class ClientsManager
    {
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public static void CreateNewConnection(TcpClient tempClient)
        {
            Client newClient = new Client();
            newClient.socket = tempClient;
            newClient.connectionId = ((IPEndPoint) tempClient.Client.RemoteEndPoint).Port;
            newClient.Start();
            clients.Add(newClient.connectionId, newClient);
            
            DataSender.SendWelcomeMessage(newClient.connectionId);
        }

        public static void SendDataTo(int connectionId, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.Write((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.Write(data);
            clients[connectionId].stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer.Dispose();
        }
    }
}