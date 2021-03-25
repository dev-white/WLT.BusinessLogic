using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using WLT.DataAccessLayer;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL {
    public class clsTripReplay : IDisposable {


        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();

        private string _vSequenceID;
        private string _vUnitID;
        private string _vGPSDateTime;
        private string _dGPSDateTime;
        private string _vPositionSendingDateTime;
        private string _vLongitude;
        private string _vLatitude;
        private string _vHeading;
        private string _vReportID;
        private string _vOdometer;
        private string _vHDOP;
        private string _vInputStatus;
        private string _vVehicleSpeed;
        private string _vOutputStatus;
        private string _vAnalogInputValue;
        private string _vDriverID;
        private string _vFirstTempSensorValue;
        private string _vSecondTemperatureSensorValue;
        private string _vTextMessage;
        private DateTime _dEnterDate;
        private string _vDeviceName;
        private int _Operation;
        private string _like;
        private string _bIsIgnitionOn;
        private string _vTimeZoneID;
        private int _ifkGroupMID;
        private int _ifkCompanyID;
        private int _ifkUserID;
        private int _iTrackerType;

        private string _DriverPhoto;
        private string _Drivername;
        private long _ipkTripID;
        private string _vRoadSpeed;

        private decimal _nAltitude;

        private string _strStartTime;
        private string _strEndTime;
        private string _iBatteryBackup;
        private double _vAnalog1;
        private double _vAnalog2;
        private bool _isAnalog1Mapped;
        private bool _isAnalog2Mapped;
        private string _Analog1MappedName;
        private string _Analog2MappedName;

        private string _vUnitText1;
        private int _iMinValue1;
        private int _iMaxValue1;
        private string _vUnitText2;
        private int _iMinValue2;
        private int _iMaxValue2;
        private string _vAssetLogo;
        private string _vEventName;

        public Dictionary<string, EL_TripReplay> dictSensorTypes { get; set; }
        public string vUnitText1 { get { return this._vUnitText1; } set { this._vUnitText1 = value; } }
        public int iMinValue1 { get { return this._iMinValue1; } set { this._iMinValue1 = value; } }
        public int iMaxValue1 { get { return this._iMaxValue1; } set { this._iMaxValue1 = value; } }
        public string vUnitText2 { get { return this._vUnitText2; } set { this._vUnitText2 = value; } }
        public int iMinValue2 { get { return this._iMinValue2; } set { this._iMinValue2 = value; } }
        public int iMaxValue2 { get { return this._iMaxValue2; } set { this._iMaxValue2 = value; } }
        public string vEventName { get { return this._vEventName; } set { this._vEventName = value; } }
        public string vAssetLogo { get { return this._vAssetLogo; } set { this._vAssetLogo = value; } }

        public double vAnalog1 {
            get {
                return this._vAnalog1;
            }
            set {
                this._vAnalog1 = value;
            }
        }
        public double vAnalog2 {
            get {
                return this._vAnalog2;
            }
            set {
                this._vAnalog2 = value;
            }
        }

        public bool isAnalog1Mapped {
            get {
                return this._isAnalog1Mapped;
            }
            set {
                this._isAnalog1Mapped = value;
            }
        }
        public bool isAnalog2Mapped {
            get {
                return this._isAnalog2Mapped;
            }
            set {
                this._isAnalog2Mapped = value;
            }
        }
        public string Analog1MappedName {
            get {
                return this._Analog1MappedName;
            }
            set {
                this._Analog1MappedName = value;
            }
        }
        public string Analog2MappedName {
            get {
                return this._Analog2MappedName;
            }
            set {
                this._Analog2MappedName = value;
            }
        }
        public string strStartTime {
            get {
                return this._strStartTime;
            }
            set {
                this._strStartTime = value;
            }
        }

        public string strEndTime {
            get {
                return this._strEndTime;
            }
            set {
                this._strEndTime = value;
            }
        }

        public decimal nAltitude {
            get {
                return this._nAltitude;
            }
            set {
                this._nAltitude = value;
            }
        }
        public string vRoadSpeed {
            get {
                return this._vRoadSpeed;
            }
            set {
                this._vRoadSpeed = value;
            }
        }

        public long ipkTripID {
            get {
                return this._ipkTripID;
            }
            set {
                this._ipkTripID = value;
            }
        }

        public decimal vDistance {
            get {
                return this._vDistance;
            }
            set {
                this._vDistance = value;
            }
        }

        public decimal iAverageSpeed {
            get {
                return this._iAverageSpeed;
            }
            set {
                this._iAverageSpeed = value;
            }
        }

        public int iTrackerType {
            get {
                return this._iTrackerType;
            }
            set {
                this._iTrackerType = value;
            }
        }

        public int ifkUserID {
            get { return _ifkUserID; }
            set { _ifkUserID = value; }
        }

        public int ifkGroupMID {
            get { return _ifkGroupMID; }
            set { _ifkGroupMID = value; }
        }

        public int ifkCompanyID {
            get { return _ifkCompanyID; }
            set { _ifkCompanyID = value; }
        }

        public string bIsIgnitionOn {
            get { return _bIsIgnitionOn; }
            set { _bIsIgnitionOn = value; }
        }
        public string vTimeZoneID {
            get { return _vTimeZoneID; }
            set { _vTimeZoneID = value; }
        }
        public Double TripStopageTime {
            get { return _tripStopageTime; }
            set { _tripStopageTime = value; }
        }
        public string EndLocation {
            get { return _tripEndLocation; }
            set { _tripEndLocation = value; }
        }
        private Double _tripStopageTime;
        public string like { get { return _like; } set { _like = value; } }
        private string _tripEndLocation;
        private int _tripcount;
        private string _totalDistanceForDay;
        private string _login;
        public string login {
            get { return _login; }
            set { _login = value; }
        }

        public int tripcount {
            get { return _tripcount; }
            set { _tripcount = value; }
        }

        public string totalDistanceForDay {
            get { return _totalDistanceForDay; }
            set { _totalDistanceForDay = value; }
        }
        private string _leftMenuDetail;

        public string leftMenuDetail {
            get { return _leftMenuDetail; }
            set { _leftMenuDetail = value; }
        }

        public int Operation {
            get { return _Operation; }
            set { _Operation = value; }
        }
        private int _ipkMSMQTrackId;

        public int IpkMSMQTrackId {
            get { return _ipkMSMQTrackId; }
            set { _ipkMSMQTrackId = value; }
        }
        public string vDeviceName {
            get { return _vDeviceName; }
            set { _vDeviceName = value; }
        }
        private string _vImage;

        public string vImage {
            get { return _vImage; }
            set { _vImage = value; }
        }

        private long _nStartEventInst;
        private long _nEndEventInst;
        private string _vpkDeviceID;
        private int _deviceDbId;
        private string _TripreplayDate;

        public string vpkDeviceID {
            get { return _vpkDeviceID; }
            set { _vpkDeviceID = value; }
        }
        public int deviceDbId {
            get { return _deviceDbId; }
            set { _deviceDbId = value; }
        }

        public string TripreplayDate {
            get { return _TripreplayDate; }
            set { _TripreplayDate = value; }
        }

        public long nEndEventInst {
            get { return _nEndEventInst; }
            set { _nEndEventInst = value; }
        }

        public long nStartEventInst {
            get { return _nStartEventInst; }
            set { _nStartEventInst = value; }
        }

        public string vLongitude {
            get { return _vLongitude; }
            set { _vLongitude = value; }
        }

        public string vLatitude {
            get { return _vLatitude; }
            set { _vLatitude = value; }
        }

        public string vSequenceID {
            get { return _vSequenceID; }
            set { _vSequenceID = value; }
        }

        public string vUnitID {
            get { return _vUnitID; }
            set { _vUnitID = value; }
        }
        public string vGPSDateTime {
            get { return _vGPSDateTime; }
            set { _vGPSDateTime = value; }
        }
        public string dGPSDateTime {
            get { return _dGPSDateTime; }
            set { _dGPSDateTime = value; }
        }
        public string vPositionSendingDateTime {
            get { return _vPositionSendingDateTime; }
            set { _vPositionSendingDateTime = value; }
        }
        public string vHeading {
            get { return _vHeading; }
            set { _vHeading = value; }
        }
        public string vReportID {
            get { return _vReportID; }
            set { _vReportID = value; }
        }
        public string vOdometer {
            get { return _vOdometer; }
            set { _vOdometer = value; }
        }
        public string vHDOP {
            get { return _vHDOP; }
            set { _vHDOP = value; }
        }
        public string vInputStatus {
            get { return _vInputStatus; }
            set { _vInputStatus = value; }
        }
        public string vVehicleSpeed {
            get { return _vVehicleSpeed; }
            set { _vVehicleSpeed = value; }
        }
        public string vOutputStatus {
            get { return _vOutputStatus; }
            set { _vOutputStatus = value; }
        }
        public string vAnalogInputValue {
            get { return _vAnalogInputValue; }
            set { _vAnalogInputValue = value; }
        }
        public string vDriverID {
            get { return _vDriverID; }
            set { _vDriverID = value; }
        }
        public string vFirstTempSensorValue {
            get { return _vFirstTempSensorValue; }
            set { _vFirstTempSensorValue = value; }
        }
        public string vSecondTemperatureSensorValue {
            get { return _vSecondTemperatureSensorValue; }
            set { _vSecondTemperatureSensorValue = value; }
        }
        public string vTextMessage {
            get { return _vTextMessage; }
            set { _vTextMessage = value; }
        }
        public DateTime dEnterDate { get { return _dEnterDate; } set { _dEnterDate = value; } }
        private DateTime _dDateFrom;
        private decimal _vDistance;
        private decimal _iAverageSpeed;

        public DateTime dDateFrom {
            get { return _dDateFrom; }
            set { _dDateFrom = value; }
        }

        private DateTime _dDateTo;

        public DateTime dDateTo {
            get { return _dDateTo; }
            set { _dDateTo = value; }
        }

        public string Drivername {
            get {
                return this._Drivername;
            }
            set {
                this._Drivername = value;
            }
        }

        public string DriverPhoto {
            get {
                return this._DriverPhoto;
            }
            set {
                this._DriverPhoto = value;
            }
        }

        public string iBatteryBackup {
            get {
                return this._iBatteryBackup;
            }
            set {
                this._iBatteryBackup = value;
            }
        }

        public DateTime UserDateTime { get; set; }

        public string CustomUrl3 { get; set; }

        public clsTripReplay () {
            // initialization constructore
        }

        public clsTripReplay (string login) {
            this.login = login;
        }

        public int AssetId { get; set; }
        public int MeasurementUnit { get; set; }
        

        public clsTripReplay (string vSequenceID, string vpkDeviceID, string dGPSDateTime, string vPositionSendingDateTime, string vLongitude,
            string vLatitude, string vHeading, string vReportID, string vOdometer, string vVehicleSpeed, string vDriverID, string vTextMessage,
            string vDeviceName, string vAssetLogo, string vImage, string bIsIgnitionOn, int iTrackerType, decimal vDistance, string Drivername, string DriverPhoto, decimal iAverageSpeed, string vRoadSpeed, decimal nAltitude, string iBatteryBackup, double vAnalog1, double vAnalog2, bool isAnalog1Mapped, bool isAnalog2Mapped, string Analog1MappedName, string Analog2MappedName,
            string vUnitText1, int iMinValue1, int iMaxValue1, string vUnitText2, int iMinValue2, int iMaxValue2) {
            this.vSequenceID = vSequenceID;
            this.vUnitID = vpkDeviceID;
            this.dGPSDateTime = dGPSDateTime;
            this.vPositionSendingDateTime = vPositionSendingDateTime;
            this.vLongitude = vLongitude;
            this.vLatitude = vLatitude;
            this.vHeading = vHeading;
            this.vReportID = vReportID;
            this.vOdometer = vOdometer;
            this.vVehicleSpeed = vVehicleSpeed;
            this.vDriverID = vDriverID;
            this.vTextMessage = vTextMessage;
            this.vDeviceName = vDeviceName;
            this.vAssetLogo = vAssetLogo;
            this.vImage = vImage;
            this.bIsIgnitionOn = bIsIgnitionOn;
            this.iTrackerType = iTrackerType;
            this.vDistance = vDistance;
            this.Drivername = Drivername;
            this.DriverPhoto = DriverPhoto;
            this.iAverageSpeed = iAverageSpeed;
            this.vRoadSpeed = vRoadSpeed;
            this.nAltitude = nAltitude;
            this.iBatteryBackup = iBatteryBackup;
            this.vAnalog1 = vAnalog1;
            this.vAnalog2 = vAnalog2;
            this.isAnalog1Mapped = isAnalog1Mapped;
            this.isAnalog2Mapped = isAnalog2Mapped;
            this.Analog1MappedName = Analog1MappedName;
            this.Analog2MappedName = Analog2MappedName;
            this.vUnitText1 = vUnitText1;
            this.iMinValue1 = iMinValue1;
            this.iMaxValue1 = iMaxValue1;
            this.vUnitText2 = vUnitText2;
            this.iMinValue2 = iMinValue2;
            this.iMaxValue2 = iMaxValue2;

        }

        public clsTripReplay (long nStartEventInst, long nEndEventInst, string vpkDeviceID) {
            this.nStartEventInst = nStartEventInst;
            this.nEndEventInst = nEndEventInst;
            this.vpkDeviceID = vpkDeviceID;
        }
        public clsTripReplay (int tripcount, string leftMenuDetail, string totalDistanceForDay, string endLocation, double stopageTime) {
            this.totalDistanceForDay = totalDistanceForDay;
            this.tripcount = tripcount;
            this.leftMenuDetail = leftMenuDetail;
            this.EndLocation = endLocation;
            this.TripStopageTime = stopageTime;
        }
        public clsTripReplay (int tripcount, string leftMenuDetail, string totalDistanceForDay) {
            this.totalDistanceForDay = totalDistanceForDay;
            this.tripcount = tripcount;
            this.leftMenuDetail = leftMenuDetail;
        }

        public DataSet MapTripData () {
            DataSet ds = new DataSet ();
            try {

                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@vpkDeviceID", SqlDbType.VarChar);
                param[1].Value = vpkDeviceID;

                param[2] = new SqlParameter ("@nStartEventInst", SqlDbType.BigInt);
                param[2].Value = nStartEventInst;

                param[3] = new SqlParameter ("@nEndEventInst", SqlDbType.BigInt);
                param[3].Value = nEndEventInst;

                param[4] = new SqlParameter ("@ipkTripID", SqlDbType.BigInt);
                param[4].Value = ipkTripID;

                param[5] = new SqlParameter ("@dateFrom", SqlDbType.DateTime);
                param[5].Value = strStartTime;

                param[6] = new SqlParameter ("@dateTo", SqlDbType.DateTime);
                param[6].Value = strEndTime;

                param[7] = new SqlParameter ("@deviceDbId", SqlDbType.Int);
                param[7].Value = deviceDbId;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "sp_GetTripReplay_AllDataPoints", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsTripReplay.cs", "MapTripData()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        public DataSet MapTripDataForDate () {
            DataSet ds = new DataSet ();
            try {

                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@vpkDeviceID", SqlDbType.VarChar);
                param[1].Value = vpkDeviceID;

                param[2] = new SqlParameter ("@nStartEventInst", SqlDbType.BigInt);
                param[2].Value = nStartEventInst;

                param[3] = new SqlParameter ("@nEndEventInst", SqlDbType.BigInt);
                param[3].Value = nEndEventInst;

                param[4] = new SqlParameter ("@TripreplayDate", SqlDbType.VarChar);
                param[4].Value = TripreplayDate;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "NewFrontData_sp", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsTripReplay.cs", "MapTripData()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        public DataSet MapTripDataToGenerateReport () {
            DataSet ds = new DataSet ();
            try {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = 19;

                param[1] = new SqlParameter ("@vpkDeviceID", SqlDbType.VarChar);
                param[1].Value = vpkDeviceID;

                param[2] = new SqlParameter ("@nStartEventInst", SqlDbType.BigInt);
                param[2].Value = nStartEventInst;

                param[3] = new SqlParameter ("@nEndEventInst", SqlDbType.BigInt);
                param[3].Value = nEndEventInst;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "NewFrontData_sp", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsTripReplay.cs", "MapTripData()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        public DataSet ReportData () {
            DataSet ds = new DataSet ();
            try {

                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@vpkDeviceID", SqlDbType.VarChar);
                param[1].Value = vpkDeviceID;

                param[2] = new SqlParameter ("@IpkMSMQTrackId", SqlDbType.Int);
                param[2].Value = IpkMSMQTrackId;

                param[3] = new SqlParameter ("@dateFrom", SqlDbType.DateTime);
                param[3].Value = dDateFrom.ToString ("yyyy-MM-dd HH:mm:ss");

                param[4] = new SqlParameter ("@dateTo", SqlDbType.DateTime);
                param[4].Value = dDateTo.ToString ("yyyy-MM-dd HH:mm:ss");

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "NewFrontData_sp", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsTripReplay.cs", "MapTripData()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        public DataSet ParticularReportData () {
            DataSet ds = new DataSet ();
            try {

                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter ("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter ("@vpkDeviceID", SqlDbType.VarChar);
                param[1].Value = vpkDeviceID;

                param[2] = new SqlParameter ("@IpkMSMQTrackId", SqlDbType.Int);
                param[2].Value = IpkMSMQTrackId;

                param[3] = new SqlParameter ("@ifkGroupMID", SqlDbType.Int);
                param[3].Value = ifkGroupMID;

                param[4] = new SqlParameter ("@ifkCompanyID", SqlDbType.Int);
                param[4].Value = ifkCompanyID;

                param[5] = new SqlParameter ("@ipkUserID", SqlDbType.Int);
                param[5].Value = ifkUserID;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "NewFrontData_sp", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsTripReplay.cs", "MapTripData()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        public void GetCSVReport (TextWriter tw, DataTable dt) {
            try {
                string Content = "";

                if (dt.Columns.Count > 0) {
                    for (int i = 0; i < dt.Columns.Count; i++) {
                        if (i != dt.Columns.Count - 1) {
                            Content += dt.Columns[i].ColumnName.ToString ().Replace (',', ' ') + ",";

                        } else {

                            Content += dt.Columns[i].ColumnName.ToString ().Replace (',', ' ');
                        }
                    }
                }
                tw.WriteLine (Content);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsTripReplay.cs", "GetCSVReport()", ex.Message  + ex.StackTrace);
                throw;

            }
        }

        public DataSet GetTripReplayList () {
            DataSet ds = new DataSet ();
            try {

                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter ("@vpkDeviceID", SqlDbType.VarChar);

                if (vpkDeviceID == "headclicked") {
                    param[0].Value = System.DBNull.Value;
                } else {
                    param[0].Value = vpkDeviceID;
                }

                param[1] = new SqlParameter ("@dateFrom", SqlDbType.DateTime);
                param[1].Value = dDateFrom;

                param[2] = new SqlParameter ("@dateTo", SqlDbType.DateTime);
                param[2].Value = dDateTo;

                param[3] = new SqlParameter ("@iParent", SqlDbType.BigInt);
                param[3].Value = ifkCompanyID;

                ds = SqlHelper.ExecuteDataset (f_strConnectionString, CommandType.StoredProcedure, "sp_GetTripReplayList", param);
            } catch (Exception ex) {
                LogError.RegisterErrorInLogFile ("clsTripReplay.cs", "GetTripReplayList()", ex.Message  + ex.StackTrace);
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