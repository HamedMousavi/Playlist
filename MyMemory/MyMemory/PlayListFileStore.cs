using System.IO;
using System.Text;


namespace MyMemory
{

    public class PlayListFileStore : IPlayListStore
    {

        private readonly ISerializer<string> _serializer;
        private readonly string _storagePath;


        public PlayListFileStore(string dirPath, ISerializer<string> serializer)
        {
            _serializer = serializer;
            _storagePath = $"{dirPath}{Path.DirectorySeparatorChar}playlist.json"; ;
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


        private void SaveContent(string content)
        {
            using (var sw = new StreamWriter(_storagePath, false, Encoding.UTF8))
            {
                sw.Write(content);
                sw.Close();
            }
        }


        public IPlayList Load()
        {
            return _serializer.Deserialize<IPlayList>(LoadContent());
        }


        public void Load(IPlayList playList)
        {
            _serializer.Deserialize(LoadContent(), playList);
        }

        public void Save(IPlayList playList)
        {
            _serializer.Serialize(playList);
        }
    }
}
