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
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View.UserControls
{
    public sealed partial class CalendarDateItem : UserControl
    {
        public Dictionary<TimeSpan, ICalendarItem> CalendarItems { get; set; }

        public ObservableCollection<ICalendarItem> CalendarItemsShow { get { return new ObservableCollection<ICalendarItem>(CalendarItems.Values); } } 

        public TimeSpan Time { get; set; }

        private CalendarVm ViewModel;

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

        public CalendarDateItem()
        {
            this.InitializeComponent();
        }

        public CalendarDateItem(TimeSpan time)
            : this()
        {
            this.Time = time;
        }

        public CalendarDateItem(TimeSpan time, Style style, Dictionary<TimeSpan, ICalendarItem> items = null)
            : this(time)
        {
            this.GridStyle = style;
            if (items != null)
                CalendarItems = items;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.CalendarItems == null || this.CalendarItems.Count < 1)
                TimeBox.Text = this.Time.Hours.ToString() + ":00";
            else
            {
                TimeSpan time = CalendarItems.FirstOrDefault().Key;
                TimeBox.Text = time.Hours.ToString() + ":" + (time.Minutes < 10 && time.Minutes > 0 ? "0" : "") + time.Minutes.ToString() + (time.Minutes == 0 ? "0" : "");
            }
            ItemBox.Style = this.GridStyle;
            ViewModel = ((CalendarVm)(MainGrid.DataContext));
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ItemsControl tempControl = ActivitiesContainer;
            foreach (var item in tempControl.Items)
            {
                var _Container = tempControl.ItemContainerGenerator.ContainerFromItem(item);
                var _Children = AllChildren(_Container);
                foreach (var obj in _Children)
                    if (obj is Grid)
                        ((Grid)obj).Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 240));
            }
            ((Grid)sender).Background = new SolidColorBrush(Color.FromArgb(180, 203, 203, 203));
            ViewModel.SelectedCalenderItem = (ICalendarItem)((Grid)sender).DataContext;
        }

        public List<UIElement> AllChildren(DependencyObject parent)
        {
            var _List = new List<UIElement> { };
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is UIElement)
                    _List.Add(_Child as UIElement);
                _List.AddRange(AllChildren(_Child));
            }
            return _List;
        }
    }
}
