using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyMemory
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class SelectedPlayListItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value != null && (bool)value;
            if (state)
            {
                return "pack://application:,,,/Images/resultset_next.png";
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
