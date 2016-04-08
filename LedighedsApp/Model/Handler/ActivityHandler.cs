using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Activity;
using LedighedsApp.Model.DataModel.Activity.DbSet;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.Handler
{
    public class ActivityHandler
    {
        public ObservableCollection<ICalendarItem> ActivityItemTypes { get { return GenerateActivityTypes(); } }
        private List<UserActivity> _userActivities = null;

        public List<UserActivity> UserActivities
        {
            get
            {
                if (_userActivities == null)
                    _userActivities = GetUserActivites();
                return _userActivities;
            }
            set { _userActivities = value; }
        }

        public string ErrorMessage;

        public List<UserActivity> GetSpecificUserActivites(Activity obj)
        {
            return UserActivities.Where(x => x.ActivityId == obj.Id).ToList();
        }

        public List<UserActivity> GetUserActivites()
        {
            return Conn.GetItems<UserActivity>("SELECT * FROM UserActivity WHERE UserId = ?", User.Instance.Id);
        }


        private ObservableCollection<ICalendarItem> GenerateActivityTypes()
        {
            ObservableCollection<ICalendarItem> tempList = new ObservableCollection<ICalendarItem>();
            foreach (Activity obj in Conn.GetItems<Activity>("SELECT * FROM Activity"))
                tempList.Add(obj);
            return tempList;
        }

        public bool AddActivity(Activity activity, DateTime dateTime)
        {
            try
            {
                List<ActivityPropertyValue> tempList = new List<ActivityPropertyValue>();
                foreach (KeyValuePair<string, string> obj in activity.PropertyValues)
                {
                    ActivityProperty currentObj = activity.Properties.FirstOrDefault(data => data.Property == obj.Key);
                    if (String.IsNullOrEmpty(obj.Value) && !currentObj.AllowNull)
                    {
                        throw new Exception("ERROR_FILL_ALL_FIELDS");
                    }
                    tempList.Add(new ActivityPropertyValue() {ActivityPropertyId = currentObj.Id, Value = obj.Value});
                }
                UserActivity uaObj = new UserActivity(){ ActivityId = activity.Id, UserId = User.Instance.Id, DateTime = dateTime };
                Conn.Insert(uaObj);

                foreach (ActivityPropertyValue obj in tempList)
                    obj.UserActivityId = uaObj.Id;
                
                Conn.InsertRange(tempList);
                AchievementHandler.UpdateProgress(AchievementType.CreatedOneActivity);
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.ReturnErrorMessage(ex.Message);
                return false;
            }
            return true;
        }

        public bool RemoveActivity(object obj)
        {
            int Id = (obj.GetType() == typeof (UserActivity) ? ((UserActivity) obj).Id : (int) obj);
            UserActivity userActivity = Conn.GetSingleItem<UserActivity>("SELECT Id FROM UserActivity WHERE Id = ?", Id);
            try
            {
                if (userActivity == null)
                {
                    throw new Exception("ERROR_ACTIVITY_DOESNT_EXIST");
                }
                Conn.DeleteRange(Conn.GetItems<ActivityPropertyValue>("SELECT * FROM ActivityPropertyValue WHERE UserActivityId = ?", Id));
                Conn.Delete(userActivity);
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.ReturnErrorMessage(ex.Message);
                return false; 
            }
            return true;
        }

        public bool UpdateActivity(UserActivity userActivity)
        {
            try
            {
                if (!(Conn.GetSingleItem<UserActivity>("SELECT Id FROM UserActivity WHERE Id = ?", userActivity.Id).Id > 0))
                {
                    throw new Exception("ERROR_ACTIVITY_DOESNT_EXIST");
                }
                List<ActivityPropertyValue> tempList = Conn.GetItems<ActivityPropertyValue>("SELECT * FROM ActivityPropertyValue WHERE UserActivityId = ?", userActivity.Id);
                foreach (KeyValuePair<string, string> obj in userActivity.Activity.PropertyValues)
                {
                    ActivityProperty currentObj = userActivity.Activity.Properties.FirstOrDefault(data => data.Property == obj.Key);
                    if (String.IsNullOrEmpty(obj.Value) && !currentObj.AllowNull)
                    {
                        throw new Exception("ERROR_FILL_ALL_FIELDS");
                    }
                    ActivityPropertyValue editObj = tempList.FirstOrDefault(x => x.ActivityPropertyId == currentObj.Id);
                    editObj.Value = obj.Value;
                }
                Conn.UpdateRange(tempList);
                Conn.Update(userActivity);
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.ReturnErrorMessage(ex.Message);
                return false;
            }
            return true;

        }
    }
}
