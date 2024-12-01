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
            IBrush brushToReturn = Brushes.Red;

            var parameters = (parameter as string)?.Split(',');
            if (value is bool isSelected && parameters?.Length == 2)
            {
                brushToReturn = isSelected ? Avalonia.Media.Brush.Parse(parameters[0]) : Avalonia.Media.Brush.Parse(parameters[1]);
            }

            //make 0.8 opacity
            if (brushToReturn is IImmutableSolidColorBrush solidColorBrush)
            {
                return new SolidColorBrush(new Color(204, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B));
            }
            return brushToReturn;
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
