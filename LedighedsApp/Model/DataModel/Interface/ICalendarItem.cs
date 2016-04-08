using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace LedighedsApp.Model.DataModel.Interface
{
    public interface ICalendarItem
    {
        int Id { get; set; }
        string Title { get; }
        string Header { get; }
        SolidColorBrush RGBColor { get; }
    }
}