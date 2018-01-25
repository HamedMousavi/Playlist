// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Message.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the Message type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network.Email
{

    using System.Collections.Generic;
    using System.Collections.ObjectModel;


    public class Message : IMessage
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public IList<FileAttachment> Attachments { get; private set; }

        public Message()
        {
            Attachments = new ObservableCollection<FileAttachment>();
        }
    }
}