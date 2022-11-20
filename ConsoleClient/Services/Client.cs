using System.Net.Sockets;
using System.Net;
using ConsoleClient;
using System.IO;
using System.Text;
using ConsoleClient.Services;
using TestProject;
using ConsoleClient.Models;

namespace ConsoleClient.Services
{
    public class Client
    {
        private readonly Socket _client;
        private IPEndPoint _endPoint;
        public Client()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _client.SendBufferSize = 1024 * 1024;
            _client.ReceiveBufferSize = 1024 * 1024;
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


        public void AnalyzingData(byte[] ByteMessage)
        {
            try
            {
                Message mess = (Message)DataSerialize.Deserialize(ByteMessage);
                byte[] decryptmess = Clear3DES.Decrypt(mess.MessageData);

                switch (mess.MessageType)
                {
                    case (byte)MessageType.Type.UserMessage:
                        PrintClass.PrintConsole("From server : " + Encoding.UTF8.GetString(decryptmess));
                    break;
                }
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }

        public async void SendMessageAsync(byte[] ByteMessage)
        {
            try
            {
                await _client.SendAsync(ByteMessage, SocketFlags.None);
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
                while (_client.Connected)
                {
                    try
                    {
                        byte[] buffer = new byte[_client.ReceiveBufferSize];
                        int received = await _client.ReceiveAsync(buffer, SocketFlags.None);

                        byte[] responce = new byte[received];
                        Array.Copy(buffer, 0, responce, 0, received);
                        AnalyzingData(responce);

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
