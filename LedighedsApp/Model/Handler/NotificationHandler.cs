using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Common;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.Handler
{
    public class NotificationHandler
    {
        private List<Notification> _notifications = null;
        private DateTime NewTime;

        public List<Notification> Notifications
        {
            get
            {
                if (_notifications == null)
                    _notifications = GetNotifications();
                return _notifications;
            }
            set { _notifications = value; }
        }

        public Notification PrimaryNotification;
        public string ErrorMessage;

        public NotificationHandler()
        {
            SetPrimaryNotification();
            //PopulateAlarmList();
        }

        public List<Notification> GetNotifications()
        {
            return Conn.GetItems<Notification>("SELECT * FROM Notification WHERE UserId = ?", User.Instance.Id);
        }

        public List<Notification> GetSpecificNotifications(KeyValueItem obj)
        {
            switch (obj.Key)
            {
                case 2:
                    return Notifications.Where(x => x.TriggerTime.Date == DateTime.Now.Date).ToList();
                case 3:
                    return Notifications.Where(x => x.TriggerTime.Month == DateTime.Now.Month && x.TriggerTime.Year == DateTime.Now.Year).ToList();
                case 4:
                    return Notifications.Where(x => x.TriggerTime.Year == DateTime.Now.Year).ToList();
                default:
                    return Notifications;
            }
        }

        public bool AddNotification(Notification notification, bool lastCheckIn = false)
        {
            try
            {
                if (String.IsNullOrEmpty(notification.Text) && !notification.Primary)
                    throw new Exception("ERROR_FILL_ALL_FIELDS");

                if (notification.Primary)
                    if (PrimaryNotification != null)
                    throw new Exception("ERROR_PRIMARY_ALREADY_EXISTS");

                if (lastCheckIn && notification.TriggerTime > DateTime.Now)
                    throw new Exception("ERROR_PAST_TIME");

                if (!lastCheckIn && notification.TriggerTime < DateTime.Now)
                    throw new Exception("ERROR_FUTURE_TIME");

                if (!TimeCheck(notification.TriggerTime, lastCheckIn))
                        throw new Exception("ERROR_TIME_INCORRECT");

                if (lastCheckIn)
                    notification.TriggerTime = NewTime;

                Conn.Insert(notification);
                ToastService.SetToastForNotification(notification);

                if (notification.Primary)
                    SetPrimaryNotification();
                else
                    AchievementHandler.UpdateProgress(AchievementType.CreatedOneNotification);
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.ReturnErrorMessage(ex.Message);
                return false;
            }
            return true;
        }

        public bool RemoveNotification(Notification notification)
        {
            Notification tempNotification = Conn.GetSingleItem<Notification>("SELECT * FROM Notification WHERE Id = ?", notification.Id);
            try
            {
                if (tempNotification == null)
                    throw new Exception("ERROR_ALARM_DOESNT_EXIST");

                if (tempNotification.Primary)
                    throw new Exception("ERROR_PRIMARY_ALARM_DELETE");

                ToastService.RemoveToastFromSchedule(notification);
                Conn.Delete(notification);
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.ReturnErrorMessage(ex.Message);
                return false;
            }
            return true;
        }

        public bool UpdateNotification(Notification notification, bool lastCheckIn = false)
        {
            Notification tempNotification = Conn.GetSingleItem<Notification>("SELECT * FROM Notification WHERE Id = ?", notification.Id);
            try
            {
                if (tempNotification == null)
                    throw new Exception("ERROR_ALARM_DOESNT_EXIST");

                if (String.IsNullOrEmpty(notification.Text) && !notification.Primary)
                    throw new Exception("ERROR_FILL_ALL_FIELDS");

                if (lastCheckIn && notification.TriggerTime > DateTime.Now)
                    throw new Exception("ERROR_PAST_TIME");

                if (!lastCheckIn && notification.TriggerTime < DateTime.Now)
                    throw new Exception("ERROR_FUTURE_TIME");

                if (!TimeCheck(notification.TriggerTime, lastCheckIn))
                    throw new Exception("ERROR_TIME_INCORRECT");

                if (lastCheckIn)
                    notification.TriggerTime = NewTime;

                ToastService.RemoveToastFromSchedule(notification);
                ToastService.SetToastForNotification(notification);
                Conn.Update(notification);

                if (notification.Primary)
                    SetPrimaryNotification();
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.ReturnErrorMessage(ex.Message);
                return false;
            }
            return true;

        }

        private bool TimeCheck(DateTime time, bool lastCheckIn)
        {
            if (lastCheckIn)
            {
                if (time <= DateTime.Now)
                {
                    bool tempCheck = true;
                    while (tempCheck)
                    {
                        time = time.Add(new TimeSpan(7, 0, 0, 0));
                        if (time > DateTime.Now)
                            tempCheck = false;
                    }
                }
                NewTime = time;
                return true;
            }

            if (time <= DateTime.Now)
                return false;
            return true;
        }

        private void SetPrimaryNotification()
        {
            Notification tempNotification = Conn.GetSingleItem<Notification>("SELECT * FROM Notification WHERE UserId = ? AND [Primary] = ?", User.Instance.Id, true);
            if (tempNotification != null)
            {
                if (tempNotification.TriggerTime <= DateTime.Now && tempNotification.TriggerTime.Date != DateTime.Now.Date)
                {
                    UpdateNotification(tempNotification, true);
                    tempNotification = Conn.GetSingleItem<Notification>("SELECT * FROM Notification WHERE UserId = ? AND [Primary] = ?", User.Instance.Id, true);
                }
                PrimaryNotification = tempNotification;
            }
        }

        public bool ResetPrimaryNotification(string text)
        {
            if (PrimaryNotification != null)
            {
                DateTime tempDateTime = DateTime.Now;
                tempDateTime = tempDateTime.Add(new TimeSpan(7, 0, 0, 0));
                PrimaryNotification.TriggerTime = tempDateTime;
                PrimaryNotification.Text = text;
                if (!UpdateNotification(PrimaryNotification))
                    return false;
            }
            return true;
        }

        public ObservableDictionary TimeUntilPrimary()
        {
            ObservableDictionary tempDic = new ObservableDictionary();
            if (PrimaryNotification != null)
            {
                if (PrimaryNotification.TriggerTime < DateTime.Now)
                    SetPrimaryNotification();

                DateTime timeleft = new DateTime(PrimaryNotification.TriggerTime.Year, PrimaryNotification.TriggerTime.Month, PrimaryNotification.TriggerTime.Day, 23, 59, 59);
                var timeLeft = timeleft.Subtract(DateTime.Now);
                tempDic["DAY_NUMBER"] = timeLeft.Days;
                tempDic["HOUR_NUMBER"] = timeLeft.Hours;
                tempDic["MINUTE_NUMBER"] = timeLeft.Minutes;
                tempDic["SECOND_NUMBER"] = timeLeft.Seconds;
            }

            return tempDic;
        }
    }
}
