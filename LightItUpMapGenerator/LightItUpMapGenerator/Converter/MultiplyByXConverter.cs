using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LightItUpMapGenerator.Converter
{
    public class MultiplyByXConverter : IValueConverter
    {
        public static MultiplyByXConverter Instance = new MultiplyByXConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int valueAsInt = System.Convert.ToInt32(value);
            int factor = System.Convert.ToInt32(parameter);

            return valueAsInt * factor;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
