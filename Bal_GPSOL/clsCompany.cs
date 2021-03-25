using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WLT.DataAccessLayer;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsCompany : IDisposable
    {
        private int _Operation;
        private int _SubOperation;
        private int _ipkCompanyID;
        private string _vCompanyName;
        private int _IstTrial;
        private string _DateToDisableAccess;


        private string _vCompanyLicencedURL;
        private string _vHelpUrl;
        private int _vLanguage;
        private int _IsClient;
        private string _vLanguageName;
        private string _vImage;
        private bool _bStatus;
        private DateTime _dEntryDate = DateTime.Now;
        private DateTime _dUpdateDate = DateTime.Now;
        private string _vIDs;
        private int _ifkThemeID;
        private int _Error;
        private bool _IsEdit;
        private string _p;
        private string _vAddress;
        private string _vCity;
        private string _vRegion;
        private int _ifkCountryID;
        private int _iParentID;
        private int _iCreatedBy;
        private string _vpkCountry;
        private string _vThemeName;
        private byte[] _ImageByteArray;
        private int _NewHeight;
        private int _NewWidth;
        private long _vDeviceId;
        private string _vParameterName;
        private int _vParameterCode;
        private int _vParameterValue;
        private string _vParameterText;
        private string _vFavicon;
        private byte[] _FaviconByteArray;
        private string _FaviconName;
        private string _vBackgroundImage;
        private byte[] _BackgroundImageByteArray;
        private string _vBackgroundImageName;

        public string LogoName { get; set; }

        private string _CustomLink1;
        private string _CustomLink2;
        private string _CustomLink3;
        private string _CustomLink1Text;
        private string _CustomLink2Text;
        private string _CustomLink3Text;
        private string _CustomLink1ToolTip;
        private string _CustomLink2ToolTip;
        private string _CustomLink3ToolTip;
        private string _CustomUrl1Photo;
        private string _CustomUrl2Photo;
        private string _CustomUrl3Photo;
        private string _vPageheading;
        private int _ifkUserId;
        private int _SearchType;
        private string _SearchValue;
        private bool _IsMiniReseller;

        private int _ShowFor;
        private string _StartDate;
        private string _EndDate;
        private bool _IsDateSelected;
        private bool _IncludeMiniResellerCount;

        private int _LoggedInUserID;

        private bool _bUseHttps;

        private string _SmsValues;
        private string _SmsFields;
        private string _SmsFailureNotifyEmail;
        private bool _IsDenyData;
        private bool _IsStreetMapEnabled;
        private string _vReportName;
        private int _ifkReportTypeId;
        private int _ifkVersionID;


        public int ifkGeocoderId { get; set; }
        public string GeocoderUserKey { get; set; }
        public bool bUseResellerGeocoder { get; set; }

        public bool IsDenyData
        {
            get { return _IsDenyData; }
            set { _IsDenyData = value; }
        }

        public bool IsStreetMapEnabled
        {
            get { return _IsStreetMapEnabled; }
            set { _IsStreetMapEnabled = value; }
        }

        public int ifkUserTypeId { get; set; }
        public string CustomJsonString { get; set; }

        public string vMiniresellerList { get; set; }

        public string vWebHooks { get; set; }
        public bool isSet { get; set; }
        public List<clsCompany> lstclsCompany { get; set; }


        private readonly wlt_Config _wlt_AppConfig;

        public string Connectionstring { get; set; }


        public string vReportName
        {
            get { return _vReportName; }
            set { _vReportName = value; }
        }

        public int ifkReportTypeId
        {
            get { return _ifkReportTypeId; }
            set { _ifkReportTypeId = value; }
        }

        public int ifkVersionID
        {
            get { return _ifkVersionID; }
            set { _ifkVersionID = value; }
        }

        public bool bUseHttps
        {
            get { return _bUseHttps; }
            set { _bUseHttps = value; }
        }

        public string SmsFailureNotifyEmail
        {
            get { return _SmsFailureNotifyEmail; }
            set { _SmsFailureNotifyEmail = value; }
        }

        public string SmsValues
        {
            get { return _SmsValues; }
            set { _SmsValues = value; }
        }
        public string SmsFields
        {
            get { return _SmsFields; }
            set { _SmsFields = value; }
        }

        public int ShowFor
        {
            get { return _ShowFor; }
            set { _ShowFor = value; }
        }

        public string StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        public string EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        public bool IsDateSelected
        {
            get { return _IsDateSelected; }
            set { _IsDateSelected = value; }
        }


        public int LoggedInUserID
        {
            get { return _LoggedInUserID; }
            set { _LoggedInUserID = value; }
        }

        public bool IsMiniReseller
        {
            get { return _IsMiniReseller; }
            set { _IsMiniReseller = value; }
        }

        public bool IncludeMiniResellerCount
        {
            get { return _IncludeMiniResellerCount; }
            set { _IncludeMiniResellerCount = value; }
        }

        protected string _TimeZoneID;
        public string TimeZoneID
        {
            get { return _TimeZoneID; }
            set { _TimeZoneID = value; }
        }

        public int SearchType
        {
            get { return _SearchType; }
            set { _SearchType = value; }
        }

        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }


        public string DateToDisableAccess
        {
            get
            {
                return this._DateToDisableAccess;
            }
            set
            {
                this._DateToDisableAccess = value;
            }
        }
        public string FaviconName
        {
            get
            {
                return this._FaviconName;
            }
            set
            {
                this._FaviconName = value;
            }
        }

        public string Displayname
        {
            get
            {
                return this._displayname;
            }
            set
            {
                this._displayname = value;
            }
        }

        public bool UseDefaultSmtp
        {
            get
            {
                return this._UseDefaultSMTP;
            }
            set
            {
                this._UseDefaultSMTP = value;
            }
        }

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

        public byte[] FaviconByteArray
        {
            get
            {
                return _FaviconByteArray;
            }
            set
            {
                _FaviconByteArray = value;
            }
        }

        public byte[] BackgroundImageByteArray
        {
            get
            {
                return _BackgroundImageByteArray;
            }
            set
            {
                _BackgroundImageByteArray = value;
            }
        }

        public string vBackgroundImage
        {
            get
            {
                return _vBackgroundImage;
            }
            set
            {
                _vBackgroundImage = value;
            }
        }

        public string vBackgroundImageName
        {
            get
            {
                return _vBackgroundImageName;
            }
            set
            {
                _vBackgroundImageName = value;
            }
        }


        public int Operation
        {
            get
            {
                return _Operation;
            }
            set
            {
                _Operation = value;
            }
        }

        public int SubOperation
        {
            get
            {
                return _SubOperation;
            }
            set
            {
                _SubOperation = value;
            }
        }

        private int _KeepDataFor;
        public int KeepDataFor { get { return _KeepDataFor; } set { _KeepDataFor = value; } }

        public int IstTrial { get { return _IstTrial; } set { _IstTrial = value; } }

        public string CustomLink1 { get { return _CustomLink1; } set { _CustomLink1 = value; } }
        public string CustomLink2 { get { return _CustomLink2; } set { _CustomLink2 = value; } }
        public string CustomLink3 { get { return _CustomLink3; } set { _CustomLink3 = value; } }
        public string CustomLink1Text { get { return _CustomLink1Text; } set { _CustomLink1Text = value; } }
        public string CustomLink2Text { get { return _CustomLink2Text; } set { _CustomLink2Text = value; } }
        public string CustomLink3Text { get { return _CustomLink3Text; } set { _CustomLink3Text = value; } }
        public string CustomLink1ToolTip { get { return _CustomLink1ToolTip; } set { _CustomLink1ToolTip = value; } }
        public string CustomLink2ToolTip { get { return _CustomLink2ToolTip; } set { _CustomLink2ToolTip = value; } }
        public string CustomLink3ToolTip { get { return _CustomLink3ToolTip; } set { _CustomLink3ToolTip = value; } }
        public string CustomUrl1Photo { get { return _CustomUrl1Photo; } set { _CustomUrl1Photo = value; } }
        public string CustomUrl2Photo { get { return _CustomUrl2Photo; } set { _CustomUrl2Photo = value; } }
        public string CustomUrl3Photo { get { return _CustomUrl3Photo; } set { _CustomUrl3Photo = value; } }

        public string vParameterName { get { return _vParameterName; } set { _vParameterName = value; } }
        public int vParameterCode { get { return _vParameterCode; } set { _vParameterCode = value; } }
        public int vParameterValue { get { return _vParameterValue; } set { _vParameterValue = value; } }
        public string vParameterText { get { return _vParameterText; } set { _vParameterText = value; } }

        public long vDeviceId { get { return _vDeviceId; } set { _vDeviceId = value; } }
        public int ipkCompanyID { get { return _ipkCompanyID; } set { _ipkCompanyID = value; } }
        public string vCompanyName { get { return _vCompanyName; } set { _vCompanyName = value; } }

        public string vCompanyLicencedURL { get { return _vCompanyLicencedURL; } set { _vCompanyLicencedURL = value; } }
        public string vHelpUrl { get { return _vHelpUrl; } set { _vHelpUrl = value; } }
        public int vLanguage { get { return _vLanguage; } set { _vLanguage = value; } }
        public int IsClient { get { return _IsClient; } set { _IsClient = value; } }

        public string vLanguageName { get { return _vLanguageName; } set { _vLanguageName = value; } }
        public string vImage { get { return _vImage; } set { _vImage = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public DateTime dEntryDate { get { return _dEntryDate; } set { _dEntryDate = value; } }
        public DateTime dUpdateDate { get { return _dUpdateDate; } set { _dUpdateDate = value; } }
        public string vIDs { get { return _vIDs; } set { _vIDs = value; } }
        public int ifkThemeID { get { return _ifkThemeID; } set { _ifkThemeID = value; } }
        public int Error { get { return _Error; } set { _Error = value; } }
        public bool IsEdit { get { return _IsEdit; } set { _IsEdit = value; } }

        public string vAddress { get { return _vAddress; } set { _vAddress = value; } }
        public string vCity { get { return _vCity; } set { _vCity = value; } }
        public string vRegion { get { return _vRegion; } set { _vRegion = value; } }
        public int ifkCountryID { get { return _ifkCountryID; } set { _ifkCountryID = value; } }
        public int iParentID { get { return _iParentID; } set { _iParentID = value; } }
        public int iCreatedBy { get { return _iCreatedBy; } set { _iCreatedBy = value; } }
        public string vpkCountry { get { return _vpkCountry; } set { _vpkCountry = value; } }
        public string p { get { return _p; } set { _p = value; } }
        public string vThemeName { get { return _vThemeName; } set { _vThemeName = value; } }
        public string vFavicon { get { return _vFavicon; } set { _vFavicon = value; } }
        public string vPageheading { get { return _vPageheading; } set { _vPageheading = value; } }
        public int ifkUserId { get { return _ifkUserId; } set { _ifkUserId = value; } }


        public bool IncludeMini_ResellerClients { get; set; }

        public string WorkModeName { get; set; }
        public int WorkModeId { get; set; }
        public string WorkModeColor { get; set; }

        public bool IsTwoWaySMS { get; set; }

        public string SmsOriginator { get; set; }

        public string SmsClientPassword { get; set; }

        public string SmsClientID { get; set; }     


        public List<int> client_role_ids { get; set; } = new List<int>();


        #region SMTP Properties

        private bool _UseDefaultSMTP;
        private string _displayname;
        private string _smtpServer;
        private string _from;
        private string _password;
        private string _to;
        private int _port;
        private string _subject;
        private string _message;
        private bool _enableSsl;
        private bool _isBodyHtml;
        private bool _isRequiredAuthontication;
        private bool _EmailEnabled;

        public bool EmailEnabled
        {
            get
            {
                return this._EmailEnabled;
            }
            set
            {
                this._EmailEnabled = value;
            }
        }

        public bool IsRequiredAuthontication
        {
            get
            {
                return this._isRequiredAuthontication;
            }
            set
            {
                this._isRequiredAuthontication = value;
            }
        }

        public bool IsBodyHtml
        {
            get
            {
                return this._isBodyHtml;
            }
            set
            {
                this._isBodyHtml = value;
            }
        }

        public bool EnableSsl
        {
            get
            {
                return this._enableSsl;
            }
            set
            {
                this._enableSsl = value;
            }
        }

        public string Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }

        public string Subject
        {
            get
            {
                return this._subject;
            }
            set
            {
                this._subject = value;
            }
        }

        public int Port
        {
            get
            {
                return this._port;
            }
            set
            {
                this._port = value;
            }
        }

        public string To
        {
            get
            {
                return this._to;
            }
            set
            {
                this._to = value;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        public string From
        {
            get
            {
                return this._from;
            }
            set
            {
                this._from = value;
            }
        }

        public string SmtpServer
        {
            get
            {
                return this._smtpServer;
            }
            set
            {
                this._smtpServer = value;
            }
        }
        #endregion


        #region SMS Properties

        private string _SMSAccountSid;
        private string _SMSNumberFrom;
        private string _SMSAuthenticationToken;
        private int _SMSProvider;
        private int _SMSMonthlyLimit;
        private bool _SmsEnabled;
        private bool _IsResellerSmsEnabled;
        private int _ifkSmsProvider;

        public string SMSAccountSid
        {
            get
            {
                return this._SMSAccountSid;
            }
            set
            {
                this._SMSAccountSid = value;
            }
        }

        public string SMSNumberFrom
        {
            get
            {
                return this._SMSNumberFrom;
            }
            set
            {
                this._SMSNumberFrom = value;
            }
        }

        public string SMSAuthenticationToken
        {
            get
            {
                return this._SMSAuthenticationToken;
            }
            set
            {
                this._SMSAuthenticationToken = value;
            }
        }

        public int SMSMonthlyLimit
        {
            get
            {
                return this._SMSMonthlyLimit;
            }
            set
            {
                this._SMSMonthlyLimit = value;
            }
        }

        public bool SmsEnabled
        {
            get
            {
                return this._SmsEnabled;
            }
            set
            {
                this._SmsEnabled = value;
            }
        }

        public bool IsResellerSmsEnabled
        {
            get
            {
                return this._IsResellerSmsEnabled;
            }
            set
            {
                this._IsResellerSmsEnabled = value;
            }
        }

        public int SMSProvider
        {
            get
            {
                return this._SMSProvider;
            }
            set
            {
                this._SMSProvider = value;
            }
        }

        public int ifkSmsProvider
        {
            get
            {
                return this._ifkSmsProvider;
            }
            set
            {
                this._ifkSmsProvider = value;
            }
        }

        #endregion


        #region LayersMapping Properties

        private string _vMapDisplayName;

        public string vMapDisplayName
        {
            get { return _vMapDisplayName; }
            set { _vMapDisplayName = value; }
        }

        private int _ipkMapId;

        public int ipkMapId
        {
            get { return _ipkMapId; }
            set { _ipkMapId = value; }
        }


        private List<clsCompany> _ListofAllMapLayersData;

        public List<clsCompany> ListofAllMapLayersData
        {
            get { return _ListofAllMapLayersData; }
            set { _ListofAllMapLayersData = value; }
        }

        private bool _bDefaultMap;

        public bool bDefaultMap
        {
            get { return _bDefaultMap; }
            set { _bDefaultMap = value; }
        }

        private bool _bUseResellerSettings;

        public bool bUseResellerSettings
        {
            get { return _bUseResellerSettings; }
            set { _bUseResellerSettings = value; }
        }

        private bool _bMapsEnabled;

        public bool bMapsEnabled
        {
            get { return _bMapsEnabled; }
            set { _bMapsEnabled = value; }
        }

        private int _iMapsID;

        public int iMapsID { get { return _iMapsID; } set { _iMapsID = value; } }

        private string _vMapsApiKey;

        public string vMapsApiKey
        { get { return _vMapsApiKey; } set { _vMapsApiKey = value; } }

        private List<clsCompany> _lstMapLayers;
        public List<clsCompany> lstMapLayers
        {
            get { return _lstMapLayers; }
            set { _lstMapLayers = value; }
        }

        #endregion

        public clsCompany()
        {
            // constructor
            lstclsCompany = new List<clsCompany>();          

            _wlt_AppConfig = AppConfiguration.GetAppSettings<wlt_Config>("wlt_config");

            Connectionstring = AppConfiguration.GetAppSettings<wlt_Config>("ConnectionStrings").wlt_WebAppConnectionString;

        }


        public clsCompany(List<clsCompany> _list)
        {
            lstclsCompany = _list;

        }

        public clsCompany(string _ReportName, int _reportId, bool _isSet)
        {
            vReportName = _ReportName;
            ifkReportTypeId = _reportId;
            isSet = _isSet;


        }


        public clsCompany(int ipkMapId, string FaviconName)
        {
            this.ipkMapId = ipkMapId;
            this.vMapDisplayName = FaviconName;
        }


        //constructor for the favicon 
        public clsCompany(byte[] FaviconByteArray, string vMapDisplayName, int ipkCompanyID)
        {
            this.ipkMapId = ipkMapId;
            this.FaviconByteArray = FaviconByteArray;
            this.ipkCompanyID = ipkCompanyID;
        }

        public clsCompany(int iMapsID, string vMapsApiKey, bool bMapsEnabled, bool bDefaultMap, string vMapDisplayName, bool bUseResellerSettings)
        {
            this.iMapsID = iMapsID;
            this.vMapsApiKey = vMapsApiKey;
            this.bMapsEnabled = bMapsEnabled;
            this.bDefaultMap = bDefaultMap;
            this.vMapDisplayName = vMapDisplayName;
            this.bUseResellerSettings = bUseResellerSettings;

        }

        public clsCompany(int ipkCompanyID, string vCompanyName, string vCompanyLicencedURL, string vImage, bool bStatus)
        {
            this.ipkCompanyID = ipkCompanyID;
            this.vCompanyName = vCompanyName;
            this.vCompanyLicencedURL = vCompanyLicencedURL;
            this.vImage = vImage;
            this.bStatus = bStatus;
        }
        public clsCompany(string vCompanyName, int ipkCompanyID)
        {
            this.vCompanyName = vCompanyName;
            this.ipkCompanyID = ipkCompanyID;
        }
        public clsCompany(int Error)
        {
            this.Error = Error;
        }
        public clsCompany(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }

        public clsCompany(int ipkCompanyID, string vCompanyName, string vCompanyLicencedURL, string vHelpUrl, int ifkThemeID, string vImage, string vFavicon, string vAddress, string vCity, string vRegion, int ifkCountryID, int iParentID, int iCreatedBy, bool bStatus, string vpkCountry, string vThemeName, string SMTPServer, int Port, string From, string Password, bool UseDefaultSMTP, bool EnableSSL, List<clsCompany> lstMapLayers, string SMSAccountSid, string SMSAuthenticationToken, string SMSNumberFrom, int SMSMonthlyLimit, bool SmsEnabled, bool IsResellerSmsEnabled, int ifkSmsProvider, bool EmailEnabled, string CustomLink1, string CustomLink2, string CustomLink3, string CustomLink1Text, string CustomLink2Text, string CustomLink3Text, string CustomLink1ToolTip, string CustomLink2ToolTip, string CustomLink3ToolTip, int vLanguage, string vLanguageName, string vPageheading, string vBackgroundImage)
        {
            this.ipkCompanyID = ipkCompanyID;
            this.vCompanyName = vCompanyName;
            this.vCompanyLicencedURL = vCompanyLicencedURL;
            this.vHelpUrl = vHelpUrl;
            this.ifkThemeID = ifkThemeID;
            this.vImage = vImage;
            this.vFavicon = vFavicon;
            this.vAddress = vAddress;
            this.vCity = vCity;
            this.vRegion = vRegion;
            this.ifkCountryID = ifkCountryID;
            this.iParentID = iParentID;
            this.iCreatedBy = iCreatedBy;
            this.bStatus = bStatus;
            this.vpkCountry = vpkCountry;
            this.vThemeName = vThemeName;
            this.SmtpServer = SMTPServer;
            this.Port = Port;
            this.From = From;
            this.Password = Password;
            this.EnableSsl = EnableSSL;
            this.UseDefaultSmtp = UseDefaultSMTP;
            this.lstMapLayers = lstMapLayers;
            this.SMSAccountSid = SMSAccountSid;
            this.SMSAuthenticationToken = SMSAuthenticationToken;
            this.SMSNumberFrom = SMSNumberFrom;
            this.SMSMonthlyLimit = SMSMonthlyLimit;
            this.SmsEnabled = SmsEnabled;
            this.IsResellerSmsEnabled = IsResellerSmsEnabled;
            this.ifkSmsProvider = ifkSmsProvider;
            this.EmailEnabled = EmailEnabled;
            this.CustomLink1 = CustomLink1;
            this.CustomLink2 = CustomLink2;
            this.CustomLink3 = CustomLink3;
            this.CustomLink1Text = CustomLink1Text;
            this.CustomLink2Text = CustomLink2Text;
            this.CustomLink3Text = CustomLink3Text;
            this.CustomLink1ToolTip = CustomLink1ToolTip;
            this.CustomLink2ToolTip = CustomLink2ToolTip;
            this.CustomLink3ToolTip = CustomLink3ToolTip;
            this.vLanguage = vLanguage;
            this.vLanguageName = vLanguageName;
            this.vPageheading = vPageheading;
            this.vBackgroundImage = vBackgroundImage;
        }

        public string SaveCompany()
        {
            SqlParameter[] param = new SqlParameter[18];
            string returnstring = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@iParentID", SqlDbType.Int);
                param[2].Value = iParentID;

                param[3] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[3].Value = iCreatedBy;

                param[4] = new SqlParameter("@vCompanyName", SqlDbType.VarChar);
                param[4].Value = vCompanyName;

                if (vCompanyLicencedURL == "")
                {
                    param[5] = new SqlParameter("@vCompanyLicencedURL", SqlDbType.VarChar);
                    param[5].Value = System.DBNull.Value;
                }
                else
                {
                    param[5] = new SqlParameter("@vCompanyLicencedURL", SqlDbType.VarChar);
                    param[5].Value = vCompanyLicencedURL;
                }

                param[6] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[6].Value = bStatus;

                param[7] = new SqlParameter("@Error", SqlDbType.Int);
                param[7].Direction = ParameterDirection.Output;

                param[8] = new SqlParameter("@LastId", SqlDbType.Int);
                param[8].Direction = ParameterDirection.Output;

                param[9] = new SqlParameter("@vHelpUrl", SqlDbType.VarChar);
                param[9].Value = vHelpUrl;

                param[10] = new SqlParameter("@vLanguage", SqlDbType.Int);
                param[10].Value = vLanguage;

                param[11] = new SqlParameter("@IsClient", SqlDbType.Bit);
                param[11].Value = IsClient;

                param[12] = new SqlParameter("@IsMiniReseller", SqlDbType.Bit);
                param[12].Value = IsMiniReseller;

                param[13] = new SqlParameter("@bUseHttps", SqlDbType.Bit);
                param[13].Value = bUseHttps;

                param[14] = new SqlParameter("@KeepDataFor", SqlDbType.Int);
                param[14].Value = KeepDataFor;

                param[15] = new SqlParameter("@bIsDenyData", SqlDbType.Bit);
                param[15].Value = IsDenyData;

                param[16] = new SqlParameter("@IsStreetMapEnabled", SqlDbType.Bit);
                param[16].Value = IsStreetMapEnabled;


                param[17] = new SqlParameter("@client_role_ids", SqlDbType.VarChar);
                param[17].Value = JsonConvert.SerializeObject(client_role_ids);

                

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                if (param[7].Value.ToString() == "-2")
                {
                    returnstring = "-2";
                }
                else if (param[7].Value.ToString() == "-1")
                {
                    returnstring = "-1";
                }
                else if (param[7].Value.ToString() != "-1" && param[7].Value.ToString() != "-2")
                {
                    returnstring = param[7].Value.ToString();
                }
                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveCompany()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error!";

            }
            return returnstring;

        }

        public async Task<DataSet> ShowResellerDashBoard()
        {
            return await Task.Run(() =>
            {

                SqlParameter[] param = new SqlParameter[7];
                DataSet ds = new DataSet();

                try
                {
                    param[0] = new SqlParameter("@ResellerID", SqlDbType.Int);
                    param[0].Value = iParentID;

                    param[1] = new SqlParameter("@ShowFor", SqlDbType.Int);
                    param[1].Value = ShowFor;

                    param[2] = new SqlParameter("@StartDate", SqlDbType.DateTime);
                    param[2].Value = StartDate;

                    param[3] = new SqlParameter("@EndDate", SqlDbType.DateTime);
                    param[3].Value = EndDate;

                    param[4] = new SqlParameter("@IsDateSelected", SqlDbType.Bit);
                    param[4].Value = IsDateSelected;

                    param[5] = new SqlParameter("@SubOperation", SqlDbType.Int);
                    param[5].Value = SubOperation;

                    param[6] = new SqlParameter("@IncludeMiniResellerCount", SqlDbType.Bit);
                    param[6].Value = IncludeMiniResellerCount;

                    if (Operation == 1)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboard_SubOps", param);
                    }
                    else if (Operation == 2)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashBoard_NewDevices", param);
                    }
                    else if (Operation == 3)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardSmses", param);
                    }
                    else if (Operation == 4)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardNewUsers", param);
                    }
                    else if (Operation == 5)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardNewClient", param);
                    }
                    else if (Operation == 6)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardDeviceOnline", param);
                    }
                    else if (Operation == 7)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardDeviceOffline", param);
                    }
                    else if (Operation == 8)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardTotalDevices", param);
                    }
                    else if (Operation == 9)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardClients", param);
                    }
                    else if (Operation == 10)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardDevices", param);
                    }
                    else if (Operation == 11)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardDevicesOnline", param);
                    }
                    else if (Operation == 12)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardDevicesOffline", param);
                    }
                    else if (Operation == 13)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardInactiveDevice", param);
                    }
                    else if (Operation == 14)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardDevicesNeverReported", param);
                    }
                    else if (Operation == 15)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardDevicesRptThisMonth", param);
                    }
                    else if (Operation == 16)
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminDashboardDevicesLastMonth", param);
                    }

                }
                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile("clsCompany.cs", "ShowResellerDashBoard()", ex.Message  + ex.StackTrace);

                }
                return ds;
            });
        }

        public DataSet ShowClientDashBoard()
        {
            SqlParameter[] param = new SqlParameter[6];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ClientID", SqlDbType.Int);
                param[1].Value = iParentID;

                param[2] = new SqlParameter("@ShowFor", SqlDbType.Int);
                param[2].Value = ShowFor;

                param[3] = new SqlParameter("@StartDate", SqlDbType.DateTime);
                param[3].Value = StartDate;

                param[4] = new SqlParameter("@EndDate", SqlDbType.DateTime);
                param[4].Value = EndDate;

                param[5] = new SqlParameter("@IsDateSelected", SqlDbType.Bit);
                param[5].Value = IsDateSelected;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminClientDashboard", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "ShowClientDashBoard()", ex.Message  + ex.StackTrace);

            }
            return ds;
        }

        public string SaveCustomLinks()
        {
            SqlParameter[] param = new SqlParameter[14];
            string returnstring = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@iParentID", SqlDbType.Int);
                param[2].Value = iParentID;

                param[3] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[3].Value = iCreatedBy;

                param[4] = new SqlParameter("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@CustomLink1", SqlDbType.VarChar);
                param[5].Value = CustomLink1;

                param[6] = new SqlParameter("@CustomLink1DisplayName", SqlDbType.VarChar);
                param[6].Value = CustomLink1Text;

                param[7] = new SqlParameter("@CustomLink2", SqlDbType.VarChar);
                param[7].Value = CustomLink2;

                param[8] = new SqlParameter("@CustomLink2DisplayName", SqlDbType.VarChar);
                param[8].Value = CustomLink2Text;

                param[9] = new SqlParameter("@CustomLink3", SqlDbType.VarChar);
                param[9].Value = CustomLink3;

                param[10] = new SqlParameter("@CustomLink3DisplayName", SqlDbType.VarChar);
                param[10].Value = CustomLink3Text;

                param[11] = new SqlParameter("@CustomLink1ToolTip", SqlDbType.VarChar);
                param[11].Value = CustomLink1ToolTip;

                param[12] = new SqlParameter("@CustomLink2ToolTip", SqlDbType.VarChar);
                param[12].Value = CustomLink2ToolTip;

                param[13] = new SqlParameter("@CustomLink3ToolTip", SqlDbType.VarChar);
                param[13].Value = CustomLink3ToolTip;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);


                if (param[4].Value.ToString() == "-2")
                {
                    returnstring = "-2";
                }

                else if (param[4].Value.ToString() != "-2")
                {
                    returnstring = param[4].Value.ToString();
                }
                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveCustomLinks()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error!";

            }
            return returnstring;

        }


        public string SaveVisualSetting()
        {
            SqlParameter[] param = new SqlParameter[11];
            string returnstring = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@vCompanyName", SqlDbType.VarChar);
                param[2].Value = vCompanyName;

                param[3] = new SqlParameter("@vImage", SqlDbType.VarChar);
                param[3].Value = vImage;

                param[4] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[4].Value = bStatus;

                param[5] = new SqlParameter("@Error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[7] = new SqlParameter("@dUpdateDate", SqlDbType.VarChar);
                param[7].Value = dUpdateDate;

                param[9] = new SqlParameter("@vCompanyLicencedURL", SqlDbType.VarChar);
                param[9].Value = vCompanyLicencedURL;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveCompany()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error!";

            }
            return returnstring;

        }

        public string SaveCompanyAddress()
        {
            SqlParameter[] param = new SqlParameter[15];
            string returnstring = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@dUpdateDate", SqlDbType.DateTime);
                param[3].Value = dUpdateDate;

                param[4] = new SqlParameter("@vAddress", SqlDbType.VarChar);
                param[4].Value = vAddress;

                param[5] = new SqlParameter("@vCity", SqlDbType.VarChar);
                param[5].Value = vCity;

                param[6] = new SqlParameter("@vRegion", SqlDbType.VarChar);
                param[6].Value = vRegion;

                param[7] = new SqlParameter("@ifkCountryID", SqlDbType.Int);
                param[7].Value = ifkCountryID;

                param[8] = new SqlParameter("@SMTPServer", SqlDbType.NVarChar);
                param[8].Value = SmtpServer;

                param[9] = new SqlParameter("@SMTPPort", SqlDbType.Int);
                param[9].Value = Port;

                param[10] = new SqlParameter("@AuthEmail", SqlDbType.NVarChar);
                param[10].Value = From;

                param[11] = new SqlParameter("@AuthPass", SqlDbType.NVarChar);
                param[11].Value = Password;

                param[12] = new SqlParameter("@EnableSSl", SqlDbType.Bit);
                param[12].Value = EnableSsl;

                param[13] = new SqlParameter("@EnableEmail", SqlDbType.Bit);
                param[13].Value = EmailEnabled;

                param[14] = new SqlParameter("@IsClient", SqlDbType.Bit);
                param[14].Value = IsClient;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                if (param[2].Value.ToString() == "1")
                {
                    returnstring = "Saved successfully";
                }
                else if (param[2].Value.ToString() == "2")
                {
                    returnstring = "Updated successfully";
                }
                else if (param[2].Value.ToString() == "-1")
                {
                    returnstring = "Client name already exists";
                }
                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveCompanyAddress()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error!";

            }

            return returnstring;

        }



        public string SaveResellerPageheading()
        {
            SqlParameter[] param = new SqlParameter[4];
            string returnstring = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@Pageheading", SqlDbType.VarChar);
                param[3].Value = vPageheading;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                if (param[2].Value.ToString() == "2")
                {
                    returnstring = "Updated successfully!";
                }
                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveResellerPageheading()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error!";

            }

            return returnstring;

        }
        public string SaveClientSMS()
        {
            SqlParameter[] param = new SqlParameter[7];
            string returnstring = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                //param[3] = new SqlParameter("@iParentID", SqlDbType.Int);
                //param[3].Value = iParentID;

                param[3] = new SqlParameter("@SmsEnabled", SqlDbType.Bit);
                param[3].Value = SmsEnabled;

                param[4] = new SqlParameter("@SMSMonthlyLimit", SqlDbType.VarChar);
                param[4].Value = SMSMonthlyLimit;


                param[5] = new SqlParameter("@SmsValues", SqlDbType.VarChar);
                param[5].Value = SmsValues;


                param[6] = new SqlParameter("@IsTwoWaySMS", SqlDbType.VarChar);
                param[6].Value = IsTwoWaySMS;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Sp_SaveClientSMS", param);

                if (param[2].Value.ToString() == "2")
                {
                    returnstring = "Updated successfully!";
                }
                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveClientSMS()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error!";

            }

            return returnstring;

        }

        public string SaveResellerSMS()
        {
            SqlParameter[] param = new SqlParameter[7];
            string returnstring = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                //param[3] = new SqlParameter("@SMSAccountSid", SqlDbType.VarChar);
                //param[3].Value = SMSAccountSid;

                //param[4] = new SqlParameter("@SMSNumberFrom", SqlDbType.VarChar);
                //param[4].Value = SMSNumberFrom;

                //param[5] = new SqlParameter("@SMSAuthenticationToken", SqlDbType.VarChar);
                //param[5].Value = SMSAuthenticationToken;

                param[3] = new SqlParameter("@SmsValues", SqlDbType.NVarChar);
                param[3].Value = SmsValues;


                param[4] = new SqlParameter("@IsResellerSmsEnabled", SqlDbType.Bit);
                param[4].Value = IsResellerSmsEnabled;

                param[5] = new SqlParameter("@ifkSMSProvider", SqlDbType.Int);
                param[5].Value = SMSProvider;

                param[6] = new SqlParameter("@SmsFailureNotifyEmail", SqlDbType.VarChar);
                param[6].Value = SmsFailureNotifyEmail;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Sp_SaveResellerSMS", param);

                if (param[2].Value.ToString() == "2")
                {
                    returnstring = "Updated successfully!";
                }
                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveResellerSMS()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error!";

            }

            return returnstring;

        }

        public DataSet GetSmsProvider()
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkSmsProviderID", SqlDbType.Int);
                param[1].Value = ifkSmsProvider;

                param[2] = new SqlParameter("@ResellerID", SqlDbType.Int);
                param[2].Value = ipkCompanyID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "SMSGatewayAPI", param);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetSmsProvider()", ex.Message  + ex.StackTrace);

            }

            return ds;

        }


        public string SaveResellerMapping()
        {
            SqlParameter[] param = new SqlParameter[9];
            string returnstring = "";
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@ResellerID", SqlDbType.Int);
                param[3].Value = iParentID;

                param[4] = new SqlParameter("@iMapsID", SqlDbType.Int);
                param[4].Value = iMapsID;

                param[5] = new SqlParameter("@vMapsApiKey", SqlDbType.NVarChar);
                param[5].Value = vMapsApiKey;

                param[6] = new SqlParameter("@bDefaultMap", SqlDbType.Bit);
                param[6].Value = bDefaultMap;

                param[7] = new SqlParameter("@bMapsEnabled", SqlDbType.Bit);
                param[7].Value = bMapsEnabled;

                param[8] = new SqlParameter("@bUseResellerSettings", SqlDbType.Bit);
                param[8].Value = bUseResellerSettings;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Sp_SaveResellerMappingData", param);

                if (param[2].Value.ToString() == "2")
                {
                    returnstring = "Updated successfully";
                }
                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveResellerMapping()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error!";

            }

            return returnstring;

        }

        public DataSet GetCompany()
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@vCompanyLicencedURL", SqlDbType.NVarChar);
                param[2].Value = vCompanyLicencedURL;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetCompany()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public List<clsCompany> GetAllCompanyName()
        {
            DataSet ds = new DataSet();
            List<clsCompany> obj = new List<clsCompany>();
            try
            {
                if (IsEdit)
                {
                    ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.Text, "select * from tblCompany_Master");
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.Text, "select ipkCompanyID,vCompanyName FROM  dbo.tblCompany_Master WHERE ipkCompanyID NOT IN(select ifkCompanyID FROM  dbo.tblAdmin_login)");
                }
                obj.Add(new clsCompany("Select", -1));
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    obj.Add(new clsCompany(row["vCompanyName"].ToString(), Convert.ToInt32(row["ipkCompanyID"].ToString())));
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetAllCompanyName()", ex.Message  + ex.StackTrace);
            }
            return obj;
        }

        public List<clsCompany.CompanyDDL> GetAllCompanyAlert()
        {
            DataSet ds = new DataSet();
            List<clsCompany.CompanyDDL> obj = new List<clsCompany.CompanyDDL>();
            try
            {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@iParentID", SqlDbType.Int);
                param[2].Value = iParentID;

                param[3] = new SqlParameter("@IncludeMini_ResellerClients", SqlDbType.Bit);
                param[3].Value = IncludeMini_ResellerClients;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                //obj.Add(new clsCompany.CompanyDDL("Select Client", -1));

                if (Operation == 18 || Operation == 12)
                {
                    //obj.Add(new clsCompany.CompanyDDL("All Clients", 0));

                }
                if ((ds.Tables.Count > 0) && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj.Add(new clsCompany.CompanyDDL(row["vCompanyName"].ToString(), Convert.ToInt32(row["ipkCompanyID"].ToString())));
                    }
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetAllCompanyAlert()", ex.Message  + ex.StackTrace);
            }
            return obj;

        }


        public List<clsCompany> GetReportTypeList()
        {
            DataSet ds = new DataSet();
            List<clsCompany> obj = new List<clsCompany>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_Reports", param);

                obj.Add(new clsCompany("Select Type", -1));

                if ((ds.Tables.Count > 0) && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj.Add(new clsCompany(row["vReportTypeName"].ToString(), Convert.ToInt32(row["ipkReportTypeId"].ToString())));
                    }
                }



            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetReportTypeList()", ex.Message  + ex.StackTrace);
            }
            return obj;

        }

        public List<ClsReport> GetReportTypeList(int second = 0)
        {
            DataSet ds = new DataSet();
            List<ClsReport> obj = new List<ClsReport>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_Reports", param);

                obj.Add(new ClsReport { report_name = "Select Type", report_type_id = -1 });

                if ((ds.Tables.Count > 0) && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj.Add(new ClsReport { iReportDisplayType = Convert.ToInt32(row["reportDisplayType"]), report_name = row["vReportTypeName"].ToString(), report_type_id = Convert.ToInt32(row["ipkReportTypeId"].ToString()), IsScheduled = Convert.ToBoolean(row["IsScheduled"]) });
                    }
                }



            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetReportTypeList()", ex.Message  + ex.StackTrace);
            }
            return obj;

        }


        public List<clsCompany> GetAlertReportDdlList()
        {
            DataSet ds = new DataSet();
            List<clsCompany> obj = new List<clsCompany>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_Reports", param);

                obj.Add(new clsCompany("Select Alert", -1));

                if ((ds.Tables.Count > 0) && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj.Add(new clsCompany(row["vAlertName"].ToString(), Convert.ToInt32(row["ipkAlertID"].ToString())));
                    }
                }



            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetReportTypeList()", ex.Message  + ex.StackTrace);
            }
            return obj;

        }

        public List<clsCompany> GetZoneReportDdlList()
        {
            DataSet ds = new DataSet();
            List<clsCompany> obj = new List<clsCompany>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@CompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_Reports", param);

                obj.Add(new clsCompany("Select Zone", -1));

                if ((ds.Tables.Count > 0) && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        obj.Add(new clsCompany(row["vGeoName"].ToString(), Convert.ToInt32(row["ipkGeoMID"].ToString())));
                    }
                }



            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetReportTypeList()", ex.Message  + ex.StackTrace);
            }
            return obj;

        }

        public List<El_Mappings> GetMapDataInfo()
        {
            DataSet ds = new DataSet();
            List<El_Mappings> lstmapdata = new List<El_Mappings>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Sp_SaveResellerMappingData", param);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstmapdata.Add(new El_Mappings(Convert.ToInt32(row["ifkMappingProvidersId"]), Convert.ToString(row["vApiKey"]), Convert.ToBoolean(row["bShowMapInApp"]), Convert.ToBoolean(row["bDefaultMap"]), Convert.ToString(row["vMapDisplayName"]), Convert.ToBoolean(row["bUseResellerSettings"])));
                    }
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetMapDataInfo()", ex.Message  + ex.StackTrace);
            }

            return lstmapdata;
        }

        public DataRow GetResellerTrialStatus(int reseller_id)
        {
            var sql = $"select top 1 vCompanyName ReselleName, IsTrial, dFullConversionDate,users.vName,users.pkUserID,users.vEmail,users.vTimeZoneID from wlt_tblReseller  resellers left join newtblUserMaster users on users.iParent = resellers.ResellerId and  " +
                                $"ifkUserTypeID = 2 where ResellerId = {reseller_id}  order by users.is_primary_owner desc  ";

            var ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.Text, sql);

            foreach (DataTable dt in ds.Tables)
                foreach (DataRow dr in dt.Rows)
                    return dr;


            return null ;
        }
        public string SaveExpiryCompanyTrial_comand(clsRegistration objclsRegistration)
        {
            DataSet ds = new DataSet();
            string return_string = "";
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@IsTrial", SqlDbType.Int);
                param[0].Value = IstTrial;

                param[1] = new SqlParameter("@Reseller_id", SqlDbType.Int);
                param[1].Value = ipkCompanyID;


                param[2] = new SqlParameter("@DateToDisableAccess", SqlDbType.NVarChar);
                param[2].Value = UserSettings.ConvertLocalDateTimeToUTCDateTime(Convert.ToDateTime(DateToDisableAccess), objclsRegistration.vTimeZoneID);





                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_UpdateResellerTrial", param);
                return_string = "1";
            }
            catch (Exception ex)
            {
                return_string = "0";
                LogError.RegisterErrorInLogFile("clsCompany.cs", "selectNewCompany()", ex.Message  + ex.StackTrace);
            }

            return return_string;
        }
        public async Task<List<clsCompany>> selectNewCompany(clsRegistration objSession)
        {
            return await Task.Run(() =>
            {

                DataSet ds = new DataSet();
                List<clsCompany> lstcompany = new List<clsCompany>();
                List<clsCompany> lstmapdata = new List<clsCompany>();
                try
                {
                    SqlParameter[] param = new SqlParameter[2];

                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;

                    param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                    param[1].Value = ipkCompanyID;


                    ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);


                    if (ds.Tables.Count > 0)
                    {

                        string uploadedFavicon = "";

                        if (Convert.ToString(ds.Tables[0].Rows[0]["FaviconName"]) != "")
                        {
                            uploadedFavicon = Convert.ToString(ds.Tables[0].Rows[0]["FaviconName"]);
                        }

                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            lstmapdata.Add(new clsCompany(Convert.ToInt32(row["ifkMappingProvidersId"]), Convert.ToString(row["vApiKey"]), Convert.ToBoolean(row["bShowMapInApp"]), Convert.ToBoolean(row["bDefaultMap"]), Convert.ToString(row["vMapDisplayName"]), Convert.ToBoolean(row["bUseResellerSettings"])));
                        }


                        foreach (DataRow row in ds.Tables[0].Rows)
                        {

                            bool? vUseDefaultSMTP = row["UseDefaultSMTP"] as bool?;

                            if (vUseDefaultSMTP != null)
                            {
                                vUseDefaultSMTP = Convert.ToBoolean(row["UseDefaultSMTP"]);
                            }
                            else
                            {
                                vUseDefaultSMTP = false;
                            }

                            bool? vEnableSSL = row["EnableSSL"] as bool?;

                            if (vEnableSSL != null)
                            {
                                vEnableSSL = Convert.ToBoolean(row["EnableSSL"]);
                            }
                            else
                            {
                                vEnableSSL = false;
                            }


                            int IsTrial = row["IsTrial"].ToString() == "" ? 0 : Convert.ToInt32(row["IsTrial"]);
                            DateTime DateToDisableAccess;
                            DateToDisableAccess = row["DateToDisableAccess"].ToString() == "" ? DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified) : Convert.ToDateTime(row["DateToDisableAccess"]);


                            int _intSMSMonthlyLimit = 0;
                            if (row["SMSMonthlyLimit"] != null && Convert.ToString(row["SMSMonthlyLimit"]) != "")
                            {
                                _intSMSMonthlyLimit = Convert.ToInt32(row["SMSMonthlyLimit"]);
                            }

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (dr["ifkThemeID"].ToString() == "")
                                {
                                    dr["ifkThemeID"] = 0;
                                }

                                if (dr["ifkCountryID"].ToString() == "")
                                {
                                    dr["ifkCountryID"] = 0;
                                }

                                if (dr["iParentID"].ToString() == "")
                                {
                                    dr["iParentID"] = 0;
                                }

                                if (dr["iCreatedBy"].ToString() == "")
                                {
                                    dr["iCreatedBy"] = 0;
                                }

                                if (dr["bStatus"].ToString() == "")
                                {
                                    dr["bStatus"] = false;
                                }

                                if (dr["SMTPPort"].ToString() == "")
                                {
                                    dr["SMTPPort"] = 0;
                                }

                                if (dr["ifkCountryID"].ToString() == "")
                                {
                                    dr["ifkCountryID"] = 0;
                                }

                                if (dr["SmsEnabled"].ToString() == "")
                                {
                                    dr["SmsEnabled"] = false;
                                }

                                if (dr["EmailEnabled"].ToString() == "")
                                {
                                    dr["EmailEnabled"] = false;
                                }


                            }

                            clsRegistration objclsRegistration = objSession;
                            var disableAccessDate = UserSettings.ConvertUTCDateTimeToShortDate(Convert.ToDateTime(DateToDisableAccess), objclsRegistration.vTimeZoneID);


                            lstcompany.Add(new clsCompany
                            {

                                ipkCompanyID = Convert.ToInt32(row["ipkCompanyID"].ToString()),
                                vCompanyName = row["vCompanyName"].ToString(),
                                vCompanyLicencedURL = row["vWebSite"].ToString(),
                                vHelpUrl = row["vHelpUrl"].ToString(),
                                ifkThemeID = Convert.ToInt32(row["ifkThemeID"].ToString()),
                                vImage = Convert.ToString(row["LogoName"]), //uploadedLogo,
                                vFavicon = uploadedFavicon,
                                vAddress = row["vAddress"].ToString(),
                                vCity = row["vCity"].ToString(),
                                vRegion = row["vRegion"].ToString(),
                                ifkCountryID = Convert.ToInt32(row["ifkCountryID"].ToString()),
                                iParentID = Convert.ToInt32(row["iParentID"].ToString()),
                                iCreatedBy = Convert.ToInt32(row["iCreatedBy"].ToString()),
                                bStatus = Convert.ToBoolean(row["bStatus"].ToString()),
                                vpkCountry = row["vcountry_name"].ToString(),
                                vThemeName = row["vThemeName"].ToString(),
                                SmtpServer = row["SMTPServer"].ToString(),
                                Port = row["SMTPPort"].ToString() == "" ? 0 : Convert.ToInt32(row["SMTPPort"].ToString()),
                                From = row["AuthEmail"].ToString(),
                                Password = row["AuthPass"].ToString(),
                                UseDefaultSmtp = Convert.ToBoolean(vUseDefaultSMTP),
                                EnableSsl = Convert.ToBoolean(vEnableSSL),
                                lstMapLayers = lstmapdata,
                                SMSAccountSid = row["SMSAccountSid"].ToString(),
                                SMSAuthenticationToken = row["SMSAuthenticationToken"].ToString(),
                                SMSNumberFrom = row["SMSNumberFrom"].ToString(),
                                SMSMonthlyLimit = Convert.ToInt32(_intSMSMonthlyLimit),
                                SmsEnabled = Convert.ToBoolean(row["SmsEnabled"].ToString()),
                                IsResellerSmsEnabled = Convert.ToBoolean(row["IsResellerSmsEnabled"].ToString()),
                                ifkSmsProvider = String.IsNullOrEmpty(Convert.ToString(row["ifkSmsProvider"].ToString())) ? 1 : Convert.ToInt32(row["ifkSmsProvider"].ToString()),
                                EmailEnabled = Convert.ToBoolean(row["EmailEnabled"].ToString()),
                                CustomLink1 = Convert.ToString(row["CustomLink1"]),
                                CustomLink2 = Convert.ToString(row["CustomLink2"]),
                                CustomLink3 = Convert.ToString(row["CustomLink3"]),
                                CustomLink1Text = Convert.ToString(row["CustomLink1DisplayName"]),
                                CustomLink2Text = Convert.ToString(row["CustomLink2DisplayName"]),
                                CustomLink3Text = Convert.ToString(row["CustomLink3DisplayName"]),
                                CustomLink1ToolTip = Convert.ToString(row["CustomLink1ToolTip"]),
                                CustomLink2ToolTip = Convert.ToString(row["CustomLink2ToolTip"]),
                                CustomLink3ToolTip = Convert.ToString(row["CustomLink3ToolTip"]),                               
                                vLanguage = String.IsNullOrEmpty(Convert.ToString(row["ifkLanguageID"].ToString())) ? 94 : Convert.ToInt32(row["ifkLanguageID"].ToString()),
                                vLanguageName = Convert.ToString(row["vLanguageName"]),
                                vPageheading = Convert.ToString(row["vPageheading"]),
                                vBackgroundImage = Convert.ToString(row["vBackgroundImageName"]), //uploadedBackgroundImage,
                                IstTrial = IsTrial,
                                DateToDisableAccess = disableAccessDate,
                                bUseHttps = Convert.ToBoolean(row["bUseHttps"]),
                                SmsFailureNotifyEmail = Convert.ToString(row["SMSFailureNotifyEmail"]),
                                SmsValues = Convert.ToString(row["SmsValues"]),
                                SmsFields = Convert.ToString(row["SmsFields"]),
                                KeepDataFor = Convert.ToInt32(row["KeepDataFor"]),
                                IsDenyData = Convert.ToBoolean(row["IsDenyData"]),
                                ifkGeocoderId = Convert.ToInt32(row["ifkGeocoderId"]),
                                GeocoderUserKey = Convert.ToString(row["GeocoderUserKey"]),
                                bUseResellerGeocoder = Convert.ToBoolean(row["bUseResellerGeocoder"]),
                                IsStreetMapEnabled = Convert.ToBoolean(row["IsStreetMapEnabled"]),
                                IsMiniReseller = Convert.ToBoolean(row["IsMiniReseller"]),
                                CustomUrl1Photo = Convert.ToString(row["custom_url1_photo"]),
                                CustomUrl2Photo = Convert.ToString(row["custom_url2_photo"]),
                                CustomUrl3Photo = Convert.ToString(row["custom_url3_photo"]),
                                IsTwoWaySMS = Convert.ToBoolean(row["is_two_way_sms"])  
                            });
                        }


                        if (Operation == 24)
                        {
                            var _newrptList = new List<clsCompany>();

                            foreach (DataRow row in ds.Tables[2].Rows)
                            {

                                _newrptList.Add(new clsCompany(
                                  Convert.ToString(row["vReportTypeName"]),
                                  Convert.ToInt32(row["ipkReportTypeId"]),
                                  Convert.ToBoolean(row["isSet"])

                                )
                               );

                            }


                            lstcompany.Add(new clsCompany(_newrptList));

                        }

                        if (Operation == 10)
                        {

                            var Parent_ResellerDashboardStatusSettings = ds.Tables[2];

                            var Mini_ResellerDashboardStatusSettings = ds.Tables[4];

                            var WebHooksDetails = ds.Tables[5];

                            lstcompany.Add(new clsCompany
                            {
                                CustomJsonString = JsonConvert.SerializeObject(Parent_ResellerDashboardStatusSettings),
                                vMiniresellerList = JsonConvert.SerializeObject(Mini_ResellerDashboardStatusSettings),
                                vWebHooks = JsonConvert.SerializeObject(WebHooksDetails)
                            });

                            //reseller dashboard settings section 
                            if (Parent_ResellerDashboardStatusSettings.Rows.Count > 0)
                            {
                                lstcompany.Add(new clsCompany { CustomJsonString = JsonConvert.SerializeObject(ds.Tables[3]) });
                            }



                        }
                    }


                }

                catch (Exception ex)
                {

                    LogError.RegisterErrorInLogFile("clsCompany.cs", "selectNewCompany()", ex.Message  + ex.StackTrace);
                }

                return lstcompany;
            });
        }

        public string SaveClientReportSettings(clsCompany _clsCompany, clsRegistration objclsRegistration)
        {
            DataSet ds = new DataSet();

            this.IsClient = _clsCompany.IsClient;

            string return_string = "";
            try
            {
                _clsCompany.Operation = 42;
                PopulateClients_Reports(_clsCompany, objclsRegistration);
                foreach (var item in _clsCompany.lstclsCompany)
                {
                    item.Operation = 43;
                    PopulateClients_Reports(item, objclsRegistration);
                }


                return_string = "1";
            }
            catch (Exception ex)
            {
                return_string = "0";
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveClientReportSettings()", ex.Message  + ex.StackTrace);
            }

            return return_string;
        }

        public int SaveDashboardActivationSettings()
        {
            int success = 0;

            var ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@vParameterText", SqlDbType.VarChar);
                param[2].Value = vParameterText;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                success = 1;
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveDashboardActivationSettings()", ex.Message  + ex.StackTrace);
                success = 0;
            }

            return success;
        }


        public int SaveMini_ResellerActivationSettings()
        {
            int success = 0;

            var ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@vParameterText", SqlDbType.VarChar);
                param[2].Value = vParameterText;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                success = 1;
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveDashboardActivationSettings()", ex.Message  + ex.StackTrace);
                success = 0;
            }

            return success;
        }


        public int SaveResellerWebHooksSettings(EL_WebHook _EL_WebHook)
        {
            int success = 0;

            var ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[6];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@vParameterText", SqlDbType.VarChar);
                param[2].Value = vParameterText;

                param[3] = new SqlParameter("@vInstallationUrl", SqlDbType.VarChar);
                param[3].Value = _EL_WebHook.vInstallationUrl;

                param[4] = new SqlParameter("@vDe_InstallationUrl", SqlDbType.VarChar);
                param[4].Value = _EL_WebHook.vDe_InstallationUrl;

                param[5] = new SqlParameter("@vDeviceStatusCheckUrl", SqlDbType.VarChar);
                param[5].Value = _EL_WebHook.vDeviceStatusCheckUrl;



                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                success = 1;
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveDashboardActivationSettings()", ex.Message  + ex.StackTrace);
                success = 0;
            }

            return success;
        }

        private void PopulateClients_Reports(clsCompany _clsCompany, clsRegistration objclsRegistration)
        {
            var ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsCompany.Operation;

                param[1] = new SqlParameter("@ifkReportType", SqlDbType.Int);
                param[1].Value = _clsCompany.ifkReportTypeId;


                param[2] = new SqlParameter("@ifkClientCode", SqlDbType.Int);
                param[2].Value = IsClient;



                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "PopulateClients_Reports()", ex.Message  + ex.StackTrace);

            }



        }


        public async Task<DataSet> GetClientList()
        {
            return await Task.Run(() =>
            {

                SqlParameter[] param = new SqlParameter[3];
                DataSet ds = new DataSet();

                try
                {
                    param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                    param[0].Value = Operation;

                    param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                    param[1].Value = ipkCompanyID;

                    param[2] = new SqlParameter("@iParentID", SqlDbType.Int);
                    param[2].Value = iParentID;

                    if (Operation == 13)//Delete Reseller
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_DeleteReseller", param);
                    }
                    else if (Operation == 14)//Delete Client
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_DeleteClient", param);
                    }
                    else
                    {
                        ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);
                    }
                }
                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile("clsCompany.cs", "GetClientList()", ex.Message  + ex.StackTrace);
                }
                return ds;
            });
        }

        public DataSet GetMiniResellerList()
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@iParentID", SqlDbType.Int);
                param[2].Value = iParentID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_MiniReseller", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetMiniResellerList()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetClientListIDForReseller()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Sp_SaveResellerMappingData", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetClientListIDForReseller()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public string SaveUploadedImage()
        {
            SqlParameter[] param = new SqlParameter[5];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[3].Value = iCreatedBy;

                param[4] = new SqlParameter("@LogoName", SqlDbType.VarChar);
                param[4].Value = LogoName;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "CompanyLogo", param);

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
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveUploadedImage()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }


        public string SaveMapping()
        {
            SqlParameter[] param = new SqlParameter[5];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                //    param[3] = new SqlParameter("@bMapEnabled", SqlDbType.VarChar);
                //    param[3].Value = bGoogleMapsEnabled;

                //    param[4] = new SqlParameter("@vMapApiKey", SqlDbType.VarChar);
                //    param[4].Value = vGoogleMapsApiKey;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "CompanyLogo", param);

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
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveMapping()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }

        public string SaveThemes()
        {
            SqlParameter[] param = new SqlParameter[5];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@ifkThemeID", SqlDbType.VarChar);
                param[3].Value = ifkThemeID;

                param[4] = new SqlParameter("@vThemeName", SqlDbType.VarChar);
                param[4].Value = vThemeName;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "CompanyLogo", param);

                if (param[2].Value.ToString() == "1")
                {
                    returnstring = "Saved successfully";
                }

                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveUploadedImage()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }

        public string SaveResellerBranding()
        {
            SqlParameter[] param = new SqlParameter[6];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[0].Value = ipkCompanyID;

                param[1] = new SqlParameter("@Error", SqlDbType.Int);
                param[1].Direction = ParameterDirection.Output;

                param[2] = new SqlParameter("@ifkThemeID", SqlDbType.VarChar);
                param[2].Value = ifkThemeID;

                param[3] = new SqlParameter("@vThemeName", SqlDbType.VarChar);
                param[3].Value = vThemeName;

                param[4] = new SqlParameter("@Pageheading", SqlDbType.VarChar);
                param[4].Value = vPageheading;

                param[5] = new SqlParameter("@vCompanyLicencedURL", SqlDbType.VarChar);
                param[5].Value = vCompanyLicencedURL;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "sp_ResellerBranding", param);

                if (param[1].Value.ToString() == "1")
                {
                    returnstring = "1";
                }
                else if (param[1].Value.ToString() == "-2")
                {
                    returnstring = "-2";
                }

                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveResellerBranding()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }

        public string YourPersonalizedLogo()
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                DataSet ds = new DataSet();

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "CompanyLogo", param);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string filePath = _wlt_AppConfig.CompanyLogoFolderPath;

                    string filename = ds.Tables[0].Rows[0]["LogoName"].ToString();


                    //TODO
                    //if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(filePath + "\\" + filename)))
                    //{
                    //    using (System.IO.FileStream fs = new System.IO.FileStream(System.Web.HttpContext.Current.Server.MapPath(filePath + "\\" + filename), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    //    {
                    //        Byte[] logobytes = (Byte[])ds.Tables[0].Rows[0]["vLogo"];
                    //        fs.Write(logobytes, 0, logobytes.Length);
                    //        fs.Close();
                    //    }
                    //}

                    returnstring = filePath + filename;
                }

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsCompany.cs", "YourPersonalizedLogo()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }




        public DataSet TimeZone()
        {
            string returnstring = "";

            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_Timezone", param);

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsCompany.cs", "sp_Timezone", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return ds;
        }

        public DataSet Language()
        {
            string returnstring = "";

            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_Language", param);

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsCompany.cs", "sp_Timezone", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return ds;
        }

        public DataSet FuelUnits()
        {
            string returnstring = "";

            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_FuelUnits", param);

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsCompany.cs", "FuelUnits", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return ds;
        }


        public DataSet Speed()
        {
            string returnstring = "";

            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_Speed", param);

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsCompany.cs", "sp_Speed", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return ds;
        }


        public DataSet GetClientsForMasterDDL()
        {
            string returnstring = "";

            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[1].Value = LoggedInUserID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_GetClientsForMasterDDL", param);

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetClientsForMasterDDL", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return ds;

        }
        public DataSet SmsProvider()
        {
            string returnstring = "";
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Sp_SaveResellerSMS", param);

            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("clsCompany.cs", "SmsProvider", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }

            return ds;

        }



        public string SaveMtrackMain()
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[7];

            try
            {
                param[0] = new SqlParameter("@SimIcc", SqlDbType.BigInt);
                param[0].Value = vDeviceId;

                param[1] = new SqlParameter("@MSISDN", SqlDbType.BigInt);
                param[1].Value = 0;

                param[2] = new SqlParameter("@ParameterName", SqlDbType.NVarChar, 60);
                param[2].Value = vParameterName;

                param[3] = new SqlParameter("@ParameterCode", SqlDbType.Int);
                param[3].Value = vParameterCode;

                param[4] = new SqlParameter("@ParameterValue", SqlDbType.Int);
                param[4].Value = vParameterValue;

                param[5] = new SqlParameter("@bProcessed", SqlDbType.Bit);
                param[5].Value = 0;

                param[6] = new SqlParameter("@retVal", SqlDbType.Int);
                param[6].Direction = ParameterDirection.ReturnValue;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "sp_SaveUnitControlMtrack", param);

                returnstring = param[6].Value.ToString();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveMtrackMain()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error";
            }

            return returnstring;
        }


        //save uploaded favicon image
        public string SaveUploadedFavicon()
        {
            SqlParameter[] param = new SqlParameter[5];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[3].Value = iCreatedBy;

                param[4] = new SqlParameter("@FaviconName", SqlDbType.NVarChar);
                param[4].Value = FaviconName;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "CompanyLogo", param);

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
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveUploadedImage()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }

        //save uploaded Background image
        public string SaveUploadedBackgroundImage()
        {
            SqlParameter[] param = new SqlParameter[5];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[3].Value = iCreatedBy;

                param[4] = new SqlParameter("@vBackgroundImageName", SqlDbType.NVarChar);
                param[4].Value = vBackgroundImageName;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "CompanyLogo", param);

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
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveUploadedBackgroundImage()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }

        public DataSet CheckSMTPConfig()
        {
            DataSet ds = new DataSet();
            //string results = "";
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "CheckSMTPConfig()", ex.Message  + ex.StackTrace);

            }

            return ds;

        }

        public string RestoreDefaultImage()
        {
            SqlParameter[] param = new SqlParameter[3];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                if (param[2].Value.ToString() == "1")
                {
                    returnstring = "1";
                }

                else
                {
                    returnstring = "-1";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "RestoreDefaultImage()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }


        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }


        public clsCompany(int ipkCompanyID, string vCompanyName, string vCompanyLicencedURL,
            string vHelpUrl, int ifkThemeID, string vImage, string vFavicon, string vAddress,
            string vCity, string vRegion, int ifkCountryID, int iParentID, int iCreatedBy,
            bool bStatus, string vpkCountry, string vThemeName, string SMTPServer, int Port,
            string From, string Password, bool UseDefaultSMTP, bool EnableSSL, List<clsCompany> lstMapLayers,
            string SMSAccountSid, string SMSAuthenticationToken, string SMSNumberFrom,
            int SMSMonthlyLimit, bool SmsEnabled, bool IsResellerSmsEnabled, int ifkSmsProvider,
            bool EmailEnabled, string CustomLink1, string CustomLink2, string CustomLink3,
            string CustomLink1Text, string CustomLink2Text, string CustomLink3Text, string CustomLink1ToolTip,
            string CustomLink2ToolTip, string CustomLink3ToolTip, int vLanguage, string vLanguageName,
            string vPageheading, string vBackgroundImage, int Trial, DateTime DateToDisableAccess,
            bool bUseHttps, string SmsFailureNotifyEmail)
        {
            this.ipkCompanyID = ipkCompanyID;
            this.vCompanyName = vCompanyName;
            this.vCompanyLicencedURL = vCompanyLicencedURL;
            this.vHelpUrl = vHelpUrl;
            this.ifkThemeID = ifkThemeID;
            this.vImage = vImage;
            this.vFavicon = vFavicon;
            this.vAddress = vAddress;
            this.vCity = vCity;
            this.vRegion = vRegion;
            this.ifkCountryID = ifkCountryID;
            this.iParentID = iParentID;
            this.iCreatedBy = iCreatedBy;
            this.bStatus = bStatus;
            this.vpkCountry = vpkCountry;
            this.vThemeName = vThemeName;
            this.SmtpServer = SMTPServer;
            this.Port = Port;
            this.From = From;
            this.Password = Password;
            this.EnableSsl = EnableSSL;
            this.UseDefaultSmtp = UseDefaultSMTP;
            this.lstMapLayers = lstMapLayers;
            this.SMSAccountSid = SMSAccountSid;
            this.SMSAuthenticationToken = SMSAuthenticationToken;
            this.SMSNumberFrom = SMSNumberFrom;
            this.SMSMonthlyLimit = SMSMonthlyLimit;
            this.SmsEnabled = SmsEnabled;
            this.IsResellerSmsEnabled = IsResellerSmsEnabled;
            this.ifkSmsProvider = ifkSmsProvider;
            this.EmailEnabled = EmailEnabled;
            this.CustomLink1 = CustomLink1;
            this.CustomLink2 = CustomLink2;
            this.CustomLink3 = CustomLink3;
            this.CustomLink1Text = CustomLink1Text;
            this.CustomLink2Text = CustomLink2Text;
            this.CustomLink3Text = CustomLink3Text;
            this.CustomLink1ToolTip = CustomLink1ToolTip;
            this.CustomLink2ToolTip = CustomLink2ToolTip;
            this.CustomLink3ToolTip = CustomLink3ToolTip;
            this.vLanguage = vLanguage;
            this.vLanguageName = vLanguageName;
            this.vPageheading = vPageheading;
            this.vBackgroundImage = vBackgroundImage;
            this.IstTrial = Trial;
            this.bUseHttps = bUseHttps;
            this.SmsFailureNotifyEmail = SmsFailureNotifyEmail;
        }

        public DataSet AdvancedSearch()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkUserID", SqlDbType.BigInt);
                param[1].Value = ifkUserId;

                param[2] = new SqlParameter("@SearchValue", SqlDbType.VarChar);
                param[2].Value = SearchValue;

                param[3] = new SqlParameter("@SearchType", SqlDbType.Int);
                param[3].Value = SearchType;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdvancedSearch", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "AdvancedSearch()", ex.Message  + ex.StackTrace);

            }

            return ds;
        }

        public DataSet GetAssetSearchedData()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[4];

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[1].Value = iParentID;

                param[2] = new SqlParameter("@IMEI", SqlDbType.BigInt);
                param[2].Value = vDeviceId;

                param[3] = new SqlParameter("@SearchType", SqlDbType.Int);
                param[3].Value = SearchType;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdvancedSearch", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "AdvancedSearch()", ex.Message  + ex.StackTrace);

            }

            return ds;
        }

        public DataSet GetAssetsForShareDDL()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];

            try
            {
                param[0] = new SqlParameter("@operation", SqlDbType.Int);
                param[0].Value = 147;

                param[1] = new SqlParameter("@iParent", SqlDbType.BigInt);
                param[1].Value = iParentID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_ClientDevices", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "AdvancedSearch()", ex.Message  + ex.StackTrace);

            }

            return ds;
        }

        public string SaveClientGeocoder()
        {
            SqlParameter[] param = new SqlParameter[6];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[1].Value = ipkCompanyID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@ifkGeocoderId", SqlDbType.Int);
                param[3].Value = ifkGeocoderId;

                param[4] = new SqlParameter("@GeocoderUserKey", SqlDbType.VarChar);
                param[4].Value = GeocoderUserKey;

                param[5] = new SqlParameter("@bUseResellerGeocoder", SqlDbType.Bit);
                param[5].Value = bUseResellerGeocoder;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                if (param[2].Value.ToString() == "1")
                {
                    returnstring = "1";
                }

                else
                {
                    returnstring = "-1";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "SaveClientGeocoder()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }

        public string MoveClient(int ClientID, int ResellerID)
        {
            SqlParameter[] param = new SqlParameter[3];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@ClientID", SqlDbType.Int);
                param[0].Value = ClientID;

                param[1] = new SqlParameter("@ResellerID", SqlDbType.Int);
                param[1].Value = ResellerID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "sp_MoveClient", param);

                if (param[2].Value.ToString() == "1")
                {
                    returnstring = "1";
                }

                else
                {
                    returnstring = "-1";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "MoveClient()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }


        public DataSet GetClientParent(int ClientID)
        { 
            SqlParameter[] param = new SqlParameter[3];

            var ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[0].Value = ClientID;


                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = 56;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                 ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);
                            

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "MoveClient()", ex.Message + ex.StackTrace);
              
            }
            return ds;
        }

        public DataSet GetDriverSMSes(int ClientID, string phoneNumber)
        {
            SqlParameter[] param = new SqlParameter[3];

            var ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@ifk_client_id", SqlDbType.Int);
                param[0].Value = ClientID;


                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = 2;

                param[2] = new SqlParameter("@phone", SqlDbType.VarChar);
                param[2].Value = phoneNumber;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_clientsms", param);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetDriverSMSes()", ex.Message + ex.StackTrace);

            }
            return ds;
        }

        public DataSet ShowAvailableContacts(int ClientID)
        {
            SqlParameter[] param = new SqlParameter[2];

            var ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@ifk_client_id", SqlDbType.Int);
                param[0].Value = ClientID;


                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = 4;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_clientsms", param);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "ShowAvailableContacts()", ex.Message + ex.StackTrace);

            }
            return ds;
        }

        public DataSet GetWorkModes()
        {
            SqlParameter[] param = new SqlParameter[2];

            var ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[0].Value = ipkCompanyID;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetWorkModes()", ex.Message + ex.StackTrace);

            }
            return ds;
        }

        public string SaveWorkMode()
        {
            SqlParameter[] param = new SqlParameter[6];
            string result = "";

            try
            {
                param[0] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[0].Value = ipkCompanyID;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@work_mode_name", SqlDbType.VarChar);
                param[2].Value = WorkModeName;

                param[3] = new SqlParameter("@ifk_mode_id", SqlDbType.Int);
                param[3].Value = WorkModeId;

                param[4] = new SqlParameter("@work_mode_color", SqlDbType.VarChar);
                param[4].Value = WorkModeColor;

                param[5] = new SqlParameter("@Error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                result = param[5].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "GetWorkModes()", ex.Message + ex.StackTrace);

            }
            return result;
        }

        public string DeleteWorkMode()
        {
            SqlParameter[] param = new SqlParameter[4];
            string result = "";

            try
            {
                param[0] = new SqlParameter("@ipkCompanyID", SqlDbType.Int);
                param[0].Value = ipkCompanyID;

                param[1] = new SqlParameter("@Operation", SqlDbType.Int);
                param[1].Value = Operation;

                param[2] = new SqlParameter("@ifk_mode_id", SqlDbType.Int);
                param[2].Value = WorkModeId;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);

                result = param[3].Value.ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsCompany.cs", "DeleteWorkMode()", ex.Message + ex.StackTrace);

            }
            return result;
        }

        public class CompanyDDL
        {
            public string vCompanyName { get; set; }
            public int ipkCompanyID { get; set; }

            public bool IncludeMiniResellerClients { get; set; }

            public string Message { get; set; }

            public CompanyDDL()
            {

            }
            public CompanyDDL(string vCompanyName)
            {
                this.vCompanyName = vCompanyName;
            }

            public CompanyDDL(string vCompanyName, int ipkCompanyID)
            {
                this.vCompanyName = vCompanyName;
                this.ipkCompanyID = ipkCompanyID;
            }
        }

        public class AssetsDDL
        {
            public string Name { get; set; }
            public int ifkDeviceID { get; set; }

            public AssetsDDL(string Name)
            {
                this.Name = Name;
            }

            public AssetsDDL(string Name, int ifkDeviceID)
            {
                this.Name = Name;
                this.ifkDeviceID = ifkDeviceID;
            }
        }
    }
}
