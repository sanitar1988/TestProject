using ConsoleClient;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace TestProject
{
    class Program
    {
        public static Client client = new();
        static void Main()
        {

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            //Console.WriteLine("Client start...");

            //Console.Write("Enter ipaddress or domain server: ");
            //string serveradd = Console.ReadLine();

            //Console.Write("Enter port server: ");
            //int serverport = Convert.ToInt32(Console.ReadLine());

            //client.Connection(serveradd, serverport);

            //client.ListenServerAsync();

            //for (int i = 0; i < 1000; i++)
            //{
            //    Console.Write("Enter message: ");
            //    string mess = Console.ReadLine();
            //    client.SendMessageAsync(mess);
            //}

            Console.Write("Press enter for exit: ");
            Console.ReadLine();
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            string mess = "Byby";
            //byte[] b = new byte[mess.Length + 1];
            //b[0] = (byte)MessageType.EventCommand.UserDisconnected;

            //byte[] m = Encoding.UTF8.GetBytes(mess);
            //Array.Copy(b, 1, m, 0, m.Length);

            //mess = Encoding.UTF8.GetString(b);
            //client.SendMessageAsync(mess);
            Thread.Sleep(1000);
            //client.SendMessageAsync(mess);
        }

    }
}