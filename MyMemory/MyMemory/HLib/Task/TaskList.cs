// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskList.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the TaskList type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public class TaskList : BindingList<ITask>
    {

        public ITask this[string taskName]
        {
            get
            {
                ITask result = null;

                Parallel.ForEach(
                    this,
                    (task, state) =>
                        {
                            if (string.Equals(task.Name, taskName, StringComparison.InvariantCulture))
                            {
                                result = task;
                                state.Break();
                            }
                        });

                return result;
            }
        }


        protected override void RemoveItem(int index)
        {
            this[index].Dispose();
            base.RemoveItem(index);
        }
    }
}
