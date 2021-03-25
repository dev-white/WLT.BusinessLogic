using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsGroup : General
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();

        private int _ifkCompanyID;
        private int _Operation;
        private int _ipkGroupMID;
        private String _vpkGroupName;
        private int _ifkUserID;
        private bool _bStatus;
        private int _ipkGroupDID;
        private int _ifkGroupMID;
        private int _ifkDeviceID;
        private string _vIDs;
        private int _Error;
        private List<clsGroup> _Trackers;
        private List<clsGroup> _SelectedTrackers;

        private int _ipkTrackerTypeID;
        private string _vTrackerTypeName;
        private int _iParentGMID;
        private int _ifkGroupID;
        private string _DeviceName;
        private string _vpkDeviceID;
        private int _top;
        private int _ifkReportID;
        private string _like;

        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkGroupMID { get { return _ipkGroupMID; } set { _ipkGroupMID = value; } }
        public string vpkGroupName { get { return _vpkGroupName; } set { _vpkGroupName = value; } }
        public int ifkUserID { get { return _ifkUserID; } set { _ifkUserID = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public string vIDs { get { return _vIDs; } set { _vIDs = value; } }
        public int Error { get { return _Error; } set { _Error = value; } }
        public int ipkGroupDID { get { return _ipkGroupDID; } set { _ipkGroupDID = value; } }
        public int ifkGroupMID { get { return _ifkGroupMID; } set { _ifkGroupMID = value; } }
        public int ifkDeviceID { get { return _ifkDeviceID; } set { _ifkDeviceID = value; } }
        public List<clsGroup> Trackers { get { return _Trackers; } set { _Trackers = value; } }
        public List<clsGroup> SelectedTrackers { get { return _SelectedTrackers; } set { _SelectedTrackers = value; } }
        public int ipkTrackerTypeID { get { return _ipkTrackerTypeID; } set { _ipkTrackerTypeID = value; } }
        public string vTrackerTypeName { get { return _vTrackerTypeName; } set { _vTrackerTypeName = value; } }
        public int iParentGMID { get { return _iParentGMID; } set { _iParentGMID = value; } }
        public int ifkCompanyID { get { return _ifkCompanyID; } set { _ifkCompanyID = value; } }
        public int ifkGroupID { get { return _ifkGroupID; } set { _ifkGroupID = value; } }
        public string DeviceName { get { return _DeviceName; } set { _DeviceName = value; } }
        public string vpkDeviceID { get { return _vpkDeviceID; } set { _vpkDeviceID = value; } }
        public int top { get { return _top; } set { _top = value; } }
        public int ifkReportID { get { return _ifkReportID; } set { _ifkReportID = value; } }
        public string like { get { return _like; } set { _like = value; } }

        public string DeleteGroup()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkGroupMID", SqlDbType.Int);
                param[1].Value = ipkGroupMID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[3].Value = vIDs;

               SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_Group", param);
                return param[2].Value.ToString(); ;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public clsGroup(string vpkDeviceID, string vTrackerTypeName)
        {
            this.vpkDeviceID = vpkDeviceID;
            this.vTrackerTypeName = vTrackerTypeName;
        }
        public clsGroup(string vTrackerTypeName)
        {
            this.vTrackerTypeName = vTrackerTypeName;

        }
        public clsGroup(int ipkTrackerTypeID, string vTrackerTypeName)
        {
            this.ipkTrackerTypeID = ipkTrackerTypeID;
            this.vTrackerTypeName = vTrackerTypeName;

        }
        public clsGroup(string vpkGroupName, int ipkGroupMID)
        {
            this.vpkGroupName = vpkGroupName;
            this.ipkGroupMID = ipkGroupMID;
        }
        public clsGroup(string vpkGroupName, List<clsGroup> SelectedTrackers, List<clsGroup> Trackers)
        {
            this.vpkGroupName = vpkGroupName;
            this.SelectedTrackers = SelectedTrackers;
            this.Trackers = Trackers;
        }
        public clsGroup()
        {
            //const

        }

        public clsGroup(int ipkGroupMID, string vpkGroupName, bool bStatus, int iParentGMID, int ifkUserID, int ifkCompanyID)
        {
            this.vpkGroupName = vpkGroupName;
            this.ipkGroupMID = ipkGroupMID;
            this.bStatus = bStatus;
            this.iParentGMID = iParentGMID;
            this.ifkUserID = ifkUserID;
            this.ifkCompanyID = ifkCompanyID;

        }


        public clsGroup EditGroupName()
        {
            DataSet ds = new DataSet();
            clsGroup objGroupMaster = new clsGroup();
            List<clsGroup> lstSelectedTracker = new List<clsGroup>();
            List<clsGroup> lstTracker = new List<clsGroup>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkGroupMID", SqlDbType.Int);
                param[1].Value = ipkGroupMID;

                ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_Group", param);
                vpkGroupName = ds.Tables[0].Rows[0][0].ToString();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drsltracker in ds.Tables[1].Rows)
                    {
                        lstSelectedTracker.Add(new clsGroup(drsltracker["vTrackerTypeName"].ToString()));
                    }
                    foreach (DataRow drTracker in ds.Tables[2].Rows)
                    {
                        lstTracker.Add(new clsGroup(Convert.ToInt32(drTracker["ipkTrackerTypeID"].ToString()), drTracker["vTrackerTypeName"].ToString()));
                    }
                    DataRow drMaster = ds.Tables[0].Rows[0];
                    {
                        objGroupMaster = new clsGroup(drMaster["vpkGroupName"].ToString(), lstSelectedTracker, lstTracker);


                    }
                }

            }
            catch (Exception ex)
            {
                clsGroup objreturn = new clsGroup();
                LogError.RegisterErrorInLogFile( "clsGeofenceMaster.cs", "EditGeoFence()", ex.Message  + ex.StackTrace);
                objreturn.Error = 1;
                return objreturn;


            }
            return objGroupMaster;
        }

        public string SaveGroupName(SqlTransaction ts)
        {
            General obj = new General();
            string returnString = "";
            SqlParameter[] param = new SqlParameter[7];
            SqlCommand cmd = new SqlCommand("sp_Group");
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;
                param[1] = new SqlParameter("@ipkGroupMID", SqlDbType.Decimal);
                param[1].Value = ipkGroupMID;
                param[2] = new SqlParameter("@ifkDeviceID", SqlDbType.Decimal);
                param[2].Value = ifkDeviceID;
                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Decimal);
                param[3].Value = ifkUserID;
                param[4] = new SqlParameter("@vpkGroupName", SqlDbType.VarChar);
                param[4].Value = base.RemoveSpecialCharacters(vpkGroupName);
                param[5] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[5].Value = bStatus;
                param[6] = new SqlParameter("@Error", SqlDbType.Int);
                param[6].Direction = ParameterDirection.Output;

                cmd.Transaction = ts;

                cmd.Connection = ts.Connection;
                cmd.Parameters.Add(param[0]);
                cmd.Parameters.Add(param[1]);
                cmd.Parameters.Add(param[2]);
                cmd.Parameters.Add(param[3]);
                cmd.Parameters.Add(param[4]);
                cmd.Parameters.Add(param[5]);
                cmd.Parameters.Add(param[6]);

                cmd.Transaction = ts;
                cmd.Connection = ts.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                returnString = param[6].Value.ToString();
                if (returnString == "-1")
                {
                    returnString = "-1";
                }
            }
            catch (Exception)
            {
                returnString = "-1";
            }
            return returnString;
        }

        public String SaveSelectedTracker(SqlTransaction ts)
        {
            SqlParameter[] param = new SqlParameter[4];
            SqlCommand cmd = new SqlCommand("sp_Group");
            string returnValue = "";
            try
            {

                param[0] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[0].Value = ifkGroupMID;
                param[1] = new SqlParameter("@Error", SqlDbType.Int);
                param[1].Direction = ParameterDirection.Output;
                param[2] = new SqlParameter("@Operation", SqlDbType.Int);
                param[2].Value = 11;
                param[3] = new SqlParameter("@vTrackerTypeName", SqlDbType.VarChar);
                param[3].Value = base.RemoveSpecialCharacters(vTrackerTypeName);


                cmd.Transaction = ts;
                cmd.Connection = ts.Connection;
                cmd.Parameters.Add(param[0]);
                cmd.Parameters.Add(param[1]);
                cmd.Parameters.Add(param[2]);
                cmd.Parameters.Add(param[3]);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                returnValue = param[1].Value.ToString();

            }
            catch (Exception)
            {
                returnValue = "-1";
            }
            return returnValue;
        }

        public void SaveAvilableTrackers(SqlTransaction ts)
        {

            SqlParameter[] param = new SqlParameter[3];
            SqlCommand cmd = new SqlCommand("sp_Group");
            string returnValue = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 12;

                param[1] = new SqlParameter("@ipkTrackerTypeID", SqlDbType.Int);
                param[1].Value = ipkTrackerTypeID;


                param[2] = new SqlParameter("@vTrackerTypeName", SqlDbType.VarChar);
                param[2].Value = base.RemoveSpecialCharacters(vTrackerTypeName);


                cmd.Transaction = ts;
                cmd.Connection = ts.Connection;
                cmd.Parameters.Add(param[0]);
                cmd.Parameters.Add(param[1]);
                cmd.Parameters.Add(param[2]);


                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                returnValue = param[1].Value.ToString();

            }
            catch (Exception)
            {

            }

        }

        public void UpDateTable()
        {
            try
            {
                string qry = "update dbo.tblTracker_Type set bStatus=0";
               SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.Text, qry);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "UpDateTable()", ex.Message  + ex.StackTrace);
            }
        }

        public List<clsGroup> GetAllGroupforDeviceMaster()
        {
            DataSet ds = new DataSet();
            List<clsGroup> obj = new List<clsGroup>();
            try
            {
                ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, "select * from tblGroup_Master");
                obj.Add(new clsGroup("Select", -1));
                obj.Add(new clsGroup("Self", 0));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    obj.Add(new clsGroup(row["vpkGroupName"].ToString(), Convert.ToInt32(row["ipkGroupMID"].ToString())));
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "GetAllGroup()", ex.Message  + ex.StackTrace);
            }
            return obj;
        }

        public string SaveGroup()
        {

            string returnstring = "";
            SqlParameter[] param = new SqlParameter[8];
            try
            {

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkGroupMID", SqlDbType.Int);
                param[1].Value = ipkGroupMID;

                param[2] = new SqlParameter("@vpkGroupName", SqlDbType.VarChar);
                param[2].Value = vpkGroupName;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter("@error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[5].Value = vIDs;

                param[6] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[6].Value = bStatus;

                param[7] = new SqlParameter("@iParentGMID", SqlDbType.Int);
                param[7].Value = iParentGMID;



               SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_Group", param);

                if (param[4].Value.ToString() == "1")
                {
                    returnstring = "Group Saved successful";
                }
                else if (param[4].Value.ToString() == "-1")
                {
                    returnstring = "Group Already Exists!";
                }
                else if (param[4].Value.ToString() == "2")
                {
                    returnstring = "Group Status Changed successful";
                }
                else if (param[4].Value.ToString() == "7")
                {
                    returnstring = "Group Deleted Successfuly";
                }
                else if (param[4].Value.ToString() == "5")
                {
                    returnstring = "Group Updated successful";
                }
                else if (param[4].Value.ToString() == "8")
                {
                    returnstring = "Group Delete successful";
                }
                else if (param[4].Value.ToString() == "-1")
                {
                    returnstring = "Group Update not allow";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "SaveGroup", ex.Message  + ex.StackTrace);
                returnstring = "Error in Saving group!" + ex.Message;
            }
            return returnstring;

        }

        public clsGroup GetGroup()
        {
            DataSet ds = new DataSet();
            clsGroup obj = new clsGroup();
            string Query = "select * from tblGroup_master where ipkGroupMID=" + ipkGroupMID;
            try
            {
                ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, Query);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    obj = new clsGroup(Convert.ToInt32(ds.Tables[0].Rows[0]["ipkGroupMID"].ToString()), ds.Tables[0].Rows[0]["vpkGroupName"].ToString(), Convert.ToBoolean(ds.Tables[0].Rows[0]["bStatus"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[0]["iParentGMID"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[0]["ifkUserID"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[0]["ifkCompanyID"].ToString()));
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "GetGroup()", ex.Message  + ex.StackTrace);
            }
            return obj;
        }

        public string SaveCompnyGroup()
        {

            string returnstring = "";
            SqlParameter[] param = new SqlParameter[9];
            try
            {

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkGroupMID", SqlDbType.Int);
                param[1].Value = ipkGroupMID;

                param[2] = new SqlParameter("@vpkGroupName", SqlDbType.VarChar);
                param[2].Value = vpkGroupName;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter("@error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[5].Value = vIDs;

                param[6] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[6].Value = bStatus;

                param[7] = new SqlParameter("@iParentGMID", SqlDbType.Int);
                param[7].Value = iParentGMID;

                param[8] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[8].Value = ifkCompanyID;

               SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_Company_Group", param);

                if (param[4].Value.ToString() == "1")
                {
                    returnstring = "Group Saved successful";
                }
                else if (param[4].Value.ToString() == "-1")
                {
                    returnstring = "Group Already Exists!";
                }
                else if (param[4].Value.ToString() == "2")
                {
                    returnstring = "Group Status Changed successful";
                }
                else if (param[4].Value.ToString() == "7")
                {
                    returnstring = "Group Deleted Successfuly";
                }
                else if (param[4].Value.ToString() == "5")
                {
                    returnstring = "Group Updated successful";
                }
                else if (param[4].Value.ToString() == "8")
                {
                    returnstring = "Group Delete successful";
                }
                else if (param[4].Value.ToString() == "-1")
                {
                    returnstring = "Group Update not allow";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "sp_Company_Group", ex.Message  + ex.StackTrace);
                returnstring = "Error in Saving group!" + ex.Message;
            }
            return returnstring;

        }


        public string SaveFrontGroup(int userid)
        {

            string returnstring = "";
            SqlParameter[] param = new SqlParameter[9];
            try
            {

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkGroupMID", SqlDbType.Int);
                param[1].Value = ipkGroupMID;

                param[2] = new SqlParameter("@vpkGroupName", SqlDbType.VarChar);
                param[2].Value = vpkGroupName;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = userid;

                param[4] = new SqlParameter("@error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[5].Value = vIDs;

                param[6] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[6].Value = bStatus;

                param[7] = new SqlParameter("@iParentGMID", SqlDbType.Int);
                param[7].Value = iParentGMID;


                param[8] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[8].Value = ifkCompanyID;



               SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_Group", param);

                if (param[4].Value.ToString() == "1")
                {
                    returnstring = "Group Saved successful";
                }
                else if (param[4].Value.ToString() == "-1")
                {
                    returnstring = "Group Already Exists!";
                }
                else if (param[4].Value.ToString() == "2")
                {
                    returnstring = "Group Status Changed successful";
                }
                else if (param[4].Value.ToString() == "7")
                {
                    returnstring = "Group Deleted Successfuly";
                }
                else if (param[4].Value.ToString() == "5")
                {
                    returnstring = "Group Updated successful";
                }
                else if (param[4].Value.ToString() == "8")
                {
                    returnstring = "Group Delete successful";
                }
                else if (param[4].Value.ToString() == "-1")
                {
                    returnstring = "Group Update not allow";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "SaveGroup", ex.Message  + ex.StackTrace);
                returnstring = "Error in Saving group!" + ex.Message;
            }
            return returnstring;

        }


        public List<clsGroup> GetAllGroupforAlert()
        {
            DataSet ds = new DataSet();
            List<clsGroup> obj = new List<clsGroup>();
            try
            {
                if (ifkCompanyID != -1)
                {
                    ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, "select * from tblGroup_Master WHERE ifkCompanyID =" + ifkCompanyID + " AND ifkUserID = " + ifkUserID + " ");
                }
                else
                {
                    ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, "select * from tblGroup_Master where ifkUserID = " + ifkUserID + " ");
                }

                obj.Add(new clsGroup("Select Groups", -1));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    clsGroup objclsGroup = new clsGroup();
                    objclsGroup.ifkGroupMID = Convert.ToInt32(row["ipkGroupMID"].ToString());
                    int count = objclsGroup.CountDeviceInGroup();

                    obj.Add(new clsGroup((row["vpkGroupName"].ToString() + " (" + count + ")"), Convert.ToInt32(row["ipkGroupMID"].ToString())));
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "GetAllGroup()", ex.Message  + ex.StackTrace);
            }
            return obj;
        }

        public List<clsGroup> GetTrackerTypesDevice()
        {
            DataSet ds = new DataSet();
            List<clsGroup> obj2 = new List<clsGroup>();

            try
            {
                if (ifkGroupMID == 0 || ifkGroupMID == -1)
                {
                    ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, "select ipkTrackerTypeID,vDeviceName,vpkDeviceID,vTrackerTypeName from tblTracker_Type inner join vwfrontDevice on ipkTrackerTypeID = iTrackerType inner join tblGroup_Master on ipkGroupMID = ifkGroupMID where ipkGroupMID in (SELECT ipkGroupMID FROM tblGroup_Master WHERE ifkUserID = " + ifkUserID + " )");
                }
                else
                {
                    ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, "select ipkTrackerTypeID,vDeviceName,vpkDeviceID,vTrackerTypeName from tblTracker_Type inner join vwfrontDevice on ipkTrackerTypeID = iTrackerType inner join tblGroup_Master on ipkGroupMID = ifkGroupMID where ipkGroupMID  = " + ifkGroupID + "");
                }

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    obj2.Add(new clsGroup(-1, "Select Assets"));

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj2.Add(new clsGroup(Convert.ToInt32(row["ipkTrackerTypeID"].ToString()), row["vDeviceName"].ToString() + "(" + row["vTrackerTypeName"].ToString() + ")"));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "GetTrackerTypesDevice()", ex.Message  + ex.StackTrace);

            }
            return obj2;
        }

        public List<clsGroup> GetAssetTracker()
        {
            DataSet ds = new DataSet();
            List<clsGroup> obj2 = new List<clsGroup>();
            string qry = "";

            try
            {
                if (ifkGroupID == 0 || ifkGroupID == -1)
                {
                    // qry = "select ipkTrackerTypeID,vDeviceName,vpkDeviceID,vTrackerTypeName from tblTracker_Type inner join vwfrontDevice on ipkTrackerTypeID = iTrackerType inner join tblGroup_Master on ipkGroupMID = ifkGroupMID where ipkGroupMID in (SELECT ipkGroupMID FROM tblGroup_Master WHERE ifkUserID = " + ifkUserID + " )";

                    qry = "select DISTINCT vDeviceName,vpkDeviceID,vTrackerTypeName from vwfrontDevice WHERE ifkGroupMID in (SELECT ipkGroupMID FROM tblGroup_Master WHERE ifkUserID = " + ifkUserID + ")";

                    ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, qry);
                }
                else
                {
                    qry = "select vDeviceName,vpkDeviceID,vTrackerTypeName from vwfrontDevice WHERE ifkGroupMID  = " + ifkGroupID + "";

                    ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, qry);
                }

                obj2.Add(new clsGroup("-1", "Select Assets"));

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj2.Add(new clsGroup(row["vpkDeviceID"].ToString(), row["vDeviceName"].ToString() + "(" + row["vTrackerTypeName"].ToString() + ")"));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "GetTrackerTypesDevice()", ex.Message  + ex.StackTrace);

            }
            return obj2;
        }

        public DataSet AlertPresenterView()
        {

            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[7];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkGroupID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;

                param[2] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[2].Value = ifkUserID;

                param[3] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[3].Value = ifkCompanyID;

                param[4] = new SqlParameter("@top", SqlDbType.Int);
                param[4].Value = top;

                param[5] = new SqlParameter("@ifkReportID", SqlDbType.Int);
                param[5].Value = ifkReportID;

                param[6] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[6].Value = vpkDeviceID;


                ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "AlertPresentView_sp", param);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "Alert()->to get Alert data", ex.Message  + ex.StackTrace);

            }
            return ds;

        }

        public DataSet AlertHistory()
        {

            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[7];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkGroupID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;

                param[2] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[2].Value = ifkUserID;

                param[3] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[3].Value = ifkCompanyID;

                param[4] = new SqlParameter("@like", SqlDbType.VarChar);
                param[4].Value = like;

                param[5] = new SqlParameter("@ifkReportID", SqlDbType.Int);
                param[5].Value = ifkReportID;

                param[6] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[6].Value = vpkDeviceID;



                ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "AlertHistory_sp", param);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "AlertHistory()->to get Alert data to history", ex.Message  + ex.StackTrace);

            }
            return ds;

        }

        public int CountDeviceInGroup()
        {
            int result = 0;

            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 4;

                param[1] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;

                result = Convert.ToInt32(SqlHelper.ExecuteScalar(f_strConnectionString, CommandType.StoredProcedure, "NewFrontData_sp", param));
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsGroup.cs", "CountDeviceInGroup()", ex.Message  + ex.StackTrace);
            }
            return result;
        }

    }
}