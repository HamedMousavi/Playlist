// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyIsTrue.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the PropertyIsTrue type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task.Conditions
{

    using System.ComponentModel;


    public class PropertyIsTrue : ICondition
    {

        private readonly object _owner;

        private readonly string _propertyName;


        public PropertyIsTrue(object owner, string propertyName)
        {
            _owner = owner;
            _propertyName = propertyName;

            var notify = owner as INotifyPropertyChanged;
            if (notify != null)
            {
                ConditionStateChanged = new Observable();
                notify.PropertyChanged += (sender, args) => ConditionStateChanged.Notify(true);
            }
            else
            {
                ConditionStateChanged = null;
            }
        }

        /// <summary>
        /// Gets NULL. Will never change.
        /// </summary>
        public IObservable ConditionStateChanged { get; private set; }


        public bool IsMet()
        {
            var value = _owner.GetType().GetProperty(_propertyName).GetValue(_owner, null);

            if (value is bool)
            {
                return (bool)value;
            }

            return false;
        }
    }
}
