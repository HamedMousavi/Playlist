// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SenderSettings.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the SenderSettings type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network.Email
{

    using System.Security;


    public class SenderSettings : ISenderSettings
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public SecureString Password { get; set; }
    }
}
