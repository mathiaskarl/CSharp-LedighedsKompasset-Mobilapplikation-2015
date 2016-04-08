using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedighedsApp.Model.DataModel.Translation
{
    public class TranslationActivity
    {
        public int ActivityId { get; set; }
        public int LanguageId { get; set; }
        public string Title { get; set; }
    }
}
