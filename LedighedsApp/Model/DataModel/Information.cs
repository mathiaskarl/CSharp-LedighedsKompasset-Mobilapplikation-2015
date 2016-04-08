using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using LedighedsApp.Model.DataModel.Translation;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class Information
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int InfoTypeId { get; set; }
        [Ignore]
        public string Text { get { return _translation.Text; } }
        [Ignore]
        public string Title { get { return _translation.Title; } }
        
        private TranslationInformation _translationContainer = null;
        private TranslationInformation _translation
        {
            get
            {
                if (_translationContainer == null)
                {
                    string sql =
                        @"SELECT TranslationInformation.Text, TranslationInformation.Title FROM TranslationInformation
                               INNER JOIN Settings ON Settings.LanguageId = TranslationInformation.LanguageId
                               WHERE InfoId = ?";
                    _translationContainer = Conn.GetSingleItem<TranslationInformation>(sql, Id);
                }
                return _translationContainer;
            }
        }


    }
}
