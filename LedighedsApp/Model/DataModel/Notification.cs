using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class Notification : ICalendarItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime TriggerTime { get; set; }
        public bool Primary { get; set; }

        [Ignore]
        public SolidColorBrush RGBColor { get { return new SolidColorBrush(HexToColor.GetColor("#000000")); } }
        
        [Ignore]
        public string Date { get { return TriggerTime.Day + "/" + TriggerTime.Month + "/" + TriggerTime.Year ; } }

        [Ignore]
        public string Time { get { return (TriggerTime.Hour < 10 ? "0" + TriggerTime.Hour.ToString() : TriggerTime.Hour.ToString()) + ":" + (TriggerTime.Minute < 1 ? "00" : (TriggerTime.Minute < 10 ? "0" + TriggerTime.Minute.ToString() : TriggerTime.Minute.ToString())); } }


        [Ignore]
        public string Title { get { return "Alarm"; } }

        [Ignore]
        public string Header { get { return Text; } }

        public Notification(int userId, string text, DateTime triggerTime)
        {
            UserId = userId;
            Text = text;
            TriggerTime = triggerTime;
        }

        public Notification() { }
    }
}
