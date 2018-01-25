// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Interfaces.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the Interfaces type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.View
{
    using HLib.Logging;

    public delegate void ActionCompletedDelegate(object sender, object results);


    public interface INotifyActionComplete
    {
        event ActionCompletedDelegate ActionCompleted;
    }


    public interface IActionLogic : INotifyActionComplete
    {
        ILoggable Logger { get; set; }

        void Run();
    }
}
