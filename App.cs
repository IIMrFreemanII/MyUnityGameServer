using System;

namespace MyUnityGameServer
{
    public static class App
    {
        public static void InitializeServer()
        {
            UnityServer.InitializeNetwork();
            Console.WriteLine("Server has been started!");
        }
    }
}