using System.Net;
using System.Net.Sockets;
using ConsoleServer.Models;

namespace ConsoleServer.Services
{
    public class SocketServer
    {
        private readonly Socket _Server;
        private Socket _FirstSoket;
        private Connection _FirstConnection;

        public OnlineConnections OnlineConnection;

        public SocketServer()
        {
            _Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _Server.SendBufferSize = 1024 * 1024 * 2;
            _Server.ReceiveBufferSize = 1024 * 1024 * 2;
            _FirstConnection = new Connection();
            OnlineConnection = new OnlineConnections();
        }
        
        public void PrepareServer(int serverPort)
        {
            try
            {
                IPEndPoint IPEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
                _Server.Bind(IPEndPoint);
                _Server.Listen(1);
            }
            catch (Exception ex)
            {
                PrintClass.PrintConsole(ex.Message);
            }
        }

        public void RunSocketServer()
        {
            Task.Run(() => ListenAsync());
            Task.Run(() => ListenDataSocketClientsAsync());
        }

        private async Task ListenAsync()
        {
            while (_Server.IsBound)
            {
                _FirstSoket = await _Server.AcceptAsync();
                _FirstConnection = new Connection
                {
                    Socket = _FirstSoket
                };
            }
        }

        private async Task ListenDataSocketClientsAsync()
        {
            while (_Server.IsBound)
            {
                try
                {
                    if (OnlineConnection.Connections.Count > 0)
                    {
                        foreach (Connection Connection in OnlineConnection.Connections.ToList())
                        {
                            if (Connection.Socket != null && Connection.Socket.Available > 0)
                            {
                                await ReceiveDataAsync(Connection);
                            }
                        }
                    }
                    else
                    {
                        if (_FirstConnection.Socket != null && _FirstConnection.Socket.Available > 0)
                        {
                            await ReceiveDataAsync(_FirstConnection);
                        }
                    }
                }
                catch (InvalidOperationException ex)
                {
                    PrintClass.PrintConsole(ex.Message);
                }
                Task.Delay(1).Wait();
            }
        }
        
        public async void SendDataAsync(Socket someClient, byte[] someData)
        {
            try
            {
                await someClient.SendAsync(someData, SocketFlags.None);
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
                        MessageTypeMetods.UserMessage(someConnection, IncomingMessage.IMDataBytes);
                        break;

                    case MessageType.Type.UserDisconnected:
                        MessageTypeMetods.UserDisconnected(someConnection, IncomingMessage.IMDataBytes);
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
