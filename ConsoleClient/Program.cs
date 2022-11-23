using ConsoleClient.Models;
using ConsoleClient.Services;
using System.Net.Http.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestProject
{
    class Program
    {
        public static SocketClient client = new();

        static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            Console.WriteLine("Client start...");

            Console.Write("Enter ipaddress or domain server: ");
            string serveradd = Console.ReadLine();

            Console.Write("Enter port server: ");
            int serverport = Convert.ToInt32(Console.ReadLine());

            client.Connection(serveradd, serverport);

            client.ListenServerAsync();

            Console.WriteLine("User registarion: ");

            UserFirstInfo userFirstInfo = new();
            Console.WriteLine("Enter user name: ");
            //userFirstInfo.Username = Console.ReadLine();
            userFirstInfo.UserName = "Andrey";
            Console.WriteLine("Enter user password: ");
            //userFirstInfo.Password = Console.ReadLine();
            userFirstInfo.UserPassword = "12345";
            Console.WriteLine("Enter user email: ");
            //userFirstInfo.Email = Console.ReadLine();
            userFirstInfo.UserEmail = "andrey.kuznetzov@yandex.ru";

            Message message = new(MessageType.Type.UserAuthorization);
            message.SetData(userFirstInfo.UserName);
            message.SetData(userFirstInfo.UserPassword);
            message.SetData(userFirstInfo.UserEmail);

            byte[] encryptmess = Clear3DES.Encrypt(message.ConvertToBytes());

            client.SendMessageAsync(encryptmess);

            Console.Write("Press enter for exit: ");
            Console.ReadLine();
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Message message = new Message(MessageType.Type.UserDisconnected);
            message.SetData("Byby");

            byte[] encryptmess = Clear3DES.Encrypt(message.ConvertToBytes());
            client.SendMessageAsync(encryptmess);
        }

    }
}