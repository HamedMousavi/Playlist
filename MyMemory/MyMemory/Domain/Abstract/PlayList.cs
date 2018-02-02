using System;
using System.Collections.Generic;


namespace MyMemory.Domain.Abstract
{

    public interface IPlaylist
    {
        bool IsEmpty { get; }

        IPlaylistItem ActiveItem { get; set; }
        IPlaylistItem Next { get; }
        IPlaylistItem Prev { get; }

        IPlaylistItem FindById(string itemId);

        event EventHandler WhenLoaded;
        event EventHandler WhenPlayed;
        event EventHandler WhenActiveItemChanged;

        void Save();
        void Save(IPlaylistSaver saver);

        void Load();
        void Load(IPlaylistLoader loader);
    }


    public interface IPlaylistItem : IResource
    {
        void Play();
    }


    public interface IPlaylistState
    {
        bool IsEmpty { get; }

        string SelectedItemId { get; }

        IEnumerable<IResource> Resources { get; }
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
