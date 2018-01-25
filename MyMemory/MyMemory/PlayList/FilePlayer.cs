using System;
using System.IO;


namespace MyMemory
{

    public class FilePlayer : IPlayListItemPlayer
    {

        private readonly string _basePath;


        public FilePlayer(string basePath)
        {
            _basePath = basePath;
        }

        public void Play<T>(T fileName)
        {
            System.Diagnostics.Process.Start($"{_basePath}{Path.DirectorySeparatorChar}{fileName}");
            OnWhenPlayed($"fileName");
        }

        public event EventHandler WhenPlayed;

        public class FilePlayedEventArgs : EventArgs
        {
            public FilePlayedEventArgs(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; }
        }

        protected virtual void OnWhenPlayed(string filename)
        {
            WhenPlayed?.Invoke(this, new FilePlayedEventArgs(filename));
        }
    }
}
