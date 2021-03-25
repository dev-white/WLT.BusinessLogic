using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class MyTableClass : MyInputControls
    {
        protected string _tablealign;
        protected string _tablebackground;
        protected string _tablebgcolor;
        protected string _tableborder;
        protected string _tablebordercolor;
        protected string _tablebordercolordark;
        protected string _tablebordercolorlight;
        private string _tablecellspacing;
        private string _tablecellpadding;
        private string _tableclass;
        private string _tableheight;
        private string _tablewidth;
        private string _tablestyle;

        private string _tralign;
        private string _trclass;
        private string _trheight;
        private string _trvalign;
        private string _trheaderValign;
        private string _trwidth;
        private string _trbgcolor = "#fff";
        private string _tralterbgcolor;
        private string _trhighlightcolor;
        private string _trheaderBackgroundImg;
        private string _trheadercolor;
        private string _trfootercolor;
        private string _trfooterTextColor;
        private string _trheadingclass;
        private string _trheadingforeColor;
        private string _trforecolor;
        private string _trheaderStyle;
        private string _trfooterStyle;
        private string _trStyle;
        private string _trheaderhighlightcolor;

        private string _tdalign;
        private string _tdclass;
        private bool _tdnowrap;
        private string _tdrowspan;
        private string _tdcolspan;
        private string _tdvalign;
        private string _tdheight;
        private string _tdwidth;
        private string _tdstyle;
        private string _tdgridcontentclass;
        private string _tdgridcontentstyle;
        private string _tdgridcontentwidth;

        public string tablealign
        {
            get { return _tablealign; }
            set { _tablealign = value; }
        }
        public string tablebackground
        {
            get { return _tablebackground; }
            set { _tablebackground = value; }
        }
        public string tablebgcolor
        {
            get { return _tablebgcolor; }
            set { _tablebgcolor = value; }
        }
        public string tableborder
        {
            get { return _tableborder; }
            set { _tableborder = value; }
        }
        public string tablebordercolor
        {
            get { return _tablebordercolor; }
            set { _tablebordercolor = value; }
        }
        public string tablebordercolordark
        {
            get { return _tablebordercolordark; }
            set { _tablebordercolordark = value; }
        }
        public string tablebordercolorlight
        {
            get { return _tablebordercolorlight; }
            set { _tablebordercolorlight = value; }
        }
        public string tablecellspacing
        {
            get { return _tablecellspacing; }
            set { _tablecellspacing = value; }
        }
        public string tablecellpadding
        {
            get { return _tablecellpadding; }
            set { _tablecellpadding = value; }
        }
        public string tableclass
        {
            get { return _tableclass; }
            set { _tableclass = value; }
        }
        public string tableheight
        {
            get { return _tableheight; }
            set { _tableheight = value; }
        }
        public string tablewidth
        {
            get { return _tablewidth; }
            set { _tablewidth = value; }
        }
        public string tablestyle
        {
            get { return _tablestyle; }
            set { _tablestyle = value; }
        }
        public string tralign
        {
            get { return _tralign; }
            set { _tralign = value; }
        }
        public string trclass
        {
            get { return _trclass; }
            set { _trclass = value; }
        }
        public string trheight
        {
            get { return _trheight; }
            set { _trheight = value; }
        }
        public string trwidth
        {
            get { return _trwidth; }
            set { _trwidth = value; }
        }
        public string trvalign
        {
            get { return _trvalign; }
            set { _trvalign = value; }
        }
        public string trbgcolor
        {
            get { return _trbgcolor; }
            set { _trbgcolor = value; }
        }
        public string tralterbgcolor
        {
            get { return _tralterbgcolor; }
            set { _tralterbgcolor = value; }
        }
        public string trhighlightcolor
        {
            get { return _trhighlightcolor; }
            set { _trhighlightcolor = value; }
        }
        public string trheadercolor
        {
            get { return _trheadercolor; }
            set { _trheadercolor = value; }
        }
        public string trheaderBackgroundImg
        {
            get { return _trheaderBackgroundImg; }
            set { _trheaderBackgroundImg = value; }
        }

        public string trfootercolor
        {
            get { return _trfootercolor; }
            set { _trfootercolor = value; }
        }
        public string trFooterTextColor
        {
            get { return _trfooterTextColor; }
            set { _trfooterTextColor = value; }
        }
        public string trHeadingClass
        {
            get { return _trheadingclass; }
            set { _trheadingclass = value; }
        }
        public string trHeadingTextColor
        {
            get { return _trheadingforeColor; }
            set { _trheadingforeColor = value; }
        }
        public string trTextColor
        {
            get { return _trforecolor; }
            set { _trforecolor = value; }
        }
        public string trHeaderStyle
        {
            get { return _trheaderStyle; }
            set { _trheaderStyle = value; }
        }
        public string trheaderhighlightcolor
        {
            get { return _trheaderhighlightcolor; }
            set { _trheaderhighlightcolor = value; }
        }
        public string trFooterStyle
        {
            get { return _trfooterStyle; }
            set { _trfooterStyle = value; }
        }
        public string trStyle
        {
            get { return _trStyle; }
            set { _trStyle = value; }
        }

        public string tdalign
        {
            get { return _tdalign; }
            set { _tdalign = value; }
        }
        public string tdclass
        {
            get { return _tdclass; }
            set { _tdclass = value; }
        }
        public bool tdnowrape
        {
            get { return _tdnowrap; }
            set { _tdnowrap = value; }
        }
        public string tdrowspan
        {
            get { return _tdrowspan; }
            set { _tdrowspan = value; }
        }
        public string tdcolspan
        {
            get { return _tdcolspan; }
            set { _tdcolspan = value; }
        }
        public string tdvalign
        {
            get { return _tdvalign; }
            set { _tdvalign = value; }
        }
        public string tdheight
        {
            get { return _tdheight; }
            set { _tdheight = value; }
        }
        public string tdwidth
        {
            get { return _tdwidth; }
            set { _tdwidth = value; }
        }
        public string tdstyle
        {
            get { return _tdstyle; }
            set { _tdstyle = value; }
        }
        public string tdgridContentClass
        {
            get { return _tdgridcontentclass; }
            set { _tdgridcontentclass = value; }
        }
        public string tdgridContentStyle
        {
            get { return _tdgridcontentstyle; }
            set { _tdgridcontentstyle = value; }
        }
        public string tdgridContentWidth
        {
            get { return _tdgridcontentwidth; }
            set { _tdgridcontentwidth = value; }
        }
        public string trHeaderValign
        {
            get { return _trheaderValign; }
            set { _trheaderValign = value; }
        }
        public MyTableClass()
        {

            //
            // TODO: Add constructor logic here
            //

            _tablealign = "center";
            _tableborder = "0";
            _tablecellpadding = "0";
            _tablecellspacing = "0";
            _tablestyle = "";
            _tableclass = "";
            _tdalign = "center";
            _tdclass = "";
            _tdcolspan = "1";
            _tdrowspan = "1";
            _tdstyle = "";
            _tdvalign = "middle";
            _tralign = "center";
            _trclass = "";
            _trvalign = "middle";
            _trheaderValign = "middle";
        }

    }
}