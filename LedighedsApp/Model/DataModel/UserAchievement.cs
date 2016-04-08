using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class UserAchievement
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime Date { get; set; }
        public bool HasBeenSeen { get; set; }

        private Achievement _achievementContainer = null;
        [Ignore]
        public Achievement Achievement
        {
            get
            {
                if (_achievementContainer == null)
                {
                    _achievementContainer = Conn.GetSingleItem<Achievement>("SELECT * FROM Achievement WHERE Id = ?", AchievementId);
                }
                return _achievementContainer;
            }
            set { _achievementContainer = value; }
        }

        public UserAchievement() { }
        public UserAchievement(Achievement achievement)
        {
            Achievement = achievement;
            AchievementId = achievement.Id;
            Date = DateTime.Now;
            UserId = User.Instance.Id;
            HasBeenSeen = false;
        }
        
    }
}
