using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
using LedighedsApp.Model.DataModel.Activity;
using LedighedsApp.Model.DataModel.Activity.Models;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.Animation;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View.UserControls
{
    public sealed partial class Calendar : UserControl
    {
        public DateTime CurrentDate { get; set; }
        public DateTime? SelectedDate { get { return ViewModel.SelectedDate; } set { ViewModel.SelectedDate = value; } }

        private ObservableCollection<KeyValuePair<DateTime, ICalendarItem>> CalendarItems;
        private CalendarHandler handler;
        private CalendarVm ViewModel;

        public Calendar()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((CalendarVm)(MainGrid.DataContext));
            handler = ViewModel.handler;
            this.CurrentDate = DateTime.Today;
            CalendarItems = handler.CalendarDictionary;
            InitializeCalendar();
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
        private void InitializeCalendar()
        {
            this.SelectedDate = null;
            CalendarGrid.RowDefinitions.Clear();
            CalendarGrid.Children.Clear();
            ViewModel.SelectedDateCalendarItems = null;

            InitializeHeader();
            InitializePreviousMonthBoxes();
            InitializeCurrentMonthBoxes();
            InitializeNextMonthBoxes();
        }

        private void InitializeHeader()
        {
            // Set current month + year.
            CurrentDateText.Text = App.Content["MONTH_" + (int) this.CurrentDate.Month].ToString() + " " + this.CurrentDate.Year;
            CurrentDayName.Text = App.Content["DAY_" + ((int)DateTime.Now.DayOfWeek+1).ToString()].ToString();
            CurrentDayNumber.Text = DateTime.Now.Day.ToString();
            // Create calendar days
            int column = 0;
            int dayNum = 1;
            CalendarGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            foreach (var day in Enum.GetValues(typeof(DayOfWeek)))
            {
                
                Border calendarDayBox = new Border { Style = (Style)Application.Current.Resources["CalendarDayBox"] };
                SetGridLocation(calendarDayBox, 0, column);

                TextBlock calendarDayLabel = new TextBlock { Style = (Style)Application.Current.Resources["CalendarDayLabel"], Text = App.Content["DAY_"+dayNum].ToString().Substring(0, 3) };
                SetGridLocation(calendarDayLabel, 0, column);

                CalendarGrid.Children.Add(calendarDayBox);
                CalendarGrid.Children.Add(calendarDayLabel);

                column++;
                dayNum++;
            }
        }

        private void InitializeCurrentMonthBoxes()
        {
            int row = 1;
            int maxDay = DateTime.DaysInMonth(this.CurrentDate.Year, this.CurrentDate.Month);
            var monthList = new ObservableCollection<KeyValuePair<DateTime, ICalendarItem>>(
                        CalendarItems.Where(x => x.Key.Year == this.CurrentDate.Year && x.Key.Month == this.CurrentDate.Month));
            for (int day = 1; day <= maxDay; day++)
            {
                DateTime dateIteration = new DateTime(this.CurrentDate.Year, this.CurrentDate.Month, day);
                int dayOfWeek = (int)dateIteration.DayOfWeek;
                
                var CurrentDateCalendarItems = handler.GetDateItems(monthList, dateIteration, ViewModel.SelectedCalendarType);

                CalendarItem calendarItem = new CalendarItem(dateIteration, day.ToString(), (Style)Application.Current.Resources["CalendarItemBox"], CurrentDateCalendarItems);

                calendarItem.Tapped += (sender, args) =>
                {
                    ((CalendarItem) sender).GridStyle = (Style) Application.Current.Resources["CalendarSelectedItemBox"];
                    ViewModel.SelectedDateCalendarItems = ((CalendarItem)sender).CalendarItems;

                    var selectedItem = (CalendarItem)CalendarGrid.Children.FirstOrDefault(x => x is CalendarItem && ((CalendarItem)x).Value == this.SelectedDate);
                    if(selectedItem != null && ((CalendarItem)sender).Value != this.SelectedDate)
                        selectedItem.GridStyle = (Style)Application.Current.Resources["CalendarItemBox"];
                    this.SelectedDate = ((CalendarItem)sender).Value;
                    OnSelectionChange(sender, args);
                };

                if (this.SelectedDate == dateIteration)
                    calendarItem.GridStyle = (Style) Application.Current.Resources["CalendarSelectedItemBox"];

                SetGridLocation(calendarItem, row, dayOfWeek);
                CalendarGrid.Children.Add(calendarItem);

                if (dayOfWeek == 6 && day != maxDay)
                {
                    row++;
                    CalendarGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }
            }
        }

        private void InitializePreviousMonthBoxes()
        {
            DateTime previousMonthDate = this.CurrentDate.AddMonths(-1);
            int DaysInMonth = DateTime.DaysInMonth(previousMonthDate.Year, previousMonthDate.Month);
            DateTime previousMonthDateIteration = new DateTime(previousMonthDate.Year, previousMonthDate.Month, DaysInMonth);
            CalendarGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            if (previousMonthDateIteration.DayOfWeek != DayOfWeek.Saturday)
            {
                
                for (int dayOfWeek = (int) previousMonthDateIteration.DayOfWeek; dayOfWeek >= 0; dayOfWeek--)
                {
                    CalendarItem calendarItem = new CalendarItem(previousMonthDateIteration,
                        previousMonthDateIteration.Day.ToString(),
                        (Style) Application.Current.Resources["CalendarOtherMonthItemBox"]);

                    calendarItem.Tapped += (sender, args) =>
                    {
                        this.SelectedDate = ((CalendarItem) sender).Value;
                        this.CurrentDate = this.CurrentDate.AddMonths(-1);
                        InitializeCalendar();
                        OnSelectionChange(sender, args);
                    };

                    SetGridLocation(calendarItem, 1, dayOfWeek);
                    CalendarGrid.Children.Add(calendarItem);

                    previousMonthDateIteration = previousMonthDateIteration.AddDays(-1);
                }
            }
        }

        private void InitializeNextMonthBoxes()
        {
            DateTime nextMonthDate = this.CurrentDate.AddMonths(1);
            DateTime nextMonthDateIteration = new DateTime(nextMonthDate.Year, nextMonthDate.Month, 1);

            int lastRow = CalendarGrid.RowDefinitions.Count - 1;

            if (nextMonthDateIteration.DayOfWeek != DayOfWeek.Sunday)
                for (int dayOfWeek = (int)nextMonthDateIteration.DayOfWeek; dayOfWeek < 7; dayOfWeek++)
                {
                    CalendarItem calendarItem = new CalendarItem(nextMonthDateIteration, nextMonthDateIteration.Day.ToString(), (Style)Application.Current.Resources["CalendarOtherMonthItemBox"]);

                    calendarItem.Tapped += (sender, args) =>
                    {
                        this.SelectedDate = ((CalendarItem)sender).Value;
                        this.CurrentDate = this.CurrentDate.AddMonths(1);
                        InitializeCalendar();

                        OnSelectionChange(sender, args);
                    };

                    SetGridLocation(calendarItem, lastRow, dayOfWeek);
                    CalendarGrid.Children.Add(calendarItem);

                    nextMonthDateIteration = nextMonthDateIteration.AddDays(1);
                }
        }

        private void SetGridLocation(FrameworkElement obj, int row, int column)
        {
            obj.SetValue(Grid.RowProperty, row);
            obj.SetValue(Grid.ColumnProperty, column);
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
            InitializeCalendar();
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
            this.CurrentDate = this.CurrentDate.AddMonths(-1);
            InitializeCalendar();
        }

        private void NextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CurrentDate = this.CurrentDate.AddMonths(1);
            InitializeCalendar();
        }

        private void GoBack_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
