namespace MyUnityGameServer
{
    public enum ServerPackets
    {
        ServerWelcomeMessage = 1,
        WelcomeClientWithName,
    }
    
    public static class DataSender
    {
        public static void SendWelcomeMessage(int connectionId)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.Write((int) ServerPackets.ServerWelcomeMessage);
            buffer.Write("Welcome to the Unity server!");
            ClientsManager.SendDataTo(connectionId, buffer.ToArray());
            buffer.Dispose();
        }
        
        public static void SendWelcomeToClientWithName(int connectionId)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.Write((int) ServerPackets.WelcomeClientWithName);
            buffer.Write($"Welcome to the Unity server {ClientsManager.clients[connectionId].clientName}!");
            ClientsManager.SendDataTo(connectionId, buffer.ToArray());
            buffer.Dispose();
        }
    }
}