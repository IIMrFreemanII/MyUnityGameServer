using System;
using System.Net.Sockets;

namespace MyUnityGameServer
{
    public class Client
    {
        public int connectionId;
        public string clientName;
        public TcpClient socket;
        public NetworkStream stream;
        private byte[] receiveBuffer;
        public ByteBuffer buffer;

        public void Start()
        {
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            stream = socket.GetStream();
            receiveBuffer = new byte[4096];
            stream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, OnReceiverData, null);

            Console.WriteLine($"Incoming connection from {socket.Client.RemoteEndPoint}");
        }

        private void OnReceiverData(IAsyncResult result)
        {
            try
            {
                int length = stream.EndRead(result);

                if (length <= 0)
                {
                    CloseConnection();
                    return;
                }

                byte[] newBytes = new byte[length];
                Array.Copy(receiveBuffer, newBytes, length);
                
                //Handle data here.
                ServerHandleData.HandleData(connectionId, newBytes);
                //
                
                stream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, OnReceiverData, null);
            }
            catch
            {
                CloseConnection();
            }
        }

        private void CloseConnection()
        {
            Console.WriteLine($"Connection from {socket.Client.RemoteEndPoint} has been terminated.");
            socket.Close();
        }
    }
}