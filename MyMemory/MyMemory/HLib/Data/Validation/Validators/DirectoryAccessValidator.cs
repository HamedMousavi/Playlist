namespace HLib.Data.Validation.Validators
{

    using System;
    using System.Security;
    using System.Security.Permissions;


    public class DirectoryAccessValidator : IValidator
    {

        public DirectoryAccessValidator(AccessValidationType validationType)
        {
            _validationType = validationType;
        }


        public bool Validate(object value)
        {
            try
            {
                var path = value as string;
                if (string.IsNullOrWhiteSpace(path))
                {
                    Message = "Path is not a valid string";
                    return false;
                }

                if (!System.IO.Directory.Exists(path))
                {
                    Message = "Given path does not exist, is hidden or this application/user does not have access to it.";
                    return false;
                }

                FileIOPermissionAccess accessType;
                switch (_validationType)
                {
                    case AccessValidationType.Read:
                        accessType = FileIOPermissionAccess.Read;
                        break;

                        case AccessValidationType.Write:
                        accessType = FileIOPermissionAccess.Write;
                        break;

                        case AccessValidationType.ReadAndWrite:
                        accessType = FileIOPermissionAccess.AllAccess;
                        break;

                    default:
                        accessType = FileIOPermissionAccess.AllAccess;
                        break;
                }

                var permission = new FileIOPermission(accessType, path);
                var permissionSet = new PermissionSet(PermissionState.None);
                permissionSet.AddPermission(permission);
                
                if (!permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
                {
                    Message = "This application/user does not have access to given directory.";
                    return false;
                }
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public string Message { get; private set; }


        public enum AccessValidationType
        {
            Read,
            Write,
            ReadAndWrite
        }


        private readonly AccessValidationType _validationType;
    }
}
