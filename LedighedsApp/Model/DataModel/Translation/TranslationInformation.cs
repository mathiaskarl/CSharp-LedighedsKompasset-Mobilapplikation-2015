using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel.Translation
{
    public class TranslationInformation
    {
        public int InfoId { get; set; }
        public int LanguageId { get; set; }
        public string Text { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }

    }
}
