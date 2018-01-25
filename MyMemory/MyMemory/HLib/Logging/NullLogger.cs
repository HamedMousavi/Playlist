// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullLogger.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the NullLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Logging
{

    using System;


    public class NullLogger : ILoggable
    {

        public void LogException(Exception exception)
        {
            Append(string.Format("NULL LOGGER: {0}", exception.ToString()));
        }


        public void LogError(string errorMessage)
        {
            Append(string.Format("NULL LOGGER: {0}", errorMessage));
        }


        public void LogEvent(string eventMessage)
        {
            Append(string.Format("NULL LOGGER: {0}", eventMessage));
        }


        public void LogFromStream(System.IO.StreamReader stream)
        {
            while (true)
            {
                string line = stream.ReadLine();
                if (line == null) break;

                Append(line);
            }
        }


        private void Append(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
            //System.Windows.MessageBox.Show(message);
        }
    }
}
