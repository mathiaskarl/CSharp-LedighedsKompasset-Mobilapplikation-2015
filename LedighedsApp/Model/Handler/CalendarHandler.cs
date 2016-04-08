using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Activity;
using LedighedsApp.Model.DataModel.Activity.Models;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.Handler
{
    public class CalendarHandler
    {
        public ObservableCollection<KeyValuePair<DateTime, ICalendarItem>> CalendarDictionary = new ObservableCollection<KeyValuePair<DateTime, ICalendarItem>>();
        public ObservableCollection<ICalendarItem> CalendarItemTypes { get { return GenerateCalendarTypes(); } }

        public CalendarHandler() { GenerateItems(); }

        public void GenerateItems()
        {
            List<Notification> Notifications = Conn.GetItems<Notification>("SELECT * FROM Notification WHERE UserId = ?", User.Instance.Id);
                foreach(Notification objn in Notifications)
                    AddToDictionary(objn.TriggerTime, objn);

            List<UserActivity> UserActivities = Conn.GetItems<UserActivity>("SELECT * FROM UserActivity WHERE UserId = ?", User.Instance.Id);
            foreach (UserActivity obj in UserActivities)
                AddToDictionary(obj.DateTime, obj.Activity);

            CalendarDictionary = SortItems(CalendarDictionary);
        }

        public void RefreshCalendarDictionary()
        {
            CalendarDictionary = new ObservableCollection<KeyValuePair<DateTime, ICalendarItem>>();
            GenerateItems();
        }

        private ObservableCollection<ICalendarItem> GenerateCalendarTypes()
        {
            ObservableCollection<ICalendarItem> tempList = new ObservableCollection<ICalendarItem>();
            tempList.Add(new Notification());
            foreach(Activity obj in Conn.GetItems<Activity>("SELECT * FROM Activity"))
                tempList.Add(obj);
            return tempList;
        }

        private void AddToDictionary(DateTime date, ICalendarItem item)
        {
            CalendarDictionary.Add(new KeyValuePair<DateTime, ICalendarItem>(date, item));
        }

        public ObservableCollection<KeyValuePair<DateTime, ICalendarItem>> SortItems(ObservableCollection<KeyValuePair<DateTime, ICalendarItem>> list)
        {
            return new ObservableCollection<KeyValuePair<DateTime, ICalendarItem>>(list.OrderBy(x => x.Key).ToList());
        }

        public ObservableCollection<ICalendarItem> GetDateItems(ObservableCollection<KeyValuePair<DateTime, ICalendarItem>> list, DateTime date, ICalendarItem item = null)
        {
            ObservableCollection<ICalendarItem> tempList = new ObservableCollection<ICalendarItem>();
            foreach(KeyValuePair<DateTime, ICalendarItem> obj in list)
                if (obj.Key.Date == date.Date)
                {
                    if (item != null)
                    {
                        if (item.GetType() == typeof (ActivityContainer))
                        {
                            tempList.Add(obj.Value);
                            continue;
                        }

                        if (item.GetType() == obj.Value.GetType() && item.GetType() != typeof (Activity))
                        {
                            tempList.Add(obj.Value);
                            continue;
                        }

                        if(item.GetType() == typeof(Activity) && obj.Value.GetType() == typeof(Activity))
                            if(((Activity)item).Id == ((Activity)obj.Value).Id)
                            {
                                tempList.Add(obj.Value);
                            }
                    }
                    else
                        tempList.Add(obj.Value);
                }
            return tempList;
        }

        public Dictionary<TimeSpan, ICalendarItem> GetHourItems(ObservableCollection<KeyValuePair<DateTime, ICalendarItem>> list, DateTime date, int from, ICalendarItem item = null)
        {
            Dictionary<TimeSpan, ICalendarItem> tempList = new Dictionary<TimeSpan, ICalendarItem>();
            foreach (KeyValuePair<DateTime, ICalendarItem> obj in list)
                if (date.Date == obj.Key.Date && obj.Key.Hour == from)
                {
                    TimeSpan ProperTime = ExistsInDictionary(tempList, obj.Key.TimeOfDay);
                    if (item != null)
                    {
                        if (item.GetType() == typeof(ActivityContainer))
                        {
                            tempList.Add(ProperTime, obj.Value);
                            continue;
                        }

                        if (item.GetType() == obj.Value.GetType() && item.GetType() != typeof(Activity))
                        {
                            tempList.Add(ProperTime, obj.Value);
                            continue;
                        }

                        if (item.GetType() == typeof(Activity) && obj.Value.GetType() == typeof(Activity))
                            if (((Activity)item).Id == ((Activity)obj.Value).Id)
                            {
                                tempList.Add(ProperTime, obj.Value);
                            }
                    }
                    else
                        tempList.Add(ProperTime, obj.Value);
                }
            return tempList;
        }

        private TimeSpan ExistsInDictionary(Dictionary<TimeSpan, ICalendarItem> list, TimeSpan time)
        {
            TimeSpan tempTimeSpan = time;
            bool checkBool = Exists(list, time);
            while (checkBool)
            {
                 tempTimeSpan = tempTimeSpan.Add(new TimeSpan(0, 0, 0, 1));
                checkBool = Exists(list, tempTimeSpan);
            }
            return tempTimeSpan;
        }

        private bool Exists(Dictionary<TimeSpan, ICalendarItem> list, TimeSpan time)
        {
            return list.ContainsKey(time);
        }
    }
}
