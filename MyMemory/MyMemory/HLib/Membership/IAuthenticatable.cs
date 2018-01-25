// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthenticatable.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the AuthenticationCompleteHandler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Membership
{

    public delegate void AuthenticationCompleteHandler(IUser user, AuthenticationResult result);


    public enum AuthenticationResult
    {
        Successful,
        Failed
    }
        
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAuthenticatable
    {
        event AuthenticationCompleteHandler AuthenticationCompleted;

        void Authenticate();

        void Authenticate(IUser user);
    }
}
