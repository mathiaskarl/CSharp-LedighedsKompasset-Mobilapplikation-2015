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
    public sealed partial class BottomBarActivity : UserControl
    {
        public ActivityVm ViewModel;

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

        public BottomBarActivity()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((ActivityVm)(MainGrid.DataContext));
            initAnimations();
        }

        private void ButtonDelete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.SelectedActivity == null)
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("ERROR_SELECT_ACTIVITY"));
            else
            {
                if (_buttonDeleteEvent != null)
                    _buttonDeleteEvent(this, EventArgs.Empty);
            }
        }

        private void ButtonEdit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.SelectedActivity == null)
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("ERROR_SELECT_ACTIVITY"));
            else
                NavigationService.NavigateToPage(typeof(HandleActivityView), ViewModel.SelectedActivity);
        }

        private void ButtonShow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.SelectedActivity == null)
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
            _height = (int)ActivityTypePanel.ActualHeight;
            ActivityTypePanel.Height = 0;
        }

        void timer_Tick(object sender, object e)
        {
            foreach (GuiAnimation animation in _animations.Where(animation => !animation.IsDone))
            {
                animation.Animate();
            }
        }

        private void ActivityTypeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int _height = _menuIsOpen ? -(int)ActivityTypePanel.ActualHeight : (int)ActivityTypePanel.ActualHeight;
            GuiAnimation animation = new GuiAnimation(ActivityTypePanel, AnimationType.Size);
            animation.AddStep(ActivityTypePanel.ActualWidth, 0, 20);
            _animations.Add(animation);

            GuiAnimation animation2 = new GuiAnimation(ActivityTypePanel, AnimationType.Position);
            animation2.AddStep(0, Canvas.GetTop(ActivityTypeCanvas.Children[0]) - _height, 20);
            _animations.Add(animation2);
            _menuIsOpen = false;
            if(ActivityTypeListView.SelectedItem.GetType() == typeof(Activity))
                NavigationService.NavigateToPage(typeof(HandleActivityView), ActivityTypeListView.SelectedItem);
        }

        private void InitListView()
        {
            ObservableCollection<ICalendarItem> tempList = ViewModel.CalendarItemTypes;
            ActivityTypeListView.ItemsSource = tempList;
        }

        private void ButtonAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _height = _menuIsOpen ? 0 : (int)ActivityTypePanel.ActualHeight;
            int _height2 = _menuIsOpen ? -(int)ActivityTypePanel.ActualHeight : (int)ActivityTypePanel.ActualHeight;

            GuiAnimation animation = new GuiAnimation(ActivityTypePanel, AnimationType.Size);
            animation.AddStep(ActivityTypePanel.ActualWidth, _height, 20);
            _animations.Add(animation);

            GuiAnimation animation2 = new GuiAnimation(ActivityTypePanel, AnimationType.Position);
            animation2.AddStep(0, Canvas.GetTop(ActivityTypeCanvas.Children[0]) - _height2, 20);
            _animations.Add(animation2);
            timer.Start();
            _menuIsOpen = !_menuIsOpen;
        }
        #endregion
    }
}
