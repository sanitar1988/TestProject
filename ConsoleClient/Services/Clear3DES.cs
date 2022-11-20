using System.Security.Cryptography;
using System.Text;

namespace ConsoleClient.Services
{
    public class Clear3DES
    {
        public static byte[] Encrypt(byte[] DataToEncrypt)
        {
            RandomNumberGenerator rNG = RandomNumberGenerator.Create();
            byte[] salt = new byte[30];
            rNG.GetBytes(salt, 0, salt.Length);

            MD5 md5 = MD5.Create();
            var desKey = md5.ComputeHash(new byte[] { salt[24], salt[7], salt[19], salt[8], salt[9] });
            md5.Clear();

            TripleDES des = TripleDES.Create();
            des.KeySize = 128;
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;

            ICryptoTransform en = des.CreateEncryptor();
            byte[] resultArray = en.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);

            byte[] sendBytes = new byte[salt.Length + resultArray.Length];

            salt.CopyTo(sendBytes, 0);
            resultArray.CopyTo(sendBytes, 30);

            return sendBytes;
        }
        public static byte[] Decrypt(byte[] DataToDecrypt)
        {
            MD5 md5 = MD5.Create();
            var desKey = md5.ComputeHash(new byte[] { DataToDecrypt[24], DataToDecrypt[7], DataToDecrypt[19], DataToDecrypt[8], DataToDecrypt[9] });
            md5.Clear();

            TripleDES des = TripleDES.Create();
            des.KeySize = 128;
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;

            byte[] resultArray = new byte[DataToDecrypt.Length - 30];

            Array.Copy(DataToDecrypt, 30, resultArray, 0, resultArray.Length);

            ICryptoTransform de = des.CreateDecryptor();

            byte[] sendBytes = de.TransformFinalBlock(resultArray, 0, resultArray.Length);

            return sendBytes;
        }
    }
}
