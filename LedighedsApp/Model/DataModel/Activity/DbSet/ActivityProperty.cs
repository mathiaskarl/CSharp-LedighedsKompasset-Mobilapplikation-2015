using System.Collections.Generic;
using LedighedsApp.Model.DataModel.Translation;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel.Activity.DbSet
{
    public class ActivityProperty
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Property { get; set; }

        public bool AllowNull { get; set; }

        public int Type { get; set; }

        [Ignore]
        public string Name { get { return _translation.Value; } }

        private TranslationActivityProperty _translationContainer = null;
        private TranslationActivityProperty _translation
        {
            get
            {
                if (_translationContainer == null)
                {
                    string sql =
                        @"SELECT TranslationActivityProperty.Value FROM TranslationActivityProperty
                               INNER JOIN Settings ON Settings.LanguageId = TranslationActivityProperty.LanguageId
                               WHERE ActivityPropertyId = ?";
                    _translationContainer = Conn.GetSingleItem<TranslationActivityProperty>(sql, Id);
                }
                return _translationContainer;
            }
        }

        private List<ActivityPropertyTypeValue> _staticPropertyValues = null;

        [Ignore]
        public List<ActivityPropertyTypeValue> StaticPropertyValues
        {
            get
            {
                if (Type == 0 || Type < 2)
                    return null;
                if (_staticPropertyValues == null)
                {
                    string sql =
                        @"SELECT * FROM ActivityPropertyTypeValue
                               WHERE ActivityPropertyTypeValue.PropertyId = ?";
                    _staticPropertyValues = Conn.GetItems<ActivityPropertyTypeValue>(sql, Id);
                }
                return _staticPropertyValues;
            }
        }
    }
}
