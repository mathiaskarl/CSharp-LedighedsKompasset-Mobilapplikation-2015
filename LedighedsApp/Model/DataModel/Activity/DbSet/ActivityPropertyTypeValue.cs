using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel.Translation;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel.Activity.DbSet
{
    public class ActivityPropertyTypeValue
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PropertyId { get; set; }

        [Ignore]
        public string Name { get { return _translation.Value; } }

        private TranslationActivityPropertyTypeValue _translationContainer = null;
        private TranslationActivityPropertyTypeValue _translation
        {
            get
            {
                if (_translationContainer == null)
                {
                    string sql =
                        @"SELECT TranslationActivityPropertyTypeValue.Value FROM TranslationActivityPropertyTypeValue
                               INNER JOIN Settings ON Settings.LanguageId = TranslationActivityPropertyTypeValue.LanguageId
                               WHERE ActivityPropertyTypeValueId = ?";
                    _translationContainer = Conn.GetSingleItem<TranslationActivityPropertyTypeValue>(sql, Id);
                }
                return _translationContainer;
            }
        }
    }
}
