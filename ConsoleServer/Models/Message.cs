using System;
using System.Linq;
using System.Text;

namespace ConsoleServer.Models
{
    public class Message
    {
        public MessageType.Type MessageType = 0;
        public int CountRows = 0;


        private byte[] MessageDataBytes = new byte[1];
        private string[] MessageDataStrings = new string[1];
        private List<byte[]> BytesListData = new List<byte[]>();

        public Message(MessageType.Type messageType)
        {
            this.MessageType = messageType;
        }
        public void SetData(string data)
        {
            BytesListData.Add(Encoding.UTF8.GetBytes(data));
        }

        public byte[] ConvertToBytes()
        {
            Stream MemoryStream = new MemoryStream();
            for (int i = 0; i < BytesListData.Count; i++)
            {
                MemoryStream.Write(BytesListData[i], 0, BytesListData[i].Length);
            }

            MemoryStream.Seek(0, System.IO.SeekOrigin.Begin);

            MessageDataBytes = new byte[MemoryStream.Length + (BytesListData.Count * 4) + 5];
            
            MessageDataBytes[0] = (byte)MessageType;
            byte[] MassCountRows = BitConverter.GetBytes(BytesListData.Count);

            Array.Copy(MassCountRows, 0, MessageDataBytes, 1, MassCountRows.Length);

            int j = 5;
            for (int i = 0; i < BytesListData.Count; i++)
            {
                byte[] MassListLength = BitConverter.GetBytes(BytesListData[i].Length);
                Array.Copy(MassListLength, 0, MessageDataBytes, j, MassListLength.Length);
                j += MassListLength.Length;
            }

            j = (BytesListData.Count * 4) + 5;
            for (int i = 0; i < BytesListData.Count; i++)
            {
                Array.Copy(BytesListData[i], 0, MessageDataBytes, j, BytesListData[i].Length);
                j += BytesListData[i].Length;
            }

            return MessageDataBytes;
        }

        public string[] ConvertToString(byte[] data)
        {
            MessageType = (MessageType.Type)data[0];
            CountRows = BitConverter.ToInt32(new byte[] { data[1], data[2], data[3], data[4] });
            List<int> RowsLength = new List<int>();

            int t = 5;
            byte[] MassRowsLength = new byte[1];
            for (int i = 0; i < CountRows; i++)
            {
                MassRowsLength = new byte[] { data[t], data[t + 1], data[t + 2], data[t + 3]};
                RowsLength.Add(BitConverter.ToInt32(MassRowsLength));
                t += MassRowsLength.Length;
            }

            List<string> ListDataStrings = new List<string>();
            int j = 0;
            for (int i = (CountRows * 4) + 5; i < data.Length;)
            {
                byte[] MassDataString = new byte[RowsLength[j]];
                Array.Copy(data, i, MassDataString, 0, RowsLength[j]);
                ListDataStrings.Add(Encoding.UTF8.GetString(MassDataString));
                i += RowsLength[j];
                j++;
            }
            MessageDataStrings = new string[ListDataStrings.Count];
            for (int i = 0; i < ListDataStrings.Count; i++)
            {
                MessageDataStrings[i] = ListDataStrings[i];
            }
            return MessageDataStrings;
        }

    }
} 
