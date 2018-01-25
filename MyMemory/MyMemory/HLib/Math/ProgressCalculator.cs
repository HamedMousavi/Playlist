// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressCalculator.cs" company="www.OrderedSoft.com">
//   Author: Hamed Mousavi: HamedMosavi[at] Yahoo (dot) com
//   License agreement: Please read License.txt provided in solution directory
// </copyright>
// <summary>
//   Defines the ProgressCalculator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace HLib.Math
{

    using System;
    using System.Threading;

    using HLib.Data;


    public class ProgressCalculator : NotifyPropertyChanged
    {

        private readonly short _lastPercent;
        private readonly object _valueLock;
        private double _minValue;
        private double _maxValue;
        private double _value;
        private double _delta;
        private short _percent;


        public ProgressCalculator()
        {
            _lastPercent = 0;
            _valueLock = new object();
        }


        public double MinValue
        {
            get
            {
                return _minValue;
            }

            set
            {
                if (Math.Abs(_minValue - value) > 0.01)
                {
                    _minValue = value;
                    RecalculateValue();
                }
            }
        }


        public double MaxValue
        {
            get
            {
                return _maxValue;
            }

            set
            {
                if (Math.Abs(_maxValue - value) > 0.01)
                {
                    _maxValue = value;
                    RecalculateValue();
                }
            }
        }


        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                lock (_valueLock)
                {
                    _value = value;
                    RecalculatePercent();
                }
            }
        }


        public short Percent
        {
            get { return _percent; }
        }


        public void Reset(double maxValue)
        {
            Value = 0;
            MaxValue = maxValue;
        }


        public void Increment()
        {
            lock (_valueLock)
            {
                _value++;
            }

            RecalculatePercent();
        }


        private void RecalculatePercent()
        {
            if (Math.Abs(_delta - 0) < 1)
            {
                return;
            }

            _percent = Convert.ToInt16(_value * 100.0d / _delta);

            // Don't raise event unless 1% or more has changed
            if (_percent - _lastPercent >= 1)
            {
                FirePropertyChanged(this, "Percent");
            }
        }


        private void RecalculateValue()
        {
            _delta = _maxValue - _minValue;
            _value = _percent * _delta / 100.0d;
            RecalculatePercent();
        }
    }
}
