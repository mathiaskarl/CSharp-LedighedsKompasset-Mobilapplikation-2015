// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.UserControls;
using LedighedsApp.View.UserControls.BottomBar;

namespace LedighedsApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            TimerBackground.BorderBrush = new SolidColorBrush(HexToColor.GetColor("778899"));
            TimerBackground.Background = new SolidColorBrush(HexToColor.GetColor("778899"));
        }

        private void PrimaryAlarmNavigate(object sender, TappedRoutedEventArgs e)
        {
            WaterUc.Stop();
            NavigationService.NavigateToPage(typeof(HandlePrimaryView));
        }

        private void Navigate(object sender, TappedRoutedEventArgs e)
        {
            WaterUc.Stop();
            switch (((Button) sender).DataContext.ToString())
            {
                case "CalendarView":
                    NavigationService.NavigateToPage(typeof(CalendarView));
                    break;

                case "ActivityView":
                    NavigationService.NavigateToPage(typeof(ActivityView));
                    break;

                case "SettingsView":
                    NavigationService.NavigateToPage(typeof(SettingsView));
                    break;

                case "NotificationView":
                    NavigationService.NavigateToPage(typeof(NotificationView));
                    break;

                case "InfoCategoryView":
                    NavigationService.NavigateToPage(typeof(InfoCategoryView));
                    break;

                case "AchievementView":
                    NavigationService.NavigateToPage(typeof(AchievementView));
                    break;
            } 
        }
    }
}
