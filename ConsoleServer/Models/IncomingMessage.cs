namespace ConsoleServer.Models
{
    public class IncomingMessage
    {
        private MessageType.Type _IMType;
        private byte[] _IMDataBytes;

        public MessageType.Type IMType
        {
            get => _IMType;
            set => _IMType = value;
        }

        public byte[] IMDataBytes
        {
            get => _IMDataBytes;
            set => _IMDataBytes = value;
        }

        public IncomingMessage(byte[] Data) 
        {
            _IMType = (MessageType.Type)Data[0];
            byte[] IMDataBytes = new byte[Data.Length - 1];
            Array.Copy(Data, 1, IMDataBytes, 0, Data.Length - 1);
            _IMDataBytes = IMDataBytes;
        }
    }
}
