using System.Collections.Generic;

namespace MyUnityGameServer
{
    public static class ServerHandleData
    {
        public delegate void Packet(int connectionId, byte[] data);
        public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

        public static void InitializePackets()
        {
            packets.Add((int) ClientPackets.ClientHelloServer, DataReceiver.HandleHelloServer);
            packets.Add((int) ClientPackets.ClientName, DataReceiver.HandleClientName);
        }
        
        public static void HandleData(int connectionId, byte[] data)
        {
            byte[] buffer = (byte[]) data.Clone();
            int packetLength = 0;

            if (ClientsManager.clients[connectionId].buffer == null)
            {
                ClientsManager.clients[connectionId].buffer = new ByteBuffer();
            }
            
            ClientsManager.clients[connectionId].buffer.Write(buffer);

            if (ClientsManager.clients[connectionId].buffer.Count == 0)
            {
                ClientsManager.clients[connectionId].buffer.Clear();
                return;
            }

            if (ClientsManager.clients[connectionId].buffer.Length >= 4)
            {
                packetLength = ClientsManager.clients[connectionId].buffer.ReadInt(false);
                if (packetLength <= 0)
                {
                    ClientsManager.clients[connectionId].buffer.Clear();
                    return;
                }
            }

            while (packetLength > 0 && packetLength <= ClientsManager.clients[connectionId].buffer.Length - 4)
            {
                if (packetLength <= ClientsManager.clients[connectionId].buffer.Length - 4)
                {
                    ClientsManager.clients[connectionId].buffer.ReadInt();
                    data = ClientsManager.clients[connectionId].buffer.ReadBytes(packetLength);
                    HandleDataPackets(connectionId, data);
                }

                packetLength = 0;
                
                if (ClientsManager.clients[connectionId].buffer.Length >= 4)
                {
                    packetLength = ClientsManager.clients[connectionId].buffer.ReadInt(false);
                    if (packetLength <= 0)
                    {
                        ClientsManager.clients[connectionId].buffer.Clear();
                        return;
                    }
                }
            }

            if (packetLength <= 1)
            {
                ClientsManager.clients[connectionId].buffer.Clear();
            }
        }

        private static void HandleDataPackets(int connectionId, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.Write(data);
            int packedId = buffer.ReadInt();
            buffer.Dispose();

            if (packets.TryGetValue(packedId, out Packet packet))
            {
                packet.Invoke(connectionId, data);
            }
        }
    }
}