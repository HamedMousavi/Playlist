using System.IO;
using System.Linq;
using MyMemory.Domain.Abstract;


namespace MyMemory.Domain
{

    public class DirFileLoader : IPlaylistLoader
    {

        private readonly IPlaylistLoader _cacheLoader;
        private readonly string _dirPath;


        public DirFileLoader(IPlaylistLoader cacheLoader, string dirPath)
        {
            _cacheLoader = cacheLoader;
            _dirPath = dirPath;
        }


        public IPlaylistState Load()
        {
            var state = _cacheLoader.Load();
            return state == null || state.IsEmpty ? LoadFromDirectory() : state;
        }


        private IPlaylistState LoadFromDirectory()
        {
            return new PlaylistState(
                Directory
                .EnumerateFiles(_dirPath, "*.*", SearchOption.AllDirectories)
                .Select(p => new FileResource(p))
                .ToList());
        }
    }
}
