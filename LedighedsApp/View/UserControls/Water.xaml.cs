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
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Activity;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236


namespace LedighedsApp.View.UserControls
{
    public sealed partial class Water : UserControl
    {
        private WaterSystem _water = new WaterSystem();
        private DispatcherTimer _timer;

        public bool RunOnce { get; set; }
        public int Offset { set { _water.Offset = value; } }
        public int WaterHeight { set { _water.CurrentWaterHeight = value; } }
        public int NextWaterHeight { set { _water.SetWaterHeight(value); } }

        public Water()
        {
            this.InitializeComponent();
            Offset = 0;
            WaterHeight = 685;
            NextWaterHeight = 685;
        }

        public void Stop()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        private void WaterCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            _water.InitWater(WaterCanvas);
            SetLevels();
            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 20)};
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            _water.Update();

            if (!User.Instance.Settings.Animation)
            {
                _timer.Stop();
            }
        }
        
        private void SetLevels()
        {
            int height = NewHeight();
            if (User.Instance.Settings.Animation)
            {
                if (User.Instance.Settings.IsInitialBoot)
                {
                    _water.CurrentWaterHeight = User.Instance.Settings.PreviousWaterHeight;
                    _water.SetWaterHeight(height);
                    User.Instance.Settings.PreviousWaterHeight = height;
                    User.Instance.Settings.IsInitialBoot = false;
                }
                else
                {
                    _water.CurrentWaterHeight = User.Instance.Settings.PreviousWaterHeight;
                    _water.SetWaterHeight(height);
                    User.Instance.Settings.PreviousWaterHeight = height;
                }
            }
            else
            {
                if (User.Instance.Settings.IsInitialBoot)
                {
                    _water.CurrentWaterHeight = height;
                    User.Instance.Settings.PreviousWaterHeight = height;
                    User.Instance.Settings.IsInitialBoot = false;
                }
                else
                {
                    _water.CurrentWaterHeight = height;
                    User.Instance.Settings.PreviousWaterHeight = height;
                }
            }
        }

        private int NewHeight()
        {
            ActivityHandler activityHandler = new ActivityHandler();
            DateTime date = DateTime.Today;
            double coefficient = 680.0/User.Instance.Settings.ActivityAmount;

            while (User.Instance.Settings.WaterResetDay != (int) date.DayOfWeek)
            {
                date = date.Subtract(TimeSpan.FromDays(1));
            }

            int count = activityHandler.UserActivities.Count(a => a.DateTime.Date >= date && a.DateTime.Date <= DateTime.Now);

            if (count >= User.Instance.Settings.ActivityAmount)
            {
                return User.Instance.Settings.ActivityAmount+1;
            }

            return (int) (680.0 - (coefficient*count));
        }

    }
}
