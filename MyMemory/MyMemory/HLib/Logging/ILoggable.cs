// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILoggable.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the ILoggable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Logging
{

    using System;


    public interface ILoggable
    {
        void LogException(Exception exception);

        void LogError(string errorMessage);

        void LogEvent(string eventMessage);

        void LogFromStream(System.IO.StreamReader stream);
    }
}
