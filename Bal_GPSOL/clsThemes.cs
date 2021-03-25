using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsThemes:IDisposable
    {


        string  f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        #region
        private int _ipkThemeID;
        private string _vThemeName;
        public int ipkThemeID
        {
            get { return _ipkThemeID; }
            set { _ipkThemeID = value; }
        }
        public string vThemeName
        {
            get { return _vThemeName; }
            set { _vThemeName = value; }
        }
        #endregion
        public clsThemes()
        {
            // initialization constructore

        }

        public clsThemes(int ipkThemeID, string vThemeName)
        {
            this.ipkThemeID = ipkThemeID;
            this.vThemeName = vThemeName;

        }
        public List<clsThemes> GetThemes()
        {
            DataSet ds = new DataSet();
            List<clsThemes> lstThemes = new List<clsThemes>();
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@operation",SqlDbType.Int);
                param[0].Value =25;
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "NewFrontData_sp", param);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstThemes.Add(new clsThemes(Convert.ToInt32(row["ipkThemeID"].ToString()), row["vThemeName"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsThemes.cs", "GetThemes()", ex.Message  + ex.StackTrace);

            }

            return lstThemes;
        }

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}