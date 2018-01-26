using System.Collections.Generic;
using System.IO;
using System.Text;


namespace MyMemory.Domain
{

    public class DirectoryListStore : IDirListSaver, IDirListLoader
    {

        private readonly ISerializer<string> _serializer;
        private readonly string _storagePath;


        public DirectoryListStore(string storagePath, ISerializer<string> serializer)
        {
            _storagePath = storagePath;
            _serializer = serializer;
        }


        private void SaveContent(string content)
        {
            using (var sw = new StreamWriter(_storagePath, false, Encoding.UTF8))
            {
                sw.Write(content);
                sw.Close();
            }
        }


        private string LoadContent()
        {
            if (!File.Exists(_storagePath)) return null;
            using (var sr = new StreamReader(_storagePath, Encoding.UTF8))
            {
                var content = sr.ReadToEnd();
                sr.Close();

                return content;
            }
        }



        public void Save(IDirListState list)
        {
            SaveContent(_serializer.Serialize(list));
        }

        public IDirListState Load()
        {
            return _serializer.Deserialize<DirectoryListState>(LoadContent());
        }
    }


    public class DirList : IDirList
    {

        private readonly IDirListLoader _loader;
        private readonly IDirListSaver _saver;
        private List<DirectoryItem> _directories;


        public DirList(IDirListLoader loader, IDirListSaver saver)
        {
            _loader = loader;
            _saver = saver;
        }


        public void Save()
        {
            _saver?.Save(new DirectoryListState(_directories));
        }


        public void Load()
        {
            _directories = _loader?.Load()?.Items;
        }


        public void Save(IDirListSaver saver)
        {
            saver?.Save(new DirectoryListState(_directories));
        }


        public void Load(IDirListLoader loader)
        {
            _directories = loader?.Load().Items;
        }


        public void Add(string dirTitle, string dirPath)
        {
            if (_directories == null) _directories = new List<DirectoryItem>();
            _directories.Add(new DirectoryItem(dirTitle, dirPath));
        }
    }


    public class DirectoryListState : IDirListState
    {

        public DirectoryListState(List<DirectoryItem> directories)
        {
            Items = directories;
        }

        public List<DirectoryItem> Items { get; set; }
    }

    public class DirectoryItem
    { 

        public DirectoryItem(string dirTitle, string dirPath)
        {
            Name = dirTitle;
            Path = dirPath;
        }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}
