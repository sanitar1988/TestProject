using ConsoleClient;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TestProject
{
    [Serializable]
    public class User
    {
        public string _name { get; set; }
        public int _year { get; set; }
        public int [] _idFrends { get; set; }
        public MessageType.EventCommand _type { get; set; }

        public User(string name, int year, int coundfreds)
        {
            _name = name;
            _year = year;

            Random r = new Random();
            _idFrends = new int[coundfreds]; 
            for (int i = 0; i < coundfreds; i++)
            {
                _idFrends[i] = r.Next(0, 255);  
            }
            _type = MessageType.EventCommand.RegInfoUser;
        }
    }

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

            BinaryFormatter formatter = new BinaryFormatter();
            User u = new User("Tom", 29, 100);

            byte[] bmass = new byte[1];

            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, u);
            bmass = new byte[ms.ToArray().Length];
            bmass = ms.ToArray();

            ms = new MemoryStream(bmass);
            User newUser = (User)formatter.Deserialize(ms);


            Console.Write("Press enter for exit: ");
            Console.ReadLine();
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            string mess = "Byby";
            Console.WriteLine("Wait 5 sec and close...");
            Thread.Sleep(5000);
            //byte[] b = new byte[mess.Length + 1];
            //b[0] = (byte)MessageType.EventCommand.UserDisconnected;

            //byte[] m = Encoding.UTF8.GetBytes(mess);
            //Array.Copy(b, 1, m, 0, m.Length);

            //mess = Encoding.UTF8.GetString(b);
            //client.SendMessageAsync(mess);;
            //client.SendMessageAsync(mess);
        }

    }
}