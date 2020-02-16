using System;

namespace MyUnityGameServer
{
    public enum ClientPackets
    {
        ClientHelloServer = 1,
        ClientName,
    }
    
    public static class DataReceiver
    {
        public static void HandleHelloServer(int connectionId, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.Write(data);
            int packetId = buffer.ReadInt();
            string message = buffer.ReadString();
            buffer.Dispose();

            Console.WriteLine(message);
        }

        public static void HandleClientName(int connectionId, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.Write(data);
            int packetId = buffer.ReadInt();
            string clientName = buffer.ReadString();
            buffer.Dispose();

            ClientsManager.clients[connectionId].clientName = clientName;

            Console.WriteLine($"{clientName} has been connected.");
            
            DataSender.SendWelcomeToClientWithName(connectionId);
        }
    }
}