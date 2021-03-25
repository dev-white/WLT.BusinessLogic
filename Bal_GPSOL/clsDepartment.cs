using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsDepartment
    {


        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        private int _Operation;
        private int _ipkDepartmentId;
        private string _vDepartmentName;
        private Boolean _bStatus;
        private int _error;
        private string _vIDs;


        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkDepartmentId { get { return _ipkDepartmentId; } set { _ipkDepartmentId = value; } }
        public string vDepartmentName { get { return _vDepartmentName; } set { _vDepartmentName = value; } }
        public Boolean bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public int error { get { return _error; } set { _error = value; } }
        public string vIDs { get { return _vIDs; } set { _vIDs = value; } }

        public clsDepartment()
        {
            //constructore
        }
        public clsDepartment(int ipkDepartmentId, string vDepartmentName, bool bStatus)
        {
            this.ipkDepartmentId = ipkDepartmentId;
            this.vDepartmentName = vDepartmentName;
            this.bStatus = bStatus;

        }
        public string SaveDepartment()
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[6];
            try
            {

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@vDepartmentName", SqlDbType.VarChar);
                param[1].Value = vDepartmentName;

                param[2] = new SqlParameter("@ipkDepartmentId", SqlDbType.Int);
                param[2].Value = ipkDepartmentId;
                              

                param[3] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[3].Value = bStatus;

                param[4] = new SqlParameter("@error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[5].Value = vIDs;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_Department", param);

                if (param[4].Value.ToString() == "1")
                {
                    returnstring = "Department Saved successful";
                }
                else if (param[4].Value.ToString() == "0")
                {
                    returnstring = "Department Already Exists!";
                }
                else if (param[4].Value.ToString() == "2")
                {
                    returnstring = "Status Changed successful";
                }
                else if (param[4].Value.ToString() == "3")
                {
                    returnstring = "Department Deleted Successfuly";
                }
                else if (param[4].Value.ToString() == "5")
                {
                    returnstring = "Department Updated successful";
                }
                else if (param[4].Value.ToString() == "4")
                {
                    returnstring = "Delete successful";
                }
                else if (param[4].Value.ToString() == "-1")
                {
                    returnstring = "Department Update not allow";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsDepartment.cs", "SaveDepartment()", ex.Message  + ex.StackTrace);
                returnstring = "Error in Saving state!" + ex.Message;
            }
            return returnstring;

        }


        public clsDepartment GetDepartment()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            clsDepartment obj = new clsDepartment();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDepartmentId", SqlDbType.Int);
                param[1].Value = ipkDepartmentId;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_Department", param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    obj = new clsDepartment(Convert.ToInt32(ds.Tables[0].Rows[0]["ipkDepartmentId"].ToString()), ds.Tables[0].Rows[0]["vDepartmentName"].ToString(), Convert.ToBoolean(ds.Tables[0].Rows[0]["bStatus"].ToString()));
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