using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Annotations;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.Handler;

namespace LedighedsApp.ViewModel
{
    class AchievementVm : Contents
    {
        private UserAchievement _selectedAchievement;

        public UserAchievement SelectedAchievement
        {
            get { return _selectedAchievement; }
            set
            {
                if (Equals(value, _selectedAchievement)) return;
                _selectedAchievement = value;
                OnPropertyChanged("SelectedAchievement");
            }
        }

        public ObservableCollection<UserAchievement> EarnedAchievements { get; set; }
        public ObservableCollection<Achievement> UnearnedAchievements { get; set; }
        public AchievementHandler AchievementHandler { get; set; }
        public string UnearnedAchievementImage { get; set; }


        public AchievementVm()
        {
            EarnedAchievements = new ObservableCollection<UserAchievement>();
            UnearnedAchievements = new ObservableCollection<Achievement>();
            AchievementHandler = new AchievementHandler();
            UnearnedAchievementImage = "Unearned.png";

            PopulateLists();
        }

        private void PopulateLists()
        {
            var ids = new List<int>();

            foreach (var userAchievement in AchievementHandler.UserAchievements)
            {
                EarnedAchievements.Add(userAchievement);
                ids.Add(userAchievement.AchievementId);
            }
            EarnedAchievements = new ObservableCollection<UserAchievement>(EarnedAchievements.OrderByDescending(x => x.Date));

            foreach (var achivement in AchievementHandler.Achievements)
            {
                if (!ids.Contains(achivement.Id))
                {
                    UnearnedAchievements.Add(achivement);
                }
            }
        }

    }
}
