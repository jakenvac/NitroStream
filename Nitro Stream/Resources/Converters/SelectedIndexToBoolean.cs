using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;

namespace Nitro_Stream.Resources.Converters
{
    class SelectedIndexToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result;
            bool Boolean;

            if(bool.TryParse(value.ToString(), out Boolean))
                result = Boolean ? 1 : 0;
            else
                result = -1;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int i = (int)value;
            return (i == 1);
        }
    }
}
