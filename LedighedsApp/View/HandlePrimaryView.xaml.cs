using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.UserControls.BottomBar;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View
{
    public sealed partial class HandlePrimaryView : Page
    {
        private NotificationVm ViewModel;
        private NotificationHandler handler;

        private DateTime CurrentDate = DateTime.Now;
        private Notification PrimaryNotification;
        private bool UpdateCheck = false;

        public HandlePrimaryView()
        {
            this.InitializeComponent();
            ResetBorder.Background = new SolidColorBrush(HexToColor.GetColor("778899"));
            ResetBorder.BorderBrush = new SolidColorBrush(HexToColor.GetColor("778899"));
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ((NotificationVm)(MainGrid.DataContext));
            handler = ViewModel.handler;
            if (handler.PrimaryNotification != null)
            {
                CurrentDate = handler.PrimaryNotification.TriggerTime;
                PrimaryNotification = handler.PrimaryNotification;
                UpdateCheck = true;
            }
            PickDate.Date = CurrentDate.Date;
            PickTime.Time = CurrentDate.TimeOfDay;
            BottomBarSaveItem.ButtonSaveEvent += new EventHandler(SavePrimaryAlarm);
        }

        private void SavePrimaryAlarm(object sender, EventArgs e)
        {
            DateTime tempTime = PickDate.Date.Date.Add(PickTime.Time);
            if (TimeType1.IsChecked != true && TimeType2.IsChecked != true)
            {
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("ERROR_FILL_ALL_FIELDS"));
                return;
            }
            
            Notification tempNotification = new Notification();
            if (PrimaryNotification != null)
                tempNotification.Id = PrimaryNotification.Id;
            tempNotification.UserId = User.Instance.Id;
            tempNotification.Primary = true;
            tempNotification.Text = ViewModel.Content["TEXT_REMEMBER_PRIMARY"].ToString();
            tempNotification.TriggerTime = tempTime;
            bool tempBool = (TimeType1.IsChecked == true);

            if (UpdateCheck)
                if (!handler.UpdateNotification(tempNotification, tempBool))
                    ErrorHandler.ShowError(handler.ErrorMessage);
                else
                    ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("DIALOG_ALARM_UPDATED"));
            else
                if (!handler.AddNotification(tempNotification, tempBool))
                    ErrorHandler.ShowError(handler.ErrorMessage);
                else
                    ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("DIALOG_ALARM_UPDATED"));
        }

        private void GoBack_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ResetPrimaryNotification(object sender, TappedRoutedEventArgs e)
        {
            if (!handler.ResetPrimaryNotification(ViewModel.Content["TEXT_REMEMBER_PRIMARY"].ToString()))
                ErrorHandler.ShowError(handler.ErrorMessage);
            else
            {
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("DIALOG_ALARM_UPDATED"));
                PickDate.Date = handler.PrimaryNotification.TriggerTime.Date;
                PickTime.Time = handler.PrimaryNotification.TriggerTime.TimeOfDay;
            }

        }
    }
}
