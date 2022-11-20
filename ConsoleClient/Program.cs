using ConsoleClient.Models;
using ConsoleClient.Services;

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
            userFirstInfo.Username = Console.ReadLine();
            Console.WriteLine("Enter user password: ");
            userFirstInfo.Password = Console.ReadLine();
            Console.WriteLine("Enter user email: ");
            userFirstInfo.Email = Console.ReadLine();

            Message message = new();
            message.MessageType = (byte)MessageType.Type.UserAuthorization;
            message.MessageData = DataSerialize.Serialize(userFirstInfo);

            byte[] encryptmess = Clear3DES.Encrypt(message.MessageData);

            client.SendMessageAsync(encryptmess);

            Console.Write("Press enter for exit: ");
            Console.ReadLine();
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Message message = new Message();
            message.MessageType = (byte)MessageType.Type.UserDisconnected;
            message.MessageData = DataSerialize.Serialize("Byby");

            byte[] encryptmess = Clear3DES.Encrypt(message.MessageData);
            client.SendMessageAsync(encryptmess);
        }

    }
}