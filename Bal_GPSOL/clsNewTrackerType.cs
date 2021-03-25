using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsNewTrackerType:IDisposable
    {
        private int _Operation;
        private int _ipkTrackerTypeID;
        private string _vTrackerTypeName;
        private int _Error;
        private bool _bStatus;
        private string _vColor;
        private string _vMake;
        private string _vModel;
        private int _iParent;
        private int _iCreatedBy;
      
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkTrackerTypeID { get { return _ipkTrackerTypeID; } set { _ipkTrackerTypeID = value; } }
        public int Error { get { return _Error; } set { _Error = value; } }
        public string vTrackerTypeName { get { return _vTrackerTypeName; } set { _vTrackerTypeName = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public string vColor { get { return _vColor; } set { _vColor = value; } }
        public string vMake { get { return _vMake; } set { _vMake = value; } }
        public string vModel { get { return _vModel; } set { _vModel = value; } }
        public int iParent { get { return _iParent; } set { _iParent = value; } }
        public int iCreatedBy { get { return _iCreatedBy; } set { _iCreatedBy = value; } }

        public clsNewTrackerType()
        {
            // initialization constructore
        }

        public clsNewTrackerType(string vTrackerTypeName, int ipkTrackerTypeID)
        {
            this.vTrackerTypeName = vTrackerTypeName;
            this.ipkTrackerTypeID = ipkTrackerTypeID;
        }

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}