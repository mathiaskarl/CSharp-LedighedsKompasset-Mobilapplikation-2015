using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.Animation;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View.UserControls
{
    public sealed partial class CalendarDate : UserControl
    {
        public DateTime? SelectedDate { get { return ViewModel.SelectedDate; } set { ViewModel.SelectedDate = value; } }
        public ICalendarItem SelectedCalendarItem { get; set; }

        private ObservableCollection<KeyValuePair<DateTime, ICalendarItem>> CalendarItems;
        private CalendarHandler handler;
        private CalendarVm ViewModel;

        public CalendarDate()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((CalendarVm)(MainGrid.DataContext));
            handler = ViewModel.handler;
            CalendarItems = handler.CalendarDictionary;
            InitializeCalendarDate();
            initAnimations();
        }

        #region Events
        private event EventHandler<TappedRoutedEventArgs> _selectionChange;
        public event EventHandler<TappedRoutedEventArgs> SelectionChange
        {
            add { _selectionChange += value; }
            remove { _selectionChange -= value; }
        }

        public void OnSelectionChange(object sender, TappedRoutedEventArgs e)
        {
            if (_selectionChange != null)
                _selectionChange(sender, e);
        }
        #endregion

        #region initialize calendar

        public void Refresh()
        {
            handler.RefreshCalendarDictionary();
            CalendarItems = handler.CalendarDictionary;
            InitializeCalendarDate();
        }

        private void InitializeCalendarDate()
        {
            CalendarDateGrid.RowDefinitions.Clear();
            CalendarDateGrid.Children.Clear();
            ViewModel.SelectedCalenderItem = null;

            InitializeHeader();
            InitializeCurrentHourBoxes();
        }

        private void InitializeHeader()
        {
            CurrentDateText.Text = ((DateTime)ViewModel.SelectedDate).Day.ToString() + " " + App.Content["MONTH_" + ((DateTime)ViewModel.SelectedDate).Month].ToString() + " " + ((DateTime)ViewModel.SelectedDate).Year.ToString();
            CurrentDayName.Text = App.Content["DAY_" + ((int)((DateTime)ViewModel.SelectedDate).DayOfWeek + 1).ToString()].ToString();
            CurrentDayNumber.Text = ((DateTime) ViewModel.SelectedDate).Day.ToString();

            Border calendarDayBox = new Border { Style = (Style)Application.Current.Resources["CalendarDayBox"] };
            SetGridLocation(calendarDayBox, 0);

            CalendarDateGrid.Children.Add(calendarDayBox);
        }

        private void InitializeCurrentHourBoxes()
        {
            int row = 0;

            DateTime dateIteration = DateTime.Parse(ViewModel.SelectedDate.Value.ToString());
            var todaysList = new ObservableCollection<KeyValuePair<DateTime, ICalendarItem>>(
                        CalendarItems.Where(x => x.Key.Year == dateIteration.Year && x.Key.Month == dateIteration.Month && x.Key.Day == dateIteration.Day));

            for (int hour = 0; hour < 24; hour++)
            {
                Dictionary<TimeSpan, ICalendarItem> CurrentDateCalendarItems = handler.GetHourItems(todaysList, dateIteration, hour, ViewModel.SelectedCalendarType);
                Dictionary<TimeSpan, ICalendarItem> tempDictionary = new Dictionary<TimeSpan, ICalendarItem>();
                foreach(KeyValuePair<TimeSpan, ICalendarItem> obj in CurrentDateCalendarItems)
                    if(obj.Key.Minutes == 0)
                        tempDictionary.Add(obj.Key, obj.Value);

                CalendarDateItem calendarDateItem = CreateDateItem(hour, tempDictionary);

                SetGridLocation(calendarDateItem, row);
                CalendarDateGrid.Children.Add(calendarDateItem);
                row++;
                CalendarDateGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                List<int> UsedMinutes = new List<int>(); 
                tempDictionary = new Dictionary<TimeSpan, ICalendarItem>();
                foreach (KeyValuePair<TimeSpan, ICalendarItem> obj in CurrentDateCalendarItems.Where(x => x.Key.Minutes != 0))
                    if (!UsedMinutes.Contains(obj.Key.Minutes))
                    {
                        UsedMinutes.Add(obj.Key.Minutes);
                        foreach(KeyValuePair<TimeSpan, ICalendarItem> objs in CurrentDateCalendarItems.Where(x => x.Key.Minutes == obj.Key.Minutes))
                            tempDictionary.Add(obj.Key, obj.Value);
                        CalendarDateItem tempItem = CreateDateItem(hour, tempDictionary);

                        SetGridLocation(tempItem, row);
                        CalendarDateGrid.Children.Add(tempItem);
                        row++;
                        CalendarDateGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    }
            }
        }

        private CalendarDateItem CreateDateItem(int hour, Dictionary<TimeSpan, ICalendarItem> dictionary)
        {
            CalendarDateItem calendarDateItem = new CalendarDateItem(new TimeSpan(0, hour, 0, 0), (Style)Application.Current.Resources["CalendarItemBox"], dictionary);

            return calendarDateItem;
        }

        private void SetGridLocation(FrameworkElement obj, int row)
        {
            obj.SetValue(Grid.RowProperty, row);
        }
        #endregion

        #region Animations
        private List<GuiAnimation> _animations = new List<GuiAnimation>();
        private DispatcherTimer timer = new DispatcherTimer();
        private bool _menuIsOpen = false;
        private int _height = 0;

        private void initAnimations()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += timer_Tick;
            //Smid lidt fyld i et ListView
            InitListView();
            _height = (int)CalendarTypePanel.ActualHeight;
            CalendarTypePanel.Height = 0;
        }

        void timer_Tick(object sender, object e)
        {
            foreach (GuiAnimation animation in _animations.Where(animation => !animation.IsDone))
            {
                animation.Animate();
            }
        }

        private void CalendarTypeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedCalendarType = (ICalendarItem)CalendarTypeListView.SelectedItems[0];
            GuiAnimation animation = new GuiAnimation(CalendarTypePanel, AnimationType.Size);
            animation.AddStep(CalendarTypePanel.ActualWidth, 0, 20);
            _animations.Add(animation);
            _menuIsOpen = false;
            InitializeCalendarDate();
        }

        private void InitListView()
        {
            ObservableCollection<ICalendarItem> tempList = ViewModel.CalendarItemTypes;
            tempList.Insert(0, ViewModel._staticItemType);
            CalendarTypeListView.ItemsSource = tempList;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _height = _menuIsOpen ? 0 : (int)CalendarTypePanel.ActualHeight;

            GuiAnimation animation = new GuiAnimation(CalendarTypePanel, AnimationType.Size);
            animation.AddStep(CalendarTypePanel.ActualWidth, _height, 20);
            _animations.Add(animation);
            timer.Start();
            _menuIsOpen = !_menuIsOpen;
        }
        #endregion

        private void PreviousButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SelectedDate = ((DateTime)ViewModel.SelectedDate).AddDays(-1);
            InitializeCalendarDate();
        }

        private void NextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SelectedDate = ((DateTime)ViewModel.SelectedDate).AddDays(1);
            InitializeCalendarDate();
        }

        private void GoBack_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
