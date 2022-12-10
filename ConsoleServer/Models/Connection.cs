using System.Net.Sockets;

namespace ConsoleServer.Models
{
    public class Connection
    {
        public Guid Token { get; set; }
        public Socket? Socket { get; set; }
        public string UserName { get; set; }
        
        public Connection()
        {
            this.Token = Guid.NewGuid();
            this.Socket = null;
            this.UserName = "";
        }
    }
}
