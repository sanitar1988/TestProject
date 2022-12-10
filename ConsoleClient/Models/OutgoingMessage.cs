using System.Text;
using System.Text.Json;

namespace ConsoleClient.Models
{
    public class OutgoingMessage
    {
        private MessageType.Type _OMType;
        private byte[] _OMDataBytes;

        public OutgoingMessage()
        {
            _OMType = MessageType.Type.none;
            _OMDataBytes = new byte[1];
        }

        public void SetType(MessageType.Type Type)
        {
            _OMType = Type;
        }

        public void SetData(object Data)
        {
            byte[] Buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Data));
            _OMDataBytes = new byte[Buffer.Length + 1];
            _OMDataBytes[0] = (byte)_OMType;
            Array.Copy(Buffer, 0, _OMDataBytes, 1, Buffer.Length);
        }

        public byte[] GetDataBytes()
        {
            return _OMDataBytes;
        }
    }

}
