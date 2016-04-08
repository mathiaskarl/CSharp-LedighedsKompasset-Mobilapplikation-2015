using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using LedighedsApp.Common;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DBMS;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.UserControls.BottomBar;

namespace LedighedsApp.ViewModel
{
    class SettingsVm : Contents
    {
        private Settings _settings;
        private string _name;
        private string _email;
        private Language _selectedLanguage;
        private bool _languageChanged = false;

        private ObservableCollection<KeyValueItem> _days;
        public ObservableCollection<KeyValueItem> Days { get { return _days; } set { _days = value; OnPropertyChanged("Days"); } }
        private KeyValueItem _selectedDay = null;
        public KeyValueItem SelectedDay
        {
            get { return _selectedDay; }
            set
            {
                _selectedDay = value;
                _settings.WaterResetDay = value.Key;
                OnPropertyChanged("SelectedDay");
            }
        }

        private ObservableCollection<int> _amount;
        public ObservableCollection<int> Amount { get { return _amount; } set { _amount = value; OnPropertyChanged("Amount"); } }

        private int _selectedAmount = 0;
        public int SelectedAmount
        {
            get { return _selectedAmount; }
            set
            {
                _selectedAmount = value;
                _settings.ActivityAmount = value;
                OnPropertyChanged("SelectedAmount");
            }
        }

        public bool TutorialsEnabled {  get { return _settings.Tutorial; } set { _settings.Tutorial = value; OnPropertyChanged("TutorialsEnabled"); }}

        public bool AnimationEnabled { get { return _settings.Animation; } set { _settings.Animation = value; OnPropertyChanged("AnimationEnabled"); } }

        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); }}

        public string Email { get { return _email; } set { _email = value; OnPropertyChanged("Email"); }}

        public ObservableCollection<Language> Languages { get; set; }

        public Language SelectedLanguage { get { return _selectedLanguage; } set { _selectedLanguage = value; _languageChanged = true; OnPropertyChanged("SelectedLanguage"); } }


        public SettingsVm()
        {
            _settings = new Settings(User.Instance.Settings);
            Name = User.Instance.Name;
            Email = User.Instance.Email;

            InitAmount();
            InitDays();
            InitLanguages();
            InitTutorialUc(PageName.SettingsView);
            BottomBarSaveItem.ButtonSaveEvent += new EventHandler(SaveSettings);
        }

        private void InitDays()
        {
            ObservableCollection<KeyValueItem> tempDays = new ObservableCollection<KeyValueItem>();
            for (int i = 0; i < 7; i++)
                tempDays.Add(new KeyValueItem(i, App.Content["DAY_" + (i + 1)].ToString()));
            Days = tempDays;
            SelectedDay = Days.FirstOrDefault(x => x.Key == _settings.WaterResetDay);
        }

        private void InitAmount()
        {
            ObservableCollection<int> tempAmount = new ObservableCollection<int>();
            for (int i = 1; i < 11; i++)
                tempAmount.Add(i);
            Amount = tempAmount;
            SelectedAmount = Amount.FirstOrDefault(x => x == _settings.ActivityAmount);
        }

        private void InitLanguages()
        {
            Languages = new ObservableCollection<Language>(Conn.GetItems<Language>("SELECT * FROM Language"));
            SelectedLanguage = Languages.FirstOrDefault(x => x.Id == _settings.LanguageId);
        }

        private void SaveSettings(object sender, EventArgs e)
        {

            if(User.Instance.Name != _name || User.Instance.Email != _email)
                if (!User.Instance.UpdateInformation(_name, _email))
                    return;

            _settings.LanguageId = SelectedLanguage.Id;
            User.Instance.UpdateSettings(_settings);


            if (_languageChanged)
            {
                App.Content = ContentHandler.UpdateContent();
                AchievementHandler.ResetAchievements();
                OnPropertyChanged("Content");
            }
            
            _languageChanged = false;
            ErrorHandler.ShowError(Content["TEXT_SETTINGS_UPDATED"].ToString());
        }
    }
}
