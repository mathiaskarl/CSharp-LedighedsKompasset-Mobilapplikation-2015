using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.Handler;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View.UserControls.BottomBar
{
    public sealed partial class BottomBarNotification : UserControl
    {
        public NotificationVm ViewModel;

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

        public BottomBarNotification()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((NotificationVm)(MainGrid.DataContext));
        }

        private void ButtonDelete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.SelectedNotification == null)
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("ERROR_SELECT_ACTIVITY"));
            else
            {
                if (_buttonDeleteEvent != null)
                    _buttonDeleteEvent(this, EventArgs.Empty);
            }
        }

        private void ButtonEdit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.SelectedNotification == null)
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("ERROR_SELECT_ACTIVITY"));
            else
                NavigationService.NavigateToPage(typeof(HandleNotificationView), ViewModel.SelectedNotification);
        }

        private void ButtonAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.NavigateToPage(typeof(HandleNotificationView));
        }

        private void ButtonShow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.SelectedNotification == null)
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("ERROR_SELECT_ACTIVITY"));
            else
                if (_buttonShowEvent != null)
                    _buttonShowEvent(this, EventArgs.Empty);
        }
    }
}
