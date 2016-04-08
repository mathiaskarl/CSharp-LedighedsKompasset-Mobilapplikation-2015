using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.Handler;
using LedighedsApp.ViewModel;

namespace LedighedsApp.View
{
    public sealed partial class AchievementView : Page
    {
        public AchievementView()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void GoBack_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            AchievementHandler.SetToSeen();
        }
    }
}
