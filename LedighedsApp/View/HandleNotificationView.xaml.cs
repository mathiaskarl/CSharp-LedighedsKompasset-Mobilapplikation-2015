using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.UserControls.BottomBar;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View
{
    public sealed partial class HandleNotificationView : Page
    {
        private NotificationVm ViewModel;
        private NotificationHandler handler;

        private Notification CurrentNotification;
        private DateTime CurrentDate = DateTime.Now;
        private bool UpdateCheck = false;

        public HandleNotificationView()
        {
            this.InitializeComponent();
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((NotificationVm)(MainGrid.DataContext));
            MainGrid.DataContext = ViewModel;
            handler = ViewModel.handler;
            BottomBarSaveItem.ButtonSaveEvent += new EventHandler(SaveNotification);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
                if (e.Parameter.GetType() == typeof (Notification))
                {
                    UpdateCheck = true;
                    CurrentNotification = (Notification) e.Parameter;
                    Text.Text = CurrentNotification.Text;
                }
                else if (e.Parameter is DateTime)
                    CurrentDate = (DateTime) e.Parameter;

            PickDate.Date = (CurrentNotification == null ? CurrentDate.Date : CurrentNotification.TriggerTime.Date);
            PickTime.Time = (CurrentNotification == null ? CurrentDate.TimeOfDay : CurrentNotification.TriggerTime.TimeOfDay);
        }

        private void SaveNotification(object sender, EventArgs e)
        {
            DateTime tempTime = PickDate.Date.Date.Add(PickTime.Time);
            CurrentNotification = CurrentNotification ?? new Notification();
            CurrentNotification.UserId = User.Instance.Id;
            CurrentNotification.Text = Text.Text;
            CurrentNotification.TriggerTime = tempTime;

            if(UpdateCheck)
                if (!handler.UpdateNotification(CurrentNotification))
                    ErrorHandler.ShowError(handler.ErrorMessage);
                else
                    ErrorHandler.DialogGoBack(ViewModel.Content["DIALOG_ALARM_UPDATED"].ToString());
            else
                if (!handler.AddNotification(CurrentNotification))
                    ErrorHandler.ShowError(handler.ErrorMessage);
                else
                    ErrorHandler.DialogGoBack(ViewModel.Content["DIALOG_ALARM_CREATED"].ToString());
        }

        private void GoBack_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
