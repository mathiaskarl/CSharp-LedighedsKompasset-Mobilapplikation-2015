using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedighedsApp.Model.DataModel.Activity.Models
{
    public class JobApplication : Activity
    {
        public JobApplication() { }
        public JobApplication(int id, int rowId) : base(id, rowId) { }

        public string Text { get { return PropertyValues["Text"]; } set { PropertyValues["Text"] = value; } }
    }
}
