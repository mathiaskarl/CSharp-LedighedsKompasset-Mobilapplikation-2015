using LedighedsApp.Model.DataModel.Translation;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class InfoType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Ignore]
        public string Title { get { return _translation.Title; } }

        private TranslationInfoType _translationContainer = null;
        private TranslationInfoType _translation
        {
            get
            {
                if (_translationContainer == null)
                {
                    string sql =
                        @"SELECT TranslationInfoType.Title FROM TranslationInfoType
                               INNER JOIN Settings ON Settings.LanguageId = TranslationInfoType.LanguageId
                               WHERE InfoTypeId = ?";
                    _translationContainer = Conn.GetSingleItem<TranslationInfoType>(sql, Id);
                }
                return _translationContainer;
            }
        }
        
    }
}
