using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Common;
using LedighedsApp.Model.DataModel.Translation;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class Content
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Query { get; set; }
    }
}
