using System.IO;
using System.Linq;


namespace MyMemory.Domain
{

    public class DirectoryLoader : IPlayListLoader
    {

        private readonly IPlayListLoader _cacheLoader;
        private readonly string _dirPath;


        public DirectoryLoader(IPlayListLoader cacheLoader, string dirPath)
        {
            _cacheLoader = cacheLoader;
            _dirPath = dirPath;
        }


        public IPlayListState Load()
        {
            var state = _cacheLoader.Load();
            return state == null || state.IsEmpty ? LoadFromDirectory() : state;
        }


        private IPlayListState LoadFromDirectory()
        {
            return new FileListState(
                Directory
                .EnumerateFiles(_dirPath, "*.*", SearchOption.AllDirectories)
                .Select(Path.GetFileName)
                .ToList(),
                0);
        }
    }
}
