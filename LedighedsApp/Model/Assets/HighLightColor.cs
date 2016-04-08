using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace LedighedsApp.Model.Assets
{
    class HighLightColor : IValueConverter
    {
        public object Convert(object value, Type typeName, object parameter, string language)
        {
            SolidColorBrush obj = new SolidColorBrush((!(bool)value ? HexToColor.GetColor("507693") : HexToColor.GetColor("fffff0")));
            return obj;
        }

        public object ConvertBack(object value, Type typeName, object parameter, string language)
        {
            return null;
        }
    }
}
