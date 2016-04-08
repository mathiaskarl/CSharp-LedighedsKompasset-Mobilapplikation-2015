using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class NavigationStep
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [Ignore]
        public PageName NavigatedFrom { get; set; }
        [Ignore]
        public PageName NavigatedTo { get; set; }
        
        [MaxLength(2)]
        public int NavigatedFromId { get; set; }
        
        [MaxLength(2)]
        public int NavigatedToId { get; set; }

        public NavigationStep(PageName from, PageName to)
        {
            NavigatedFrom = from;
            NavigatedTo = to;
            NavigatedFromId = (int)from;
            NavigatedToId = (int)to;
        }

        public NavigationStep(int fromId, int toId)
        {
            NavigatedFrom = (PageName)fromId;
            NavigatedTo = (PageName)toId;
            NavigatedFromId = fromId;
            NavigatedToId = toId;
        }

        public NavigationStep() { }
    }
}
