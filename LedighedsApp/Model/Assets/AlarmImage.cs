using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace LedighedsApp.Model.Assets
{
    public class AlarmImage : IValueConverter
    {
        public object Convert(object value, Type typeName, object parameter, string language)
        {
            return ((DateTime)value) < DateTime.Now ? "../Assets/Images/AlarmSmallWhite.png" : "../Assets/Images/AlarmSmall.png";
        }

        public object ConvertBack(object value, Type typeName, object parameter, string language)
        {
            return null;
        }
    }
}
