namespace HLib.Data.Validation.Validators
{

    public class EmptyOrWhitespaceValidator : IValidator
    {

        public bool Validate(object value)
        {
            var str = value as string;
            if (string.IsNullOrWhiteSpace(str))
            {
                Message = "This value should not be left empty";
                return false;
            }

            return true;
        }

        public string Message { get; private set; }
    }
}
