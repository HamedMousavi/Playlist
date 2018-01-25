// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InsecureAesCryptStrategy.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the InsecureAesCryptStrategy type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Security.Crypt
{

    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;


    public class InsecureAesCryptStrategy : ISecureCryptStrategy
    {

        public string Encrypt(SecureString plainText)
        {
            if (plainText == null) return string.Empty;
            var unsecure = ConvertToUnsecureString(plainText);
            if (string.IsNullOrEmpty(unsecure)) return string.Empty;

            var data = new InsecureAesData { PlainBytes = Encoding.UTF8.GetBytes(unsecure) };
            Encrypt(data);

            // UNDONE:
            // This isn't even secure! Maybe just a tad better than plain text
            // How can I store public and IV in a secure way?
            return string.Format(
                "{0}|{1}|{2}",
                Convert.ToBase64String(data.CipherBytes),
                Convert.ToBase64String(data.Key),
                Convert.ToBase64String(data.IV));
        }


        public SecureString Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return null;

            var plainData = cipherText.Split('|');
            if (plainData.Length < 3) return null;

            var data = new InsecureAesData
            {
                CipherBytes = Convert.FromBase64String(plainData[0]),
                Key = Convert.FromBase64String(plainData[1]),
                IV = Convert.FromBase64String(plainData[2])
            };

            Decrypt(data);
            var plain = Encoding.UTF8.GetString(data.PlainBytes);

            unsafe
            {
                fixed (char* passwordChars = plain)
                {
                    var securePassword = new SecureString(passwordChars, plain.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }
        }


        // Borrowed from here: http://blogs.msdn.com/b/fpintos/archive/2009/06/12/how-to-properly-convert-securestring-to-string.aspx
        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }


        private void Encrypt(InsecureAesData data)
        {
            RijndaelManaged aes = null;
            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;

            try
            {
                aes = new RijndaelManaged();
                aes.GenerateKey();
                aes.GenerateIV();

                ICryptoTransform encryptor = aes.CreateEncryptor();

                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                csEncrypt.Write(data.PlainBytes, 0, data.PlainBytes.Length);
                csEncrypt.Close();
                csEncrypt = null;

                data.CipherBytes = msEncrypt.ToArray();
                data.Key = aes.Key;
                data.IV = aes.IV;
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // UNDONE : REPORT EXCEPTION
            }
            finally
            {
                if (csEncrypt != null) csEncrypt.Close();
                if (msEncrypt != null) msEncrypt.Close();

                // Clear the AesManaged object.
                if (aes != null) aes.Clear();
            }
        }


        private void Decrypt(InsecureAesData data)
        {
            RijndaelManaged aes = null;
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;

            try
            {
                aes = new RijndaelManaged();
                aes.Key = data.Key;
                aes.IV = data.IV;

                ICryptoTransform decryptor = aes.CreateDecryptor();

                msDecrypt = new MemoryStream(data.CipherBytes);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                byte[] buffer = new byte[data.CipherBytes.Length];
                int read = csDecrypt.Read(buffer, 0, buffer.Length);

                data.PlainBytes = new byte[read];
                Buffer.BlockCopy(buffer, 0, data.PlainBytes, 0, read);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // UNDONE : REPORT EXCEPTION
            }
            finally
            {
                if (csDecrypt != null) csDecrypt.Close();
                if (msDecrypt != null) msDecrypt.Close();

                // Clear the AesManaged object.
                if (aes != null) aes.Clear();
            }
        }
    }
}

// UNDONE:
// MAYBE LATER WE CAN USE THIS CODE TO INCREASE SECURITY
//
//
//void HandleSecureString(SecureString value){
//  IntPtr valuePtr = IntPtr.Zero;
//  try{
//    valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
//    for(int i=0;i<value.Length;i++){
//      short unicodeChar = Marshal.ReadInt16(valuePtr, i*2);
//      // handle unicodeChar
//    }
//  }
//  finally{
//    Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
//  }
//}

//private string DecryptPassword()
//{
//    if (string.IsNullOrWhiteSpace(_encryptedPass))
//    {
//        if (_cryptStrategyStrategy != null && SecureText != null)
//        {
//            return ConvertToUnsecureString(SecureText);
//        }
//    }

//    return _cryptStrategyStrategy.Decrypt(_encryptedPass);
//}