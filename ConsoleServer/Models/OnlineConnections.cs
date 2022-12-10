using ConsoleServer.Services;

namespace ConsoleServer.Models
{
    public class OnlineConnections
    {
        public List<Connection> Connections { get; set; }

        public OnlineConnections()
        {
            Connections = new List<Connection>();
        }

        public void AddConnection(Connection someConnection)
        {
            foreach (Connection Connection in Connections.ToList())
            {
                if (Connection.Token == someConnection.Token)
                {
                    PrintClass.PrintConsole("Error add session!!! Session is have! ");
                }
            }

            Connections.Add(someConnection);
        }

        public void DeleteConnection(Guid token)
        {
            Connection ConnectionToDelete = null;

            foreach (Connection Connection in Connections.ToList())
            {
                if (Connection.Token == token)
                {
                    ConnectionToDelete = Connection;
                }
            }

            if (ConnectionToDelete == null)
            {
                PrintClass.PrintConsole("Error remove session! Session is not faund!");
            }

            Connections.Remove(ConnectionToDelete);
        }
    }
}
