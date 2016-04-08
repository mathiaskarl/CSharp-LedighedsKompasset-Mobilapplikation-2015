using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Activity;
using LedighedsApp.Model.DataModel.Activity.DbSet;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View.UserControls
{
    public sealed partial class ViewCalendarItem : UserControl
    {
        private Contents ViewModel;
        private ICalendarItem CurrentItem;

        private static EventHandler _buttonCloseEvent;
        public static event EventHandler ButtonCloseEvent
        {
            add
            {
                _buttonCloseEvent = null;
                _buttonCloseEvent += value;
            }
            remove
            {
                _buttonCloseEvent -= value;
            }
        }

        public ViewCalendarItem(Contents obj)
        {
            this.InitializeComponent();
            ViewModel = obj;
            MainGrid.DataContext = ViewModel;
            CurrentItem = GetItemType(ViewModel);
            InitView();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentItem.GetType() == typeof(Activity))
            {
                Activity tempItem = (Activity) CurrentItem;
                int row = 0;
                AddItemToGrid(ViewModel.Content["TYPE"].ToString(), tempItem.Title, row);
                row++;

                foreach (ActivityProperty obj in tempItem.Properties)
                {
                    AddItemToGrid(obj.Name, tempItem.PropertyValues.FirstOrDefault(x => x.Key == obj.Property).Value, row);
                    row++;
                }
            }
            else if (CurrentItem.GetType() == typeof (Notification))
            {
                PropertyGrid.ColumnDefinitions.Clear();
                PropertyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
                PropertyGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
                Notification tempItem = (Notification) CurrentItem;
                AddItemToGrid(ViewModel.Content["TYPE"].ToString(), tempItem.Title, 0);
                AddItemToGrid(ViewModel.Content["ALARM_NAME"].ToString() + " " + ViewModel.Content["TYPE"].ToString().ToLower(), (tempItem.Primary ? ViewModel.Content["ALARM_PRIMARY"].ToString() : ViewModel.Content["ALARM_SECONDARY"].ToString()), 1);
                AddItemToGrid(ViewModel.Content["ALARM_TRIGGERTIME"].ToString(), tempItem.TriggerTime.ToString("dd-MM-yyyy HH:mm"), 2);
                if(!tempItem.Primary)
                    AddItemToGrid(ViewModel.Content["ALARM_TEXT"].ToString(), tempItem.Text, 3);
            }
        }

        private void AddItemToGrid(string property, string propertyValue, int row)
        {
            PropertyGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            TextBlock tempBlock = new TextBlock();
            tempBlock.Style = (Style)Application.Current.Resources["CalendarItemProperty"];
            tempBlock.Text = property + ":";
            tempBlock.SetValue(Grid.ColumnProperty, 0);
            tempBlock.SetValue(Grid.RowProperty, row);

            TextBlock tempBlock2 = new TextBlock();
            tempBlock2.Style = (Style)Application.Current.Resources["CalendarItemPropertyValue"];
            tempBlock2.Text = propertyValue;
            tempBlock2.SetValue(Grid.ColumnProperty, 1);
            tempBlock2.SetValue(Grid.RowProperty, row);

            PropertyGrid.Children.Add(tempBlock);
            PropertyGrid.Children.Add(tempBlock2);
        }

        private ICalendarItem GetItemType(Contents viewModel)
        {
            if (viewModel.GetType() == typeof (CalendarVm))
                return ((CalendarVm) viewModel).SelectedCalenderItem;

            if (viewModel.GetType() == typeof (ActivityVm))
                return ((ActivityVm) viewModel).SelectedActivity.Activity;

            if (viewModel.GetType() == typeof (NotificationVm))
                return ((NotificationVm) viewModel).SelectedNotification;
            return null;
        }

        private void InitView()
        {
            Container.BorderBrush = new SolidColorBrush(HexToColor.GetColor("#6893b4"));
            Header.Background = new SolidColorBrush(HexToColor.GetColor("#88badd"));
            ItemTitle.Text = CurrentItem.Title;
            ItemDate.Text = (ViewModel.GetType() == typeof (CalendarVm)
                ? ((CalendarVm) ViewModel).SelectedDateFormat
                : ViewModel.GetType() == typeof (ActivityVm)
                    ? ((ActivityVm) ViewModel).SelectedActivity.Date.ToString()
                    : ((NotificationVm) ViewModel).SelectedNotification.Date.ToString());
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_buttonCloseEvent != null)
            {
                _buttonCloseEvent(this, EventArgs.Empty);
            }
        }
    }
}
