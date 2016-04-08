using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LedighedsApp.Model.Assets
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type typeName, object parameter, string language)
        {
            if (value != null)
                if((bool)value)
                    return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type typeName, object parameter, string language)
        {
            return null;
        }
    }
}
