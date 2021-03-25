using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    [Serializable()]
    public class General
    {
        Regex objSpecialCharacters = new Regex(@"[^\w\.@-]");

        public string RemoveSpecialCharacters(string input)
        {
            return objSpecialCharacters.Replace(input, "");
        }

        public General()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    //public static class RadGridReport
    //{
    //    public static int ReportID = 0;
    //    public static int ReportTypeID = 0;
    //    public static int CompanyID = 0;
    //}


}