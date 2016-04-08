using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LedighedsApp.Common;
using LedighedsApp.Model.DataModel.Activity.Models;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DataModel.Interface;
using LedighedsApp.Model.Handler;
using LedighedsApp.View.UserControls;

namespace LedighedsApp.ViewModel
{
    public class CalendarVm : Contents
    {
        private ICalendarItem _selectedCalendarItemType = null;

        public DateTime? SelectedDate { get; set; }
        public string SelectedDateFormat { get { return (SelectedDate != null ? ((DateTime)SelectedDate).Day.ToString() + "/" + ((DateTime)SelectedDate).Month.ToString() + "/" + ((DateTime)SelectedDate).Year.ToString() : SelectedDate.ToString()); } }
        public ICalendarItem SelectedCalenderItem { get; set; }
        public ObservableCollection<ICalendarItem> SelectedDateCalendarItems { get; set; }
        public ObservableCollection<ICalendarItem> CalendarItemTypes { get { return handler.CalendarItemTypes; } }
        public CalendarHandler handler = new CalendarHandler();

        public ICalendarItem SelectedCalendarType
        {
            get
            {
                return _selectedCalendarItemType;
            }
            set
            {
                _selectedCalendarItemType = value;
                OnPropertyChanged("SelectedCalendarType");
            }
        }

        

        public ICalendarItem _staticItemType;

        public CalendarVm() 
        { 
            _staticItemType = new ActivityContainer() { Title = Content["CALENDAR_ALL"].ToString() };
            _selectedCalendarItemType = _staticItemType;
            InitTutorialUc(PageName.CalendarView);
        }

    }
}
