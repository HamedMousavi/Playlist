using System.IO;
using System.Text;


namespace MyMemory.Domain
{

    public class FileCache : IPlayListSaver, IPlayListLoader
    {

        private readonly ISerializer<string> _serializer;
        private readonly string _storagePath;


        public FileCache(string storagePath, ISerializer<string> serializer)
        {
            _serializer = serializer;
            _storagePath = storagePath;
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


        public void Save(IPlayListState playList)
        {
            SaveContent(_serializer.Serialize(playList));
        }


        public IPlayListState Load()
        {
            return _serializer.Deserialize<FileListState>(LoadContent());
        }
    }
}
