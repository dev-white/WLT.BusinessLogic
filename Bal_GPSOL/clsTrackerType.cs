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
    public class clsTrackerType
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        private int _Operation;
        private int _ipkTrackerTypeID;
        private string _vTrackerTypeName;
        private string _vImage;
        private int _Error;
        private bool _bStatus;
        private DateTime _dEnterDate = DateTime.Now;
        private DateTime _dUpdateDate = DateTime.Now;
        private string _vIDs;
        private string _vMovingImage;

        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkTrackerTypeID { get { return _ipkTrackerTypeID; } set { _ipkTrackerTypeID = value; } }
        public int Error { get { return _Error; } set { _Error = value; } }
        public string vTrackerTypeName { get { return _vTrackerTypeName; } set { _vTrackerTypeName = value; } }
        public string vImage { get { return _vImage; } set { _vImage = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public DateTime dEnterDate { get { return _dEnterDate; } set { _dEnterDate = value; } }
        public DateTime dUpdateDate { get { return _dUpdateDate; } set { _dUpdateDate = value; } }
        public string vIDs { get { return _vIDs; } set { _vIDs = value; } }
        public string vMovingImage { get { return _vMovingImage; } set { _vMovingImage = value; } }

        public clsTrackerType()
        {
            // constructor
        }
        public clsTrackerType(string vTrackerTypeName, int ipkTrackerTypeID)
        {
            this.vTrackerTypeName = vTrackerTypeName;
            this.ipkTrackerTypeID = ipkTrackerTypeID;
        }

        public clsTrackerType(int Error)
        {
            this.Error = Error;
        }
        public clsTrackerType(int ipkTrackerTypeID, string vTrackerTypeName, string vImage, bool bStatus)
        {
            this.ipkTrackerTypeID = ipkTrackerTypeID;
            this.vTrackerTypeName = vTrackerTypeName;
            this.vImage = vImage;
            this.bStatus = bStatus;
        }

        public string SaveTrackerType()
        {
            SqlParameter[] param = new SqlParameter[9];
            string returnstring = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkTrackerTypeID", SqlDbType.Int);
                param[1].Value = ipkTrackerTypeID;

                param[2] = new SqlParameter("@vTrackerTypeName", SqlDbType.VarChar);
                param[2].Value = vTrackerTypeName;

                param[3] = new SqlParameter("@vImage", SqlDbType.VarChar);
                param[3].Value = vImage;

                param[4] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[4].Value = bStatus;

                param[5] = new SqlParameter("@Error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@dEnterDate", SqlDbType.VarChar);
                param[6].Value = dEnterDate;

                param[7] = new SqlParameter("@dUpdateDate", SqlDbType.VarChar);
                param[7].Value = dUpdateDate;

                param[8] = new SqlParameter("@vIDs", SqlDbType.VarChar);
                param[8].Value = vIDs;

                //param[9] = new SqlParameter("@vMovingImage", SqlDbType.VarChar);
                //param[9].Value = vMovingImage;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_TrackerType", param);

                if (param[5].Value.ToString() == "-1")
                {
                    returnstring = "Tracker type already exist!";
                }
                else if (param[5].Value.ToString() == "1")
                {
                    returnstring = "Save successful!";
                }
                else if (param[5].Value.ToString() == "2")
                {
                    returnstring = "Update successful!";
                }
                else if (param[5].Value.ToString() == "3")
                {
                    returnstring = "Delete successful!";
                }
                else if (param[5].Value.ToString() == "6")
                {
                    returnstring = "Status change successful!";
                }
                else
                {
                    returnstring = "Internal execution error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsTrackerType.cs", "SaveTrackerType()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error!";

            }
            return returnstring;

        }

        public DataSet GetTrackerType()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkTrackerTypeID", SqlDbType.Int);
                param[1].Value = ipkTrackerTypeID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_TrackerType", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsTrackerType.cs", "GetTrackerType()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public List<clsTrackerType> GetTrackerTypesTrue()
        {
            DataSet ds = new DataSet();
            List<clsTrackerType> obj = new List<clsTrackerType>();
            clsTrackerType objTracker;
            try
            {
                ds = SqlHelper.ExecuteDataset(f_strConnectionString.ToString(), CommandType.Text, "select ipkTrackerTypeID,vTrackerTypeName from tblTracker_Type where bStatus=1");
                obj.Add(new clsTrackerType("Select", -1));
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj.Add(new clsTrackerType(row["vTrackerTypeName"].ToString(), Convert.ToInt32(row["ipkTrackerTypeID"].ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsTrackerType.cs", "GetTrackerTypesTrue()", ex.Message  + ex.StackTrace);
                objTracker = new clsTrackerType(1);
                obj.Add(objTracker);
            }
            return obj;
        }


        public List<clsTrackerType> GetTrackerTypesAlert()
        {
            DataSet ds = new DataSet();
            List<clsTrackerType> obj = new List<clsTrackerType>();
            clsTrackerType objTracker;
            try
            {
                ds = SqlHelper.ExecuteDataset(f_strConnectionString.ToString(), CommandType.Text, "select * from tblTracker_Type ");
                obj.Add(new clsTrackerType("All Asset", -1));
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj.Add(new clsTrackerType(row["vTrackerTypeName"].ToString(), Convert.ToInt32(row["ipkTrackerTypeID"].ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsTrackerType.cs", "GetTrackerTypesAlert()", ex.Message  + ex.StackTrace);
                objTracker = new clsTrackerType(1);
                obj.Add(objTracker);
            }
            return obj;
        }
        
    }

}