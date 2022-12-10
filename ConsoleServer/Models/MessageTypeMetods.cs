using ConsoleServer.Services;
using System.Net.Sockets;
using System.Text.Json;
using TestProject;

namespace ConsoleServer.Models
{
    public class MessageTypeMetods
    {
        public static void UserConnected(Socket someSocket, byte[] iMDataBytes)
        {
            string SaySomeSocket = JsonSerializer.Deserialize<string>(iMDataBytes);
            PrintClass.PrintConsole("Client say " + SaySomeSocket);

            OutgoingMessage OMMessage = new();
            OMMessage.SetType(MessageType.Type.UserConnected);
            OMMessage.SetData("WellCUM to the server, buddy!!!");
            byte[] encryptMessage = Clear3DES.Encrypt(OMMessage.GetDataBytes());

            Program.server.SendDataAsync(someSocket, encryptMessage);
        }

        public static void UserDisconnected(Connection someConnection, byte[] iMDataBytes)
        {
            string SaySomeSocket = JsonSerializer.Deserialize<string>(iMDataBytes);
            PrintClass.PrintConsole("Client " + someConnection.UserName + " say " + SaySomeSocket);
            PrintClass.PrintConsole("SomeSocket " + someConnection.UserName + " disconnected!!!");

            Program.server.OnlineConnection.DeleteConnection(someConnection.Token);
        }
        public static void UserMessage(Connection someConnection, byte[] iMDataBytes)
        {
            //UserFirstInfo UserFirstInfo = JsonSerializer.Deserialize<UserFirstInfo>(someData);
            //string FileName = @"\servertest.jpg";
            //WriteBytesFile(FileName, UserFirstInfo.UserAvatar);

            string SaySomeSocket = JsonSerializer.Deserialize<string>(iMDataBytes);
            PrintClass.PrintConsole("Client " + someConnection.UserName + " say " + SaySomeSocket);

            OutgoingMessage OMMessage = new();
            OMMessage.SetType(MessageType.Type.UserMessage);

            if (SaySomeSocket.Contains("hi") || SaySomeSocket.Contains("Hi")) SaySomeSocket = "I heard you, fagot!!!";
            if (SaySomeSocket.Contains("fuck") || SaySomeSocket.Contains("Fuck")) SaySomeSocket = "Your mom used to talk in bed too!";
            
            OMMessage.SetData(SaySomeSocket);
            
            byte[] encryptMessage = Clear3DES.Encrypt(OMMessage.GetDataBytes());

            Program.server.SendDataAsync(someConnection.Socket, encryptMessage);
        }

        public static void UserRegistration(Connection someConnection, byte[] iMDataBytes)
        {
            string UserName = JsonSerializer.Deserialize<string>(iMDataBytes);
            someConnection.UserName = UserName;

            PrintClass.PrintConsole("Client " + someConnection.UserName + " registration!");
            PrintClass.PrintConsole("SomeSocket " + someConnection.UserName + " connected!!!");
            Program.server.OnlineConnection.AddConnection(someConnection);

            OutgoingMessage OMMessage = new();
            OMMessage.SetType(MessageType.Type.UserRegistration);
            OMMessage.SetData("Registation ok!!!");
            byte[] encryptMessage = Clear3DES.Encrypt(OMMessage.GetDataBytes());

            Program.server.SendDataAsync(someConnection.Socket, encryptMessage);
        }

        private static async void WriteBytesFile(string fileName, byte[] iMDataBytes)
        {
            string FilePath = Directory.GetCurrentDirectory() + fileName;
            // запись в файл
            using (FileStream FileStream = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                // запись массива байтов в файл
                await FileStream.WriteAsync(iMDataBytes, 0, iMDataBytes.Length);
            }
        }

    }
}
