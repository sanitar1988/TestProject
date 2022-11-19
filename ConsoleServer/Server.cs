﻿using ConsoleServer;
using System.Net;
using System.Net.Sockets;


namespace TestProject
{
    public  class Server
    {
        private readonly Socket _server;
        private Dictionary<EndPoint, Socket> _onlineUsers;

        public Server()
        {
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.SendBufferSize = 1024 * 1024;
            _server.ReceiveBufferSize = 1024 * 1024;
        }

        public void StartServerAsync(int serverport)
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, serverport);
                _server.Bind(iPEndPoint);
                _server.Listen(100);
                PrintClass.PrintConsole("Server start done!");
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }  
        }

        public async void SendMessageAsync(Socket cl, string message)
        {
            try
            {
                byte[] buffer = Clear3DES.Encrypt(message);
                await cl.SendAsync(buffer, SocketFlags.None);
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
                    PrintClass.PrintConsole("OnlineUsers: " +  _onlineUsers.Count);

                    await Task.Factory.StartNew(async () =>
                    {
                        while (cl.Connected)
                        {
                            
                            try
                            {
                                byte[] buffer = new byte[1024 * 1024];

                                int received = await cl.ReceiveAsync(buffer, SocketFlags.None);
                                
                                byte[] responce = new byte[received];
                                Array.Copy(buffer,0,responce,0,received);

                                SendMessageAsync(cl, Clear3DES.Decrypt(responce));
                                //PrintClass.PrintConsole("Client " + cl.RemoteEndPoint + " say: " + response);
                                PrintClass.PrintConsole("Client taskID: " + Environment.CurrentManagedThreadId);
                                PrintClass.PrintConsole("Task count: " + ThreadPool.ThreadCount); 
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
