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
        public static Client client = new();

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
            userFirstInfo.Username = "Andrey";
            Console.WriteLine("Enter user password: ");
            //userFirstInfo.Password = Console.ReadLine();
            userFirstInfo.Userpassword = "12345";
            Console.WriteLine("Enter user email: ");
            //userFirstInfo.Email = Console.ReadLine();
            userFirstInfo.Useremail = "andrey.kuznetzov@yandex.ru";

            Message message = new();
            message.MessageType = (byte)MessageType.Type.UserAuthorization;
            message.MessageData = userFirstInfo;

            byte[] encryptmess = Clear3DES.Encrypt(DataSerialize.Serialize(message));


            //byte[] decryptmess = Clear3DES.Decrypt(encryptmess);
            //Message inmess = (Message)DataSerialize.Deserialize(decryptmess);

            client.SendMessageAsync(encryptmess);

            Console.Write("Press enter for exit: ");
            Console.ReadLine();
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Message message = new Message();
            message.MessageType = (byte)MessageType.Type.UserDisconnected;
            message.MessageData = "Byby";

            PrintClass.PrintConsole("AAAAAAAAAAAAAAAA");

            byte[] encryptmess = Clear3DES.Encrypt(DataSerialize.Serialize(message));
            client.SendMessageAsync(encryptmess);
        }

    }
}