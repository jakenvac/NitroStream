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
    class SelectedIndexToEnum : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result;
            Model.Orientations o = (Model.Orientations)value;

            result = (int)o;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.Orientations result;
            int i = (int)value;

            result = (Model.Orientations)i;

            return result;
        }
    }
}
