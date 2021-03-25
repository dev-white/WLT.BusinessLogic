using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsLanguage:IDisposable
    {
        private int _ipkLanguageID;
        private string _vLanguageName;
        private bool _bStatus;

        public int ipkLanguageID { get { return _ipkLanguageID; } set { _ipkLanguageID=value; } }
        public string vLanguageName { get { return _vLanguageName; } set { _vLanguageName = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }

        public clsLanguage()
        {
            // constructor
        }


        public clsLanguage(string vLanguageName, int ipkLanguageID)
        {
            this.vLanguageName = vLanguageName;
            this.ipkLanguageID = ipkLanguageID;
        }

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}