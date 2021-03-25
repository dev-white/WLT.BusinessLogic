using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using WLT.BusinessLogic;
using WLT.BusinessLogic.BAL;
using Newtonsoft.Json;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.Bal_GPSOL
{
   
    public class ReportView
    {

        public string vpkDeviceID { get; set; }
        public string vDeviceName { get; set; }
        public string dAge { get; set; }
        public string vLongitude { get; set; }
        public string vLatitude { get; set; }
        public string iVehicleSpeed { get; set; }
        public string bIsIgnitionOn { get; set; }
        public string vAlertName { get; set; }

    }
}