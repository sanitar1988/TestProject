using System.Net;
using System.Net.Sockets;
using System.Text;
using ConsoleServer.Models;

namespace ConsoleServer.Services
{
    public class SocketServer
    {
        private readonly Socket Server;
        private Dictionary<EndPoint, Socket> OnlineUsers = new Dictionary<EndPoint, Socket>();
        public SocketServer()
        {
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Server.SendBufferSize = 1024 * 1024;
            Server.ReceiveBufferSize = 1024 * 1024;
        }
        public void StartServerAsync(int serverPort)
        {
            try
            {
                IPEndPoint IPEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
                Server.Bind(IPEndPoint);
                Server.Listen(100);
                PrintClass.PrintConsole("Server start done!");
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }
        public void AnalyzingData(Socket someClient, byte[] byteMessage)
        {
            try
            {
                byte[] Decryptmess = Clear3DES.Decrypt(byteMessage);
                Message IncomingMessage = new Message(MessageType.Type.none);
                string[] ArrayString = IncomingMessage.ConvertToString(Decryptmess);

                switch (IncomingMessage.MessageType)
                {
                    case MessageType.Type.UserAuthorization:

                        Message Message = new Message(MessageType.Type.UserMessage);
                        Message.SetData("Hello user! WellCUM in server");

                        byte[] encryptmess = Clear3DES.Encrypt(Message.ConvertToBytes());
                        SendMessageAsync(someClient, encryptmess);
                        break;

                    case MessageType.Type.UserDisconnected:
                        UserDisconnected(someClient);
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }
        private void UserDisconnected(Socket someClient)
        {
            PrintClass.PrintConsole("Client " + someClient.RemoteEndPoint + " disconnected!!!");
            OnlineUsers.Remove(someClient.RemoteEndPoint);
            PrintClass.PrintConsole("OnlineUsers: " + OnlineUsers.Count);
        }
        private void UserConnected(Socket someClient)
        {
            PrintClass.PrintConsole("Client " + someClient.RemoteEndPoint + " connected!!!");
            OnlineUsers.Add(someClient.RemoteEndPoint, someClient);
            PrintClass.PrintConsole("OnlineUsers: " + OnlineUsers.Count);
        }
        public async void SendMessageAsync(Socket someClient, byte[] byteMessage)
        {
            try
            {
                await someClient.SendAsync(byteMessage, SocketFlags.None);
                PrintClass.PrintConsole("Send message done!");
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }
        public async void ListenAsync()
        {
            PrintClass.PrintConsole("Server listen...");

            while (Server.IsBound)
            {
                try
                {
                    Socket Client = await Server.AcceptAsync();
                    UserConnected(Client);

                    await Task.Factory.StartNew(async () =>
                    {
                        while (Client.Connected)
                        {
                            try
                            {
                                byte[] Buffer = new byte[Server.ReceiveBufferSize];
                                int Received = await Client.ReceiveAsync(Buffer, SocketFlags.None);

                                byte[] Responce = new byte[Received];
                                Array.Copy(Buffer, 0, Responce, 0, Received);
                                AnalyzingData(Client, Responce);

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
                catch (Exception ex)
                {
                    PrintClass.PrintConsole(ex.Message);
                }
            }
        }
    }
}
