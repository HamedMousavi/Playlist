// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Abstract.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the TransmitState type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network.Email
{

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Security;


    public enum TransmitState
    {
        NotSent,
        Pending,
        Sending,
        SendSuccessful,
        SendFailed,
        GotReply
    }


    public interface IServerSettings
    {
        int Port { get; set; }

        string Address { get; set; }

        bool SupportsSsl { get; set; }
    }


    public interface IRecepiant : INotifyPropertyChanged
    {
        string Address { get; set; }

        TransmitState TransmitState { get; set; }
    }


    public interface IRecepiantSettings
    {
        ObservableCollection<IRecepiant> RecepiantList { get; set; }
    }


    public interface ISenderSettings
    {
        string Name { get; set; }

        string Address { get; set; }

        SecureString Password { get; set; }
    }


    public interface IMessage
    {
        string Subject { get; set; }

        string Body { get; set; }

        IList<FileAttachment> Attachments { get; }
    }


    public interface IEmail : INotifyPropertyChanged
    {
        IServerSettings ServerSettings { get; set; }

        ISenderSettings SenderSettings { get; set; }

        IRecepiantSettings RecepiantSettings { get; set; }

        IMessage Message { get; set; }
    }


    public interface IEmailTransmitter : IDisposable
    {
        bool Transmit(IEmail email);
    }
}
