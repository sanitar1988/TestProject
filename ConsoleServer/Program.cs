
using ConsoleServer.Services;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Server start...");
            Console.WriteLine("Task count: " + ThreadPool.ThreadCount);
            Console.Write("Enter port server: ");
            int serverport = Convert.ToInt32(Console.ReadLine());

            SocketServer server = new();
            server.StartServerAsync(serverport);
            server.ListenAsync();

            Console.Write("Press enter for exit: ");
            Console.ReadLine();
        }
    }
}