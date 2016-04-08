using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel.Activity.DbSet;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.DataModel.Translation;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel.Activity
{
    public class Activity : ICalendarItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Color { get; set; }
        [Ignore]
        public SolidColorBrush RGBColor { get { return new SolidColorBrush(HexToColor.GetColor(Color)); } }
        [Ignore]
        public string Title { get { return _translation.Title; } }
        [Ignore]
        public string Header { get { return PropertyValues["Title"]; } set { PropertyValues["Title"] = value; } }
        [Ignore]
        public int UserActivityId { get; set; }

        public Activity() { }
        public Activity(int id, int rowId)
        {
            try
            {
                Activity tempActivity = Conn.GetSingleItem<Activity>("SELECT * FROM Activity WHERE Id = ?", id);
                this.Id = tempActivity.Id;
                this.Color = tempActivity.Color;
                this.UserActivityId = rowId;
            }
            catch (Exception ex)
            {
            }
        }

        #region GetSetters
        private TranslationActivity _translationContainer = null;
        private TranslationActivity _translation
        {
            get
            {
                if (_translationContainer == null)
                {
                    string sql =
                        @"SELECT TranslationActivity.Title FROM TranslationActivity
                               INNER JOIN Settings ON Settings.LanguageId = TranslationActivity.LanguageId
                               WHERE ActivityId = ?";
                    _translationContainer = Conn.GetSingleItem<TranslationActivity>(sql, Id);
                }
                return _translationContainer;
            }
        }

        private List<ActivityProperty> _propertiesContainer = null;
        public List<ActivityProperty> Properties
        {
            get
            {
                if(_propertiesContainer == null)
                    _propertiesContainer = Conn.GetItems<ActivityProperty>(@"SELECT ActivityProperty.Property, ActivityProperty.Id, ActivityProperty.AllowNull, ActivityProperty.Type FROM ActivityProperty
                                                                    INNER JOIN ActivityProperties ON ActivityProperties.PropertyId = ActivityProperty.Id
                                                                    WHERE ActivityProperties.ActivityId = ?", Id);
                return _propertiesContainer;
            }
        }

        private Dictionary<string, string> _propertyValueContainer = null;
        public Dictionary<string, string> PropertyValues
        {
            get
            {
                Dictionary<string, string> tempDic = new Dictionary<string, string>();
                if (this.UserActivityId < 1)
                {
                    if (_propertyValueContainer == null)
                    {
                        foreach (ActivityProperty obj in Properties)
                            tempDic.Add(obj.Property, null);
                        _propertyValueContainer = tempDic;
                    }
                    return _propertyValueContainer;
                }

                if (_propertyValueContainer == null)
                {
                    List<ActivityPropertyValue> tempList =
                        Conn.GetItems<ActivityPropertyValue>(
                            @"SELECT ActivityPropertyValue.Value, ActivityPropertyValue.ActivityPropertyId FROM ActivityPropertyValue
                              INNER JOIN UserActivity ON UserActivity.Id = ActivityPropertyValue.UserActivityId
                              INNER JOIN ActivityProperty ON ActivityProperty.Id = ActivityPropertyValue.ActivityPropertyId
                              INNER JOIN ActivityProperties ON ActivityProperties.PropertyId = ActivityProperty.Id
                              WHERE ActivityProperties.ActivityId = ? AND UserActivity.Id = ?", Id, UserActivityId);
                    foreach (ActivityProperty obj in Properties)
                    {
                        if (obj.Type < 2 || obj.Type == 0)
                        {
                            tempDic.Add(obj.Property, tempList.FirstOrDefault(data => data.ActivityPropertyId == obj.Id).Value);
                            continue;
                        }
                        int staticValue = Int32.Parse(tempList.FirstOrDefault(data => data.ActivityPropertyId == obj.Id).Value);
                        tempDic.Add(obj.Property, obj.StaticPropertyValues.FirstOrDefault(x => x.Id == staticValue).Name);
                    }
                        
                    _propertyValueContainer = tempDic;
                }
                return _propertyValueContainer;
            }
            set { _propertyValueContainer = value; }
        }

        #endregion
    }
}
