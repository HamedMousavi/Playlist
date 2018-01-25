namespace HLib.Data.Validation
{
    public interface IValidator
    {
        bool Validate(object value);
        string Message { get; }
    }
}
