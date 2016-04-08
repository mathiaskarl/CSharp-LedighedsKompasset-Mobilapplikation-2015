using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LedighedsApp.Common;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.UserControls;
using LedighedsApp.View.UserControls.BottomBar;

namespace LedighedsApp.ViewModel
{
    public class NotificationVm : Contents
    {
        private List<Notification> _notifications = null;
        private ObservableCollection<KeyValueItem> _sortType;
        private KeyValueItem _selectedSortType = null;
        

        public Notification SelectedNotification { get; set; }
        public Notification PrimaryNotifiation { get { return handler.PrimaryNotification; } }
        

        public ObservableCollection<Notification> Notifications
        {
            get
            {
                if (_notifications == null)
                    _notifications = handler.Notifications;
                return new ObservableCollection<Notification>(_notifications.OrderByDescending(x => x.TriggerTime).Where(x => x.Primary != true).ToList());
            }
            set { _notifications = new List<Notification>(value); }
        }

        
        public ObservableCollection<KeyValueItem> SortType { get { return _sortType; } }

        
        public KeyValueItem SelectedSortType
        {
            get
            {
                return _selectedSortType;
            }
            set
            {
                _selectedSortType = value;
                if (value.Key == 0 || value.Key == 1)
                    _notifications = null;
                else
                    _notifications = handler.GetSpecificNotifications(value);
                OnPropertyChanged("SelectedSortType");
                OnPropertyChanged("Notifications");
            }
        }

        public NotificationHandler handler = new NotificationHandler();

        public NotificationVm()
        {
            _selectedSortType = new KeyValueItem(1, Content["SORT_ALL"].ToString());
            _sortType = new ObservableCollection<KeyValueItem>()
            {
                new KeyValueItem(1, Content["SORT_ALL"].ToString()),
                new KeyValueItem(2, Content["SORT_TODAY"].ToString()),
                new KeyValueItem(3, Content["SORT_MONTH"].ToString()),
                new KeyValueItem(4, Content["SORT_YEAR"].ToString())
            };
            initTimer();
            BottomBarNotification.ButtonDeleteEvent += new EventHandler(DeleteNotification);
            InitTutorialUc(PageName.NotificationView);
        }

        private void DeleteNotification(object sender, EventArgs e)
        {
            if (!handler.RemoveNotification(SelectedNotification))
            {
                ErrorHandler.ShowError(handler.ErrorMessage);
                return;
            }
            handler.Notifications = handler.GetNotifications();
            _notifications.Remove(SelectedNotification);
            SelectedNotification = null;
            OnPropertyChanged("Notifications");
        }

        #region Timer
        private ObservableDictionary _countDown;
        private DispatcherTimer _timer;
        public ObservableDictionary Countdown { get { return _countDown; } set { _countDown = value; OnPropertyChanged("Countdown"); } }

        private void initTimer()
        {
            _timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 1) };
            _timer.Tick += _timer_Tick;
            _timer.Start();
            _timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 1) };
        }

        void _timer_Tick(object sender, object e)
        {
            Countdown = handler.TimeUntilPrimary();
        }
        #endregion
    }
}
