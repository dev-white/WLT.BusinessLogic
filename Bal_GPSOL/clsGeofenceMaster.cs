using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer;
using WLT.ErrorLog;
using Microsoft.AspNetCore.Http;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsGeofenceMaster : General
    {
        //SqlConnection con = new SqlConnection(System.Configuration.conString.ToString());
        String conString = AppConfiguration.Getwlt_WebAppConnectionString();

        #region Variables
        private int _ZoneTypeId;
        private int _iGeoZoneTypeId;
        private int _Operation;
        private int _ipkGeoMID;
        private int _ifkDeviceID;
        private int _ifkUserID;
        private int _ifkGroupMID;
        private int _ifkCompanyId;
        //This will  be used as sort parameter for client user since cmbComapanyAlert is not displayed
        private int _ipkComapnyId;
        private string _vGeoName;
        private string _vDescription;
        private DateTime _dCreateDate = DateTime.Now;
        private DateTime _dLastAccessed = DateTime.Now;
        private bool _bStatus;
        private int _Error = 0;
        private int _ifkRouteMID;
        private string _vIDs;
        private string _ErrorText = "";

        private int _ipkGeoDID;
        private int _ifkGeoMID;
        private string _vLatitude;
        private string _vLongitude;
        private string _vpkUserName;
        private string _vpkDeviceID;
        private List<clsGeofenceMaster> _Points;
        private bool _bUpdate;

        private string _Trackerid = "";
        private string _Trdatetime = "";
        private string _speed = "";
        private decimal _Radius;
        private int _iTrackerType;
        private string _vImage;
        private int _iType;
        private string _cIN_OUT;
        private string _StartDate;
        private string _EndDate;
        private int _Zoomlevel;

        private int _ifkUserTypeID;

        private string _iMaxSpeed;
        private string _iProximity;
        private string _iAreaSqMeters;
        private string _GeoRadius;
        private bool _IsGeoCircle;

        public int Zoomlevel { get { return _Zoomlevel; } set { _Zoomlevel = value; } }
        public string iMaxSpeed { get { return _iMaxSpeed; } set { _iMaxSpeed = value; } }
        public string iProximity { get { return _iProximity; } set { _iProximity = value; } }
        public string iAreaSqMeters { get { return _iAreaSqMeters; } set { _iAreaSqMeters = value; } }
        public string GeoRadius { get { return _GeoRadius; } set { _GeoRadius = value; } }
        public bool IsGeoCircle { get { return _IsGeoCircle; } set { _IsGeoCircle = value; } }

        public int BufferSize { get; set; }

        public int ZoneTypeId
        {
            get { return _ZoneTypeId; }
            set { _ZoneTypeId = value; }
        }
        public int iGeoZoneTypeId { get { return _iGeoZoneTypeId; } set { _iGeoZoneTypeId = value; } }
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkGeoMID { get { return _ipkGeoMID; } set { _ipkGeoMID = value; } }
        public int ifkDeviceID { get { return _ifkDeviceID; } set { _ifkDeviceID = value; } }
        public int ifkUserID { get { return _ifkUserID; } set { _ifkUserID = value; } }
        public int ifkGroupMID { get { return _ifkGroupMID; } set { _ifkGroupMID = value; } }
        public int ifkCompanyId { get { return _ifkCompanyId; } set { _ifkCompanyId = value; } }
        public int IpkCompanyId { get { return _ipkComapnyId; } set { _ipkComapnyId = value; } }
        public string vGeoName { get { return _vGeoName; } set { _vGeoName = value; } }
        public string vDescription { get { return _vDescription; } set { _vDescription = value; } }
        public DateTime dCreateDate { get { return _dCreateDate; } set { _dCreateDate = value; } }
        public DateTime dLastAccessed { get { return _dLastAccessed; } set { _dLastAccessed = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public int Error { get { return _Error; } set { _Error = value; } }
        public int ifkRouteMID { get { return _ifkRouteMID; } set { _ifkRouteMID = value; } }
        public string vIDs { get { return _vIDs; } set { _vIDs = value; } }
        //currently sessions userTypeId
        public int IfkUserTypeID { get { return _ifkUserTypeID; } set { _ifkUserTypeID = value; } }

        public int ipkGeoDID { get { return _ipkGeoDID; } set { _ipkGeoDID = value; } }
        public int ifkGeoMID { get { return _ifkGeoMID; } set { _ifkGeoMID = value; } }
        public string vLatitude { get { return _vLatitude; } set { _vLatitude = value; } }
        public string vLongitude { get { return _vLongitude; } set { _vLongitude = value; } }
        public string vpkUserName { get { return _vpkUserName; } set { _vpkUserName = value; } }
        public string vpkDeviceID { get { return _vpkDeviceID; } set { _vpkDeviceID = value; } }
        public List<clsGeofenceMaster> Points { get { return _Points; } set { _Points = value; } }
        public bool bUpdate { get { return _bUpdate; } set { _bUpdate = value; } }

        public string Trackerid { get { return _Trackerid; } set { _Trackerid = value; } }
        public string Trdatetime { get { return _Trdatetime; } set { _Trdatetime = value; } }
        public string Speed { get { return _speed; } set { _speed = value; } }
        public decimal Radius { get { return _Radius; } set { _Radius = value; } }
        public int iTrackerType { get { return _iTrackerType; } set { _iTrackerType = value; } }
        public string vImage { get { return _vImage; } set { _vImage = value; } }
        public int iType { get { return _iType; } set { _iType = value; } }
        public string ErrorText { get { return _ErrorText; } set { _ErrorText = value; } }
        public string cIN_OUT { get { return _cIN_OUT; } set { _cIN_OUT = value; } }
        public string StartDate { get { return _StartDate; } set { _StartDate = value; } }
        public string EndDate { get { return _EndDate; } set { _EndDate = value; } }
        public string ZoneContact { get; set; }
        public string ZoneContactTel { get; set; }

        public string[] SelectedDevices { get; set; }
        public bool isSupported { get; set; }
        public string view { get; set; }

        #endregion

        public clsGeofenceMaster()
        {
            //Need a different implementation

            //var objRegistration = new HttpContext.Session.GetObject<clsRegistration>("clsRegistration");

            //this.IfkUserTypeID = objRegistration.ifkUserTypeID;
        }

        public clsGeofenceMaster(int ifkGeoMID, int ifkCompanyId, int ifkGroupMID, int iGeoZoneTypeId, string vGeoName)
        {
            this.ifkGeoMID = ifkGeoMID;
            this.ifkCompanyId = ifkCompanyId;
            this.ifkGroupMID = ifkGroupMID;
            this.iGeoZoneTypeId = iGeoZoneTypeId;
            this.vGeoName = vGeoName;

        }

        public clsGeofenceMaster(int ipkGeoMID, int ifkDeviceID, int ifkUserID, string vGeoName, string vDescription, bool bStatus, List<clsGeofenceMaster> Points, string cIN_OUT, int ZoneTypeId)
        {
            this.ipkGeoMID = ipkGeoMID;
            this.ifkDeviceID = ifkDeviceID;
            this.ifkUserID = ifkUserID;
            this.vGeoName = vGeoName;
            this.vDescription = vDescription;
            this.bStatus = bStatus;
            this.Points = Points;
            this.cIN_OUT = cIN_OUT;
            this.ZoneTypeId = ZoneTypeId;
        }

        public string SaveGeofence(SqlTransaction ts)
        {
            General obj = new General();
            string returnString = "";

            using (SqlConnection con = new SqlConnection(conString))
            {

                SqlParameter[] param = new SqlParameter[22];
                SqlCommand cmd = new SqlCommand("sp_Geofence");
                try
                {
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;
                    param[1] = new SqlParameter("@ipkGeoMID", SqlDbType.Decimal);
                    param[1].Value = ipkGeoMID;
                    param[2] = new SqlParameter("@ifkDeviceID", SqlDbType.Decimal);
                    param[2].Value = ifkDeviceID;
                    param[3] = new SqlParameter("@ifkUserID", SqlDbType.Decimal);
                    param[3].Value = ifkUserID;
                    param[4] = new SqlParameter("@vGeoName", SqlDbType.VarChar);
                    param[4].Value = vGeoName; // base.RemoveSpecialCharacters(vGeoName);
                    param[5] = new SqlParameter("@dCreateDate", SqlDbType.DateTime);
                    param[5].Value = dCreateDate;
                    param[6] = new SqlParameter("@dLastAccessed", SqlDbType.DateTime);
                    param[6].Value = dLastAccessed;
                    param[7] = new SqlParameter("@bStatus", SqlDbType.Bit);
                    param[7].Value = bStatus;

                    param[8] = new SqlParameter("@Error", SqlDbType.Int);
                    param[8].Direction = ParameterDirection.Output;
                    param[9] = new SqlParameter("@vDescription", SqlDbType.VarChar);
                    param[9].Value = vDescription;
                    param[10] = new SqlParameter("@cIN_OUT", SqlDbType.VarChar);
                    param[10].Value = cIN_OUT;
                    param[11] = new SqlParameter("@ZoneTypeId", SqlDbType.Int);
                    param[11].Value = ZoneTypeId;
                    param[12] = new SqlParameter("@ifkCompanyId", SqlDbType.Int);
                    param[12].Value = ifkCompanyId;
                    param[13] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                    param[13].Value = ifkGroupMID;
                    param[14] = new SqlParameter("@vZoomleval", SqlDbType.Int);
                    param[14].Value = Zoomlevel;

                    param[15] = new SqlParameter("@iMaxSpeed", SqlDbType.Int);
                    param[15].Value = iMaxSpeed;
                    param[16] = new SqlParameter("@iProximity", SqlDbType.Decimal);
                    param[16].Value = iProximity;
                    param[17] = new SqlParameter("@iAreaSqMeters", SqlDbType.Decimal);
                    param[17].Value = iAreaSqMeters;

                    param[18] = new SqlParameter("@IsGeoCircle", SqlDbType.Bit);
                    param[18].Value = IsGeoCircle;

                    param[19] = new SqlParameter("@GeoRadius", SqlDbType.Decimal);
                    param[19].Value = GeoRadius;

                    param[20] = new SqlParameter("@ZoneContact", SqlDbType.VarChar);
                    param[20].Value = ZoneContact;

                    param[21] = new SqlParameter("@ZoneContactTel", SqlDbType.VarChar);
                    param[21].Value = ZoneContactTel;

                    cmd.Transaction = ts;

                    cmd.Connection = ts.Connection;
                    cmd.Parameters.Add(param[0]);
                    cmd.Parameters.Add(param[1]);
                    cmd.Parameters.Add(param[2]);
                    cmd.Parameters.Add(param[3]);
                    cmd.Parameters.Add(param[4]);
                    cmd.Parameters.Add(param[5]);
                    cmd.Parameters.Add(param[6]);
                    cmd.Parameters.Add(param[7]);
                    cmd.Parameters.Add(param[8]);
                    cmd.Parameters.Add(param[9]);
                    cmd.Parameters.Add(param[10]);
                    cmd.Parameters.Add(param[11]);
                    cmd.Parameters.Add(param[12]);
                    cmd.Parameters.Add(param[13]);
                    cmd.Parameters.Add(param[14]);
                    cmd.Parameters.Add(param[15]);
                    cmd.Parameters.Add(param[16]);
                    cmd.Parameters.Add(param[17]);
                    cmd.Parameters.Add(param[18]);
                    cmd.Parameters.Add(param[19]);
                    cmd.Parameters.Add(param[20]);
                    cmd.Parameters.Add(param[21]);

                    cmd.Transaction = ts;
                    cmd.Connection = ts.Connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    returnString = param[8].Value.ToString();
                    if (returnString == "-1")
                    {
                        returnString = "-1";
                    }
                }
                catch (Exception)
                {
                    returnString = "-1";
                }
            }
            return returnString;

        }

        public String SaveGeofenceDetail(SqlTransaction ts)
        {
            string returnValue = "";

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlParameter[] param = new SqlParameter[9];
                SqlCommand cmd = new SqlCommand("sp_Geofence");

                try
                {

                    param[0] = new SqlParameter("@ipkGeoDID", SqlDbType.Int);
                    param[0].Value = ipkGeoDID;
                    param[1] = new SqlParameter("@ifkGeoMID", SqlDbType.Int);
                    param[1].Value = ifkGeoMID;
                    param[2] = new SqlParameter("@vLatitude", SqlDbType.VarChar);
                    param[2].Value = vLatitude;
                    param[3] = new SqlParameter("@vLongitude", SqlDbType.VarChar);
                    param[3].Value = vLongitude;
                    param[4] = new SqlParameter("@Error", SqlDbType.Int);
                    param[4].Direction = ParameterDirection.Output;
                    param[5] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[5].Value = 5;
                    param[6] = new SqlParameter("@bUpdate", SqlDbType.Int);
                    param[6].Value = bUpdate;
                    param[7] = new SqlParameter("@Radius", SqlDbType.VarChar);
                    param[7].Value = Radius;
                    param[8] = new SqlParameter("@iGeoZoneTypeId", SqlDbType.VarChar);
                    param[8].Value = iGeoZoneTypeId;


                    cmd.Transaction = ts;
                    cmd.Connection = ts.Connection;
                    cmd.Parameters.Add(param[0]);
                    cmd.Parameters.Add(param[1]);
                    cmd.Parameters.Add(param[2]);
                    cmd.Parameters.Add(param[3]);
                    cmd.Parameters.Add(param[4]);
                    cmd.Parameters.Add(param[5]);
                    cmd.Parameters.Add(param[6]);
                    cmd.Parameters.Add(param[7]);
                    cmd.Parameters.Add(param[8]);


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    returnValue = param[4].Value.ToString();

                }
                catch (Exception)
                {
                    returnValue = "-1";
                }
            }
            return returnValue;
        }

        public string SaveGeofenceforZones(SqlTransaction ts)
        {
            General obj = new General();
            string returnString = "";
            using (SqlConnection con = new SqlConnection(conString))
            {

                SqlParameter[] param = new SqlParameter[12];
                SqlCommand cmd = new SqlCommand("sp_ZonesGeofence");
                try
                {
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;
                    param[1] = new SqlParameter("@ipkZonesGeoMID", SqlDbType.Decimal);
                    param[1].Value = ipkGeoMID;
                    param[3] = new SqlParameter("@ifkUserID", SqlDbType.Decimal);
                    param[3].Value = ifkUserID;
                    param[4] = new SqlParameter("@vZonesGeoName", SqlDbType.VarChar);
                    param[4].Value = base.RemoveSpecialCharacters(vGeoName);
                    param[5] = new SqlParameter("@dZonesCreateDate", SqlDbType.DateTime);
                    param[5].Value = dCreateDate;
                    param[6] = new SqlParameter("@dZonesLastAccessed", SqlDbType.DateTime);
                    param[6].Value = dLastAccessed;
                    param[7] = new SqlParameter("@bZonesStatus", SqlDbType.Bit);
                    param[7].Value = bStatus;
                    param[8] = new SqlParameter("@Error", SqlDbType.Int);
                    param[8].Direction = ParameterDirection.Output;

                    param[9] = new SqlParameter("@ZoneTypeId", SqlDbType.Int);
                    param[9].Value = ZoneTypeId;
                    param[10] = new SqlParameter("@ifkCompanyId", SqlDbType.Int);
                    param[10].Value = ifkCompanyId;
                    param[11] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                    param[11].Value = ifkGroupMID;


                    cmd.Transaction = ts;

                    cmd.Connection = ts.Connection;
                    cmd.Parameters.Add(param[0]);
                    cmd.Parameters.Add(param[1]);
                    cmd.Parameters.Add(param[2]);
                    cmd.Parameters.Add(param[3]);
                    cmd.Parameters.Add(param[4]);
                    cmd.Parameters.Add(param[5]);
                    cmd.Parameters.Add(param[6]);
                    cmd.Parameters.Add(param[7]);
                    cmd.Parameters.Add(param[8]);
                    cmd.Parameters.Add(param[9]);
                    cmd.Parameters.Add(param[10]);
                    cmd.Parameters.Add(param[11]);
                    //cmd.Parameters.Add(param[12]);

                    cmd.Transaction = ts;
                    cmd.Connection = ts.Connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    returnString = param[8].Value.ToString();
                    if (returnString == "-1")
                    {
                        returnString = "-1";
                    }
                }
                catch (Exception)
                {
                    returnString = "-1";
                }
            }
            return returnString;
        }
        public DataSet GetGeoList()
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(conString))
            {

                SqlParameter[] param = new SqlParameter[4];
                try
                {
                    //  select *From Newtbl_ZoneGroupMaster Where iZonesParentGMID=@iZonesParentGMID AND iZonesParentID= @iZonesParentID
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;

                    param[1] = new SqlParameter("@ifkCompanyId", SqlDbType.Int);
                    param[1].Value = ifkCompanyId;

                    param[2] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                    param[2].Value = ifkGroupMID;

                    param[3] = new SqlParameter("@ZoneTypeId", SqlDbType.Int);
                    param[3].Value = ZoneTypeId;


                    ds = SqlHelper.ExecuteDataset(conString, CommandType.StoredProcedure, "sp_Geofence", param);

                }

                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile("clsGeofenceMaster.cs", "GetGeoList()", ex.Message  + ex.StackTrace);

                }
            }
            return ds;

        }


        public clsGeofenceMaster(string vGeoName, int ipkGeoMID)
        {
            this.ipkGeoMID = ipkGeoMID;
            this.vGeoName = vGeoName;
        }
        public clsGeofenceMaster(int ipkGeoMID, string ErrorText)
        {
            this.ipkGeoMID = ipkGeoMID;
            this.ErrorText = ErrorText;
        }

        //public clsGeofenceMaster()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}


        public string DeleteGeoFence()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                //DataSet ds = new DataSet();
                try
                {
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;

                    param[1] = new SqlParameter("@ipkGeoMID", SqlDbType.Int);
                    param[1].Value = ipkGeoMID;

                    param[2] = new SqlParameter("@Error", SqlDbType.Int);
                    param[2].Direction = ParameterDirection.Output;

                    SqlHelper.ExecuteNonQuery(conString.ToString(), CommandType.StoredProcedure, "sp_Geofence", param);
                    return param[2].Value.ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public DataSet EditGeoFence()
        {
            DataSet ds = new DataSet();
            //clsGeofenceMaster objGeofenceMaster = new clsGeofenceMaster();
            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = 7;

                    param[1] = new SqlParameter("@ipkGeoMID", SqlDbType.Int);
                    param[1].Value = ipkGeoMID;

                    ds = SqlHelper.ExecuteDataset(con.ConnectionString.ToString(), CommandType.StoredProcedure, "sp_Geofence", param);


                }
                catch (Exception ex)
                {

                    LogError.RegisterErrorInLogFile("clsGeofenceMaster.cs", "EditGeoFence()", ex.Message  + ex.StackTrace);
                    throw;


                }
            }
            return ds;
        }

        public DataSet GetZeoZones()
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;

                    param[1] = new SqlParameter("@ifkCompanyId", SqlDbType.Int);
                    param[1].Value = ifkCompanyId;

                    ds = SqlHelper.ExecuteDataset(con.ConnectionString.ToString(), CommandType.StoredProcedure, "sp_Geofence", param);


                }
                catch (Exception ex)
                {

                    LogError.RegisterErrorInLogFile("clsGeofenceMaster.cs", "GetZeoZones()", ex.Message  + ex.StackTrace);
                    throw;


                }
            }
            return ds;
        }

        public DataSet GetEditGeofence()
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;

                    param[1] = new SqlParameter("@ifkGeoMID", SqlDbType.Int);
                    param[1].Value = ifkGeoMID;

                    ds = SqlHelper.ExecuteDataset(con.ConnectionString.ToString(), CommandType.StoredProcedure, "sp_Geofence", param);


                }
                catch (Exception ex)
                {

                    LogError.RegisterErrorInLogFile("clsGeofenceMaster.cs", "GetZeoZones()", ex.Message  + ex.StackTrace);
                    throw;


                }
            }
            return ds;
        }

        public DataSet GetZoneBuffer()
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    SqlParameter[] param = new SqlParameter[4];
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = 29;

                    param[1] = new SqlParameter("@ipkGeoMID", SqlDbType.Int);
                    param[1].Value = ipkGeoMID;

                    param[2] = new SqlParameter("@BufferSize", SqlDbType.Int);
                    param[2].Value = BufferSize;

                    param[3] = new SqlParameter("@iGeoZoneTypeId", SqlDbType.Int);
                    param[3].Value = iGeoZoneTypeId;

                    ds = SqlHelper.ExecuteDataset(con.ConnectionString.ToString(), CommandType.StoredProcedure, "sp_Geofence", param);

                }
                catch (Exception ex)
                {

                    LogError.RegisterErrorInLogFile("clsGeofenceMaster.cs", "GetZoneBuffer()", ex.Message  + ex.StackTrace);
                    throw;


                }
            }
            return ds;
        }

        public DataSet LoadGeofenceOnMap()
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    SqlParameter[] param = new SqlParameter[3];
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;

                    param[1] = new SqlParameter("@ZoneTypeId", SqlDbType.Int);
                    param[1].Value = ZoneTypeId;

                    param[2] = new SqlParameter("@ifkCompanyId", SqlDbType.Int);
                    param[2].Value = ifkCompanyId;

                    ds = SqlHelper.ExecuteDataset(con.ConnectionString.ToString(), CommandType.StoredProcedure, "sp_Geofence", param);


                }
                catch (Exception ex)
                {

                    LogError.RegisterErrorInLogFile("clsGeofenceMaster.cs", "GetZeoZones()", ex.Message  + ex.StackTrace);
                    throw;


                }
            }
            return ds;
        }

        public DataSet ShowGeofence()
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;

                    param[1] = new SqlParameter("@ZoneTypeId", SqlDbType.Int);
                    param[1].Value = ZoneTypeId;

                    ds = SqlHelper.ExecuteDataset(con.ConnectionString.ToString(), CommandType.StoredProcedure, "sp_Geofence", param);


                }
                catch (Exception ex)
                {

                    LogError.RegisterErrorInLogFile("clsGeofenceMaster.cs", "GetZeoZones()", ex.Message  + ex.StackTrace);
                    throw;


                }
            }
            return ds;
        }

    }
}