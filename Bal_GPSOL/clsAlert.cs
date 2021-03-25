using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WLT.DataAccessLayer;
using WLT.DataAccessLayer.GPSOL;
using WLT.EntityLayer;
using WLT.EntityLayer.GPSOL;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;
using WLT.BusinessLogic.BAL;
using WLT.DataAccessLayer.Classes;

namespace WLT.BusinessLogic.Bal_GPSOL {
    public class clsAlert : IDisposable {
        #region varibles
        private int _ipkAlertID;
        private string _vAlertName;
        private string _vpkDeviceID;
        private DateTime _dAge;
        private DateTime _dDate;
        private bool _bStatus;
        private int _ifkReportID;
        private string _vPriority;
        private string _vLongitude;
        private string _vLatitude;
        private string _CurrentDateTime;
        private string _TwoDaysBeforeDate;
        private string _TenMnsBeforeDate;
        private int _Operation;
        private int _iParentID;
        private string _vQuickAlertContent;
        private string _HPAlert;
        private List<clsAlert> _lstQuickAlertContent;
        private List<clsAlert> _lstTimePeriodContent;
        private List<clsAlert> _lstOtherContent;
        private string _DateTime_24hrs;
        private string _Login;
        private int _ifkCompanyID;
        private int _ifkPriorityID;
        private bool _bAnyVehicle;
        private int _ifkGeoMID;
        private int _ipkAddAlertID;
        private int _ifkGroupMID;
        private string _vZone;
        private string _vZoneType;

        private string _vTrip;
        private int _iTriggeredEventID;
        private bool _bTriggeredEventStatus;
        private string _vDigitalEvent;
        private bool _bDigitalEventStatus;
        private string _vAnaloglEvent;
        private bool _bAnalogEventStatus;
        private bool _bTemeperatureViolation;
        private bool _bOverspeed;
        private bool _bExcessiveIdle;
        private string _vReminder;
        private int _iTrackerTypeID;
        private string _AlertSound;
        private string _QuickAlertsEnabled;
        private bool _bZoneOverspeed;

        private bool _IsAnyDriver;
        private int _AddAlertDriverID;
        private int _iSensorID;
        private int _AnalogType;

        private string _dCreatedOn;
        private string _iCreatedBy;
        private string _dLastAccessed;
        private string _iUpdatedBy;
        private bool _bDigitalState;
        private bool _bIsDigitalAlert;
        private int _iPercentageTolerance;
        private int _Filter;
        private string _AlertTimeRange;

        public string AlertTimeRange {
            get { return _AlertTimeRange; }
            set { _AlertTimeRange = value; }
        }
        public int iPercentageTolerance {
            get { return _iPercentageTolerance; }
            set { _iPercentageTolerance = value; }
        }

        public bool bDigitalState {
            get { return _bDigitalState; }
            set { _bDigitalState = value; }
        }
        public bool bIsDigitalAlert {
            get { return _bIsDigitalAlert; }
            set { _bIsDigitalAlert = value; }
        }

        public bool bZoneOverspeed {
            get { return _bZoneOverspeed; }
            set { _bZoneOverspeed = value; }
        }
        public int iTrackerTypeID {
            get { return _iTrackerTypeID; }
            set { _iTrackerTypeID = value; }
        }

        private bool _bZoneEnterOrExit;

        public bool bZoneEnterOrExit {
            get { return _bZoneEnterOrExit; }
            set { _bZoneEnterOrExit = value; }
        }

        private bool _bAnyNoGoZone;

        public bool bAnyNoGoZone {
            get { return _bAnyNoGoZone; }
            set { _bAnyNoGoZone = value; }
        }

        private bool _bAnyKeepInZone;

        public bool bAnyKeepInZone {
            get { return _bAnyKeepInZone; }
            set { _bAnyKeepInZone = value; }
        }

        private bool _bAnyLocationZone;

        public bool bAnyLocationZone {
            get { return _bAnyLocationZone; }
            set { _bAnyLocationZone = value; }
        }

        private bool _isAnyZone;

        public bool isAnyZone {
            get { return _isAnyZone; }
            set { _isAnyZone = value; }
        }

        private int _zoneTypeId;

        public int zoneTypeId {
            get { return _zoneTypeId; }
            set { _zoneTypeId = value; }
        }

        private bool _bHoursType;
        private string _vStartsTime;
        private string _vEndsTime;
        private string _vDaysOfWeek;

        private string _vEventName;
        private bool _bEventStatus;

        private string _vUserIDs;
        private string _vRoleIDs;
        private int _bOnScreenNotification;
        private int _iRecentMoreAlert;
        private string _vAlertfilterID;

        public bool IsZoneAvaibale { get; set; }
        public int FilterCode { get; set; }
        public string Grou_DeviceName { get; set; }
        public string EventName { get; set; }

        private string _htmlSelectAsset;
        private string _htmlTriggredEvent;
        private string _htmlTimePeriod;
        private string _htmlOtherCriteria;
        private string _htmlNotify;
        private string _vHoursName;
        private string _sbNotifyMe;

        private int _ifkCommonEventLookupID;
        private string _iAnalogRange_UpperValue;
        private string _iAnalogRange_LowerValue;
        private string _vNotifyWay;
        private int _ifkUserId;
        private string _iPkAlertNotifyID;
        private string _AlertAssets;

        public string sbNotifyMe { get { return _sbNotifyMe; } set { _sbNotifyMe = value; } }
        public int ifkUserId { get { return _ifkUserId; } set { _ifkUserId = value; } }
        public bool bIsForAllCompanies { get; set; }
        public string htmlSelectAsset { get { return _htmlSelectAsset; } set { _htmlSelectAsset = value; } }
        public string htmlTriggredEvent { get { return _htmlTriggredEvent; } set { _htmlTriggredEvent = value; } }
        public string htmlTimePeriod { get { return _htmlTimePeriod; } set { _htmlTimePeriod = value; } }
        public string htmlOtherCriteria { get { return _htmlOtherCriteria; } set { _htmlOtherCriteria = value; } }
        public string htmlNotify { get { return _htmlNotify; } set { _htmlNotify = value; } }
        public string vHoursName { get { return _vHoursName; } set { _vHoursName = value; } }

        public string vAlertfilterID { get { return _vAlertfilterID; } set { _vAlertfilterID = value; } }
        public string DateTime_24hrs { get { return _DateTime_24hrs; } set { _DateTime_24hrs = value; } }
        public string CurrentDateTime { get { return _CurrentDateTime; } set { _CurrentDateTime = value; } }
        public string TwoDaysBeforeDate { get { return _TwoDaysBeforeDate; } set { _TwoDaysBeforeDate = value; } }
        public string TenMnsBeforeDate { get { return _TenMnsBeforeDate; } set { _TenMnsBeforeDate = value; } }
        public string vQuickAlertContent { get { return _vQuickAlertContent; } set { _vQuickAlertContent = value; } }
        public string HPAlert { get { return _HPAlert; } set { _HPAlert = value; } }
        public List<clsAlert> lstQuickAlertContent { get { return _lstQuickAlertContent; } set { _lstQuickAlertContent = value; } }
        public List<clsAlert> lstTimePeriodContent { get { return _lstTimePeriodContent; } set { _lstTimePeriodContent = value; } }
        public List<clsAlert> lstOtherContent { get { return _lstOtherContent; } set { _lstOtherContent = value; } }

