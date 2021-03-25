using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class MyInputControls : MySelectClass
    {
        protected string _class;
        protected string _size;
        protected string _style;
        private string _buttonclass;

        public string InputClass
        {
            get { return _class; }
            set { _class = value; }
        }
        public string Inputsize
        {
            get { return _size; }
            set { _size = value; }
        }
        public string Inputstyle
        {
            get { return _style; }
            set { _style = value; }
        }
        public string buttonclass
        {
            get { return _buttonclass; }
            set { _buttonclass = value; }
        }

        public MyInputControls()
        {
            //
            // TODO: Add constructor logic here
            //
            _class = "";
            _size = "";
            _style = "";
            _buttonclass = "";
        }
    }
}