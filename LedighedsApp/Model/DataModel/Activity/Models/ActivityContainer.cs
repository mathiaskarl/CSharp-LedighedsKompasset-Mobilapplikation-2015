using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel.Interface;

namespace LedighedsApp.Model.DataModel.Activity.Models
{
    public class ActivityContainer : Activity
    {
        public ActivityContainer() { }

        public int Id { get { return 0; } set { value = 0; } }
        public string Title { get { return _title; } set { _title = value; } }
        private string _title = null;
        public string Header { get { return null; } }
        public SolidColorBrush RGBColor { get { return new SolidColorBrush(HexToColor.GetColor("#6495ed")); } }
    }
}
