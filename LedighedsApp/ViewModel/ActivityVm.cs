using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel.Activity;
using LedighedsApp.Model.DataModel.Activity.Models;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.UserControls;
using LedighedsApp.View.UserControls.BottomBar;

namespace LedighedsApp.ViewModel
{
    public class ActivityVm : Contents
    {
        private List<UserActivity> _userActivities = null;
        private ICalendarItem _selectedActivityItemType = null;
        public ObservableCollection<ICalendarItem> CalendarItemTypes { get { return handler.ActivityItemTypes; } }

        public ObservableCollection<UserActivity> UserActivities
        {
            get
            {
                if (_userActivities == null)
                    _userActivities = handler.UserActivities;
                return new ObservableCollection<UserActivity>(_userActivities.OrderByDescending(x => x.DateTime));
            }
            set { _userActivities = new List<UserActivity>(value); }
        }
        public UserActivity ModifyUserActivity { get; set; }

        public UserActivity SelectedActivity { get; set; }
        public ICalendarItem SelectedActivityType
        {
            get
            {
                return _selectedActivityItemType;
            }
            set
            {
                _selectedActivityItemType = value;
                if (((Activity) value).Id == 0)
                    _userActivities = null;
                else
                    _userActivities = handler.GetSpecificUserActivites(((Activity)value));
                OnPropertyChanged("SelectedActivityType");
                OnPropertyChanged("UserActivities");
            }
        }
        
        public ICalendarItem _staticItemType;

        public ActivityHandler handler = new ActivityHandler();

        public ActivityVm() 
        { 
            _staticItemType = new ActivityContainer() { Title = Content["CALENDAR_ALL"].ToString() };
            _selectedActivityItemType = _staticItemType;
            BottomBarActivity.ButtonDeleteEvent += new EventHandler(DeleteActivity);
            InitTutorialUc(PageName.ActivityView);
        }

        private void DeleteActivity(object sender, EventArgs e)
        {
            if (!handler.RemoveActivity(SelectedActivity))
            {
                ErrorHandler.ShowError(handler.ErrorMessage);
                return;
            }
            handler.UserActivities = handler.GetUserActivites();
            _userActivities.Remove(SelectedActivity);
            SelectedActivity = null;
            OnPropertyChanged("UserActivities");
        }
    }
}
