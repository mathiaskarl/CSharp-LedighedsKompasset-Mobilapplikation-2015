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
    public class Tutorial
    {
        public bool HasBeenSeen { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(1)]
        public int StepNum { get; set; }
        [Ignore]
        public string Text 
        {
            get {
                return _translation != null ? _translation.Text : "Just some placeholder data";
            }
        }
        
        public PageName PageName
        {
            get { return (PageName) PageId; }
        }

        public int PageId { get; set; }
        private TranslationTutorial _translationContainer = null;
        private TranslationTutorial _translation
        {
            get
            {
                string sql =
                    @"SELECT TranslationTutorial.Text, TranslationTutorial.LanguageId FROM TranslationTutorial
                            INNER JOIN Settings ON Settings.LanguageId = TranslationTutorial.LanguageId
                            WHERE TutorialId = ? AND Settings.Id = ?";
                _translationContainer = Conn.GetSingleItem<TranslationTutorial>(sql, Id, 1);
                return _translationContainer;
            }
        }
    }
}
