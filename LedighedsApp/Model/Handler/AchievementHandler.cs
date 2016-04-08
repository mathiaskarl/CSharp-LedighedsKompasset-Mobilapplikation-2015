using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.Handler
{
    class AchievementHandler
    {
        private static List<Achievement> _achievementContainer = null;
        public static List<Achievement> Achievements
        {
            get
            {
                if (_achievementContainer == null)
                    _achievementContainer = PopulateAchievementList();
                return _achievementContainer;
            }
        }

        private static List<UserAchievement> _userAchievementContainer = null;
        public static List<UserAchievement> UserAchievements
        {
            get
            {
                if (_userAchievementContainer == null)
                    _userAchievementContainer = PopulateUserAchievementList();
                return _userAchievementContainer;
            }
        }

        public static bool NewAchievementCheck()
        {
            foreach(UserAchievement obj in UserAchievements)
                if (!obj.HasBeenSeen)
                    return true;
            return false;
        }

        public static void ResetAchievements()
        {
            _userAchievementContainer = PopulateUserAchievementList();
            _achievementContainer = PopulateAchievementList();
        }

        public static void SetToSeen()
        {
            if (NewAchievementCheck())
            {
                List<UserAchievement> tempList = UserAchievements.Where(x => x.HasBeenSeen != true).ToList();
                foreach (UserAchievement obj in tempList)
                    obj.HasBeenSeen = true;
                Conn.UpdateRange(tempList);
            }

        }

        private static List<Achievement> PopulateAchievementList()
        {
            return Conn.GetItems<Achievement>("SELECT * FROM Achievement");
        }
        private static List<UserAchievement> PopulateUserAchievementList()
        {
            return Conn.GetItems<UserAchievement>("SELECT * FROM UserAchievement WHERE UserId = ?", User.Instance.Id);
        }

        public static void UpdateProgress(AchievementType obj)
        {
            Achievement currentAchievement = CheckAchievement(obj);

            if (currentAchievement == null)
                return;

            UserAchievement newUserAchievement = new UserAchievement(currentAchievement);
            UserAchievements.Add(newUserAchievement);
            Conn.Insert(newUserAchievement);
        }

        private static Achievement CheckAchievement(AchievementType obj)
        {
            switch (obj)
            {
                case AchievementType.CreatedOneActivity:
                case AchievementType.CreatedTenActivity:
                case AchievementType.CreatedFiftyActivity:
                    List<UserAchievement> tempList = Conn.GetItems<UserAchievement>("SELECT * FROM UserActivity WHERE UserId = ?", User.Instance.Id);
                    if (tempList.Count >= 50)
                        if (!AlreadyAssigned((int)AchievementType.CreatedFiftyActivity))
                            return GetAchievement((int)AchievementType.CreatedFiftyActivity);

                    if (tempList.Count >= 10)
                        if (!AlreadyAssigned((int)AchievementType.CreatedTenActivity))
                            return GetAchievement((int)AchievementType.CreatedTenActivity);

                    if (tempList.Count >= 1)
                        if (!AlreadyAssigned((int)AchievementType.CreatedOneActivity))
                            return GetAchievement((int)AchievementType.CreatedOneActivity);
                    break;

                case AchievementType.CreatedOneNotification:
                case AchievementType.CreatedTenNotification:
                case AchievementType.CreatedFiftyNotification:
                    List<Notification> tempNotificationList = Conn.GetItems<Notification>("SELECT * FROM Notification WHERE UserId = ?", User.Instance.Id);
                    tempNotificationList = tempNotificationList.Where(x => x.Primary != true).ToList();
                    if (tempNotificationList.Count >= 50)
                        if (!AlreadyAssigned((int)AchievementType.CreatedFiftyNotification))
                            return GetAchievement((int)AchievementType.CreatedFiftyNotification);

                    if (tempNotificationList.Count >= 10)
                        if (!AlreadyAssigned((int)AchievementType.CreatedTenNotification))
                            return GetAchievement((int)AchievementType.CreatedTenNotification);

                    if (tempNotificationList.Count >= 1)
                        if (!AlreadyAssigned((int)AchievementType.CreatedOneNotification))
                            return GetAchievement((int)AchievementType.CreatedOneNotification);
                    break;
            }
            return null;
        }

        private static Achievement GetAchievement(int id)
        {
            return Achievements.FirstOrDefault(data => data.Id == id);
        }

        private static bool AlreadyAssigned(int id)
        {
            return (UserAchievements.FirstOrDefault(data => data.AchievementId == id) != null);
        }


    }
}