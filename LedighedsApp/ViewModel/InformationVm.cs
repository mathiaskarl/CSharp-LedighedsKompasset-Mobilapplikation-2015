using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DataModel.Enum;
using LedighedsApp.Model.DBMS;
using LedighedsApp.Model.Handler;

namespace LedighedsApp.ViewModel
{
    public class InformationVm : Contents
    {
        
        private InfoType _selectedInfoType = null;
        public InfoType SelectedInfoType { get { return _selectedInfoType; } 
            set 
            { 
            _selectedInfoType = value;
            OnPropertyChanged("SpecificInformation");
            OnPropertyChanged("SelectedInfoType");
            SelectedCategoryCheck = (value != null);
            }   
        }

        private Information _selectedInformation = null;
        public Information SelectedInformation
        {
            get { return _selectedInformation; }
            set
            {
                _selectedInformation = value;
                OnPropertyChanged("SelectedInformation");
                SelectedInformationCheck = (value != null);
            }
        }

        private List<InfoType> _categories = null;
        public ObservableCollection<InfoType> Categories
        {
            get
            {
                if (_categories == null)
                    _categories = handler.InfoTypes;
                return new ObservableCollection<InfoType>(_categories.OrderBy(x => x.Id));
            }
        }

        public ObservableCollection<Information> SpecificInformation
        {
            get
            {
                if(SelectedInfoType != null)
                    return new ObservableCollection<Information>(handler.SpecificInformation(SelectedInfoType));
                return null;
            }
        }

        public InformationHandler handler = new InformationHandler();

        public InformationVm() { InitTutorialUc(PageName.InfoCategoryView); }

        #region ShowBools
        private bool _selectedCategoryCheck = false;
        private bool _selectedInformationCheck = false;
        public bool SelectedCategoryCheck { get { return _selectedCategoryCheck; } set
        {
            _selectedCategoryCheck = value;
            OnPropertyChanged("SelectedCategoryCheck");
        } }
        public bool SelectedInformationCheck
        {
            get { return _selectedInformationCheck; }
            set
            {
                _selectedInformationCheck = value;
                OnPropertyChanged("SelectedInformationCheck");
            }
        }
        #endregion
    }
}
