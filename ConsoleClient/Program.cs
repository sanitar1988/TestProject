using ConsoleClient.Models;
using ConsoleClient.Services;
using System.IO;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace TestProject
{
    class Program
    {
        public static SocketClient client = new();

        static async Task Main()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            Console.WriteLine("Client start...");

            //Console.Write("Enter ipaddress or domain server: ");
            //string serveradd = Console.ReadLine();
            string ServerAddress = "192.168.1.65";

            //Console.Write("Enter port server: ");
            //int serverport = Convert.ToInt32(Console.ReadLine());
            int ServerPort = 5555;

            Console.Write("Please press Enter to connect");
            Console.ReadLine();

            client.ConnectionAsync(ServerAddress, ServerPort);
            client.RunSocketClientAsync();


            OutgoingMessage OMMessage = new();
            OMMessage.SetType(MessageType.Type.UserConnected);
            OMMessage.SetData("I em connect");
            byte[] encryptMessage = Clear3DES.Encrypt(OMMessage.GetDataBytes());

            client.SendDataAsync(encryptMessage);

            await Task.Run(() => { while (true) { Task.Delay(1).Wait(); }; });
        }
        public static void Registation()
        {

            Console.Write("Please say your name: ");
            string Name = Console.ReadLine();

            OutgoingMessage OMMessage = new();
            OMMessage.SetType(MessageType.Type.UserRegistration);
            OMMessage.SetData(Name);
            byte[] encryptMessage = Clear3DES.Encrypt(OMMessage.GetDataBytes());

            client.SendDataAsync(encryptMessage);
        }

        public static void SendMessages()
        {
            Console.Write("I em say: ");
            string Say = Console.ReadLine();

            OutgoingMessage OMMessage = new();
            OMMessage.SetType(MessageType.Type.UserMessage);
            OMMessage.SetData(Say);
            byte[] encryptMessage = Clear3DES.Encrypt(OMMessage.GetDataBytes());

            client.SendDataAsync(encryptMessage);

            //for (int i = 0; i < 1000; i++)
            //{
                
            //}
        }

        private static async Task<byte[]> ReadBytesFile(string FileName)
        {
            byte[] Buffer = new byte[1];
            string FilePath = Directory.GetCurrentDirectory() + FileName;
            using (FileStream FileStream = new FileStream(FilePath, FileMode.Open))
            {
                // выделяем массив для считывания данных из файла
                Buffer = new byte[FileStream.Length];
                // считываем данные
                await FileStream.ReadAsync(Buffer, 0, Buffer.Length);
            }
            return Buffer;
        }
        
        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            OutgoingMessage OMMessage = new();
            OMMessage.SetType(MessageType.Type.UserDisconnected);
            OMMessage.SetData("Byby");

            byte[] encryptmess = Clear3DES.Encrypt(OMMessage.GetDataBytes());
            client.SendDataAsync(encryptmess);

            Task.Delay(1000).Wait();
        }
    }
}