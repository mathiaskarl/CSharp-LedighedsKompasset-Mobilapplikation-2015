using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using Windows.UI.Xaml.Shapes;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;

namespace LedighedsApp.View.UserControls
{
    public sealed partial class CalendarItem : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<ICalendarItem> CalendarItems { get; set; }

        public DateTime Value { get; set; }
        public string Text { get; set; }

        private Style _gridStyle;
        public Style GridStyle
        {
            get { return _gridStyle; }
            set
            {
                _gridStyle = value;
                PropertyChangedEventHandler handler = _propertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs("GridStyle"));
                }
            }
        }

        private void _propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemBox.Style = this.GridStyle;
        }

        public CalendarItem()
        {
            this.InitializeComponent();
        }

        public CalendarItem(DateTime value, string text) : this()
        {
            this.Value = value;
            this.Text = text;
        }

        public CalendarItem(DateTime value, string text, Style style, IEnumerable<ICalendarItem> items = null) : this(value, text)
        {
            this.GridStyle = style;
            if(items == null)
                ItemValue.Foreground = new SolidColorBrush(Colors.Silver);
            if (items != null)
                CalendarItems = (ObservableCollection<ICalendarItem>)items;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ItemValue.Text = this.Text;
            ItemBox.Style = this.GridStyle;
            if (this.Value.CompareTo(DateTime.Today) == 0)
                ItemBox.Background = new SolidColorBrush(Colors.DarkSlateGray);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
