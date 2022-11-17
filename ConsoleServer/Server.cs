using ConsoleServer;
using System.Net;
using System.Net.Sockets;


namespace TestProject
{
    public  class Server
    {
        private readonly Socket _server;

        public Server()
        {
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
                //byte[] buffer = Encoding.UTF8.GetBytes(message);
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
                    PrintClass.PrintConsole("Client " + cl.RemoteEndPoint + " connected!!!");

                    await Task.Factory.StartNew(async () =>
                    {
                        while (cl.Connected)
                        {
                            
                            try
                            {
                                byte[] buffer = new byte[1024];
                                //var received = await cl.ReceiveAsync(buffer, SocketFlags.None);
                                await cl.ReceiveAsync(buffer, SocketFlags.None);
                                //var response = Encoding.UTF8.GetString(buffer, 0, received);
                                //await Task.Run(() => {


                                //    return Task.CompletedTask;
                                //});

                                //bd job
                                //Task.Delay(2000).Wait();

                                SendMessageAsync(cl, Clear3DES.Decrypt(buffer.Where(x => x != 0).ToArray()));
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
