// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Interfaces.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the ICryptStrategy type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Security.Crypt
{

    using System.Security;


    public interface ICryptStrategy
    {
        string Encrypt(string plainText);

        string Decrypt(string cipherText);
    }


    public interface ISecureCryptStrategy
    {
        string Encrypt(SecureString plainText);

        SecureString Decrypt(string cipherText);
    }
}
