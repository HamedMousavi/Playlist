using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;


namespace MyMemory
{

    internal class PlayList : INotifyPropertyChanged, IPlayList
    {
        private readonly IPlayListStore _history;

        [JsonProperty]
        private List<PlayListItem> _list;

        [JsonProperty]
        private PlayListItem _current;


        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        private int CurrentIndex
        {
            get
            {
                if (_current == null) SetupList();
                if (_current == null) return 0;

                var index = _list.IndexOf(_list.SingleOrDefault(f => string.Equals(_current.Name, f.Name, System.StringComparison.InvariantCultureIgnoreCase)));

                return index < 0 ? 0 : index;
            }
        }

        [JsonIgnore]
        private PlayListItem Current
        {
            get => _current;
            set
            {
                bool shouldSave, shouldNotify;

                if (value != null && _current != null && string.Equals(_current.Name, value.Name, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    // different objects referring to the same file
                    shouldSave = false;
                    shouldNotify = true;
                }
                else
                {
                    shouldSave = _current != value;
                    shouldNotify = shouldSave;
                }

                _current = value;
                if (shouldSave) SaveTo(_history);
                if (shouldNotify) FireEvents();
            }
        }


        [JsonIgnore]
        public int Count => _list?.Count ?? 0;

        [JsonIgnore]
        public string PlayingFile => _current?.Name;

        [JsonIgnore]
        public int PlayingIndex => CurrentIndex + 1;

        [JsonIgnore]
        public bool IsEmpty => _list == null || !_list.Any();

        public PlayList(IPlayListStore store)
        {
            _history = store;
        }


        //private void Load()
        //{
        //    if (!File.Exists(_storagePath)) return;

        //    using (var sr = new StreamReader(_storagePath, Encoding.UTF8))
        //    {
        //        JsonConvert.PopulateObject(sr.ReadToEnd(), this);
        //        sr.Close();
        //    }
        //}


        //private void Save()
        //{
        //    using (var sw = new StreamWriter(_storagePath, false, Encoding.UTF8))
        //    {
        //        sw.Write(JsonConvert.SerializeObject(this));
        //        sw.Close();
        //    }
        //}


        private void Play(int index)
        {
            SetupList();

            if (IsEmpty) return;
            if (index >= _list.Count || index < 0) index = 0;

            Current = _list[index];

            if (_current != null)
                System.Diagnostics.Process.Start($"{_dirPath}{Path.DirectorySeparatorChar}{_current.Name}");
        }


        private void SetupList()
        {
            if (IsEmpty)
            {
                LoadFrom(_history);

                if (IsEmpty)
                {
                    _list = Directory
                        .EnumerateFiles(_dirPath, "*.*", SearchOption.AllDirectories)
                        .Select(f => new PlayListItem(Path.GetFileName(f)))
                        .ToList();

                    if (!IsEmpty) Current = _list[0];
                }
            }
        }


        internal void Play()
        {
            Play(CurrentIndex);
        }


        internal void PlayNext()
        {
            Play(CurrentIndex + 1);
        }


        internal void PlayPrevious()
        {
            Play(CurrentIndex - 1);
        }


        private void FireEvents()
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Count)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(PlayingFile)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(PlayingIndex)));
        }

        public void LoadFrom(IPlayListStore store)
        {
            store.Load(this);
        }

        public void SaveTo(IPlayListStore store)
        {
            store.Save(this);
        }
    }


    internal class PlayListItem
    {
        public PlayListItem(string name)
        {
            Name = name;
        }

        public string Name { get; }
        //public int Index { get; set; }
        //public bool IsPlaying { get; set; }
    }
}