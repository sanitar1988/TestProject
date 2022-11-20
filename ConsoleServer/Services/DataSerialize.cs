using System.Runtime.Serialization.Formatters.Binary;
namespace ConsoleServer.Services
{
    public class DataSerialize
    {
        [Obsolete]
        public static byte[]? Serialize(Object obj)
        {
            if (obj == null) return null;
            BinaryFormatter bf = new();
            MemoryStream ms = new();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        [Obsolete]
        public static Object Deserialize(byte[] arrBytes)
        {
            MemoryStream memStream = new();
            BinaryFormatter binForm = new();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
    }
}
