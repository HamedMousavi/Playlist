using System.IO;
using System.Text;
using MyMemory.Domain.Abstract;


namespace MyMemory.Domain
{

    public class PlaylistContainerFileStore : IPlaylistContainerStore
    {

        private readonly ISerializer<string> _serializer;
        private readonly string _storagePath;


        public PlaylistContainerFileStore(string storagePath, ISerializer<string> serializer)
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
        

        public void Save(IPlaylistContainerState list)
        {
            SaveContent(_serializer.Serialize(list));
        }


        public IPlaylistContainerState Load()
        {
            return _serializer.Deserialize<DirectoryListState>(LoadContent());
        }
    }
}
