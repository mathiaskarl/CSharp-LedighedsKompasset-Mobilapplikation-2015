using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedighedsApp.Model.DataModel.Translation
{
    public class TranslationActivityPropertyTypeValue
    {
        public int TranslationActivityPropertyTypeValueId { get; set; }
        public int LanguageId { get; set; }
        public string Value { get; set; }
    }
}
