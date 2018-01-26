using System;
using System.Collections.Generic;


namespace MyMemory.Domain
{

    public interface ISerializer<TOut>
    {
        TOut Serialize<TIn>(TIn serializable);
        TIn Deserialize<TIn>(TOut serialized);
        void Deserialize<TIn>(TOut serialized, TIn serializable);
    }


    public interface IPlayList
    {
        IPlayListItem Current();
        IPlayListItem Next();
        IPlayListItem Prev();

        int IndexOf(IPlayListItem item);

        event EventHandler WhenLoaded;
        event EventHandler WhenPlayed;
        
        bool IsEmpty { get; }

        void Save();
        void Save(IPlayListSaver saver);

        void Load();
        void Load(IPlayListLoader loader);
    }


    public interface IPlayListItem
    {
        void Play();
    }


    public interface IPlayListState
    {
        bool IsEmpty { get; }

        IEnumerable<string> Names { get; }
        int Index { get; }
    }


    public interface IPlayListSaver
    {
        void Save(IPlayListState playList);
    }


    public interface IPlayListLoader
    {
        IPlayListState Load();
    }


    public interface IPlayListItemPlayer
    {
        void Play<T>(T t);
        event EventHandler WhenPlayed;
    }
}
