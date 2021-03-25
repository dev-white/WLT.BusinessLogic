using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class MySelectClass
    {
        private string _selectclass;
        private string _selectstyle;

        public string selectclass
        {
            get { return _selectclass; }
            set { _selectclass = value; }
        }
        public string selectstyle
        {
            get { return _selectstyle; }
            set { _selectstyle = value; }
        }
        public MySelectClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}