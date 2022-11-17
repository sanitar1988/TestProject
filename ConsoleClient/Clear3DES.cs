using System.Security.Cryptography;
using System.Text;

namespace ConsoleClient
{
    public  class Clear3DES
    {
        public static byte [] Encrypt(string TextToEncrypt)
        {
            RandomNumberGenerator rNG = RandomNumberGenerator.Create();
            byte[] salt = new byte[30];
            rNG.GetBytes(salt, 0, salt.Length);

            byte[] clearBytes = Encoding.UTF8.GetBytes(TextToEncrypt);

            MD5 md5 = MD5.Create();
            TripleDES des = TripleDES.Create();
            
            var desKey = md5.ComputeHash(new byte[] { salt[24], salt[7], salt[19], salt[8], salt[9] });

            string s = "";
            for (int i = 0; i < desKey.Length; i++)
            {
                s += desKey[i];
            }
            PrintClass.PrintConsole(s);

            des.Key = desKey;
            des.KeySize = 128;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;

            ICryptoTransform en = des.CreateEncryptor();
            byte[] resultArray = en.TransformFinalBlock(clearBytes, 0, clearBytes.Length);

            
            byte[] sendBytes = new byte[salt.Length + resultArray.Length];
            salt.CopyTo(sendBytes, 0);
            resultArray.CopyTo(sendBytes, 30);

            s = "\n\n\n";
            for (int i = 0; i < sendBytes.Length; i++)
            {
                s += sendBytes[i];
            }
            PrintClass.PrintConsole(s);

            return sendBytes;
        }
        public static string Decrypt(byte [] TextToDecrypt)
        {
            MD5 md5 = MD5.Create();
            TripleDES des = TripleDES.Create();
            var desKey = md5.ComputeHash(new byte[] { TextToDecrypt[24], TextToDecrypt[7], TextToDecrypt[19], TextToDecrypt[8], TextToDecrypt[9] });
            
            string s = "";
            for (int i = 0; i < desKey.Length; i++)
            {
                s += desKey[i];
            }
            PrintClass.PrintConsole(s);

            des.Key = desKey;
            des.KeySize = 128;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;

            byte[] resultArray = new byte[TextToDecrypt.Length - 30];

            Array.Copy(TextToDecrypt, 30, resultArray, 0, resultArray.Length);

            ICryptoTransform de = des.CreateDecryptor();

            byte[] DEresultArray = de.TransformFinalBlock(resultArray, 0, resultArray.Length);

            return Encoding.UTF8.GetString(DEresultArray, 0, DEresultArray.Length);
        }
    }
}
