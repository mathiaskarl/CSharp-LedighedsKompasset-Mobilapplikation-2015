using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedighedsApp.Model.DataModel;
using LedighedsApp.Model.DBMS;

namespace LedighedsApp.Model.Handler
{
    public class InformationHandler
    {
        private InfoType CurrentInfoType;
        private List<InfoType> _infoTypes = null;

        public List<InfoType> InfoTypes
        {
            get
            {
                if (_infoTypes == null)
                    _infoTypes = Conn.GetItems<InfoType>("SELECT * FROM InfoType");
                return _infoTypes;
            }
            set { _infoTypes = value; }
        }

        private List<Information> _informations = null;

        public List<Information> Informations
        {
            get
            {
                if (_informations == null)
                    _informations = Conn.GetItems<Information>("SELECT * FROM Information");
                return _informations;
            }
        }

        public List<Information> SpecificInformation(InfoType obj)
        {
            return Informations.Where(x => x.InfoTypeId == obj.Id).ToList();
        } 
    }

    
}
