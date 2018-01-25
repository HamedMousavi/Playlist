// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Email.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the Email type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network.Email
{

    using Data;
    

    public class Email : NotifyPropertyChanged, IEmail
    {
        
        public Email()
        {
            ServerSettings = new ServerSettings();
            SenderSettings = new SenderSettings();
            RecepiantSettings = new RecepiantSettings();
            Message = new Message();
        }


        public IServerSettings ServerSettings { get; set; }


        public ISenderSettings SenderSettings { get; set; }


        public IRecepiantSettings RecepiantSettings { get; set; }


        public IMessage Message { get; set; }
    }
}
