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
using LedighedsApp.Model.DataModel.Activity;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.Animation;
using LedighedsApp.View.UserControls;
using LedighedsApp.View.UserControls.BottomBar;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View
{
    public sealed partial class ActivityView : Page
    {
        private ActivityVm ViewModel;
        private ActivityHandler handler;
        private Popup Popup;

        public ActivityView()
        {
            this.InitializeComponent();
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((ActivityVm)(MainGrid.DataContext));
            handler = ViewModel.handler;
            initAnimations();
            BottomBarActivity.ButtonShowEvent += new EventHandler(ShowActivityItem);
            ViewCalendarItem.ButtonCloseEvent += new EventHandler(CloseActivityItem);
        }

        #region PopUp
        private void ShowActivityItem(object sender, EventArgs e)
        {
            bool CreateNew = this.Popup == null;
            this.Popup = new Popup()
            {
                Child = new ViewCalendarItem(((BottomBarActivity)sender).ViewModel),
                IsOpen = true,
                Margin = new Thickness(0, 80, 0, 0),
                HorizontalOffset = (Window.Current.Bounds.Width - 350) / 2
            };
            if (CreateNew)
                MainGrid.Children.Add(Popup);
            Background.Visibility = Visibility.Visible;
        }

        private void CloseActivityItem(object sender, EventArgs e)
        {
            this.Popup.IsOpen = false;
            this.Popup = null;
            Background.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region initialize activities
        private void InitializeActivities()
        {
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
            ViewModel.SelectedActivityType = (ICalendarItem)ActivityTypeListView.SelectedItems[0];
            GuiAnimation animation = new GuiAnimation(ActivityTypePanel, AnimationType.Size);
            animation.AddStep(ActivityTypePanel.ActualWidth, 0, 20);
            _animations.Add(animation);
            _menuIsOpen = false;
            InitializeActivities();
        }

        private void InitListView()
        {
            ObservableCollection<ICalendarItem> tempList = ViewModel.CalendarItemTypes;
            tempList.Insert(0, ViewModel._staticItemType);
            ActivityTypeListView.ItemsSource = tempList;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _height = _menuIsOpen ? 0 : (int)ActivityTypePanel.ActualHeight;

            GuiAnimation animation = new GuiAnimation(ActivityTypePanel, AnimationType.Size);
            animation.AddStep(ActivityTypePanel.ActualWidth, _height, 20);
            _animations.Add(animation);
            timer.Start();
            _menuIsOpen = !_menuIsOpen;
        }
        #endregion

        private void GoBack_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
