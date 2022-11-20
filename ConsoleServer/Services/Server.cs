using System.Net;
using System.Net.Sockets;
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


        public void AnalyzingData(Socket SomeClient, byte[] Message)
        {
            try
            {
                //Message message = new();
                //Message newmessage = message.Deserialize(Message);

                //if (Encrypt) await SomeClient.SendAsync(Clear3DES.Encrypt(Message), SocketFlags.None);
                //else await SomeClient.SendAsync(Message, SocketFlags.None);

                //PrintClass.PrintConsole("Send message done!");
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }

        public async void SendMessageAsync(Socket SomeClient, byte[] Message)
        {
            try
            {
                await SomeClient.SendAsync(Message, SocketFlags.None);
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
                    _onlineUsers.Add(cl.RemoteEndPoint, cl);

                    PrintClass.PrintConsole("Client " + cl.RemoteEndPoint + " connected!!!");
                    PrintClass.PrintConsole("OnlineUsers: " + _onlineUsers.Count);

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
