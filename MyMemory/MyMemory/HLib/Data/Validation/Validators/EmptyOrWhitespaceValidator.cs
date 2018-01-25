namespace HLib.Data.Validation.Validators
{

    public class FileAccessValidator : IValidator
    {

        public bool Validate(object value)
        {

            var path = value as string;
            if (string.IsNullOrWhiteSpace(path))
            {
                Message = "Path is not a valid string";
                return false;
            }

            if (!System.IO.File.Exists(path))
            {
                Message = "Given file does not exist";
                return false;
            }

            return true;
        }

        public string Message { get; private set; }
    }
}
