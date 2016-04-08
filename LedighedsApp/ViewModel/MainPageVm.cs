using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using LedighedsApp.Common;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.Animation;
using LedighedsApp.View.UserControls;

namespace LedighedsApp.ViewModel
{
    class MainPageVm : Contents
    {
        private ObservableDictionary _countdown;
        private NotificationHandler _notificationHandler;
        public DispatcherTimer _timer;
        public bool NewAchievement { get { return AchievementHandler.NewAchievementCheck(); } }

        public ObservableDictionary Countdown { get { return _countdown; } set { _countdown = value; OnPropertyChanged("Countdown"); }}
        
        public MainPageVm()
        {
            InitTutorialUc(PageName.MainPage);
            _notificationHandler = new NotificationHandler();
            InitTimer();
        }

        private void InitTimer()
        {
            _timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 1) };
            _timer.Tick += _timer_Tick;
            _timer.Start();
            _timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 1) };
        }
   
        void _timer_Tick(object sender, object e)
        {
                Countdown = _notificationHandler.TimeUntilPrimary();    
        }

        
    }
}
