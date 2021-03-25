using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsUserDevice
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString().ToString();
        private int _Operation;
        private int _ipkDeviceDID;
        private int _ifkUserID;
        private string _vpkDeviceID = "";
        private int _ipkDeviceID;
        private bool _bStatus;
        private List<clsUserDevice> _DeviceID;
        private int _ifkDeviceID;
        private int _ifkGroupMID;
      

     
        public int ipkDeviceDID { get { return _ipkDeviceDID; } set { _ipkDeviceDID = value; } }
        public int ifkUserID { get { return _ifkUserID; } set { _ifkUserID = value; } }
        public string vpkDeviceID { get { return _vpkDeviceID; } set { _vpkDeviceID = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public List<clsUserDevice> DeviceID { get { return _DeviceID; } set { _DeviceID = value; } }
        public int ipkDeviceID { get { return _ipkDeviceID; } set { _ipkDeviceID = value; } }
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ifkDeviceID { get { return _ifkDeviceID; } set { _ifkDeviceID = value; } }
        public int ifkGroupMID { get { return _ifkGroupMID; } set { _ifkGroupMID = value; } }

        public clsUserDevice()
        {
            // constructore
        }

        public clsUserDevice(int ifkDeviceID, string vpkDeviceID, int ifkUserID)
        {
            this.ifkDeviceID = ifkDeviceID;
            this.vpkDeviceID = vpkDeviceID;
            this.ifkUserID = ifkUserID;
        }

        public clsUserDevice(int ifkGroupMID, int ifkDeviceID, string vpkDeviceID)
        {
            this.ifkGroupMID = ifkGroupMID;
            this.ifkDeviceID = ifkDeviceID;
            this.vpkDeviceID = vpkDeviceID;
        }

        public string SaveUserDevice()
        {
            SqlParameter[] param = new SqlParameter[5];
            string returnData;
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[1].Value = ifkUserID;

                param[2] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[2].Value = ipkDeviceID;

                param[3] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[3].Value = vpkDeviceID;

                param[4] = new SqlParameter("@error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_UserDevice", param);
                returnData = param[4].Value.ToString();
                
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsUserDevice.cs", "SaveUserDevice()", ex.Message  + ex.StackTrace);
                returnData = "Error in Device Master=" + ex.GetBaseException().ToString();
            }
            return returnData;
        }

        public List<clsUserDevice> FillUserDevice()
        {
            List<clsUserDevice> lstMaster = new List<clsUserDevice>();
            clsUserDevice objMaster = new clsUserDevice();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;
                param[1] = new SqlParameter("@ifkuserID", SqlDbType.Int);
                param[1].Value = ifkUserID;
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_UserDevice", param);
                // lstMaster.Add(new clsDeviceMaster(-1, "Select"));
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        lstMaster.Add(new clsUserDevice(Convert.ToInt32(drRow["ifkDeviceID"].ToString()), drRow["vpkDeviceID"].ToString(), Convert.ToInt32(drRow["ifkUserID"].ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                ////HttpContext.Current.Response.Write(ex.Message  + ex.StackTrace);
            }
            return lstMaster;

        }


        public List<clsUserDevice> FillFrontGroupAssignDevice()
        {
            List<clsUserDevice> lstMaster = new List<clsUserDevice>();
            clsUserDevice objMaster = new clsUserDevice();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;
                param[1] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;
             
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_FrontGroupDevice", param);
                // lstMaster.Add(new clsDeviceMaster(-1, "Select"));
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        lstMaster.Add(new clsUserDevice(Convert.ToInt32(drRow["ifkGroupMID"].ToString()), Convert.ToInt32(drRow["ifkDeviceID"].ToString()), drRow["vpkDeviceID"].ToString()));
                       
                    }
                }
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Response.Write(ex.Message  + ex.StackTrace);
            }
            return lstMaster;
        }


        public string SaveFrontGroupDevice()
        {
            SqlParameter[] param = new SqlParameter[6];
            string returnData;
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[1].Value = ifkUserID;

                param[2] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[2].Value = ipkDeviceID;

                param[3] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[3].Value = vpkDeviceID;

                param[4] = new SqlParameter("@error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[5].Value = ifkGroupMID;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_FrontGroupDevice", param);
                returnData = param[4].Value.ToString();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsUserDevice.cs", "SaveFrontGroupDevice()", ex.Message  + ex.StackTrace);
                returnData = "Error in Device Master=" + ex.GetBaseException().ToString();
            }
            return returnData;
        }

    }
}