        public int ipkAlertID { get { return _ipkAlertID; } set { _ipkAlertID = value; } }
        public string vAlertName { get { return _vAlertName; } set { _vAlertName = value; } }
        public string vpkDeviceID { get { return _vpkDeviceID; } set { _vpkDeviceID = value; } }
        public DateTime dAge { get { return _dAge; } set { _dAge = value; } }
        public DateTime dDate { get { return _dDate; } set { _dDate = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public int ifkReportID { get { return _ifkReportID; } set { _ifkReportID = value; } }
        public string vPriority { get { return _vPriority; } set { _vPriority = value; } }
        public string vLongitude { get { return _vLongitude; } set { _vLongitude = value; } }
        public string vLatitude { get { return _vLatitude; } set { _vLatitude = value; } }
        public string Login { get { return _Login; } set { _Login = value; } }
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int iParentID { get { return _iParentID; } set { _iParentID = value; } }
        public int ifkCompanyID { get { return _ifkCompanyID; } set { _ifkCompanyID = value; } }
        public int ifkPriorityID { get { return _ifkPriorityID; } set { _ifkPriorityID = value; } }
        public bool bAnyVehicle { get { return _bAnyVehicle; } set { _bAnyVehicle = value; } }
        public int ifkGeoMID { get { return _ifkGeoMID; } set { _ifkGeoMID = value; } }
        public int ipkAddAlertID { get { return _ipkAddAlertID; } set { _ipkAddAlertID = value; } }
        public int ifkGroupMID { get { return _ifkGroupMID; } set { _ifkGroupMID = value; } }
        public string vZone { get { return _vZone; } set { _vZone = value; } }
        public string vZoneType { get { return _vZoneType; } set { _vZoneType = value; } }

        public string vTrip { get { return _vTrip; } set { _vTrip = value; } }
        public int iTriggeredEventID { get { return _iTriggeredEventID; } set { _iTriggeredEventID = value; } }
        public bool bTriggeredEventStatus { get { return _bTriggeredEventStatus; } set { _bTriggeredEventStatus = value; } }
        public string vDigitalEvent { get { return _vDigitalEvent; } set { _vDigitalEvent = value; } }
        public bool bDigitalEventStatus { get { return _bDigitalEventStatus; } set { _bDigitalEventStatus = value; } }
        public string vAnaloglEvent { get { return _vAnaloglEvent; } set { _vAnaloglEvent = value; } }
        public bool bAnalogEventStatus { get { return _bAnalogEventStatus; } set { _bAnalogEventStatus = value; } }
        public bool bTemeperatureViolation { get { return _bTemeperatureViolation; } set { _bTemeperatureViolation = value; } }
        public bool bOverspeed { get { return _bOverspeed; } set { _bOverspeed = value; } }
        public bool bExcessiveIdle { get { return _bExcessiveIdle; } set { _bExcessiveIdle = value; } }
        public string vReminder { get { return _vReminder; } set { _vReminder = value; } }

        public bool bHoursType { get { return _bHoursType; } set { _bHoursType = value; } }
        public string vStartsTime { get { return _vStartsTime; } set { _vStartsTime = value; } }
        public string vEndsTime { get { return _vEndsTime; } set { _vEndsTime = value; } }
        public string vDaysOfWeek { get { return _vDaysOfWeek; } set { _vDaysOfWeek = value; } }

        public string vEventName { get { return _vEventName; } set { _vEventName = value; } }
        public bool bEventStatus { get { return _bEventStatus; } set { _bEventStatus = value; } }

        public string vUserIDs { get { return _vUserIDs; } set { _vUserIDs = value; } }
        public string vRoleIDs { get { return _vRoleIDs; } set { _vRoleIDs = value; } }
        public int bOnScreenNotification { get { return _bOnScreenNotification; } set { _bOnScreenNotification = value; } }
        public int iRecentMoreAlert { get { return _iRecentMoreAlert; } set { _iRecentMoreAlert = value; } }

        public int ifkCommonEventLookupID { get { return _ifkCommonEventLookupID; } set { _ifkCommonEventLookupID = value; } }
        public string iAnalogRange_UpperValue { get { return _iAnalogRange_UpperValue; } set { _iAnalogRange_UpperValue = value; } }
        public string iAnalogRange_LowerValue { get { return _iAnalogRange_LowerValue; } set { _iAnalogRange_LowerValue = value; } }

        public string vNotifyWay { get { return _vNotifyWay; } set { _vNotifyWay = value; } }
        public string iPkAlertNotifyID { get { return _iPkAlertNotifyID; } set { _iPkAlertNotifyID = value; } }
        public string AlertSound { get { return _AlertSound; } set { _AlertSound = value; } }
        public string QuickAlertsEnabled { get { return _QuickAlertsEnabled; } set { _QuickAlertsEnabled = value; } }
        public int ifkDigitalMasterID { get; set; }

        public bool IsAnyDriver { get { return _IsAnyDriver; } set { _IsAnyDriver = value; } }
        public int AddAlertDriverID { get { return _AddAlertDriverID; } set { _AddAlertDriverID = value; } }

        public string AlertAssets { get { return _AlertAssets; } set { _AlertAssets = value; } }
        public int iSensorID { get { return _iSensorID; } set { _iSensorID = value; } }
        public int AnalogType { get { return _AnalogType; } set { _AnalogType = value; } }

        private string _Dn;
        private string _Td;
        private string _Mc;
        private string _Sc;
        private string _An;
        private Dictionary<string, clsAlert> _jsondic;
        private string _strJson;
        private int _Id;
        private int _acId;
        private DateTime _dGPSDateTime;

        public int Id { get { return _Id; } set { _Id = value; } }
        public string An { get { return _An; } set { _An = value; } }
        public string Dn { get { return _Dn; } set { _Dn = value; } }
        public string Td { get { return _Td; } set { _Td = value; } }
        public string Mc { get { return _Mc; } set { _Mc = value; } }
        public string Sc { get { return _Sc; } set { _Sc = value; } }
        public int acId { get { return _acId; } set { _acId = value; } }
        public Dictionary<string, clsAlert> jsondic { get { return _jsondic; } set { _jsondic = value; } }
        public string strJson { get { return _strJson; } set { _strJson = value; } }

        public string dCreatedOn { get { return _dCreatedOn; } set { _dCreatedOn = value; } }
        public string iCreatedBy { get { return _iCreatedBy; } set { _iCreatedBy = value; } }
        public string dLastAccessed { get { return _dLastAccessed; } set { _dLastAccessed = value; } }
        public string iUpdatedBy { get { return _iUpdatedBy; } set { _iUpdatedBy = value; } }
        public DateTime dGPSDateTime { get { return _dGPSDateTime; } set { _dGPSDateTime = value; } }

        public int Filter { get { return _Filter; } set { _Filter = value; } }
        public bool isAlertTimePeriodActive { get; set; }

        public bool isSMSMeNotify { get; set; }
        public bool isEmailMeNotify { get; set; }
        public bool isOnscreenMeNotify { get; set; }

        #endregion

        #region Constructors
        public clsAlert () {
            

            _wlt_AppConfig = AppConfiguration.GetAppSettings<wlt_Config> ("wlt_config");

            Connectionstring = AppConfiguration.GetAppSettings<wlt_Config> ("ConnectionStrings").wlt_WebAppConnectionString;

        }
        public clsAlert (Dictionary<string, clsAlert> jsondic, string str, string HPAlert, string AlertSound, int QuickAlertsEnabled) {
            this.jsondic = jsondic;
            this.strJson = str;
            this.HPAlert = HPAlert;
            this.AlertSound = AlertSound;
            this.QuickAlertsEnabled = QuickAlertsEnabled.ToString ();
        }

        public clsAlert (int ipkAlertID, string vAlertName, string vpkDeviceID, DateTime dAge, DateTime dDate, bool bStatus, int ifkReportID, string vPriority, string vLongitude, string vLatitude) {
            this.ipkAlertID = ipkAlertID;
            this.vAlertName = vAlertName;
            this.vpkDeviceID = vpkDeviceID;
            this.dAge = dAge;
            this.dDate = dDate;
            this.bStatus = bStatus;
            this.ifkReportID = ifkReportID;
            this.vPriority = vPriority;
            this.vLongitude = vLongitude;
            this.vLatitude = vLatitude;
        }
        public clsAlert (string Login) {
            this.Login = Login; //  for null session
        }

        public clsAlert (string vAlertName, int ifkCompanyID, int ifkPriorityID, string htmlSelectAsset, string htmlTriggredEvent, string htmlTimePeriod, string htmlOtherCriteria, string htmlNotify, List<clsAlert> lstTimePeriodContent) {
            this.vAlertName = vAlertName;
            this.ifkCompanyID = ifkCompanyID;
            this.ifkPriorityID = ifkPriorityID;
            this.htmlSelectAsset = htmlSelectAsset;
            this.htmlTriggredEvent = htmlTriggredEvent;
            this.htmlTimePeriod = htmlTimePeriod;
            this.htmlOtherCriteria = htmlOtherCriteria;
            this.htmlNotify = htmlNotify;
            this.lstTimePeriodContent = lstTimePeriodContent;

        }
        public clsAlert (string vAlertName, int ifkCompanyID, int ifkPriorityID, bool IsAnyVehicle, int GroupId, string DeviceID, string Grou_DeviceName, int iTriggeredEventID, string EventName,
            bool bDigitalEventStatus, bool bAnalogEventStatus, bool bTemeperatureViolation, bool bOverspeed, bool bExcessiveIdle, string htmlTimePeriod, string htmlOtherCriteria, string htmlNotify, List<clsAlert> lstTimePeriodContent) {
            this.vAlertName = vAlertName;
            this.ifkCompanyID = ifkCompanyID;
            this.ifkPriorityID = ifkPriorityID;
            this.bAnyVehicle = IsAnyVehicle;
            this.ifkGroupMID = GroupId;
            this.vpkDeviceID = DeviceID;
            this.Grou_DeviceName = Grou_DeviceName;
            this.iTriggeredEventID = iTriggeredEventID;
            this.EventName = EventName;
            this.bDigitalEventStatus = bDigitalEventStatus;
            this.bAnalogEventStatus = bAnalogEventStatus;
            this.bTemeperatureViolation = bTemeperatureViolation;
            this.bOverspeed = bOverspeed;
            this.bExcessiveIdle = bExcessiveIdle;
            this.htmlTimePeriod = htmlTimePeriod;
            this.htmlOtherCriteria = htmlOtherCriteria;
            this.htmlNotify = htmlNotify;
            this.lstTimePeriodContent = lstTimePeriodContent;
        }
        public clsAlert (string vAlertName, int ifkCompanyID, int ifkPriorityID, string iCreatedBy, string dCreatedOn, string iUpdatedBy, string dLastAccessed) {
            this.vAlertName = vAlertName;
            this.ifkCompanyID = ifkCompanyID;
            this.ifkPriorityID = ifkPriorityID;
            this.iCreatedBy = iCreatedBy;
            this.dCreatedOn = dCreatedOn;
            this.iUpdatedBy = iUpdatedBy;
            this.dLastAccessed = dLastAccessed;

        }

        public clsAlert (List<clsAlert> lstTimePeriodContent, string htmlOtherCriteria, string sbNotify, string sbTimePeriod, string sbNotifyMe) {
            this.lstTimePeriodContent = lstTimePeriodContent;
            this.htmlOtherCriteria = htmlOtherCriteria;
            this.htmlNotify = sbNotify;
            this.htmlTimePeriod = sbTimePeriod;
            this.sbNotifyMe = sbNotifyMe;
        }

        public clsAlert (List<clsAlert> lstTimePeriodContent, string htmlOtherCriteria, string sbNotify, string sbTimePeriod) {
            this.lstTimePeriodContent = lstTimePeriodContent;
            this.htmlOtherCriteria = htmlOtherCriteria;
            this.htmlNotify = sbNotify;
            this.htmlTimePeriod = sbTimePeriod;
        }
        public clsAlert (bool IsAnyVehicle, int GroupId, string DeviceID, string Grou_DeviceName, int iTriggeredEventID, string EventName,
            string DigitalEvt, bool bDigitalEventStatus, string AnalogEvt, bool bAnalogEventStatus, bool bTemeperatureViolation, bool bOverspeed, bool bExcessiveIdle, string Trip, bool IsZoneAvaibale,
            string zone, int iTrackerTypeID, bool bAnyNoGoZone, bool bAnyKeepInZone, bool bAnyLocationZone, bool bZoneEnterOrExit, int ifkGeoMID, int iPercentageTolerance, bool isAnyZone, int zoneTypeId) {
            this.bAnyVehicle = IsAnyVehicle;
            this.ifkGroupMID = GroupId;
            this.vpkDeviceID = DeviceID;
            this.Grou_DeviceName = Grou_DeviceName;
            this.iTriggeredEventID = iTriggeredEventID;
            this.EventName = EventName;
            this.vDigitalEvent = DigitalEvt;
            this.bDigitalEventStatus = bDigitalEventStatus;
            this.vAnaloglEvent = AnalogEvt;
            this.bAnalogEventStatus = bAnalogEventStatus;
            this.bTemeperatureViolation = bTemeperatureViolation;
            this.bOverspeed = bOverspeed;
            this.bExcessiveIdle = bExcessiveIdle;
            this.vTrip = Trip;
            this.IsZoneAvaibale = IsZoneAvaibale;
            this.vZone = zone;
            this.iTrackerTypeID = iTrackerTypeID;
            this.bAnyNoGoZone = bAnyNoGoZone;
            this.bAnyKeepInZone = bAnyKeepInZone;
            this.bAnyLocationZone = bAnyLocationZone;
            this.bZoneEnterOrExit = bZoneEnterOrExit;
            this.ifkGeoMID = ifkGeoMID;
            this.iPercentageTolerance = iPercentageTolerance;

            this.isAnyZone = isAnyZone;
            this.zoneTypeId = zoneTypeId;
        }
        public clsAlert (string vHoursName, string vStartsTime, string vEndsTime, string vDaysOfWeek) {
            this.vHoursName = vHoursName;
            this.vStartsTime = vStartsTime;
            this.vEndsTime = vEndsTime;
            this.vDaysOfWeek = vDaysOfWeek;
        }

        public clsAlert (int ipkAlertID, int ifkAlertCapturedID, string vEventName, string vDeviceName, string vAlertName, string TrackerDate, string msgCount, string strSaveComment, DateTime dGPSDateTime) {
            this.Id = ipkAlertID;
            this.acId = ifkAlertCapturedID;
            this.vEventName = vEventName;
            this.Dn = vDeviceName;
            this.An = vAlertName;
            this.Td = TrackerDate;
            this.Mc = msgCount;
            this.Sc = strSaveComment;
            this.dGPSDateTime = dGPSDateTime;
        }

        #endregion

        //#region methods
        //public DataSet GetQuickAlerts()
        //{
        //    DataSet ds = new DataSet();
        //    SqlParameter[] param = new SqlParameter[6];
        //    try
        //    {
        //        param[0] = new SqlParameter("@Operation", SqlDbType.Int);
        //        param[0].Value = Operation;

        //        param[1] = new SqlParameter("@CurrentDateTime", SqlDbType.VarChar);
        //        param[1].Value = CurrentDateTime;

        //        param[2] = new SqlParameter("@TwoDaysBeforeDate", SqlDbType.VarChar);
        //        param[2].Value = TwoDaysBeforeDate;

        //        param[3] = new SqlParameter("@iParentID", SqlDbType.Int);
        //        param[3].Value = iParentID;

        //        param[4] = new SqlParameter("@TenMnsBeforeDate", SqlDbType.VarChar);
        //        param[4].Value = TenMnsBeforeDate;

        //        param[5] = new SqlParameter("@userId", SqlDbType.BigInt);
        //        param[5].Value = ifkUserId;

        //        ds =SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_QuickAlerts", param);

        //    }
        //    catch (Exception ex)
        //    {
        //        LogError.RegisterErrorInLogFile( "clsAlert.cs", "GetQuickAlerts()", ex.Message  + ex.StackTrace);
        //    }

        //    return ds;
        //}
        #region methods
        //public DataSet GetQuickAlerts()
        //{
        //    DataSet ds = new DataSet();
        //    SqlParameter[] param = new SqlParameter[6];
        //    try
        //    {

        //        param[0] = new SqlParameter("@userId", SqlDbType.BigInt);
        //        param[0].Value = ifkUserId;

        //        ds =SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_QuickAlerts", param);

        //    }
        //    catch (Exception ex)
        //    {
        //        LogError.RegisterErrorInLogFile( "clsAlert.cs", "GetQuickAlerts()", ex.Message  + ex.StackTrace);
        //    }

        //    return ds;
        //}

        private readonly wlt_Config _wlt_AppConfig;

        public string Connectionstring { get; set; }

        public DataSet GetQuickAlerts () {
            DataSet ds = new DataSet ();
            SqlParameter[] param = new SqlParameter[3];
            try {
                param[0] = new SqlParameter ("@CurrentloggedInUser", SqlDbType.Int);
                param[0].Value = _ifkUserId;

                param[1] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter ("@Filter", SqlDbType.Int);
                param[2].Value = Filter;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alerts_New_Onscreen", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "sp_Alerts_New_Onscreen", ex.Message  + ex.StackTrace);
            }
            return ds;
        }
        public DataSet GetRecentEventsOnAssetInfo () {
            DataSet ds = new DataSet ();
            SqlParameter[] param = new SqlParameter[6];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@CurrentDateTime", SqlDbType.VarChar);
                param[1].Value = CurrentDateTime;

                param[2] = new SqlParameter ("@DateTime_24hrs", SqlDbType.VarChar);
                param[2].Value = DateTime_24hrs;

                param[3] = new SqlParameter ("@vpkDeviceID", SqlDbType.VarChar);
                param[3].Value = vpkDeviceID;

                param[4] = new SqlParameter ("@ifkCompanyID", SqlDbType.VarChar);
                param[4].Value = ifkCompanyID;

                param[5] = new SqlParameter ("@IDs", SqlDbType.VarChar);
                param[5].Value = vAlertfilterID;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_RecentEventsOnAssetTab", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetRecentEventsOnAssetInfo()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        #region Save Data for Alerts
        public string SaveAddAlert () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[7];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@vAlertName", SqlDbType.VarChar);
                param[1].Value = vAlertName;

                param[2] = new SqlParameter ("@ifkCompanyID", SqlDbType.VarChar);
                param[2].Value = ifkCompanyID;

                param[3] = new SqlParameter ("@ifkPriorityID", SqlDbType.Int);
                param[3].Value = ifkPriorityID;

                param[4] = new SqlParameter ("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[5].Value = ipkAddAlertID;

                param[6] = new SqlParameter ("@IsForAllCompanies", SqlDbType.Bit);
                param[6].Value = bIsForAllCompanies;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_AddAlert", param);
                Result = param[4].Value.ToString ();
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAddAlert()", ex.Message  + ex.StackTrace);
                Result = "-1";
            }

            return Result;
        }

        public string SaveAddAlertCriteria () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[20];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.NVarChar);
                param[1].Value = ipkAddAlertID;

                param[2] = new SqlParameter ("@vpkDeviceID", SqlDbType.NVarChar);
                param[2].Value = vpkDeviceID;

                param[3] = new SqlParameter ("@ifkGroupMID", SqlDbType.NVarChar);
                param[3].Value = ifkGroupMID;

                param[4] = new SqlParameter ("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter ("@bAnyVehicle", SqlDbType.Bit);
                param[5].Value = bAnyVehicle;

                param[6] = new SqlParameter ("@vZone", SqlDbType.NVarChar);
                param[6].Value = vZone;

                param[7] = new SqlParameter ("@vZoneType", SqlDbType.NVarChar);
                param[7].Value = vZoneType;

                param[8] = new SqlParameter ("@ifkGeoMID", SqlDbType.NVarChar);
                param[8].Value = ifkGeoMID;

                param[9] = new SqlParameter ("@vTrip", SqlDbType.NVarChar);
                param[9].Value = vTrip;

                param[10] = new SqlParameter ("@iTriggeredEventID", SqlDbType.Int);
                param[10].Value = iTriggeredEventID;

                param[11] = new SqlParameter ("@bTriggeredEventStatus", SqlDbType.Bit);
                param[11].Value = bTriggeredEventStatus;

                param[12] = new SqlParameter ("@vDigitalEvent", SqlDbType.NVarChar);
                param[12].Value = vDigitalEvent;

                param[13] = new SqlParameter ("@bDigitalEventStatus", SqlDbType.Bit);
                param[13].Value = bDigitalEventStatus;

                param[14] = new SqlParameter ("@vAnaloglEvent", SqlDbType.NVarChar);
                param[14].Value = vAnaloglEvent;

                param[15] = new SqlParameter ("@bAnalogEventStatus", SqlDbType.Bit);
                param[15].Value = bAnalogEventStatus;

                param[16] = new SqlParameter ("@bTemeperatureViolation", SqlDbType.Bit);
                param[16].Value = bTemeperatureViolation;

                param[17] = new SqlParameter ("@bOverspeed", SqlDbType.Bit);
                param[17].Value = bOverspeed;

                param[18] = new SqlParameter ("@bExcessiveIdle", SqlDbType.Bit);
                param[18].Value = bExcessiveIdle;

                param[19] = new SqlParameter ("@vReminder", SqlDbType.NVarChar);
                param[19].Value = vReminder;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_AddAlert", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAddAlertCriteria()", ex.Message  + ex.StackTrace);
                Result = "Internal Execution Problem:" + ex.Message;
            }

            return Result;
        }

        public string SaveAddAlertTimePeriods () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[6];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@bHoursType", SqlDbType.Bit);
                param[1].Value = bHoursType;

                param[2] = new SqlParameter ("@tStartsTime", SqlDbType.Time);
                param[2].Value = vStartsTime;

                param[3] = new SqlParameter ("@tEndsTime", SqlDbType.Time);
                param[3].Value = vEndsTime;;

                param[4] = new SqlParameter ("@vDaysOfWeek", SqlDbType.NVarChar);
                param[4].Value = vDaysOfWeek;

