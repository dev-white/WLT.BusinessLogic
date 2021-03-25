using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsMeasurement:IDisposable
    {
        private int _ipkMeasurementID;
        private string _vMeasurementName;
        private bool _bStatus;

        public int ipkMeasurementID { get { return _ipkMeasurementID; } set { _ipkMeasurementID=value; } }
        public string vMeasurementName { get { return _vMeasurementName; } set { _vMeasurementName = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }

        public clsMeasurement()
        {
            // constructor
        }


        public clsMeasurement(string vMeasurementName, int ipkMeasurementID)
        {
            this.vMeasurementName = vMeasurementName;
            this.ipkMeasurementID = ipkMeasurementID;
        }

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}