using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Activity;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.Animation;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View.UserControls.BottomBar
{
    public sealed partial class BottomBarCalendarDate : UserControl
    {
        public CalendarVm ViewModel;

        private static EventHandler _buttonShowEvent;
        public static event EventHandler ButtonShowEvent
        {
            add
            {
                _buttonShowEvent = null;
                _buttonShowEvent += value;
            }
            remove
            {
                _buttonShowEvent -= value;
            }
        }

        private static EventHandler _buttonDeleteEvent;
        public static event EventHandler ButtonDeleteEvent
        {
            add
            {
                _buttonDeleteEvent = null;
                _buttonDeleteEvent += value;
            }
            remove
            {
                _buttonDeleteEvent -= value;
            }
        }

        public BottomBarCalendarDate()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((CalendarVm)(MainGrid.DataContext));
            initAnimations();
        }

        private void ButtonDelete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.SelectedCalenderItem == null)
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("ERROR_SELECT_ACTIVITY"));
            else
            {
                if (ViewModel.SelectedCalenderItem.GetType() == typeof (Activity))
                {
                    ActivityHandler handler = new ActivityHandler();
                    if (handler.RemoveActivity(((Activity) ViewModel.SelectedCalenderItem).UserActivityId))
                    {
                        if (_buttonDeleteEvent != null)
                            _buttonDeleteEvent(this, EventArgs.Empty);
                    }
                    else
                        ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage(handler.ErrorMessage));

                }
                else if (ViewModel.SelectedCalenderItem.GetType() == typeof (Notification))
                {
                    NotificationHandler handler = new NotificationHandler();
                    if (handler.RemoveNotification(((Notification)ViewModel.SelectedCalenderItem)))
                    {
                        if (_buttonDeleteEvent != null)
                            _buttonDeleteEvent(this, EventArgs.Empty);
                    }
                    else
                        ErrorHandler.ShowError(handler.ErrorMessage);
                }

            }
        }

        private void ButtonShow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.SelectedCalenderItem == null)
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("ERROR_SELECT_ACTIVITY"));
            else
                if (_buttonShowEvent != null)
                    _buttonShowEvent(this, EventArgs.Empty);
        }

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
            int _height = _menuIsOpen ? -(int)CalendarTypePanel.ActualHeight : (int)CalendarTypePanel.ActualHeight;
            GuiAnimation animation = new GuiAnimation(CalendarTypePanel, AnimationType.Size);
            animation.AddStep(CalendarTypePanel.ActualWidth, 0, 20);
            _animations.Add(animation);

            GuiAnimation animation2 = new GuiAnimation(CalendarTypePanel, AnimationType.Position);
            animation2.AddStep(0, Canvas.GetTop(CalendarTypeCanvas.Children[0]) - _height, 20);
            _animations.Add(animation2);
            _menuIsOpen = false;
            if (CalendarTypeListView.SelectedItem.GetType() == typeof(Activity))
            {
                List<Object> Parameter = new List<object>() { CalendarTypeListView.SelectedItem, };
                if (ViewModel.SelectedDate != null)
                {
                    Parameter.Add((DateTime)ViewModel.SelectedDate);
                }

                NavigationService.NavigateToPage(typeof(HandleActivityView), Parameter);
            }
            else if (CalendarTypeListView.SelectedItem.GetType() == typeof(Notification))
                NavigationService.NavigateToPage(typeof(HandleNotificationView), ViewModel.SelectedDate);
        }

        private void InitListView()
        {
            ObservableCollection<ICalendarItem> tempList = ViewModel.CalendarItemTypes;
            CalendarTypeListView.ItemsSource = tempList;
        }

        private void ButtonAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _height = _menuIsOpen ? 0 : (int)CalendarTypePanel.ActualHeight;
            int _height2 = _menuIsOpen ? -(int)CalendarTypePanel.ActualHeight : (int)CalendarTypePanel.ActualHeight;

            GuiAnimation animation = new GuiAnimation(CalendarTypePanel, AnimationType.Size);
            animation.AddStep(CalendarTypePanel.ActualWidth, _height, 20);
            _animations.Add(animation);

            GuiAnimation animation2 = new GuiAnimation(CalendarTypePanel, AnimationType.Position);
            animation2.AddStep(0, Canvas.GetTop(CalendarTypeCanvas.Children[0]) - _height2, 20);
            _animations.Add(animation2);
            timer.Start();
            _menuIsOpen = !_menuIsOpen;
        }
        #endregion
    }
}
