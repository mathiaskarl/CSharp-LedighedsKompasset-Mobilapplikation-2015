using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel.Activity.Models;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel.Activity
{
    public class UserActivity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        [Ignore]
        public string Date { get { return this.DateTime.Day + "/" + this.DateTime.Month + "/" + this.DateTime.Year; } }

        private Activity _activity;

        [Ignore]
        public Activity Activity {
            get
            {
                if (_activity == null)
                    _activity = new Activity(ActivityId, Id);
                return _activity;
            } set { _activity = value; } }
    }


}
