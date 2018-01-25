namespace MyMemory
{

    public interface ISerializer<TOut>
    {
        TOut Serialize<TIn>(TIn serializable);
        TIn Deserialize<TIn>(TOut serialized);
        void Deserialize<TIn>(TOut serialized, TIn serializable);
    }


    public interface IPlayList
    {
        void LoadFrom(IPlayListStore store);
        void SaveTo(IPlayListStore store);
    }


    public interface IPlayListStore
    {
        void Load(IPlayList playList);
        void Save(IPlayList playList);
    }


    public interface IPlayListLoader
    {
        IPlayList Load();
    }
}
