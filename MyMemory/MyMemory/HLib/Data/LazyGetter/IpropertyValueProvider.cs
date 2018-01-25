
namespace HLib.Data.LazyGetter
{
    public interface IPropertyValueProvider<out T> where T : class
    {
        T Value { get; }
    }
}
