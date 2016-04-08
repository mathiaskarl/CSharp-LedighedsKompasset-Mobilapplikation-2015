using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.Animation;
using LedighedsApp.View.UserControls;
using LedighedsApp.View.UserControls.BottomBar;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View
{
    public sealed partial class NotificationView : Page
    {

        private NotificationVm ViewModel;
        private NotificationHandler handler;
        private Popup Popup;

        public NotificationView()
        {
            this.InitializeComponent();
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((NotificationVm)(MainGrid.DataContext));
            handler = ViewModel.handler;
            initAnimations();
            BottomBarNotification.ButtonShowEvent += new EventHandler(ShowNotificationItem);
            ViewCalendarItem.ButtonCloseEvent += new EventHandler(CloseNotificationItem);
            PrimaryTopBorder.BorderBrush = new SolidColorBrush(HexToColor.GetColor("204c6a"));
        }

        #region PopUp
        private void ShowNotificationItem(object sender, EventArgs e)
        {
            bool CreateNew = this.Popup == null;
            this.Popup = new Popup()
            {
                Child = new ViewCalendarItem(((BottomBarNotification)sender).ViewModel),
                IsOpen = true,
                Margin = new Thickness(0, 80, 0, 0),
                HorizontalOffset = (Window.Current.Bounds.Width - 350) / 2
            };
            if (CreateNew)
                MainGrid.Children.Add(Popup);
            Background.Visibility = Visibility.Visible;
        }

        private void CloseNotificationItem(object sender, EventArgs e)
        {
            this.Popup.IsOpen = false;
            this.Popup = null;
            Background.Visibility = Visibility.Collapsed;
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
            _height = (int)NotificationDatePanel.ActualHeight;
            NotificationDatePanel.Height = 0;
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
            ViewModel.SelectedSortType = (KeyValueItem)NotificationDateListView.SelectedItems[0];
            GuiAnimation animation = new GuiAnimation(NotificationDatePanel, AnimationType.Size);
            animation.AddStep(NotificationDatePanel.ActualWidth, 0, 20);
            _animations.Add(animation);
            _menuIsOpen = false;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _height = _menuIsOpen ? 0 : (int)NotificationDatePanel.ActualHeight;

            GuiAnimation animation = new GuiAnimation(NotificationDatePanel, AnimationType.Size);
            animation.AddStep(NotificationDatePanel.ActualWidth, _height, 20);
            _animations.Add(animation);
            timer.Start();
            _menuIsOpen = !_menuIsOpen;
        }
        #endregion

        private void GoBack_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PrimaryAlarmNavigate(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.NavigateToPage(typeof(HandlePrimaryView));
        }
    }
}
