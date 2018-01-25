// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyIsFalse.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the PropertyIsFalse type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Task.Conditions
{
    using System;
    using System.ComponentModel;


    public class PropertyIsFalse : ICondition
    {

        private readonly Type _type;
        private readonly object _owner;

        private readonly string _propertyName;


        public PropertyIsFalse(object owner, string propertyName)
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


        public PropertyIsFalse(Type ownerType, string propertyName)
        {
            _type = ownerType;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Gets NULL. Will never change.
        /// </summary>
        public IObservable ConditionStateChanged { get; private set; }


        public bool IsMet()
        {
            object value = null;

            if (_type != null)
            {
                value = _type.GetProperty(_propertyName).GetValue(null, null);
            }
            else if (_owner != null)
            {
                value = _owner.GetType().GetProperty(_propertyName).GetValue(_owner, null);
            }

            if (value is bool)
            {
                return (bool)value == false;
            }

            return false;
        }
    }
}
