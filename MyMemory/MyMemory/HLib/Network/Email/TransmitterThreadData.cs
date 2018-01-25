// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransmitterThreadData.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the TransmitterThreadData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Network.Email
{

    using System.Collections.Concurrent;
    using System.Threading;

    using HLib.Logging;


    public class TransmitterThreadData
    {
        public BlockingCollection<IEmail> Queue { get; set; }

        public ILoggable Logger { get; set; }

        public CancellationToken ExitCancelationToken { get; set; }
    }
}