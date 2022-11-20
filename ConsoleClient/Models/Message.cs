namespace ConsoleClient.Models
{
    [Serializable]
    public class Message
    {                       
        public byte MessageType { get; set; }
        public object MessageData { get; set; }
    } 
}
