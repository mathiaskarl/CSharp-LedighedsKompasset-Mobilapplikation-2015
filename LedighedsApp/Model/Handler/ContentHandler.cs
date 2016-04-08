using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Common;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Translation;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.Handler
{
    public static class ContentHandler
    {
        public static ObservableDictionary ContentDictionary
        {
            get
            {
                if (_contentDictionary == null)
                    _contentDictionary = GenerateContent();
                return _contentDictionary;
            }
        }

        private static ObservableDictionary _contentDictionary = null;
        private static List<Content> _contentList;
        private static List<TranslationContent> _translationContentList;

        private static ObservableDictionary GenerateContent()
        {
            _contentList = Conn.GetItems<Content>("SELECT * FROM Content");
             string sql =
                        @"SELECT * FROM TranslationContent
                               INNER JOIN Settings ON Settings.LanguageId = TranslationContent.LanguageId";
            _translationContentList = Conn.GetItems<TranslationContent>(sql);
            ObservableDictionary tempDictionary = new ObservableDictionary();
            foreach (Content obj in _contentList)
                tempDictionary.Add(obj.Query, _translationContentList.FirstOrDefault(data => data.ContentId == obj.Id).Text);
            return tempDictionary;
        }

        public static ObservableDictionary UpdateContent()
        {
            return GenerateContent();
        }

    }
}
