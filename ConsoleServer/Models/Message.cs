using System.Runtime.Serialization.Formatters.Binary;
using ConsoleServer;
using ConsoleServer.Models;
using ConsoleServer.Services;

namespace ConsoleServer.Models
{
    [Serializable]
    public class Message
    {
        public byte MessageType;

        public byte [] MessageData = new byte[1];
        public byte[] MessageGetBytes()
        {
            return DataSerialize.Serialize(this);
        }
    }
}
