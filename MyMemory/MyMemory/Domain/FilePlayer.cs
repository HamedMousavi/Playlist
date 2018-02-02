using System;
using System.IO;
using MyMemory.Domain.Abstract;


namespace MyMemory.Domain
{

    public class FilePlayer : IPlaylistItemPlayer
    {

        private readonly string _basePath;


        public FilePlayer(string basePath)
        {
            _basePath = basePath;
        }


        public FilePlayer()
        {
        }


        public void Play<T>(T fileName)
        {
            var path =
                string.IsNullOrWhiteSpace(_basePath)
                    ? $"{fileName}"
                    : Path.Combine(_basePath, $"{fileName}");

            System.Diagnostics.Process.Start(path);
            OnWhenPlayed($"{fileName}");
        }


        #region Events

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

        #endregion
    }
}
