using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleServer.Services
{
    public class DataSerialize
    {
        public static byte[] Serialize(object Object)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new();
            formatter.Serialize(ms, Object);
            return ms.ToArray();
        }

        public static object Deserialize(byte[] Data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new(Data);
            return formatter.Deserialize(ms);
        }
    }
}
