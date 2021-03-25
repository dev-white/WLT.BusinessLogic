using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL {
    public class clsNewGroup : IDisposable {


        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        private int _Operation;
        private int _ipkGroupMID;
        private string _vpkGroupName;
        private int _ifkUserID;
        private bool _bStatus;
        private int _ipkGroupDID;
        private int _Error;
        private int _iParentGMID;
        private int _ifkGroupID;
        private int _ifkGroupMID;
        private int _iParentID;
        private int _iCreatedBy;
        private string _chkGID;
        private List<clsNewGroup> _chkGroupID;
        private string _chkGHTML;
        private Int64 _nIMEINo;
        private int _DeviceID;
        private string _vpkDeviceID;
        private string _vTrackerTypeName;
        private int _ifkCompanyID;
        private string _vCompanyName;

        //for Zones
        private int _ipkZonesGroupDID;
        private string _vZonesGroupName;
        private int _iZonesParentGMID;

        private int _iZonesParentID;
        private int _iZonesCreatedBy;
        private int _ifkZonesGroupMID;
        private string _isAddAction;
        private int _divId;
        private int _ifkDeviceID;

        private DateTime _ZoneVisitedDate;

        public DateTime ZoneVisitedDate {
            get { return _ZoneVisitedDate; }
            set { _ZoneVisitedDate = value; }
        }
        public string isAddAction {
            get { return _isAddAction; }
            set { _isAddAction = value; }
        }
        public string vCompanyName {
            get { return _vCompanyName; }
            set { _vCompanyName = value; }
        }
        public string chkGID {
            get { return _chkGID; }
            set { _chkGID = value; }
        }

        public List<clsNewGroup> chkGroupID {
            get { return _chkGroupID; }
            set { _chkGroupID = value; }
        }

        private List<clsNewGroup> childgroup1;

        public List<clsNewGroup> Childgroup {
            get { return childgroup1; }
            set { childgroup1 = value; }
        }
        public string chkGHTML {
            get { return _chkGHTML; }
            set { _chkGHTML = value; }
        }
        public int ifkGroupMID {
            get { return _ifkGroupMID; }
            set { _ifkGroupMID = value; }
        }
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkGroupMID { get { return _ipkGroupMID; } set { _ipkGroupMID = value; } }
        public string vpkGroupName { get { return _vpkGroupName; } set { _vpkGroupName = value; } }
        public int ifkUserID { get { return _ifkUserID; } set { _ifkUserID = value; } }

        public int ifkDeviceID { get { return _ifkDeviceID; } set { _ifkDeviceID = value; } }

        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public int Error { get { return _Error; } set { _Error = value; } }
        public int ipkGroupDID { get { return _ipkGroupDID; } set { _ipkGroupDID = value; } }
        public int iParentGMID { get { return _iParentGMID; } set { _iParentGMID = value; } }
        public int ifkGroupID { get { return _ifkGroupID; } set { _ifkGroupID = value; } }
        public int iParentID { get { return _iParentID; } set { _iParentID = value; } }
        public int iCreatedBy { get { return _iCreatedBy; } set { _iCreatedBy = value; } }
        public Int64 nIMEINo { get { return _nIMEINo; } set { _nIMEINo = value; } }
        public int DeviceID { get { return _DeviceID; } set { _DeviceID = value; } }
        public string vTrackerTypeName { get { return _vTrackerTypeName; } set { _vTrackerTypeName = value; } }
        public string vpkDeviceID { get { return _vpkDeviceID; } set { _vpkDeviceID = value; } }
        public int ifkCompanyID { get { return _ifkCompanyID; } set { _ifkCompanyID = value; } }

        //Property for Zones Groups
        public int ipkZonesGroupDID { get { return _ipkZonesGroupDID; } set { _ipkZonesGroupDID = value; } }
        public int iZonesParentGMID { get { return _iZonesParentGMID; } set { _iZonesParentGMID = value; } }
        public string vZonesGroupName { get { return _vZonesGroupName; } set { _vZonesGroupName = value; } }
        public int iZonesfkGroupID { get { return _ifkGroupID; } set { _ifkGroupID = value; } }
        public int iZonesParentID { get { return _iZonesParentID; } set { _iZonesParentID = value; } }
        public int iZonesCreatedBy { get { return _iZonesCreatedBy; } set { _iZonesCreatedBy = value; } }
        public int ifkZonesGroupMID { get { return _ifkZonesGroupMID; } set { _ifkZonesGroupMID = value; } }

        public int divId { get { return _divId; } set { _divId = value; } }
        public int IsDriver { get; set; }
        public int[] zoneIds { get; set; }

        public clsNewGroup () {
            // constrocter here
        }

        public clsNewGroup (string chkGID) {
            this.chkGID = chkGID;

        }
        public clsNewGroup (int ifkGroupID) {
            this.ifkGroupID = ifkGroupID;
        }

        public clsNewGroup (string vpkGroupName, int ipkGroupMID) {
            this.vpkGroupName = vpkGroupName;
            this.ipkGroupMID = ipkGroupMID;
        }

        public clsNewGroup (List<clsNewGroup> childgroup, string vpkGroupName, int ipkGroupMID) {
            this.vpkGroupName = vpkGroupName;
            this.ipkGroupMID = ipkGroupMID;
            this.Childgroup = childgroup;
        }

        public clsNewGroup (string vpkDeviceID, string vTrackerTypeName) {
            this.vpkDeviceID = vpkDeviceID;
            this.vTrackerTypeName = vTrackerTypeName;
        }

        public clsNewGroup (string vCompanyName, string vpkGroupName, int ifkCompanyID, int ifkGroupID) {
            this.vCompanyName = vCompanyName;
            this.vpkGroupName = vpkGroupName;
            this.ifkCompanyID = ifkCompanyID;
            this.ifkGroupID = ifkGroupID;

        }

        public clsNewGroup (string chkGHTML, List<clsNewGroup> chkGroupID) {
            this.chkGHTML = chkGHTML;
            this.chkGroupID = chkGroupID;
        }

        public DataSet GetGroups () {
            DataSet ds = new DataSet ();
            SqlParameter[] param = new SqlParameter[7];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@iParentGMID", SqlDbType.Int);
                param[1].Value = iParentGMID;

                param[2] = new SqlParameter ("@iParentID", SqlDbType.Int);
                param[2].Value = iParentID;

                param[3] = new SqlParameter ("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter ("@ifkDeviceID", SqlDbType.Int);
                param[4].Value = ifkDeviceID;

                param[5] = new SqlParameter ("@ifkGroupID", SqlDbType.Int);
                param[5].Value = ifkGroupID;

                param[6] = new SqlParameter ("@nfkIMIENo", SqlDbType.BigInt);
                param[6].Value = nIMEINo;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "Newsp_Group", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "GetGroups()", ex.Message  + ex.StackTrace);

            }
            return ds;

        }

        public string SaveGroup () {
            DataSet ds = new DataSet ();
            string result = "";
            SqlParameter[] param = new SqlParameter[7];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ipkGroupMID", SqlDbType.Int);
                param[1].Value = ipkGroupDID;

                param[2] = new SqlParameter ("@vpkGroupName", SqlDbType.VarChar);
                param[2].Value = vpkGroupName;

                param[3] = new SqlParameter ("@iParentGMID", SqlDbType.Int);
                param[3].Value = iParentGMID;

                param[4] = new SqlParameter ("@iParentID", SqlDbType.Int);
                param[4].Value = iParentID;

                param[5] = new SqlParameter ("@iCreatedBy", SqlDbType.Int);
                param[5].Value = iCreatedBy;

                param[6] = new SqlParameter ("@Error", SqlDbType.Int);
                param[6].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery (f_strConnectionString, CommandType.StoredProcedure, "Newsp_Group", param);
                result = param[6].Value.ToString ();
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "SaveGroup()", ex.Message  + ex.StackTrace);
                result = "Internal Execution Error :" + ex.Message;
            }
            return result;

        }
        public DataSet GetZonesGroups () {
            DataSet ds = new DataSet ();
            SqlParameter[] param = new SqlParameter[6];
            try {

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@iZonesParentGMID", SqlDbType.Int);
                param[1].Value = iZonesParentGMID;

                param[2] = new SqlParameter ("@iZonesParentID", SqlDbType.Int);
                param[2].Value = iZonesParentID;

                param[3] = new SqlParameter ("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter ("@ifkGroupID", SqlDbType.Int);
                param[4].Value = ifkGroupID;

                param[5] = new SqlParameter ("@nfkIMIENo", SqlDbType.VarChar);
                param[5].Value = nIMEINo;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "Newsp_ZonesGroup", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "GetGroups()", ex.Message  + ex.StackTrace);

            }
            return ds;

        }

        public DataSet GetZonesGroupsCounts () {
            DataSet ds = new DataSet ();
            SqlParameter[] param = new SqlParameter[1];
            try {

                param[0] = new SqlParameter ("@ifkCompanyID", SqlDbType.Int);
                param[0].Value = ifkCompanyID;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "sp_GeoZonesGroups", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "GetZonesGroupsCounts()", ex.Message  + ex.StackTrace);

            }
            return ds;

        }

        public string SaveZonesGroup () {
            DataSet ds = new DataSet ();
            string result = "";
            SqlParameter[] param = new SqlParameter[7];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ipkZonesGroupMID", SqlDbType.Int);
                param[1].Value = ipkZonesGroupDID;

                param[2] = new SqlParameter ("@vpkZonesGroupName", SqlDbType.VarChar);
                param[2].Value = vZonesGroupName;

                param[3] = new SqlParameter ("@iZonesParentGMID", SqlDbType.Int);
                param[3].Value = iZonesParentGMID;

                param[4] = new SqlParameter ("@iZonesParentID", SqlDbType.Int);
                param[4].Value = iZonesParentID;

                param[5] = new SqlParameter ("@iZonesCreatedBy", SqlDbType.Int);
                param[5].Value = iZonesCreatedBy;

                param[6] = new SqlParameter ("@Error", SqlDbType.Int);
                param[6].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery (f_strConnectionString, CommandType.StoredProcedure, "Newsp_ZonesGroup", param);
                result = param[6].Value.ToString ();
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "SaveZonesGroup()", ex.Message  + ex.StackTrace);
                result = "Internal Execution Error :" + ex.Message;
            }

            return result;

        }

        public string SaveGroupDevice () {
            DataSet ds = new DataSet ();
            string result = "";
            SqlParameter[] param = new SqlParameter[6];
            try {

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@iParentID", SqlDbType.Int);
                param[1].Value = iParentID;

                param[2] = new SqlParameter ("@ifkGroupID", SqlDbType.Int);
                param[2].Value = ifkGroupID;

                param[3] = new SqlParameter ("@ifkDeviceID", SqlDbType.BigInt);
                param[3].Value = ifkDeviceID;

                param[4] = new SqlParameter ("@nfkIMIENo", SqlDbType.BigInt);
                param[4].Value = nIMEINo;

                param[5] = new SqlParameter ("@IsDriver", SqlDbType.Int);
                param[5].Value = IsDriver;

                SqlHelper.ExecuteNonQuery (f_strConnectionString, CommandType.StoredProcedure, "Newsp_Group", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "SaveGroup()", ex.Message  + ex.StackTrace);
                result = "Internal Execution Error :" + ex.Message;
            }
            return result;

        }

        public List<clsNewGroup> GetAllGroupforAlert () {
            DataSet ds = new DataSet ();
            SqlParameter[] param = new SqlParameter[5];
            List<clsNewGroup> obj = new List<clsNewGroup> ();
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@iParentGMID", SqlDbType.Int);
                param[1].Value = iParentGMID;

                param[2] = new SqlParameter ("@iParentID", SqlDbType.Int);
                param[2].Value = ifkCompanyID;

                param[3] = new SqlParameter ("@ifkGroupID", SqlDbType.Int);
                param[3].Value = ifkGroupID;

                param[4] = new SqlParameter ("@ifkUserID", SqlDbType.Int);
                param[4].Value = ifkUserID;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "Newsp_Group", param);
                obj.Add (new clsNewGroup ("Select Groups", -1));

                foreach (DataRow row in ds.Tables[0].Rows) {
                    clsNewGroup objclsGroup = new clsNewGroup ();
                    objclsGroup.ifkGroupMID = Convert.ToInt32 (row["ipkGroupMID"].ToString ());
                    int count = objclsGroup.CountDeviceInGroup ();

                    obj.Add (new clsNewGroup ((row["vpkGroupName"].ToString () + " (" + count + ")"), Convert.ToInt32 (row["ipkGroupMID"].ToString ())));
                }
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "GetAllGroup()", ex.Message  + ex.StackTrace);
            }
            return obj;
        }

        public int CountDeviceInGroup () {
            int result = 0;

            try {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 4;

                param[1] = new SqlParameter ("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;

                result = Convert.ToInt32 (SqlHelper.ExecuteScalar (f_strConnectionString.ToString (), CommandType.StoredProcedure, "NewFrontData_sp", param));
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsGroup.cs", "CountDeviceInGroup()", ex.Message  + ex.StackTrace);
            }
            return result;
        }

        public string GetGroupDeviceIDs () {
            string result = "";

            try {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 30;

                param[1] = new SqlParameter ("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;

                result = SqlHelper.ExecuteScalar (f_strConnectionString.ToString (), CommandType.StoredProcedure, "NewFrontData_sp", param).ToString ();
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsGroup.cs", "CountDeviceInGroup()", ex.Message  + ex.StackTrace);
            }
            return result;
        }

        public int CountDeviceInAllGroup () {
            int result = 0;

            try {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@iParent", SqlDbType.Int);
                param[1].Value = iParentID;

                param[2] = new SqlParameter ("@ipkUserID", SqlDbType.Int);
                param[2].Value = ifkUserID;

                result = Convert.ToInt32 (SqlHelper.ExecuteScalar (f_strConnectionString.ToString (), CommandType.StoredProcedure, "NewFrontData_sp", param));
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsGroup.cs", "CountDeviceInAllGroup()", ex.Message  + ex.StackTrace);
            }
            return result;
        }

        public List<clsNewGroup> GetAssetTracker () {
            DataSet ds = new DataSet ();
            List<clsNewGroup> obj2 = new List<clsNewGroup> ();
            //string qry = "";

            try {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@iParent", SqlDbType.Int);
                param[1].Value = iParentID;

                param[2] = new SqlParameter ("@ifkGroupMID", SqlDbType.Int);
                param[2].Value = ifkGroupID;

                param[3] = new SqlParameter ("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                obj2.Add (new clsNewGroup ("-1", "Select Assets"));

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                    foreach (DataRow row in ds.Tables[0].Rows) {
                        obj2.Add (new clsNewGroup (row["vpkDeviceID"].ToString (), row["vDeviceName"].ToString () + "(" + row["vTrackerTypeName"].ToString () + ")"));
                    }
                }
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsGroup.cs", "GetTrackerTypesDevice()", ex.Message  + ex.StackTrace);

            }
            return obj2;
        }

        public DataSet GetTripReplayAsset () {
            DataSet ds = new DataSet ();

            try {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@iParent", SqlDbType.Int);
                param[1].Value = iParentID;

                param[2] = new SqlParameter ("@ifkGroupMID", SqlDbType.Int);
                param[2].Value = ifkGroupID;

                param[3] = new SqlParameter ("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "GetTripReplayAsset()", ex.Message  + ex.StackTrace);

            }
            return ds;
        }
        public int CountZonesGoupScreen () {
            int result = 0;

            try {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;

                param[2] = new SqlParameter ("@ifkCompanyId", SqlDbType.Int);
                param[2].Value = ifkCompanyID;

                result = Convert.ToInt32 (SqlHelper.ExecuteScalar (f_strConnectionString.ToString (), CommandType.StoredProcedure, "sp_Geofence", param));
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsGroup.cs", "CountDeviceInGroup()", ex.Message  + ex.StackTrace);
            }
            return result;
        }

        public class NewGroupDDL {
            public string vpkGroupName { get; set; }
            public int ipkGroupMID { get; set; }

            public NewGroupDDL (string vpkGroupName) {
                this.vpkGroupName = vpkGroupName;
            }

            public NewGroupDDL (string vpkGroupName, int ipkGroupMID) {
                this.vpkGroupName = vpkGroupName;
                this.ipkGroupMID = ipkGroupMID;
            }

        }

        public class NewGroupAssetsDDL {
            public string vpkDeviceID { get; set; }
            public string vTrackerTypeName { get; set; }

            public NewGroupAssetsDDL (string vTrackerTypeName) {

            }
            public NewGroupAssetsDDL (string vpkDeviceID, string vTrackerTypeName) {
                this.vpkDeviceID = vpkDeviceID;
                this.vTrackerTypeName = vTrackerTypeName;
            }

        }

        public DataSet GetZonesActivity (string StartDateRange, string EndDateRange) {
            DataSet ds = new DataSet ();

            try {
                SqlParameter[] param = new SqlParameter[5];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;

                param[2] = new SqlParameter ("@ifkCompanyId", SqlDbType.Int);
                param[2].Value = ifkCompanyID;

                param[3] = new SqlParameter ("@StartDateRange", SqlDbType.DateTime);
                param[3].Value = StartDateRange;

                param[4] = new SqlParameter ("@EndDateRange", SqlDbType.DateTime);
                param[4].Value = EndDateRange;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString.ToString (), CommandType.StoredProcedure, "ZonePage", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "GetZonesActivity()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetZonesAssets () {
            DataSet ds = new DataSet ();

            try {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;

                param[2] = new SqlParameter ("@ifkCompanyId", SqlDbType.Int);
                param[2].Value = ifkCompanyID;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString.ToString (), CommandType.StoredProcedure, "ZonePage", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsNewGroup.cs", "GetZonesAssets()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public void Dispose () {
            Dispose ();
            GC.Collect ();
            GC.SuppressFinalize (this);
        }

    }
}