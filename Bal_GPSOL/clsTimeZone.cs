using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsTimeZone
    {
        private int _ipkTimeZoneID;
        private string _vTimeZoneName;
        private bool _bStatus;
        private string _vTimeZoneCode;

        public int ipkTimeZoneID { get { return _ipkTimeZoneID; } set { _ipkTimeZoneID = value; } }
        public string vTimeZoneName { get { return _vTimeZoneName; } set { _vTimeZoneName = value; } }
        public string vTimeZoneCode { get { return _vTimeZoneCode; } set { _vTimeZoneCode = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }

        public clsTimeZone()
        {
            // constructor
        }
         

        public clsTimeZone(string vTimeZoneName, int ipkTimeZoneID)
        {
            this.vTimeZoneName = vTimeZoneName;
            this.ipkTimeZoneID = ipkTimeZoneID;
        }

        public clsTimeZone(string vTimeZoneName, string vTimeZoneCode)
        {
            this.vTimeZoneName = vTimeZoneName;
            this.vTimeZoneCode = vTimeZoneCode;
        }

        public clsTimeZone(string vTimeZoneName, string vTimeZoneCode, int ipkTimeZoneID)
        {
            this.vTimeZoneName = vTimeZoneName;
            this.ipkTimeZoneID = ipkTimeZoneID;
            this.vTimeZoneCode = vTimeZoneCode;
        }
    }
}