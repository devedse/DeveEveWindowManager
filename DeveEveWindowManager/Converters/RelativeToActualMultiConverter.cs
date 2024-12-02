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
            double retval = 0;

            if (values.Count == 2)
            {
                //Console.WriteLine("values[0]: " + values[0] + " values[1]: " + values[1]);
                if (values[0] is double relativeValue && values[1] is double actualSize)
                {
                    retval = relativeValue * actualSize;
                }
            }

            //Console.WriteLine("\tretval: " + retval);
            return retval;
        }

        public object? ConvertBack(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
