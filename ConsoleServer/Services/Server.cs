using System.Net;
using System.Net.Sockets;
using System.Text;
using ConsoleClient.Models;
using ConsoleServer.Models;

namespace ConsoleServer.Services
{
    public class Server
    {
        private readonly Socket _server;
        private Dictionary<EndPoint, Socket> _onlineUsers = new Dictionary<EndPoint, Socket>();

        public Server()
        {
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.SendBufferSize = 1024 * 1024;
            _server.ReceiveBufferSize = 1024 * 1024;
        }

        public void StartServerAsync(int Serverport)
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, Serverport);
                _server.Bind(iPEndPoint);
                _server.Listen(100);
                PrintClass.PrintConsole("Server start done!");
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }

        public void AnalyzingData(Socket SomeClient, byte[] ByteMessage)
        {
            try
            {
                byte[] decryptmess = Clear3DES.Decrypt(ByteMessage);
                PrintClass.PrintConsole(Encoding.UTF8.GetString(decryptmess));

                //Message inmess = (Message)DataSerialize.Deserialize(decryptmess);

                //switch (inmess.MessageType)
                //{
                //    case (byte)MessageType.Type.UserAuthorization:

                //        Message outmess = new Message();
                //        outmess.MessageType = (byte)MessageType.Type.UserMessage;
                //        outmess.MessageData = DataSerialize.Serialize("Hello user! WellCUM in server");
                //        byte[] outencryptmess = Clear3DES.Encrypt(outmess.MessageData);
                //        SendMessageAsync(SomeClient, outencryptmess);
                //    break;

                //    case (byte)MessageType.Type.UserDisconnected:
                //        UserDisconnected(SomeClient);
                //    break;
                //}
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }

        private void UserDisconnected(Socket SomeClient)
        {
            PrintClass.PrintConsole("Client " + SomeClient.RemoteEndPoint + " disconnected!!!");
            _onlineUsers.Remove(SomeClient.RemoteEndPoint);
            PrintClass.PrintConsole("OnlineUsers: " + _onlineUsers.Count);
        }

        private void UserConnected(Socket SomeClient)
        {
            PrintClass.PrintConsole("Client " + SomeClient.RemoteEndPoint + " connected!!!");
            _onlineUsers.Add(SomeClient.RemoteEndPoint, SomeClient);
            PrintClass.PrintConsole("OnlineUsers: " + _onlineUsers.Count);
        }

        public async void SendMessageAsync(Socket SomeClient, byte[] ByteMessage)
        {
            try
            {
                await SomeClient.SendAsync(ByteMessage, SocketFlags.None);
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

            while (_server.IsBound)
            {
                try
                {
                    Socket cl = await _server.AcceptAsync();
                    UserConnected(cl);

                    await Task.Factory.StartNew(async () =>
                    {
                        while (cl.Connected)
                        {
                            try
                            {
                                byte[] buffer = new byte[_server.ReceiveBufferSize];
                                int received = await cl.ReceiveAsync(buffer, SocketFlags.None);

                                byte[] responce = new byte[received];
                                Array.Copy(buffer, 0, responce, 0, received);
                                AnalyzingData(cl, responce);

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
