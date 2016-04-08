using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Translation;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class Achievement
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Icon { get; set; }
        public string Title { get { return _translation.Title; } }
        public string Text { get { return _translation.Text; } }

        private TranslationAchievement _translationContainer = null;
        private TranslationAchievement _translation
        {
            get
            {
                if (_translationContainer == null)
                {
                    string sql =
                        @"SELECT TranslationAchievement.Title, TranslationAchievement.Text FROM TranslationAchievement
                               INNER JOIN Settings ON Settings.LanguageId = TranslationAchievement.LanguageId
                               WHERE AchievementId = ?";
                    _translationContainer = Conn.GetSingleItem<TranslationAchievement>(sql, Id);
                }
                return _translationContainer;
            }
        }
    }
}
