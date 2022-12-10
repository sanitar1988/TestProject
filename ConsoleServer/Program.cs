
using ConsoleServer.Services;

namespace TestProject
{
    class Program
    {
        public static SocketServer server = new();
        static void Main()
        {
            Console.WriteLine("Server start...");
            Console.WriteLine("Task count: " + ThreadPool.ThreadCount);
            Console.Write("Enter port server: ");
            //int ServerPort = Convert.ToInt32(Console.ReadLine());
            int ServerPort = 5555;

            server.PrepareServer(ServerPort);
            server.RunSocketServer();

            Console.Write("Press enter for exit: ");
            Console.ReadLine();
        }
    }
}