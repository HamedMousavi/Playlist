// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AesData.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the AesData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HLib.Security.Crypt
{
    public class InsecureAesData
    {
        public byte[] Key { get; set; }

        public byte[] IV { get; set; }

        public byte[] CipherBytes { get; set; }

        public byte[] PlainBytes { get; set; }
    }
}
