using System.Net.Sockets;
using System.Net;
using ConsoleClient;

namespace TestProject
{
    public class Client
    {
        private readonly Socket _client;
        public IPEndPoint _endPoint;
        public Client()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connection(string serveradd, int serverport)
        {
            try
            {
                _endPoint = new IPEndPoint(IPAddress.Parse(serveradd), serverport);
                _client.Connect(serveradd, serverport);
                PrintClass.PrintConsole("Connection done!");
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }

        public async void SendMessageAsync(string message)
        {
            //await Task.Run(async () =>
            //{
            //    Random random = new Random();

            //    while (_client.Connected)
            //    {

                    try
                    {
                        //byte[] buffer = Encoding.UTF8.GetBytes(message);
                        byte[] buffer = Clear3DES.Encrypt(message);
                        await _client.SendAsync(buffer, SocketFlags.None);
                        PrintClass.PrintConsole("Send message done!");
                    }
                    catch (Exception ex)
                    {
                        PrintClass.PrintConsole(ex.Message);
                    }
                    //Task.Delay(random.Next(1000,4000)).Wait();
            //    }
            //});
        }

        public async void ListenServerAsync()
        {
            await Task.Run(async () =>
            {
                while (_client.Connected)
                {
                    try
                    {
                        byte[] buffer = new byte[1024];
                        //var received = await _client.ReceiveAsync(buffer, SocketFlags.None);
                        await _client.ReceiveAsync(buffer, SocketFlags.None);
                        //var response = Encoding.UTF8.GetString(buffer, 0, received);
                        PrintClass.PrintConsole("From server: " + Clear3DES.Decrypt(buffer.Where(x => x != 0).ToArray()));
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
