using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveEveWindowManager.Converters
{
    public class RelativeToActualMultiConverter : IMultiValueConverter
    {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count >= 2)
            {
                Console.WriteLine("values[0]: " + values[0] + " values[1]: " + values[1]);
                if (values[0] is double relativeValue && values[1] is double actualSize)
                {
                    return relativeValue * actualSize;
                }
            }

            return 0;
        }

        public object? ConvertBack(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
