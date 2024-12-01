using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace DeveEveWindowManager.Converters
{
    public class RelativeToActualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return 0;

            double relativeValue = (double)value;
            double actualSize = (double)parameter;

            return relativeValue * actualSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return 0;

            double actualValue = (double)value;
            double actualSize = (double)parameter;

            return actualValue / actualSize;
        }
    }
}