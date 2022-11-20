
using System.Text;
using System.Text.Json;

namespace ConsoleClient.Services
{
    public class DataSerialize
    {
        public static byte[] Serialize(object Data)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Data));
        }

        public static object Deserialize(byte[] Data)
        {
            return JsonSerializer.Deserialize(Data, typeof(object));
        }
    }
}
