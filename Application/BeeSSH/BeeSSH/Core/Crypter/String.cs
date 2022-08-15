using System;
using System.Security.Cryptography;
using System.Text;

namespace BeeSSH.Core.Crypter
{
    internal static class String
    {
        internal static string Encrypt(string text, string key)
        {

            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            RijndaelManaged rijndael = SetRijndaelManaged(key);
            ICryptoTransform transform = rijndael.CreateEncryptor();

            // Erstellt ein Byte Array mit dem verschlüsselten String
            byte[] encryptBytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);

            string newText = Convert.ToBase64String(encryptBytes);
            return newText;
        }

        internal static string Decrypt(string text, string password)
        {

            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CFB;
            rijndaelCipher.Padding = PaddingMode.ISO10126;
            rijndaelCipher.KeySize = 256;
            rijndaelCipher.BlockSize = 128;
            byte[] encryptedData = Convert.FromBase64String(text);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
            var _key = new Rfc2898DeriveBytes(pwdBytes, Salt, 10);

            rijndaelCipher.Key = _key.GetBytes(rijndaelCipher.KeySize / 8); ;
            rijndaelCipher.IV = _key.GetBytes(rijndaelCipher.BlockSize / 8);

            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText;
            try
            {
                plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            }
            catch (Exception)
            {

                plainText = null;
            }

            rijndaelCipher.Clear();
            _key.Dispose();
            return Encoding.UTF8.GetString(plainText);
        }
        internal static byte[] Salt = new byte[16];

        internal static RijndaelManaged SetRijndaelManaged(string pass)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged
            {
                Mode = CipherMode.CFB,
                Padding = PaddingMode.ISO10126,
                KeySize = 256,
                BlockSize = 128
            };


            byte[] keyArray = Encoding.UTF8.GetBytes(pass);
            var key = new Rfc2898DeriveBytes(keyArray, Salt, 10);
            rijndaelManaged.Key = key.GetBytes(rijndaelManaged.KeySize / 8);
            rijndaelManaged.IV = key.GetBytes(rijndaelManaged.BlockSize / 8);
            key.Dispose();
            return rijndaelManaged;
        }
    }
}
