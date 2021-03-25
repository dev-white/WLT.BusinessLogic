using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsDeviceMaster
    {


        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        private int _ifkGroupMID;
        private int _Operation;
        private int _ipkDeviceID;
        private string _vBatchNo = "";
        private int _ifkUserID;
        private string _vManufactureID = "";
        private string _vpkDeviceID = "";
        private bool _bStatus;
        private int _Error;
        private string _vIDs = "";
        private string _vIDsUnAssign = "";

        private bool _EditOrAdd;
        private string _vDeviceName;
        private int _iTrackerType;
        private string _login;

        /*
            Device Details Starts here
        
        */
        private int _ipkDeviceDID;
        private int _ifkDeviceId;
        private DateTime _dConfigureData = DateTime.Now;
        private string _vMobileNo = "";
        private string _vIpLocation = "";
        private string _vPort = "";
        private bool _bIsGprsActive;
        private int _iTimeInterval;
        private bool _bIsActive;
        private int _iParentID;
        private bool _IsDriver;

        public bool IsDriver { get { return _IsDriver; } set { _IsDriver = value; } }
        public int iParentID { get { return _iParentID; } set { _iParentID = value; } }
        public int ifkGroupMID { get { return _ifkGroupMID; } set { _ifkGroupMID = value; } }
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkDeviceID { get { return _ipkDeviceID; } set { _ipkDeviceID = value; } }
        public string vBatchNo { get { return _vBatchNo; } set { _vBatchNo = value; } }
        public int ifkUserID { get { return _ifkUserID; } set { _ifkUserID = value; } }
        public string vManufactureID { get { return _vManufactureID; } set { _vManufactureID = value; } }
        public string vpkDeviceID { get { return _vpkDeviceID; } set { _vpkDeviceID = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public int Error { get { return _Error; } set { _Error = value; } }
        public string vIDs { get { return _vIDs; } set { _vIDs = value; } }
        public string vIDsUnAssign { get { return _vIDsUnAssign; } set { _vIDsUnAssign = value; } }
        public bool EditOrAdd { get { return _EditOrAdd; } set { _EditOrAdd = value; } }

        public int ipkDeviceDID { get { return _ipkDeviceDID; } set { _ipkDeviceDID = value; } }
        public int ifkDeviceId { get { return _ifkDeviceId; } set { _ifkDeviceId = value; } }
        public DateTime dConfigureData { get { return _dConfigureData; } set { _dConfigureData = value; } }
        public string vMobileNo { get { return _vMobileNo; } set { _vMobileNo = value; } }
        public string vIpLocation { get { return _vIpLocation; } set { _vIpLocation = value; } }
        public string vPort { get { return _vPort; } set { _vPort = value; } }
        public bool bIsGprsActive { get { return _bIsGprsActive; } set { _bIsGprsActive = value; } }
        public int iTimeInterval { get { return _iTimeInterval; } set { _iTimeInterval = value; } }
        public bool bIsActive { get { return _bIsActive; } set { _bIsActive = value; } }
        public string vDeviceName { get { return _vDeviceName; } set { _vDeviceName = value; } }
        public int iTrackerType { get { return _iTrackerType; } set { _iTrackerType = value; } }

        public string login { get { return _login; } set { _login = value; } }

        public int WorkMode { get; set; }

        public clsDeviceMaster()
        {
            //constructor
        }
        public clsDeviceMaster(int ipkDeviceID, string vBatchNo, int ifkUserID, string vManufactureID, string vpkDeviceID, bool bStatus, string vDeviceName, int iTrackerType)
        {
            this.ipkDeviceID = ipkDeviceID;
            this.vBatchNo = vBatchNo;
            this.ifkUserID = ifkUserID;
            this.vManufactureID = vManufactureID;
            this.vpkDeviceID = vpkDeviceID;
            this.bStatus = bStatus;
            this._vDeviceName = vDeviceName;
            this.iTrackerType = iTrackerType;
        }
        public clsDeviceMaster(int ipkDeviceID, string vpkDeviceID)
        {
            this.ipkDeviceID = ipkDeviceID;
            this.vpkDeviceID = vpkDeviceID;
        }
        public clsDeviceMaster(int ipkDeviceID, string vpkDeviceID, int ifkUserID)
        {
            this.ipkDeviceID = ipkDeviceID;
            this.vpkDeviceID = vpkDeviceID;
            this.ifkUserID = ifkUserID;
        }
        public clsDeviceMaster(int ifkGroupMID, int ipkDeviceID, string vpkDeviceID)
        {
            this.ifkUserID = ifkGroupMID;
            this.ipkDeviceID = ipkDeviceID;
            this.vpkDeviceID = vpkDeviceID;

        }
        public string SaveData()
        {
            SqlParameter[] param = new SqlParameter[22];
            string returnData;
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@vBatchNo", SqlDbType.VarChar);
                param[2].Value = vBatchNo;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter("@vManufactureID", SqlDbType.VarChar);
                param[4].Value = vManufactureID;

                param[5] = new SqlParameter("@error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[6].Value = vpkDeviceID;

                param[7] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[7].Value = bStatus;

                param[8] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[8].Value = vIDs;

                param[9] = new SqlParameter("@EditOrAdd", SqlDbType.Bit);
                param[9].Value = EditOrAdd;

                param[10] = new SqlParameter("@ipkDeviceDID", SqlDbType.Int);
                param[10].Value = ipkDeviceDID;

                param[11] = new SqlParameter("@dConfigureData", SqlDbType.DateTime);
                param[11].Value = dConfigureData;

                param[12] = new SqlParameter("@vMobileNo", SqlDbType.VarChar);
                param[12].Value = vMobileNo;

                param[13] = new SqlParameter("@vIpLocation", SqlDbType.VarChar);
                param[13].Value = vIpLocation;

                param[14] = new SqlParameter("@vPort", SqlDbType.VarChar);
                param[14].Value = vPort;

                param[15] = new SqlParameter("@bIsGprsActive", SqlDbType.Bit);
                param[15].Value = bIsGprsActive;

                param[16] = new SqlParameter("@iTimeInterval", SqlDbType.Int);
                param[16].Value = iTimeInterval;

                param[17] = new SqlParameter("@bIsActive", SqlDbType.Bit);
                param[17].Value = bIsActive;

                param[18] = new SqlParameter("@ifkDeviceId", SqlDbType.Int);
                param[18].Value = ifkDeviceId;

                param[19] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar, 300);
                param[19].Value = vDeviceName;

                param[20] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[20].Value = iTrackerType;

                param[21] = new SqlParameter("@vIDsUnAssign", SqlDbType.VarChar);
                param[21].Value = vIDsUnAssign;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_device", param);
                returnData = param[5].Value.ToString();

                if (param[5].Value.ToString() == "5")
                {
                    returnData = "Update  successful...!!";
                }
                else if (param[5].Value.ToString() == "1")
                {
                    returnData = "Save successful...!!";
                }
                else if (param[5].Value.ToString() == "2")
                {
                    returnData = "Update successful...!!";
                }
                else if (param[5].Value.ToString() == "-2")
                {
                    returnData = "Some Device Are not enable Because it needs to Configure first ...!!";
                    //Some Device are not Enable Because it needs to Configure first...!!
                }
                else if (param[5].Value.ToString() == "3")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "4")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "-1")
                {
                    returnData = "error## Device ID Already Exists...!!";
                }
                else if (param[5].Value.ToString() == "-8")
                {
                    //returnData = "Error Occur While Processing!";
                    returnData = "Error Occur While Processing !";
                }
                else if (param[5].Value.ToString() == "-11")
                {
                    returnData = "error##Device Name Already Exists...!!";
                }
                else if (param[5].Value.ToString() == "12")
                {
                    returnData = "Device Detail Save successful ...!!";
                }
                else if (param[5].Value.ToString() == "-12")
                {

                    returnData = "Mobile Number not available.Please assign another...!!";

                }
                else if (param[5].Value.ToString() == "13")
                {
                    returnData = " Device Congure successful!";

                }
                else if (param[5].Value.ToString() == "-13")
                {
                    returnData = "Device DisCongure successful!";

                }
                else if (param[5].Value.ToString() == "15")
                {
                    returnData = "Device name update successful!";

                }
                else if (param[5].Value.ToString() == "16")
                {
                    returnData = "Device status change successful!";

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsDeviceMaster.cs", "SaveData()", ex.Message + ex.StackTrace);
                returnData = "Error in Device Master=" + ex.GetBaseException().ToString();
            }
            return returnData;
        }

        public clsDeviceMaster(int ipkDeviceID)
        {
            this.ipkDeviceID = ipkDeviceID;
        }

        public clsDeviceMaster(string login)
        {
            this.login = login;
        }
        public clsDeviceMaster GetDevice()
        {
            DataSet ds = new DataSet();
            clsDeviceMaster obj = new clsDeviceMaster();
            string Query = "select * from tblDevice_Master where ipkDeviceId=" + ipkDeviceID;
            try
            {
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, Query);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    obj = new clsDeviceMaster(Convert.ToInt32(ds.Tables[0].Rows[0]["ipkDeviceID"].ToString()), ds.Tables[0].Rows[0]["vBatchNo"].ToString(), Convert.ToInt32(ds.Tables[0].Rows[0]["ifkUserID"].ToString()), ds.Tables[0].Rows[0]["vManufactureID"].ToString(), ds.Tables[0].Rows[0]["vpkDeviceID"].ToString(), Convert.ToBoolean(ds.Tables[0].Rows[0]["bStatus"].ToString()), ds.Tables[0].Rows[0]["vDeviceName"].ToString(), Convert.ToInt32(ds.Tables[0].Rows[0]["iTrackerType"].ToString()));
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsDeviceMaster.cs", "GetDevice()", ex.Message + ex.StackTrace);
            }
            return obj;
        }

        public List<clsDeviceMaster> FillDeviceMaster()
        {
            List<clsDeviceMaster> lstMaster = new List<clsDeviceMaster>();
            clsDeviceMaster objMaster = new clsDeviceMaster();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;
                param[1] = new SqlParameter("@ifkuserID", SqlDbType.Int);
                param[1].Value = ifkUserID;
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_Admindevice", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        lstMaster.Add(new clsDeviceMaster(Convert.ToInt32(drRow["ipkDeviceID"].ToString()), drRow["vpkDeviceID"].ToString(), Convert.ToInt32(drRow["ifkUserID"].ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                ////HttpContext.Current.Response.Write(ex.Message);
            }
            return lstMaster;
        }

        public List<clsDeviceMaster> FillDeviceMaster1(int reselerId)
        {
            List<clsDeviceMaster> lstMaster = new List<clsDeviceMaster>();
            clsDeviceMaster objMaster = new clsDeviceMaster();
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, "select ipkDeviceID,vpkDeviceID,ifkUserID from tblDevice_Master where (ifkUserID = " + reselerId + ") and bStatus=1");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        lstMaster.Add(new clsDeviceMaster(Convert.ToInt32(drRow["ipkDeviceID"].ToString()), drRow["vpkDeviceID"].ToString(), Convert.ToInt32(drRow["ifkUserID"].ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                ////HttpContext.Current.Response.Write(ex.Message);
            }
            return lstMaster;
        }

        public string SaveDataForAdmin()
        {
            SqlParameter[] param = new SqlParameter[22];
            string returnData;
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@vBatchNo", SqlDbType.VarChar);
                param[2].Value = vBatchNo;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter("@vManufactureID", SqlDbType.VarChar);
                param[4].Value = vManufactureID;

                param[5] = new SqlParameter("@error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[6].Value = vpkDeviceID;

                param[7] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[7].Value = bStatus;

                param[8] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[8].Value = vIDs;

                param[9] = new SqlParameter("@EditOrAdd", SqlDbType.Bit);
                param[9].Value = EditOrAdd;

                param[10] = new SqlParameter("@ipkDeviceDID", SqlDbType.Int);
                param[10].Value = ipkDeviceDID;

                param[11] = new SqlParameter("@dConfigureData", SqlDbType.DateTime);
                param[11].Value = dConfigureData;

                param[12] = new SqlParameter("@vMobileNo", SqlDbType.VarChar);
                param[12].Value = vMobileNo;

                param[13] = new SqlParameter("@vIpLocation", SqlDbType.VarChar);
                param[13].Value = vIpLocation;

                param[14] = new SqlParameter("@vPort", SqlDbType.VarChar);
                param[14].Value = vPort;

                param[15] = new SqlParameter("@bIsGprsActive", SqlDbType.Bit);
                param[15].Value = bIsGprsActive;

                param[16] = new SqlParameter("@iTimeInterval", SqlDbType.Int);
                param[16].Value = iTimeInterval;

                param[17] = new SqlParameter("@bIsActive", SqlDbType.Bit);
                param[17].Value = bIsActive;

                param[18] = new SqlParameter("@ifkDeviceId", SqlDbType.Int);
                param[18].Value = ifkDeviceId;

                param[19] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar, 300);
                param[19].Value = vDeviceName;

                param[20] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[20].Value = iTrackerType;

                param[21] = new SqlParameter("@vIDsUnAssign", SqlDbType.VarChar);
                param[21].Value = vIDsUnAssign;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_device", param);
                returnData = param[5].Value.ToString();

                if (param[5].Value.ToString() == "5")
                {
                    returnData = "Update successful ...!!";
                }
                else if (param[5].Value.ToString() == "1")
                {
                    returnData = "Save successful ...!!";
                }
                else if (param[5].Value.ToString() == "2")
                {
                    returnData = "Update  successful ...!!";
                }
                else if (param[5].Value.ToString() == "-2")
                {

                    returnData = "Some Device are not Enable Because it needs to Configure first...!!";
                }
                else if (param[5].Value.ToString() == "3")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "4")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "-1")
                {
                    returnData = "error##Device ID Already Exists ...!!";
                }
                else if (param[5].Value.ToString() == "-8")
                {
                    returnData = "Error Occur While Processing!";

                }
                else if (param[5].Value.ToString() == "-11")
                {
                    returnData = "error##Device Name Already Exists ...!!";
                }
                else if (param[5].Value.ToString() == "12")
                {
                    returnData = "Device  Detail Save successful ...!!";
                }
                else if (param[5].Value.ToString() == "-12")
                {
                    returnData = "Mobile No not available.Please assign another...!!";

                }
                else if (param[5].Value.ToString() == "13")
                {
                    returnData = " Device Congure successful!";

                }
                else if (param[5].Value.ToString() == "-13")
                {
                    returnData = "Device DisCongure successful!";

                }
                else if (param[5].Value.ToString() == "15")
                {
                    returnData = "Device name update successful!";

                }
                else if (param[5].Value.ToString() == "16")
                {
                    returnData = "Device status change successful!";

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsDeviceMaster.cs", "SaveDataForAdmin()", ex.Message + ex.StackTrace);
                returnData = "Error in Device Master=" + ex.GetBaseException().ToString();
            }
            return returnData;
        }


        public string SaveGroupDevice()
        {
            SqlParameter[] param = new SqlParameter[23];
            string returnData;
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@vBatchNo", SqlDbType.VarChar);
                param[2].Value = vBatchNo;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter("@vManufactureID", SqlDbType.VarChar);
                param[4].Value = vManufactureID;

                param[5] = new SqlParameter("@error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[6].Value = vpkDeviceID;

                param[7] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[7].Value = bStatus;

                param[8] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[8].Value = vIDs;

                param[9] = new SqlParameter("@EditOrAdd", SqlDbType.Bit);
                param[9].Value = EditOrAdd;

                param[10] = new SqlParameter("@ipkDeviceDID", SqlDbType.Int);
                param[10].Value = ipkDeviceDID;

                param[11] = new SqlParameter("@dConfigureData", SqlDbType.DateTime);
                param[11].Value = dConfigureData;

                param[12] = new SqlParameter("@vMobileNo", SqlDbType.VarChar);
                param[12].Value = vMobileNo;

                param[13] = new SqlParameter("@vIpLocation", SqlDbType.VarChar);
                param[13].Value = vIpLocation;

                param[14] = new SqlParameter("@vPort", SqlDbType.VarChar);
                param[14].Value = vPort;

                param[15] = new SqlParameter("@bIsGprsActive", SqlDbType.Bit);
                param[15].Value = bIsGprsActive;

                param[16] = new SqlParameter("@iTimeInterval", SqlDbType.Int);
                param[16].Value = iTimeInterval;

                param[17] = new SqlParameter("@bIsActive", SqlDbType.Bit);
                param[17].Value = bIsActive;

                param[18] = new SqlParameter("@ifkDeviceId", SqlDbType.Int);
                param[18].Value = ifkDeviceId;

                param[19] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar, 300);
                param[19].Value = vDeviceName;

                param[20] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[20].Value = iTrackerType;

                param[21] = new SqlParameter("@vIDsUnAssign", SqlDbType.VarChar);
                param[21].Value = vIDsUnAssign;

                param[22] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[22].Value = ifkGroupMID;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_device", param);
                returnData = param[5].Value.ToString();

                if (param[5].Value.ToString() == "5")
                {
                    returnData = "Update successful ...!!";
                }
                else if (param[5].Value.ToString() == "1")
                {
                    returnData = "Save successful ...!!";
                }
                else if (param[5].Value.ToString() == "2")
                {
                    returnData = "Update  successful ...!!";
                }
                else if (param[5].Value.ToString() == "-2")
                {
                    //returnData = Resources.language.Some + " " + Resources.language.Device + " " + Resources.language.Are + " " + Resources.language.not + " " + Resources.language.enable + " " + Resources.language.Because + " " + Resources.language.it + " " + Resources.language.needs + " " + Resources.language.to + " " + Resources.language.Configure + " " + Resources.language.first + "...!!";
                    returnData = "Some Device are not Enable Because it needs to Configure first...!!";
                }
                else if (param[5].Value.ToString() == "3")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "4")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "-1")
                {
                    returnData = "error##Device ID Already Exists ...!!";
                }
                else if (param[5].Value.ToString() == "-8")
                {
                    returnData = "Error Occur While Processing!";
                    //returnData = Resources.language.Error + " " + Resources.language.Occur + " " + Resources.language.While + " " + Resources.language.Processing + "!";
                }
                else if (param[5].Value.ToString() == "-11")
                {
                    returnData = "error##Device Name Already Exists ...!!";
                }
                else if (param[5].Value.ToString() == "12")
                {
                    returnData = "Device  Detail Save successful ...!!";
                }
                else if (param[5].Value.ToString() == "-12")
                {
                    returnData = "Mobile No not available.Please assign another...!!";
                    //returnData = Resources.language.Mobile + " " + Resources.language.Number + " " + Resources.language.not + " " + Resources.language.available + ".  " + Resources.language.Please + " " + Resources.language.Assign + " " + Resources.language.another + "...!!";
                }
                else if (param[5].Value.ToString() == "13")
                {
                    returnData = " Device Congure successful!";
                    //returnData = Resources.language.Device + " " + Resources.language.Configure + " " + Resources.language.successful + "!";
                }
                else if (param[5].Value.ToString() == "-13")
                {
                    returnData = "Device DisCongure successful!";
                    //returnData = Resources.language.Device + " " + Resources.language.Disconfigure + " " + Resources.language.successful + "!";
                }
                else if (param[5].Value.ToString() == "15")
                {
                    returnData = "Device name update successful!";
                    //returnData = Resources.language.Device + " " + Resources.language.Name + " " + Resources.language.Update + " " + Resources.language.successful + "...!!";
                }
                else if (param[5].Value.ToString() == "16")
                {
                    returnData = "Device status change successful!";
                    //returnData = Resources.language.Device + " " + Resources.language.Status + " " + Resources.language.Change + " " + Resources.language.successful + "...!!";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsDeviceMaster.cs", "SaveDataForAdmin()", ex.Message + ex.StackTrace);
                returnData = "Error in Device Master=" + ex.GetBaseException().ToString();
            }
            return returnData;
        }

        public List<clsDeviceMaster> FillGroupDeviceMaster()
        {
            List<clsDeviceMaster> lstMaster = new List<clsDeviceMaster>();
            clsDeviceMaster objMaster = new clsDeviceMaster();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;
                param[1] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_device", param);
                // lstMaster.Add(new clsDeviceMaster(-1, "Select"));
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        lstMaster.Add(new clsDeviceMaster(Convert.ToInt32(drRow["ifkGroupMID"].ToString()), Convert.ToInt32(drRow["ipkDeviceID"].ToString()), drRow["vpkDeviceID"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                ////HttpContext.Current.Response.Write(ex.Message);
            }
            return lstMaster;
        }


        public List<clsDeviceMaster> FillCompanyDevice(int iUserMasterID)
        {
            List<clsDeviceMaster> lstMaster = new List<clsDeviceMaster>();
            clsDeviceMaster objMaster = new clsDeviceMaster();
            DataSet ds = new DataSet();
            try
            {
                //SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                //param[0].Value = Operation;
                //param[1] = new SqlParameter("@ifkuserID", SqlDbType.Int);
                //param[1].Value = ifkUserID;
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, "SELECT ipkDeviceID,vpkDeviceID,ifkUserID FROM  tblDevice_Master where ifkUserID=" + iUserMasterID + " ");
                // lstMaster.Add(new clsDeviceMaster(-1, "Select"));
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        lstMaster.Add(new clsDeviceMaster(Convert.ToInt32(drRow["ipkDeviceID"].ToString()), drRow["vpkDeviceID"].ToString(), Convert.ToInt32(drRow["ifkUserID"].ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                ////HttpContext.Current.Response.Write(ex.Message);
            }
            return lstMaster;
        }

        public string SaveAdminDevice()
        {
            SqlParameter[] param = new SqlParameter[22];
            string returnData;
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@vBatchNo", SqlDbType.VarChar);
                param[2].Value = vBatchNo;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter("@vManufactureID", SqlDbType.VarChar);
                param[4].Value = vManufactureID;

                param[5] = new SqlParameter("@error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[6].Value = vpkDeviceID;

                param[7] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[7].Value = bStatus;

                param[8] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[8].Value = vIDs;

                param[9] = new SqlParameter("@EditOrAdd", SqlDbType.Bit);
                param[9].Value = EditOrAdd;

                param[10] = new SqlParameter("@ipkDeviceDID", SqlDbType.Int);
                param[10].Value = ipkDeviceDID;

                param[11] = new SqlParameter("@dConfigureData", SqlDbType.DateTime);
                param[11].Value = dConfigureData;

                param[12] = new SqlParameter("@vMobileNo", SqlDbType.VarChar);
                param[12].Value = vMobileNo;

                param[13] = new SqlParameter("@vIpLocation", SqlDbType.VarChar);
                param[13].Value = vIpLocation;

                param[14] = new SqlParameter("@vPort", SqlDbType.VarChar);
                param[14].Value = vPort;

                param[15] = new SqlParameter("@bIsGprsActive", SqlDbType.Bit);
                param[15].Value = bIsGprsActive;

                param[16] = new SqlParameter("@iTimeInterval", SqlDbType.Int);
                param[16].Value = iTimeInterval;

                param[17] = new SqlParameter("@bIsActive", SqlDbType.Bit);
                param[17].Value = bIsActive;

                param[18] = new SqlParameter("@ifkDeviceId", SqlDbType.Int);
                param[18].Value = ifkDeviceId;

                param[19] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar, 300);
                param[19].Value = vDeviceName;

                param[20] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[20].Value = iTrackerType;

                param[21] = new SqlParameter("@vIDsUnAssign", SqlDbType.VarChar);
                param[21].Value = vIDsUnAssign;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_Admindevice", param);
                returnData = param[5].Value.ToString();

                if (param[5].Value.ToString() == "5")
                {
                    returnData = "Update successful ...!!";
                }
                else if (param[5].Value.ToString() == "1")
                {
                    returnData = "Save successful ...!!";
                }
                else if (param[5].Value.ToString() == "2")
                {
                    returnData = "Update  successful ...!!";
                }
                else if (param[5].Value.ToString() == "-2")
                {

                    returnData = "Some Device are not Enable Because it needs to Configure first...!!";
                }
                else if (param[5].Value.ToString() == "3")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "4")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "-1")
                {
                    returnData = "error##Device ID Already Exists ...!!";
                }
                else if (param[5].Value.ToString() == "-8")
                {
                    returnData = "Error Occur While Processing!";

                }
                else if (param[5].Value.ToString() == "-11")
                {
                    returnData = "error##Device Name Already Exists ...!!";
                }
                else if (param[5].Value.ToString() == "12")
                {
                    returnData = "Device  Detail Save successful ...!!";
                }
                else if (param[5].Value.ToString() == "-12")
                {
                    returnData = "Mobile No not available.Please assign another...!!";

                }
                else if (param[5].Value.ToString() == "13")
                {
                    returnData = " Device Congure successful!";

                }
                else if (param[5].Value.ToString() == "-13")
                {
                    returnData = "Device DisCongure successful!";

                }
                else if (param[5].Value.ToString() == "15")
                {
                    returnData = "Device name update successful!";

                }
                else if (param[5].Value.ToString() == "16")
                {
                    returnData = "Device status change successful!";

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsDeviceMaster.cs", "SaveDataForAdmin()", ex.Message + ex.StackTrace);
                returnData = "Error in Device Master=" + ex.GetBaseException().ToString();
            }
            return returnData;
        }
        public string SaveAdminDevice1(int reselerId)
        {
            SqlParameter[] param = new SqlParameter[23];
            string returnData;
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 18;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@vBatchNo", SqlDbType.VarChar);
                param[2].Value = vBatchNo;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter("@vManufactureID", SqlDbType.VarChar);
                param[4].Value = vManufactureID;

                param[5] = new SqlParameter("@error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[6].Value = vpkDeviceID;

                param[7] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[7].Value = bStatus;

                param[8] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[8].Value = vIDs;

                param[9] = new SqlParameter("@EditOrAdd", SqlDbType.Bit);
                param[9].Value = EditOrAdd;

                param[10] = new SqlParameter("@ipkDeviceDID", SqlDbType.Int);
                param[10].Value = ipkDeviceDID;

                param[11] = new SqlParameter("@dConfigureData", SqlDbType.DateTime);
                param[11].Value = dConfigureData;

                param[12] = new SqlParameter("@vMobileNo", SqlDbType.VarChar);
                param[12].Value = vMobileNo;

                param[13] = new SqlParameter("@vIpLocation", SqlDbType.VarChar);
                param[13].Value = vIpLocation;

                param[14] = new SqlParameter("@vPort", SqlDbType.VarChar);
                param[14].Value = vPort;

                param[15] = new SqlParameter("@bIsGprsActive", SqlDbType.Bit);
                param[15].Value = bIsGprsActive;

                param[16] = new SqlParameter("@iTimeInterval", SqlDbType.Int);
                param[16].Value = iTimeInterval;

                param[17] = new SqlParameter("@bIsActive", SqlDbType.Bit);
                param[17].Value = bIsActive;

                param[18] = new SqlParameter("@ifkDeviceId", SqlDbType.Int);
                param[18].Value = ifkDeviceId;

                param[19] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar, 300);
                param[19].Value = vDeviceName;

                param[20] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[20].Value = iTrackerType;

                param[21] = new SqlParameter("@vIDsUnAssign", SqlDbType.VarChar);
                param[21].Value = vIDsUnAssign;

                param[22] = new SqlParameter("@reselerId", SqlDbType.Int);
                param[22].Value = reselerId;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_Admindevice", param);
                returnData = param[5].Value.ToString();

                if (param[5].Value.ToString() == "5")
                {
                    returnData = "Update successful ...!!";
                }
                else if (param[5].Value.ToString() == "1")
                {
                    returnData = "Save successful ...!!";
                }
                else if (param[5].Value.ToString() == "2")
                {
                    returnData = "Update  successful ...!!";
                }
                else if (param[5].Value.ToString() == "-2")
                {

                    returnData = "Some Device are not Enable Because it needs to Configure first...!!";
                }
                else if (param[5].Value.ToString() == "3")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "4")
                {
                    returnData = "Delete successful ...!!";
                }
                else if (param[5].Value.ToString() == "-1")
                {
                    returnData = "error##Device ID Already Exists ...!!";
                }
                else if (param[5].Value.ToString() == "-8")
                {
                    returnData = "Error Occur While Processing!";

                }
                else if (param[5].Value.ToString() == "-11")
                {
                    returnData = "error##Device Name Already Exists ...!!";
                }
                else if (param[5].Value.ToString() == "12")
                {
                    returnData = "Device  Detail Save successful ...!!";
                }
                else if (param[5].Value.ToString() == "-12")
                {
                    returnData = "Mobile No not available.Please assign another...!!";

                }
                else if (param[5].Value.ToString() == "13")
                {
                    returnData = " Device Congure successful!";

                }
                else if (param[5].Value.ToString() == "-13")
                {
                    returnData = "Device DisCongure successful!";

                }
                else if (param[5].Value.ToString() == "15")
                {
                    returnData = "Device name update successful!";

                }
                else if (param[5].Value.ToString() == "16")
                {
                    returnData = "Device status change successful!";

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsDeviceMaster.cs", "SaveDataForAdmin()", ex.Message + ex.StackTrace);
                returnData = "Error in Device Master=" + ex.GetBaseException().ToString();
            }
            return returnData;
        }

        public List<clsDeviceMaster> FillFrontGroupDevice(int userid)
        {
            List<clsDeviceMaster> lstMaster = new List<clsDeviceMaster>();
            clsDeviceMaster objMaster = new clsDeviceMaster();
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@userid", SqlDbType.Int);
                param[1].Value = userid;
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_FrontGroupDevice", param);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        lstMaster.Add(new clsDeviceMaster(Convert.ToInt32(drRow["ifkDeviceID"].ToString()), drRow["vpkDeviceID"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                ////HttpContext.Current.Response.Write(ex.Message);
            }
            return lstMaster;
        }

        public string SaveSharedDevice()
        {
            SqlParameter[] param = new SqlParameter[6];
            string results = "";
            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[1].Value = iParentID;

                param[2] = new SqlParameter("@ifkDeviceID", SqlDbType.BigInt);
                param[2].Value = ifkDeviceId;

                param[3] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar);
                param[3].Value = vDeviceName;

                param[4] = new SqlParameter("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[5].Value = ifkUserID;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                results = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveSharedDevice()", ex.Message + ex.StackTrace);
                results = "Internal execution error occured!";
            }

            return results;
        }

        public string MoveDevice()
        {
            SqlParameter[] param = new SqlParameter[5];
            string results = "";
            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[1].Value = iParentID;

                param[2] = new SqlParameter("@ifkDeviceID", SqlDbType.BigInt);
                param[2].Value = ifkDeviceId;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;

                param[4] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[4].Value = ifkUserID;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                results = param[3].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "MoveDevice()", ex.Message + ex.StackTrace);
                results = "Internal execution error occured!";
            }

            return results;
        }

        public string SaveAssetName()
        {
            SqlParameter[] param = new SqlParameter[5];
            string results = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceId;

                param[2] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar);
                param[2].Value = vDeviceName;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;

                param[4] = new SqlParameter("@IsDriver", SqlDbType.Bit);
                param[4].Value = IsDriver;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "DevicesManagement", param);

                results = param[3].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveAssetName()", ex.Message + ex.StackTrace);
                results = "Internal execution error occured!";
            }

            return results;
        }

        public string UpdateAssetWorkMode()
        {
            SqlParameter[] param = new SqlParameter[4];
            string results = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceId;

                param[2] = new SqlParameter("@WorkMode", SqlDbType.Int);
                param[2].Value = WorkMode;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "DevicesManagement", param);

                results = param[3].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveAssetName()", ex.Message + ex.StackTrace);
                results = "Internal execution error occured!";
            }

            return results;
        }


    }
}