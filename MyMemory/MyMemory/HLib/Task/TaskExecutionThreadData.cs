// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskExecutionThreadData.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the TaskDelegate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task
{

    using System.Collections.Generic;
    using System.Threading;

    using HLib.Logging;

    public class TaskExecutionThreadData
    {
        public List<ICondition> StartConditions { get; set; }

        public object StartConditionLock { get; set; }

        public AutoResetEvent CancelEvent { get; set; }

        public ITask Task { get; set; }

        public ILoggable Logger { get; set; }

        public TaskDelegate RunDelegate { get; set; }

        public AutoResetEvent TaskCompleteEvent { get; set; }
    }
}
