using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace DeveEveWindowManager.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        private static Brush _selectionColor = new SolidColorBrush(new Color(0, 0, 0, 128));

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var parameters = (parameter as string)?.Split(',');
            if (value is bool isSelected && parameters?.Length == 2)
            {
                return isSelected ? Avalonia.Media.Brush.Parse(parameters[0]) : Avalonia.Media.Brush.Parse(parameters[1]);
            }
            return Brushes.DarkGreen;
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
