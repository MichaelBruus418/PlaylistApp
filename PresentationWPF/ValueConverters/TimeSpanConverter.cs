using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PresentationWPF.ValueConverters 
{
    [ValueConversion(typeof(TimeSpan), typeof(String))]
    internal class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            TimeSpan ts  = (TimeSpan) value;
            return (ts.Hours > 0) ? ts.ToString(@"h\:mm\:ss") : ts.ToString(@"mm\:ss");
        }

        // Never called as we don't use two-way binindg.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return value;
        }
    }
}
