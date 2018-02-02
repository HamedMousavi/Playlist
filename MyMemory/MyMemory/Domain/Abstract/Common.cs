namespace MyMemory.Domain.Abstract
{

    public interface IResource
    {
        string Name { get; }
        string Path { get; }
        string Id { get; }
    }


    public interface ISerializer<TOut>
    {
        TOut Serialize<TIn>(TIn serializable);
        TIn Deserialize<TIn>(TOut serialized);
        void Deserialize<TIn>(TOut serialized, TIn serializable);
    }
}
