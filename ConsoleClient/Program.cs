using ConsoleClient;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Client start...");

            Console.Write("Enter ipaddress or domain server: ");
            string serveradd = Console.ReadLine();

            Console.Write("Enter port server: ");
            int serverport = Convert.ToInt32(Console.ReadLine());

            Client client = new();
            client.Connection(serveradd, serverport);

            client.ListenServerAsync();

            for (int i = 0; i < 1000; i++)
            {
                Console.Write("Enter message: ");
                string mess = Console.ReadLine();
                client.SendMessageAsync(mess);
            }

            Console.Write("Press enter for exit: ");
            Console.ReadLine();
        }

    }
}