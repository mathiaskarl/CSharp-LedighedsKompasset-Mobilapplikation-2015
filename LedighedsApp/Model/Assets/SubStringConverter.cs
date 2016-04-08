using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace LedighedsApp.Model.Assets
{
    public class SubStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            int param = Int32.Parse((string)parameter);
            if(value != null)
                if(value.ToString().Length >= param)
                    return ((string) value).Substring(0, param) + "..";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return null;
        }

    }
}
