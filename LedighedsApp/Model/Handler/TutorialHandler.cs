using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.Handler
{
    public class TutorialHandler
    {
        public Tutorial CurrentTutorial { get; set; }

        private static List<Tutorial> _tutorialContainer = null;

        public static List<Tutorial> Tutorials
        {
            get
            {
                if (_tutorialContainer == null)
                    _tutorialContainer = PopulateTutorialList();
                return _tutorialContainer;
            }
        }

        public TutorialHandler()
        {
            PopulateTutorialList();
        }

        public static List<Tutorial> GetTutorials(List<Tutorial> list, PageName page)
        {
            return list.Where(t => page == t.PageName).ToList();
        }

        public bool GetTutorial(PageName page)
        {
            bool returnCheck = false;
            foreach (Tutorial t in Tutorials.Where(t => t.PageName == page))
            {
                if (!t.HasBeenSeen && !returnCheck)
                {
                    CurrentTutorial = t;
                    returnCheck = true;
                    break;
                }
            }
            return returnCheck;
        }

        public bool UpdateTutorial(Tutorial obj = null)
        {
            obj = (obj ?? CurrentTutorial);
            obj.HasBeenSeen = true;
            Conn.Update(obj);
            return GetTutorial(obj.PageName);
        }

        private static List<Tutorial> PopulateTutorialList()
        {
            return Conn.GetItems<Tutorial>("SELECT * FROM Tutorial");
        }
    }
}
