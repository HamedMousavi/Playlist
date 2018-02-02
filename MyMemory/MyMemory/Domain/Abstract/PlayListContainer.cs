using System.Collections.Generic;


namespace MyMemory.Domain.Abstract
{

    public interface IPlaylistContainer
    {
        void Save(IPlaylistContainerSaver saver);
        void Load(IPlaylistContainerLoader loader);
    }


    public interface IPlaylistContainerSaver
    {
        void Save(IPlaylistContainerState list);
    }


    public interface IPlaylistContainerLoader
    {
        IPlaylistContainerState Load();
    }


    public interface IPlaylistContainerStore : IPlaylistContainerSaver, IPlaylistContainerLoader
    { }


    public interface IPlaylistContainerState
    {
        List<DirectoryItem> Items { get; set; }
    }
}