                param[5] = new SqlParameter ("@ifkAddAlertID", SqlDbType.NVarChar);
                param[5].Value = ipkAddAlertID;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_AddAlert", param);
                Result = "";

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAddAlert()", ex.Message  + ex.StackTrace);
                Result = "-1";
            }

            return Result;
        }

        public string SaveAddAlertotherCriteria () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[4];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@vEventName", SqlDbType.NVarChar);
                param[1].Value = vEventName;

                param[2] = new SqlParameter ("@bEventStatus", SqlDbType.Bit);
                param[2].Value = bEventStatus;

                param[3] = new SqlParameter ("@ifkAddAlertID", SqlDbType.NVarChar);
                param[3].Value = ipkAddAlertID;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_AddAlert", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAddAlert()", ex.Message  + ex.StackTrace);
                Result = "-1";
            }

            return Result;
        }

        public string SaveAddAlertNotifyUsers () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[5];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@vUserIDs", SqlDbType.NVarChar);
                param[1].Value = vUserIDs;

                param[2] = new SqlParameter ("@vRoleIDs", SqlDbType.NVarChar);
                param[2].Value = vRoleIDs;

                param[3] = new SqlParameter ("@ifkAddAlertID", SqlDbType.NVarChar);
                param[3].Value = ipkAddAlertID;

                param[4] = new SqlParameter ("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_AddAlert", param);
                if (param[4].Value.ToString () == "1") {
                    Result = "Save successful";
                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAddAlertNotifyUsers()", ex.Message  + ex.StackTrace);
                Result = "Internal Execution Error" + ex.Message;
            }

            return Result;
        }

        #endregion //old one

        #region Save Data for Alerts
        public string SaveAlert () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[9];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@vAlertName", SqlDbType.VarChar);
                param[1].Value = vAlertName;

                param[2] = new SqlParameter ("@ifkCompanyID", SqlDbType.VarChar);
                param[2].Value = ifkCompanyID;

                param[3] = new SqlParameter ("@ifkPriorityID", SqlDbType.Int);
                param[3].Value = ifkPriorityID;

                param[4] = new SqlParameter ("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[5].Value = ipkAddAlertID;

                param[6] = new SqlParameter ("@IsForAllCompanies", SqlDbType.Bit);
                param[6].Value = bIsForAllCompanies;

                param[7] = new SqlParameter ("@ifkUserId", SqlDbType.Int);
                param[7].Value = ifkUserId;

                param[8] = new SqlParameter ("@AlertAssetsImei", SqlDbType.VarChar);
                param[8].Value = AlertAssets;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                Result = param[4].Value.ToString ();
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAlert()", ex.Message  + ex.StackTrace);
                Result = "-1";
            }

            return Result;
        }

        public string SaveAlertCriteria () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[39];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.NVarChar);
                param[1].Value = ipkAddAlertID;

                param[2] = new SqlParameter ("@vpkDeviceID", SqlDbType.NVarChar);
                param[2].Value = vpkDeviceID;

                param[3] = new SqlParameter ("@ifkGroupMID", SqlDbType.NVarChar);
                param[3].Value = ifkGroupMID;

                param[4] = new SqlParameter ("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter ("@bAnyVehicle", SqlDbType.Bit);
                param[5].Value = bAnyVehicle;

                param[6] = new SqlParameter ("@vZone", SqlDbType.NVarChar);
                param[6].Value = vZone;

                param[7] = new SqlParameter ("@vZoneType", SqlDbType.NVarChar);
                param[7].Value = vZoneType;

                param[8] = new SqlParameter ("@ifkGeoMID", SqlDbType.NVarChar);
                param[8].Value = ifkGeoMID;

                param[9] = new SqlParameter ("@vTrip", SqlDbType.NVarChar);
                param[9].Value = vTrip;

                param[10] = new SqlParameter ("@iTriggeredEventID", SqlDbType.Int);
                param[10].Value = iTriggeredEventID;

                param[11] = new SqlParameter ("@bTriggeredEventStatus", SqlDbType.Bit);
                param[11].Value = bTriggeredEventStatus;

                param[12] = new SqlParameter ("@vDigitalEvent", SqlDbType.NVarChar);
                param[12].Value = vDigitalEvent;

                param[13] = new SqlParameter ("@bDigitalEventStatus", SqlDbType.Bit);
                param[13].Value = bDigitalEventStatus;

                param[14] = new SqlParameter ("@vAnaloglEvent", SqlDbType.NVarChar);
                param[14].Value = vAnaloglEvent;

                param[15] = new SqlParameter ("@bAnalogEventStatus", SqlDbType.Bit);
                param[15].Value = bAnalogEventStatus;

                param[16] = new SqlParameter ("@bTemeperatureViolation", SqlDbType.Bit);
                param[16].Value = bTemeperatureViolation;

                param[17] = new SqlParameter ("@bOverspeed", SqlDbType.Bit);
                param[17].Value = bOverspeed;

                param[18] = new SqlParameter ("@bExcessiveIdle", SqlDbType.Bit);
                param[18].Value = bExcessiveIdle;

                param[19] = new SqlParameter ("@vReminder", SqlDbType.NVarChar);
                param[19].Value = vReminder;

                param[20] = new SqlParameter ("@ifkCommonEventLookupID", SqlDbType.Int);
                param[20].Value = ifkCommonEventLookupID;

                param[21] = new SqlParameter ("@iAnalogRange_UpperValue", SqlDbType.NVarChar);
                param[21].Value = iAnalogRange_UpperValue;

                param[22] = new SqlParameter ("@iAnalogRange_LowerValue", SqlDbType.NVarChar);
                param[22].Value = iAnalogRange_LowerValue;

                param[23] = new SqlParameter ("@ifkTrackerTypeId", SqlDbType.Int);
                param[23].Value = iTrackerTypeID;

                param[24] = new SqlParameter ("@bZoneEnterOrExit", SqlDbType.Int);
                param[24].Value = bZoneEnterOrExit;

                param[25] = new SqlParameter ("@bAnyNoGoZone", SqlDbType.Int);
                param[25].Value = bAnyNoGoZone;

                param[26] = new SqlParameter ("@bAnyKeepInZone", SqlDbType.Int);
                param[26].Value = bAnyKeepInZone;

                param[27] = new SqlParameter ("@bAnyLocationZone", SqlDbType.Int);
                param[27].Value = bAnyLocationZone;

                param[28] = new SqlParameter ("@IsAnyDriver", SqlDbType.Int);
                param[28].Value = IsAnyDriver;

                param[29] = new SqlParameter ("@AddAlertDriverID", SqlDbType.Int);
                param[29].Value = AddAlertDriverID;

                param[30] = new SqlParameter ("@bIsDigitalAlert", SqlDbType.Bit);
                param[30].Value = bIsDigitalAlert;

                param[31] = new SqlParameter ("@bDigitalState", SqlDbType.Bit);
                param[31].Value = bDigitalState;

                param[32] = new SqlParameter ("@ifkDigitalMasterID", SqlDbType.BigInt);
                param[32].Value = ifkDigitalMasterID;

                param[33] = new SqlParameter ("@iPercentageTolerance", SqlDbType.Int);
                param[33].Value = iPercentageTolerance;

                param[34] = new SqlParameter ("@isAnyZone", SqlDbType.Bit);
                param[34].Value = isAnyZone;

                param[35] = new SqlParameter ("@zoneTypeId", SqlDbType.Int);
                param[35].Value = zoneTypeId;

                param[36] = new SqlParameter ("@AlertAssetsImei", SqlDbType.VarChar);
                param[36].Value = AlertAssets;

                param[37] = new SqlParameter ("@iSensorID", SqlDbType.Int);
                param[37].Value = iSensorID;

                param[38] = new SqlParameter ("@AnalogType", SqlDbType.Int);
                param[38].Value = AnalogType;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                Result = Convert.ToString (param[4].Value);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAlertCriteria()", ex.Message  + ex.StackTrace);
                Result = "Internal Execution Problem:" + ex.Message;
            }

            return Result;
        }

        public string SaveAlertTimeRange () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[4];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@AlertTimeRange", SqlDbType.VarChar);
                param[1].Value = AlertTimeRange;

                param[2] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[2].Value = ipkAddAlertID;

                param[3] = new SqlParameter ("@isAlertTimePeriodActive", SqlDbType.Bit);
                param[3].Value = isAlertTimePeriodActive;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                Result = "";
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAlertTimeRange()", ex.Message  + ex.StackTrace);
                Result = "-1";
            }

            return Result;
        }

        public string SaveAlertTimePeriods () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[6];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@bHoursType", SqlDbType.Bit);
                param[1].Value = bHoursType;

                param[2] = new SqlParameter ("@tStartsTime", SqlDbType.Time);
                param[2].Value = vStartsTime;

                param[3] = new SqlParameter ("@tEndsTime", SqlDbType.Time);
                param[3].Value = vEndsTime;

                param[4] = new SqlParameter ("@vDaysOfWeek", SqlDbType.NVarChar);
                param[4].Value = vDaysOfWeek;

                param[5] = new SqlParameter ("@ifkAddAlertID", SqlDbType.NVarChar);
                param[5].Value = ipkAddAlertID;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                Result = "";

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAlertTimePeriods()", ex.Message  + ex.StackTrace);
                Result = "-1";
            }

            return Result;
        }

        public string SaveAlertotherCriteria () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[4];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkDigitalMasterID", SqlDbType.BigInt);
                param[1].Value = ifkDigitalMasterID;

                param[2] = new SqlParameter ("@bEventStatus", SqlDbType.Bit);
                param[2].Value = bEventStatus;

                param[3] = new SqlParameter ("@ifkAddAlertID", SqlDbType.NVarChar);
                param[3].Value = ipkAddAlertID;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAlertotherCriteria()", ex.Message  + ex.StackTrace);
                Result = "-1";
            }

            return Result;
        }

        public string SaveAlertNotifyUsers () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[6];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@vUserIDs", SqlDbType.NVarChar);
                param[1].Value = vUserIDs;

                param[2] = new SqlParameter ("@vRoleIDs", SqlDbType.NVarChar);
                param[2].Value = vRoleIDs;

                param[3] = new SqlParameter ("@ifkAddAlertID", SqlDbType.NVarChar);
                param[3].Value = ipkAddAlertID;

                param[4] = new SqlParameter ("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter ("@vNotifyWays", SqlDbType.NVarChar);
                param[5].Value = vNotifyWay;

                //param[6] = new SqlParameter("@iPkAlertNotifyID", SqlDbType.Bit);
                //param[6].Value = iPkAlertNotifyID;
                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                if (param[4].Value.ToString () == "1") {
                    Result = "Save successful";
                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "SaveAlertNotifyUsers()", ex.Message  + ex.StackTrace);
                Result = "Internal Execution Error" + ex.Message;
            }

            return Result;
        }

        #endregion  // new one

        public void PutAcknowledge () {
            SqlParameter[] param = new SqlParameter[2];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ipkAlertID", SqlDbType.Int);
                param[1].Value = ipkAlertID;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "AlertPresentView_sp", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "PutAcknowledge()", ex.Message  + ex.StackTrace);
            }

        }
        public string GetPriorityImages () {
            //string ImagePath = ConfigurationManager.AppSettings["PriorityImagePath"].ToString();
            string ImageCss = String.Empty;
            if (ifkPriorityID == 1) {
                ImageCss = "red-icon";
            } else if (ifkPriorityID == 2) {
                ImageCss = "darkred-icon";
            } else if (ifkPriorityID == 3) {
                ImageCss = "yello-icon";
            }

            //ImageName = ImagePath + ImageName;
            return ImageCss;
        }

        public string GetAlertPriorityCss () {
            string ImageCss = String.Empty;
            if (ifkPriorityID == 1) {
                ImageCss = "alertHigh";
            } else if (ifkPriorityID == 2) {
                ImageCss = "alertMedium";
            } else if (ifkPriorityID == 3) {
                ImageCss = "alertLow";
            }
            return ImageCss;
        }
        public string GetZoneName () {
            string Result = String.Empty;
            if (bZoneOverspeed) {
                vZone = "Zone Overspeed";
            } else if (bZoneEnterOrExit) {
                vZone = "Entered the";
            } else if (!bZoneEnterOrExit) {
                vZone = "Exits the";
            } else {
                vZone = "UnName";
            }

            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet ();
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ipkGeoMID", SqlDbType.Int);
                param[1].Value = ifkGeoMID;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Geofence", param);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0)) {
                    var vGeofenceTypeName = "";

                    if (Convert.ToInt32 (ds.Tables[0].Rows[0]["ZoneTypeId"]) == 0) {
                        vGeofenceTypeName = "Location";
                    } else if (Convert.ToInt32 (ds.Tables[0].Rows[0]["ZoneTypeId"]) == 1) {
                        vGeofenceTypeName = "No-Go";
                    } else if (Convert.ToInt32 (ds.Tables[0].Rows[0]["ZoneTypeId"]) == 2) {
                        vGeofenceTypeName = "Keep-In";
                    } else if (Convert.ToInt32 (ds.Tables[0].Rows[0]["ZoneTypeId"]) == 3) {
                        vGeofenceTypeName = "Route";
                    } else if (Convert.ToInt32 (ds.Tables[0].Rows[0]["ZoneTypeId"]) == 4) {
                        vGeofenceTypeName = "Proximity";
                    }

                    Result = vZone + " " + (vGeofenceTypeName == "" ? "Uknown" : vGeofenceTypeName) + " zone: " + (ds.Tables[0].Rows[0]["vGeoName"].ToString () == "" ? "Uknown" : ds.Tables[0].Rows[0]["vGeoName"].ToString ());
                } else {
                    Result = vZone + " " + "Uknown" + " zone: " + "Not available";
                }
                vZone = Result;

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetZoneName()", ex.Message  + ex.StackTrace);
                vZone = "Internal execution error" + ex.Message;
            }

            return vZone;

        }

        public string AlertNotificationListNew () {
            StringBuilder sb = new StringBuilder ();
            string statusText = "Active";
            try {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 6;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[1].Value = ipkAddAlertID;

                param[2] = new SqlParameter ("@ifkCompanyID", SqlDbType.Int);
                param[2].Value = ifkCompanyID;

                param[3] = new SqlParameter ("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserId;

                DataSet ds = new DataSet ();
                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) {
                    sb.Append ("<div>");
                    sb.Append ("<P class='position-con-title' data-localize='al_AlertName'>ALERT NAME & OVERVIEW</P>");
                    sb.Append ("</div>");

                    foreach (DataRow dr in ds.Tables[0].Rows) {

                        sb.Append ("<div class='position-notification1' id='divAlertNotification-" + Convert.ToInt32 (dr["ipkAlertID"]) + "' >");
                        sb.Append ("<table width='100%' cellspacing='0' cellpadding='0' align='left'>");

                        sb.Append ("<tr>");

                        if (Convert.ToBoolean (dr["bStatus"]) == true) {
                            statusText = "Inactive";
                            sb.Append ("<td width='40' valign='top' align='left'>");

                            //sb.Append("<img align='top' style='margin: -8px 0 0 3px;' src='../images/online-Alert.png'>");
                            string alertImageCss = ReturnImageLarge (Convert.ToInt32 (dr["vPriority"]));
                            sb.Append ("<span align='top' style='margin: 0px 0 0 0px;' class='img_status " + alertImageCss + "'></span>");

                            sb.Append ("</td>");

                            sb.Append ("<td>");
                            sb.Append ("<table width='100%' cellspacing='0' cellpadding='0'>");
                            sb.Append ("<tr>");
                            sb.Append ("<td width='555'>");
                            sb.Append ("<p class='position-overspeeding' ");
                            sb.Append (" onclick='return GetAlertNotificationDetails(" + Convert.ToInt32 (dr["ipkAlertID"]) + "," + Convert.ToInt32 (dr["ifkCommonEventLookupID"]) + "," + Convert.ToInt32 (_wlt_AppConfig.ShowNextDataCount) + ");'>");
                            sb.Append (dr["vAlertName"]);
                            sb.Append ("</p>");
                            sb.Append ("</td>");
                        } else {
                            statusText = "Active";
                            sb.Append ("<td width='40' valign='top' align='left'>");
                            sb.Append ("<img align='top' style='margin: 0px 0 0 0px;' src='../images/grey-icon.png'>");
                            sb.Append ("</td>");

                            sb.Append ("<td>");
                            sb.Append ("<table width='100%' cellspacing='0' cellpadding='0'>");
                            sb.Append ("<tr>");
                            sb.Append ("<td width='555'>");
                            sb.Append ("<p class='Alert_falseStatus' ");
                            sb.Append ("onclick='return GetAlertNotificationDetails(" + Convert.ToInt32 (dr["ipkAlertID"]) + "," + Convert.ToInt32 (dr["ifkCommonEventLookupID"]) + "," + Convert.ToInt32 (_wlt_AppConfig.ShowNextDataCount) + ");'>");
                            sb.Append (dr["vAlertName"]);
                            sb.Append ("</p>");
                            sb.Append ("</td>");
                        }

                        sb.Append ("<td valign='middle'  width='32%' >");

                        sb.Append ("<div class='PopupExtra alertsAction' style='/*position:absolute;*/'  onclick='return OpenActionPopup(" + Convert.ToInt32 (dr["ipkAlertID"]) + ",\"alert\");' >");

                        sb.Append ("<i class='icon-options'></i>");

                        sb.Append ("<div id='divAlertAction-" + Convert.ToInt32 (dr["ipkAlertID"]) + "' class='ConfigureAlertNotificationPopup PopupExtra' style='display: none;'>");
                        sb.Append ("<div id='Div2'>");
                        sb.Append ("<table border='0' cellpadding='0' cellspacing='0' style='width: 100%;'>");
                        sb.Append ("<tr>");

                        sb.Append ("<td >");
                        sb.Append ("<img alt='' src='../Images/edit-icon.png' style='float: left; vertical-align: middle' />");
                        sb.Append ("</td>");

                        sb.Append ("<td>");
                        sb.Append ("<p class='popu_operation'  onclick='return GetEditableDataBackToPopup(" + Convert.ToInt32 (dr["ipkAlertID"]) + ",\"Edit\");' data-localize='al_EditAlert'> Edit Alert</p>");
                        sb.Append ("</td>");

                        sb.Append ("</tr>");

                        sb.Append ("<tr>");

                        sb.Append ("<td >");
                        if (statusText == "Active") {
                            sb.Append ("<img alt='' src='../Images/tickSmall.png'  style='float: left; vertical-align: middle' />");
                        } else {
                            sb.Append ("<img alt='' src='../Images/cross-icon.png'  style='float: left; vertical-align: middle' />");
                        }
                        sb.Append ("</td>");

                        sb.Append ("<td>");
                        sb.Append ("<p class='popu_operation' onclick='return Edit_Status_Delete_NewAlert(" + Convert.ToInt32 (dr["ipkAlertID"]) + ",\"" + statusText + "\");' data-localize='al_Set" + statusText + "'> Set " + statusText + "</p>");
                        sb.Append ("</td>");

                        sb.Append ("</tr>");

                        sb.Append ("<tr>");

                        sb.Append ("<td >");
                        sb.Append ("<img alt='' src='../Images/delete-icon.png' style='float: left; vertical-align: middle' />");
                        sb.Append ("</td>");

                        sb.Append ("<td>");
                        sb.Append ("<p class='popu_operation' onclick='return Edit_Status_Delete_NewAlert(" + Convert.ToInt32 (dr["ipkAlertID"]) + ",\"Delete\");' data-localize='al_DeleteAlert'> Delete Alert</p>");
                        sb.Append ("</td>");

                        sb.Append ("</tr>");

                        sb.Append ("<tr>");

                        sb.Append ("<td >");
                        sb.Append ("<img alt='' src='../Images/edit-icon.png' style='float: left; vertical-align: middle' />");
                        sb.Append ("</td>");

                        sb.Append ("<td>");
                        sb.Append ("<p class='popu_operation' onclick='return DuplicateAlert(" + Convert.ToInt32 (dr["ipkAlertID"]) + ",\"Edit\");' data-localize='al_DuplicateAlert'> Duplicate Alert</p>");
                        sb.Append ("</td>");

                        sb.Append ("</tr>");

                        sb.Append ("<tr>");

                        sb.Append ("<td colspan='2' >");
                        sb.Append ("<img alt='' src='../Images/line.png'  style='float: left; vertical-align: middle;margin: 4px;' />");
                        sb.Append ("</td>");

                        sb.Append ("</tr>");

                        sb.Append ("<tr>");

                        sb.Append ("<td>");
                        sb.Append ("<img alt='' src='../Images/right-icon.png' style='float: left; vertical-align: middle' />");
                        sb.Append ("</td>");

                        sb.Append ("<td>");
                        sb.Append ("<p style='color: #235C9F;margin: 4px;cursor: pointer;' ");
                        sb.Append (" onclick='return GetAlertNotificationDetails(" + Convert.ToInt32 (dr["ipkAlertID"]) + "," + Convert.ToInt32 (dr["ifkCommonEventLookupID"]) + "," + Convert.ToInt32 (_wlt_AppConfig.ShowNextDataCount) + ");' data-localize='al_ShowResults'> Show Results</p>");
                        sb.Append ("</td>");

                        sb.Append ("</tr>");
                        sb.Append ("</table>");
                        sb.Append ("</div>");
                        sb.Append ("</div>");

                        sb.Append ("</div>");

                        sb.Append ("</td>");
                        sb.Append ("</tr>");

                        sb.Append ("<tr>");
                        sb.Append ("<td width='100%' colspan='2'>");
                        sb.Append ("<div class='teb' style='width:98%;'>");
                        sb.Append ("<ul>");
                        if (Convert.ToBoolean (dr["bAnyVehicle"])) {
                            if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 1) {
                                sb.Append ("<li><a href='#'><span data-localize='al_AnyDriver'>Any Driver</span></a></li>");
                            } else if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 2) {
                                sb.Append ("<li><a href='#'><span data-localize='al_AnyPortable'>Any Portable Device</span></a></li>");
                            } else if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 0) {
                                sb.Append ("<li><a href='#'><span data-localize='al_AnyAsset'>Any Asset</span></a></li>");
                            } else if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 3) {
                                sb.Append ("<li><a href='#'><span data-localize='al_AnyVehicle'>Any Vehicle</span></a></li>");
                            }
                        } else if (Convert.ToInt32 (dr["ifkGroupId_groupBasedAlert"].ToString ()) != 0) {
                            sb.Append ("<li><a href='#'>" + GetGroup_DeviceName (Convert.ToInt32 (dr["ifkGroupId_groupBasedAlert"].ToString ()), "0", Convert.ToInt32 (dr["ifkCompanyId"])) + "</a></li>");
                        } else if ((dr["ifkDeviceID"].ToString ()) != "0") {

                            if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 1) {
                                sb.Append ("<li><a href='#'>" + GetDriverName (dr["ifkDriverId"].ToString ()) + "</a></li>");
                            } else if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 2) {
                                sb.Append ("<li><a href='#'>" + GetGroup_DeviceName (0, dr["ifkDeviceID"].ToString (), Convert.ToInt32 (dr["ifkCompanyId"])) + "</a></li>");
                            } else if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 3) {
                                sb.Append ("<li><a href='#'>" + GetGroup_DeviceName (0, dr["ifkDeviceID"].ToString (), Convert.ToInt32 (dr["ifkCompanyId"])) + "</a></li>");
                            }
                        } else {
                            if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 1) {
                                //sb.Append("<li><a href='#'>Specific Driver</a></li>");
                                sb.Append ("<li><a href='#'>" + GetDriverName (dr["ifkDriverId"].ToString ()) + "</a></li>");
                            } else if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 2) {
                                sb.Append ("<li><a href='#'><span data-localize='al_SpecificPortable'>Specific Portable Device</span></a></li>");
                            } else if (Convert.ToInt32 (dr["ifkTrackerTypeId"]) == 3) {
                                sb.Append ("<li><a href='#'><span data-localize='al_SpeficAsset'>Specific Asset</span></a></li>");
                            }

                        }
                        sb.Append ("|");

                        if (Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 0 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 807 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 808 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 809 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 800 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 801 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 802 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 803 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 804 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 805 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 826 && Convert.ToInt32 (dr["ifkCommonEventLookupID"]) != 827) {
                            EventName = GetEventName (Convert.ToInt32 (dr["ifkCommonEventLookupID"].ToString ()), true);
                            sb.Append ("<li><a href='#'><span data-localize='al_TriggerEvent'>Triggers Event: </span><span data-localize='al_" + EventName + "'>" + EventName + "</span></a></li>");
                        } else if (Convert.ToBoolean (dr["bIsDigitalAlert"])) {
                            string ZoneText = "";

                            ZoneText = GetDigitalEventName (Convert.ToInt32 (dr["ifkDigitalMasterID"]), Convert.ToBoolean (dr["bDigitalState"]));

                            sb.Append ("<li><a href='#'><span data-localize='al_" + ZoneText + "'>Triggers Event: " + ZoneText + "</a></li>");
                        } else {

                            string ZoneText = String.Empty;
                            //if (Convert.ToInt32(dr["ifkCommonEventLookupID"]) == 0)
                            //{
                            IsZoneAvaibale = true;

                            //if (Convert.ToBoolean(dr["bAnyNoGoZone"]))
                            //{
                            //    bAnyNoGoZone = true;
                            //    bAnyKeepInZone = false;
                            //    ZoneText = Convert.ToBoolean(dr["bZoneEnterOrExit"]) == true ? "Enters Any No-Go" : "Exits Any No-Go";
                            //    sb.Append("<li><a href='#'>" + ZoneText + "</a></li>");
                            //}
                            //else if (Convert.ToBoolean(dr["bAnyKeepInZone"]))
                            //{
                            //    bAnyNoGoZone = true;
                            //    bAnyKeepInZone = false;
                            //    ZoneText = Convert.ToBoolean(dr["bZoneEnterOrExit"]) == true ? "Enters Any Keep-In" : "Exits Any Keep-In";
                            //    sb.Append("<li><a href='#'>" + ZoneText + "</a></li>");
                            //}

                            if (Convert.ToBoolean (dr["isAnyZone"]) && Convert.ToInt32 (dr["zoneTypeId"]) == 1) {
                                bAnyNoGoZone = true;
                                bAnyKeepInZone = false;
                                bAnyLocationZone = false;

                                if (Convert.ToInt32 (dr["ifkCommonEventLookupID"]) == 802) {
                                    ZoneText = "Enters Any No-Go";
                                } else {
                                    ZoneText = "Zone Overspeed In Any No-Go";
                                }
                                sb.Append ("<li><a href='#'><span data-localize='al_" + ZoneText + "'>" + ZoneText + "</a></li>");
                            } else if (Convert.ToBoolean (dr["isAnyZone"]) && Convert.ToInt32 (dr["zoneTypeId"]) == 2) {
                                bAnyNoGoZone = true;
                                bAnyKeepInZone = false;
                                bAnyLocationZone = false;

                                if (Convert.ToInt32 (dr["ifkCommonEventLookupID"]) == 805) {
                                    ZoneText = "Exits Any Keep-In";
                                } else {
                                    ZoneText = "Zone Overspeed In Any Keep-In";
                                }
                                sb.Append ("<li><a href='#'><span data-localize='al_" + ZoneText + "'>" + ZoneText + "</a></li>");
                            } else if (Convert.ToBoolean (dr["isAnyZone"]) && Convert.ToInt32 (dr["zoneTypeId"]) == 0) {
                                bAnyLocationZone = true;
                                bAnyNoGoZone = false;
                                bAnyKeepInZone = false;

                                ZoneText = "Zone Overspeed In Any Location";
                                sb.Append ("<li><a href='#'><span data-localize='al_" + ZoneText + "'>" + ZoneText + "</a></li>");
                            } else if (Convert.ToInt32 (dr["ifkZoneID"]) > 0) {
                                clsAlert objAlert = new clsAlert ();
                                objAlert.Operation = 18;

                                objAlert.ifkGeoMID = Convert.ToInt32 (dr["ifkZoneID"]);
                                objAlert.bZoneEnterOrExit = Convert.ToBoolean (dr["bZoneEnterOrExit"]);

                                if (Convert.ToInt32 (dr["ifkCommonEventLookupID"]) == 807 || Convert.ToInt32 (dr["ifkCommonEventLookupID"]) == 808 || Convert.ToInt32 (dr["ifkCommonEventLookupID"]) == 809 || Convert.ToInt32 (dr["ifkCommonEventLookupID"]) == 810) {
                                    objAlert.bZoneOverspeed = true;
                                } else {
                                    objAlert.bZoneOverspeed = false;
                                }
                                ZoneText = objAlert.GetZoneName ();
                                //ZoneText = GetEventName(Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString()), true);
                                bAnyNoGoZone = false;
                                bAnyKeepInZone = false;
                                sb.Append ("<li><a href='#'><span data-localize='al_" + ZoneText.Split (':') [0] + "'>" + ZoneText.Split (':') [0] + "</span>" + ZoneText.Split (':') [1] + "</a></li>");
                            }
                            //else if (Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 0 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 807 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 808 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 809 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 810 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 800 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 801 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 802 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 803 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 804 && Convert.ToInt32(dr["ifkCommonEventLookupID"]) != 805)
                            //{
                            //    EventName = GetEventName(Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString()), true);
                            //    //sb.Append("<li><a href='#'> Triggered Event: " + EventName + "</a></li>");
                            //    ZoneText = "Triggers Event: " + EventName;
                            //}
                            else {
                                sb.Append ("<li><a href='#'><span data-localize='al_Uknown'>Unknown</span></a></li>");
                            }
                            //}

                        }

                        //sb.Append("|");
                        param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                        param[0].Value = 7;

                        param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                        param[1].Value = Convert.ToInt32 (dr["ipkAlertID"]);

                        DataSet dsTime = new DataSet ();
                        dsTime = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);

                        string TruncatedContent = "";

                        string bSendSms;
                        string notify;
                        string bSendEmail = "";
                        string bOnScreenNotification;

                        if (dsTime.Tables.Count > 0) {

                            string OrigionalContents = "";
                            //if (dsTime.Tables[0].Rows.Count > 0)
                            //{
                            //    foreach (DataRow drTime in dsTime.Tables[0].Rows)
                            //    {
                            //        if (Convert.ToBoolean(drTime["bInsideofHours"]) == true)
                            //        {
                            //            OrigionalContents = OrigionalContents + " Between " + drTime["tStartsTime"].ToString() + " - " + drTime["tEndsTime"].ToString() + " ,";
                            //        }
                            //        else
                            //        {
                            //            OrigionalContents = OrigionalContents + " Not Between " + drTime["tStartsTime"].ToString() + " - " + drTime["tEndsTime"].ToString() + " ,";
                            //        }
                            //    }
                            //    if (!String.IsNullOrEmpty(OrigionalContents))
                            //        OrigionalContents = OrigionalContents.Substring(0, OrigionalContents.Length - 1);

                            //    if (OrigionalContents.Length > 20)
                            //    {
                            //        TruncatedContent = OrigionalContents.Substring(0, 20) + "...";
                            //        sb.Append("<li><a href='#' title='" + OrigionalContents + "'>" + TruncatedContent + "</a></li>");
                            //    }
                            //    else
                            //    {
                            //        sb.Append("<li><a href='#' title='" + OrigionalContents + "'>" + OrigionalContents + "</a></li>");
                            //    }
                            //    sb.Append("|");
                            //}

                            OrigionalContents = "";

                            TruncatedContent = "";
                            bSendSms = "";
                            notify = "";
                            bool showSms = false;
                            bool showEmail = true;
                            bool showOnscreen = false;

                            if (dsTime.Tables[2].Rows.Count > 0) {

                                bool showComma = false;

                                foreach (DataRow drTime in dsTime.Tables[2].Rows) {
                                    OrigionalContents = drTime["vName"].ToString ();
                                    bSendSms = drTime["bSendSms"].ToString ();
                                    bOnScreenNotification = drTime["bOnScreenNotification"].ToString ();
                                    bSendEmail = drTime["bSendEmail"].ToString ();
                                    if (bSendEmail.Equals ("True")) {
                                        if (showEmail) {
                                            sb.Append (" | ");
                                            sb.Append ("<li><a href='#' title='" + OrigionalContents + "'>");
                                            sb.Append ("Email: ");

                                        }

                                        showEmail = false;

                                        if (showComma == true) {
                                            sb.Append (", ");
                                        }
                                        sb.Append (OrigionalContents);
                                        showComma = true;

                                    }

                                    if (bSendSms.Equals ("True")) {
                                        showSms = true;
                                    }
                                    if (bOnScreenNotification.Equals ("True")) {
                                        showOnscreen = true;
                                    }

                                    sb.Append ("</a></li>");

                                }

                                if (showSms) {
                                    sb.Append ("<li><a href='#' title='" + OrigionalContents + "'>");
                                    sb.Append (" | ");
                                    sb.Append ("SMS: ");

                                    showComma = false;

                                    foreach (DataRow drTime in dsTime.Tables[2].Rows) {

                                        OrigionalContents = drTime["vName"].ToString ();
                                        bSendSms = drTime["bSendSms"].ToString ();

                                        if (bSendSms.Equals ("True")) {

                                            if (showComma == true) {
                                                sb.Append (", ");
                                            }
                                            sb.Append (OrigionalContents);
                                            showComma = true;
                                        }

                                        sb.Append ("</a></li>");

                                    }

                                }

                                if (showOnscreen) {
                                    sb.Append ("<li><a href='#' title='" + OrigionalContents + "'>");
                                    sb.Append (" | ");
                                    sb.Append ("Onscreen: ");

                                    showComma = false;

                                    foreach (DataRow drTime in dsTime.Tables[2].Rows) {

                                        OrigionalContents = drTime["vName"].ToString ();
                                        bOnScreenNotification = drTime["bOnScreenNotification"].ToString ();

                                        if (bOnScreenNotification.Equals ("True")) {

                                            if (showComma == true) {
                                                sb.Append (", ");
                                            }
                                            sb.Append (OrigionalContents);
                                            showComma = true;
                                        }

                                        sb.Append ("</a></li>");

                                    }

                                }
                            }
                        }
                        sb.Append ("</ul>");
                        sb.Append ("</div>");
                        sb.Append ("</td>");

                        sb.Append ("</tr>");
                        sb.Append ("</table>");
                        sb.Append ("</td>");
                        sb.Append ("</tr>");
                        sb.Append ("</table>");
                        sb.Append ("</div>");
                    }
                } else {
                    sb.Append ("<div>");
                    sb.Append ("<P class='position-con-title' data-localize='al_NoDataAvailble'>NO DATA AVAILABLE</P>");
                    sb.Append ("</div>");
                }
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "AlertNotificationList()", ex.Message  + ex.StackTrace);
                sb.Append ("Error : " + ex.Message.ToString ());
            }
            return sb.ToString ();
        } // use this one from date 13-Dec-2013

        public DataSet getAlertsForReports () {

            DataSet ds = new DataSet ();
            string statusText = "Active";
            try {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 6;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[1].Value = ipkAddAlertID;

                param[2] = new SqlParameter ("@ifkCompanyID", SqlDbType.Int);
                param[2].Value = ifkCompanyID;

                param[3] = new SqlParameter ("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserId;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "getAlertsForReports()", ex.Message  + ex.StackTrace);
                vZone = "Internal execution error" + ex.Message;

            }
            return ds;
        }

        public string AlertNotificationDetails (string GridView, string TimeZone, int count, clsRegistration objRegisteration) {
            StringBuilder sb = new StringBuilder ();
            try {
                SqlParameter[] param = new SqlParameter[8];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[1].Value = ipkAddAlertID;

                param[2] = new SqlParameter ("@FilterCode", SqlDbType.Int);
                param[2].Value = FilterCode;

                param[3] = new SqlParameter ("@GridView", SqlDbType.NVarChar);
                param[3].Value = GridView;

                param[4] = new SqlParameter ("@ifkCompanyId", SqlDbType.Int);
                param[4].Value = ifkCompanyID;

                param[5] = new SqlParameter ("@MinusDays", SqlDbType.Int);
                param[5].Value = count;

                param[6] = new SqlParameter ("@OrigionalCountValue", SqlDbType.Int);
                param[6].Value = Convert.ToInt32 (_wlt_AppConfig.ShowNextDataCount);

                param[7] = new SqlParameter ("@ifkUserID", SqlDbType.Int);
                param[7].Value = ifkUserId;

                DataSet ds = new DataSet ();

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);

                string TimeZoneID = String.Empty;

                TimeZoneID = objRegisteration.vTimeZoneID;

                if (GridView.ToLower () == "presenter") {
                    #region Preseneter View

                    sb.Append ("<div style='padding:10px;padding-top:0;'><div class='position-right-title'>");
                    sb.Append ("<span class='error-icon'></span>");
                    sb.Append ("<p class='filtering' data-localize='al_ShowAll'>" + ds.Tables[2].Rows[0]["vAlertName"].ToString () + "</p>"); //2

                    if (ds.Tables[2].Rows[0]["vAlertName"].ToString () != "Showing all alerts") {
                        sb.Append ("<p class='filtering1'><a href='#' onclick='return RemoveFilter(\"presenter\"," + ipkAddAlertID + ");'><span data-localize='al_Remove'>Remove</span></a></p>");
                    }
                    sb.Append ("</div></div>");

                    sb.Append ("  <div class='right-con'  id='divAlertNotificationDetails-Presenter' style='margin: 0 15px 10px 8px;'>");

                    string CurrentDateTime = "";
                    string DateTime_24hrs = "";
                    string DateToday = UserSettings.ConvertUTCDateTimeToLocalDateTime (DateTime.UtcNow, TimeZoneID);

                    if (count == 0) {
                        DateTime_24hrs = DateTime.UtcNow.ToString ("yyyy-MM-dd HH:mm:ss");
                    } else {
                        DateTime_24hrs = DateTime.UtcNow.AddDays (count).ToString ("yyyy-MM-dd HH:mm:ss");
                    }

                    DateTime_24hrs = UserSettings.ConvertUTCDateTimeToLocalDateTime (Convert.ToDateTime (DateTime_24hrs), TimeZoneID);
                    CurrentDateTime = Convert.ToDateTime (DateTime_24hrs).AddHours (24).ToString ("yyyy-MM-dd HH:mm:ss tt");

                    string _dateDisplay = "";

                    if (count < 0) {
                        DateTime UserDateTime = Convert.ToDateTime (UserSettings.ConvertUTCDateTimeToLocalDateTime (DateTime.UtcNow, TimeZoneID)).AddDays (count);

                        _dateDisplay = UserDateTime.ToString ("dddd-d MMMM yyyy");
                    } else {
                        _dateDisplay = Convert.ToDateTime (UserSettings.ConvertUTCDateTimeToLocalDateTime (DateTime.UtcNow, TimeZoneID)).ToString ("dddd-d MMMM yyyy");

                    }

                    if (Convert.ToDateTime (DateToday).ToString ("dddd-d MMMM yyyy") == _dateDisplay) {
                        sb.Append ("<p class='tebbar-title7 col-xs-9' style='padding-left:0 !important;'><strong data-localize='ai_Today'>Today</strong> - <span data-localize='admin_" + _dateDisplay.Split ('-') [0] + "'>" + _dateDisplay.Split ('-') [0] + "</span>" + "-" + _dateDisplay.Split ('-') [1].Split (' ') [0] + "&nbsp;<span data-localize='admin_" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "'>" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "</span>" + "&nbsp;" + _dateDisplay.Split ('-') [1].Split (' ') [2] + "</p>");
                    } else if (Convert.ToDateTime (DateToday).AddDays (-1).ToString ("dddd-d MMMM yyyy") == _dateDisplay) {
                        sb.Append ("<p class='tebbar-title7 col-xs-9' style='padding-left:0 !important;'><strong data-localize='ai_Yesterday'>Yesterday</strong> - <span data-localize='admin_" + _dateDisplay.Split ('-') [0] + "'>" + _dateDisplay.Split ('-') [0] + "</span>" + "-" + _dateDisplay.Split ('-') [1].Split (' ') [0] + "&nbsp;<span data-localize='admin_" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "'>" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "</span>" + "&nbsp;" + _dateDisplay.Split ('-') [1].Split (' ') [2] + "</p>");
                    } else {
                        sb.Append ("<p class='tebbar-title7 col-xs-9' style='padding-left:0 !important;'><span data-localize='admin_" + _dateDisplay.Split ('-') [0] + "'>" + _dateDisplay.Split ('-') [0] + "</span>" + "-" + _dateDisplay.Split ('-') [1].Split (' ') [0] + "&nbsp;<span data-localize='admin_" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "'>" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "</span>" + "&nbsp;" + _dateDisplay.Split ('-') [1].Split (' ') [2] + "</p>");
                    }

                    sb.Append ("<div id='alertsPresenterBtns' class='col-xs-3'>");
                    if (count < 0) {
                        sb.Append ("<input type='button' id='btnShowNextXDays-PresenterTop' value='>' title='Show Next Day' onclick='return ShowMainAlertsToNext24hrs(" + ipkAddAlertID + "," + FilterCode + ");' class='button buttonSmall' style='float:right;margin-left:10px;' />");
                    } else {
                        sb.Append ("<input type='button' disabled style='text-shadow: none;margin-left:10px;float:right;' value='>' id='btnShowNextXDays-PresenterTop' value='>' title='Show Next Day' onclick='return ShowMainAlertsToNext24hrs(" + ipkAddAlertID + "," + FilterCode + ");' class='button buttonSmall buttonDisabled'   />");
                    }

                    sb.Append ("<input type='button' id='btnShowPrevXDays-PresenterTop' value='<' title='Show Previous Day' onclick='return ShowMainAlertsToPrev24hrs(" + ipkAddAlertID + "," + FilterCode + ");' class='button buttonSmall' style='float:right;' />");
                    sb.Append ("</div>");
                    sb.Append ("<p class='col-xs-12' style='width:100%;margin-top: 5px;'></p>");

                    if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)) {

                        //string strPrevious = sb.ToString();
                        bool _blnExists = false;

                        if (ds.Tables[0].Rows.Count > 0) {
                            DataTable orders = ds.Tables[1];

                            IEnumerable<DataRow> query = (from order in orders.AsEnumerable () orderby order.Field<DateTime> ("dt") descending select order);

                            foreach (DataRow dr in ds.Tables[0].Rows) {

                                //DateTime DisplayDate = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(dr["dAge"]), "UTC", TimeZone));

                                DateTime DisplayDate = Convert.ToDateTime (UserSettings.ConvertUTCDateTimeToLocalDateTime (Convert.ToDateTime (dr["dAge"]), TimeZoneID));

                                if (_dateDisplay == DisplayDate.ToString ("dddd-d MMMM yyyy")) {
                                    _blnExists = true;
                                    sb.Append ("<div class='alertRows'>");
                                    sb.Append ("<div class='col-xs-11'><p class='time' >" + DisplayDate.ToString ("HH") + ":" + DisplayDate.ToString ("mm") + "<span style='font-size:10px'>:" + DisplayDate.ToString ("ss") + "</span>" + "</p></div>");
                                    sb.Append ("<div class='col-xs-1'>");

                                    //if (Convert.ToBoolean(Convert.ToInt16(dr["vIsComment"])))
                                    //{
                                    //sb.Append("<img id='imgPresenterComment-" + dr["ipkAlertID"] + "' src='../images/icon-o.png' style='cursor: pointer;' alt='' onclick='return ShowAlertComments(" + dr["ipkAlertID"] + ");' />");
                                    //}
                                    //else
                                    //{
                                    sb.Append ("<span id='imgPresenterComment-" + dr["ipkAlertID"] + "' style='cursor: pointer;float:right;position:relative;top: 2px;' onclick='return ShowAlertComments(\"" + Convert.ToInt32 (dr["ifkAlertID"]) + "\",\"" + Convert.ToInt32 (dr["ipkAlertID"]) + "\");'><i class='icon-bubble icons'></i></span>");
                                    //}

                                    //if (Convert.ToBoolean(Convert.ToInt16(dr["vIsacknowledged"])))
                                    //{
                                    //sb.Append("<img  id='imgPresenterAcknoledgement-" + dr["ipkAlertID"] + "'  style='margin:0 0 0 5px;cursor: pointer;' src='../images/online-icon1.png' onclick='return SaveComment(\"M\"," + dr["ipkAlertID"] + ");' />");
                                    //}
                                    //else
                                    //{
                                    //sb.Append("<img   id='imgPresenterAcknoledgement-" + dr["ipkAlertID"] + "' style='margin:0 0 0 5px;cursor: pointer;' src='../images/offline-icon1.png' onclick='return SaveComment(\"M\"," + dr["ipkAlertID"] + ");' />");
                                    //}

                                    sb.Append ("</div>");
                                    string _strAlert = Convert.ToString (dr["vAlertName"]);
                                    if (_strAlert == "") {
                                        if (Convert.ToInt32 (dr["bZoneEnterOrExit"]) > 0) {
                                            _strAlert = "Entered";
                                        } else {
                                            _strAlert = "Exited";
                                        }

                                        if (Convert.ToBoolean (dr["isAnyZone"]) && Convert.ToInt32 (dr["zoneTypeId"]) == 1) {
                                            _strAlert += " NoGoZone";
                                        } else if (Convert.ToBoolean (dr["isAnyZone"]) && Convert.ToInt32 (dr["zoneTypeId"]) == 2) {
                                            _strAlert += " KeepInZone";
                                        } else if (Convert.ToBoolean (dr["isAnyZone"]) && Convert.ToInt32 (dr["zoneTypeId"]) == 0) {
                                            _strAlert += " AnyLocationZone";
                                        }
                                    }

                                    if (_strAlert.Split (' ').Length > 4 && _strAlert.Split (' ') [4] == "charging") {
                                        string _strAlert1 = _strAlert.Substring (0, 40);
                                        string _strAlert2 = " ";

                                        if (_strAlert.Split (' ') [3] == "stopped") {

                                        } else {
                                            _strAlert2 = _strAlert.Split (' ') [5];

                                        }

                                        sb.Append ("<div class='col-xs-12'><p><span data-localize='al_Asset'>Asset</span> <a onclick='return GetDeviceContact(\"" + Convert.ToString (dr["vpkDeviceID"]) + "\",\"" + Convert.ToString (dr["vDeviceName"]) + "\");'>" + Convert.ToString (dr["vDeviceName"]) + "</a> <span data-localize='al_" + _strAlert1 + "'>" + _strAlert1 + " </span>" + _strAlert2 + " ");

                                    } else {
                                        sb.Append ("<div class='col-xs-12'><p><span data-localize='al_Asset'>Asset</span> <a onclick='return GetDeviceContact(\"" + Convert.ToString (dr["vpkDeviceID"]) + "\",\"" + Convert.ToString (dr["vDeviceName"]) + "\");'>" + Convert.ToString (dr["vDeviceName"]) + "</a> <span data-localize='al_" + _strAlert + "'>" + _strAlert + " </span>");

                                    }

                                    clsCurrentTrackPoint objclsTrackPoint = new clsCurrentTrackPoint ();
                                    objclsTrackPoint.Heading = Convert.ToInt32 (dr["vHeading"].ToString ());
                                    objclsTrackPoint.vVehicleSpeed = dr["vVehicleSpeed"].ToString ();
                                    objclsTrackPoint.vRoadSpeed = dr["vRoadSpeed"].ToString ();
                                    objclsTrackPoint.EventName = dr["vAlertName"].ToString ();
                                    objclsTrackPoint.bIsIgnitionOn = dr["bIsIgnitionOn"].ToString ();

                                    var _dalTrail = new DAL_TrackerPoint ();

                                    string maparrow = _dalTrail.MapArrow (objclsTrackPoint);

                                    if (Operation == 121) {
                                        sb.Append ("<a class='location_tag' onclick='return ShowCurrentAssetLocation(\"" + Convert.ToString (dr["vLongitude"]) + "\",\"" + Convert.ToString (dr["vLatitude"]) + "\",\"" + maparrow + "\",\"" + Convert.ToString (dr["vDeviceName"]) + "\");'>" + Convert.ToString (dr["vTextMessage"]) + "</a> (" + Convert.ToString (ds.Tables[2].Rows[0]["vAlertName"].ToString ()) + ")</p></div>");

                                    } else {
                                        sb.Append ("<a class='location_tag' onclick='return ShowCurrentAssetLocation(\"" + Convert.ToString (dr["vLongitude"]) + "\",\"" + Convert.ToString (dr["vLatitude"]) + "\",\"" + maparrow + "\",\"" + Convert.ToString (dr["vDeviceName"]) + "\");'>" + Convert.ToString (dr["vTextMessage"]) + "</a> (" + Convert.ToString (dr["vAlertNameDisplay"]) + ")</p></div>");

                                    }

                                    sb.Append ("</div>");
                                }
                            }

                        }

                        sb.Append ("</div>");

                        if (_blnExists == false) {
                            //sb.Append("<p class='tebbar-title3'>Sorry, there are no alerts for " + _dateDisplay + " </p>");
                            //sb.Append("<img src='../images/error-icon.png' alt='' />");
                            sb.Append ("<p class='tebbar-title3' style='padding-left:27px;'><span data-localize='al_SorryNoAlerts'>There are no alerts for </span><span data-localize='admin_" + _dateDisplay.Split ('-') [0] + "'>" + _dateDisplay.Split ('-') [0] + "</span>" + "-" + _dateDisplay.Split ('-') [1].Split (' ') [0] + "&nbsp;<span data-localize='admin_" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "'>" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "</span>" + "&nbsp;" + _dateDisplay.Split ('-') [1].Split (' ') [2] + " </p>");
                        }

                        sb.Append ("<div class='divfooterAlert' id='divfooterAlert-Presenter' style='width:98%;'>");
                        sb.Append ("<table cellpadding='0' cellspacing='0' width='100%' style='margin-bottom:15px;'>");
                        sb.Append ("<tr>");
                        sb.Append ("<td align='center'>");

                        if (count < 0) {
                            sb.Append ("<input type='button' id='btnShowNextXDays-Presenter' value='Next Day >' onclick='return ShowMainAlertsToNext24hrs(" + ipkAddAlertID + "," + FilterCode + ");' class='button buttonSmall' style='float:right;margin-left:10px;' data-localize='al_btnNextDay'/>");
                        } else {
                            sb.Append ("<input type='button' disabled style='border: none !important; text-shadow: none;float:right;margin-left:10px;' value='Next Day >' id='btnShowNextXDays-Presenter' value='Next Day >' onclick='return ShowMainAlertsToNext24hrs(" + ipkAddAlertID + "," + FilterCode + ");' class='button buttonSmall buttonDisabled' data-localize='al_btnNextDay'/>");
                        }

                        sb.Append ("<input type='button' id='btnShowPrevXDays-Presenter' value='< Previous Day' onclick='return ShowMainAlertsToPrev24hrs(" + ipkAddAlertID + "," + FilterCode + ");' class='button buttonSmall' style='float:right;' data-localize='al_btnPreviousDay'/>");

                        sb.Append ("</td>");
                        sb.Append ("</tr>");
                        sb.Append ("</table>");
                        sb.Append ("</div>");
                    } else {

                        //sb.Append("<div class='position-right-title'>");
                        //sb.Append("<img src='../images/error-icon.png' alt='' />");
                        sb.Append ("<p class='tebbar-title3' style='padding-left:27px;'><span data-localize='al_SorryNoAlerts'>There are no alerts for </span><span data-localize='admin_" + _dateDisplay.Split ('-') [0] + "'>" + _dateDisplay.Split ('-') [0] + "</span>" + "-" + _dateDisplay.Split ('-') [1].Split (' ') [0] + "&nbsp;<span data-localize='admin_" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "'>" + _dateDisplay.Split ('-') [1].Split (' ') [1] + "</span>" + "&nbsp;" + _dateDisplay.Split ('-') [1].Split (' ') [2] + " </p>");
                        //sb.Append("</div>");

                        sb.Append ("<div class='divfooterAlert' id='divfooterAlert-Presenter' >");
                        sb.Append ("<table cellpadding='0' cellspacing='0' width='100%' style='margin-bottom:15px;'>");
                        sb.Append ("<tr>");
                        sb.Append ("<td align='center'>");

                        if (count < 0) {
                            sb.Append ("<input type='button' id='btnShowNextXDays-Presenter' value='Next Day >' onclick='return ShowMainAlertsToNext24hrs(" + ipkAddAlertID + "," + FilterCode + ");' class='button buttonSmall' style='float:right;margin-left:10px;' data-localize='al_btnNextDay'/>");
                        } else {
                            sb.Append ("<input type='button' disabled style='border: none !important; text-shadow: none;float:right;margin-left:10px;' value='>' id='btnShowNextXDays-Presenter' value='Next Day >' onclick='return ShowMainAlertsToNext24hrs(" + ipkAddAlertID + "," + FilterCode + ");' class='button buttonSmall buttonDisabled' data-localize='al_btnNextDay'/>");
                        }

                        sb.Append ("<input type='button' id='btnShowPrevXDays-Presenter' value='< Previous Day' onclick='return ShowMainAlertsToPrev24hrs(" + ipkAddAlertID + "," + FilterCode + ");' class='button buttonSmall' style='float:right;' data-localize='al_btnPreviousDay'/>");

                        sb.Append ("</td>");
                        sb.Append ("</tr>");
                        sb.Append ("</table>");
                        sb.Append ("</div>");
                    }

                    #endregion
                } else {
                    #region No View (:))
                    sb.Append ("");
                    #endregion
                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "AlertNotificationDetails()", ex.Message  + ex.StackTrace);
                sb.Append ("Error : " + ex.Message.ToString ());
            }

            return sb.ToString ();
        }
        public string ShowAlertNotificationDetailsForNextXDays (string GridView, string TimeZone, int count) {
            StringBuilder sb = new StringBuilder ();
            try {
                SqlParameter[] param = new SqlParameter[7];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 8;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[1].Value = ipkAddAlertID;

                param[2] = new SqlParameter ("@FilterCode", SqlDbType.Int);
                param[2].Value = FilterCode;

                param[3] = new SqlParameter ("@GridView", SqlDbType.NVarChar);
                param[3].Value = GridView;

                param[4] = new SqlParameter ("@ifkCompanyID", SqlDbType.Int);
                param[4].Value = ifkCompanyID;

                param[5] = new SqlParameter ("@MinusDays", SqlDbType.Int);
                param[5].Value = -count;

                param[6] = new SqlParameter ("@OrigionalCountValue", SqlDbType.Int);
                param[6].Value = Convert.ToInt32 (_wlt_AppConfig.ShowNextDataCount);

                DataSet ds = new DataSet ();

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_AddAlert", param);

                if (GridView.ToLower () == "presenter") {
                    #region Preseneter View

                    if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)) {
                        #region  Data

                        if (ds.Tables[0].Rows.Count > 0) {

                            foreach (DataRow drmain in ds.Tables[1].Rows) {
                                sb.Append ("<table width='90%' cellspacing='0' cellpadding='0' align='center'>");
                                DateTime DisplayManinDate = Convert.ToDateTime (TimeZoneInfo.ConvertTimeBySystemTimeZoneId (Convert.ToDateTime (drmain["dt"]), "UTC", TimeZone));

                                if (DisplayManinDate.ToShortDateString () == DateTime.Now.ToShortDateString ()) {
                                    sb.Append ("<tr>");
                                    sb.Append ("<td><p class='today'><b>Today</b> - " + DisplayManinDate.DayOfWeek + " " + DisplayManinDate.Day + "  " + DisplayManinDate.ToString ("MMMM") + "</p></td>");
                                    sb.Append ("</tr>");

                                    sb.Append ("<tr>");
                                    sb.Append ("<td>");
                                } else if (DisplayManinDate.ToShortDateString () == DateTime.Now.AddDays (-1).ToShortDateString ()) {
                                    sb.Append ("<tr>");
                                    sb.Append ("<td><p class='today'><b>Yesterday</b> - " + DisplayManinDate.DayOfWeek + " " + DisplayManinDate.Day + "  " + DisplayManinDate.ToString ("MMMM") + "</p></td>");
                                    sb.Append ("</tr>");

                                    sb.Append ("<tr>");
                                    sb.Append ("<td>");
                                } else {
                                    sb.Append ("<tr>");
                                    sb.Append ("<td><p class='today'><b>On</b> " + DisplayManinDate.DayOfWeek + " " + DisplayManinDate.Day + "  " + DisplayManinDate.ToString ("MMMM") + " " + DisplayManinDate.Year + "</p></td>");
                                    sb.Append ("</tr>");

                                    sb.Append ("<tr>");
                                    sb.Append ("<td>");
                                }

                                sb.Append ("<table width='100%' cellspacing='0' cellpadding='0'>");
                                foreach (DataRow dr in ds.Tables[0].Rows) {

                                    DateTime DisplayDate = Convert.ToDateTime (TimeZoneInfo.ConvertTimeBySystemTimeZoneId (Convert.ToDateTime (dr["dAge"]), "UTC", TimeZone));
                                    if (DisplayManinDate.ToShortDateString () == DisplayDate.ToShortDateString ()) {
                                        sb.Append ("<tr>");
                                        sb.Append ("<td height='33'><p class='time' >" + DisplayDate.ToString ("HH") + ":" + DisplayDate.ToString ("mm") + "</p></td>");
                                        sb.Append ("<td><p class='time1'>Vehicle <a href='#'  onclick='return GetDeviceContact(\"" + Convert.ToString (dr["vpkDeviceID"]) + "\",\"" + Convert.ToString (dr["vDeviceName"]) + "\");'>" + Convert.ToString (dr["vDeviceName"]) + "</a> " + Convert.ToString (dr["vAlertName"]) + " ");

                                        clsCurrentTrackPoint objclsTrackPoint = new clsCurrentTrackPoint ();
                                        objclsTrackPoint.Heading = Convert.ToInt32 (dr["iHeading"].ToString ());
                                        objclsTrackPoint.vVehicleSpeed = dr["iVehicleSpeed"].ToString ();
                                        objclsTrackPoint.vRoadSpeed = "0";
                                        objclsTrackPoint.EventName = dr["vAlertName"].ToString ();
                                        objclsTrackPoint.bIsIgnitionOn = dr["bIsIgnitionOn"].ToString ();

                                        var _dalrackerPoint = new DAL_TrackerPoint ();

                                        string maparrow = _dalrackerPoint.MapArrow (objclsTrackPoint);

                                        sb.Append ("<a href='#' class='location_tag' onclick='return ShowCurrentAssetLocation(\"" + Convert.ToString (dr["vLongitude"]) + "\",\"" + Convert.ToString (dr["vLatitude"]) + "\",\"" + maparrow + "\",\"" + Convert.ToString (dr["vDeviceName"]) + "\");'>" + Convert.ToString (dr["vTextMessage"]) + "</a> </p></td>");
                                        sb.Append ("<td align='left' width='35'>");

                                        if (Convert.ToBoolean (Convert.ToInt16 (dr["vIsComment"]))) {
                                            sb.Append ("<img id='imgPresenterComment-" + dr["ipkAlertID"] + "' src='../images/icon-o.png' style='cursor: pointer;' alt='' onclick='return ShowComment(" + dr["ipkAlertID"] + ");' />");
                                        } else {
                                            sb.Append ("<span id='imgPresenterComment-" + dr["ipkAlertID"] + "' class='icon-g' style='cursor: pointer;' onclick='return ShowComment(" + dr["ipkAlertID"] + ");'></span>");
                                        }

                                        if (Convert.ToBoolean (Convert.ToInt16 (dr["vIsacknowledged"]))) {
                                            sb.Append ("<img  id='imgPresenterAcknoledgement-" + dr["ipkAlertID"] + "'  style='margin:0 0 0 5px;cursor: pointer;' src='../images/online-icon1.png' onclick='return PutAcknoledgement(" + dr["ipkAlertID"] + ");' />");
                                        } else {
                                            sb.Append ("<img   id='imgPresenterAcknoledgement-" + dr["ipkAlertID"] + "' style='margin:0 0 0 5px;cursor: pointer;' src='../images/offline-icon1.png' onclick='return PutAcknoledgement(" + dr["ipkAlertID"] + ");' />");
                                        }

                                        sb.Append ("</td>");
                                        sb.Append ("</tr>");
                                    }
                                }
                                sb.Append ("</table>");

                                sb.Append ("</td>");
                                sb.Append ("</tr>");
                                sb.Append ("</table>");
                            }
                        }

                        #endregion
                    } else {
                        sb.Append ("");
                    }

                    #endregion
                } else {
                    #region No View (:))
                    sb.Append ("");
                    #endregion
                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "AlertNotificationDetails()", ex.Message  + ex.StackTrace);
                sb.Append ("Error : " + ex.Message.ToString ());
            }

            return sb.ToString ();
        }
        public string Edit_Status_Delete_NewAlert () {
            string Result = "";

            SqlParameter[] param = new SqlParameter[4];
            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[1].Value = ipkAddAlertID;

                param[2] = new SqlParameter ("@bStatus", SqlDbType.Int);
                param[2].Value = bStatus;

                param[3] = new SqlParameter ("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                Result = param[3].Value.ToString ();
                if (Result == "10") {
                    Result = "Delete successful";
                } else if (Result == "11") {
                    Result = "Update successful";
                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "Edit_Status_Delete_NewAlert()", ex.Message  + ex.StackTrace);
                Result = "Internal execution error:" + ex.Message;
            }

            return Result;
        }
        public string ChangeAlertDetailStatus () {
            string Result = "";
            try {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 12;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[1].Value = ipkAddAlertID;

                param[2] = new SqlParameter ("@bStatus", SqlDbType.Bit);
                param[2].Value = bStatus;

                SqlHelper.ExecuteNonQuery (Connectionstring, CommandType.StoredProcedure, "sp_AddAlert", param);
                Result = "Status changed with success.";
            } catch (Exception ex) {
                Result = "error";
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "ChangeAlertStatus()", ex.Message  + ex.StackTrace);
            }
            return Result;
        }

        public List<clsAlert> GetEditableDataBackToPopupNew (clsRegistration objRegistration) {
            string iCreatedBy = "";
            string dCreatedOn = "";
            string iUpdatedBy = "";
            string dLastAccessed = "";

            string TimeZoneID = objRegistration.vTimeZoneID;

            DataSet ds = new DataSet ();
            List<clsAlert> objalertlst = new List<clsAlert> ();
            List<clsAlert> lstTimePeriodContent = new List<clsAlert> ();
            StringBuilder sbSelectAsset = new StringBuilder ();
            StringBuilder sbTriggredEvent = new StringBuilder ();
            StringBuilder sbTimePeriod = new StringBuilder ();
            StringBuilder sbOtherCriteria = new StringBuilder ();
            StringBuilder sbNotify = new StringBuilder ();
            SqlParameter[] param = new SqlParameter[2];
            string Grou_DeviceName = String.Empty;
            string EventName = String.Empty;

            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[1].Value = ipkAddAlertID;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                if (ds.Tables.Count > 0) {

                    #region Alert Main Table Data
                    if (ds.Tables[0].Rows.Count > 0) {
                        if (ds.Tables[5].Rows.Count > 0) {
                            iCreatedBy = Convert.ToString (ds.Tables[5].Rows[0]["iCreatedBy"].ToString ());
                            dCreatedOn = UserSettings.ConvertUTCDateTimeToProperLocalDateTime (Convert.ToDateTime (ds.Tables[5].Rows[0]["dCreatedOn"]), TimeZoneID);

                        }
                        if (ds.Tables[6].Rows.Count > 0) {
                            iUpdatedBy = Convert.ToString (ds.Tables[6].Rows[0]["iUpdatedBy"].ToString ());
                            dLastAccessed = UserSettings.ConvertUTCDateTimeToProperLocalDateTime (Convert.ToDateTime (ds.Tables[6].Rows[0]["dLastAccessed"]), TimeZoneID);
                        }

                        objalertlst.Add (new clsAlert (ds.Tables[0].Rows[0]["vAlertName"].ToString (), Convert.ToInt32 (ds.Tables[0].Rows[0]["ifkCompanyId"].ToString ()), Convert.ToInt32 (ds.Tables[0].Rows[0]["vPriority"].ToString ()), iCreatedBy, dCreatedOn, iUpdatedBy, dLastAccessed));
                    } else {
                        return null;
                    }
                    #endregion

                    #region Alert Details table data
                    if (ds.Tables[1].Rows.Count > 0) {
                        if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkGroupId_groupBasedAlert"].ToString ()) != 0) {
                            Grou_DeviceName = GetGroup_DeviceName (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkGroupId_groupBasedAlert"].ToString ()), "0", 0);
                        } else if ((ds.Tables[1].Rows[0]["ifkDeviceID"].ToString ()) != "0") {
                            if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkTrackerTypeId"].ToString ()) == 1) // for driver
                            {
                                Grou_DeviceName = GetDriverName (ds.Tables[1].Rows[0]["ifkDriverId"].ToString ());
                            } else // person/vehicle
                            {
                                Grou_DeviceName = GetGroup_DeviceName (0, ds.Tables[1].Rows[0]["ifkDeviceID"].ToString (), 0);
                            }
                        }

                        string ZoneText = "";
                        bool IsZoneAvaibale = false;

                        if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 0) {
                            IsZoneAvaibale = true;
                        }

                        if (Convert.ToBoolean (ds.Tables[1].Rows[0]["isAnyZone"]) && Convert.ToInt32 (ds.Tables[1].Rows[0]["zoneTypeId"]) == 1) {
                            bAnyNoGoZone = true;
                            bAnyKeepInZone = false;
                            bAnyLocationZone = false;

                            if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 802) {
                                ZoneText = "Enters Any No-Go";
                            } else {
                                ZoneText = "Zone Overspeed Any No-Go";
                            }
                        } else if (Convert.ToBoolean (ds.Tables[1].Rows[0]["isAnyZone"]) && Convert.ToInt32 (ds.Tables[1].Rows[0]["zoneTypeId"]) == 2) {
                            bAnyNoGoZone = false;
                            bAnyKeepInZone = true;
                            bAnyLocationZone = false;

                            if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 805) {
                                ZoneText = "Exits Any Keep-In";
                            } else {
                                ZoneText = "Zone Overspeed Any Keep-In";
                            }

                        } else if (Convert.ToBoolean (ds.Tables[1].Rows[0]["isAnyZone"]) && Convert.ToInt32 (ds.Tables[1].Rows[0]["zoneTypeId"]) == 0) {
                            bAnyLocationZone = true;
                            bAnyNoGoZone = false;
                            bAnyKeepInZone = false;

                            ZoneText = "Zone Overspeed Any Location";

                        } else if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkZoneID"]) > 0) {
                            clsAlert objAlert = new clsAlert ();
                            objAlert.Operation = 18;
                            //objAlert.vZone = ds.Tables[1].Rows[0]["vZone"].ToString();
                            //  objAlert.vZoneType = ds.Tables[1].Rows[0]["vZoneType"].ToString();
                            objAlert.ifkGeoMID = Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkZoneID"]);

                            //if (Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 0)
                            //{
                            //    objAlert.bZoneEnterOrExit = Convert.ToBoolean(ds.Tables[1].Rows[0]["bZoneEnterOrExit"]);
                            //    objAlert.bZoneOverspeed = false;
                            //}
                            //else
                            //{
                            //    if (Convert.ToBoolean(ds.Tables[1].Rows[0]["bZoneEnterOrExit"]) == true)
                            //    {
                            //        objAlert.bZoneEnterOrExit = Convert.ToBoolean(ds.Tables[1].Rows[0]["bZoneEnterOrExit"]);
                            //        objAlert.bZoneOverspeed = false;
                            //    }
                            //    else
                            //    {
                            //        objAlert.bZoneOverspeed = true;
                            //    }
                            //}

                            objAlert.bZoneEnterOrExit = Convert.ToBoolean (ds.Tables[1].Rows[0]["bZoneEnterOrExit"]);
                            if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 807 || Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 808 || Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 809 || Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 810) {
                                objAlert.bZoneOverspeed = true;
                            } else {
                                objAlert.bZoneOverspeed = false;
                            }

                            ZoneText = objAlert.GetZoneName ();
                            bAnyNoGoZone = false;
                            bAnyKeepInZone = false;
                            bAnyLocationZone = false;
                        } else if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 0 && Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 807 && Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 808 && Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 809) // && Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 800 && Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 801
                        {
                            EventName = GetEventName (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString ()), true);
                            //ZoneText = "Triggered Event 1: " + EventName;
                            ZoneText = EventName;
                        } else if (Convert.ToBoolean (ds.Tables[1].Rows[0]["bIsDigitalAlert"])) {

                            ZoneText = "Event: " + GetDigitalEventName (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkDigitalMasterID"]), Convert.ToBoolean (ds.Tables[1].Rows[0]["bDigitalState"]));

                        } else {
                            ZoneText = "Unknown";
                        }

                        objalertlst.Add (new clsAlert (Convert.ToBoolean (ds.Tables[1].Rows[0]["bAnyVehicle"]), Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkGroupId_groupBasedAlert"].ToString ()), ds.Tables[1].Rows[0]["ifkDeviceID"].ToString (), Grou_DeviceName, Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString ())
                            //, (Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString()) != 0 ? GetEventName(Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString()), true) : "")
                            , (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString ()) != 0 ? ZoneText : ""), "" //Convert.ToString(ds.Tables[1].Rows[0]["vDigitalEvent"])
                            , true // Convert.ToBoolean(ds.Tables[1].Rows[0]["bDigitalEventStatus"])
                            , "" //Convert.ToString(ds.Tables[1].Rows[0]["vAnaloglEvent"])
                            , true //Convert.ToBoolean(ds.Tables[1].Rows[0]["bAnalogEventStatus"])
                            , true // Convert.ToBoolean(ds.Tables[1].Rows[0]["bTemeperatureViolation"])
                            , true // Convert.ToBoolean(ds.Tables[1].Rows[0]["bOverspeed"])
                            , true // Convert.ToBoolean(ds.Tables[1].Rows[0]["bExcessiveIdle"])
                            , "" // Convert.ToString(ds.Tables[1].Rows[0]["vTrip"])
                            , IsZoneAvaibale, ZoneText, Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkTrackerTypeId"].ToString ()), bAnyNoGoZone, bAnyKeepInZone, bAnyLocationZone, Convert.ToBoolean (ds.Tables[1].Rows[0]["bZoneEnterOrExit"]), Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkZoneID"]), ds.Tables[1].Rows[0]["iPercentageTolerance"].ToString () == "" ? 0 : Convert.ToInt32 (ds.Tables[1].Rows[0]["iPercentageTolerance"]), Convert.ToBoolean (ds.Tables[1].Rows[0]["isAnyZone"]), Convert.ToInt32 (ds.Tables[1].Rows[0]["zoneTypeId"].ToString ())
                        ));

                    }
                    #endregion

                    #region Time Period Data
                    if (ds.Tables[2].Rows.Count > 0) {
                        int TimeArrayCount = 0;
                        foreach (DataRow dr in ds.Tables[2].Rows) {
                            string InOutTime = String.Empty;
                            string StartTime = (dr["tStartsTime"].ToString ()).Substring (0, (dr["tStartsTime"].ToString ()).Length - 3);
                            string EndTime = (dr["tEndsTime"].ToString ()).Substring (0, (dr["tEndsTime"].ToString ()).Length - 3);
                            if (Convert.ToBoolean (dr["bInsideofHours"].ToString ())) {
                                InOutTime = "Inside the hours of : ";
                            } else {
                                InOutTime = "Outside the hours of : ";
                            }
                            sbTimePeriod.Append ("<span class='selected_text' style='width: 170px;' id=\"" + (InOutTime + "-" + StartTime + "-and-" + EndTime + "-on-" + dr["vDaysOfWeek"]).ToString ().Replace (" ", "-") + "\">");
                            sbTimePeriod.Append ("<img name='Off' class='img_closeEvent' src='../images/notifyclose.jpg' ");
                            sbTimePeriod.Append (" onclick='RemoveTimePeriods(" + TimeArrayCount + ",\"" + (InOutTime + "-" + StartTime + "-and-" + EndTime + "-on-" + dr["vDaysOfWeek"]).ToString ().Replace (" ", "-") + "\");' /> ");
                            sbTimePeriod.Append ("<a href='#' style='color:#95B22D;width:190px;' ");
                            sbTimePeriod.Append (" onclick='EditTimePeriods(" + TimeArrayCount + ",\"" + Convert.ToBoolean (dr["bInsideofHours"]).ToString ().ToLower () + "\",\"" + StartTime + "\",\"" + EndTime + "\",\"" + dr["vDaysOfWeek"].ToString () + "\",\"" + (InOutTime + "-" + StartTime + "-and-" + EndTime + "-on-" + dr["vDaysOfWeek"]).ToString ().Replace (" ", "-") + "\");' >");
                            sbTimePeriod.Append (InOutTime + " " + StartTime + " and " + EndTime + " on " + dr["vDaysOfWeek"] + "  ");
                            sbTimePeriod.Append ("</a>");

                            sbTimePeriod.Append ("</span>");
                            lstTimePeriodContent.Add (new clsAlert (Convert.ToString (Convert.ToBoolean (dr["bInsideofHours"])), StartTime, EndTime, dr["vDaysOfWeek"].ToString ()));
                            TimeArrayCount++;
                        }
                    }
                    #endregion

                    #region Alert Other Criteria
                    if (ds.Tables[3].Rows.Count > 0) {
                        ClsReport oro = new ClsReport ();

                        foreach (DataRow drOther in ds.Tables[3].Rows) {
                            string onOff = String.Empty;
                            onOff = Convert.ToString (drOther["status"]);
                            sbOtherCriteria.Append ("<span class='selected_text' title='" + drOther["ifkDigitalMasterID"].ToString () + "," + drOther["bEventStatus"].ToString () + "' id='" + drOther["vEventName"].ToString ().Replace (" ", "") + "'>");
                            sbOtherCriteria.Append ("<img onclick='RemoveDigitalEvents(\"" + drOther["vEventName"].ToString ().Replace (" ", "") + "\");' src='../images/notifyclose.jpg' ");
                            sbOtherCriteria.Append (" class='img_closeEvent' name='" + onOff + "'>");
                            sbOtherCriteria.Append (drOther["vEventName"].ToString () + " is " + onOff);
                            sbOtherCriteria.Append ("</span>");
                        }
                        oro.HTML = sbOtherCriteria.ToString ();
                    }

                    #endregion

                    if (ds.Tables[4].Rows.Count > 0) {
                        vUserIDs = "0";
                        foreach (DataRow drMain in ds.Tables[4].Rows) {
                            vUserIDs += "," + Convert.ToString (drMain["ifkUserID"]);
                        }

                        DataSet dsUserRole = new DataSet ();
                        dsUserRole = GetUser_RoleName ();
                        string _strName = "";
                        string _strNotifyWay = "";
                        string _strSendSMS = "";

                        foreach (DataRow drMain in ds.Tables[4].Rows) {

                            foreach (DataRow dr in dsUserRole.Tables[0].Select ("pkUserID=" + Convert.ToString (drMain["ifkUserID"]))) {
                                _strName = Convert.ToString (dr["vName"]);
                            }
                            if (Convert.ToInt16 (drMain["bSendEmail"]) == 1) {
                                _strNotifyWay = "Email";
                                _strSendSMS = "1";
                                sbNotify.Append ("<div class='NotifyContent' id='divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "'><img style='float:left;' src='../images/user.jpg' alt=''><div style='float: right; width:90%; position:relative;'><img onclick='RemoveNotifyContent(\"divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "\");' class='img_notifyClose' src='../images/notifyclose.jpg' alt=''><span class='span_notify' title='User' id='" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strSendSMS + "'>" + _strName + "</span><br><a href='#'>" + _strNotifyWay + "</a></div><div></div></div>");
                            }
                            if (Convert.ToInt16 (drMain["bSendSMS"]) == 1) {
                                _strNotifyWay = "SMS";
                                _strSendSMS = "0";
                                sbNotify.Append ("<div class='NotifyContent' id='divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "'><img style='float:left;' src='../images/user.jpg' alt=''><div style='float: right; width:90%; position:relative;'><img onclick='RemoveNotifyContent(\"divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "\");' class='img_notifyClose' src='../images/notifyclose.jpg' alt=''><span class='span_notify' title='User' id='" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strSendSMS + "'>" + _strName + "</span><br><a href='#'>" + _strNotifyWay + "</a></div><div></div></div>");
                            }

                            if (Convert.ToInt16 (drMain["bOnScreenNotification"]) == 1) {
                                _strNotifyWay = "Onscreen";
                                _strSendSMS = "2";
                                sbNotify.Append ("<div class='NotifyContent' id='divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "'><img style='float:left;' src='../images/user.jpg' alt=''><div style='float: right; width:90%; position:relative;'><img onclick='RemoveNotifyContent(\"divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "\");' class='img_notifyClose' src='../images/notifyclose.jpg' alt=''><span class='span_notify' title='User' id='" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strSendSMS + "'>" + _strName + "</span><br><a href='#'>" + _strNotifyWay + "</a></div><div></div></div>");
                            }

                        }

                        //if (!String.IsNullOrEmpty(ds.Tables[3].Rows[0]["vUserIDs"].ToString()))
                        //{
                        //    vUserIDs = ds.Tables[3].Rows[0]["vUserIDs"].ToString();
                        //}
                        //else
                        //{
                        //    vUserIDs = "0";
                        //}

                        //if (!String.IsNullOrEmpty(ds.Tables[3].Rows[0]["vNotifyWays"].ToString()))
                        //{
                        //    vNotifyWay = ds.Tables[3].Rows[0]["vNotifyWays"].ToString();
                        //}
                        //else
                        //{
                        //    vNotifyWay = "0";
                        //}

                        //if (!String.IsNullOrEmpty(ds.Tables[3].Rows[0]["vRoleIDs"].ToString()))
                        //{
                        //    vRoleIDs = ds.Tables[3].Rows[0]["vRoleIDs"].ToString();
                        //}
                        //else
                        //{
                        //    vRoleIDs = "0";
                        //}

                        //string[] _vUserIDs = vUserIDs.Split(',');
                        //string[] _vNotifyWay = vNotifyWay.Split(',');

                        //DataSet dsUserRole = new DataSet();
                        //dsUserRole = GetUser_RoleName();

                        //int _intCount = 0;
                        //string _strName = "";
                        //while (_vUserIDs.Length > _intCount)
                        //{
                        //    foreach (DataRow dr in dsUserRole.Tables[0].Select("pkUserID=" + Convert.ToString(_vUserIDs[_intCount])))
                        //    {
                        //        _strName = Convert.ToString(dr["vName"]);
                        //    }

                        //    sbNotify.Append("<div class='NotifyContent' id='divNotifyName_" + Convert.ToString(_vUserIDs[_intCount]) + "_" + Convert.ToString(_vNotifyWay[_intCount]) + "'><img style='float:left;' src='../images/user.jpg' alt=''><div style='float: right; width:90%; position:relative;'><img onclick='RemoveNotifyContent(\"divNotifyName_" + Convert.ToString(_vUserIDs[_intCount]) + "_" + Convert.ToString(_vNotifyWay[_intCount]) + "\");' class='img_notifyClose' src='../images/notifyclose.jpg' alt=''><span class='span_notify' title='User' id='" + Convert.ToString(_vUserIDs[_intCount]) + "_" + Convert.ToString(_vNotifyWay[_intCount]) + "'>" + _strName + "</span><br><a href='#'>" + Convert.ToString(_vNotifyWay[_intCount]) + "</a></div><div></div></div>");
                        //    _intCount++;
                        //}

                        ////if (dsUserRole.Tables.Count > 0)
                        ////{
                        ////    if (dsUserRole.Tables[0].Rows.Count > 0)
                        ////    {
                        ////        foreach (DataRow dr in dsUserRole.Tables[0].Rows)
                        ////        {
                        ////            sbNotify.Append("<div class='NotifyContent' id='divNotifyName" + Convert.ToInt32(dr["pkUserID"].ToString()) + "'><img style='float:left;' src='../images/user.jpg' alt=''><div style='float: right; width:90%; position:relative;'><img onclick='RemoveNotifyContent(\"divNotifyName" + Convert.ToInt32(dr["pkUserID"].ToString()) + "\");' class='img_notifyClose' src='../images/notifyclose.jpg' alt=''><span class='span_notify' title='User' id='" + Convert.ToInt32(dr["pkUserID"].ToString()) + "'>" + dr["vName"].ToString() + "</span><br><a href='#'>User</a></div><div></div></div>");
                        ////        }
                        ////    }

                        ////    if (dsUserRole.Tables[1].Rows.Count > 0)
                        ////    {
                        ////        foreach (DataRow dr in dsUserRole.Tables[1].Rows)
                        ////        {
                        ////            sbNotify.Append("<div class='NotifyContent' id='divNotifyTypeName" + Convert.ToInt32(dr["ipkContactTypeID"].ToString()) + "''><img style='float:left;' src='../images/user_role.jpg' alt=''><div style='float: right; width:90%; position:relative;'><img onclick='RemoveNotifyContent(\"divNotifyTypeName" + Convert.ToInt32(dr["ipkContactTypeID"].ToString()) + "\");' class='img_notifyClose' src='../images/notifyclose.jpg' alt=''><span class='span_notify' title='User Role' id='" + Convert.ToInt32(dr["ipkContactTypeID"].ToString()) + "''>" + dr["vTypeName"].ToString() + "</span><br><a href='#'>User Role</a></div><div></div></div>");
                        ////        }
                        ////    }
                        ////}

                    }

                    objalertlst.Add (new clsAlert (lstTimePeriodContent, sbOtherCriteria.ToString (), sbNotify.ToString (), sbTimePeriod.ToString ()));

                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetEditableDataBackToPopup()", ex.Message  + ex.StackTrace);
            }

            return objalertlst;
        } // use from date 13-Dec-2013

        public List<clsAlert> GetEditableAlertDataToPopup (clsRegistration objRegistration) {
            string iCreatedBy = "";
            string dCreatedOn = "";
            string iUpdatedBy = "";
            string dLastAccessed = "";

            string TimeZoneID = objRegistration.vTimeZoneID;

            DataSet ds = new DataSet ();
            List<clsAlert> objalertlst = new List<clsAlert> ();
            List<clsAlert> lstTimePeriodContent = new List<clsAlert> ();
            StringBuilder sbSelectAsset = new StringBuilder ();
            StringBuilder sbTriggredEvent = new StringBuilder ();
            StringBuilder sbTimePeriod = new StringBuilder ();
            StringBuilder sbOtherCriteria = new StringBuilder ();
            StringBuilder sbNotify = new StringBuilder ();
            StringBuilder sbNotifyMe = new StringBuilder ();
            int isSmsNotifyMe = 0;
            int isEmailNotifyMe = 0;
            int isOnScreenMe = 0;

            SqlParameter[] param = new SqlParameter[2];
            string Grou_DeviceName = String.Empty;
            string EventName = String.Empty;

            try {
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.Int);
                param[1].Value = ipkAddAlertID;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                if (ds.Tables.Count > 0) {

                    #region Alert Main Table Data
                    if (ds.Tables[0].Rows.Count > 0) {
                        if (ds.Tables[5].Rows.Count > 0) {
                            iCreatedBy = Convert.ToString (ds.Tables[5].Rows[0]["iCreatedBy"].ToString ());
                            dCreatedOn = UserSettings.ConvertUTCDateTimeToProperLocalDateTime (Convert.ToDateTime (ds.Tables[5].Rows[0]["dCreatedOn"]), TimeZoneID);

                        }
                        if (ds.Tables[6].Rows.Count > 0) {
                            iUpdatedBy = Convert.ToString (ds.Tables[6].Rows[0]["iUpdatedBy"].ToString ());
                            dLastAccessed = UserSettings.ConvertUTCDateTimeToProperLocalDateTime (Convert.ToDateTime (ds.Tables[6].Rows[0]["dLastAccessed"]), TimeZoneID);
                        }

                        objalertlst.Add (new clsAlert (ds.Tables[0].Rows[0]["vAlertName"].ToString (), Convert.ToInt32 (ds.Tables[0].Rows[0]["ifkCompanyId"].ToString ()), Convert.ToInt32 (ds.Tables[0].Rows[0]["vPriority"].ToString ()), iCreatedBy, dCreatedOn, iUpdatedBy, dLastAccessed));
                    } else {
                        return null;
                    }
                    #endregion

                    #region Alert Details table data
                    if (ds.Tables[1].Rows.Count > 0) {
                        if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkGroupId_groupBasedAlert"].ToString ()) != 0) {
                            Grou_DeviceName = GetGroup_DeviceName (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkGroupId_groupBasedAlert"].ToString ()), "0", 0);
                        } else if ((ds.Tables[1].Rows[0]["ifkDeviceID"].ToString ()) != "0") {
                            if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkTrackerTypeId"].ToString ()) == 1) // for driver
                            {
                                Grou_DeviceName = GetDriverName (ds.Tables[1].Rows[0]["ifkDriverId"].ToString ());
                            } else // person/vehicle
                            {
                                Grou_DeviceName = GetGroup_DeviceName (0, ds.Tables[1].Rows[0]["ifkDeviceID"].ToString (), 0);
                            }
                        }

                        string ZoneText = "";
                        bool IsZoneAvaibale = false;

                        if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 0) {
                            IsZoneAvaibale = true;
                        }

                        if (Convert.ToBoolean (ds.Tables[1].Rows[0]["isAnyZone"]) && Convert.ToInt32 (ds.Tables[1].Rows[0]["zoneTypeId"]) == 1) {
                            bAnyNoGoZone = true;
                            bAnyKeepInZone = false;
                            bAnyLocationZone = false;

                            if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 802) {
                                ZoneText = "Enters Any No-Go";
                            } else {
                                ZoneText = "Zone Overspeed Any No-Go";
                            }
                        } else if (Convert.ToBoolean (ds.Tables[1].Rows[0]["isAnyZone"]) && Convert.ToInt32 (ds.Tables[1].Rows[0]["zoneTypeId"]) == 2) {
                            bAnyNoGoZone = false;
                            bAnyKeepInZone = true;
                            bAnyLocationZone = false;

                            if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 805) {
                                ZoneText = "Exits Any Keep-In";
                            } else {
                                ZoneText = "Zone Overspeed Any Keep-In";
                            }

                        } else if (Convert.ToBoolean (ds.Tables[1].Rows[0]["isAnyZone"]) && Convert.ToInt32 (ds.Tables[1].Rows[0]["zoneTypeId"]) == 0) {
                            bAnyLocationZone = true;
                            bAnyNoGoZone = false;
                            bAnyKeepInZone = false;

                            ZoneText = "Zone Overspeed Any Location";

                        } else if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkZoneID"]) > 0) {
                            clsAlert objAlert = new clsAlert ();
                            objAlert.Operation = 18;
                            //objAlert.vZone = ds.Tables[1].Rows[0]["vZone"].ToString();
                            //  objAlert.vZoneType = ds.Tables[1].Rows[0]["vZoneType"].ToString();
                            objAlert.ifkGeoMID = Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkZoneID"]);

                            //if (Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 0)
                            //{
                            //    objAlert.bZoneEnterOrExit = Convert.ToBoolean(ds.Tables[1].Rows[0]["bZoneEnterOrExit"]);
                            //    objAlert.bZoneOverspeed = false;
                            //}
                            //else
                            //{
                            //    if (Convert.ToBoolean(ds.Tables[1].Rows[0]["bZoneEnterOrExit"]) == true)
                            //    {
                            //        objAlert.bZoneEnterOrExit = Convert.ToBoolean(ds.Tables[1].Rows[0]["bZoneEnterOrExit"]);
                            //        objAlert.bZoneOverspeed = false;
                            //    }
                            //    else
                            //    {
                            //        objAlert.bZoneOverspeed = true;
                            //    }
                            //}

                            objAlert.bZoneEnterOrExit = Convert.ToBoolean (ds.Tables[1].Rows[0]["bZoneEnterOrExit"]);
                            if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 807 || Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 808 || Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 809 || Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) == 810) {
                                objAlert.bZoneOverspeed = true;
                            } else {
                                objAlert.bZoneOverspeed = false;
                            }

                            ZoneText = objAlert.GetZoneName ();
                            bAnyNoGoZone = false;
                            bAnyKeepInZone = false;
                            bAnyLocationZone = false;
                        } else if (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 0 && Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 807 && Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 808 && Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 809) // && Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 800 && Convert.ToInt32(ds.Tables[1].Rows[0]["ifkCommonEventLookupID"]) != 801
                        {
                            EventName = GetEventName (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString ()), true);
                            //ZoneText = "Triggered Event 1: " + EventName;
                            ZoneText = EventName;
                        } else if (Convert.ToBoolean (ds.Tables[1].Rows[0]["bIsDigitalAlert"])) {

                            ZoneText = "Event: " + GetDigitalEventName (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkDigitalMasterID"]), Convert.ToBoolean (ds.Tables[1].Rows[0]["bDigitalState"]));

                        } else {
                            ZoneText = "Unknown";
                        }

                        objalertlst.Add (new clsAlert {
                            bAnyVehicle = Convert.ToBoolean (ds.Tables[1].Rows[0]["bAnyVehicle"]),
                                IsAnyDriver = Convert.ToBoolean (ds.Tables[1].Rows[0]["bIsAnyDriver"]),
                                AddAlertDriverID = ds.Tables[1].Rows[0]["ifkDriverId"].ToString () == "" ? 0 : Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkDriverId"]),
                                ifkGroupMID = Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkGroupId_groupBasedAlert"].ToString ()),
                                vpkDeviceID = ds.Tables[1].Rows[0]["ifkDeviceID"].ToString (),
                                Grou_DeviceName = Grou_DeviceName,
                                iTriggeredEventID = Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString ()),
                                EventName = (Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkCommonEventLookupID"].ToString ()) != 0 ? ZoneText : ""),
                                vDigitalEvent = "",
                                bDigitalEventStatus = true,
                                vAnaloglEvent = "",
                                bAnalogEventStatus = true,
                                bTemeperatureViolation = true,
                                bOverspeed = true,
                                bExcessiveIdle = true,
                                vTrip = "",
                                IsZoneAvaibale = IsZoneAvaibale,
                                vZone = ZoneText,
                                iTrackerTypeID = Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkTrackerTypeId"].ToString ()),
                                bAnyNoGoZone = bAnyNoGoZone,
                                bAnyKeepInZone = bAnyKeepInZone,
                                bAnyLocationZone = bAnyLocationZone,
                                bZoneEnterOrExit = Convert.ToBoolean (ds.Tables[1].Rows[0]["bZoneEnterOrExit"]),
                                ifkGeoMID = Convert.ToInt32 (ds.Tables[1].Rows[0]["ifkZoneID"]),
                                iPercentageTolerance = ds.Tables[1].Rows[0]["iPercentageTolerance"].ToString () == "" ? 0 : Convert.ToInt32 (ds.Tables[1].Rows[0]["iPercentageTolerance"]),
                                isAnyZone = Convert.ToBoolean (ds.Tables[1].Rows[0]["isAnyZone"]),
                                zoneTypeId = Convert.ToInt32 (ds.Tables[1].Rows[0]["zoneTypeId"]),
                                AlertAssets = Convert.ToString (ds.Tables[1].Rows[0]["vALertAssets"]),
                                iSensorID = Convert.ToString (ds.Tables[1].Rows[0]["SensorId"]) == "" ? 0 : Convert.ToInt32 (ds.Tables[1].Rows[0]["SensorId"]),
                                AnalogType = Convert.ToString (ds.Tables[1].Rows[0]["AnalogType"]) == "" ? 0 : Convert.ToInt32 (ds.Tables[1].Rows[0]["AnalogType"])
                        });

                    }
                    #endregion

                    #region Time Period Data
                    if (ds.Tables[2].Rows.Count > 0) {
                        //int TimeArrayCount = 0;
                        //foreach (DataRow dr in ds.Tables[2].Rows)
                        //{
                        //    string InOutTime = String.Empty;
                        //    string StartTime = (dr["tStartsTime"].ToString()).Substring(0, (dr["tStartsTime"].ToString()).Length - 3);
                        //    string EndTime = (dr["tEndsTime"].ToString()).Substring(0, (dr["tEndsTime"].ToString()).Length - 3);
                        //    if (Convert.ToBoolean(dr["bInsideofHours"].ToString()))
                        //    {
                        //        InOutTime = "Inside the hours of : ";
                        //    }
                        //    else
                        //    {
                        //        InOutTime = "Outside the hours of : ";
                        //    }
                        //    sbTimePeriod.Append("<span class='selected_text' style='width: 170px;' id=\"" + (InOutTime + "-" + StartTime + "-and-" + EndTime + "-on-" + dr["vDaysOfWeek"]).ToString().Replace(" ", "-") + "\">");
                        //    sbTimePeriod.Append("<img name='Off' class='img_closeEvent' src='../images/notifyclose.jpg' ");
                        //    sbTimePeriod.Append(" onclick='RemoveTimePeriods(" + TimeArrayCount + ",\"" + (InOutTime + "-" + StartTime + "-and-" + EndTime + "-on-" + dr["vDaysOfWeek"]).ToString().Replace(" ", "-") + "\");' /> ");
                        //    sbTimePeriod.Append("<a href='#' style='color:#95B22D;width:190px;' ");
                        //    sbTimePeriod.Append(" onclick='EditTimePeriods(" + TimeArrayCount + ",\"" + Convert.ToBoolean(dr["bInsideofHours"]).ToString().ToLower() + "\",\"" + StartTime + "\",\"" + EndTime + "\",\"" + dr["vDaysOfWeek"].ToString() + "\",\"" + (InOutTime + "-" + StartTime + "-and-" + EndTime + "-on-" + dr["vDaysOfWeek"]).ToString().Replace(" ", "-") + "\");' >");
                        //    sbTimePeriod.Append(InOutTime + " " + StartTime + " and " + EndTime + " on " + dr["vDaysOfWeek"] + "  ");
                        //    sbTimePeriod.Append("</a>");

                        //    sbTimePeriod.Append("</span>");
                        //    lstTimePeriodContent.Add(new clsAlert(Convert.ToString(Convert.ToBoolean(dr["bInsideofHours"])), StartTime, EndTime, dr["vDaysOfWeek"].ToString()));
                        //    TimeArrayCount++;
                        //}

                        TimeRangeConverter converter = new TimeRangeConverter ();
                        AlertTimeRange = converter.ConvertTimeRangeToUserTime (Convert.ToString (ds.Tables[2].Rows[0]["AlertTimeRange"]), TimeZoneID);

                        AlertTimeRange = AlertTimeRange + ":" + (Convert.ToBoolean (ds.Tables[2].Rows[0]["isActive"]) == true ? "1" : "0");

                    }
                    #endregion

                    #region Alert Other Criteria
                    if (ds.Tables[3].Rows.Count > 0) {
                        ClsReport oro = new ClsReport ();

                        foreach (DataRow drOther in ds.Tables[3].Rows) {
                            string onOff = String.Empty;
                            onOff = Convert.ToString (drOther["status"]);
                            sbOtherCriteria.Append ("<span class='selected_text' title='" + drOther["ifkDigitalMasterID"].ToString () + "," + drOther["bEventStatus"].ToString () + "' id='" + drOther["vEventName"].ToString ().Replace (" ", "") + "'>");
                            sbOtherCriteria.Append ("<img onclick='RemoveDigitalEvents(\"" + drOther["vEventName"].ToString ().Replace (" ", "") + "\");' src='../images/notifyclose.jpg' ");
                            sbOtherCriteria.Append (" class='img_closeEvent' name='" + onOff + "'>");
                            sbOtherCriteria.Append (drOther["vEventName"].ToString () + " is " + onOff);
                            sbOtherCriteria.Append ("</span>");
                        }
                        oro.HTML = sbOtherCriteria.ToString ();
                    }

                    #endregion

                    if (ds.Tables[4].Rows.Count > 0) {
                        vUserIDs = "0";
                        foreach (DataRow drMain in ds.Tables[4].Rows) {
                            vUserIDs += "," + Convert.ToString (drMain["ifkUserID"]);
                        }

                        DataSet dsUserRole = new DataSet ();
                        dsUserRole = GetUser_RoleName ();
                        string _strName = "";
                        string _strNotifyWay = "";
                        string _strSendSMS = "";

                        foreach (DataRow drMain in ds.Tables[4].Rows) {

                            foreach (DataRow dr in dsUserRole.Tables[0].Select ("pkUserID=" + Convert.ToString (drMain["ifkUserID"]))) {
                                _strName = Convert.ToString (dr["vName"]);
                            }
                            if (Convert.ToInt16 (drMain["bSendEmail"]) == 1) {
                                if (Convert.ToString (drMain["ifkUserID"]) == objRegistration.pkUserID.ToString ()) {
                                    isEmailNotifyMe = 1;

                                } else {
                                    _strNotifyWay = "Email";
                                    _strSendSMS = "1";
                                    sbNotify.Append ("<div class='NotifyContent' id='divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "'><img style='float:left;' src='../images/user.jpg' alt=''><div style='float: right; width:90%; position:relative;'><img onclick='RemoveNotifyContent(\"divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "\");' class='img_notifyClose' src='../images/notifyclose.jpg' alt=''><span class='span_notify' title='User' id='" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strSendSMS + "'>" + _strName + "</span><br><a href='#'>" + _strNotifyWay + "</a></div><div></div></div>");
                                }

                            }
                            if (Convert.ToInt16 (drMain["bSendSMS"]) == 1) {
                                if (Convert.ToString (drMain["ifkUserID"]) == objRegistration.pkUserID.ToString ()) {
                                    isSmsNotifyMe = 1;
                                } else {
                                    _strNotifyWay = "SMS";
                                    _strSendSMS = "0";
                                    sbNotify.Append ("<div class='NotifyContent' id='divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "'><img style='float:left;' src='../images/user.jpg' alt=''><div style='float: right; width:90%; position:relative;'><img onclick='RemoveNotifyContent(\"divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "\");' class='img_notifyClose' src='../images/notifyclose.jpg' alt=''><span class='span_notify' title='User' id='" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strSendSMS + "'>" + _strName + "</span><br><a href='#'>" + _strNotifyWay + "</a></div><div></div></div>");
                                }

                            }

                            if (Convert.ToInt16 (drMain["bOnScreenNotification"]) == 1) {
                                if (Convert.ToString (drMain["ifkUserID"]) == objRegistration.pkUserID.ToString ()) {
                                    isOnScreenMe = 1;
                                } else {
                                    _strNotifyWay = "Onscreen";
                                    _strSendSMS = "2";
                                    sbNotify.Append ("<div class='NotifyContent' id='divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "'><img style='float:left;' src='../images/user.jpg' alt=''><div style='float: right; width:90%; position:relative;'><img onclick='RemoveNotifyContent(\"divNotifyName_" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strNotifyWay + "\");' class='img_notifyClose' src='../images/notifyclose.jpg' alt=''><span class='span_notify' title='User' id='" + Convert.ToString (drMain["ifkUserID"]) + "_" + _strSendSMS + "'>" + _strName + "</span><br><a href='#'>" + _strNotifyWay + "</a></div><div></div></div>");
                                }

                            }

                        }

                    }

                    sbNotifyMe.Append ("{\"Email\":" + isEmailNotifyMe + "," + "\"SMS\":" + isSmsNotifyMe + "," + "\"OnScreen\":" + isOnScreenMe + "}");

                    objalertlst.Add (new clsAlert (lstTimePeriodContent, sbOtherCriteria.ToString (), sbNotify.ToString (), AlertTimeRange, sbNotifyMe.ToString ()));

                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetEditableDataBackToPopup()", ex.Message  + ex.StackTrace);
            }

            return objalertlst;
        }

        public string ReturnImage (int Priority) {
            string Path = _wlt_AppConfig.PriorityImagePath;

            string Image = String.Empty;
            if (Priority == 1) {
                Image = "y-icon.png";
            } else if (Priority == 2) {
                Image = "o-icon.png";
            } else if (Priority == 3) {
                Image = "r-icon.png";
            }
            Image = Path + Image;
            return Image;
        }

        public string ReturnImageLarge (int Priority) {
            //string Path = System.Configuration.ConfigurationManager.AppSettings["PriorityImagePath"].ToString();
            string ImageCss = String.Empty;

            if (Priority == 1) {
                ImageCss = "red-icon";
            } else if (Priority == 2) {
                ImageCss = "darkred-icon";
            } else if (Priority == 3) {
                ImageCss = "yello-icon";
            }
            //Image = Path + Image;
            return ImageCss;
        }

        public string GetGroup_DeviceName (int groupID, string DeviceID, int ifkCompanyId) {
            DataSet ds = new DataSet ();
            string Group_DeviceName = String.Empty;
            int Operation = 0;
            if (groupID != 0) {
                Operation = 13;
            } else {
                Operation = 14;
            }

            try {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = groupID;

                param[2] = new SqlParameter ("@vpkDeviceID", SqlDbType.NVarChar);
                param[2].Value = DeviceID;

                param[3] = new SqlParameter ("@ifkCompanyId", SqlDbType.Int);
                param[3].Value = ifkCompanyId;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_AddAlert", param);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0)) {
                    Group_DeviceName = ds.Tables[0].Rows[0]["Group_DeviceName"].ToString ();
                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetGroup_DeviceName()", ex.Message  + ex.StackTrace);
            }

            return Group_DeviceName;
        }
        public string GetDriverName (string ipkDriverID) {
            DataSet ds = new DataSet ();
            string DriverName = String.Empty;

            try {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 18;

                param[1] = new SqlParameter ("@ipkDriverID", SqlDbType.NVarChar);
                param[1].Value = ipkDriverID;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0)) {
                    DriverName = AppConfiguration.ToTitleCase (ds.Tables[0].Rows[0]["DriverName"].ToString ());
                } else {
                    DriverName = "Unknown";
                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetDriverName()", ex.Message  + ex.StackTrace);
            }

            return DriverName;
        }
        public string GetEventName (int EventID, bool status) {
            DataSet ds = new DataSet ();
            string EventName = String.Empty;
            string StatusName = String.Empty;

            try {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 15;

                param[1] = new SqlParameter ("@iTriggeredEventID", SqlDbType.Int);
                param[1].Value = EventID;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0)) {
                    //if (Convert.ToInt32(ds.Tables[0].Rows[0]["iStatusCode"].ToString()) == 1)
                    //{
                    //    if (status)
                    //    {
                    //        StatusName = "On";
                    //    }
                    //    else
                    //    {
                    //        StatusName = "Off";
                    //    }
                    //}
                    //else
                    //{
                    //    if (status)
                    //    {
                    //        StatusName = "Start";
                    //    }
                    //    else
                    //    {
                    //        StatusName = "Stop";
                    //    }

                    //}

                    EventName = AppConfiguration.ToTitleCase (ds.Tables[0].Rows[0]["vEventName"].ToString ());
                    //EventName = EventName + " " + StatusName;

                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetGroup_DeviceName()", ex.Message  + ex.StackTrace);
            }

            return EventName;
        }
        public string GetDigitalEventName (int EventID, bool isDigitalStatus) {
            DataSet ds = new DataSet ();
            string EventName = String.Empty;
            string StatusName = String.Empty;

            try {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 126;

                param[1] = new SqlParameter ("@ifkDigitalMasterID", SqlDbType.Int);
                param[1].Value = EventID;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0)) {
                    if (isDigitalStatus) {
                        EventName = AppConfiguration.ToTitleCase (ds.Tables[0].Rows[0]["vName"].ToString ()) + " - " + AppConfiguration.ToTitleCase (ds.Tables[0].Rows[0]["vOnText"].ToString ());
                    } else {
                        EventName = AppConfiguration.ToTitleCase (ds.Tables[0].Rows[0]["vName"].ToString ()) + " - " + AppConfiguration.ToTitleCase (ds.Tables[0].Rows[0]["vOffText"].ToString ());
                    }

                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetGroup_DeviceName()", ex.Message  + ex.StackTrace);
            }

            return EventName;
        }
        public DataSet GetUser_RoleName () {
            DataSet ds = new DataSet ();

            try {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 16;

                param[1] = new SqlParameter ("@vRoleIDs", SqlDbType.NVarChar);
                param[1].Value = vRoleIDs;

                param[2] = new SqlParameter ("@vUserIDs", SqlDbType.NVarChar);
                param[2].Value = vUserIDs;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetUser_RoleName()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        public string GetAlertName (string ipkAlertID) {
            DataSet ds = new DataSet ();
            string AlertName = String.Empty;

            try {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 125;

                param[1] = new SqlParameter ("@ifkAddAlertID", SqlDbType.NVarChar);
                param[1].Value = ipkAlertID;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);
                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0)) {
                    AlertName = AppConfiguration.ToTitleCase (ds.Tables[0].Rows[0]["vAlertName"].ToString ());
                } else {
                    AlertName = "Unknown";
                }

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "GetDriverName()", ex.Message  + ex.StackTrace);
            }

            return AlertName;
        }

        public DataSet CheckSMSEMAILCapability () {
            DataSet ds = new DataSet ();

            try {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 127;

                param[1] = new SqlParameter ("@ifkCompanyID", SqlDbType.Int);
                param[1].Value = ifkCompanyID;

                param[2] = new SqlParameter ("@ifkUserId", SqlDbType.Int);
                param[2].Value = ifkUserId;

                ds = SqlHelper.ExecuteDataset (Connectionstring, CommandType.StoredProcedure, "sp_Alert", param);

            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsAlert.cs", "CheckSMSEMAILCapability()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        public void RollbackAlertData () {
            Operation = 10;
            SaveAlert ();
        }

        #endregion

        public void Dispose () {
            Dispose ();
            GC.Collect ();
            GC.SuppressFinalize (this);
        }
    }
}