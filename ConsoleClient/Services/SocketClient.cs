using System.Net;
using System.Net.Sockets;
using ConsoleClient.Models;

namespace ConsoleClient.Services
{
    public class SocketClient
    {
        private readonly Socket _Client;
        private IPEndPoint _EndPoint;
        private Connection NewConnection;

        public SocketClient()
        {
            _Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _Client.SendBufferSize = 1024 * 1024 * 2;
            _Client.ReceiveBufferSize = 1024 * 1024 * 2;
            NewConnection = new Connection();
            NewConnection.Socket = _Client;
        }
        
        public async void ConnectionAsync(string serverAddress, int serverPort)
        {
            try
            {
                _EndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);
                await _Client.ConnectAsync(serverAddress, serverPort);
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }
        
        public void RunSocketClientAsync()
        {
            Task.Run(() => ListenDataSocketServerAsync());
        }

        private async Task ListenDataSocketServerAsync()
        {
            if (NewConnection.Socket.Connected)
            { 
                while (NewConnection.Socket.Connected)
                {
                    try
                    {
                        await ReceiveDataAsync(NewConnection); 
                    }
                    catch (Exception ex)
                    {
                        PrintClass.PrintConsole(ex.Message);
                    }
                    Task.Delay(1).Wait();
                };
            }
        }

        public async void SendDataAsync(byte[] someData)
        {
            try
            {
                await NewConnection.Socket.SendAsync(someData, SocketFlags.None);
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }

        public async void SendDataToUserAsync(Socket someClient, byte[] someData)
        {
            try
            {
                await NewConnection.Socket.SendToAsync(someData, SocketFlags.None, someClient.RemoteEndPoint);
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }

        private async Task ReceiveDataAsync(Connection someConnection)
        {
            try
            {
                byte[] ReceiveData = new byte[someConnection.Socket.ReceiveBufferSize];
                int LengthBytes = await someConnection.Socket.ReceiveAsync(ReceiveData, SocketFlags.None);

                if (LengthBytes > 0)
                {
                    byte[] ResponceData = new byte[LengthBytes];
                    Array.Copy(ReceiveData, 0, ResponceData, 0, LengthBytes);

                    byte[] DecryptData = new byte[ResponceData.Length];
                    DecryptData = Clear3DES.Decrypt(ResponceData);

                    IncomingMessage IncomingMessage = new IncomingMessage(DecryptData);

                    SelectMetods(someConnection, IncomingMessage);
                }
            }
            catch (InvalidOperationException ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }

        private void SelectMetods(Connection someConnection, IncomingMessage IncomingMessage)
        {
            try
            {
                switch (IncomingMessage.IMType)
                {
                    case MessageType.Type.UserConnected:
                        MessageTypeMetods.UserConnected(someConnection.Socket, IncomingMessage.IMDataBytes);
                        break;

                    case MessageType.Type.UserRegistration:
                        MessageTypeMetods.UserRegistration(someConnection, IncomingMessage.IMDataBytes);
                        break;

                    case MessageType.Type.UserMessage:
                        MessageTypeMetods.UserMessage(IncomingMessage.IMDataBytes);
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }
    }
}
