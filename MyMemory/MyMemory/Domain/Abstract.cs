using System;
using System.Collections.Generic;


namespace MyMemory.Domain
{

    public interface INameableResource
    {
        string Name { get; }
        string Path { get; }
    }

    public interface ISerializer<TOut>
    {
        TOut Serialize<TIn>(TIn serializable);
        TIn Deserialize<TIn>(TOut serialized);
        void Deserialize<TIn>(TOut serialized, TIn serializable);
    }


    public interface IDirList
    {
        void Save();
        void Load();

        void Save(IDirListSaver saver);
        void Load(IDirListLoader loader);
    }


    public interface IDirListSaver
    {
        void Save(IDirListState list);
    }


    public interface IDirListLoader
    {
        IDirListState Load();
    }

    public interface IDirListState
    {
        List<DirectoryItem> Items { get; set; }
    }



    public interface IPlaylist
    {
        IPlaylistItem Current();
        IPlaylistItem Next();
        IPlaylistItem Prev();

        int IndexOf(IPlaylistItem item);

        event EventHandler WhenLoaded;
        event EventHandler WhenPlayed;

        bool IsEmpty { get; }

        void Save();
        void Save(IPlaylistSaver saver);

        void Load();
        void Load(IPlaylistLoader loader);
    }


    public interface IPlaylistItem : INameableResource
    {
        void Play();
    }


    public interface IPlaylistState
    {
        bool IsEmpty { get; }

        int Index { get; }

        IEnumerable<INameableResource> Resources { get; }
    }


    public interface IPlaylistSaver
    {
        void Save(IPlaylistState playlist);
    }


    public interface IPlaylistLoader
    {
        IPlaylistState Load();
    }


    public interface IPlaylistItemPlayer
    {
        void Play<T>(T t);
        event EventHandler WhenPlayed;
    }
}
