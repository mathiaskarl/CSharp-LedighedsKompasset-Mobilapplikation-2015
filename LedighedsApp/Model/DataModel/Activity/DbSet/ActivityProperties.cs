using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel.Activity.DbSet
{
    public class ActivityProperties
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ActivityId { get; set; }
        public int PropertyId { get; set; }
    }
}
