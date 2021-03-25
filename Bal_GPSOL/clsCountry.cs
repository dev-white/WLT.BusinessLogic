using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WLT.BusinessLogic.Bal_GPSOL;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsCountry:General
    {
        private int _Operation;
        private int _pkcountryID;
        private string _vcountry_name;
        private Boolean _bcountry_status;
        private int _error;
        private string _vIDs;

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int pkcountryID { get { return _pkcountryID; } set { _pkcountryID = value; } }
        public string vcountry_name { get { return _vcountry_name; } set { _vcountry_name = value; } }
        public Boolean bcountry_status { get { return _bcountry_status; } set { _bcountry_status = value; } }
        public int error { get { return _error; } set { _error = value; } }
        public string vIDs { get { return _vIDs; } set { _vIDs = value; } }

        public clsCountry()
        {
            //constructor
        }

        public clsCountry(int pkcountryID, string vcountry_name, bool bcountry_status)
        {
            this.pkcountryID = pkcountryID;
            this.vcountry_name = vcountry_name;
            this.bcountry_status = bcountry_status;
        }
        public clsCountry(string CountryName, bool CountryStatus)
        {
            vcountry_name = CountryName;
            bcountry_status = CountryStatus;
        }
        public clsCountry(string CountryName, int primaryKey)
        {
            vcountry_name = CountryName;
            pkcountryID = primaryKey;
        }

        public string savecountry()
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[6];
            try
            {

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@vcountry_name", SqlDbType.VarChar);
                param[1].Value = vcountry_name;

                param[2] = new SqlParameter("@pkcountryID", SqlDbType.Int);
                param[2].Value = pkcountryID;

                //param[3] = new SqlParameter("@fkcountryID", SqlDbType.Int);
                //param[3].Value = fkcountryID;

                param[3] = new SqlParameter("@bcountry_status", SqlDbType.Bit);
                param[3].Value = bcountry_status;

                param[4] = new SqlParameter("@error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[5].Value = vIDs;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_country", param);

                if (param[4].Value.ToString() == "1")
                {
                    returnstring = "Country Saved successful"; 
                }
                else if (param[4].Value.ToString() == "0")
                {
                    returnstring = "Country Already Exists!";
                }
                else if (param[4].Value.ToString() == "2")
                {
                    returnstring = "Status Changed successful";
                }
                else if (param[4].Value.ToString() == "3")
                {
                    returnstring = "Country Deleted Successfuly";
                }
                else if (param[4].Value.ToString() == "5")
                {
                    returnstring = "Country Updated successful";
                }
                else if (param[4].Value.ToString() == "4")
                {
                    returnstring = "Delete successful";
                }
                else if (param[4].Value.ToString() == "-1")
                {
                    returnstring = "Country Update not allow";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsCountry.cs", "savecountry()", ex.Message  + ex.StackTrace);
                returnstring = "Error in Saving state!" + ex.Message;
            }
            return returnstring;

        }

        public clsCountry Getcountry()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            clsCountry obj = new clsCountry();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@pkcountryId", SqlDbType.Int);
                param[1].Value = pkcountryID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_country", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj = new clsCountry(Convert.ToInt32(ds.Tables[0].Rows[0]["pkcountryID"].ToString()), ds.Tables[0].Rows[0]["vcountry_name"].ToString(), Convert.ToBoolean(ds.Tables[0].Rows[0]["bcountry_status"].ToString()));
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "ClsCountry.cs", "Getcountry", ex.Message  + ex.StackTrace);
            }
            return obj;
        }

        public List<clsCountry> GetAllCountry()
        {
            DataSet ds = new DataSet();
            List<clsCountry> obj = new List<clsCountry>();
            try
            {
                ds = SqlHelper.ExecuteDataset(f_strConnectionString ,CommandType.Text, "select * from tblCountry_Master");
                obj.Add(new clsCountry("Select", -1));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    obj.Add(new clsCountry(row["vcountry_name"].ToString(), Convert.ToInt32(row["pkcountryID"].ToString())));
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsCountry.cs", "GetAllCountry()", ex.Message  + ex.StackTrace);
            }
            //return new clsCountry(ds.Tables[0].Rows[0]["vCountryName"].ToString(), Convert.ToBoolean(ds.Tables[0].Rows[0]["bCountryStatus"].ToString()));
            return obj;
        }
    }

}