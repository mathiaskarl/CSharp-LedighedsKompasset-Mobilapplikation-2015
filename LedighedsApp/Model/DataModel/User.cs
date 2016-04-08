using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LedighedsApp.Model.Assets;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class User
    {
        
        public DateTime CreateTime { get; set; }

        [PrimaryKey]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }
        
        [MaxLength(50)]
        public string Name { get; set; }
        
        [Ignore]
        public Stats Stats 
        {
            get
            {
                if (_stats == null) 
                    GetStats();
                return _stats;
            }
        }

        [Ignore]
        public Settings Settings
        {
            get
            {
                if (_settings == null) 
                    GetSettings();
                return _settings;
            }
        }

        private static User _instance = null;
        private static readonly object Padlock = new object();

        private Stats _stats;
        private Settings _settings;

        public User() { }

        //Singleton instans
        public static User Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = Conn.GetSingleItem<User>("SELECT * FROM User WHERE Id = ?", 1));
                }
            }
        }

        public bool UpdateInformation(string name, string email)
        {
            if (!CheckEmail(email) && !String.IsNullOrEmpty(email))
            {
                ErrorHandler.ShowError(ErrorHandler.ReturnErrorMessage("ERROR_INCORRECT_EMAIL"));
                return false;
            }
            Name = name;
            Email = email;

            Conn.Update(this);
            return true;
        }

        public void UpdateSettings(Settings settings)
        {
            Conn.Update(settings);
            GetSettings();
        }

        private void GetStats()
        {
            Stats s = Conn.GetSingleItem<Stats>("SELECT * FROM Stats WHERE UserId = ?", Id);
            if (s == null)
            {
                s = new Stats()
                {
                    ActiveUseage = 0,
                    PassiveUseage = 0,
                    NumberOfActivities = 0,
                    NumberOfNotifications = 0,
                    TutorialsCompleted = 0,
                    UserId = Id
                };
                Conn.Insert(s);
            }
            _stats = s;
        }

        private void GetSettings()
        {
            Settings s = Conn.GetSingleItem<Settings>("SELECT * FROM Settings WHERE UserId = ?", Id);
            if (s == null)
            {
                s = new Settings()
                {
                    UserId = Id,
                    LanguageId = 0,
                    Sound = false,
                    Tutorial = true,
                    Animation = true
                };
                Conn.Insert(s);
            }
            _settings = s;
        }

        private bool CheckEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
