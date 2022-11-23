using System.Net;
using System.Net.Sockets;
using System.Text;
using ConsoleClient.Models;

namespace ConsoleClient.Services
{
    public class SocketClient
    {
        private readonly Socket Client;
        private IPEndPoint EndPoint;
        public SocketClient()
        {
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Client.SendBufferSize = 1024 * 1024;
            Client.ReceiveBufferSize = 1024 * 1024;
        }
        public void Connection(string serverAddress, int serverPort)
        {
            try
            {
                EndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
                Client.Connect(serverAddress, serverPort);
                PrintClass.PrintConsole("Connection done!");
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }
        public void AnalyzingData(byte[] byteMessage)
        {
            try
            {
                byte[] Decryptmess = Clear3DES.Decrypt(byteMessage);
                Message IncomingMessage = new Message(MessageType.Type.none);
                string[] ArrayString = IncomingMessage.ConvertToString(Decryptmess);

                switch (IncomingMessage.MessageType)
                {
                    case MessageType.Type.UserMessage:
                        PrintClass.PrintConsole("From server : " + ArrayString[0]);
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }
        public async void SendMessageAsync(byte[] byteMessage)
        {
            try
            {
                await Client.SendAsync(byteMessage, SocketFlags.None);
                PrintClass.PrintConsole("Send message done!");
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }
        public async void ListenServerAsync()
        {
            await Task.Run(async () =>
            {
                while (Client.Connected)
                {
                    try
                    {
                        byte[] Buffer = new byte[Client.ReceiveBufferSize];
                        int Received = await Client.ReceiveAsync(Buffer, SocketFlags.None);

                        byte[] Responce = new byte[Received];
                        Array.Copy(Buffer, 0, Responce, 0, Received);
                        AnalyzingData(Responce);

                        //PrintClass.PrintConsole("Client taskID: " + Environment.CurrentManagedThreadId);
                        //PrintClass.PrintConsole("Task count: " + ThreadPool.ThreadCount);
                    }
                    catch (Exception ex)
                    {
                        PrintClass.PrintConsole(ex.Message);
                    }
                }
            });
        }
    }
}
