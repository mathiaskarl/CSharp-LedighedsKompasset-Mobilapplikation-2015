using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.DataModel
{
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int UserId { get; set; }
        public bool Tutorial { get; set; }
        public bool Sound { get; set; }
        public bool Animation { get; set; }
        public int WaterResetDay { get; set; }
        public int ActivityAmount { get; set; }

        [Ignore]
        public bool IsInitialBoot { get; set; }
        [Ignore]
        public int PreviousWaterHeight { get; set; }

        public Settings()
        {
            IsInitialBoot = true;
            PreviousWaterHeight = 680;
        }

        public Settings(Settings obj)
        {
            Id = obj.Id;
            LanguageId = obj.LanguageId;
            UserId = obj.UserId;
            Tutorial = obj.Tutorial;
            Sound = obj.Sound;
            Animation = obj.Animation;
            WaterResetDay = obj.WaterResetDay;
            ActivityAmount = obj.ActivityAmount;
        }
    }
}
