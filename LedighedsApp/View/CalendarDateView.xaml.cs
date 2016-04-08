using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.Animation;
using LedighedsApp.View.UserControls;
using LedighedsApp.View.UserControls.BottomBar;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View
{
    public sealed partial class CalendarDateView : Page, INotifyPropertyChanged
    {
        private Popup Popup;
        public DateTime? SelectedDate { get { return ViewModel.SelectedDate; } set { ViewModel.SelectedDate = value; } }
        public ICalendarItem SelectedCalendarItem { get; set; }

        private ObservableCollection<KeyValuePair<DateTime, ICalendarItem>> CalendarItems;
        private CalendarHandler handler;
        private CalendarVm ViewModel;

        public CalendarDateView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainGrid.DataContext = (CalendarVm)e.Parameter;
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((CalendarVm)(MainGrid.DataContext));
            handler = ViewModel.handler;
            handler.RefreshCalendarDictionary();
            CalendarItems = handler.CalendarDictionary;
            InitializeCalendarDate();
            initAnimations();
            BottomBarCalendarDate.ButtonShowEvent += new EventHandler(ShowCalendarItem);
            BottomBarCalendarDate.ButtonDeleteEvent += new EventHandler(DeleteCalendarItem);
            ViewCalendarItem.ButtonCloseEvent += new EventHandler(CloseCalendarItem);
        }

        #region PopUp
        private void ShowCalendarItem(object sender, EventArgs e)
        {
            bool CreateNew = this.Popup == null;
            this.Popup = new Popup()
            {
                Child = new ViewCalendarItem(((BottomBarCalendarDate)sender).ViewModel),
                IsOpen = true,
                Margin = new Thickness(0, 80, 0, 0),
                HorizontalOffset = (Window.Current.Bounds.Width-350)/2
            };
            if(CreateNew)
                MainGrid.Children.Add(Popup);
            Background.Visibility = Visibility.Visible;
        }

        private void CloseCalendarItem(object sender, EventArgs e)
        {
            this.Popup.IsOpen = false;
            this.Popup = null;
            Background.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Initialize calendar

        private void DeleteCalendarItem(object sender, EventArgs e)
        {
            var obj = CalendarItems.FirstOrDefault(x => x.Value == ViewModel.SelectedCalenderItem);
            CalendarItems.Remove(obj);
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
                foreach (KeyValuePair<TimeSpan, ICalendarItem> obj in CurrentDateCalendarItems.Where(x => x.Key.Minutes != 0))
                    if (!UsedMinutes.Contains(obj.Key.Minutes))
                    {
                        tempDictionary = new Dictionary<TimeSpan, ICalendarItem>();
                        UsedMinutes.Add(obj.Key.Minutes);
                        foreach(KeyValuePair<TimeSpan, ICalendarItem> objs in CurrentDateCalendarItems.Where(x => x.Key.Minutes == obj.Key.Minutes))
                            tempDictionary.Add(objs.Key, objs.Value);
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
            return new CalendarDateItem(new TimeSpan(0, hour, 0, 0), (Style)Application.Current.Resources["CalendarItemBox"], dictionary);
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler1 = PropertyChanged;
            if (handler1 != null) handler1(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}