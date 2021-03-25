using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WLT.DataAccessLayer;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;
using System.Collections;
using System.Globalization;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsClientDevice : IDisposable
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        public int ignore_ign_on_mins { get; set; }
        public int spike_threshold { get; set; }
        public int spike_threshold_return { get; set; }
        public int spike_sample_points { get; set; }



        private int _Operation;
        private int _ipkDeviceID;
        private string _vDeviceName;
        private int _iTrackerType;
        private string _vColor;
        private string _vMake;
        private string _vModel;
        private int _iParent;
        private bool _bStatus;
        private int _iCreatedBy;
        private string _vTrackerTypeName;
        private int _ipkGroupAssetID;
        private int _ifkDeviceID;
        private int _ifkGroupMID;
        private int _ifkGroupGMID;
        private string _vIsConfigurable;
        private string _vUnitName;
        private int _ifkResellerID;
        private int _ifkClientID;
        private List<clsClientDevice> _LstGroupAssetID;


        private int _ipkDriverID;
        private string _vDriverName;
        private int _DriverIDType;
        private string _DriverIDNo;
        private string _PersonalIDNo;
        private string _LicenceNo;
        private string _ContactNo;
        private string _vDriverIdEncrypted;
        private string _DriverDOB;


        private string _iButtontype;

        private string _strDigitalInput;
        private string _strDigitalOutput;
        private string _strAnalogInput;
        private string _strSoftwareGeneratedEvents;
        private string _myObjects;
        private string _iImeiNumber;
        private string _hSerialNumber;
        private int _intId;

        private int _nSimCardId;
        private string _nSimCardSerialNumber;
        private string _nSimCardGSMNumber;
        private string _nSimCardNetworkProvider;
        private string _nSimCardDesc;
        private string _nSimCardPuk1Code;
        private string _nSimCardPuk2Code;
        private string _nSimCardPin1;
        private string _nSimCardPin2;
        private int _intUserId;
        private int _intCompanyId;
        private DateTime _nSimCardDateOfNote;
        private string _nSimCardNote;

        private int _SMSLoginMonth;
        private int _SMSLoginYear;
        private DateTime _UsageDate;
        private bool _IsMonthSelected;

        private int _EmailLoginMonth;
        private int _EmailLoginYear;

        private string _USerPhNo;
        private string _APNNO;
        private string _GPRSUser;
        private string _GPRSPassword;
        private string _MobileNetworkName;
        private string _OperatorCode;
        private string _CountryCode;
        private int _SelectedClientID;
        private string _DeviceCommandText;
        private string _DeviceCommandType;
        private int _DeviceSequenceID;
        private string _DeviceCommandDescription;
        private int _DeviceCommandID;
        private int _ifkCommandLookup;

        private int _IfkEventID;
        private bool _IsSupported;
        private int _IsSupportedID;
        private List<int> _ListofEventIDs;
        private List<int> _ListofSupportedIDs;
        private List<bool> _ListofIsSupported;
        private List<bool> _ListofisShowOnAlert;

        private int _ipkDeviceEventCodeId;
        private string _vEventId;
        private string _vEventIdValue;
        private string _vEventType;
        private int _ifkCommonEventLookupId;
        private string _vName;
        private int _HardwareID;

        private int _AnalogInputID;
        private int _ifk_IO_Analog_Master_ID;
        private float? _iMinVolts;
        private float? _iMaxVolts;
        private float? _iMinValue;
        private float? _iMaxValue;
        private int _iAnalogNumber;
        private int _iAveraging;
        private bool _isMaxCurrentlyOutOfRange;
        private bool _swEvent_Enabled;
        private int? _swEvent_iMinRange;
        private int? _swEvent_iMaxRange;
        private int? _swEvent_iRangeTriggerCount;
        private int? _swEvent_iSuddenDrop;
        private int? _swEvent_iSuddenIncrease;
        private bool _isMinCurrentlyOutOfRange;
        private int _IO_Analog_Mapping_ID;
        private long _vpkDeviceID;
        private bool _IsSoftwareIgnition;
        private bool _isDefaultDriver;
        private bool _isShowOnAlert;
        private string _CustomCommandValues;
        private bool _IsOutputCmd;
        private bool _IsSmsCmd;
        private bool _IgnoreWhenIgnOff;
        private string _ReportingTimeZone;
        private string _SecondaryImei;

        private int _IgnitionOverride;
        private bool _SoftwareOdometer;

        private string _vManufacturerName;

        private bool _PwaIsEnabled;
        private string _PwaName;
        private string _PwaDesc;
        private string _PwaBackgroundColor;
        private string _PwaThemeColor;
        private string _PwaLogoUrl;
        private string _vTimeOffset;
        private string _DeviceId1Wire;
        private int _SensorType;
        private double _iMaxDeviceSpeedLimit;
        private int _ifkCanBusType;
        private string _CustomID;

        private string _OwnerName;
        private string _OwnerID;
        private string _OwnerPhone;
        private string _RegNumber;
        private string _ChasisNo;
        private string _AssFuelType;
        private string _EngineSize;
        private string _FittingCompanyName;
        private string _FittingCompanyPhone;
        private string _FittingCompanyEmail;
        private string _FittingCompanyNumber;
        private string _FittingDate;
        private string _FittingLocation;
        private string _FittingCertNo;
        private string _FitterName;
        private string _FitterID;
        private string _FitterPhone;


        public string DeviceId1Wire { get { return _DeviceId1Wire; } set { _DeviceId1Wire = value; } }
        public int SensorType { get { return _SensorType; } set { _SensorType = value; } }
        public int ifkCanBusType { get { return _ifkCanBusType; } set { _ifkCanBusType = value; } }


        public string vTimeOffset { get { return _vTimeOffset; } set { _vTimeOffset = value; } }

        private int _iMaxRoadSpeedPercent;
        public int iMaxRoadSpeedPercent { get { return _iMaxRoadSpeedPercent; } set { _iMaxRoadSpeedPercent = value; } }
        public int iMaxRoadSpeedLimit { get; set; }
        public double iMaxDeviceSpeedLimit { get { return _iMaxDeviceSpeedLimit; } set { _iMaxDeviceSpeedLimit = value; } }
        public bool PwaIsEnabled { get { return _PwaIsEnabled; } set { _PwaIsEnabled = value; } }
        public string PwaName { get { return _PwaName; } set { _PwaName = value; } }
        public string PwaDesc { get { return _PwaDesc; } set { _PwaDesc = value; } }
        public string PwaBackgroundColor { get { return _PwaBackgroundColor; } set { _PwaBackgroundColor = value; } }
        public string PwaThemeColor { get { return _PwaThemeColor; } set { _PwaThemeColor = value; } }
        public string PwaLogoUrl { get { return _PwaLogoUrl; } set { _PwaLogoUrl = value; } }

        public string vManufacturerName { get { return _vManufacturerName; } set { _vManufacturerName = value; } }
        public int IgnitionOverride { get { return _IgnitionOverride; } set { _IgnitionOverride = value; } }
        public bool SoftwareOdometer { get { return _SoftwareOdometer; } set { _SoftwareOdometer = value; } }

        public string ReportingTimeZone { get { return _ReportingTimeZone; } set { _ReportingTimeZone = value; } }
        public string SecondaryImei { get { return _SecondaryImei; } set { _SecondaryImei = value; } }
        public bool IgnoreWhenIgnOff { get { return _IgnoreWhenIgnOff; } set { _IgnoreWhenIgnOff = value; } }

        public string CustomCommandValues { get { return _CustomCommandValues; } set { _CustomCommandValues = value; } }
        public bool IsOutputCmd { get { return _IsOutputCmd; } set { _IsOutputCmd = value; } }
        public bool IsSmsCmd { get { return _IsSmsCmd; } set { _IsSmsCmd = value; } }

        public bool isShowOnAlert { get { return _isShowOnAlert; } set { _isShowOnAlert = value; } }

        public int ifk_IO_Analog_Master_ID { get { return _ifk_IO_Analog_Master_ID; } set { _ifk_IO_Analog_Master_ID = value; } }
        public float? iMinVolts { get { return _iMinVolts; } set { _iMinVolts = value; } }
        public float? iMaxVolts { get { return _iMaxVolts; } set { _iMaxVolts = value; } }
        public float? iMinValue { get { return _iMinValue; } set { _iMinValue = value; } }
        public float? iMaxValue { get { return _iMaxValue; } set { _iMaxValue = value; } }
        public int iAnalogNumber { get { return _iAnalogNumber; } set { _iAnalogNumber = value; } }
        public int iAveraging { get { return _iAveraging; } set { _iAveraging = value; } }
        public bool isMaxCurrentlyOutOfRange { get { return _isMaxCurrentlyOutOfRange; } set { _isMaxCurrentlyOutOfRange = value; } }
        public bool swEvent_Enabled { get { return _swEvent_Enabled; } set { _swEvent_Enabled = value; } }
        public int? swEvent_iMinRange { get { return _swEvent_iMinRange; } set { _swEvent_iMinRange = value; } }
        public int? swEvent_iMaxRange { get { return _swEvent_iMaxRange; } set { _swEvent_iMaxRange = value; } }
        public int? swEvent_iRangeTriggerCount { get { return _swEvent_iRangeTriggerCount; } set { _swEvent_iRangeTriggerCount = value; } }
        public int? swEvent_iSuddenDrop { get { return _swEvent_iSuddenDrop; } set { _swEvent_iSuddenDrop = value; } }
        public int? swEvent_iSuddenIncrease { get { return _swEvent_iSuddenIncrease; } set { _swEvent_iSuddenIncrease = value; } }
        public bool isMinCurrentlyOutOfRange { get { return _isMinCurrentlyOutOfRange; } set { _isMinCurrentlyOutOfRange = value; } }
        public int IO_Analog_Mapping_ID { get { return _IO_Analog_Mapping_ID; } set { _IO_Analog_Mapping_ID = value; } }
        public long vpkDeviceID { get { return _vpkDeviceID; } set { _vpkDeviceID = value; } }

        public int AnalogInputID { get { return _AnalogInputID; } set { _AnalogInputID = value; } }
        public int HardwareID { get { return _HardwareID; } set { _HardwareID = value; } }
        public string vName { get { return _vName; } set { _vName = value; } }

        public int ifkResellerID { get { return _ifkResellerID; } set { _ifkResellerID = value; } }
        public int ifkClientID { get { return _ifkClientID; } set { _ifkClientID = value; } }

        public List<int> ListofEventIDs
        {
            get { return _ListofEventIDs; }
            set { _ListofEventIDs = value; }
        }

        public List<int> ListofSupportedIDs
        {
            get { return _ListofSupportedIDs; }
            set { _ListofSupportedIDs = value; }
        }

        public List<bool> ListofIsSupported
        {
            get { return _ListofIsSupported; }
            set { _ListofIsSupported = value; }
        }

        public List<bool> ListofisShowOnAlert
        {
            get { return _ListofisShowOnAlert; }
            set { _ListofisShowOnAlert = value; }
        }

        public double FuelTankCapacity { get; set; }
        public string vDeviceCalibration { get; set; }
        public int ifkCommandLookup { get { return _ifkCommandLookup; } set { _ifkCommandLookup = value; } }
        public int IfkEventID { get { return _IfkEventID; } set { _IfkEventID = value; } }
        public bool IsSupported { get { return _IsSupported; } set { _IsSupported = value; } }
        public int IsSupportedID { get { return _IsSupportedID; } set { _IsSupportedID = value; } }

        public string USerPhNo { get { return _USerPhNo; } set { _USerPhNo = value; } }
        public string APNNO { get { return _APNNO; } set { _APNNO = value; } }
        public string GPRSUser { get { return _GPRSUser; } set { _GPRSUser = value; } }
        public string GPRSPassword { get { return _GPRSPassword; } set { _GPRSPassword = value; } }
        public string MobileNetworkName { get { return _MobileNetworkName; } set { _MobileNetworkName = value; } }
        public string CountryCode { get { return _CountryCode; } set { _CountryCode = value; } }
        public string OperatorCode { get { return _OperatorCode; } set { _OperatorCode = value; } }

        public DateTime nSimCardDateOfNote { get { return _nSimCardDateOfNote; } set { _nSimCardDateOfNote = value; } }
        public string nSimCardNote { get { return _nSimCardNote; } set { _nSimCardNote = value; } }

        public int nSimCardId { get { return _nSimCardId; } set { _nSimCardId = value; } }
        public int intUserId { get { return _intUserId; } set { _intUserId = value; } }
        public int intCompanyId { get { return _intCompanyId; } set { _intCompanyId = value; } }

        public string nSimCardSerialNumber { get { return _nSimCardSerialNumber; } set { _nSimCardSerialNumber = value; } }
        public string nSimCardGSMNumber { get { return _nSimCardGSMNumber; } set { _nSimCardGSMNumber = value; } }
        public string nSimCardNetworkProvider { get { return _nSimCardNetworkProvider; } set { _nSimCardNetworkProvider = value; } }
        public string nSimCardDesc { get { return _nSimCardDesc; } set { _nSimCardDesc = value; } }
        public string nSimCardPuk1Code { get { return _nSimCardPuk1Code; } set { _nSimCardPuk1Code = value; } }
        public string nSimCardPuk2Code { get { return _nSimCardPuk2Code; } set { _nSimCardPuk2Code = value; } }
        public string nSimCardPin1 { get { return _nSimCardPin1; } set { _nSimCardPin1 = value; } }
        public string nSimCardPin2 { get { return _nSimCardPin2; } set { _nSimCardPin2 = value; } }

        public int intId { get { return _intId; } set { _intId = value; } }
        public string iImeiNumber { get { return _iImeiNumber; } set { _iImeiNumber = value; } }
        public string hSerialNumber { get { return _hSerialNumber; } set { _hSerialNumber = value; } }
        public string strDigitalInput { get { return _strDigitalInput; } set { _strDigitalInput = value; } }
        public string strDigitalOutput { get { return _strDigitalOutput; } set { _strDigitalOutput = value; } }
        public string strAnalogInput { get { return _strAnalogInput; } set { _strAnalogInput = value; } }
        public string strSoftwareGeneratedEvents { get { return _strSoftwareGeneratedEvents; } set { _strSoftwareGeneratedEvents = value; } }
        public string myObjects { get { return _myObjects; } set { _myObjects = value; } }
        public int IsDriver { get; set; }

        public string DeviceCommandText { get { return _DeviceCommandText; } set { _DeviceCommandText = value; } }
        public string DeviceCommandType { get { return _DeviceCommandType; } set { _DeviceCommandType = value; } }

        public int DeviceSequenceID { get { return _DeviceSequenceID; } set { _DeviceSequenceID = value; } }
        public string DeviceCommandDescription { get { return _DeviceCommandDescription; } set { _DeviceCommandDescription = value; } }
        public int DeviceCommandID { get { return _DeviceCommandID; } set { _DeviceCommandID = value; } }

        public bool isDefaultDriver { get { return _isDefaultDriver; } set { _isDefaultDriver = value; } }

        public int ifkGroupGMID
        {
            get
            {
                return this._ifkGroupGMID;
            }
            set
            {
                this._ifkGroupGMID = value;
            }
        }

        public string iButtontype
        {
            get
            {
                return this._iButtontype;
            }
            set
            {
                this._iButtontype = value;
            }
        }

        public int ipkiButtonID
        {
            get
            {
                return this._ipkiButtonID;
            }
            set
            {
                this._ipkiButtonID = value;
            }
        }

        public string DriverDob
        {
            get
            {
                return this._DriverDOB;
            }
            set
            {
                this._DriverDOB = value;
            }
        }

        public string ContactNo
        {
            get
            {
                return this._ContactNo;
            }
            set
            {
                this._ContactNo = value;
            }
        }

        public string vDriverIdEncrypted
        {
            get
            {
                return this._vDriverIdEncrypted;
            }
            set
            {
                this._vDriverIdEncrypted = value;
            }
        }
        

        public string vTimezoneID { get; set; }
        public string LicenceNoExpiryDate { get;  set; }
        public string LicenceNo
        {
            get
            {
                return this._LicenceNo;
            }
            set
            {
                this._LicenceNo = value;
            }
        }

        public string PersonalIDNo
        {
            get
            {
                return this._PersonalIDNo;
            }
            set
            {
                this._PersonalIDNo = value;
            }
        }

        public string DriverIDNo
        {
            get
            {
                return this._DriverIDNo;
            }
            set
            {
                this._DriverIDNo = value;
            }
        }

        public int DriverIDType
        {
            get
            {
                return this._DriverIDType;
            }
            set
            {
                this._DriverIDType = value;
            }
        }

        public string vDriverName
        {
            get
            {
                return this._vDriverName;
            }
            set
            {
                this._vDriverName = value;
            }
        }

        public int ipkDriverID
        {
            get
            {
                return this._ipkDriverID;
            }
            set
            {
                this._ipkDriverID = value;
            }
        }

        public string UploadedAssetPhoto
        {
            get
            {
                return this._UploadedAssetPhoto;
            }
            set
            {
                this._UploadedAssetPhoto = value;
            }
        }

        public List<clsClientDevice> LstGroupAssetID
        {
            get { return _LstGroupAssetID; }
            set { _LstGroupAssetID = value; }
        }

        public int ifkGroupMID
        {
            get { return _ifkGroupMID; }
            set { _ifkGroupMID = value; }
        }

        public int ifkDeviceID
        {
            get { return _ifkDeviceID; }
            set { _ifkDeviceID = value; }
        }

        public int ipkGroupAssetID
        {
            get { return _ipkGroupAssetID; }
            set { _ipkGroupAssetID = value; }
        }
        public string vTrackerTypeName
        {
            get { return _vTrackerTypeName; }
            set { _vTrackerTypeName = value; }
        }

        public int SMSLoginMonth { get { return _SMSLoginMonth; } set { _SMSLoginMonth = value; } }
        public int SMSLoginYear { get { return _SMSLoginYear; } set { _SMSLoginYear = value; } }
        public DateTime UsageDate { get { return _UsageDate; } set { _UsageDate = value; } }
        public bool IsMonthSelected { get { return _IsMonthSelected; } set { _IsMonthSelected = value; } }

        public int EmailLoginMonth { get { return _EmailLoginMonth; } set { _EmailLoginMonth = value; } }
        public int EmailLoginYear { get { return _EmailLoginYear; } set { _EmailLoginYear = value; } }

        public int SelectedClientID { get { return _SelectedClientID; } set { _SelectedClientID = value; } }

        public int ipkDeviceEventCodeId { get { return _ipkDeviceEventCodeId; } set { _ipkDeviceEventCodeId = value; } }

        public string vEventId { get { return _vEventId; } set { _vEventId = value; } }

        public string vEventIdValue { get { return _vEventIdValue; } set { _vEventIdValue = value; } }

        public string vEventType { get { return _vEventType; } set { _vEventType = value; } }

        public int ifkCommonEventLookupId { get { return _ifkCommonEventLookupId; } set { _ifkCommonEventLookupId = value; } }
        public bool IsSoftwareIgnition { get { return _IsSoftwareIgnition; } set { _IsSoftwareIgnition = value; } }
        public string CustomID { get { return _CustomID; } set { _CustomID = value; } }

        public string WorkModeColor { get; set; }

        public string OwnerName { get { return _OwnerName; } set { _OwnerName = value; } }
        public string OwnerID { get { return _OwnerID; } set { _OwnerID = value; } }
        public string OwnerPhone { get { return _OwnerPhone; } set { _OwnerPhone = value; } }
        public string RegNumber { get { return _RegNumber; } set { _RegNumber = value; } }
        public string ChasisNo { get { return _ChasisNo; } set { _ChasisNo = value; } }
        public string AssFuelType { get { return _AssFuelType; } set { _AssFuelType = value; } }
        public string EngineSize { get { return _EngineSize; } set { _EngineSize = value; } }
        public string FittingCompanyName { get { return _FittingCompanyName; } set { _FittingCompanyName = value; } }
        public string FittingCompanyPhone { get { return _FittingCompanyPhone; } set { _FittingCompanyPhone = value; } }
        public string FittingCompanyEmail { get { return _FittingCompanyEmail; } set { _FittingCompanyEmail = value; } }
        public string FittingCompanyNumber { get { return _FittingCompanyNumber; } set { _FittingCompanyNumber = value; } }
        public string FittingDate { get { return _FittingDate; } set { _FittingDate = value; } }
        public string FittingLocation { get { return _FittingLocation; } set { _FittingLocation = value; } }
        public string FittingCertNo { get { return _FittingCertNo; } set { _FittingCertNo = value; } }
        public string FitterName { get { return _FitterName; } set { _FitterName = value; } }
        public string FitterID { get { return _FitterID; } set { _FitterID = value; } }
        public string FitterPhone { get { return _FitterPhone; } set { _FitterPhone = value; } }
        public string ImeiNumber { get; set; }
        public string APN { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public int DeviceId { get; set; }
        public int ClientId { get; set; }
        public string PhoneNumber { get; set; }
        public bool isResend { get; set; }
        public string SIMNumber { get; set; }
        public string TimeZoneID { get; set; }
        public string HardwareSerialNo { get; set; }

        public El_AssetDetailsMaster AssetDetailsMaster { get; set; }

        public string incab_phone { get; set; }
        public clsClientDevice()
        {
            // initialization constructor

            AssetDetailsMaster = new El_AssetDetailsMaster();
        }

        private string _nIMEINo;
        private string _nGSMNo;
        private string _nSimSetialNo;
        private int _iUnitID;

        private string _UploadedAssetPhoto;
        private int _ifk_AssignedAssetId;
        private int _ifkSimCardId;
        private string _vDescription;
        private string _CustomUrl1;
        private string _CustomUrl2;
        private string _CustomUrl3;
        private string _CustomUrl1Photo;
        private string _CustomUrl2Photo;
        private string _CustomUrl3Photo;
        private DateTime _DateDeact;

        private int _ifkDriverIdType;
        private string _DateUploaded;



        public int ifkDriverIdType { get { return _ifkDriverIdType; } set { _ifkDriverIdType = value; } }
        public string DateUploaded { get { return _DateUploaded; } set { _DateUploaded = value; } }

        public string nIMEINo { get { return _nIMEINo; } set { _nIMEINo = value; } }
        public string nGSMNo { get { return _nGSMNo; } set { _nGSMNo = value; } }
        public string nSimSetialNo { get { return _nSimSetialNo; } set { _nSimSetialNo = value; } }
        public int iUnitID { get { return _iUnitID; } set { _iUnitID = value; } }

        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkDeviceID { get { return _ipkDeviceID; } set { _ipkDeviceID = value; } }
        public string vDeviceName { get { return _vDeviceName; } set { _vDeviceName = value; } }
        public int iTrackerType { get { return _iTrackerType; } set { _iTrackerType = value; } }
        public string vColor { get { return _vColor; } set { _vColor = value; } }
        public string vMake { get { return _vMake; } set { _vMake = value; } }
        public string vModel { get { return _vModel; } set { _vModel = value; } }
        public int iParent { get { return _iParent; } set { _iParent = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public int iCreatedBy { get { return _iCreatedBy; } set { _iCreatedBy = value; } }
        public string vIsConfigurable { get { return _vIsConfigurable; } set { _vIsConfigurable = value; } }
        public string vUnitName { get { return _vUnitName; } set { _vUnitName = value; } }

        public int ifk_AssignedAssetId { get { return _ifk_AssignedAssetId; } set { _ifk_AssignedAssetId = value; } }
        public int ifkSimCardId { get { return _ifkSimCardId; } set { _ifkSimCardId = value; } }
        public string vDescription { get { return _vDescription; } set { _vDescription = value; } }
        public string CustomUrl1 { get { return _CustomUrl1; } set { _CustomUrl1 = value; } }
        public string CustomUrl2 { get { return _CustomUrl2; } set { _CustomUrl2 = value; } }
        public string CustomUrl3 { get { return _CustomUrl3; } set { _CustomUrl3 = value; } }
        public string CustomUrl1Photo { get { return _CustomUrl1Photo; } set { _CustomUrl1Photo = value; } }
        public string CustomUrl2Photo { get { return _CustomUrl2Photo; } set { _CustomUrl2Photo = value; } }
        public string CustomUrl3Photo { get { return _CustomUrl3Photo; } set { _CustomUrl3Photo = value; } }
        public DateTime DateDeact { get { return _DateDeact; } set { _DateDeact = value; } }

        private int _NewHeight;
        private int _NewWidth;

        public int NewWidth
        {
            get
            {
                return this._NewWidth;
            }
            set
            {
                this._NewWidth = value;
            }
        }

        public int NewHeight
        {
            get
            {
                return this._NewHeight;
            }
            set
            {
                this._NewHeight = value;
            }
        }

        private int _ipkiButtonID;

        private byte[] _ImageByteArray;

        public byte[] ImageByteArray
        {
            get
            {
                return _ImageByteArray;
            }
            set
            {
                _ImageByteArray = value;
            }
        }

        public string LogoName { get; set; }

        public string vEmail { get; set; }

        public string vSerialNo { get; set; }

        public string AssetIcon { get; set; }

        public string WorkModeTriggers { get; set; }


        public clsClientDevice(int iUnitID, string DeviceCommandType, string DeviceCommandText, string vName)
        {
            this.iUnitID = iUnitID;
            this.DeviceCommandType = DeviceCommandType;
            this.DeviceCommandText = DeviceCommandText;
            this.vName = vName;
        }

        public clsClientDevice(int ipkDeviceID, string vDeviceName, int iTrackerType, string vTrackerTypeName, string vColor, string vMake, string vModel, bool bStatus, string nIMEINo, string nGSMNo, string nSimSetialNo, int iUnitID, string vIsConfigurable, string vUnitName, int iParent, string UploadedAssetPhoto, int ifk_AssignedAssetId, string vDescription, string hSerialNumber, int HardwareID, bool IsSoftwareIgnition)
        {
            this.ipkDeviceID = ipkDeviceID;
            if (vDeviceName == "")
            {
                this.vDeviceName = "Unassigned";
            }
            else
            {
                this.vDeviceName = vDeviceName;
            }
            this.iTrackerType = iTrackerType;
            this.vTrackerTypeName = vTrackerTypeName;
            this.vColor = vColor;
            this.vMake = vMake;
            this.vModel = vModel;
            this.bStatus = bStatus;
            this.nIMEINo = nIMEINo;
            this.nGSMNo = nGSMNo;
            this.nSimSetialNo = nSimSetialNo;
            this.iUnitID = iUnitID;
            this.vIsConfigurable = vIsConfigurable;
            this.vUnitName = vUnitName;
            this.iParent = iParent;
            this.UploadedAssetPhoto = UploadedAssetPhoto;
            this.ifk_AssignedAssetId = ifk_AssignedAssetId;
            this.vDescription = vDescription;
            this.hSerialNumber = hSerialNumber;
            this.HardwareID = HardwareID;
            this.IsSoftwareIgnition = IsSoftwareIgnition;
        }

        public clsClientDevice(int ipkDriverID, string DriverName, int iTrackerType, string vTrackerTypeName, int DriverIDType, string DriverIDNo, string PersonalIDNo, bool bStatus, string LicenceNo, string ContactNo, string DriverDob, int ipkiButtonID, string iButtontype, int iParent, string UploadedAssetPhoto, string licenseExpiryDate)
        {
            this.ipkDriverID = ipkDriverID;
            this.vDriverName = DriverName;
            this.iTrackerType = iTrackerType;
            this.vTrackerTypeName = vTrackerTypeName;
            this.DriverIDType = DriverIDType;
            this.DriverIDNo = DriverIDNo;
            this.PersonalIDNo = PersonalIDNo;
            this.bStatus = bStatus;
            this.LicenceNo = LicenceNo;
            this.ContactNo = ContactNo;
            this.DriverDob = DriverDob;
            this.ipkiButtonID = ipkiButtonID;
            this.iParent = iParent;
            this.UploadedAssetPhoto = UploadedAssetPhoto;
            this.iButtontype = iButtontype;

           
            this.LicenceNoExpiryDate = licenseExpiryDate;
        }

        public clsClientDevice(int nSimCardId, string nSimCardSerialNumber, string nSimCardGSMNumber, string nSimCardNetworkProvider, string nSimCardDesc, string nSimCardPuk1Code, string nSimCardPuk2Code, string nSimCardPin1, string nSimCardPin2, int ifkDeviceID, string vDeviceName, int iParent)
        {
            this.nSimCardId = nSimCardId;
            this.nSimCardSerialNumber = nSimCardSerialNumber;
            this.nSimCardGSMNumber = nSimCardGSMNumber;
            this.nSimCardNetworkProvider = nSimCardNetworkProvider;
            this.nSimCardDesc = nSimCardDesc;
            this.nSimCardPuk1Code = nSimCardPuk1Code;
            this.nSimCardPuk2Code = nSimCardPuk2Code;
            this.nSimCardPin1 = nSimCardPin1;
            this.nSimCardPin2 = nSimCardPin2;
            this.ifkDeviceID = ifkDeviceID;
            this.vDeviceName = vDeviceName;
            this.iParent = iParent;
        }
        public clsClientDevice(int SMSLoginMonth, int SMSLoginYear)
        {
            this.SMSLoginMonth = SMSLoginMonth;
            this.SMSLoginYear = SMSLoginYear;
        }

        public clsClientDevice(string vDeviceName, int ipkDeviceID)
        {
            this.vDeviceName = vDeviceName;
            this.ipkDeviceID = ipkDeviceID;
        }

        public clsClientDevice(int ipkDeviceID, string vDeviceName)
        {
            this.vDeviceName = vDeviceName;
            this.ipkDeviceID = ipkDeviceID;
        }

        public clsClientDevice(string nIMEINo, string vDeviceName)
        {
            this.nIMEINo = nIMEINo;
            this.vDeviceName = vDeviceName;
        }

        public clsClientDevice(string _strDigitalInput, string _strDigitalOutput, string _strAnalogInput, string _strSoftwareGeneratedEvents, string _myObjects)
        {
            this.strDigitalInput = _strDigitalInput;
            this.strDigitalOutput = _strDigitalOutput;
            this.strAnalogInput = _strAnalogInput;
            this.strSoftwareGeneratedEvents = _strSoftwareGeneratedEvents;
            this.myObjects = _myObjects;
        }

        public string SaveClientDeviceDetails()
        {
            DataSet ds = new DataSet();
            string result = "";
            SqlParameter[] param = new SqlParameter[18];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar);
                param[2].Value = vDeviceName;

                param[3] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[3].Value = iTrackerType;

                param[4] = new SqlParameter("@vColor", SqlDbType.VarChar);
                param[4].Value = vColor;

                param[5] = new SqlParameter("@vMake", SqlDbType.VarChar);
                param[5].Value = vMake;

                param[6] = new SqlParameter("@vModel", SqlDbType.VarChar);
                param[6].Value = vModel;

                param[7] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[7].Value = bStatus;

                param[8] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[8].Value = iCreatedBy;

                param[9] = new SqlParameter("@iParent", SqlDbType.Int);
                param[9].Value = iParent;

                param[10] = new SqlParameter("@Error", SqlDbType.Int);
                param[10].Direction = ParameterDirection.Output;

                param[11] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[11].Value = ifkDeviceID;

                param[12] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[12].Value = ifkGroupMID;

                param[13] = new SqlParameter("@ipkGroupAssetID", SqlDbType.Int);
                param[13].Value = ipkGroupAssetID;

                param[14] = new SqlParameter("@CustomId", SqlDbType.NVarChar);
                param[14].Value = CustomID;

                param[15] = new SqlParameter("@DeactDate", SqlDbType.DateTime);
                param[15].Value = DateDeact;

                param[16] = new SqlParameter("@WorkModeColor", SqlDbType.VarChar);
                param[16].Value = WorkModeColor;

                param[17] = new SqlParameter("@incab_phone", SqlDbType.VarChar);
                param[17].Value = incab_phone;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                result = param[10].Value.ToString();
               
            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveClientDeviceDetails()", ex.Message + ex.StackTrace);
                result = "Internal Execution Error :" + ex.Message;
            }
            return result;

        }

        public string SaveDriverDetails()
        {
            DataSet ds = new DataSet();
            string result = "";
            SqlParameter[] param = new SqlParameter[16];
            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDriverID", SqlDbType.Int);
                param[1].Value = ipkDriverID;

                param[2] = new SqlParameter("@vDriverName", SqlDbType.NVarChar);
                param[2].Value = vDriverName;

                param[3] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[3].Value = iTrackerType;

                param[4] = new SqlParameter("@DriverIDType", SqlDbType.Int);
                param[4].Value = DriverIDType;

                param[5] = new SqlParameter("@DriverIDNo", SqlDbType.NVarChar);
                param[5].Value = DriverIDNo;

                param[6] = new SqlParameter("@DriverPersonalID", SqlDbType.NVarChar);
                param[6].Value = PersonalIDNo;

                param[7] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[7].Value = bStatus;

                param[8] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[8].Value = iCreatedBy;

                param[9] = new SqlParameter("@iParent", SqlDbType.Int);
                param[9].Value = iParent;

                param[10] = new SqlParameter("@Error", SqlDbType.Int);
                param[10].Direction = ParameterDirection.Output;

                param[11] = new SqlParameter("@LicenceNo", SqlDbType.NVarChar);
                param[11].Value = LicenceNo;

                param[12] = new SqlParameter("@DriverDOB", SqlDbType.Date);
                param[12].Value = Convert.ToDateTime(DriverDob);

                param[13] = new SqlParameter("@ContactNo", SqlDbType.NVarChar);
                param[13].Value = ContactNo;

                param[14] = new SqlParameter("@vDriverIdEncrypted", SqlDbType.VarChar);
                param[14].Value = vDriverIdEncrypted;


                Func<string, bool> HasCorrectDateFormat = (a) => DateTime.TryParseExact(a, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);


                if (HasCorrectDateFormat(LicenceNoExpiryDate))
                {
                    LicenceNoExpiryDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(Convert.ToDateTime(LicenceNoExpiryDate), TimeZoneID).ToString("yyyy-MM-dd HH:mm:ss");
                }

                param[15] = new SqlParameter("@LicenceNoExpiryDate", SqlDbType.VarChar);
                param[15].Value = LicenceNoExpiryDate;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_DriverDetails", param);
                result = param[10].Value.ToString();

                if (param[10].Value.ToString() == "-1")
                {
                    result = "-1";
                }
                else if (param[10].Value.ToString() != "-1" && param[10].Value.ToString() != "-2")
                {
                    result = param[10].Value.ToString();
                }
                else if (param[10].Value.ToString() == "-2")
                {
                    result = "-2";
                }


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveDriverDetails()", ex.Message + ex.StackTrace);
                result = "-3";
            }
            return result;

        }


        public DataSet GetResellerDeviceList()
        {
            SqlParameter[] param = new SqlParameter[7];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@iParent", SqlDbType.Int);
                param[2].Value = iParent;

                param[3] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[3].Value = iTrackerType;

                param[4] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[4].Value = ifkDeviceID;

                param[5] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[5].Value = ifkGroupMID;

                param[6] = new SqlParameter("@IsDriver", SqlDbType.Int);
                param[6].Value = IsDriver;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetResellerDeviceList()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public string SaveAsstCustomUrl()
        {
            SqlParameter[] param = new SqlParameter[6];
            DataSet ds = new DataSet();
            var result = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@CustomUrl1", SqlDbType.VarChar);
                param[2].Value = CustomUrl1;

                param[3] = new SqlParameter("@CustomUrl2", SqlDbType.VarChar);
                param[3].Value = CustomUrl2;

                param[5] = new SqlParameter("@CustomUrl3", SqlDbType.VarChar);
                param[5].Value = CustomUrl3;

                param[4] = new SqlParameter("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                result = param[4].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveAsstCustomUrl()", ex.Message + ex.StackTrace);
            }

            return result;
        }

        public DataSet GetAsstCustomUrl()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();         

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;                

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
                               
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveAsstCustomUrl()", ex.Message + ex.StackTrace);
            }

            return ds;
        }
        public string SaveAssetAdditionalDetails()
        {
            SqlParameter[] param = new SqlParameter[23];
            string result = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipk_device_id", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@owner_name", SqlDbType.NVarChar);
                param[2].Value = OwnerName;

                param[3] = new SqlParameter("@owner_national_id", SqlDbType.NVarChar);
                param[3].Value = OwnerID;

                param[4] = new SqlParameter("@owner_phone", SqlDbType.NVarChar);
                param[4].Value = OwnerPhone;

                param[5] = new SqlParameter("@registeration_plate", SqlDbType.NVarChar);
                param[5].Value = RegNumber;

                param[6] = new SqlParameter("@chasis_no", SqlDbType.NVarChar);
                param[6].Value = ChasisNo;

                param[7] = new SqlParameter("@fuel_type", SqlDbType.NVarChar);
                param[7].Value = AssFuelType;

                param[8] = new SqlParameter("@engine_size", SqlDbType.NVarChar);
                param[8].Value = EngineSize;

                param[9] = new SqlParameter("@fitting_company_name", SqlDbType.NVarChar);
                param[9].Value = FittingCompanyName;

                param[10] = new SqlParameter("@fitting_company_phone", SqlDbType.NVarChar);
                param[10].Value = FittingCompanyPhone;

                param[11] = new SqlParameter("@fitting_company_email", SqlDbType.NVarChar);
                param[11].Value = FittingCompanyEmail;

                param[12] = new SqlParameter("@fitting_company_reg_no", SqlDbType.NVarChar);
                param[12].Value = FittingCompanyNumber;

                param[13] = new SqlParameter("@fitting_date", SqlDbType.DateTime);
                param[13].Value = FittingDate;

                param[14] = new SqlParameter("@fitting_location", SqlDbType.NVarChar);
                param[14].Value = FittingLocation;

                param[15] = new SqlParameter("@fitting_certificate_no", SqlDbType.NVarChar);
                param[15].Value = FittingCertNo;

                param[16] = new SqlParameter("@fitter_name", SqlDbType.NVarChar);
                param[16].Value = FitterName;

                param[17] = new SqlParameter("@fitter_national_id", SqlDbType.NVarChar);
                param[17].Value = FitterID;

                param[18] = new SqlParameter("@fitter_phone_no", SqlDbType.NChar);
                param[18].Value = FitterPhone;

                param[19] = new SqlParameter("@Color", SqlDbType.NVarChar);
                param[19].Value = vColor;


                param[20] = new SqlParameter("@Make", SqlDbType.NVarChar);
                param[20].Value = vMake;

                param[21] = new SqlParameter("@Model", SqlDbType.NChar);
                param[21].Value = vModel;

                param[22] = new SqlParameter("@Error", SqlDbType.Int);
                param[22].Direction = ParameterDirection.Output;



                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_AdditionalAssetDetails", param);

                result = param[22].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveAssetAdditionalDetails()", ex.Message + ex.StackTrace);

                result = ex.Message;
            }
            return result;
        }

        public DataSet SearchResellerDeviceList()
        {
            SqlParameter[] param = new SqlParameter[5];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@nIMEINo", SqlDbType.VarChar);
                param[1].Value = nIMEINo;

                param[2] = new SqlParameter("@iParent", SqlDbType.Int);
                param[2].Value = iParent;

                param[3] = new SqlParameter("@vEmail", SqlDbType.VarChar);
                param[3].Value = vEmail;

                param[4] = new SqlParameter("@vSerialNo", SqlDbType.VarChar);
                param[4].Value = vSerialNo;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_ResellerTools_IMEI_Search", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SearchResellerDeviceList()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetClientSimCardList()
        {
            SqlParameter[] param = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@ifkCompanyId", SqlDbType.BigInt);
                param[0].Value = iParent;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "GetClientSimCardList", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetClientSimCardList()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetSimCardNotes()
        {
            SqlParameter[] param = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@ifk_SimCardId", SqlDbType.BigInt);
                param[0].Value = nSimCardId;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_GetSimCardNotes", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetSimCardNotes()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetClientDriverList()
        {
            SqlParameter[] param = new SqlParameter[4];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDriverID", SqlDbType.Int);
                param[1].Value = ipkDriverID;

                param[2] = new SqlParameter("@iParent", SqlDbType.Int);
                param[2].Value = iParent;

                param[3] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[3].Value = iTrackerType;
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_DriverDetails", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetClientDriverList()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet ShowLatestDevices()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;



                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_ResellerTools_IMEI_Search", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "ShowLatestDevices()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet ShowLatestUsers()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;



                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_ResellerTools_IMEI_Search", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "ShowLatestUsers()", ex.Message + ex.StackTrace);
            }
            return ds;
        }


        public List<clsClientDevice> FillDeviceMaster()
        {
            List<clsClientDevice> lstMaster = new List<clsClientDevice>();
            clsClientDevice objMaster = new clsClientDevice();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;

                param[2] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[2].Value = ifkGroupMID;

                param[3] = new SqlParameter("@ifkGroupGMID", SqlDbType.Int);
                param[3].Value = ifkGroupGMID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString.ToString(), CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        lstMaster.Add(new clsClientDevice(Convert.ToInt32(drRow["Id"].ToString()), drRow["vDeviceName"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "FillDeviceMaster()", ex.Message + ex.StackTrace);
            }
            return lstMaster;
        }
        public List<clsClientDevice> FillGroupDeviceMaster()
        {
            List<clsClientDevice> lstMaster = new List<clsClientDevice>();
            clsClientDevice objMaster = new clsClientDevice();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[1].Value = ifkGroupMID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString.ToString(), CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        lstMaster.Add(new clsClientDevice(Convert.ToInt32(drRow["Id"].ToString()), drRow["vDeviceName"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "FillGroupDeviceMaster()", ex.Message + ex.StackTrace);
            }
            return lstMaster;
        }
        public List<clsClientDevice> SelectResellerDevice(string TimeZoneID)
        {
            DataSet ds = new DataSet();
            List<clsClientDevice> lstResellerDevice = new List<clsClientDevice>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;


                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);


                if (ds.Tables.Count > 0)
                {


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {


                        string filename = ds.Tables[0].Rows[0]["vLogoName"].ToString();


                        lstResellerDevice.Add(new clsClientDevice
                        {
                            ipkDeviceID = Convert.ToInt32(row["ipkDeviceID"].ToString()),
                            vDeviceName = row["vDeviceName"].ToString() == "" ? "Unassigned" : row["vDeviceName"].ToString(),
                            iTrackerType = Convert.ToInt32(row["iTrackerType"].ToString()),
                            vTrackerTypeName = row["vTrackerTypeName"].ToString(),
                            vColor = row["vColor"].ToString(),
                            vMake = row["vMake"].ToString(),
                            vModel = row["vModel"].ToString(),
                            bStatus = Convert.ToBoolean(row["bStatus"].ToString()),
                            nIMEINo = row["ImeiNumber"].ToString(),
                            nGSMNo = row["SimCardId"].ToString(),
                            nSimSetialNo = row["SerialNumber"].ToString(),
                            iUnitID = Convert.ToInt32(row["iUnitID"].ToString()),
                            vIsConfigurable = "", //row["bIsConfigurable"].ToString(),
                            vUnitName = row["vUnitName"].ToString(),
                            iParent = Convert.ToInt32(row["iParent"].ToString()),
                            UploadedAssetPhoto = filename,
                            ifk_AssignedAssetId = Convert.ToInt32(row["AssetId"].ToString()),
                            vDescription = row["vDescription"].ToString(),
                            hSerialNumber = row["hSerialNumber"].ToString(),
                            HardwareID = 0,
                            IsSoftwareIgnition = Convert.ToBoolean(row["IsSoftwareIgnition"]),
                            ReportingTimeZone = row["ReportingTimeZone"].ToString(),
                            IgnitionOverride = Convert.ToInt32(row["IgnitionOverride"]),
                            SoftwareOdometer = Convert.ToBoolean(row["SoftwareOdometer"]),
                            SecondaryImei = row["secondary_imei"].ToString(),
                            CustomID = row["CustomId"].ToString(),
                            WorkModeColor = row["work_mode_color"].ToString(),

                            //Additional Details
                            OwnerName = row["Asset_Owner_Name"].ToString(),
                            OwnerID = row["Asset_Owner_ID"].ToString(),
                            OwnerPhone = row["Asset_Owner_Phone_Number"].ToString(),
                            RegNumber = row["Asset_Reg_Number"].ToString(),
                            ChasisNo = row["Asset_Chasis_Number"].ToString(),
                            AssFuelType = row["Asset_Fuel_Type"].ToString(),
                            EngineSize = row["Asset_Engine_Size"].ToString(),
                            FittingCompanyName = row["Fitting_Company_Name"].ToString(),
                            FittingCompanyPhone = row["Fitting_Company_Phone_Number"].ToString(),
                            FittingCompanyEmail = row["Fitting_Company_Email_Address"].ToString(),
                            FittingCompanyNumber = row["Fitting_Company_No"].ToString(),
                            FittingDate = row["Fitting_Date"].ToString() != "" ? UserSettings.ConvertUTCDateTimeToLocalDateTime(Convert.ToDateTime(row["Fitting_Date"]), TimeZoneID) : "",
                            FittingLocation = row["Fitting_Location"].ToString(),
                            FittingCertNo = row["Fitting_Certificate_Number"].ToString(),
                            FitterName = row["Fitter_Name"].ToString(),
                            FitterID = row["Fitter_ID"].ToString(),
                            FitterPhone = row["Fitter_Phone_number"].ToString()

                        });
                    }
                }



            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SelectResellerDevice()", ex.Message + ex.StackTrace);
            }

            return lstResellerDevice;
        }

        public List<clsClientDevice> SelectAssetDetails(string TimeZoneID)
        {
            DataSet ds = new DataSet();
            List<clsClientDevice> lstResellerDevice = new List<clsClientDevice>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;


                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                
                                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);


                if (ds.Tables.Count > 0)
                {


                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        string filename = ds.Tables[0].Rows[0]["vLogoName"].ToString();

                        var _clsClientDevice = new clsClientDevice();

                        _clsClientDevice.ipkDeviceID = Convert.ToInt32(row["ipkDeviceID"].ToString());
                        _clsClientDevice.vDeviceName = row["vDeviceName"].ToString() == "" ? "Unassigned" : row["vDeviceName"].ToString();
                        _clsClientDevice.iTrackerType = Convert.ToInt32(row["iTrackerType"].ToString());
                        _clsClientDevice.vTrackerTypeName = row["vTrackerTypeName"].ToString();
                        _clsClientDevice.vColor = row["vColor"].ToString();
                        _clsClientDevice.vMake = row["vMake"].ToString();
                        _clsClientDevice.vModel = row["vModel"].ToString();
                        _clsClientDevice.bStatus = Convert.ToBoolean(row["bStatus"].ToString());
                        _clsClientDevice.nIMEINo = row["ImeiNumber"].ToString();
                        _clsClientDevice.nGSMNo = row["SimCardId"].ToString();
                        _clsClientDevice.nSimSetialNo = row["SerialNumber"].ToString();
                        _clsClientDevice.iUnitID = Convert.ToInt32(row["iUnitID"].ToString());
                        _clsClientDevice.vIsConfigurable = ""; //row["bIsConfigurable"].ToString(),
                        _clsClientDevice.vUnitName = row["vUnitName"].ToString();
                        _clsClientDevice.iParent = Convert.ToInt32(row["iParent"].ToString());
                        _clsClientDevice.UploadedAssetPhoto = filename;
                        _clsClientDevice.ifk_AssignedAssetId = Convert.ToInt32(row["AssetId"].ToString());
                        _clsClientDevice.vDescription = row["vDescription"].ToString();
                        _clsClientDevice.hSerialNumber = row["hSerialNumber"].ToString();
                        _clsClientDevice.HardwareID = 0;
                        _clsClientDevice.IsSoftwareIgnition = Convert.ToBoolean(row["IsSoftwareIgnition"]);
                        _clsClientDevice.ReportingTimeZone = row["ReportingTimeZone"].ToString();
                        _clsClientDevice.IgnitionOverride = Convert.ToInt32(row["IgnitionOverride"]);
                        _clsClientDevice.SoftwareOdometer = Convert.ToBoolean(row["SoftwareOdometer"]);
                        _clsClientDevice.CustomID = row["CustomId"].ToString();
                        _clsClientDevice.CustomUrl1 = row["custom_url1"].ToString();
                        _clsClientDevice.CustomUrl2 = row["custom_url2"].ToString();
                        _clsClientDevice.CustomUrl3 = row["custom_url3"].ToString();
                        _clsClientDevice.DateDeact = Convert.ToDateTime(row["deactivation_date"]);
                        _clsClientDevice.WorkModeColor = Convert.ToString(row["work_mode_color"]);
                        _clsClientDevice.AssetIcon = Convert.ToString(row["asset_icon"]);
                        _clsClientDevice.incab_phone = Convert.ToString(row["in_cab_phone_number"]);


                        _clsClientDevice.AssetDetailsMaster = GetAssetItemDetails(ds.Tables[0]);
                          
                        lstResellerDevice.Add(_clsClientDevice);
                    }
                }



            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SelectAssetDetails()", ex.Message + ex.StackTrace);
            }

            return lstResellerDevice;
        }

        public El_AssetDetailsMaster GetAssetItemDetails(DataTable dt)
        {

            var _El_AssetDetailsMaster = new El_AssetDetailsMaster();

            try
            {
                foreach (DataRow dr in dt.Rows)
                {

                    _El_AssetDetailsMaster.ipk_device_id = dr["ifkAssetId"] == DBNull.Value ? default(int) : Convert.ToInt32(dr["ifkAssetId"]);
                    _El_AssetDetailsMaster.owner_name = Convert.ToString(dr["Asset_Owner_Name"]);
                    _El_AssetDetailsMaster.owner_national_id = Convert.ToString(dr["Asset_Owner_ID"]);
                    _El_AssetDetailsMaster.owner_phone = Convert.ToString(dr["Asset_Owner_Phone_Number"]);
                    _El_AssetDetailsMaster.owner_gender = Convert.ToString(dr["Owner_Gender"]);


                    //device 
                    _El_AssetDetailsMaster.registeration_plate = Convert.ToString(dr["Asset_Reg_Number"]);
                    _El_AssetDetailsMaster.make = Convert.ToString(dr["Asset_Make"]);
                    _El_AssetDetailsMaster.model = Convert.ToString(dr["Asset_Model"]);

                    _El_AssetDetailsMaster.color = Convert.ToString(dr["Asset_Color"]);
                    _El_AssetDetailsMaster.chasis_no = Convert.ToString(dr["Asset_Chasis_Number"]);
                    _El_AssetDetailsMaster.year = Convert.ToInt32(dr["year"] == DBNull.Value ? default(int) : dr["year"]);
                    _El_AssetDetailsMaster.engine_no = Convert.ToString(dr["engine_no"]);
                    _El_AssetDetailsMaster.vin = Convert.ToString(dr["vin"]);
                    _El_AssetDetailsMaster.no_of_tyres = Convert.ToInt32(dr["no_of_tyres"] == DBNull.Value ? default(int) : dr["no_of_tyres"]);
                    _El_AssetDetailsMaster.tyre_size = Convert.ToString(dr["tyre_size"]);
                    _El_AssetDetailsMaster.cargo_capacity = Convert.ToString(dr["cargo_capacity"]);
                    _El_AssetDetailsMaster.asset_type = Convert.ToString(dr["asset_type"]);
                    _El_AssetDetailsMaster.no_of_pessengers = Convert.ToInt32(dr["no_of_pessengers"] == DBNull.Value ? default(int) : dr["no_of_pessengers"]);

                    _El_AssetDetailsMaster.engine_size = Convert.ToDouble(dr["Asset_Engine_Size"] == DBNull.Value ? default(int) : dr["Asset_Engine_Size"]);


                    //fitting and fitting company 
                    _El_AssetDetailsMaster.fitting_company_name = Convert.ToString(dr["Fitting_Company_Name"]);
                    _El_AssetDetailsMaster.fitting_company_email = Convert.ToString(dr["Fitting_Company_Email_Address"]);
                    _El_AssetDetailsMaster.fitting_company_phone = Convert.ToString(dr["Fitting_Company_Phone_Number"]);
                    _El_AssetDetailsMaster.fitting_company_reg_no = Convert.ToString(dr["Fitting_Company_No"]);
                    _El_AssetDetailsMaster.fitting_date = Convert.ToDateTime(dr["Fitting_Date"] == DBNull.Value ? null : dr["Fitting_Date"]);

                    _El_AssetDetailsMaster.fitting_location = Convert.ToString(dr["Fitting_Location"]);
                    _El_AssetDetailsMaster.fitting_certificate_no = Convert.ToString(dr["Fitting_Certificate_Number"]);
                    _El_AssetDetailsMaster.fitter_name = Convert.ToString(dr["Fitter_Name"]);
                    _El_AssetDetailsMaster.fitter_national_id = Convert.ToString(dr["Fitter_ID"]);
                    _El_AssetDetailsMaster.fitter_phone_no = Convert.ToString(dr["Fitter_Phone_number"]);


                    //fuel 
                    _El_AssetDetailsMaster.fuel_type = Convert.ToString(dr["fuel_type"]);
                    _El_AssetDetailsMaster.fuel_grade = Convert.ToString(dr["fuel_grade"]);
                    _El_AssetDetailsMaster.tank_capacity = Convert.ToDouble(dr["tank_capacity"] == DBNull.Value ? default(int) : dr["tank_capacity"]);
                    _El_AssetDetailsMaster.fuel_consumption = Convert.ToDouble(dr["fuel_consumption"] == DBNull.Value ? default(int) : dr["fuel_consumption"]);



                    //insurance 
                    _El_AssetDetailsMaster.insurance_policy_no = Convert.ToString(dr["insurance_policy_no"]);
                    _El_AssetDetailsMaster.insurance_expiry_date = Convert.ToDateTime(dr["insurance_expiry_date"] == DBNull.Value ? null : dr["insurance_expiry_date"]);
                    _El_AssetDetailsMaster.insurance_policy_no_two = Convert.ToString(dr["insurance_policy_no_two"]);
                    _El_AssetDetailsMaster.insurance_expiry_date_two = Convert.ToDateTime(dr["insurance_expiry_date_two"] == DBNull.Value ? null : dr["insurance_expiry_date_two"]);



                    //Puchace/selling 
                    _El_AssetDetailsMaster.purchase_note = Convert.ToString(dr["purchase_notes"]);
                    _El_AssetDetailsMaster.purchase_date = Convert.ToDateTime(dr["purchase_date"] == DBNull.Value ? null : dr["purchase_date"]);
                    _El_AssetDetailsMaster.selling_note = Convert.ToString(dr["selling_notes"]);
                    _El_AssetDetailsMaster.selling_date = Convert.ToDateTime(dr["selling_date"] == DBNull.Value ? null : dr["selling_date"]);
                    //operation = Convert.ToString(dr[""]);
                    // ipk_device_id = Convert.ToString(dr[""]);



                }

            }
            catch (Exception ex)
            {
                var exception = ex.Message;
            }

            return _El_AssetDetailsMaster;
        }
        public string RestoreDefaultAssetImage()
        {
            string result = "";
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[1].Value = iTrackerType;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[3].Value = ifkDeviceID;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "AssetPhoto", param);
                result = param[2].Value.ToString();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "RestoreAssetImageDefault()", ex.Message + ex.StackTrace);
            }

            return result;
        }

        public List<clsClientDevice> GetDeviceSetUpSettings()
        {
            DataSet ds = new DataSet();
            List<clsClientDevice> lstDeviceSettings = new List<clsClientDevice>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;


                param[1] = new SqlParameter("@iUnitID", SqlDbType.Int);
                param[1].Value = iUnitID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstDeviceSettings.Add(new clsClientDevice { DeviceCommandText = dr["CommandText"].ToString(), DeviceCommandDescription = dr["Description"].ToString() });
                    }
                }

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetDeviceSetUpSettings()", ex.Message + ex.StackTrace);
            }

            return lstDeviceSettings;
        }

        public List<clsClientDevice> GetSoftwareDrivenSettings()
        {
            DataSet ds = new DataSet();
            List<clsClientDevice> lstDeviceSettings = new List<clsClientDevice>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;


                param[1] = new SqlParameter("@iUnitID", SqlDbType.Int);
                param[1].Value = iUnitID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                    }
                }

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetSoftwareDrivenSettings()", ex.Message + ex.StackTrace);
            }

            return lstDeviceSettings;
        }
        public DataSet GetHardwareDrivenSettings()
        {
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iUnitID", SqlDbType.Int);
                param[1].Value = iUnitID;

                param[2] = new SqlParameter("@nIMEINo", SqlDbType.BigInt);
                param[2].Value = nIMEINo;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetHardwareDrivenSettings()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public DataSet GetDeviceRules()
        {
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iUnitID", SqlDbType.Int);
                param[1].Value = iUnitID;

                param[2] = new SqlParameter("@nIMEINo", SqlDbType.BigInt);
                param[2].Value = nIMEINo;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetMaxRoadSpeedPCRule()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public string SaveDeviceRules()
        {
            string result = "";
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iUnitID", SqlDbType.Int);
                param[1].Value = iUnitID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@nIMEINo", SqlDbType.BigInt);
                param[3].Value = nIMEINo;

                param[4] = new SqlParameter("@iMaxRoadSpeedPercent", SqlDbType.Int);
                param[4].Value = iMaxRoadSpeedPercent;

                param[5] = new SqlParameter("@iMaxDeviceSpeedLimit", SqlDbType.Float);
                param[5].Value = iMaxDeviceSpeedLimit;



                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);
                result = param[2].Value.ToString();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveMaxRoadSpeedPc()", ex.Message + ex.StackTrace);
            }

            return result;
        }
        public DataSet GetAssignedHardwareDrivers()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;


                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetAssignedHardwareDrivers()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public string UnAssignDriver()
        {
            string result = "";
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[3];
            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@AssignedDriverID", SqlDbType.Int);
                param[1].Value = intId;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);
                result = param[2].Value.ToString();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetAssignedHardwareDrivers()", ex.Message + ex.StackTrace);
            }

            return result;
        }

        public string SaveAssignedDriver()
        {
            string result = "";
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[5];
            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@AssignedDriverID", SqlDbType.Int);
                param[1].Value = ipkDriverID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[3].Value = ipkDeviceID;

                param[4] = new SqlParameter("@isDefaultDriver", SqlDbType.Bit);
                param[4].Value = isDefaultDriver;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);
                result = param[2].Value.ToString();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "AssignNewDriver()", ex.Message + ex.StackTrace);
            }

            return result;
        }

        public DataSet GetDriversForDDL()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;


                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@iParent", SqlDbType.Int);
                param[2].Value = iParent;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetDriversForDDL()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public List<clsClientDevice> GetDeviceCommands()
        {
            DataSet ds = new DataSet();
            List<clsClientDevice> commands = new List<clsClientDevice>();
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iUnitID", SqlDbType.Int);
                param[1].Value = iUnitID;

                param[2] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[2].Value = ipkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in ds.Tables[0].Rows)
                    {
                        commands.Add(
                            new clsClientDevice(
                                Convert.ToInt32(drRow["iUnitID"]),
                                drRow["CommandType"].ToString(),
                                drRow["CommandText"].ToString(),
                                drRow["vName"].ToString()
                            )
                        );
                    }
                }

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetDeviceCommands()", ex.Message + ex.StackTrace);
            }

            return commands;
        }
        public DataSet GetInputOutputDevicesDetails()
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();

            try
            {
                param[0] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[0].Value = ifkDeviceID;

                param[1] = new SqlParameter("@iUnitID", SqlDbType.BigInt);
                param[1].Value = iUnitID;

                param[2] = new SqlParameter("@ifkCompanyId", SqlDbType.BigInt);
                param[2].Value = iParent;

                //ds2 = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["MasterDeviceSettingServer"], CommandType.StoredProcedure, "sp_GetMasterSuperAdminDevices", param);
                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_GetInputOutputDevicesDetails", param);

                //for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                //{
                //    DataRow dr = ds2.Tables[0].Rows[i];
                //    ds.Tables[2].ImportRow(dr);
                //}


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetResellerDeviceList()", ex.Message + ex.StackTrace);
            }
            return ds;
        }


        public List<clsClientDevice> SelectClientDriver()
        {
            DataSet ds = new DataSet();
            List<clsClientDevice> lstResellerDevice = new List<clsClientDevice>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;


                param[1] = new SqlParameter("@ipkDriverID", SqlDbType.Int);
                param[1].Value = ipkDriverID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_DriverDetails", param);


                if (ds.Tables.Count > 0)
                {
                    //string filePath = ConfigurationManager.AppSettings["AssetPhotoFolderPath"].ToString();
                    //string resizeFilePath = ConfigurationManager.AppSettings["AssetPhotoResizeFolderPath"].ToString();
                    //string uploadedLogo = "";

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                       
                        string filename = ds.Tables[0].Rows[0]["vLogoNameNew"].ToString();  

                        Func<string, bool> HasCorrectDateFormat = (a) => DateTime.TryParseExact(a, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

                        var _licenseExpiryDate = Convert.ToString(row["license_expiry_date"]);

                        if (HasCorrectDateFormat(_licenseExpiryDate))
                            _licenseExpiryDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_licenseExpiryDate), TimeZoneID).ToString("dd/MM/yyyy HH:mm:ss");

                        

                        lstResellerDevice.Add(new clsClientDevice(Convert.ToInt32(row["ipkDriverID"].ToString()),
                                                      row["DriverName"].ToString(),
                                                      Convert.ToInt32(row["iTrackerType"].ToString()),
                                                      row["vTrackerTypeName"].ToString(),
                                                      Convert.ToInt32(row["DriverIDType"].ToString()),
                                                      row["DriverIDNo"].ToString(),
                                                      row["DriverPersonalID"].ToString(),
                                                      Convert.ToBoolean(row["bStatus"].ToString()),
                                                      row["LicenceNo"].ToString(),
                                                      row["ContactNo"].ToString(),
                                                      Convert.ToDateTime(row["DriverDOB"].ToString()).ToString("dd MMM yyyy"),
                                                      Convert.ToInt32(row["ipkiButtonID"].ToString()),
                                                      row["iButtonType"].ToString(),
                                                      Convert.ToInt32(row["iParent"].ToString()),
                                                      filename,
                                                      _licenseExpiryDate
                                                      ));
                    }
                }


            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SelectClientDriver()", ex.Message + ex.StackTrace);
            }

            return lstResellerDevice;
        }

        public List<clsClientDevice> FillClientDevices()
        {
            DataSet ds = new DataSet();
            List<clsClientDevice> lstResellerDevice = new List<clsClientDevice>();
            try
            {
                //SqlParameter[] param = new SqlParameter[2];

                //param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                //param[0].Value = Operation;

                //param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                //param[1].Value = iParent;


                //ds = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString(), CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                lstResellerDevice.Add(new clsClientDevice("Select", -1));
                lstResellerDevice.Add(new clsClientDevice("None", 0));

                SqlParameter[] param1 = new SqlParameter[3];
                DataSet dsnew = new DataSet();

                param1[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param1[0].Value = 26;

                param1[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param1[1].Value = iParent;

                param1[2] = new SqlParameter("@ifk_AssignedAssetId", SqlDbType.Int);
                param1[2].Value = ifk_AssignedAssetId;

                dsnew = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param1);
                if (dsnew.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsnew.Tables[0].Rows)
                    {
                        lstResellerDevice.Add(new clsClientDevice(
                                                        row["Name"].ToString(),
                                                        Convert.ToInt32(row["Id"])
                                                  ));
                    }
                }


                //if (ds.Tables.Count > 0)
                //{
                //    lstResellerDevice.Add(new clsClientDevice("Select", -1));
                //    lstResellerDevice.Add(new clsClientDevice("None", 0));

                //    foreach (DataRow row in ds.Tables[0].Rows)
                //    {
                //        SqlParameter[] param1 = new SqlParameter[2];
                //        DataSet dsnew = new DataSet();

                //        param1[0] = new SqlParameter("@Operation", SqlDbType.Int);
                //        param1[0].Value = 26;

                //        param1[1] = new SqlParameter("@iParent", SqlDbType.Int);
                //        param1[1].Value = Convert.ToInt32(row["iParent"].ToString());


                //        dsnew = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString(), CommandType.StoredProcedure, "Newsp_ClientDevices", param1);

                //        lstResellerDevice.Add(new clsClientDevice(
                //                                            dsnew.Tables[0].Rows[0]["Name"].ToString(),
                //                                            dsnew.Tables[0].Rows[0]["Id"].ToString()
                //                                      ));
                //    }
                //}



            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SelectResellerDevice()", ex.Message + ex.StackTrace);
            }

            return lstResellerDevice;
        }
        public DataSet GetUnits()
        {

            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 11;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetUnits()", ex.Message + ex.StackTrace);
            }
            return ds;


        }
        public string SaveDeviceDesttings()
        {

            SqlParameter[] param = new SqlParameter[15];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@nIMEINo", SqlDbType.VarChar);
                param[1].Value = nIMEINo;

                param[2] = new SqlParameter("@nGSMNo", SqlDbType.VarChar);
                param[2].Value = nGSMNo;

                param[3] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[3].Value = ipkDeviceID;

                param[4] = new SqlParameter("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@iUnitID", SqlDbType.Int);
                param[5].Value = iUnitID;

                param[6] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[6].Value = iParent;

                if (ifk_AssignedAssetId == -1 || ifk_AssignedAssetId == 0)
                {
                    param[7] = new SqlParameter("@ifk_AssignedAssetId", SqlDbType.BigInt);
                    param[7].Value = System.DBNull.Value;
                }
                else
                {
                    param[7] = new SqlParameter("@ifk_AssignedAssetId", SqlDbType.BigInt);
                    param[7].Value = ifk_AssignedAssetId;
                }

                if (ifkSimCardId == -1 || ifkSimCardId == 0)
                {
                    param[8] = new SqlParameter("@ifkSimCardId", SqlDbType.BigInt);
                    param[8].Value = System.DBNull.Value;
                }
                else
                {
                    param[8] = new SqlParameter("@ifkSimCardId", SqlDbType.BigInt);
                    param[8].Value = ifkSimCardId;
                }

                param[9] = new SqlParameter("@vDescription", SqlDbType.VarChar);
                param[9].Value = vDescription;

                param[10] = new SqlParameter("@hSerialNumber", SqlDbType.VarChar);
                param[10].Value = hSerialNumber;

                param[11] = new SqlParameter("@IsSoftwareIgnition", SqlDbType.Bit);
                param[11].Value = IsSoftwareIgnition;

                param[12] = new SqlParameter("@ReportingTimeZone", SqlDbType.NVarChar);
                param[12].Value = ReportingTimeZone;

                param[13] = new SqlParameter("@IgnitionOverride", SqlDbType.Int);
                param[13].Value = IgnitionOverride;

                param[14] = new SqlParameter("@SoftwareOdometer", SqlDbType.Bit);
                param[14].Value = SoftwareOdometer;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveDeviceDesttings()", ex.Message + ex.StackTrace);
            }
            return param[4].Value.ToString();


        }

        public string SaveHardwareBasicSettings()
        {

            SqlParameter[] param = new SqlParameter[13];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@nIMEINo", SqlDbType.VarChar);
                param[1].Value = nIMEINo;

                param[2] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[2].Value = ipkDeviceID;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;

                param[4] = new SqlParameter("@iUnitID", SqlDbType.Int);
                param[4].Value = iUnitID;

                param[5] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[5].Value = iParent;

                param[6] = new SqlParameter("@vDescription", SqlDbType.VarChar);
                param[6].Value = vDescription;

                param[7] = new SqlParameter("@hSerialNumber", SqlDbType.VarChar);
                param[7].Value = hSerialNumber;

                param[8] = new SqlParameter("@IsSoftwareIgnition", SqlDbType.Bit);
                param[8].Value = IsSoftwareIgnition;

                param[9] = new SqlParameter("@ReportingTimeZone", SqlDbType.VarChar);
                param[9].Value = ReportingTimeZone;

                param[10] = new SqlParameter("@IgnitionOverride", SqlDbType.Int);
                param[10].Value = IgnitionOverride;

                param[11] = new SqlParameter("@SoftwareOdometer", SqlDbType.Bit);
                param[11].Value = SoftwareOdometer;

                param[12] = new SqlParameter("@secondary_imei", SqlDbType.VarChar);
                param[12].Value = SecondaryImei;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveHardwareBasicSettings()", ex.Message + ex.StackTrace);
            }
            return param[3].Value.ToString();
        }

        public string SaveHardwareAssetSettings()
        {

            SqlParameter[] param = new SqlParameter[4];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                if (ifk_AssignedAssetId == -1 || ifk_AssignedAssetId == 0)
                {
                    param[3] = new SqlParameter("@ifk_AssignedAssetId", SqlDbType.BigInt);
                    param[3].Value = System.DBNull.Value;
                }
                else
                {
                    param[3] = new SqlParameter("@ifk_AssignedAssetId", SqlDbType.BigInt);
                    param[3].Value = ifk_AssignedAssetId;
                }

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveHardwareAssetSettings()", ex.Message + ex.StackTrace);
            }
            return param[2].Value.ToString();
        }

        public string SaveHardwareSimSettings()
        {

            SqlParameter[] param = new SqlParameter[4];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                if (ifkSimCardId == -1 || ifkSimCardId == 0)
                {
                    param[3] = new SqlParameter("@ifkSimCardId", SqlDbType.BigInt);
                    param[3].Value = System.DBNull.Value;
                }
                else
                {
                    param[3] = new SqlParameter("@ifkSimCardId", SqlDbType.BigInt);
                    param[3].Value = ifkSimCardId;
                }


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveHardwareSimSettings()", ex.Message + ex.StackTrace);
            }
            return param[2].Value.ToString();
        }

        public string SaveSimcardDetails()
        {
            SqlParameter[] param = new SqlParameter[12];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Id", SqlDbType.BigInt);
                param[0].Value = nSimCardId;

                param[1] = new SqlParameter("@SerialNumber", SqlDbType.NVarChar, -1);
                param[1].Value = nSimCardSerialNumber;

                param[2] = new SqlParameter("@GsmNumber", SqlDbType.VarChar, 50);
                param[2].Value = nSimCardGSMNumber;

                param[3] = new SqlParameter("@NetworkProvider", SqlDbType.NVarChar, -1);
                param[3].Value = nSimCardNetworkProvider;

                param[4] = new SqlParameter("@Description", SqlDbType.NVarChar, -1);
                param[4].Value = nSimCardDesc;

                param[5] = new SqlParameter("@Puk1Code", SqlDbType.VarChar, 50);
                param[5].Value = nSimCardPuk1Code;

                param[6] = new SqlParameter("@Puk2Code", SqlDbType.VarChar, 50);
                param[6].Value = nSimCardPuk2Code;

                param[7] = new SqlParameter("@Pin1", SqlDbType.VarChar, 50);
                param[7].Value = nSimCardPin1;

                param[8] = new SqlParameter("@Pin2", SqlDbType.VarChar, 50);
                param[8].Value = nSimCardPin2;

                param[9] = new SqlParameter("@userId", SqlDbType.BigInt);
                param[9].Value = intUserId;

                param[10] = new SqlParameter("@ifkCompanyId", SqlDbType.BigInt);
                param[10].Value = intCompanyId;

                param[11] = new SqlParameter("@ipkDeviceID", SqlDbType.BigInt);
                param[11].Value = ipkDeviceID;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "SaveSimcardDetails", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveSimCardNotes()", ex.Message + ex.StackTrace);
            }
            return param[5].Value.ToString();


        }

        public string SaveSimCardNotes()
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@ifk_SimCardId", SqlDbType.BigInt);
                param[0].Value = nSimCardId;

                param[1] = new SqlParameter("@ifk_UserId", SqlDbType.BigInt);
                param[1].Value = intUserId;

                param[2] = new SqlParameter("@Note", SqlDbType.NVarChar, -1);
                param[2].Value = nSimCardNote;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_AddSimCardNotes", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveSimCardNotes()", ex.Message + ex.StackTrace);
            }
            return "1";


        }


        public string SaveAssetUploadedImage()
        {
            SqlParameter[] param = new SqlParameter[6];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[3].Value = iCreatedBy;

                //param[4] = new SqlParameter("@ImageByteAray", SqlDbType.VarBinary);
                //param[4].Value = ImageByteArray;

                param[4] = new SqlParameter("@LogoName", SqlDbType.VarChar);
                param[4].Value = LogoName;

                param[5] = new SqlParameter("@ipkDriverID", SqlDbType.Int);
                param[5].Value = ipkDriverID;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "AssetPhoto", param);

                if (param[2].Value.ToString() == "1")
                {
                    returnstring = "Saved successful";
                }

                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveAssetUploadedImage()", ex.Message + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }

        public string SaveCustomUploadedImage()
        {
            SqlParameter[] param = new SqlParameter[7];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ResellerId", SqlDbType.Int);
                param[1].Value = ifkResellerID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[3].Value = iCreatedBy;

                param[4] = new SqlParameter("@CustomUrl1Photo", SqlDbType.VarChar);
                param[4].Value = CustomUrl1Photo;

                param[5] = new SqlParameter("@CustomUrl2Photo", SqlDbType.VarChar);
                param[5].Value = CustomUrl2Photo;

                param[6] = new SqlParameter("@CustomUrl3Photo", SqlDbType.VarChar);
                param[6].Value = CustomUrl3Photo;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "AssetPhoto", param);

                if (param[2].Value.ToString() == "1")
                {
                    returnstring = "Saved successful";
                }

                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveCustomUploadedImage()", ex.Message + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }



        public string SaveHardwareImage()
        {
            SqlParameter[] param = new SqlParameter[4];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@ImageByteAray", SqlDbType.VarBinary);
                param[3].Value = ImageByteArray;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "AssetPhoto", param);

                if (param[2].Value.ToString() == "1")
                {
                    returnstring = "Saved successful";
                }

                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveHardwareImage()", ex.Message + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }


        public DataSet GetHardareDevicesForSimCardList()
        {

            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[0].Value = iParent;

                param[1] = new SqlParameter("@ifkSimCardId", SqlDbType.BigInt);
                param[1].Value = ifkSimCardId;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "GetHardareDevicesForSimCardList", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetHardareDevicesForSimCardList()", ex.Message + ex.StackTrace);
            }
            return ds;


        }

        public DataSet GetSimCardForHardareDevicesList()
        {

            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[0].Value = iParent;

                param[1] = new SqlParameter("@Id", SqlDbType.BigInt);
                param[1].Value = ifkSimCardId;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "GetSimCardForHardareDevicesList", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetSimCardForHardareDevicesList()", ex.Message + ex.StackTrace);
            }
            return ds;


        }

        public string DeleteClientSimCard()
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Id", SqlDbType.BigInt);
                param[0].Value = nSimCardId;

                returnstr = SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "DeleteClientSimCardList", param).ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "DeleteClientSimCard()", ex.Message + ex.StackTrace);
            }
            return returnstr;

        }

        public List<clsClientDevice> GetSimCardDetails()
        {
            DataSet ds = new DataSet();
            List<clsClientDevice> lstSimCardDetails = new List<clsClientDevice>();
            try
            {
                SqlParameter[] param = new SqlParameter[1];

                param[0] = new SqlParameter("@Id", SqlDbType.Int);
                param[0].Value = nSimCardId;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "GetSimCardDetails", param);


                if (ds.Tables.Count > 0)
                {

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstSimCardDetails.Add(new clsClientDevice
                        {
                            nSimCardId = Convert.ToInt32(row["Id"].ToString()),
                            nSimCardSerialNumber = Convert.ToString(row["SerialNumber"]),
                            nSimCardGSMNumber = Convert.ToString(row["GsmNumber"]),
                            nSimCardNetworkProvider = Convert.ToString(row["NetworkProvider"]),
                            nSimCardDesc = Convert.ToString(row["Description"]),
                            nSimCardPuk1Code = Convert.ToString(row["Puk1Code"]),
                            nSimCardPuk2Code = Convert.ToString(row["Puk2Code"]),
                            nSimCardPin1 = Convert.ToString(row["Pin1"]),
                            nSimCardPin2 = Convert.ToString(row["Pin2"]),
                            ifkDeviceID = Convert.ToInt32(row["DeviceId"]),
                            nIMEINo = Convert.ToString(row["ImeiNumber"]),
                            iParent = Convert.ToInt32(row["ifkCompanyId"]),
                            vDeviceName = Convert.ToString(row["vDeviceName"]),
                        });
                    }
                }



            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SelectResellerDevice()", ex.Message + ex.StackTrace);
            }

            return lstSimCardDetails;
        }

        public string UnassignHardwareDeviceForSimCard()
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Id", SqlDbType.BigInt);
                param[0].Value = nSimCardId;

                returnstr = SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "UnassignHardwareDeviceForSimCard", param).ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "UnassignHardwareDeviceForSimCard()", ex.Message + ex.StackTrace);
            }
            return returnstr;

        }

        public string UnassignGSMNumber()
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Id", SqlDbType.BigInt);
                param[0].Value = nSimCardId;

                returnstr = SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "UnassignGSMNumber", param).ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "UnassignGSMNumber()", ex.Message + ex.StackTrace);
            }
            return returnstr;

        }

        public string UnassignClientAsset()
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Id", SqlDbType.BigInt);
                param[0].Value = nSimCardId;

                returnstr = SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "UnassignClientAsset", param).ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "UnassignClientAsset()", ex.Message + ex.StackTrace);
            }
            return returnstr;

        }


        public string DeleteclientHardwareDevice()
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[3];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceId", SqlDbType.BigInt);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_DeleteHardwareDevices", param);
                returnstr = param[2].Value.ToString();
            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "DeleteclientHardwareDevice()", ex.Message + ex.StackTrace);
            }

            return returnstr;
        }


        public string CheckClientAssetExist()
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar);
                param[0].Value = vDeviceName;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[2].Value = iParent;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
                returnstr = param[3].Value.ToString();

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "CheckClientAssetExist()", ex.Message + ex.StackTrace);
            }

            return returnstr;
        }

        public string CheckDeviceExist()
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@IMEINo", SqlDbType.BigInt);
                param[0].Value = @nIMEINo;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[2].Value = iParent;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;



                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);


                var error = Convert.ToString(param[3].Value);
                returnstr = error;

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "CheckClientAssetExist()", ex.Message + ex.StackTrace);
            }

            return returnstr;
        }

        public string SaveAllWizardDetails()
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[33];
            try
            {
                param[0] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar);
                param[0].Value = vDeviceName;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@iParent", SqlDbType.Int);
                param[2].Value = iParent;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;

                param[4] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[4].Value = iTrackerType;

                param[5] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[5].Value = bStatus;

                param[6] = new SqlParameter("@iCreatedBy", SqlDbType.BigInt);
                param[6].Value = iCreatedBy;

                param[7] = new SqlParameter("@vMake", SqlDbType.VarChar);
                param[7].Value = vMake;

                param[8] = new SqlParameter("@vModel", SqlDbType.VarChar);
                param[8].Value = vModel;

                param[9] = new SqlParameter("@nSimSerialNo", SqlDbType.VarChar);
                param[9].Value = nSimCardSerialNumber;

                param[10] = new SqlParameter("@nGSMNo", SqlDbType.VarChar);
                param[10].Value = nSimCardGSMNumber;

                param[11] = new SqlParameter("@nSimCardDesc", SqlDbType.VarChar);
                param[11].Value = nSimCardDesc;

                param[12] = new SqlParameter("@nSimCardNetworkProvider", SqlDbType.VarChar);
                param[12].Value = nSimCardNetworkProvider;

                param[13] = new SqlParameter("@nSimCardPuk1Code", SqlDbType.VarChar);
                param[13].Value = nSimCardPuk1Code;

                param[14] = new SqlParameter("@nSimCardPuk2Code", SqlDbType.VarChar);
                param[14].Value = nSimCardPuk2Code;

                param[15] = new SqlParameter("@nSimCardPin1", SqlDbType.VarChar);
                param[15].Value = nSimCardPin1;

                param[16] = new SqlParameter("@nSimCardPin2", SqlDbType.VarChar);
                param[16].Value = nSimCardPin2;

                param[17] = new SqlParameter("@IMEINo", SqlDbType.VarChar);
                param[17].Value = nIMEINo;

                param[18] = new SqlParameter("@vDescription", SqlDbType.VarChar);
                param[18].Value = vDescription;

                param[19] = new SqlParameter("@iUnitID", SqlDbType.VarChar);
                param[19].Value = iUnitID;

                param[20] = new SqlParameter("@vColor", SqlDbType.VarChar);
                param[20].Value = vColor;

                param[21] = new SqlParameter("@hSerialNumber", SqlDbType.VarChar);
                param[21].Value = hSerialNumber;

                param[22] = new SqlParameter("@USerPhNo", SqlDbType.VarChar);
                param[22].Value = USerPhNo;

                param[23] = new SqlParameter("@APNNO", SqlDbType.VarChar);
                param[23].Value = APNNO;

                param[24] = new SqlParameter("@GPRSUser", SqlDbType.VarChar);
                param[24].Value = GPRSUser;

                param[25] = new SqlParameter("@GPRSPassword", SqlDbType.VarChar);
                param[25].Value = GPRSPassword;

                param[26] = new SqlParameter("@MobileNetworkName", SqlDbType.VarChar);
                param[26].Value = MobileNetworkName;

                param[27] = new SqlParameter("@OperatorCode", SqlDbType.VarChar);
                param[27].Value = OperatorCode;

                param[28] = new SqlParameter("@CountryCode", SqlDbType.VarChar);
                param[28].Value = CountryCode;

                param[29] = new SqlParameter("@iMaxRoadSpeedPercent", SqlDbType.Int);
                param[29].Value = iMaxRoadSpeedPercent;

                param[30] = new SqlParameter("@iMaxRoadSpeedLimit", SqlDbType.Int);
                param[30].Value = iMaxRoadSpeedLimit;

                param[31] = new SqlParameter("@DeactDate", SqlDbType.DateTime);
                param[31].Value = DateDeact;

                param[32] = new SqlParameter("@HardwareSerialNo", SqlDbType.VarChar);
                param[32].Value = HardwareSerialNo;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                if (param[3].Value.ToString() == "-2")
                {
                    returnstr = "-2";
                }

                else if (param[3].Value.ToString() == "-3")
                {
                    returnstr = "-3";
                }

                else
                {
                    returnstr = param[3].Value.ToString();
                }


            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "SaveAllWizardDetails()", ex.Message + ex.StackTrace);
            }

            return returnstr;
        }

        public DataSet GetOTACommandsList()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = iUnitID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "GetOTACommandsList()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public DataSet GetOTAActivationStatus()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@IMEINo", SqlDbType.BigInt);
                param[1].Value = nIMEINo;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "DeviceActivation", param);

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "GetOTAActivationStatus()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public DataSet GetHardwareDevices()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;



                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetHardwareDevices()", ex.Message + ex.StackTrace);
            }
            return ds;
        }


        public string SaveWizardSimcardDetails()
        {
            SqlParameter[] param = new SqlParameter[13];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@IMEINo", SqlDbType.BigInt);
                param[0].Value = nIMEINo;

                param[1] = new SqlParameter("@nSimSerialNo", SqlDbType.NVarChar, -1);
                param[1].Value = nSimCardSerialNumber;

                param[2] = new SqlParameter("@nGSMNo", SqlDbType.VarChar, 50);
                param[2].Value = nSimCardGSMNumber;

                param[3] = new SqlParameter("@nSimCardNetworkProvider", SqlDbType.NVarChar, -1);
                param[3].Value = nSimCardNetworkProvider;

                param[4] = new SqlParameter("@nSimCardDesc", SqlDbType.NVarChar, -1);
                param[4].Value = nSimCardDesc;

                param[5] = new SqlParameter("@nSimCardPuk1Code", SqlDbType.VarChar, 50);
                param[5].Value = nSimCardPuk1Code;

                param[6] = new SqlParameter("@nSimCardPuk2Code", SqlDbType.VarChar, 50);
                param[6].Value = nSimCardPuk2Code;

                param[7] = new SqlParameter("@nSimCardPin1", SqlDbType.VarChar, 50);
                param[7].Value = nSimCardPin1;

                param[8] = new SqlParameter("@nSimCardPin2", SqlDbType.VarChar, 50);
                param[8].Value = nSimCardPin2;

                param[9] = new SqlParameter("@ifkUserID", SqlDbType.BigInt);
                param[9].Value = intUserId;

                param[10] = new SqlParameter("@Error", SqlDbType.Int);
                param[10].Direction = ParameterDirection.Output;

                param[11] = new SqlParameter("@Operation", SqlDbType.Int);
                param[11].Value = Operation;

                param[12] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[12].Value = iParent;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveWizardSimcardDetails()", ex.Message + ex.StackTrace);
            }
            return param[10].Value.ToString();
        }

        public DataSet GetDeviceActivationStatus()
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@IMEINo", SqlDbType.BigInt);
                param[1].Value = nIMEINo;

                param[2] = new SqlParameter("@iUnitID", SqlDbType.Int);
                param[2].Value = iUnitID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "DeviceActivation", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetHardwareDevices()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet getSmtpsetting()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 4;

                param[1] = new SqlParameter("@email", SqlDbType.NVarChar);
                param[1].Value = vEmail;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString.ToString(), CommandType.StoredProcedure, "sp_ForgotPassword", param);
                return ds;
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("ForgotPassword.cs", "CheckUserExist()", ex.Message + ex.StackTrace);
                return ds;
            }
        }

        public string CheckAssignedHardware()
        {

            string result = "";
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = new SqlParameter("@ifk_AssignedAssetId", SqlDbType.BigInt);
                param[0].Value = ifk_AssignedAssetId;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[2].Value = iParent;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
                result = param[3].Value.ToString();

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "CheckAssignedHardware()", ex.Message + ex.StackTrace);
            }

            return result;
        }

        public DataSet ShowClientSMSHistory()
        {
            SqlParameter[] param = new SqlParameter[6];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.BigInt);
                param[1].Value = intCompanyId;

                param[2] = new SqlParameter("@SMSLoginMonth", SqlDbType.BigInt);
                param[2].Value = SMSLoginMonth;

                param[3] = new SqlParameter("@SMSLoginYear", SqlDbType.BigInt);
                param[3].Value = SMSLoginYear;

                param[4] = new SqlParameter("@SelectedClientID", SqlDbType.Int);
                param[4].Value = SelectedClientID;

                param[5] = new SqlParameter("@vTimeOffset", SqlDbType.VarChar);
                param[5].Value = vTimeOffset;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_Company", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "ShowClientSMSHistory()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet ShowClientEmailHistory()
        {
            SqlParameter[] param = new SqlParameter[5];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.BigInt);
                param[1].Value = intCompanyId;

                param[2] = new SqlParameter("@EmailLoginMonth", SqlDbType.BigInt);
                param[2].Value = EmailLoginMonth;

                param[3] = new SqlParameter("@EmailLoginYear", SqlDbType.BigInt);
                param[3].Value = EmailLoginYear;

                param[4] = new SqlParameter("@SelectedClientID", SqlDbType.Int);
                param[4].Value = SelectedClientID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_Company", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "ShowClientEmailHistory()", ex.Message + ex.StackTrace);
            }
            return ds;
        }


        public DataSet FillManufacturer()
        {
            SqlParameter[] param = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "FillManufacturer()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetNextHardwareID()
        {
            SqlParameter[] param = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetNextHardwareID()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet LookUpCarrierDetails()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@MobileNetworkName", SqlDbType.NVarChar);
                param[1].Value = MobileNetworkName;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "LookUpCarrierDetails()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetSuperAdminCommands()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.BigInt);
                param[1].Value = ifkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetSuperAdminCommands()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public string SaveSuperAdminCommand()
        {
            SqlParameter[] param = new SqlParameter[8];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@DeviceCommandText", SqlDbType.VarChar);
                param[1].Value = DeviceCommandText;

                param[2] = new SqlParameter("@DeviceSequenceID", SqlDbType.Int);
                param[2].Value = DeviceSequenceID;

                param[3] = new SqlParameter("@DeviceCommandDescription", SqlDbType.VarChar, 255);
                param[3].Value = DeviceCommandDescription;

                param[4] = new SqlParameter("@DeviceCommandID", SqlDbType.Int);
                param[4].Value = DeviceCommandID;

                param[5] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[5].Value = ifkDeviceID;

                param[6] = new SqlParameter("@Error", SqlDbType.Int);
                param[6].Direction = ParameterDirection.Output;

                param[7] = new SqlParameter("@DeviceCommandType", SqlDbType.VarChar, 255);
                param[7].Value = DeviceCommandType;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveSuperAdminSMSCommand()", ex.Message + ex.StackTrace);
            }
            return param[6].Value.ToString();
        }

        public string DeleteSuperAdminCommandRow()
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@DeviceCommandID", SqlDbType.Int);
                param[1].Value = DeviceCommandID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "DeleteSuperAdminSMSCommandRow()", ex.Message + ex.StackTrace);
            }
            return param[2].Value.ToString();
        }

        public DataSet GetOTACommandsSettings()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetOTACommandsSettings()", ex.Message + ex.StackTrace);
            }

            return ds;
        }


        public DataSet GetDeviceCapability()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetDeviceCapability()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public string SaveOTACommandsSettings()
        {
            SqlParameter[] param = new SqlParameter[11];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceID;

                param[2] = new SqlParameter("@ifkCommandLookup", SqlDbType.Int);
                param[2].Value = ifkCommandLookup;

                param[3] = new SqlParameter("@DeviceCommandText", SqlDbType.VarChar);
                param[3].Value = DeviceCommandText;

                param[4] = new SqlParameter("@DeviceCommandType", SqlDbType.VarChar);
                param[4].Value = DeviceCommandType;

                param[5] = new SqlParameter("@DeviceCommandID", SqlDbType.Int);
                param[5].Value = DeviceCommandID;

                param[6] = new SqlParameter("@Error", SqlDbType.Int);
                param[6].Direction = ParameterDirection.Output;

                param[7] = new SqlParameter("@CustomCommandValues", SqlDbType.NVarChar);
                param[7].Value = CustomCommandValues;

                param[8] = new SqlParameter("@IsOutputCmd", SqlDbType.Bit);
                param[8].Value = IsOutputCmd;

                param[9] = new SqlParameter("@DeviceCommandDescription", SqlDbType.VarChar);
                param[9].Value = DeviceCommandDescription;

                param[10] = new SqlParameter("@IsSmsCmd", SqlDbType.Bit);
                param[10].Value = IsSmsCmd;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveOTACommandsSettings()", ex.Message + ex.StackTrace);
            }
            return param[6].Value.ToString();
        }
        public string SaveDeviceCapabilitySettings()
        {
            SqlParameter[] param = new SqlParameter[7];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceID;

                param[2] = new SqlParameter("@IfkEventID", SqlDbType.Int);
                param[2].Value = IfkEventID;

                param[3] = new SqlParameter("@IsSupported", SqlDbType.Bit);
                param[3].Value = IsSupported;

                param[4] = new SqlParameter("@SupportedID", SqlDbType.Int);
                param[4].Value = IsSupportedID;

                param[5] = new SqlParameter("@Error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@isShowOnAlert", SqlDbType.Int);
                param[6].Value = isShowOnAlert;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveDeviceCapabilitySettings()", ex.Message + ex.StackTrace);
            }
            return param[5].Value.ToString();
        }

        public string DeleteOTACommandsSettings()
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@DeviceCommandID", SqlDbType.Int);
                param[1].Value = DeviceCommandID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "DeleteOTACommandsSettings()", ex.Message + ex.StackTrace);
            }
            return param[2].Value.ToString();
        }

        public DataSet GetHardwareRawToCommonIDMapping()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetHardwareRawToCommonIDMapping()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public DataSet GetCommonEventsLookup()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetCommonEventsLookup()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public string SaveHardwareRawToCommonEventIDMapping()
        {
            SqlParameter[] param = new SqlParameter[8];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceID;

                param[2] = new SqlParameter("@ipkDeviceEventCodeId", SqlDbType.Int);
                param[2].Value = ipkDeviceEventCodeId;

                param[3] = new SqlParameter("@vEventId", SqlDbType.VarChar);
                param[3].Value = vEventId;

                param[4] = new SqlParameter("@vEventIdValue", SqlDbType.VarChar);
                param[4].Value = vEventIdValue;

                param[5] = new SqlParameter("@vEventType", SqlDbType.VarChar);
                param[5].Value = vEventType;

                param[6] = new SqlParameter("@ifkCommonEventLookupId", SqlDbType.Int);
                param[6].Value = ifkCommonEventLookupId;

                param[7] = new SqlParameter("@Error", SqlDbType.Int);
                param[7].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveHardwareRawToCommonEventIDMapping()", ex.Message + ex.StackTrace);
            }
            return param[7].Value.ToString();
        }

        public string DeleteHardwareRawToCommonEventIDMapping()
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceEventCodeId", SqlDbType.Int);
                param[1].Value = ipkDeviceEventCodeId;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "DeleteHardwareRawToCommonEventIDMapping()", ex.Message + ex.StackTrace);
            }
            return param[2].Value.ToString();
        }

        public DataSet GetFrequentlyUsedAPNs()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkClientID", SqlDbType.Int);
                param[1].Value = ifkClientID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetFrequentlyUsedAPNs()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }


        public string AssetSaveMaintanance(Int64 ipkDeviceId, string _maintanance_command_name, string _maintanance_command_type, string _maintanance_command_value, string _maintanance_command_Email, wltAppState objSession, wlt_SessionPOCO poco)
        {
            int userId = 0;

            List<clsNewGroup> objLogin = new List<clsNewGroup>();

            string Timezone = objSession.vTimeZoneID;
            userId = objSession.pkUserID;

            if (poco.blnIsLoggedIn == false)
            {
                objLogin.Add(new clsNewGroup("login"));
                //return objLogin;
                return "";
            }
            else
            {

                SqlParameter[] param = new SqlParameter[7];

                try
                {
                    param[0] = new SqlParameter("@_maintanance_command_name", SqlDbType.NVarChar, 255);
                    param[0].Value = _maintanance_command_name;

                    param[1] = new SqlParameter("@_maintanance_command_type", SqlDbType.Int);
                    param[1].Value = _maintanance_command_type;

                    if (int.Parse(_maintanance_command_type) == 2)
                    {

                        param[2] = new SqlParameter("@_maintanance_command_value", SqlDbType.NVarChar);
                        param[2].Value = UserSettings.ConvertLocalDateTimeToUTCDateTime(Convert.ToDateTime(_maintanance_command_value), Timezone);
                    }
                    else
                    {
                        param[2] = new SqlParameter("@_maintanance_command_value", SqlDbType.NChar);
                        param[2].Value = _maintanance_command_value;

                    }

                    param[3] = new SqlParameter("@_maintanance_command_Email", SqlDbType.NVarChar);
                    param[3].Value = _maintanance_command_Email;


                    param[4] = new SqlParameter("@SuccessValue", SqlDbType.Int);
                    param[4].Direction = ParameterDirection.Output;


                    param[5] = new SqlParameter("@ipkDeviceId", SqlDbType.BigInt);
                    param[5].Value = ipkDeviceId;

                    param[6] = new SqlParameter("@user_id", SqlDbType.BigInt);
                    param[6].Value = objSession.pkUserID;


                    //   SqlHelper.ExecuteNonQuery(AppConfiguration.ConnectionString(), "SaveMaintanance_sp", param);
                    SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "SaveMaintananceCommand_sp", param);

                }
                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile("clsClientDevice.cs", "AssetSaveMaintanance()", ex.Message + ex.StackTrace);


                }
                return param[4].Value.ToString();
            }
        }

        public DataSet GetMaintananceCommand(El_Maintenance _El_Maintenance)
        {
            SqlParameter[] param = new SqlParameter[4];
            DataSet ds = new DataSet();

            try
            {

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _El_Maintenance.Operation;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = _El_Maintenance.ifkDeviceID;

                param[2] = new SqlParameter("@ifkDriverID", SqlDbType.Int);
                param[2].Value = _El_Maintenance.ifkDriverID;

                param[3] = new SqlParameter("@ifkattachedID", SqlDbType.Int);
                param[3].Value = _El_Maintenance.ifkAttachmentID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Get_Maintanace_commands", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetSuperAdminCommands()", ex.Message + ex.StackTrace);
            }
            return ds;
        }


        public string UpdateAssetMaintanance(El_Maintenance _El_Maintenance, wltAppState objSession)
        {

            List<clsNewGroup> objLogin = new List<clsNewGroup>();


            SqlParameter[] param = new SqlParameter[10];

            try
            {
                param[0] = new SqlParameter("@MaintenanceItemName", SqlDbType.NVarChar, 255);
                param[0].Value = _El_Maintenance.Command_name;

                param[1] = new SqlParameter("@_maintanance_command_type", SqlDbType.Int);
                param[1].Value = _El_Maintenance.Command_type;


                if (_El_Maintenance.Command_type == "3")
                    _El_Maintenance.Command_value = (TimeSpan.FromHours(Convert.ToDouble(_El_Maintenance.Command_value)).Ticks).ToString();

                param[2] = new SqlParameter("@ParameterValue", SqlDbType.NChar);
                param[2].Value = _El_Maintenance.Command_value;

                param[3] = new SqlParameter("@NotifyEmailAddress", SqlDbType.NVarChar);
                param[3].Value = _El_Maintenance.Command_Email;

                param[4] = new SqlParameter("@id", SqlDbType.Int);
                param[4].Value = _El_Maintenance.ID;


                param[5] = new SqlParameter("@operation", SqlDbType.Int);
                param[5].Value = _El_Maintenance.Operation;

                param[6] = new SqlParameter("@user_id", SqlDbType.Int);
                param[6].Value = objSession.pkUserID;

                param[7] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[7].Value = _El_Maintenance.ipkDeviceId;

                param[8] = new SqlParameter("@ifkDriverID", SqlDbType.Int);
                param[8].Value = _El_Maintenance.ifkDriverID;

                param[9] = new SqlParameter("@ifkattachedID", SqlDbType.Int);
                param[9].Value = _El_Maintenance.ifkAttachmentID;

                //   SqlHelper.ExecuteNonQuery(AppConfiguration.ConnectionString(), "SaveMaintanance_sp", param);
                SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "UpdateAssetMaintanance_sp", param);

            }
            catch (Exception ex)
            {


                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "AssetSaveMaintanance()", ex.Message + ex.StackTrace);
                param[4].Value = 0;


            }
            return param[4].Value.ToString();
        }


        public string deleteMaintananceCommand_cs(string id)
        {

            SqlParameter[] param = new SqlParameter[1];

            try
            {
                param[0] = new SqlParameter("@id", SqlDbType.NVarChar, 255);
                param[0].Value = id;

                //   SqlHelper.ExecuteNonQuery(AppConfiguration.ConnectionString(), "SaveMaintanance_sp", param);
                SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "DeleteAssetMaintanance_sp", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "AssetSaveMaintanance()", ex.Message + ex.StackTrace);


            }
            return param[0].Value.ToString();

        }


        public List<clsClientDevice.AnalogInputs> FillAnalogMappingDDL()
        {
            List<clsClientDevice.AnalogInputs> lstAnalogInputs = new List<AnalogInputs>();
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {

                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = 149;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                lstAnalogInputs.Add(new clsClientDevice.AnalogInputs("Select", -1, "Litres"));

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstAnalogInputs.Add(new clsClientDevice.AnalogInputs(row["vName"].ToString(), Convert.ToInt32(row["id"].ToString()), row["vUnitText"].ToString(), Convert.ToInt32(row["AnalogType"])));
                    }
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "FillAnalogMappingDDL()", ex.Message + ex.StackTrace);
            }


            return lstAnalogInputs;
        }

        public List<clsClientDevice.AnalogSensorInputs> FillAnalogSensorMappingDDL()
        {
            List<clsClientDevice.AnalogSensorInputs> lstAnalogSensorInputs = new List<AnalogSensorInputs>();
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {

                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = 8;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_SupperAdminDevices", param);

                lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Select", "-1"));

                if (ds.Tables.Count > 0)
                {
                    //Analog Inputs
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["iAnalogInputs"]) == 1)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 1", "1:1"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["iAnalogInputs"]) == 2)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 1", "1:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 2", "1:2"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["iAnalogInputs"]) == 3)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 1", "1:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 2", "1:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 3", "1:3"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["iAnalogInputs"]) == 4)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 1", "1:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 2", "1:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 3", "1:3"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 4", "1:4"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["iAnalogInputs"]) == 5)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 1", "1:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 2", "1:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 3", "1:3"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 4", "1:4"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 5", "1:5"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["iAnalogInputs"]) == 6)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 1", "1:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 2", "1:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 3", "1:3"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 4", "1:4"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 5", "1:5"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Analog Input 6", "1:6"));
                    }

                    //Serial Inputs
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["SerialInputs"]) == 1)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Serial 1", "2:1"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["SerialInputs"]) == 2)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Serial 1", "2:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Serial 2", "2:2"));
                    }

                    //One Wire
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["b1Wire"]) == true)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("1Wire input", "3:0"));
                    }

                    //BtTemperature
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtTemperature"]) == 1)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 1", "4:1"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtTemperature"]) == 2)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 1", "4:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 2", "4:2"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtTemperature"]) == 3)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 1", "4:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 2", "4:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 3", "4:3"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtTemperature"]) == 4)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 1", "4:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 2", "4:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 3", "4:3"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Temperature 4", "4:4"));
                    }
                    //BtHumidity
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtHumidity"]) == 1)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 1", "5:1"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtHumidity"]) == 2)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 1", "5:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 2", "5:2"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtHumidity"]) == 3)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 1", "5:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 2", "5:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 3", "5:3"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtHumidity"]) == 4)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 1", "5:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 2", "5:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 3", "5:3"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Humidity 4", "5:4"));
                    }
                    //BtBattery
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtBattery"]) == 1)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 1", "6:1"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtBattery"]) == 2)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 1", "6:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 2", "6:2"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtBattery"]) == 3)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 1", "6:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 2", "6:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 3", "6:3"));
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["BtBattery"]) == 4)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 1", "6:1"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 2", "6:2"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 3", "6:3"));
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("Bluetooth Battery 4", "6:4"));
                    }

                    //CanBus Data
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["ObdStatus"]) == true)
                    {
                        lstAnalogSensorInputs.Add(new clsClientDevice.AnalogSensorInputs("CANbus Data", "7:0"));
                    }
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "FillAnalogSensorMappingDDL()", ex.Message + ex.StackTrace);
            }


            return lstAnalogSensorInputs;
        }

        public DataSet GetAnalogInputDataForEdit()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = 150;

                param[1] = new SqlParameter("@AnalogInputID", SqlDbType.Int);
                param[1].Value = AnalogInputID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "FillAnalogMappingDDL()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public string SaveAnalogInputsOutputs()
        {
            SqlParameter[] param = new SqlParameter[30];
            string result = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifk_IO_Analog_Master_ID", SqlDbType.Int);
                param[1].Value = ifk_IO_Analog_Master_ID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@iMinVolts", SqlDbType.Float);
                param[3].Value = iMinVolts;

                param[4] = new SqlParameter("@iMaxVolts", SqlDbType.Float);
                param[4].Value = iMaxVolts;

                param[5] = new SqlParameter("@iMinValue", SqlDbType.Float);
                param[5].Value = iMinValue;

                param[6] = new SqlParameter("@iMaxValue", SqlDbType.Float);
                param[6].Value = iMaxValue;

                param[7] = new SqlParameter("@iAnalogNumber", SqlDbType.SmallInt);
                param[7].Value = iAnalogNumber;

                param[8] = new SqlParameter("@iAveraging", SqlDbType.SmallInt);
                param[8].Value = iAveraging;

                param[9] = new SqlParameter("@isMaxCurrentlyOutOfRange", SqlDbType.Bit);
                param[9].Value = isMaxCurrentlyOutOfRange;

                param[10] = new SqlParameter("@ifkDeviceID", SqlDbType.BigInt);
                param[10].Value = ifkDeviceID;

                param[11] = new SqlParameter("@swEvent_Enabled", SqlDbType.Bit);
                param[11].Value = swEvent_Enabled;

                param[12] = new SqlParameter("@swEvent_iMinRange", SqlDbType.Int);
                param[12].Value = swEvent_iMinRange;

                param[13] = new SqlParameter("@swEvent_iMaxRange", SqlDbType.Int);
                param[13].Value = swEvent_iMaxRange;

                param[14] = new SqlParameter("@swEvent_iRangeTriggerCount", SqlDbType.Int);
                param[14].Value = swEvent_iRangeTriggerCount;

                param[15] = new SqlParameter("@swEvent_iSuddenDrop", SqlDbType.Int);
                param[15].Value = swEvent_iSuddenDrop;

                param[16] = new SqlParameter("@swEvent_iSuddenIncrease", SqlDbType.Int);
                param[16].Value = swEvent_iSuddenIncrease;

                param[17] = new SqlParameter("@isMinCurrentlyOutOfRange", SqlDbType.Bit);
                param[17].Value = isMinCurrentlyOutOfRange;

                param[18] = new SqlParameter("@IO_Analog_Mapping_ID", SqlDbType.Int);
                param[18].Value = IO_Analog_Mapping_ID;

                param[19] = new SqlParameter("@iParent", SqlDbType.Int);
                param[19].Value = iParent;

                param[20] = new SqlParameter("@ignoreWhenIgnOff", SqlDbType.Bit);
                param[20].Value = IgnoreWhenIgnOff;

                param[21] = new SqlParameter("@SensorType", SqlDbType.Int);
                param[21].Value = SensorType;

                param[22] = new SqlParameter("@DeviceId1Wire", SqlDbType.VarChar);
                param[22].Value = DeviceId1Wire;

                param[23] = new SqlParameter("@ifkCanBusType", SqlDbType.Int);
                param[23].Value = ifkCanBusType;

                param[24] = new SqlParameter("@vDeviceCalibration", SqlDbType.VarChar);
                param[24].Value = vDeviceCalibration;


                param[25] = new SqlParameter("@fuel_tank_capacity", SqlDbType.Float);
                param[25].Value = FuelTankCapacity;




                param[26] = new SqlParameter("@ignore_ign_on_mins", SqlDbType.Int);
                param[26].Value = ignore_ign_on_mins;

                param[27] = new SqlParameter("@spike_threshold", SqlDbType.Int);
                param[27].Value = spike_threshold;

                param[28] = new SqlParameter("@spike_threshold_return", SqlDbType.Int);
                param[28].Value = spike_threshold_return;

                param[29] = new SqlParameter("@spike_sample_points", SqlDbType.Int);
                param[29].Value = spike_sample_points;



                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_AddAnalogTempInputOutputDevice", param);

                result = param[2].Value.ToString();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "FillAnalogMappingDDL()", ex.Message + ex.StackTrace);
            }

            return result;
        }


        public class AnalogInputs
        {
            public string vName { get; set; }
            public int id { get; set; }
            public string vUnits { get; set; }

            public int SensorType { get; set; }
            public AnalogInputs(string vName, int id, string vUnits)
            {
                this.vName = vName;
                this.id = id;
                this.vUnits = vUnits;
            }
            public AnalogInputs(string vName, int id, string vUnits, int _SensorType)
            {
                this.vName = vName;
                this.id = id;
                this.vUnits = vUnits;
                this.SensorType = _SensorType;
            }
        }


        public class AnalogSensorInputs
        {
            public string vName { get; set; }
            public string value { get; set; }
            public AnalogSensorInputs(string vName, string value)
            {
                this.vName = vName;
                this.value = value;

            }
        }

        public string SaveBatchDevices()
        {
            string returnstr = "-2";

            SqlParameter[] param = new SqlParameter[9];
            try
            {
                param[0] = new SqlParameter("@Error", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Output;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar);
                param[2].Value = vDeviceName;

                param[3] = new SqlParameter("@iParent", SqlDbType.Int);
                param[3].Value = iParent;

                param[4] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[4].Value = bStatus;

                param[5] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[5].Value = iCreatedBy;

                param[6] = new SqlParameter("@nGSMNo", SqlDbType.VarChar);
                param[6].Value = nSimCardGSMNumber;

                param[7] = new SqlParameter("@IMEINo", SqlDbType.VarChar);
                param[7].Value = nIMEINo;

                param[8] = new SqlParameter("@iUnitID", SqlDbType.VarChar);
                param[8].Value = iUnitID;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                if (param[0].Value.ToString() == "-2")
                {
                    returnstr = "-2";
                }

                else if (param[0].Value.ToString() == "-3")
                {
                    returnstr = "-3";
                }

                else if (param[0].Value.ToString() == "")
                {
                    returnstr = "-2";
                }

                else
                {
                    returnstr = param[0].Value.ToString();
                }


            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "SaveAllWizardDetails()", ex.Message + ex.StackTrace);
            }

            return returnstr;
        }


        public string SaveManufacturerName()
        {
            string returnstr = "";

            SqlParameter[] param = new SqlParameter[3];
            try
            {
                param[0] = new SqlParameter("@Error", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Output;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@vManufacturerName", SqlDbType.VarChar);
                param[2].Value = vManufacturerName;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);

                returnstr = param[0].Value.ToString();

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "SaveManufacturerName()", ex.Message + ex.StackTrace);
            }

            return returnstr;
        }

        public string SavePwaSettings()
        {
            string returnstr = "";

            SqlParameter[] param = new SqlParameter[9];
            try
            {
                param[0] = new SqlParameter("@Error", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Output;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@PwaIsEnabled", SqlDbType.Bit);
                param[2].Value = PwaIsEnabled;

                param[3] = new SqlParameter("@PwaName", SqlDbType.VarChar);
                param[3].Value = PwaName;

                param[4] = new SqlParameter("@PwaDesc", SqlDbType.VarChar);
                param[4].Value = PwaDesc;

                param[5] = new SqlParameter("@PwaLogoUrl", SqlDbType.VarChar);
                param[5].Value = PwaLogoUrl;

                param[6] = new SqlParameter("@PwaBackgroundColor", SqlDbType.VarChar);
                param[6].Value = PwaBackgroundColor;

                param[7] = new SqlParameter("@PwaThemeColor", SqlDbType.VarChar);
                param[7].Value = PwaThemeColor;

                param[8] = new SqlParameter("@ResellerId", SqlDbType.Int);
                param[8].Value = ifkResellerID;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_PwaSettings", param);

                returnstr = param[0].Value.ToString();

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "SavePwaSettings()", ex.Message + ex.StackTrace);
            }

            return returnstr;
        }

        public DataSet GetPwaSettings()
        {
            var ds = new DataSet();

            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ResellerId", SqlDbType.VarChar);
                param[1].Value = ifkResellerID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_PwaSettings", param);


            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "GetPwaSettings()", ex.Message + ex.StackTrace);
            }

            return ds;
        }

        public DataSet ShowHighAssetUsage()
        {
            SqlParameter[] param = new SqlParameter[6];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@SMSLoginMonth", SqlDbType.Int);
                param[1].Value = SMSLoginMonth;

                param[2] = new SqlParameter("@SMSLoginYear", SqlDbType.Int);
                param[2].Value = SMSLoginYear;

                param[3] = new SqlParameter("@UsageDate", SqlDbType.Date);
                param[3].Value = UsageDate;

                param[4] = new SqlParameter("@IsMonthSelected", SqlDbType.Bit);
                param[4].Value = IsMonthSelected;

                param[5] = new SqlParameter("@iParentID", SqlDbType.Int);
                param[5].Value = iParent;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_Company", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "ShowSuperAdminTools()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public string SaveClientDataForwardSettings(int Operation, bool Enabled, int ClientID, string URL, int Format, int AssetID)
        {
            string returnstr = "";

            SqlParameter[] param = new SqlParameter[7];
            try
            {
                param[0] = new SqlParameter("@Error", SqlDbType.Int);
                param[0].Direction = ParameterDirection.Output;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@iParent", SqlDbType.Int);
                param[2].Value = ClientID;

                param[3] = new SqlParameter("@URL", SqlDbType.VarChar);
                param[3].Value = URL;

                param[4] = new SqlParameter("@FormatID", SqlDbType.Int);
                param[4].Value = Format;

                param[5] = new SqlParameter("@Enabled", SqlDbType.Bit);
                param[5].Value = Enabled;

                param[6] = new SqlParameter("@ifkAssetId", SqlDbType.Int);
                param[6].Value = AssetID;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);

                returnstr = param[0].Value.ToString();

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsClientDevices.cs", "SaveClientDataForwardSettings()", ex.Message + ex.StackTrace);
            }

            return returnstr;
        }

        public DataSet ShowDataForwardingSettings(int Operation, int ClientID, int AssetID)
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = ClientID;

                param[2] = new SqlParameter("@ifkAssetId", SqlDbType.Int);
                param[2].Value = AssetID;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_HardwareDeviceSettings", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "ShowDataForwardingSettings()", ex.Message + ex.StackTrace);
            }
            return ds;
        }

        public El_AssetDetailsMaster GetAssetExtraDetails(El_AssetDetailsMaster _El_AssetDetailsMaster)
        {
            SqlParameter[] param = new SqlParameter[5];
            string result = "";

            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = _El_AssetDetailsMaster.operation;

                param[1] = new SqlParameter("@ipk_device_id", SqlDbType.Int);
                param[1].Value = _El_AssetDetailsMaster.ipk_device_id;

                param[2] = new SqlParameter("@client_id", SqlDbType.NVarChar);
                param[2].Value = _El_AssetDetailsMaster.client_id;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;




                var ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_AdditionalAssetDetails", param);

                foreach (DataTable dt in ds.Tables)
                    _El_AssetDetailsMaster = GetAssetItemDetails(dt);

                result = param[3].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetAssetExtraDetails()", ex.Message + ex.StackTrace);

                result = ex.Message;
            }
            return _El_AssetDetailsMaster;
        }


        public Hashtable GetDefaultFittingDetails(El_AssetDetailsMaster _El_AssetDetailsMaster)
        {

            var fittingItems = new Hashtable();

            SqlParameter[] param = new SqlParameter[5];

            string result = "";

            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = _El_AssetDetailsMaster.operation;

                param[1] = new SqlParameter("@ipk_device_id", SqlDbType.Int);
                param[1].Value = _El_AssetDetailsMaster.ipk_device_id;

                param[2] = new SqlParameter("@owner_national_id", SqlDbType.NVarChar);
                param[2].Value = _El_AssetDetailsMaster.owner_national_id;


                param[3] = new SqlParameter("@client_id", SqlDbType.NVarChar);
                param[3].Value = _El_AssetDetailsMaster.client_id;


                param[4] = new SqlParameter("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;


                var ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_AdditionalAssetDetails", param);

                var tableCountIndex = 0;

                foreach (DataTable dt in ds.Tables)
                {
                    if (tableCountIndex == 0)
                    {

                        var _default_fitter_companies = new List<El_AssetDetailsMaster>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["Fitting_Company_Name"])))
                                _default_fitter_companies.Add(new El_AssetDetailsMaster
                                {
                                    fitting_company_name = Convert.ToString(dr["Fitting_Company_Name"]),
                                    fitting_company_email = Convert.ToString(dr["Fitting_Company_Email_Address"]),
                                    fitting_company_phone = Convert.ToString(dr["Fitting_Company_Phone_Number"]),
                                    fitting_company_reg_no = Convert.ToString(dr["Fitting_Company_No"]),
                                    fitting_date = Convert.ToDateTime(dr["Fitting_Date"] == DBNull.Value ? "2000-01-01" : dr["fitting_date"]),
                                    fitting_certificate_no = Convert.ToString(dr["Fitting_Certificate_Number"]),
                                });
                        }


                        fittingItems.Add("default_companies", _default_fitter_companies);
                    }

                    if (tableCountIndex == 1)
                    {
                        var _default_fitter = new List<El_AssetDetailsMaster>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["Fitter_Name"])))
                                _default_fitter.Add(new El_AssetDetailsMaster
                                {

                                    fitter_name = Convert.ToString(dr["Fitter_Name"]),
                                    fitter_national_id = Convert.ToString(dr["Fitter_ID"]),
                                    fitter_phone_no = Convert.ToString(dr["Fitter_Phone_number"]),

                                });
                        }



                        fittingItems.Add("default_fitter_users", _default_fitter);
                    }

                    if (tableCountIndex == 2)   // fitting locations /centers
                    {
                        var _default_fitter = new List<El_AssetDetailsMaster>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["Fitting_Location"])))
                                _default_fitter.Add(new El_AssetDetailsMaster
                                {
                                    fitting_location = Convert.ToString(dr["Fitting_Location"]),

                                });
                        }


                        fittingItems.Add("default_fitter_centers", _default_fitter);
                    }


                    tableCountIndex++;
                }

                result = param[3].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetDefaultFittingDetails()", ex.Message + ex.StackTrace);

                result = ex.Message;
            }
            return fittingItems;
        }


        public string SaveAdditionalAssetDetails(El_AssetDetailsMaster _El_AssetDetailsMaster)
        {
            SqlParameter[] param = new SqlParameter[44];
            string result = "";

            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = _El_AssetDetailsMaster.operation;

                param[1] = new SqlParameter("@ipk_device_id", SqlDbType.Int);
                param[1].Value = _El_AssetDetailsMaster.ipk_device_id;

                param[2] = new SqlParameter("@owner_name", SqlDbType.VarChar);
                param[2].Value = _El_AssetDetailsMaster.owner_name;

                param[3] = new SqlParameter("@owner_national_id", SqlDbType.NVarChar);
                param[3].Value = _El_AssetDetailsMaster.owner_national_id;

                param[4] = new SqlParameter("@owner_phone", SqlDbType.NVarChar);
                param[4].Value = _El_AssetDetailsMaster.owner_phone;

                param[5] = new SqlParameter("@registeration_plate", SqlDbType.NVarChar);
                param[5].Value = _El_AssetDetailsMaster.registeration_plate;

                param[6] = new SqlParameter("@chasis_no", SqlDbType.NVarChar);
                param[6].Value = _El_AssetDetailsMaster.chasis_no;

                param[7] = new SqlParameter("@fuel_type", SqlDbType.NVarChar);
                param[7].Value = _El_AssetDetailsMaster.fuel_type;

                param[8] = new SqlParameter("@EngineSize", SqlDbType.NVarChar);
                param[8].Value = EngineSize;

                param[9] = new SqlParameter("@fitting_company_name", SqlDbType.NVarChar);
                param[9].Value = _El_AssetDetailsMaster.fitting_company_name;

                param[10] = new SqlParameter("@fitting_company_phone", SqlDbType.NVarChar);
                param[10].Value = _El_AssetDetailsMaster.fitting_company_phone;

                param[11] = new SqlParameter("@fitting_company_email", SqlDbType.NVarChar);
                param[11].Value = _El_AssetDetailsMaster.fitting_company_email;

                param[12] = new SqlParameter("@fitting_company_reg_no", SqlDbType.NVarChar);
                param[12].Value = _El_AssetDetailsMaster.fitting_company_reg_no;

                param[13] = new SqlParameter("@fitting_date", SqlDbType.DateTime);
                param[13].Value = _El_AssetDetailsMaster.fitting_date;

                param[14] = new SqlParameter("@fitting_location", SqlDbType.NVarChar);
                param[14].Value = _El_AssetDetailsMaster.fitting_location;

                param[15] = new SqlParameter("@fitting_certificate_no", SqlDbType.NVarChar);
                param[15].Value = _El_AssetDetailsMaster.fitting_certificate_no;

                param[16] = new SqlParameter("@fitter_name", SqlDbType.NVarChar);
                param[16].Value = _El_AssetDetailsMaster.fitter_name;

                param[17] = new SqlParameter("@fitter_national_id", SqlDbType.NVarChar);
                param[17].Value = _El_AssetDetailsMaster.fitter_national_id;

                param[18] = new SqlParameter("@fitter_phone_no", SqlDbType.NChar);
                param[18].Value = _El_AssetDetailsMaster.fitter_phone_no;

                param[19] = new SqlParameter("@color", SqlDbType.NVarChar);
                param[19].Value = _El_AssetDetailsMaster.color;


                param[20] = new SqlParameter("@Make", SqlDbType.NVarChar);
                param[20].Value = _El_AssetDetailsMaster.make;

                param[21] = new SqlParameter("@Model", SqlDbType.NChar);
                param[21].Value = _El_AssetDetailsMaster.model;

                param[22] = new SqlParameter("@Year", SqlDbType.Int);
                param[22].Value = _El_AssetDetailsMaster.year;


                param[23] = new SqlParameter("@engine_no", SqlDbType.VarChar);
                param[23].Value = _El_AssetDetailsMaster.engine_no;


                param[24] = new SqlParameter("@vin", SqlDbType.VarChar);
                param[24].Value = _El_AssetDetailsMaster.vin;


                param[25] = new SqlParameter("@no_of_tyres", SqlDbType.VarChar);
                param[25].Value = _El_AssetDetailsMaster.no_of_tyres;

                param[26] = new SqlParameter("@tyre_size", SqlDbType.NVarChar);
                param[26].Value = _El_AssetDetailsMaster.tyre_size;

                param[27] = new SqlParameter("@cargo_capacity", SqlDbType.NVarChar);
                param[27].Value = _El_AssetDetailsMaster.cargo_capacity;

                param[28] = new SqlParameter("@asset_type", SqlDbType.VarChar);
                param[28].Value = _El_AssetDetailsMaster.asset_type;


                param[29] = new SqlParameter("@Owner_Gender", SqlDbType.VarChar);
                param[29].Value = _El_AssetDetailsMaster.owner_gender;


                param[30] = new SqlParameter("@no_of_pessengers", SqlDbType.VarChar);
                param[30].Value = _El_AssetDetailsMaster.no_of_pessengers;


                param[31] = new SqlParameter("@fuel_grade", SqlDbType.VarChar);
                param[31].Value = _El_AssetDetailsMaster.fuel_grade;

                param[32] = new SqlParameter("@tank_capacity", SqlDbType.VarChar);
                param[32].Value = _El_AssetDetailsMaster.tank_capacity;

                param[33] = new SqlParameter("@fuel_consumption", SqlDbType.VarChar);
                param[33].Value = _El_AssetDetailsMaster.fuel_consumption;

                param[34] = new SqlParameter("@insurance_policy_no", SqlDbType.VarChar);
                param[34].Value = _El_AssetDetailsMaster.insurance_policy_no;

                param[35] = new SqlParameter("@insurance_policy_date", SqlDbType.VarChar);
                param[35].Value = _El_AssetDetailsMaster.insurance_expiry_date?.ToString("yyyy-MM-dd HH:mm:ss"); ;

                param[36] = new SqlParameter("@insurance_policy_no_two", SqlDbType.VarChar);
                param[36].Value = _El_AssetDetailsMaster.insurance_policy_no_two;

                param[37] = new SqlParameter("@insurance_policy_date_two", SqlDbType.VarChar);
                param[37].Value = _El_AssetDetailsMaster.insurance_expiry_date_two?.ToString("yyyy-MM-dd HH:mm:ss");




                param[38] = new SqlParameter("@purchase_note", SqlDbType.VarChar);
                param[38].Value = _El_AssetDetailsMaster.purchase_note;

                param[39] = new SqlParameter("@purchase_date", SqlDbType.VarChar);
                param[39].Value = _El_AssetDetailsMaster.purchase_date?.ToString("yyyy-MM-dd HH:mm:ss"); ;

                param[40] = new SqlParameter("@selling_note", SqlDbType.VarChar);
                param[40].Value = _El_AssetDetailsMaster.selling_note;

                param[41] = new SqlParameter("@selling_date", SqlDbType.VarChar);
                param[41].Value = _El_AssetDetailsMaster.selling_date?.ToString("yyyy-MM-dd HH:mm:ss");

                param[42] = new SqlParameter("@engine_size", SqlDbType.Float);
                param[42].Value = _El_AssetDetailsMaster.engine_size;

                param[43] = new SqlParameter("@Error", SqlDbType.Int);
                param[43].Direction = ParameterDirection.Output;




                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_AdditionalAssetDetails", param);

                result = param[43].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveAdditionalAssetDetails()", ex.Message + ex.StackTrace);

                result = ex.Message;
            }
            return result;
        }

        public string GetGsmNumber()
        {
            SqlParameter[] param = new SqlParameter[2];
            string result = "";

            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = 168;

                param[1] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[1].Value = ifkDeviceID;

                var ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetGsmNumber()", ex.Message + ex.StackTrace);

                result = ex.Message;
            }
            return result;
        }

        public DataSet FillWorkMode()
        {

            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@iParent", SqlDbType.Int);
                param[0].Value = iParent;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = 164;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "FillWorkMode()", ex.Message + ex.StackTrace);
            }
            return ds;


        }

        public string SaveAssetIcon()
        {
            SqlParameter[] param = new SqlParameter[4];
            string result = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@Error", SqlDbType.Int);
                param[1].Direction = ParameterDirection.Output;

                param[2] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[2].Value = ipkDeviceID;

                param[3] = new SqlParameter("@AssetIcon", SqlDbType.VarChar);
                param[3].Value = AssetIcon;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                result = param[1].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveAssetAdditionalDetails()", ex.Message + ex.StackTrace);

                result = ex.Message;
            }

            return result;
        }

        public DataSet GetWorkModeTrig()
        {

            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[0].Value = ipkDeviceID;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = 166;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "GetWorkModeTrig()", ex.Message + ex.StackTrace);
            }
            return ds;

        }

        public string SaveWorkModeTrig()
        {
            SqlParameter[] param = new SqlParameter[4];
            string result = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@Error", SqlDbType.Int);
                param[1].Direction = ParameterDirection.Output;

                param[2] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[2].Value = ipkDeviceID;

                param[3] = new SqlParameter("@WorkModeTriggers", SqlDbType.VarChar);
                param[3].Value = WorkModeTriggers;


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

                result = param[1].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveWorkModeTrig()", ex.Message + ex.StackTrace);

                result = ex.Message;
            }

            return result;
        }


    }
}