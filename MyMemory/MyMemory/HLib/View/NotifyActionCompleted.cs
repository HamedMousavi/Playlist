// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotifyActionCompleted.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the NotifyActionCompleted type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.View
{

    public class NotifyActionCompleted : INotifyActionComplete
    {

        public void RaiseActionCompleted(bool results)
        {
            if (ActionCompleted != null)
            {
                ActionCompleted(this, results);
            }
        }


        public event ActionCompletedDelegate ActionCompleted;
    }
}
