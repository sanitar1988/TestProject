using ConsoleClient.Models;
using ConsoleClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestProject;

namespace ConsoleClient.Models
{
    public class MessageTypeMetods
    {
        public static void UserConnected(Socket someSocket, byte[] iMDataBytes)
        {
            string ServerSay = JsonSerializer.Deserialize<string>(iMDataBytes);
            PrintClass.PrintConsole("Server say " + ServerSay);
            Program.Registation();
        }

        public static void UserDisconnected(Socket someSocket, byte[] iMDataBytes)
        {
            //string SaySomeSocket = JsonSerializer.Deserialize<string>(someData);
            //PrintClass.PrintConsole("SomeSocket say " + SaySomeSocket);
            //PrintClass.PrintConsole("SomeSocket " + someSocket.RemoteEndPoint + " disconnected!!!");

            //someSocket.Disconnect(false);
            //someSocket.Shutdown(SocketShutdown.Both);
            //someSocket.Close();
            //someSocket.Dispose();
        }
        public static void UserMessage(byte[] iMDataBytes)
        {
            //UserFirstInfo UserFirstInfo = JsonSerializer.Deserialize<UserFirstInfo>(someData);
            //string FileName = @"\servertest.jpg";
            //WriteBytesFile(FileName, UserFirstInfo.UserAvatar);

            string SaySomeSocket = JsonSerializer.Deserialize<string>(iMDataBytes);
            PrintClass.PrintConsole("Server say " + SaySomeSocket);
        }

        public static void UserRegistration(Connection someConnection, byte[] iMDataBytes)
        {
            string ServerSay = JsonSerializer.Deserialize<string>(iMDataBytes);
            PrintClass.PrintConsole("Server say " + ServerSay);
            Program.SendMessages();
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
