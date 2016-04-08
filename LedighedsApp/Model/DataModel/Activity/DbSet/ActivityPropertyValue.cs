using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel.Activity.DbSet
{
    public class ActivityPropertyValue
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ActivityPropertyId { get; set; }
        public int UserActivityId { get; set; }
        public string Value { get; set; }
    }
}
