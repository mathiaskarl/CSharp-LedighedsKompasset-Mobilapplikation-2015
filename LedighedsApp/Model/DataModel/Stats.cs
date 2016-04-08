using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class Stats
    {
        public int UserId { get; set; }
        public int NumberOfActivities { get; set; }
        public int NumberOfNotifications { get; set; }
        public int TutorialsCompleted { get; set; }
        public int ActiveUseage { get; set; }
        public int PassiveUseage { get; set; }
        
        [Ignore]
        public TimeSpan ActiveUseageTimeSpan { get; set; }
        
        [Ignore]
        public TimeSpan PassiveUseageTimeSpan { get; set; }
        
        [Ignore]
        public List<NavigationStep> NavigationSteps { get; set; }

        public Stats()
        {
            PopulateNavigationSteps();
            ActiveUseageTimeSpan = TimeSpan.FromMinutes(ActiveUseage);
            PassiveUseageTimeSpan = TimeSpan.FromMinutes(PassiveUseage);
        }

        /*
        public void AddNavigationStep(PageName from, PageName to)
        {
            NavigationStep navStep = new NavigationStep(from,to);
            NavigationSteps.Add(navStep);
            Conn.Insert(navStep);
        }
        */  

        public void UpdateActiveUseage(TimeSpan time)
        {
            ActiveUseageTimeSpan += time;
            ActiveUseage = (int)ActiveUseageTimeSpan.TotalMinutes;
        }

        public void UpdatePassiveUseage(DateTime createTime)
        {
            PassiveUseageTimeSpan = DateTime.Now - createTime;
            PassiveUseage = (int)PassiveUseageTimeSpan.TotalMinutes;
        }

        private void PopulateNavigationSteps()
        {
            NavigationSteps = new List<NavigationStep>();
            NavigationSteps = Conn.GetItems<NavigationStep>("SELECT * FROM NavigationStep WHERE UserId = ?", User.Instance.Id);
        }

        public void Update()
        {
            Conn.Update(this);
        }




    }
}
