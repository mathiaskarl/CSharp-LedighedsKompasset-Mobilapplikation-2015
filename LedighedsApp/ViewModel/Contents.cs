using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LedighedsApp.Common;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.UserControls;

namespace LedighedsApp.ViewModel
{
    public class Contents : INotifyPropertyChanged
    {
        public ObservableDictionary Content
        {
            get { return App.Content; }
        }
        public List<Tutorial> Tutorials
        {
            get { return App.Tutorials; }
        }


        protected TutorialUc _tutorial;
        public TutorialUc Tutorial { get { return _tutorial; } set { _tutorial = value; OnPropertyChanged("Tutorial"); } }


        public Contents()
        {
            OnPropertyChanged("Content");
            MascotUc.MascotTapped += MascotUc_MascotTapped;
        }

        private void MascotUc_MascotTapped(object sender, System.EventArgs e)
        {
            if(Tutorial != null)
                Tutorial.Trigger();
        }

        protected void InitTutorialUc(PageName obj)
        {
            Tutorial = new TutorialUc(TutorialHandler.GetTutorials(Tutorials, obj));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
