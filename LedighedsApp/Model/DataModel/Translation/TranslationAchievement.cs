using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel.Translation
{
    public class TranslationAchievement
    {

        public int AchievementId { get; set; }
        public int LanguageId { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
