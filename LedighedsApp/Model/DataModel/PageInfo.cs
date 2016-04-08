using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    class PageInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public PageName PageName { get; set; }
        public string Title { get; set; }

        public PageInfo() { }

        public PageInfo(PageName pageName)
        {
            PageName = pageName;
            Id = (int) pageName;
            Title = pageName.ToString();
        }

        public PageInfo(int id)
        {
            PageName = (PageName) id;
            Id = id;
            Title = PageName.ToString();
        }
    }
}
