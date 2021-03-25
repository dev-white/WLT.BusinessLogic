using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsLocation
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        private int _Operation;
        private int _ipkLocationId;
        private string _vLocationName;
        private Boolean _bStatus;
        private int _error;
        private string _vIDs;


        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkLocationId { get { return _ipkLocationId; } set { _ipkLocationId = value; } }
        public string vLocationName { get { return _vLocationName; } set { _vLocationName = value; } }
        public Boolean bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public int error { get { return _error; } set { _error = value; } }
        public string vIDs { get { return _vIDs; } set { _vIDs = value; } }

        public clsLocation()
        {
            // constructore
        }
        public clsLocation(int ipkLocationId, string vLocationName, bool bStatus)
        {
            this.ipkLocationId = ipkLocationId;
            this.vLocationName = vLocationName;
            this.bStatus = bStatus;
        }

        public string SaveLocation()
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[6];
            try
            {

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@vLocationName", SqlDbType.VarChar);
                param[1].Value = vLocationName;

                param[2] = new SqlParameter("@ipkLocationId", SqlDbType.Int);
                param[2].Value = ipkLocationId;


                param[3] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[3].Value = bStatus;

                param[4] = new SqlParameter("@error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[5].Value = vIDs;

               SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_Location", param);

                if (param[4].Value.ToString() == "1")
                {
                    returnstring = "Location Saved successful";
                }
                else if (param[4].Value.ToString() == "0")
                {
                    returnstring = "Location Already Exists!";
                }
                else if (param[4].Value.ToString() == "2")
                {
                    returnstring = "Status Changed successful";
                }
                else if (param[4].Value.ToString() == "3")
                {
                    returnstring = "Location Deleted Successfuly";
                }
                else if (param[4].Value.ToString() == "5")
                {
                    returnstring = "Location Updated successful";
                }
                else if (param[4].Value.ToString() == "4")
                {
                    returnstring = "Delete successful";
                }
                else if (param[4].Value.ToString() == "-1")
                {
                    returnstring = "Location Update not allow";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsLocation.cs", "SaveLocation()", ex.Message  + ex.StackTrace);
                returnstring = "Error in Saving state!" + ex.Message;
            }
            return returnstring;

        }

        public clsLocation GetLocation()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            clsLocation obj = new clsLocation();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkLocationId", SqlDbType.Int);
                param[1].Value = ipkLocationId;

                ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_Location", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj = new clsLocation(Convert.ToInt32(ds.Tables[0].Rows[0]["ipkLocationId"].ToString()), ds.Tables[0].Rows[0]["vLocationName"].ToString(), Convert.ToBoolean(ds.Tables[0].Rows[0]["bStatus"].ToString()));
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "ClsCountry.cs", "Getcountry", ex.Message  + ex.StackTrace);
            }
            return obj;
        }
    }
}