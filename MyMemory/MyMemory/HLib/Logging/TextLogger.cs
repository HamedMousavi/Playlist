// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextLogger.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the TextLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Logging
{

    using System;
    using System.IO;
    using System.Text;


    public class TextLogger : ILoggable
    {
        private readonly string _dirPath;
        private readonly object _syncLock;
        private string _filePath;
        private DateTime _fileTime;


        public TextLogger(string dirPath)
        {
            _dirPath = dirPath;
            _syncLock = new object();
            UpdateFileName();
        }


        public void LogException(Exception exception)
        {
            Append(exception.ToString());
        }


        public void LogError(string errorMessage)
        {
            Append(errorMessage);
        }


        public void LogEvent(string eventMessage)
        {
            Append(eventMessage);
        }


        public void LogFromStream(StreamReader stream)
        {
            lock (_syncLock)
            {
                UpdateFileName();

                using (var sw = new StreamWriter(_filePath, true, Encoding.Unicode))
                {
                    while (true)
                    {
                        string line = stream.ReadLine();
                        if (line == null) break;

                        sw.WriteLine(line);
                        System.Diagnostics.Trace.WriteLine(line);
                    }

                    sw.Close();
                }
            }

        }


        private void UpdateFileName()
        {
            if (!UpdateIsNeeded()) return;

            _fileTime = DateTime.Now;
            _filePath = Path.Combine(_dirPath, string.Format("{0}.log", _fileTime.ToString("yyyy-MMM-dd")));
        }


        private bool UpdateIsNeeded()
        {
            if (string.IsNullOrWhiteSpace(_filePath))
            {
                return true;
            }

            var diff = DateTime.Now - _fileTime;
            return diff.Days >= 1;
        }


        private void Append(string text)
        {
            //System.Windows.MessageBox.Show(string.Format("{0}\r\n{1}", _filePath, text));
            lock (_syncLock)
            {
                UpdateFileName();

                using (var sw = new StreamWriter(_filePath, true, Encoding.Unicode))
                {
                    sw.WriteLine("[{0}]: {1}", DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"), text);
                    sw.Close();
                }
            }

            System.Diagnostics.Trace.WriteLine(text);
        }
    }
}
