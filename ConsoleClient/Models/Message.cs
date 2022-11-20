using System.Runtime.Serialization.Formatters.Binary;
using ConsoleClient;
using ConsoleClient.Models;
using ConsoleClient.Services;

namespace ConsoleClient.Models
{
    [Serializable]
    public class Message
    {
        public byte MessageType;

        public byte [] MessageData;
        public byte[] MessageGetBytes()
        {
            return DataSerialize.Serialize(this);
        }
    }
}
