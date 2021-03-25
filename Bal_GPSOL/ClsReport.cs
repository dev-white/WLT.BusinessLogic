using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Text;
using WLT.BusinessLogic.BAL;
using WLT.DataAccessLayer;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class ClsReport : IDisposable
    {
        #region Variables
        private long _nStartEventInst;

        public int ActivityId { get; set; }

        public string ResponseMessage { get; set; }
        public string TimeZoneID { get; set; }
        private long _nEndEventInst;
        private string _vpkDeviceID;
        private List<ClsReport> _TripPoint;
        private int _top;
        private int _AnalogId;

        int _TrackerAssetType;

        private float _Odometer;
        public float Odometer
        {
            get { return _Odometer; }
            set { _Odometer = value; }
        }
        public bool IncludeMini_ResellerClients { get; set; }
        public int isShow
        {
            get; set;
        }

        public int TrackerAssetType
        {
            get { return _TrackerAssetType; }
            set { _TrackerAssetType = value; }
        }
        public int AnalogId
        {
            get { return _AnalogId; }
            set { _AnalogId = value; }
        }


        public string vDriverIds { get; set; }

        public int WebHookGroupingId { get; set; }
        public bool bIncludeVehicleDetail { get; set; }
        public bool bAnyDriver { get; set; }

        public string Attributes { get; set; }
        public string vpkDeviceID
        {
            get { return _vpkDeviceID; }
            set { _vpkDeviceID = value; }
        }

        public long nEndEventInst
        {
            get { return _nEndEventInst; }
            set { _nEndEventInst = value; }
        }

        public long nStartEventInst
        {
            get { return _nStartEventInst; }
            set { _nStartEventInst = value; }
        }
        private DateTime _dDateFrom;

        public DateTime dDateFrom
        {
            get { return _dDateFrom; }
            set { _dDateFrom = value; }
        }

        private DateTime _dDateTo;
        private object p;


        public bool isAnyDriver { get; set; }
        public string sDriverIDsCSVs { get; set; }


        public bool isCustomDateEnabled { get; set; }
        public int iEnabledDateType { get; set; }

        public int iTripType { get; set; }
        public string DeviceOrGroupName { get; set; }
        public DateTime dDateTo
        {
            get { return _dDateTo; }
            set { _dDateTo = value; }
        }
        public bool bIsForAllCompanies { get; set; }

        public int FilterCode { get; set; }
        public int operation { get; set; }
        public int report_type_id { get; set; }
        public bool status { get; set; }
        public string report_name { get; set; }
        public int companyid { get; set; }
        public bool isAnyAsset { get; set; }
        public int AssetTrackerType { get; set; }
        public string vAssetList_CSV { get; set; }

        public string vClient_CSV { get; set; }

        public int iAnyClient { get; set; }
        public string DeviceID { get; set; }
        public int GroupMID { get; set; }
        public string Zone { get; set; }
        public string ZoneType { get; set; }
        public string ZoneTypeId { get; set; }
        public int GeoMID { get; set; }
        public string Trip { get; set; }
        public int TriggeredEventID { get; set; }
        public bool TriggeredEventStatus { get; set; }
        public int ifkDigitalMasterID { get; set; }
        public string vDigitalEvent { get; set; }
        public bool DigitalEventStatus { get; set; }
        public string AnaloglEventName { get; set; }
        public bool AnalogEventStatus { get; set; }
        public bool IsTemeperatureViolation { get; set; }
        public bool IsOverspeed { get; set; }
        public bool IsExcessiveIdle { get; set; }
        public List<ClsReport> lstTimePeriodContent { get; set; }
        public List<ClsReport> lstOtherContent { get; set; }
        public int ifkUserID { get; set; }
        public string UserIDs { get; set; }
        public string RoleIDs { get; set; }
        public int ReportID { get; set; }
        public bool bHoursType { get; set; }

        public string vStartsTime { get; set; }
        public string Frequency { get; set; }
        public string vEndsTime { get; set; }
        public string vDaysOfWeek { get; set; }

        public string TimeRange { get; set; }
        public bool isAnyZone { get; set; }
        public string ParamZoneID_Csv { get; set; }
        public string vEventName { get; set; }
        public bool bEventStatus { get; set; }
        public string Reminder { get; set; }
        public List<ClsReport> TripPoint { get { return _TripPoint; } set { _TripPoint = value; } }
        public string HTML { get; set; }

        public int top { get { return _top; } set { _top = value; } }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime StratDateReport { get; set; }
        public DateTime EndDateReport { get; set; }
        public string Description { get; set; }
        public bool IsScheduled { get; set; }
        public bool bIsSystem { get; set; }
        public string Format { get; set; }
        public string Reportnewid { get; set; }
        public string Reporttypenewid { get; set; }

        public int iAlertPriority { get; set; }
        public string alert_Csv { get; set; }


        //Added By Shivam
        public int ReportTypeId { get; set; }
        public string ShowMeDataFor { get; set; }

        public string FrequecyParameters { get; set; }

        public string vSpeedType { get; set; }

        public int AlertId { get; set; }
        public string IgnitionState { get; set; }
        public string BusinessInput { get; set; }
        public string DigitalType { get; set; }
        public string IdlingReportMaxDuration { get; set; }
        public string OverSpeedMode { get; set; }
        public string OverSpeedMaxSpeed { get; set; }
        public string StopReportType { get; set; }
        public string StopReportTypeMinDuration { get; set; }
        public string TripListingMinDistance { get; set; }
        public string SendReportDays { get; set; }
        public string AtThisTime { get; set; }

        // End Added By Shivam

        //Added by Lawrence: To handle the device in use/device not in use
        public int DeviceCompanyID { get; set; }
        public int DeviceParentID { get; set; }
        public int SelectInactive { get; set; }
        public int iCreatedByID { get; set; }
        public int iUpdatedByID { get; set; }

        public string iCreatedBy { get; set; }
        public string iCreatedOn { get; set; }
        public string iUpdatedBy { get; set; }
        public string iUpdatedOn { get; set; }
        public int iDigitalInputMapping { get; set; }

        public int iReportDisplayType { get; set; }

        public string vAdditionalParams { get; set; }

        public int ifkMeasurementUnit { get; set; }

        


        // End Added By Lawrence
        #endregion

        private readonly wlt_Config _wlt_AppConfig;

        private readonly string Connectionstring;
        public ClsReport()
        {
           

            _wlt_AppConfig = AppConfiguration.GetAppSettings<wlt_Config>("wlt_config");

            Connectionstring = AppConfiguration.GetAppSettings<wlt_Config>("ConnectionStrings").wlt_WebAppConnectionString;


            this.lstTimePeriodContent = new List<ClsReport>();
            this.lstOtherContent = new List<ClsReport>();
            this.TripPoint = new List<ClsReport>();

        }
      
        #region Constructors       

        public ClsReport(long nStartEventInst, long nEndEventInst, string vpkDeviceID)
        {
            this.nStartEventInst = nStartEventInst;
            this.nEndEventInst = nEndEventInst;
            this.vpkDeviceID = vpkDeviceID;
        }

        public ClsReport(object p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }

        public ClsReport(string reportName, string description)
        {
            // TODO: Complete member initialization
            this.report_name = reportName;
            this.Description = description;
        }

        public ClsReport(bool bHoursType, string vStartsTime, string vEndsTime, string vDaysOfWeek, string html)
        {
            // TODO: Complete member initialization
            this.bHoursType = bHoursType;
            this.vStartsTime = vStartsTime;
            this.vEndsTime = vEndsTime;
            this.vDaysOfWeek = vDaysOfWeek;
            this.HTML = html;
        }


        #endregion

        #region Methods
     
        public DataSet AssetRecentEvent()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 8;

                param[1] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[1].Value = vpkDeviceID;

                param[2] = new SqlParameter("@top", SqlDbType.Int);
                param[2].Value = top;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "NewFrontData_sp", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "AssetRecentEvent()", ex.Message  + ex.StackTrace);
            }


            return ds;
        }

        public string PrepareReportlist(clsRegistration objclsRegistration)
        {
            StringBuilder listHtml = new StringBuilder();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 1;

                param[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                param[1].Value = companyid;

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_Reports", param);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //listHtml.Append("<p class='report-title'>Available Reports</p>");
                    //listHtml.Append("<p class='report-title1'>to add a new report click the Create Report button above</p>");

                    //listHtml.Append("<p id='report_title' class='report-title'>Custom & Scheduled Reports</p>");
                    //listHtml.Append("<p id='report_subtitle' class='report-title1'>here you can create your own reports based on your desired parameters</p>");


                    //listHtml.Append("<input type='button' id='btnAddNewReports' class='btnCreateAlertClass' value='Create report' onclick='return CreateNewReports();' />");

                  
                    string timeZoneID = objclsRegistration.vTimeZoneID;
                    string UserId = Convert.ToString(objclsRegistration.pkUserID);

                    foreach (DataRow drReport in ds.Tables[0].Rows)
                    {

                        var _reportTypeClass = Convert.ToInt32(drReport["reportDisplayType"]) == 4 ? "webhook-report" : "normal-report";

                        listHtml.Append("<div data-display-type ='" + Convert.ToInt32(drReport["reportDisplayType"]) + "' class='position-notification1 " + _reportTypeClass + "' id='divReportNotification-" + Convert.ToInt32(drReport["ipkReportId"]) + "' >");
                        listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0' align='left'>");
                        listHtml.Append("<tr>");

                        listHtml.Append("<td width='60' valign='top' align='left'>");
                        if (Convert.ToBoolean(drReport["IsScheduleReport"]))
                        {
                            if (Convert.ToBoolean(drReport["bStatus"]))
                            {
                                listHtml.Append("<span align='top' class='r_RowIcon reports-icon-scheduled35' style='width:35px;'></span>");
                            }
                            else
                            {
                                listHtml.Append("<span align='top' class='r_RowIcon reports-icon-scheduled-disabled35' style='width:35px;'></span>");
                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(drReport["bStatus"]))
                            {
                                listHtml.Append("<span align='top' class='r_RowIcon reports-icon35' style='width:35px;'></span>");
                            }
                            else
                            {
                                listHtml.Append("<img align='top' class='r_RowIcon'  src='../images/reports_Icon_Disabled.png' style='width:35px;'>");
                            }
                        }
                        listHtml.Append("</td>");

                        listHtml.Append("<td>");
                        listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                        listHtml.Append("<tr>");
                        listHtml.Append("<td width='555'>");
                        if (Convert.ToBoolean(drReport["bStatus"]))
                        {
                            listHtml.Append("<p class='position-overspeeding'  title='" + Convert.ToString(drReport["vReportName"]) + "'");
                        }
                        else
                        {
                            listHtml.Append("<p class='Alert_falseStatus' title='" + Convert.ToString(drReport["vReportName"]) + "' ");
                        }
                        //listHtml.Append(" onclick='return ViewReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ");'>");
                        listHtml.Append(" onclick='return ViewReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ",\"" + timeZoneID + "\",\"" + UserId + "\"," + 2 + ",\"" + Convert.ToString(drReport["vReportName"]) + "\");'>");
                        listHtml.Append(drReport["vReportName"]);
                        listHtml.Append("</p>");
                        listHtml.Append("</td>");


                        listHtml.Append("<td valign='top'>");

                        //if (Convert.ToBoolean(drReport["bStatus"]))
                        //{
                        //    listHtml.Append("<img class='img_status' src='../images/online-icon.png'>");
                        //}
                        //else
                        //{
                        //    listHtml.Append("<img class='img_status' src='../images/offline-icon.png'>");
                        //}
                        listHtml.Append("<div class='PopupExtra'  style='position: relative;float: right;margin-right: 15px;margin-top: 2px;'  onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportId"]) + ",\"report\");' >");

                        //listHtml.Append("<img style='float: left; margin: 7px 0px 0 5px;cursor: pointer;' src='../images/arrow-b.png' onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportId"]) + ",\"report\");' />");
                        listHtml.Append("<i class='icon-options'></i>");

                        //listHtml.Append("</div>");
                        listHtml.Append("<div id='divReportAction-" + Convert.ToInt32(drReport["ipkReportId"]) + "' class='ConfigureAlertNotificationPopup PopupExtra' style='display: none;'>");
                        listHtml.Append("<div id='Div2' >");
                        listHtml.Append("<table border='0' cellpadding='0' cellspacing='0' >");
                        listHtml.Append("<tr class='tdHoverReport'>");
                        listHtml.Append("<td >");
                        listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='20px' height='15px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 736 1024'><path d='M416 0H64Q37 0 18.5 18.5T0 64v896q0 27 18.5 45.5T64 1024h608q27 0 45.5-18.5T736 960V320zm256 347v5H384V64h6zM64 960V64h256v352h352v544H64z' fill='#626262'/></svg>");
                        listHtml.Append("</td>");
                        listHtml.Append("<td>");
                        listHtml.Append("<p class='popu_operationn'  onclick='return EditReport(" + Convert.ToInt32(drReport["ipkReportID"]) + ");'> Edit Report</p>");
                        listHtml.Append("</td>");
                        listHtml.Append("</tr>");
                        listHtml.Append("<tr>");
                        listHtml.Append("<td >");


                        if (Convert.ToBoolean(drReport["bStatus"]))
                        {

                            listHtml.Append("<img alt='' src='../Images/cross-icon.png'  style='margin-right: 5px;vertical-align: middle' />");
                        }
                        else
                        {
                            listHtml.Append("<img alt='' src='../Images/tickSmall.png'  style='margin-right: 5px;vertical-align: middle' />");
                        }

                        listHtml.Append("</td>");

                        listHtml.Append("<td>");
                        if (Convert.ToBoolean(drReport["bStatus"]))
                        {
                            listHtml.Append("<p class='popu_operationn' onclick='return ReportOptions(" + Convert.ToInt32(drReport["ipkReportID"]) + ",\"status\"," + companyid + ",\"" + !Convert.ToBoolean(drReport["bStatus"]) + "\");'> Set as Inactive</p>");
                        }
                        else
                        {
                            listHtml.Append("<p class='popu_operationn' onclick='return ReportOptions(" + Convert.ToInt32(drReport["ipkReportID"]) + ",\"status\"," + companyid + ",\"" + !Convert.ToBoolean(drReport["bStatus"]) + "\");'> Set as Active</p>");
                        }
                        listHtml.Append("</td>");

                        listHtml.Append("</tr>");

                        listHtml.Append("<tr>");

                        listHtml.Append("<td >");
                        listHtml.Append("<img alt='' src='../Images/delete-icon.png' style='margin-right: 5px; vertical-align: middle' />");
                        listHtml.Append("</td>");

                        listHtml.Append("<td>");
                        listHtml.Append("<p class='popu_operationn' onclick='return ReportOptions(" + Convert.ToInt32(drReport["ipkReportID"]) + ",\"delete\"," + companyid + ",\"\");'> Delete Report</p>");
                        listHtml.Append("</td>");

                        listHtml.Append("</tr>");

                        listHtml.Append("<tr>");

                        listHtml.Append("<td >");
                        listHtml.Append("<img alt='' src='../Images/edit-icon.png' style='margin-right: 5px; vertical-align: middle' />");
                        listHtml.Append("</td>");

                        listHtml.Append("<td>");
                        listHtml.Append("<p class='popu_operationn' onclick='return DuplicateReport(" + Convert.ToInt32(drReport["ipkReportID"]) + ");'> Duplicate Report</p>");
                        listHtml.Append("</td>");

                        listHtml.Append("</tr>");

                        /**/
                        if (!Convert.ToBoolean(drReport["IsScheduleReport"]))
                        {
                            listHtml.Append("<tr>");

                            listHtml.Append("<td >");
                            listHtml.Append("<img alt='' src='../Images/edit-icon.png' style='margin-right: 5px; vertical-align: middle' />");
                            listHtml.Append("</td>");

                            listHtml.Append("<td>");
                            listHtml.Append("<p class='popu_operationn' onclick='return DuplicateReport(" + Convert.ToInt32(drReport["ipkReportID"]) + ");'> Schedule Report</p>");
                            listHtml.Append("</td>");

                            listHtml.Append("</tr>");
                        }
                        /**/

                        listHtml.Append("<tr>");

                        listHtml.Append("<td colspan='2' style='display:none' >");
                        listHtml.Append("<img alt='' src='../Images/line.png'  style='float: left; vertical-align: middle;margin: 4px;' />");
                        listHtml.Append("</td>");

                        listHtml.Append("</tr>");

                        listHtml.Append("<tr>");

                        listHtml.Append("<td>");
                        listHtml.Append("<img alt='' src='../Images/right-icon.png' style='margin-right: 5px; vertical-align: middle' />");
                        listHtml.Append("</td>");

                        listHtml.Append("<td>");
                        listHtml.Append("<p style='color: #235C9F;margin: 4px;cursor: pointer; text-transform:uppercase; margin-left: 17px;' ");
                        //listHtml.Append(" onclick='return ViewReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ");'> Show Results</p>");
                        listHtml.Append(" onclick='return ViewReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ",\"" + timeZoneID + "\",\"" + UserId + "\"," + 2 + ",\"" + Convert.ToString(drReport["vReportName"]) + "\");'> Show Results</p>");
                        listHtml.Append("</td>");

                        listHtml.Append("</tr>");
                        listHtml.Append("</table>");
                        listHtml.Append("</div>");
                        listHtml.Append("</div>");

                        listHtml.Append("</td>");
                        listHtml.Append("</tr>");

                        listHtml.Append("<tr>");
                        listHtml.Append("<td colspan='2'>");
                        listHtml.Append("<div class='teb'>");
                        listHtml.Append(Convert.ToString(drReport["vDescription"]));
                        listHtml.Append("</div>");
                        listHtml.Append("</td>");

                        listHtml.Append("</tr>");
                        listHtml.Append("</table>");
                        listHtml.Append("</td>");
                        listHtml.Append("</tr>");
                        listHtml.Append("</table>");
                        listHtml.Append("</div>");
                    }

                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "PrepareReportlist()", ex.Message  + ex.StackTrace);
                listHtml.Append("");
            }
            return listHtml.ToString();
        }

        public string PrepareReportDetailHtml(clsRegistration objclsRegistration, Func<string, bool>  PemissionChecker)
        {
            StringBuilder listHtml = new StringBuilder();
            try
            {
                DataSet dsReports;


                string timeZoneID = objclsRegistration.vTimeZoneID;
                string UserId = Convert.ToString(objclsRegistration.pkUserID);
                ifkUserID = Convert.ToInt32(UserId);
                string userType = Convert.ToString(objclsRegistration.ifkUserTypeID);
                switch (report_type_id.ToString())
                {
                    case "1":  // static Reports
                        #region Reports

                        dsReports = GetReportList();

                        if (dsReports != null && dsReports.Tables.Count > 0 && dsReports.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drReport in dsReports.Tables[0].Rows)
                            {
                                var reportDisplayType = Convert.ToInt32(drReport["reportDisplayType"]);


                                var diplayClassName = reportDisplayType == 4 ? "json-display" : "normal-display";

                                if (userType == "3")
                                {



                                    {
                                        if (Convert.ToBoolean(drReport["bStatus"]))
                                        {
                                            //if (Convert.ToString(drReport["vReportTypeName"]) != null && Convert.ToString(drReport["vReportTypeName"]) != "")
                                            //{

                                            listHtml.Append("<img id='imgbtnstaticreport' alt='loading...' src='../images/Loading.gif' class='imgload_alert_TripReplay' style='display: none; left: -15px;' />");
                                            listHtml.Append("<div  class='position-notification1  " + diplayClassName + "' id='divReportNotification-" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "' >");
                                            listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0' align='left'>");

                                            listHtml.Append("<tr>");
                                            listHtml.Append("<td width='60' valign='top' align='left'>");

                                            if (Convert.ToBoolean(drReport["bStatus"]))
                                            {
                                                if (Convert.ToInt32(drReport["reportDisplayType"]) == 1)
                                                {
                                                    listHtml.Append("<span align='top' class='r_RowIcon reports-icon35' style='width:35px;'></span>");
                                                }
                                                else
                                                {
                                                    listHtml.Append("<span align='top' class='r_RowIcon graph-con-fin4-35' style='width:35px;'></span>");
                                                }
                                            }
                                            else
                                            {
                                                listHtml.Append("<img align='top' class='r_RowIcon' src='../images/reports_Icon_Disabled.png' style='width:35px;'>");
                                            }

                                            listHtml.Append("</td>");
                                            listHtml.Append("<td>");
                                            listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0'>");

                                            listHtml.Append("<tr>");
                                            listHtml.Append("<td width='555'>");

                                            if (Convert.ToBoolean(drReport["bStatus"]))
                                            {
                                                listHtml.Append("<p class='position-overspeeding'  title='" + Convert.ToString(drReport["vReportTypeName"]) + "'");
                                            }
                                            else
                                            {

                                                listHtml.Append("<p class='Alert_falseStatus' title='" + Convert.ToString(drReport["vReportTypeName"]) + "' ");
                                            }

                                            listHtml.Append(" onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\"," + Convert.ToInt32(drReport["reportDisplayType"]) + "," + Convert.ToInt32(drReport["isAnalog"]) + "," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_" + Convert.ToString(drReport["vReportTypeName"]) + "'>");
                                            listHtml.Append(drReport["vReportTypeName"]);
                                            listHtml.Append("</p>");

                                            listHtml.Append("<td valign='top'>");

                                            listHtml.Append("<div class='PopupExtra'  style='position: relative;float: right;margin-right: 15px;margin-top: 2px;' onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"report\");'>");

                                            ;

                                            listHtml.Append("<i class='icon-options'></i>");



                                            if (PemissionChecker("EDIT_REPORT"))
                                            {

                                                listHtml.Append("<div id='divReportAction-" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "' class='ConfigureAlertNotificationPopup PopupExtra' style='display: none;'>");
                                                listHtml.Append("<div id='Div2' >");


                                                listHtml.Append("<table border='0' cellpadding='0' cellspacing='0' >");


                                                if (PemissionChecker("SCHEDULE_REPORT"))
                                                {
                                                    listHtml.Append("<tr class='tdHoverReport'>");
                                                    listHtml.Append("<td >");
                                                    listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='20px' height='15px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 736 1024'><path d='M416 0H64Q37 0 18.5 18.5T0 64v896q0 27 18.5 45.5T64 1024h608q27 0 45.5-18.5T736 960V320zm256 347v5H384V64h6zM64 960V64h256v352h352v544H64z' fill='#626262'/></svg>");
                                                    listHtml.Append("</td>");

                                                    listHtml.Append("<td>");
                                                    listHtml.Append("<p class='popu_operationn'  onclick='return ScheduledReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "," + 1 + ");' data-localize='rpt_Schedule'> Schedule this</p>");
                                                    listHtml.Append("</td>");
                                                    listHtml.Append("</tr>");
                                                }



                                               
                                                    listHtml.Append("<tr class='tdHoverReport'>");
                                                    listHtml.Append("<td colspan='2' style='display:none' >");
                                                    listHtml.Append("<img alt='' src='../Images/line.png'  style='float: left; vertical-align: middle;margin: 4px;' />");
                                                    listHtml.Append("</td>");
                                                    listHtml.Append("</tr>");



                                              
                                                    listHtml.Append("<tr class='tdHoverReport'>");
                                                    listHtml.Append("<td>");
                                                    listHtml.Append("<img alt='' src='../Images/right-icon.png' style='margin-right:5px; vertical-align: middle' />");
                                                    listHtml.Append("</td>");

                                                    listHtml.Append("<td>");
                                                    listHtml.Append("<p class = 'popu_operationn' style='cursor: pointer; text-transform:uppercase;' ");
                                                    listHtml.Append(" onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\"," + Convert.ToInt32(drReport["reportDisplayType"]) + "," + Convert.ToInt32(drReport["isAnalog"]) + "," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_ShowResults'> Show Results</p>");
                                                    listHtml.Append("</td>");
                                                    listHtml.Append("</tr>");


                                                if (PemissionChecker("EXCLUDE_FAVORIRE_REPORT"))
                                                {
                                                    listHtml.Append("<tr>");
                                                    listHtml.Append("<td>");
                                                    listHtml.Append("<i  class='fa fa-trash sub-remove' aria-hidden='true'></i>");
                                                    listHtml.Append("</td>");
                                                    listHtml.Append("<td>");
                                                    listHtml.Append("<p class='popu_operationn' style = 'cursor: pointer; text-transform:uppercase;' onClick ='return UnpinReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ")'> Exclude </p>");
                                                    listHtml.Append("</td>");
                                                    listHtml.Append("</tr>");

                                                }

                                                listHtml.Append("</table>");

                                                listHtml.Append("</div>");
                                                listHtml.Append("</div>");
                                            }
                                        }

                                        listHtml.Append("</td>");
                                        listHtml.Append("</tr>");
                                        listHtml.Append("<tr>");
                                        listHtml.Append("<td colspan='2'>");
                                        listHtml.Append("<div class='teb' data-localize='rpt_" + Convert.ToString(drReport["vDescription"]) + "'>");
                                        listHtml.Append(Convert.ToString(drReport["vDescription"]));
                                        listHtml.Append("</div>");
                                        listHtml.Append("</td>");


                                        listHtml.Append("</tr>");
                                        listHtml.Append("</table>");

                                        listHtml.Append("</td>");

                                        listHtml.Append("</tr>");



                                        listHtml.Append("</table>");
                                        listHtml.Append("</div>");
                                    }
                                }
                                else
                                {
                                    if (Convert.ToBoolean(drReport["bStatus"]))
                                    {
                                        //if (Convert.ToString(drReport["vReportTypeName"]) != null && Convert.ToString(drReport["vReportTypeName"]) != "")
                                        //{

                                        listHtml.Append("<img id='imgbtnstaticreport' alt='loading...' src='../images/Loading.gif' class='imgload_alert_TripReplay' style='display: none; left: -15px;' />");
                                        listHtml.Append("<div class='position-notification1 " + diplayClassName + "' id='divReportNotification-" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "' >");
                                        listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0' align='left'>");

                                        listHtml.Append("<tr>");
                                        listHtml.Append("<td width='60' valign='top' align='left'>");

                                        if (Convert.ToBoolean(drReport["bStatus"]))
                                        {
                                            if (Convert.ToInt32(drReport["reportDisplayType"]) == 1)
                                            {
                                                listHtml.Append("<span align='top' class='r_RowIcon reports-icon35' style='width:35px;'></span>");
                                            }
                                            else
                                            {
                                                listHtml.Append("<span align='top' class='r_RowIcon graph-con-fin4-35' style='width:35px;'></span>");
                                            }
                                        }
                                        else
                                        {
                                            listHtml.Append("<img align='top' class='r_RowIcon' src='../images/reports_Icon_Disabled.png' style='width:35px;'>");
                                        }

                                        listHtml.Append("</td>");
                                        listHtml.Append("<td>");
                                        listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0'>");

                                        listHtml.Append("<tr>");
                                        listHtml.Append("<td width='555'>");

                                        if (Convert.ToBoolean(drReport["bStatus"]))
                                        {
                                            listHtml.Append("<p class='position-overspeeding'  title='" + Convert.ToString(drReport["vReportTypeName"]) + "'");
                                        }
                                        else
                                        {
                                            listHtml.Append("<p class='Alert_falseStatus' title='" + Convert.ToString(drReport["vReportTypeName"]) + "' ");
                                        }
                                        listHtml.Append("  onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\"," + Convert.ToInt32(drReport["reportDisplayType"]) + "," + Convert.ToInt32(drReport["isAnalog"]) + "," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_" + Convert.ToString(drReport["vReportTypeName"]) + "'>");
                                        // listHtml.Append(" onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\");' data-localize='rpt_" + Convert.ToString(drReport["vReportTypeName"]) + "'>");
                                        listHtml.Append(drReport["vReportTypeName"]);
                                        listHtml.Append("</p>");

                                        listHtml.Append("<td valign='top'>");




                                        if (PemissionChecker("EDIT_REPORT"))
                                        {
                                            listHtml.Append("<div class='PopupExtra'  style='position: relative;float: right;margin-right: 15px;margin-top: 2px;' onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"report\");'>");
                                            listHtml.Append("<i class='icon-options'></i>");
                                            //listHtml.Append("</div>");

                                            listHtml.Append("<div id='divReportAction-" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "' class='ConfigureAlertNotificationPopup PopupExtra' style='display: none;'>");
                                            listHtml.Append("<div id='Div2' >");
                                            listHtml.Append("<table border='0' cellpadding='0' cellspacing='0' >");



                                            if (PemissionChecker("SCHEDULE_REPORT"))
                                            {
                                                listHtml.Append("<tr class='tdHoverReport'>");
                                                listHtml.Append("<td>");
                                                listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 736 1024'><path d='M416 0H64Q37 0 18.5 18.5T0 64v896q0 27 18.5 45.5T64 1024h608q27 0 45.5-18.5T736 960V320zm256 347v5H384V64h6zM64 960V64h256v352h352v544H64z' fill='#626262'/></svg>");
                                                listHtml.Append("</td>");

                                                listHtml.Append("<td>");
                                                listHtml.Append("<p class='popu_operationn'  onclick='return ScheduledReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "," + 1 + ");' data-localize='rpt_Schedule'> Schedule this</p>");
                                                listHtml.Append("</td>");

                                                listHtml.Append("</tr>");
                                            }



                                            listHtml.Append("<tr class='tdHoverReport'>");
                                            listHtml.Append("<td colspan='2' style='display:none'>");
                                            listHtml.Append("<img alt='' src='../Images/line.png'  style='float: left; vertical-align: middle;margin: 4px;' />");
                                            listHtml.Append("</td>");
                                            listHtml.Append("</tr>");




                                            listHtml.Append("<tr class='tdHoverReport'>");
                                            listHtml.Append("<td>");
                                            listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M512 0Q373 0 255 68.5T68.5 255 0 512t68.5 257T255 955.5t257 68.5 257-68.5T955.5 769t68.5-257-68.5-257T769 68.5 512 0zm0 961q-73 0-141.5-22.5T247 874t-96.5-97-64-123.5T64 512q0-91 35.5-174T195 195t143-95.5T512 64t174 35.5T829 195t95.5 143T960 512t-35.5 174T829 829.5t-143 96T512 961zm204-636L416 627 281 492q-10-10-23-10t-23 10q-2 2-4 5.5t-3 6.5-1.5 6.5 0 7 1.5 7 3 6.5l4 6 159 158q9 10 22.5 10t22.5-10l4-4 319-321q9-9 9-22.5t-9-22.5q-18-18-39-5-4 2-7 5z' fill='#626262'/></svg>");
                                            listHtml.Append("</td>");

                                            listHtml.Append("<td>");
                                            listHtml.Append("<p class = 'popu_operationn' style= 'cursor: pointer; text-transform:uppercase;' ");
                                            listHtml.Append("  onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\"," + Convert.ToInt32(drReport["reportDisplayType"]) + "," + Convert.ToInt32(drReport["isAnalog"]) + "," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_ShowResults'> Show Results</p>");
                                            // listHtml.Append(" onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\");' data-localize='rpt_ShowResults'> Show Results</p>");
                                            listHtml.Append("</td>");
                                            listHtml.Append("</tr>");



                                            if (PemissionChecker("EXCLUDE_FAVORIRE_REPORT"))
                                            {
                                                listHtml.Append("<tr class='tdHoverReport'>");
                                                listHtml.Append("<td>");
                                                listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 832 1056'><path d='M801 175H576V88q0-20-10-37t-27-26.5-37-9.5H330q-30 0-52 21t-22 52v87H31q-13 0-22.5 9.5T-1 207t9.5 22.5T31 239h44l74 740q3 26 22.5 44t45.5 18h398q26 0 45.5-18t21.5-44l75-740h44q8 0 15.5-4.5t12-11.5 4.5-16q0-13-9.5-22.5T801 175zM320 88q0-10 10-10h172q10 0 10 10v87H320V88zm299 885q-1 4-4 4H217q-3 0-4-4l-73-734h552z' fill='#626262'/></svg>");
                                                listHtml.Append("</td>");
                                                listHtml.Append("<td>");
                                                listHtml.Append("<p  class = 'popu_operationn' style = 'cursor: pointer; text-transform:uppercase; ' onClick ='return UnpinReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ")'> Exclude </p>");
                                                listHtml.Append("</td>");
                                                listHtml.Append("</tr>");

                                            }

                                            listHtml.Append("</table>");

                                            listHtml.Append("</div>");
                                            listHtml.Append("</div>");
                                            //}
                                        }

                                        listHtml.Append("</td>");
                                        listHtml.Append("</tr>");
                                        listHtml.Append("<tr>");
                                        listHtml.Append("<td colspan='2'>");
                                        listHtml.Append("<div class='teb' data-localize='rpt_" + Convert.ToString(drReport["vDescription"]) + "'>");
                                        listHtml.Append(Convert.ToString(drReport["vDescription"]));
                                        listHtml.Append("</div>");
                                        listHtml.Append("</td>");


                                        listHtml.Append("</tr>");
                                        listHtml.Append("</table>");

                                        listHtml.Append("</td>");

                                        listHtml.Append("</tr>");



                                        listHtml.Append("</table>");
                                        listHtml.Append("</div>");
                                    }
                                }



                            }
                        }

                        #endregion
                        break;
                    case "2": // Custom Reports                       
                        #region Custom Reports

                        dsReports = GetReportList();

                        if (dsReports != null && dsReports.Tables.Count > 0 && dsReports.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow drReport in dsReports.Tables[0].Rows)
                            {

                                var _reportTypeClass = Convert.ToInt32(drReport["reportDisplayType"]) == 4 ? "webhook-report" : "normal-report";


                                listHtml.Append("<img id='imgbtnstaticreport' alt='loading...' src='../images/Loading.gif' class='imgload_alert_TripReplay' style='display: none; left: -15px;' />");
                                listHtml.Append("<div data-display-type ='" + Convert.ToInt32(drReport["reportDisplayType"]) + "' class='position-notification1  " + _reportTypeClass + " ' id='divReportNotification-" + Convert.ToInt32(drReport["ipkReportId"]) + "' >");
                                listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0' align='left'>");
                                listHtml.Append("<tr>");

                                listHtml.Append("<td width='60' valign='top' align='left'>");
                                if (Convert.ToBoolean(drReport["IsScheduleReport"]))
                                {
                                    if (Convert.ToBoolean(drReport["bStatus"]))
                                    {
                                        listHtml.Append("<span align='top' class='r_RowIcon reports-icon-scheduled35' style='width:35px;'></span>");
                                    }
                                    else
                                    {
                                        listHtml.Append("<span align='top' class='r_RowIcon reports-icon-scheduled-disabled35' style='width:35px;'></span>");
                                    }
                                }
                                else
                                {
                                    if (Convert.ToBoolean(drReport["bStatus"]))
                                    {
                                        listHtml.Append("<span align='top' class='r_RowIcon reports-icon35' style='width:35px;'></span>");
                                    }
                                    else
                                    {
                                        listHtml.Append("<img align='top' class='r_RowIcon' src='../images/reports_Icon_Disabled.png' style='width:35px;'>");
                                    }
                                }
                                listHtml.Append("</td>");

                                listHtml.Append("<td>");
                                listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                                listHtml.Append("<tr>");
                                listHtml.Append("<td width='555'>");
                                if (Convert.ToBoolean(drReport["bStatus"]))
                                {
                                    listHtml.Append("<p class='position-overspeeding'  title='" + Convert.ToString(drReport["vReportName"]) + "'");
                                }
                                else
                                {
                                    listHtml.Append("<p class='Alert_falseStatus' title='" + Convert.ToString(drReport["vReportName"]) + "' ");
                                }
                                listHtml.Append(" onclick='return ViewReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ",\"" + timeZoneID + "\",\"" + UserId + "\"," + 2 + ",\"" + Convert.ToString(drReport["vReportName"]) + "\"," + Convert.ToInt32(drReport["isCustom"]) + ");'>");
                                listHtml.Append(drReport["vReportName"]);
                                listHtml.Append("</p>");
                                listHtml.Append("</td>");










                                listHtml.Append("<td valign='top' >");



                                if (PemissionChecker("EDIT_REPORT"))
                                {






                                    listHtml.Append("<div class='PopupExtra' style='position: relative;float: right;margin-right: 15px;margin-top: 2px;'  onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportId"]) + ",\"report\");'>");

                                    //listHtml.Append("<img style='float: left; margin: 7px 0px 0 5px;cursor: pointer;' src='../images/arrow-b.png' onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportId"]) + ",\"report\");' />");
                                    listHtml.Append("<i class='icon-options'></i>");
                                    //listHtml.Append("</div>");
                                    listHtml.Append("<div id='divReportAction-" + Convert.ToInt32(drReport["ipkReportId"]) + "' class='ConfigureAlertNotificationPopup PopupExtra' style='display: none;'>");
                                    listHtml.Append("<div id='Div2' >");
                                    listHtml.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                                    listHtml.Append("<tr class='tdHoverReport'>");
                                    listHtml.Append("<td >");
                                    listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1056 1024'><path d='M815 960H79V160h450l65-62-2-2H79q-26 0-45 19t-19 45v800q0 27 19 45.5t45 18.5h736q27 0 45.5-18.5T879 960V492l-64 61v407zM990 41Q947 0 895 0q-57 0-102 45L354 483q-2 2-3.5 4t-2.5 4-1 4q-15 54-70 233-1 5-1 10t2 9.5 6 7.5q8 8 19 8 4 0 8-1 131-44 229-73 6-2 11-7 427-421 441-436 50-51 49-104-2-54-51-101zm-44 160q-27 28-414 410l-20 19q-20 6-62 19.5T359 679q36-118 47-158Q822 106 838 90q26-26 57-26 26 0 51 24 30 29 31 55 0 26-31 58z' fill='#626262'/></svg>");
                                    listHtml.Append("</td>");
                                    listHtml.Append("<td>");
                                    listHtml.Append("<p id='pid-" + Convert.ToInt32(drReport["ifkReportTypeId"]) + "' class='popu_operationn'  onclick='return EditReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ");' data-localize='rpt_EditReport'> Edit Report</p>");
                                    listHtml.Append("</td>");
                                    listHtml.Append("</tr>");



                                    if (PemissionChecker("EDIT_REPORT"))
                                    {

                                        listHtml.Append("<tr>");
                                        listHtml.Append("<td >");
                                        if (Convert.ToBoolean(drReport["bStatus"]))
                                        {
                                            listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M512 0Q373 0 255 68.5T68.5 255 0 512t68.5 257T255 955.5t257 68.5 257-68.5T955.5 769t68.5-257-68.5-257T769 68.5 512 0zm0 961q-73 0-141.5-22.5T247 874t-96.5-97-64-123.5T64 512q0-91 35.5-174T195 195t143-95.5T512 64t174 35.5T829 195t95.5 143T960 512t-35.5 174T829 829.5t-143 96T512 961zm181-630q-9-9-22.5-9t-22.5 9L512 467 376 331q-9-9-22.5-9t-22.5 9-9 22.5 9 22.5l136 136-136 136q-6 6-8.5 14t0 16.5T331 693q9 9 22.5 9t22.5-9l136-136 136 136q9 9 22.5 9t22.5-9q6-6 8.5-14.5t0-16.5-8.5-14L557 512l136-136q9-9 9-22.5t-9-22.5z' fill='#626262'/></svg>");
                                        }
                                        else
                                        {
                                            listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M512 0Q373 0 255 68.5T68.5 255 0 512t68.5 257T255 955.5t257 68.5 257-68.5T955.5 769t68.5-257-68.5-257T769 68.5 512 0zm0 961q-73 0-141.5-22.5T247 874t-96.5-97-64-123.5T64 512q0-91 35.5-174T195 195t143-95.5T512 64t174 35.5T829 195t95.5 143T960 512t-35.5 174T829 829.5t-143 96T512 961zm204-636L416 627 281 492q-10-10-23-10t-23 10q-2 2-4 5.5t-3 6.5-1.5 6.5 0 7 1.5 7 3 6.5l4 6 159 158q9 10 22.5 10t22.5-10l4-4 319-321q9-9 9-22.5t-9-22.5q-18-18-39-5-4 2-7 5z' fill='#626262'/></svg>");
                                        }

                                        listHtml.Append("</td>");



                                        listHtml.Append("<td>");
                                        if (Convert.ToBoolean(drReport["bStatus"]))
                                        {
                                            listHtml.Append("<p class='popu_operationn' onclick='return ReportOptions(" + Convert.ToInt32(drReport["ipkReportID"]) + ",\"status\"," + companyid + ",\"" + !Convert.ToBoolean(drReport["bStatus"]) + "\");' data-localize='rpt_SetInactive'> Set as Inactive</p>");
                                        }
                                        else
                                        {
                                            listHtml.Append("<p class='popu_operationn' onclick='return ReportOptions(" + Convert.ToInt32(drReport["ipkReportID"]) + ",\"status\"," + companyid + ",\"" + !Convert.ToBoolean(drReport["bStatus"]) + "\");' data-localize='rpt_SetActive'> Set as Active</p>");
                                        }
                                        listHtml.Append("</td>");


                                        listHtml.Append("</tr>");
                                    }


                                    if (PemissionChecker("DELETE_REPORT")){ 
                                    listHtml.Append("<tr>");
                                    listHtml.Append("<td >");
                                    listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 832 1056'><path d='M801 175H576V88q0-20-10-37t-27-26.5-37-9.5H330q-30 0-52 21t-22 52v87H31q-13 0-22.5 9.5T-1 207t9.5 22.5T31 239h44l74 740q3 26 22.5 44t45.5 18h398q26 0 45.5-18t21.5-44l75-740h44q8 0 15.5-4.5t12-11.5 4.5-16q0-13-9.5-22.5T801 175zM320 88q0-10 10-10h172q10 0 10 10v87H320V88zm299 885q-1 4-4 4H217q-3 0-4-4l-73-734h552z' fill='#626262'/></svg>");
                                    listHtml.Append("</td>");

                                    listHtml.Append("<td>");
                                    listHtml.Append("<p class='popu_operationn' onclick='return ReportOptions(" + Convert.ToInt32(drReport["ipkReportID"]) + ",\"delete\"," + companyid + ",\"\");' data-localize='rpt_DeleteReport'> Delete Report</p>");
                                    listHtml.Append("</td>");

                                    listHtml.Append("</tr>");
                                }

                                    if (PemissionChecker("DUPLICATE_REPORTS"))
                                    {

                                        listHtml.Append("<tr>");

                                        listHtml.Append("<td >");
                                        listHtml.Append(" <svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M768 0H416q-27 0-45.5 18.5T352 64h352v256h256v512H736v64h224q27 0 45.5-18.5T1024 832V256zm0 256V90l165 166H768zM64 128q-27 0-45.5 18.5T0 192v768q0 27 18.5 45.5T64 1024h544q27 0 45.5-18.5T672 960V384L416 128H64zm544 832H64V192h288v256h256v512zM416 384V218l165 166H416z' fill='#626262'/></svg>");
                                        listHtml.Append("</td>");

                                        listHtml.Append("<td>");
                                        listHtml.Append("<p class='popu_operationn' onclick='return DuplicateReport(" + Convert.ToInt32(drReport["ipkReportID"]) + ");' data-localize='rpt_Duplicate'> Duplicate Report</p>");
                                        listHtml.Append("</td>");

                                        listHtml.Append("</tr>");
                                    }

                                    if (PemissionChecker("SCHEDULE_REPORT"))
                                    {
                                        if (!Convert.ToBoolean(drReport["IsScheduleReport"]))
                                        {
                                            listHtml.Append("<tr>");

                                            listHtml.Append("<td >");
                                            listHtml.Append(" <svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M768 0H416q-27 0-45.5 18.5T352 64h352v256h256v512H736v64h224q27 0 45.5-18.5T1024 832V256zm0 256V90l165 166H768zM64 128q-27 0-45.5 18.5T0 192v768q0 27 18.5 45.5T64 1024h544q27 0 45.5-18.5T672 960V384L416 128H64zm544 832H64V192h288v256h256v512zM416 384V218l165 166H416z' fill='#626262'/></svg>");
                                            listHtml.Append("</td>");

                                            listHtml.Append("<td>");
                                            listHtml.Append("<p class='popu_operationn' onclick='return DuplicateReport(" + Convert.ToInt32(drReport["ipkReportID"]) + ");' data-localize='rpt_Schedule'> Schedule Report</p>");
                                            listHtml.Append("</td>");

                                            listHtml.Append("</tr>");
                                        }
                                    }

                                    listHtml.Append("<tr>");

                                    listHtml.Append("<td colspan='2' style='display:none' >");
                                    listHtml.Append("<img alt='' src='../Images/line.png'  style='float: left; vertical-align: middle;margin: 4px;' />");
                                    listHtml.Append("</td>");

                                    listHtml.Append("</tr>");

                                    listHtml.Append("<tr>");

                                    listHtml.Append("<td>");
                                    listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M512 0Q373 0 255 68.5T68.5 255 0 512t68.5 257T255 955.5t257 68.5 257-68.5T955.5 769t68.5-257-68.5-257T769 68.5 512 0zm0 961q-73 0-141.5-22.5T247 874t-96.5-97-64-123.5T64 512q0-91 35.5-174T195 195t143-95.5T512 64t174 35.5T829 195t95.5 143T960 512t-35.5 174T829 829.5t-143 96T512 961zm204-636L416 627 281 492q-10-10-23-10t-23 10q-2 2-4 5.5t-3 6.5-1.5 6.5 0 7 1.5 7 3 6.5l4 6 159 158q9 10 22.5 10t22.5-10l4-4 319-321q9-9 9-22.5t-9-22.5q-18-18-39-5-4 2-7 5z' fill='#626262'/></svg>");
                                    listHtml.Append("</td>");

                                    listHtml.Append("<td>");
                                    listHtml.Append("<p class='popu_operationn' style='cursor: pointer; text-transform:uppercase;' ");
                                    listHtml.Append(" onclick='return ViewReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ",\"" + timeZoneID + "\",\"" + UserId + "\"," + 2 + ",\"" + Convert.ToString(drReport["vReportName"]) + "\"," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_ViewReport'>View Report</p>");
                                    listHtml.Append("</td>");

                                    listHtml.Append("</tr>");
                                    listHtml.Append("</table>");



                                    listHtml.Append("</div>");
                                    listHtml.Append("</div>");
                                }



                                listHtml.Append("</td>");
                                listHtml.Append("</tr>");
                                listHtml.Append("<tr>");
                                listHtml.Append("<td colspan='2'>");
                                listHtml.Append("<div class='teb'>");
                                listHtml.Append(Convert.ToString(drReport["vDescription"]));
                                listHtml.Append("</div>");
                                listHtml.Append("</td>");


                                listHtml.Append("</tr>");
                                listHtml.Append("</table>");

                            



                                listHtml.Append("</td>");
                                listHtml.Append("</tr>");
                                listHtml.Append("</table>");
                                listHtml.Append("</div>");
                            }
                        }

                        #endregion
                        break;
                    case "4": // Report History
                        #region Report Reports
                        listHtml.Append("<div class='right-hedding'>Report History</div>");
                        #endregion
                        break;

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsReport.cs", "PrepareReportlist()", ex.Message);
                listHtml.Append("500 Internal Error");
            }

            return listHtml.ToString();
        }

        public string GetReports(clsRegistration objclsRegistration)
        {
            StringBuilder listHtml = new StringBuilder();
            try
            {
                DataSet dsReports;

                
                string timeZoneID = objclsRegistration.vTimeZoneID;
                string UserId = Convert.ToString(objclsRegistration.pkUserID);
                ifkUserID = Convert.ToInt32(UserId);
                string userType = Convert.ToString(objclsRegistration.ifkUserTypeID);
                switch (report_type_id.ToString())
                {
                    case "1":  // static Reports
                        #region Reports

                        dsReports = GetReportList();

                        if (dsReports != null && dsReports.Tables.Count > 0 && dsReports.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drReport in dsReports.Tables[0].Rows)
                            {
                                var reportDisplayType = Convert.ToInt32(drReport["reportDisplayType"]);


                                var diplayClassName = reportDisplayType == 4 ? "json-display" : "normal-display";

                                if (userType == "3")
                                {



                                    {
                                        if (Convert.ToBoolean(drReport["bStatus"]))
                                        {
                                            //if (Convert.ToString(drReport["vReportTypeName"]) != null && Convert.ToString(drReport["vReportTypeName"]) != "")
                                            //{

                                            listHtml.Append("<img id='imgbtnstaticreport' alt='loading...' src='../images/Loading.gif' class='imgload_alert_TripReplay' style='display: none; left: -15px;' />");
                                            listHtml.Append("<div  class='position-notification1  " + diplayClassName + "' id='divReportNotification-" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "' >");
                                            listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0' align='left'>");

                                            listHtml.Append("<tr>");
                                            listHtml.Append("<td width='60' valign='top' align='left'>");

                                            if (Convert.ToBoolean(drReport["bStatus"]))
                                            {
                                                if (Convert.ToInt32(drReport["reportDisplayType"]) == 1)
                                                {
                                                    listHtml.Append("<span align='top' class='r_RowIcon reports-icon35' style='width:35px;'></span>");
                                                }
                                                else
                                                {
                                                    listHtml.Append("<span align='top' class='r_RowIcon graph-con-fin4-35' style='width:35px;'></span>");
                                                }
                                            }
                                            else
                                            {
                                                listHtml.Append("<img align='top' class='r_RowIcon' src='../images/reports_Icon_Disabled.png' style='width:35px;'>");
                                            }

                                            listHtml.Append("</td>");
                                            listHtml.Append("<td>");
                                            listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0'>");

                                            listHtml.Append("<tr>");
                                            listHtml.Append("<td width='555'>");

                                            if (Convert.ToBoolean(drReport["bStatus"]))
                                            {
                                                listHtml.Append("<p class='position-overspeeding'  title='" + Convert.ToString(drReport["vReportTypeName"]) + "'");
                                            }
                                            else
                                            {

                                                listHtml.Append("<p class='Alert_falseStatus' title='" + Convert.ToString(drReport["vReportTypeName"]) + "' ");
                                            }

                                            listHtml.Append(" onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\"," + Convert.ToInt32(drReport["reportDisplayType"]) + "," + Convert.ToInt32(drReport["isAnalog"]) + "," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_" + Convert.ToString(drReport["vReportTypeName"]) + "'>");
                                            listHtml.Append(drReport["vReportTypeName"]);
                                            listHtml.Append("</p>");

                                            listHtml.Append("<td valign='top'>");

                                            listHtml.Append("<div class='PopupExtra'  style='position: relative;float: right;margin-right: 15px;margin-top: 2px;' onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"report\");'>");

                                            ;

                                            listHtml.Append("<i class='icon-options'></i>");
                                            //listHtml.Append("</div>");

                                            listHtml.Append("<div id='divReportAction-" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "' class='ConfigureAlertNotificationPopup PopupExtra' style='display: none;'>");
                                            listHtml.Append("<div id='Div2' >");
                                            listHtml.Append("<table border='0' cellpadding='0' cellspacing='0' >");

                                            listHtml.Append("<tr class='tdHoverReport'>");

                                            listHtml.Append("<td >");
                                            listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='20px' height='15px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 736 1024'><path d='M416 0H64Q37 0 18.5 18.5T0 64v896q0 27 18.5 45.5T64 1024h608q27 0 45.5-18.5T736 960V320zm256 347v5H384V64h6zM64 960V64h256v352h352v544H64z' fill='#626262'/></svg>");
                                            listHtml.Append("</td>");

                                            listHtml.Append("<td>");
                                            listHtml.Append("<p class='popu_operationn'  onclick='return ScheduledReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "," + 1 + ");' data-localize='rpt_Schedule'> Schedule this</p>");
                                            listHtml.Append("</td>");

                                            listHtml.Append("</tr>");

                                            listHtml.Append("<tr class='tdHoverReport'>");

                                            listHtml.Append("<td colspan='2' style='display:none' >");
                                            listHtml.Append("<img alt='' src='../Images/line.png'  style='float: left; vertical-align: middle;margin: 4px;' />");
                                            listHtml.Append("</td>");

                                            listHtml.Append("</tr>");

                                            listHtml.Append("<tr class='tdHoverReport'>");

                                            listHtml.Append("<td>");
                                            listHtml.Append("<img alt='' src='../Images/right-icon.png' style='margin-right:5px; vertical-align: middle' />");
                                            listHtml.Append("</td>");

                                            listHtml.Append("<td>");
                                            listHtml.Append("<p class = 'popu_operationn' style='cursor: pointer; text-transform:uppercase;' ");
                                            listHtml.Append(" onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\"," + Convert.ToInt32(drReport["reportDisplayType"]) + "," + Convert.ToInt32(drReport["isAnalog"]) + "," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_ShowResults'> Show Results</p>");
                                            //  listHtml.Append(" onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\");' data-localize='rpt_ShowResults'> Show Results</p>");
                                            listHtml.Append("</td>");

                                            listHtml.Append("</tr>");


                                            listHtml.Append("<tr>");
                                            listHtml.Append("<td>");
                                            listHtml.Append("<i  class='fa fa-trash sub-remove' aria-hidden='true'></i>");
                                            listHtml.Append("</td>");
                                            listHtml.Append("<td>");
                                            listHtml.Append("<p class='popu_operationn' style = 'cursor: pointer; text-transform:uppercase;' onClick ='return UnpinReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ")'> Exclude </p>");
                                            listHtml.Append("</td>");
                                            listHtml.Append("</tr>");
                                            //if (Convert.ToInt32(drReport["reportDisplayType"]) == 1)
                                            //{
                                            //    listHtml.Append("<tr>");
                                            //    listHtml.Append("<td>");
                                            //    listHtml.Append("<i class='fa fa-pencil-square-o' aria-hidden='true'></i>'");
                                            //    listHtml.Append("</td>");
                                            //    listHtml.Append("<td>");
                                            //    listHtml.Append("<p style = 'color: #235C9F;cursor: pointer;' onClick ='return CustomizeReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\")'> Customize </p>");
                                            //    listHtml.Append("</td>");
                                            //    listHtml.Append("</tr>");
                                            //}


                                            listHtml.Append("</table>");

                                            listHtml.Append("</div>");
                                            listHtml.Append("</div>");
                                            //}
                                        }

                                        listHtml.Append("</td>");
                                        listHtml.Append("</tr>");
                                        listHtml.Append("<tr>");
                                        listHtml.Append("<td colspan='2'>");
                                        listHtml.Append("<div class='teb' data-localize='rpt_" + Convert.ToString(drReport["vDescription"]) + "'>");
                                        listHtml.Append(Convert.ToString(drReport["vDescription"]));
                                        listHtml.Append("</div>");
                                        listHtml.Append("</td>");


                                        listHtml.Append("</tr>");
                                        listHtml.Append("</table>");

                                        listHtml.Append("</td>");

                                        listHtml.Append("</tr>");



                                        listHtml.Append("</table>");
                                        listHtml.Append("</div>");
                                    }
                                }
                                else
                                {
                                    if (Convert.ToBoolean(drReport["bStatus"]))
                                    {
                                        //if (Convert.ToString(drReport["vReportTypeName"]) != null && Convert.ToString(drReport["vReportTypeName"]) != "")
                                        //{

                                        listHtml.Append("<img id='imgbtnstaticreport' alt='loading...' src='../images/Loading.gif' class='imgload_alert_TripReplay' style='display: none; left: -15px;' />");
                                        listHtml.Append("<div class='position-notification1 " + diplayClassName + "' id='divReportNotification-" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "' >");
                                        listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0' align='left'>");

                                        listHtml.Append("<tr>");
                                        listHtml.Append("<td width='60' valign='top' align='left'>");

                                        if (Convert.ToBoolean(drReport["bStatus"]))
                                        {
                                            if (Convert.ToInt32(drReport["reportDisplayType"]) == 1)
                                            {
                                                listHtml.Append("<span align='top' class='r_RowIcon reports-icon35' style='width:35px;'></span>");
                                            }
                                            else
                                            {
                                                listHtml.Append("<span align='top' class='r_RowIcon graph-con-fin4-35' style='width:35px;'></span>");
                                            }
                                        }
                                        else
                                        {
                                            listHtml.Append("<img align='top' class='r_RowIcon' src='../images/reports_Icon_Disabled.png' style='width:35px;'>");
                                        }

                                        listHtml.Append("</td>");
                                        listHtml.Append("<td>");
                                        listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0'>");

                                        listHtml.Append("<tr>");
                                        listHtml.Append("<td width='555'>");

                                        if (Convert.ToBoolean(drReport["bStatus"]))
                                        {
                                            listHtml.Append("<p class='position-overspeeding'  title='" + Convert.ToString(drReport["vReportTypeName"]) + "'");
                                        }
                                        else
                                        {
                                            listHtml.Append("<p class='Alert_falseStatus' title='" + Convert.ToString(drReport["vReportTypeName"]) + "' ");
                                        }
                                        listHtml.Append("  onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\"," + Convert.ToInt32(drReport["reportDisplayType"]) + "," + Convert.ToInt32(drReport["isAnalog"]) + "," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_" + Convert.ToString(drReport["vReportTypeName"]) + "'>");
                                        // listHtml.Append(" onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\");' data-localize='rpt_" + Convert.ToString(drReport["vReportTypeName"]) + "'>");
                                        listHtml.Append(drReport["vReportTypeName"]);
                                        listHtml.Append("</p>");

                                        listHtml.Append("<td valign='top'>");

                                        listHtml.Append("<div class='PopupExtra'  style='position: relative;float: right;margin-right: 15px;margin-top: 2px;' onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"report\");'>");



                                        listHtml.Append("<i class='icon-options'></i>");
                                        //listHtml.Append("</div>");

                                        listHtml.Append("<div id='divReportAction-" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "' class='ConfigureAlertNotificationPopup PopupExtra' style='display: none;'>");
                                        listHtml.Append("<div id='Div2' >");
                                        listHtml.Append("<table border='0' cellpadding='0' cellspacing='0' >");

                                        listHtml.Append("<tr class='tdHoverReport'>");

                                        listHtml.Append("<td >");
                                        listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 736 1024'><path d='M416 0H64Q37 0 18.5 18.5T0 64v896q0 27 18.5 45.5T64 1024h608q27 0 45.5-18.5T736 960V320zm256 347v5H384V64h6zM64 960V64h256v352h352v544H64z' fill='#626262'/></svg>");
                                        listHtml.Append("</td>");

                                        listHtml.Append("<td>");
                                        listHtml.Append("<p class='popu_operationn'  onclick='return ScheduledReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + "," + 1 + ");' data-localize='rpt_Schedule'> Schedule this</p>");
                                        listHtml.Append("</td>");

                                        listHtml.Append("</tr>");

                                        listHtml.Append("<tr class='tdHoverReport'>");

                                        listHtml.Append("<td colspan='2' style='display:none'>");
                                        listHtml.Append("<img alt='' src='../Images/line.png'  style='float: left; vertical-align: middle;margin: 4px;' />");
                                        listHtml.Append("</td>");

                                        listHtml.Append("</tr>");

                                        listHtml.Append("<tr class='tdHoverReport'>");

                                        listHtml.Append("<td>");
                                        listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M512 0Q373 0 255 68.5T68.5 255 0 512t68.5 257T255 955.5t257 68.5 257-68.5T955.5 769t68.5-257-68.5-257T769 68.5 512 0zm0 961q-73 0-141.5-22.5T247 874t-96.5-97-64-123.5T64 512q0-91 35.5-174T195 195t143-95.5T512 64t174 35.5T829 195t95.5 143T960 512t-35.5 174T829 829.5t-143 96T512 961zm204-636L416 627 281 492q-10-10-23-10t-23 10q-2 2-4 5.5t-3 6.5-1.5 6.5 0 7 1.5 7 3 6.5l4 6 159 158q9 10 22.5 10t22.5-10l4-4 319-321q9-9 9-22.5t-9-22.5q-18-18-39-5-4 2-7 5z' fill='#626262'/></svg>");
                                        listHtml.Append("</td>");

                                        listHtml.Append("<td>");

                                        listHtml.Append("<p class = 'popu_operationn' style= 'cursor: pointer; text-transform:uppercase;' ");
                                        listHtml.Append("  onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\"," + Convert.ToInt32(drReport["reportDisplayType"]) + "," + Convert.ToInt32(drReport["isAnalog"]) + "," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_ShowResults'> Show Results</p>");
                                        // listHtml.Append(" onclick='return ViewStaticReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + timeZoneID + "\"," + UserId + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\");' data-localize='rpt_ShowResults'> Show Results</p>");
                                        listHtml.Append("</td>");

                                        listHtml.Append("</tr>");


                                        listHtml.Append("<tr class='tdHoverReport'>");
                                        listHtml.Append("<td>");
                                        listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 832 1056'><path d='M801 175H576V88q0-20-10-37t-27-26.5-37-9.5H330q-30 0-52 21t-22 52v87H31q-13 0-22.5 9.5T-1 207t9.5 22.5T31 239h44l74 740q3 26 22.5 44t45.5 18h398q26 0 45.5-18t21.5-44l75-740h44q8 0 15.5-4.5t12-11.5 4.5-16q0-13-9.5-22.5T801 175zM320 88q0-10 10-10h172q10 0 10 10v87H320V88zm299 885q-1 4-4 4H217q-3 0-4-4l-73-734h552z' fill='#626262'/></svg>");
                                        listHtml.Append("</td>");
                                        listHtml.Append("<td>");
                                        listHtml.Append("<p  class = 'popu_operationn' style = 'cursor: pointer; text-transform:uppercase; ' onClick ='return UnpinReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ")'> Exclude </p>");
                                        listHtml.Append("</td>");
                                        listHtml.Append("</tr>");

                                        //if (Convert.ToInt32(drReport["reportDisplayType"]) == 1)
                                        //{
                                        //    listHtml.Append("<tr>");
                                        //    listHtml.Append("<td>");
                                        //    listHtml.Append("<i class='fa fa-pencil-square-o' aria-hidden='true'></i>'");
                                        //    listHtml.Append("</td>");
                                        //    listHtml.Append("<td>");
                                        //    listHtml.Append("<p style = 'color: #235C9F;cursor: pointer;' onClick ='return CustomizeReport(" + Convert.ToInt32(drReport["ipkReportTypeId"]) + ",\"" + Convert.ToString(drReport["vReportTypeName"]) + "\")'> Customize </p>");
                                        //    listHtml.Append("</td>");
                                        //    listHtml.Append("</tr>");
                                        //}

                                        listHtml.Append("</table>");

                                        listHtml.Append("</div>");
                                        listHtml.Append("</div>");
                                        //}
                                    }

                                    listHtml.Append("</td>");
                                    listHtml.Append("</tr>");
                                    listHtml.Append("<tr>");
                                    listHtml.Append("<td colspan='2'>");
                                    listHtml.Append("<div class='teb' data-localize='rpt_" + Convert.ToString(drReport["vDescription"]) + "'>");
                                    listHtml.Append(Convert.ToString(drReport["vDescription"]));
                                    listHtml.Append("</div>");
                                    listHtml.Append("</td>");


                                    listHtml.Append("</tr>");
                                    listHtml.Append("</table>");

                                    listHtml.Append("</td>");

                                    listHtml.Append("</tr>");



                                    listHtml.Append("</table>");
                                    listHtml.Append("</div>");
                                }



                            }
                        }

                        #endregion
                        break;
                    case "2": // Custom Reports                       
                        #region Custom Reports

                        dsReports = GetReportList();

                        if (dsReports != null && dsReports.Tables.Count > 0 && dsReports.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow drReport in dsReports.Tables[0].Rows)
                            {

                                var _reportTypeClass = Convert.ToInt32(drReport["reportDisplayType"]) == 4 ? "webhook-report" : "normal-report";


                                listHtml.Append("<img id='imgbtnstaticreport' alt='loading...' src='../images/Loading.gif' class='imgload_alert_TripReplay' style='display: none; left: -15px;' />");
                                listHtml.Append("<div data-display-type ='" + Convert.ToInt32(drReport["reportDisplayType"]) + "' class='position-notification1  " + _reportTypeClass + " ' id='divReportNotification-" + Convert.ToInt32(drReport["ipkReportId"]) + "' >");
                                listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0' align='left'>");
                                listHtml.Append("<tr>");

                                listHtml.Append("<td width='60' valign='top' align='left'>");
                                if (Convert.ToBoolean(drReport["IsScheduleReport"]))
                                {
                                    if (Convert.ToBoolean(drReport["bStatus"]))
                                    {
                                        listHtml.Append("<span align='top' class='r_RowIcon reports-icon-scheduled35' style='width:35px;'></span>");
                                    }
                                    else
                                    {
                                        listHtml.Append("<span align='top' class='r_RowIcon reports-icon-scheduled-disabled35' style='width:35px;'></span>");
                                    }
                                }
                                else
                                {
                                    if (Convert.ToBoolean(drReport["bStatus"]))
                                    {
                                        listHtml.Append("<span align='top' class='r_RowIcon reports-icon35' style='width:35px;'></span>");
                                    }
                                    else
                                    {
                                        listHtml.Append("<img align='top' class='r_RowIcon' src='../images/reports_Icon_Disabled.png' style='width:35px;'>");
                                    }
                                }
                                listHtml.Append("</td>");

                                listHtml.Append("<td>");
                                listHtml.Append("<table width='100%' cellspacing='0' cellpadding='0'>");
                                listHtml.Append("<tr>");
                                listHtml.Append("<td width='555'>");
                                if (Convert.ToBoolean(drReport["bStatus"]))
                                {
                                    listHtml.Append("<p class='position-overspeeding'  title='" + Convert.ToString(drReport["vReportName"]) + "'");
                                }
                                else
                                {
                                    listHtml.Append("<p class='Alert_falseStatus' title='" + Convert.ToString(drReport["vReportName"]) + "' ");
                                }
                                listHtml.Append(" onclick='return ViewReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ",\"" + timeZoneID + "\",\"" + UserId + "\"," + 2 + ",\"" + Convert.ToString(drReport["vReportName"]) + "\"," + Convert.ToInt32(drReport["isCustom"]) + ");'>");
                                listHtml.Append(drReport["vReportName"]);
                                listHtml.Append("</p>");
                                listHtml.Append("</td>");


                                listHtml.Append("<td valign='top' >");

                                //if (Convert.ToBoolean(drReport["bStatus"]))
                                //{
                                //    listHtml.Append("<img class='img_status' src='../images/online-icon.png'>");
                                //}
                                //else
                                //{
                                //    listHtml.Append("<img class='img_status' src='../images/offline-icon.png'>");
                                //}
                                listHtml.Append("<div class='PopupExtra' style='position: relative;float: right;margin-right: 15px;margin-top: 2px;'  onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportId"]) + ",\"report\");'>");

                                //listHtml.Append("<img style='float: left; margin: 7px 0px 0 5px;cursor: pointer;' src='../images/arrow-b.png' onclick='return OpenActionPopup(" + Convert.ToInt32(drReport["ipkReportId"]) + ",\"report\");' />");
                                listHtml.Append("<i class='icon-options'></i>");
                                //listHtml.Append("</div>");
                                listHtml.Append("<div id='divReportAction-" + Convert.ToInt32(drReport["ipkReportId"]) + "' class='ConfigureAlertNotificationPopup PopupExtra' style='display: none;'>");
                                listHtml.Append("<div id='Div2' >");
                                listHtml.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                                listHtml.Append("<tr class='tdHoverReport'>");
                                listHtml.Append("<td >");
                                listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1056 1024'><path d='M815 960H79V160h450l65-62-2-2H79q-26 0-45 19t-19 45v800q0 27 19 45.5t45 18.5h736q27 0 45.5-18.5T879 960V492l-64 61v407zM990 41Q947 0 895 0q-57 0-102 45L354 483q-2 2-3.5 4t-2.5 4-1 4q-15 54-70 233-1 5-1 10t2 9.5 6 7.5q8 8 19 8 4 0 8-1 131-44 229-73 6-2 11-7 427-421 441-436 50-51 49-104-2-54-51-101zm-44 160q-27 28-414 410l-20 19q-20 6-62 19.5T359 679q36-118 47-158Q822 106 838 90q26-26 57-26 26 0 51 24 30 29 31 55 0 26-31 58z' fill='#626262'/></svg>");
                                listHtml.Append("</td>");
                                listHtml.Append("<td>");
                                listHtml.Append("<p id='pid-" + Convert.ToInt32(drReport["ifkReportTypeId"]) + "' class='popu_operationn'  onclick='return EditReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ");' data-localize='rpt_EditReport'> Edit Report</p>");
                                listHtml.Append("</td>");
                                listHtml.Append("</tr>");
                                listHtml.Append("<tr>");
                                listHtml.Append("<td >");

                                if (Convert.ToBoolean(drReport["bStatus"]))
                                {
                                    listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M512 0Q373 0 255 68.5T68.5 255 0 512t68.5 257T255 955.5t257 68.5 257-68.5T955.5 769t68.5-257-68.5-257T769 68.5 512 0zm0 961q-73 0-141.5-22.5T247 874t-96.5-97-64-123.5T64 512q0-91 35.5-174T195 195t143-95.5T512 64t174 35.5T829 195t95.5 143T960 512t-35.5 174T829 829.5t-143 96T512 961zm181-630q-9-9-22.5-9t-22.5 9L512 467 376 331q-9-9-22.5-9t-22.5 9-9 22.5 9 22.5l136 136-136 136q-6 6-8.5 14t0 16.5T331 693q9 9 22.5 9t22.5-9l136-136 136 136q9 9 22.5 9t22.5-9q6-6 8.5-14.5t0-16.5-8.5-14L557 512l136-136q9-9 9-22.5t-9-22.5z' fill='#626262'/></svg>");
                                }
                                else
                                {
                                    listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M512 0Q373 0 255 68.5T68.5 255 0 512t68.5 257T255 955.5t257 68.5 257-68.5T955.5 769t68.5-257-68.5-257T769 68.5 512 0zm0 961q-73 0-141.5-22.5T247 874t-96.5-97-64-123.5T64 512q0-91 35.5-174T195 195t143-95.5T512 64t174 35.5T829 195t95.5 143T960 512t-35.5 174T829 829.5t-143 96T512 961zm204-636L416 627 281 492q-10-10-23-10t-23 10q-2 2-4 5.5t-3 6.5-1.5 6.5 0 7 1.5 7 3 6.5l4 6 159 158q9 10 22.5 10t22.5-10l4-4 319-321q9-9 9-22.5t-9-22.5q-18-18-39-5-4 2-7 5z' fill='#626262'/></svg>");
                                }

                                listHtml.Append("</td>");

                                listHtml.Append("<td>");
                                if (Convert.ToBoolean(drReport["bStatus"]))
                                {
                                    listHtml.Append("<p class='popu_operationn' onclick='return ReportOptions(" + Convert.ToInt32(drReport["ipkReportID"]) + ",\"status\"," + companyid + ",\"" + !Convert.ToBoolean(drReport["bStatus"]) + "\");' data-localize='rpt_SetInactive'> Set as Inactive</p>");
                                }
                                else
                                {
                                    listHtml.Append("<p class='popu_operationn' onclick='return ReportOptions(" + Convert.ToInt32(drReport["ipkReportID"]) + ",\"status\"," + companyid + ",\"" + !Convert.ToBoolean(drReport["bStatus"]) + "\");' data-localize='rpt_SetActive'> Set as Active</p>");
                                }
                                listHtml.Append("</td>");

                                listHtml.Append("</tr>");

                                listHtml.Append("<tr>");

                                listHtml.Append("<td >");
                                listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 832 1056'><path d='M801 175H576V88q0-20-10-37t-27-26.5-37-9.5H330q-30 0-52 21t-22 52v87H31q-13 0-22.5 9.5T-1 207t9.5 22.5T31 239h44l74 740q3 26 22.5 44t45.5 18h398q26 0 45.5-18t21.5-44l75-740h44q8 0 15.5-4.5t12-11.5 4.5-16q0-13-9.5-22.5T801 175zM320 88q0-10 10-10h172q10 0 10 10v87H320V88zm299 885q-1 4-4 4H217q-3 0-4-4l-73-734h552z' fill='#626262'/></svg>");
                                listHtml.Append("</td>");

                                listHtml.Append("<td>");
                                listHtml.Append("<p class='popu_operationn' onclick='return ReportOptions(" + Convert.ToInt32(drReport["ipkReportID"]) + ",\"delete\"," + companyid + ",\"\");' data-localize='rpt_DeleteReport'> Delete Report</p>");
                                listHtml.Append("</td>");

                                listHtml.Append("</tr>");

                                listHtml.Append("<tr>");

                                listHtml.Append("<td >");
                                listHtml.Append(" <svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M768 0H416q-27 0-45.5 18.5T352 64h352v256h256v512H736v64h224q27 0 45.5-18.5T1024 832V256zm0 256V90l165 166H768zM64 128q-27 0-45.5 18.5T0 192v768q0 27 18.5 45.5T64 1024h544q27 0 45.5-18.5T672 960V384L416 128H64zm544 832H64V192h288v256h256v512zM416 384V218l165 166H416z' fill='#626262'/></svg>");
                                listHtml.Append("</td>");

                                listHtml.Append("<td>");
                                listHtml.Append("<p class='popu_operationn' onclick='return DuplicateReport(" + Convert.ToInt32(drReport["ipkReportID"]) + ");' data-localize='rpt_Duplicate'> Duplicate Report</p>");
                                listHtml.Append("</td>");

                                listHtml.Append("</tr>");

                                /**/
                                if (!Convert.ToBoolean(drReport["IsScheduleReport"]))
                                {
                                    listHtml.Append("<tr>");

                                    listHtml.Append("<td >");
                                    listHtml.Append(" <svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M768 0H416q-27 0-45.5 18.5T352 64h352v256h256v512H736v64h224q27 0 45.5-18.5T1024 832V256zm0 256V90l165 166H768zM64 128q-27 0-45.5 18.5T0 192v768q0 27 18.5 45.5T64 1024h544q27 0 45.5-18.5T672 960V384L416 128H64zm544 832H64V192h288v256h256v512zM416 384V218l165 166H416z' fill='#626262'/></svg>");
                                    listHtml.Append("</td>");

                                    listHtml.Append("<td>");
                                    listHtml.Append("<p class='popu_operationn' onclick='return DuplicateReport(" + Convert.ToInt32(drReport["ipkReportID"]) + ");' data-localize='rpt_Schedule'> Schedule Report</p>");
                                    listHtml.Append("</td>");

                                    listHtml.Append("</tr>");
                                }
                                /**/

                                listHtml.Append("<tr>");

                                listHtml.Append("<td colspan='2' style='display:none' >");
                                listHtml.Append("<img alt='' src='../Images/line.png'  style='float: left; vertical-align: middle;margin: 4px;' />");
                                listHtml.Append("</td>");

                                listHtml.Append("</tr>");

                                listHtml.Append("<tr>");

                                listHtml.Append("<td>");
                                listHtml.Append("<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='21px' height='21px' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M512 0Q373 0 255 68.5T68.5 255 0 512t68.5 257T255 955.5t257 68.5 257-68.5T955.5 769t68.5-257-68.5-257T769 68.5 512 0zm0 961q-73 0-141.5-22.5T247 874t-96.5-97-64-123.5T64 512q0-91 35.5-174T195 195t143-95.5T512 64t174 35.5T829 195t95.5 143T960 512t-35.5 174T829 829.5t-143 96T512 961zm204-636L416 627 281 492q-10-10-23-10t-23 10q-2 2-4 5.5t-3 6.5-1.5 6.5 0 7 1.5 7 3 6.5l4 6 159 158q9 10 22.5 10t22.5-10l4-4 319-321q9-9 9-22.5t-9-22.5q-18-18-39-5-4 2-7 5z' fill='#626262'/></svg>");
                                listHtml.Append("</td>");

                                listHtml.Append("<td>");
                                listHtml.Append("<p class='popu_operationn' style='cursor: pointer; text-transform:uppercase;' ");
                                listHtml.Append(" onclick='return ViewReport(" + Convert.ToInt32(drReport["ipkReportID"]) + "," + Convert.ToInt32(drReport["ifkReportTypeId"]) + ",\"" + timeZoneID + "\",\"" + UserId + "\"," + 2 + ",\"" + Convert.ToString(drReport["vReportName"]) + "\"," + Convert.ToInt32(drReport["isCustom"]) + ");' data-localize='rpt_ViewReport'>View Report</p>");
                                listHtml.Append("</td>");

                                listHtml.Append("</tr>");
                                listHtml.Append("</table>");
                                listHtml.Append("</div>");
                                listHtml.Append("</div>");

                                listHtml.Append("</td>");
                                listHtml.Append("</tr>");

                                listHtml.Append("<tr>");
                                listHtml.Append("<td colspan='2'>");
                                listHtml.Append("<div class='teb'>");
                                listHtml.Append(Convert.ToString(drReport["vDescription"]));
                                listHtml.Append("</div>");
                                listHtml.Append("</td>");


                                listHtml.Append("</tr>");
                                listHtml.Append("</table>");
                                listHtml.Append("</td>");
                                listHtml.Append("</tr>");
                                listHtml.Append("</table>");
                                listHtml.Append("</div>");
                            }
                        }

                        #endregion
                        break;
                    case "4": // Report History
                        #region Report Reports
                        listHtml.Append("<div class='right-hedding'>Report History</div>");
                        #endregion
                        break;

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "PrepareReportlist()", ex.Message  + ex.StackTrace);
                listHtml.Append("500 Internal Error");
            }

            return listHtml.ToString();
        }

        public string SaveCustomReports_(clsRegistration objclsRegistration )
        {
           
            string timeZoneID = objclsRegistration.vTimeZoneID;
            int ifkMeasurementUnit = objclsRegistration.ifkMeasurementUnit;

            string result = "";
            try
            {
                //here
                SqlParameter[] param = new SqlParameter[73];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[54] = new SqlParameter("@vAssetList_CSV", SqlDbType.NChar);
                param[54].Value = vAssetList_CSV;

                param[55] = new SqlParameter("@Frequency", SqlDbType.NChar);
                param[55].Value = Frequency;

                param[56] = new SqlParameter("@ParamZoneID_Csv", SqlDbType.NChar);
                param[56].Value = ParamZoneID_Csv;

                param[57] = new SqlParameter("@isAnyZone", SqlDbType.Bit);
                param[57].Value = isAnyZone;

                param[58] = new SqlParameter("@vClientCSV", SqlDbType.NChar);
                param[58].Value = vClient_CSV;

                param[59] = new SqlParameter("@iAnyClient", SqlDbType.Int);
                param[59].Value = iAnyClient;

                param[60] = new SqlParameter("@iDigitalInputMapping", SqlDbType.Int);
                param[60].Value = iDigitalInputMapping;

                param[61] = new SqlParameter("@isShow", SqlDbType.Int);
                param[61].Value = isShow;

                param[62] = new SqlParameter("@iTripType", SqlDbType.Int);
                param[62].Value = iTripType;



                if (ReportTypeId == 54)

                {
                    param[63] = new SqlParameter("@TimeRange", SqlDbType.VarChar);
                    param[63].Value = TimeSpan.FromHours(Convert.ToInt32(TimeRange)).Ticks;
                }
                else
                {
                    param[63] = new SqlParameter("@TimeRange", SqlDbType.VarChar);
                    param[63].Value = TimeRange;
                }




                param[64] = new SqlParameter("@isAnyDriver", SqlDbType.Bit);
                param[64].Value = isAnyDriver;

                param[65] = new SqlParameter("@vDriverIDsCSVs", SqlDbType.VarChar);
                param[65].Value = sDriverIDsCSVs;


                param[66] = new SqlParameter("@isCustomDateEnabled", SqlDbType.Bit);
                param[66].Value = isCustomDateEnabled;

                param[67] = new SqlParameter("@iEnabledDateType", SqlDbType.Int);
                param[67].Value = iEnabledDateType;

                param[68] = new SqlParameter("@vReportPeriods", SqlDbType.VarChar);
                param[68].Value = FrequecyParameters;

                param[69] = new SqlParameter("@vSpeedType", SqlDbType.VarChar);
                param[69].Value = vSpeedType;


                param[70] = new SqlParameter("@bIncludeVehicleDetail", SqlDbType.Bit);
                param[70].Value = bIncludeVehicleDetail;


                param[71] = new SqlParameter("@WebHookGroupingId", SqlDbType.Int);
                param[71].Value = WebHookGroupingId;


                param[71] = new SqlParameter("@WebHookGroupingId", SqlDbType.Int);
                param[71].Value = WebHookGroupingId;


                param[72] = new SqlParameter("@IncludeMini_ResellerClients", SqlDbType.Bit);
                param[72].Value = IncludeMini_ResellerClients;



                param[53] = new SqlParameter("@Odometer", SqlDbType.Float);
                param[53].Value = Convert.ToDouble(UserSettings.ConvertXxToKms(ifkMeasurementUnit, Convert.ToString(Odometer), false, 2));

                param[1] = new SqlParameter("@ReportTypeId", SqlDbType.Int);
                param[1].Value = ReportTypeId;

                param[2] = new SqlParameter("@CompanyId", SqlDbType.Int);
                param[2].Value = companyid;

                param[3] = new SqlParameter("@Status", SqlDbType.Bit);
                param[3].Value = status;
                ;
                param[4] = new SqlParameter("@ReportName", SqlDbType.NVarChar);
                param[4].Value = report_name;

                param[5] = new SqlParameter("@Error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@ReportId", SqlDbType.Int);
                param[6].Value = ReportID;

                param[7] = new SqlParameter("@IsForAllCompanies", SqlDbType.Bit);
                param[7].Value = bIsForAllCompanies;

                param[8] = new SqlParameter("@Description", SqlDbType.NVarChar);
                param[8].Value = Description;

                param[9] = new SqlParameter("@bIsSystem", SqlDbType.Bit);
                param[9].Value = bIsSystem;

                param[10] = new SqlParameter("@DeviceID", SqlDbType.NVarChar);
                param[10].Value = DeviceID;

                param[11] = new SqlParameter("@GroupMID", SqlDbType.NVarChar);
                param[11].Value = GroupMID;

                param[12] = new SqlParameter("@Temp", SqlDbType.NVarChar);
                param[12].Value = "";

                param[13] = new SqlParameter("@isAnyAsset", SqlDbType.Bit);
                param[13].Value = isAnyAsset;

                param[14] = new SqlParameter("@Zone", SqlDbType.NVarChar);
                param[14].Value = Zone;


                param[15] = new SqlParameter("@ZoneType", SqlDbType.NVarChar);
                param[15].Value = ZoneType;

                param[16] = new SqlParameter("@GeoMID", SqlDbType.NVarChar);
                param[16].Value = GeoMID;

                param[17] = new SqlParameter("@Trip", SqlDbType.NVarChar);
                param[17].Value = Trip;

                param[18] = new SqlParameter("@TriggeredEventID", SqlDbType.Int);
                param[18].Value = TriggeredEventID;

                param[19] = new SqlParameter("@TriggeredEventStatus", SqlDbType.Bit);
                param[19].Value = TriggeredEventStatus;

                param[20] = new SqlParameter("@ifkDigitalMasterID", SqlDbType.BigInt);
                param[20].Value = ifkDigitalMasterID;

                param[21] = new SqlParameter("@DigitalEventStatus", SqlDbType.Bit);
                param[21].Value = DigitalEventStatus;

                param[22] = new SqlParameter("@AnaloglEventName", SqlDbType.NVarChar);
                param[22].Value = AnaloglEventName;

                param[23] = new SqlParameter("@AnalogEventStatus", SqlDbType.Bit);
                param[23].Value = AnalogEventStatus;

                param[24] = new SqlParameter("@IsTemeperatureViolation", SqlDbType.Bit);
                param[24].Value = IsTemeperatureViolation;

                //hdhdhdhdhdhd hdhdhd hddhhdd
                param[25] = new SqlParameter("@alert_CSV", SqlDbType.Char);
                param[25].Value = alert_Csv;

                param[26] = new SqlParameter("@IsExcessiveIdle", SqlDbType.Bit);
                param[26].Value = IsExcessiveIdle;

                param[27] = new SqlParameter("@Reminder", SqlDbType.NVarChar);
                param[27].Value = Reminder;

                param[28] = new SqlParameter("@iAlertPriority", SqlDbType.Int);
                param[28].Value = iAlertPriority;

                param[29] = new SqlParameter("@StartDate", SqlDbType.DateTime);
                param[29].Value = UserSettings.ConvertLocalDateTimeToUTCDateTime(Convert.ToDateTime(StartDate), timeZoneID);

                //param[21] = new SqlParameter("@StartDateReport", SqlDbType.DateTime);
                //param[21].Value = StratDateReport;


                param[30] = new SqlParameter("@IsScheduleReport", SqlDbType.Bit);
                param[30].Value = IsScheduled;

                param[31] = new SqlParameter("@ReportFormat", SqlDbType.NVarChar);
                param[31].Value = Format;

                param[32] = new SqlParameter("@ShowMeDataFor", SqlDbType.NVarChar);
                param[32].Value = ShowMeDataFor;

                // Added on 20-06-2014 



                if (ReportTypeId == 1)
                {
                    param[33] = new SqlParameter("@AlertId", SqlDbType.Int);
                    param[33].Value = AlertId;
                }
                else
                {
                    param[33] = new SqlParameter("@AlertId", SqlDbType.Int);
                    param[33].Value = 0;
                }


                param[34] = new SqlParameter("@IgnitionState", SqlDbType.NVarChar);
                param[34].Value = IgnitionState;

                param[35] = new SqlParameter("@BusinessInput ", SqlDbType.NVarChar);
                param[35].Value = BusinessInput;

                //param[36] = new SqlParameter("@AnalogEventStatus", SqlDbType.NVarChar);
                //param[36].Value = AnalogEventStatus;

                param[36] = new SqlParameter("@DigitalType ", SqlDbType.NVarChar);
                param[36].Value = DigitalType;

                param[37] = new SqlParameter("@IdlingReportMaxDuration", SqlDbType.NVarChar);
                param[37].Value = IdlingReportMaxDuration;

                param[38] = new SqlParameter("@OverSpeedMode", SqlDbType.NVarChar);
                param[38].Value = OverSpeedMode;

                param[39] = new SqlParameter("@OverSpeedMaxSpeed", SqlDbType.NVarChar);
                param[39].Value = OverSpeedMaxSpeed;

                param[40] = new SqlParameter("@StopReportType", SqlDbType.NVarChar);
                param[40].Value = StopReportType;

                param[41] = new SqlParameter("@StopReportTypeMinDuration", SqlDbType.NVarChar);
                param[41].Value = StopReportTypeMinDuration;


                param[42] = new SqlParameter("@TripListingMinDistance", SqlDbType.NVarChar);
                param[42].Value = TripListingMinDistance;

                param[43] = new SqlParameter("@SendReportDays", SqlDbType.NVarChar);
                param[43].Value = SendReportDays;

                param[44] = new SqlParameter("@AtThisTime", SqlDbType.NVarChar);
                param[44].Value = AtThisTime;

                if (EndDate == "")
                {
                    param[45] = new SqlParameter("@EndDate", SqlDbType.DateTime);
                    param[45].Value = System.DBNull.Value;
                }
                else
                {
                    param[45] = new SqlParameter("@EndDate", SqlDbType.DateTime);
                    param[45].Value = UserSettings.ConvertLocalDateTimeToUTCDateTime(Convert.ToDateTime(EndDate), timeZoneID);
                }

                param[46] = new SqlParameter("@DeviceCompanyID", SqlDbType.Int);
                param[46].Value = DeviceCompanyID;

                param[47] = new SqlParameter("@DeviceParentID", SqlDbType.Int);
                param[47].Value = DeviceParentID;

                param[48] = new SqlParameter("@SelectInactive", SqlDbType.Int);
                param[48].Value = SelectInactive;

                param[49] = new SqlParameter("@iCreatedByID", SqlDbType.Int);
                param[49].Value = iCreatedByID;

                param[50] = new SqlParameter("@iUpdatedByID", SqlDbType.Int);
                param[50].Value = iUpdatedByID;


                param[51] = new SqlParameter("@TrackerAssetType", SqlDbType.Int);
                param[51].Value = TrackerAssetType;

                param[52] = new SqlParameter("@AnalogType", SqlDbType.Int);
                param[52].Value = AnalogId;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "sp_Reports", param);

                if (operation == 8 && Convert.ToInt32(param[5].Value) == 8)
                {

                    result = "Report deleted";

                    Bal_HangfireScheduledReports.DeleteJob(ReportID);


                }

                else if (operation == 9 && Convert.ToInt32(param[5].Value) == 9)
                {
                    result = "Status changed";
                }
                else
                {
                    result = param[5].Value.ToString();

                    if ((operation == 2 || operation == 11) && IsScheduled)
                    {
                        var _EL_Hangfire = new EL_Hangfire
                        {

                            _sd = Convert.ToDateTime(StartDate),
                            _st = Convert.ToDateTime(AtThisTime),
                            ifkReportID = Convert.ToInt32(result),
                            FrequecyParameters = FrequecyParameters,
                            Frequency = Frequency,
                            StartingDate = Convert.ToDateTime(StartDate),
                            ifkUserID = objclsRegistration.pkUserID,
                            TimezoneID = objclsRegistration.vTimeZoneID,
                            iDisplayTypeId = iReportDisplayType,
                            iReportOperation = operation

                        };



                        Bal_HangfireScheduledReports.SheduleJobToStartAsFrom(_EL_Hangfire);

                    }


                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "SaveCustomReports()", ex.Message  + ex.StackTrace);
                result = "";
            }
            return result;
        }
        public string SaveCustomReports(clsRegistration objclsRegistration)
        {
            string timeZoneID = objclsRegistration.vTimeZoneID;
            int ifkMeasurementUnit = objclsRegistration.ifkMeasurementUnit;

            string result = "";
            try
            {
                //here
                SqlParameter[] param = new SqlParameter[75];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[54] = new SqlParameter("@vAssetList_CSV", SqlDbType.NChar);
                param[54].Value = vAssetList_CSV;

                param[55] = new SqlParameter("@Frequency", SqlDbType.NChar);
                param[55].Value = Frequency;

                param[56] = new SqlParameter("@ParamZoneID_Csv", SqlDbType.NChar);
                param[56].Value = ParamZoneID_Csv;

                param[57] = new SqlParameter("@isAnyZone", SqlDbType.Bit);
                param[57].Value = isAnyZone;

                param[58] = new SqlParameter("@vClientCSV", SqlDbType.NChar);
                param[58].Value = vClient_CSV;

                param[59] = new SqlParameter("@iAnyClient", SqlDbType.Int);
                param[59].Value = iAnyClient;

                param[60] = new SqlParameter("@iDigitalInputMapping", SqlDbType.Int);
                param[60].Value = iDigitalInputMapping;

                param[61] = new SqlParameter("@isShow", SqlDbType.Int);
                param[61].Value = isShow;

                param[62] = new SqlParameter("@iTripType", SqlDbType.Int);
                param[62].Value = iTripType;



                if (ReportTypeId == 54)

                {
                    param[63] = new SqlParameter("@TimeRange", SqlDbType.VarChar);
                    param[63].Value = TimeSpan.FromHours(Convert.ToInt32(TimeRange)).Ticks;
                }
                else
                {
                    param[63] = new SqlParameter("@TimeRange", SqlDbType.VarChar);
                    param[63].Value = TimeRange;
                }




                param[64] = new SqlParameter("@isAnyDriver", SqlDbType.Bit);
                param[64].Value = isAnyDriver;

                param[65] = new SqlParameter("@vDriverIDsCSVs", SqlDbType.VarChar);
                param[65].Value = sDriverIDsCSVs;


                param[66] = new SqlParameter("@isCustomDateEnabled", SqlDbType.Bit);
                param[66].Value = isCustomDateEnabled;

                param[67] = new SqlParameter("@iEnabledDateType", SqlDbType.Int);
                param[67].Value = iEnabledDateType;

                param[68] = new SqlParameter("@vReportPeriods", SqlDbType.VarChar);
                param[68].Value = FrequecyParameters;

                param[69] = new SqlParameter("@vSpeedType", SqlDbType.VarChar);
                param[69].Value = vSpeedType;


                param[70] = new SqlParameter("@bIncludeVehicleDetail", SqlDbType.Bit);
                param[70].Value = bIncludeVehicleDetail;


                param[71] = new SqlParameter("@WebHookGroupingId", SqlDbType.Int);
                param[71].Value = WebHookGroupingId;


              
                param[72] = new SqlParameter("@IncludeMini_ResellerClients", SqlDbType.Bit);
                param[72].Value = IncludeMini_ResellerClients;


                param[73] = new SqlParameter("@vAdditionalParams", SqlDbType.VarChar);
                param[73].Value = vAdditionalParams;


                param[74] = new SqlParameter("@Attributes", SqlDbType.VarChar);
                param[74].Value = Attributes;




                param[53] = new SqlParameter("@Odometer", SqlDbType.Float);
                param[53].Value = Convert.ToDouble(UserSettings.ConvertXxToKms(ifkMeasurementUnit, Convert.ToString(Odometer), false, 2));

                param[1] = new SqlParameter("@ReportTypeId", SqlDbType.Int);
                param[1].Value = ReportTypeId;

                param[2] = new SqlParameter("@CompanyId", SqlDbType.Int);
                param[2].Value = companyid;

                param[3] = new SqlParameter("@Status", SqlDbType.Bit);
                param[3].Value = status;
                ;
                param[4] = new SqlParameter("@ReportName", SqlDbType.NVarChar);
                param[4].Value = report_name;

                param[5] = new SqlParameter("@Error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@ReportId", SqlDbType.Int);
                param[6].Value = ReportID;

                param[7] = new SqlParameter("@IsForAllCompanies", SqlDbType.Bit);
                param[7].Value = bIsForAllCompanies;

                param[8] = new SqlParameter("@Description", SqlDbType.NVarChar);
                param[8].Value = Description;

                param[9] = new SqlParameter("@bIsSystem", SqlDbType.Bit);
                param[9].Value = bIsSystem;

                param[10] = new SqlParameter("@DeviceID", SqlDbType.NVarChar);
                param[10].Value = DeviceID;

                param[11] = new SqlParameter("@GroupMID", SqlDbType.NVarChar);
                param[11].Value = GroupMID;

                param[12] = new SqlParameter("@Temp", SqlDbType.NVarChar);
                param[12].Value = "";

                param[13] = new SqlParameter("@isAnyAsset", SqlDbType.Bit);
                param[13].Value = isAnyAsset;

                param[14] = new SqlParameter("@Zone", SqlDbType.NVarChar);
                param[14].Value = Zone;


                param[15] = new SqlParameter("@ZoneType", SqlDbType.NVarChar);
                param[15].Value = ZoneType;

                param[16] = new SqlParameter("@GeoMID", SqlDbType.NVarChar);
                param[16].Value = GeoMID;

                param[17] = new SqlParameter("@Trip", SqlDbType.NVarChar);
                param[17].Value = Trip;

                param[18] = new SqlParameter("@TriggeredEventID", SqlDbType.Int);
                param[18].Value = TriggeredEventID;

                param[19] = new SqlParameter("@TriggeredEventStatus", SqlDbType.Bit);
                param[19].Value = TriggeredEventStatus;

                param[20] = new SqlParameter("@ifkDigitalMasterID", SqlDbType.BigInt);
                param[20].Value = ifkDigitalMasterID;

                param[21] = new SqlParameter("@DigitalEventStatus", SqlDbType.Bit);
                param[21].Value = DigitalEventStatus;

                param[22] = new SqlParameter("@AnaloglEventName", SqlDbType.NVarChar);
                param[22].Value = AnaloglEventName;

                param[23] = new SqlParameter("@AnalogEventStatus", SqlDbType.Bit);
                param[23].Value = AnalogEventStatus;

                param[24] = new SqlParameter("@IsTemeperatureViolation", SqlDbType.Bit);
                param[24].Value = IsTemeperatureViolation;

                //hdhdhdhdhdhd hdhdhd hddhhdd
                param[25] = new SqlParameter("@alert_CSV", SqlDbType.Char);
                param[25].Value = alert_Csv;

                param[26] = new SqlParameter("@IsExcessiveIdle", SqlDbType.Bit);
                param[26].Value = IsExcessiveIdle;

                param[27] = new SqlParameter("@Reminder", SqlDbType.NVarChar);
                param[27].Value = Reminder;

                param[28] = new SqlParameter("@iAlertPriority", SqlDbType.Int);
                param[28].Value = iAlertPriority;

                param[29] = new SqlParameter("@StartDate", SqlDbType.DateTime);
                param[29].Value = UserSettings.ConvertLocalDateTimeToUTCDateTime(Convert.ToDateTime(StartDate), timeZoneID);

                //param[21] = new SqlParameter("@StartDateReport", SqlDbType.DateTime);
                //param[21].Value = StratDateReport;


                param[30] = new SqlParameter("@IsScheduleReport", SqlDbType.Bit);
                param[30].Value = IsScheduled;

                param[31] = new SqlParameter("@ReportFormat", SqlDbType.NVarChar);
                param[31].Value = Format;

                param[32] = new SqlParameter("@ShowMeDataFor", SqlDbType.NVarChar);
                param[32].Value = ShowMeDataFor;

                // Added on 20-06-2014 



                if (ReportTypeId == 1)
                {
                    param[33] = new SqlParameter("@AlertId", SqlDbType.Int);
                    param[33].Value = AlertId;
                }
                else
                {
                    param[33] = new SqlParameter("@AlertId", SqlDbType.Int);
                    param[33].Value = 0;
                }


                param[34] = new SqlParameter("@IgnitionState", SqlDbType.NVarChar);
                param[34].Value = IgnitionState;

                param[35] = new SqlParameter("@BusinessInput ", SqlDbType.NVarChar);
                param[35].Value = BusinessInput;

                //param[36] = new SqlParameter("@AnalogEventStatus", SqlDbType.NVarChar);
                //param[36].Value = AnalogEventStatus;

                param[36] = new SqlParameter("@DigitalType ", SqlDbType.NVarChar);
                param[36].Value = DigitalType;

                param[37] = new SqlParameter("@IdlingReportMaxDuration", SqlDbType.NVarChar);
                param[37].Value = IdlingReportMaxDuration;

                param[38] = new SqlParameter("@OverSpeedMode", SqlDbType.NVarChar);
                param[38].Value = OverSpeedMode;

                param[39] = new SqlParameter("@OverSpeedMaxSpeed", SqlDbType.NVarChar);
                param[39].Value = OverSpeedMaxSpeed;

                param[40] = new SqlParameter("@StopReportType", SqlDbType.NVarChar);
                param[40].Value = StopReportType;

                param[41] = new SqlParameter("@StopReportTypeMinDuration", SqlDbType.NVarChar);
                param[41].Value = StopReportTypeMinDuration;


                param[42] = new SqlParameter("@TripListingMinDistance", SqlDbType.NVarChar);
                param[42].Value = TripListingMinDistance;

                param[43] = new SqlParameter("@SendReportDays", SqlDbType.NVarChar);
                param[43].Value = SendReportDays;

                param[44] = new SqlParameter("@AtThisTime", SqlDbType.NVarChar);
                param[44].Value = AtThisTime;

                if (EndDate == "")
                {
                    param[45] = new SqlParameter("@EndDate", SqlDbType.DateTime);
                    param[45].Value = System.DBNull.Value;
                }
                else
                {
                    param[45] = new SqlParameter("@EndDate", SqlDbType.DateTime);
                    param[45].Value = UserSettings.ConvertLocalDateTimeToUTCDateTime(Convert.ToDateTime(EndDate), timeZoneID);
                }

                param[46] = new SqlParameter("@DeviceCompanyID", SqlDbType.Int);
                param[46].Value = DeviceCompanyID;

                param[47] = new SqlParameter("@DeviceParentID", SqlDbType.Int);
                param[47].Value = DeviceParentID;

                param[48] = new SqlParameter("@SelectInactive", SqlDbType.Int);
                param[48].Value = SelectInactive;

                param[49] = new SqlParameter("@iCreatedByID", SqlDbType.Int);
                param[49].Value = iCreatedByID;

                param[50] = new SqlParameter("@iUpdatedByID", SqlDbType.Int);
                param[50].Value = iUpdatedByID;


                param[51] = new SqlParameter("@TrackerAssetType", SqlDbType.Int);
                param[51].Value = TrackerAssetType;

                param[52] = new SqlParameter("@AnalogType", SqlDbType.Int);
                param[52].Value = AnalogId;


                SqlHelper.ExecuteNonQuery(AppConfiguration.Getwlt_WebAppConnectionString(), CommandType.StoredProcedure, "sp_Reports", param);

                if (operation == 8 && Convert.ToInt32(param[5].Value) == 8)
                {

                    result = "Report deleted";

                    Bal_HangfireScheduledReports.DeleteJob(ReportID);


                }

                else if (operation == 9 && Convert.ToInt32(param[5].Value) == 9)
                {
                    result = "Status changed";
                }
                else
                {
                    result = param[5].Value.ToString();

                    if ((operation == 2 || operation == 11) && IsScheduled)
                    {
                        var _EL_Hangfire = new EL_Hangfire
                        {

                            _sd = Convert.ToDateTime(StartDate),
                            _st = Convert.ToDateTime(AtThisTime),
                            ifkReportID = Convert.ToInt32(result),
                            FrequecyParameters = FrequecyParameters,
                            Frequency = Frequency,
                            StartingDate = Convert.ToDateTime(StartDate),
                            ifkUserID = objclsRegistration.pkUserID,
                            TimezoneID = objclsRegistration.vTimeZoneID,
                            iDisplayTypeId = iReportDisplayType,
                            iReportOperation = operation

                        };



                        Bal_HangfireScheduledReports.SheduleJobToStartAsFrom(_EL_Hangfire);

                    }


                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsReport.cs", "SaveCustomReports()", ex.Message  + ex.StackTrace);
                result = "";
            }
            return result;
        }

        public string SaveReportCriteria()
        {
            string result = "";

            SqlParameter[] param = new SqlParameter[24];
            try
            {
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[1] = new SqlParameter("@ReportId", SqlDbType.NVarChar);
                param[1].Value = ReportID;

                param[2] = new SqlParameter("@DeviceID", SqlDbType.NVarChar);
                param[2].Value = DeviceID;

                param[3] = new SqlParameter("@GroupMID", SqlDbType.NVarChar);
                param[3].Value = GroupMID;

                param[4] = new SqlParameter("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@IsAnyVehicle", SqlDbType.Bit);
                param[5].Value = isAnyAsset;

                param[6] = new SqlParameter("@Zone", SqlDbType.NVarChar);
                param[6].Value = Zone;

                param[7] = new SqlParameter("@ZoneType", SqlDbType.NVarChar);
                param[7].Value = ZoneType;

                param[8] = new SqlParameter("@GeoMID", SqlDbType.NVarChar);
                param[8].Value = GeoMID;

                param[9] = new SqlParameter("@Trip", SqlDbType.NVarChar);
                param[9].Value = Trip;

                param[10] = new SqlParameter("@TriggeredEventID", SqlDbType.Int);
                param[10].Value = TriggeredEventID;

                param[11] = new SqlParameter("@TriggeredEventStatus", SqlDbType.Bit);
                param[11].Value = TriggeredEventStatus;

                param[12] = new SqlParameter("@ifkDigitalMasterID", SqlDbType.BigInt);
                param[12].Value = ifkDigitalMasterID;

                param[13] = new SqlParameter("@DigitalEventStatus", SqlDbType.Bit);
                param[13].Value = DigitalEventStatus;

                param[14] = new SqlParameter("@AnaloglEventName", SqlDbType.NVarChar);
                param[14].Value = AnaloglEventName;

                param[15] = new SqlParameter("@AnalogEventStatus", SqlDbType.Bit);
                param[15].Value = AnalogEventStatus;

                param[16] = new SqlParameter("@IsTemeperatureViolation", SqlDbType.Bit);
                param[16].Value = IsTemeperatureViolation;

                param[17] = new SqlParameter("@IsOverspeed", SqlDbType.Bit);
                param[17].Value = IsOverspeed;

                param[18] = new SqlParameter("@IsExcessiveIdle", SqlDbType.Bit);
                param[18].Value = IsExcessiveIdle;

                param[19] = new SqlParameter("@Reminder", SqlDbType.NVarChar);
                param[19].Value = Reminder;

                param[20] = new SqlParameter("@Frequency", SqlDbType.NVarChar);
                param[20].Value = Frequency;

                param[21] = new SqlParameter("@StartDate", SqlDbType.NVarChar);
                param[21].Value = StartDate;

                //param[21] = new SqlParameter("@StartDateReport", SqlDbType.DateTime);
                //param[21].Value = StratDateReport;


                param[22] = new SqlParameter("@IsScheduleReport", SqlDbType.Bit);
                param[22].Value = IsScheduled;

                param[23] = new SqlParameter("@ReportFormat", SqlDbType.NVarChar);
                param[23].Value = Format;

                //param[24] = new SqlParameter("@EndDate", SqlDbType.NVarChar);
                //param[24].Value = EndDate;


                SqlHelper.ExecuteNonQuery(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);
                result = "data save success";


            }
            catch (Exception ex)
            {


                LogError.RegisterErrorInLogFile( "clsReport.cs", "SaveAddAlertCriteria()", ex.Message  + ex.StackTrace);
                result = "-1";

            }

            return result;
        }

        public string SaveReportTimePeriods()
        {
            string result = "";

            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[1] = new SqlParameter("@HoursType", SqlDbType.Bit);
                param[1].Value = bHoursType;

                param[2] = new SqlParameter("@StartsTime", SqlDbType.Time);
                param[2].Value = vStartsTime;

                param[3] = new SqlParameter("@EndsTime", SqlDbType.Time);
                param[3].Value = vEndsTime;

                param[4] = new SqlParameter("@DaysOfWeek", SqlDbType.NVarChar);
                param[4].Value = vDaysOfWeek;

                param[5] = new SqlParameter("@ReportID", SqlDbType.NVarChar);
                param[5].Value = ReportID;

                SqlHelper.ExecuteNonQuery(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);

                result = "";



            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "SaveAddAlertTimePeriods()", ex.Message  + ex.StackTrace);
                result = "-1";
            }

            return result;
        }

        public string SaveReportOtherCriteria()
        {
            string result = "";

            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[1] = new SqlParameter("@ifkDigitalMasterID", SqlDbType.BigInt);
                param[1].Value = ifkDigitalMasterID;

                param[2] = new SqlParameter("@EventStatus", SqlDbType.Bit);
                param[2].Value = bEventStatus;

                param[3] = new SqlParameter("@ReportID", SqlDbType.NVarChar);
                param[3].Value = ReportID;

                SqlHelper.ExecuteNonQuery(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "SaveAddAlertotherCriteria()", ex.Message  + ex.StackTrace);
                result = "-1";
            }

            return result;
        }

        public string SaveReportNotifyUsers()
        {
            string result = "";

            SqlParameter[] param = new SqlParameter[5];
            try
            {
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[1] = new SqlParameter("@UserIDs", SqlDbType.NVarChar);
                param[1].Value = UserIDs;

                param[2] = new SqlParameter("@RoleIDs", SqlDbType.NVarChar);
                param[2].Value = RoleIDs;

                param[3] = new SqlParameter("@ReportID", SqlDbType.NVarChar);
                param[3].Value = ReportID;

                param[4] = new SqlParameter("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);
                if (param[4].Value.ToString() == "1")
                {
                    result = "Save successful";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "SaveAddAlertNotifyUsers()", ex.Message  + ex.StackTrace);
                result = "-1";
            }

            return result;
        }

        public DataSet GetReportList()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                param[1].Value = companyid;

                param[2] = new SqlParameter("@ReportTypeId", SqlDbType.Int);
                param[2].Value = report_type_id;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                DataSet ds = SqlHelper.ExecuteDataset(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);
                return ds;
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "GetReportList()", ex.Message  + ex.StackTrace);
                return null;
            }
        }

        public DataSet GetStaticReportList()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[1] = new SqlParameter("@CompanyId", SqlDbType.Int);
                param[1].Value = companyid;

                param[2] = new SqlParameter("@ReportTypeId", SqlDbType.Int);
                param[2].Value = report_type_id;

                DataSet ds = SqlHelper.ExecuteDataset(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);
                return ds;
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "GetStaticReportList()", ex.Message  + ex.StackTrace);
                return null;
            }
        }





        public List<ClsReport> GetReportInfo(string TimeZoneId)
        {
          

            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@op", SqlDbType.Int);
            param[0].Value = operation;

            param[1] = new SqlParameter("@ReportID", SqlDbType.Int);
            param[1].Value = ReportID;

            DataSet ds = SqlHelper.ExecuteDataset(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);

            List<ClsReport> reportValueList = new List<ClsReport>();

            if (ds.Tables.Count > 0)
            {
                #region Report Main Criteria
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ClsReport rCritria = new ClsReport();

                    rCritria.report_name = ds.Tables[0].Rows[0]["vReportName"].ToString();
                    rCritria.DeviceID = ds.Tables[0].Rows[0]["vfkDeviceID"].ToString();
                    rCritria.companyid = Convert.ToInt32(ds.Tables[0].Rows[0]["ifkCompanyID"]);
                    // rCritria.StartDate =  Convert.ToDateTime(ds.Tables[0].Rows[0]["dStartDate"]).ToString("dd-MM-yyyy");

                    rCritria.StartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(ds.Tables[0].Rows[0]["dStartDate"]), TimeZoneId).ToString("dd-MM-yyyy");
                    //asset
                    rCritria.vAssetList_CSV = Convert.ToString(ds.Tables[0].Rows[0]["vAssetList_CSV"]);
                    rCritria.isAnyAsset = Convert.ToBoolean(ds.Tables[0].Rows[0]["isAnyAsset"]);
                    rCritria.AssetTrackerType = Convert.ToInt32(ds.Tables[0].Rows[0]["AssetTrackerType"]);
                    //alert
                    rCritria.alert_Csv = Convert.ToString(ds.Tables[0].Rows[0]["alert_Csv"]);
                    rCritria.iAlertPriority = Convert.ToInt32(ds.Tables[0].Rows[0]["iAlertPriority"]);


                    //zone
                    rCritria.ParamZoneID_Csv = Convert.ToString(ds.Tables[0].Rows[0]["ParamZoneID_Csv"]);
                    rCritria.ZoneTypeId = Convert.ToString(ds.Tables[0].Rows[0]["vZoneType"]);
                    rCritria.isAnyZone = Convert.ToBoolean(ds.Tables[0].Rows[0]["isAnyZone"]);

                    //specific to only zone trips report
                    rCritria.Zone = Convert.ToString(ds.Tables[0].Rows[0]["vZone"]);


                    //specific to only devices not in use report
                    rCritria.iAnyClient = Convert.ToInt32(ds.Tables[0].Rows[0]["iAnyclient"]);

                    rCritria.vClient_CSV = Convert.ToString(ds.Tables[0].Rows[0]["vClientsCSV"]);

                    rCritria.isShow = Convert.ToInt32(ds.Tables[0].Rows[0]["IsShow"]);


                    rCritria.bAnyDriver = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["isAnyDriver"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["isAnyDriver"]);

                    rCritria.vDriverIds = Convert.ToString(ds.Tables[0].Rows[0]["vDriverIDs"]);

                    rCritria.iDigitalInputMapping = Convert.ToInt32(ds.Tables[0].Rows[0]["iDigitalInputMapping"]);

                    rCritria.FrequecyParameters = Convert.ToString(ds.Tables[0].Rows[0]["vReportPeriods"]);

                    rCritria.Format = Convert.ToString(ds.Tables[0].Rows[0]["ReportFormat"]);

                    rCritria.iEnabledDateType = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["iEnabledDateType"].ToString()) ? 0 : Convert.ToInt16(ds.Tables[0].Rows[0]["iEnabledDateType"]);

                    rCritria.isCustomDateEnabled = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["isCustomDateEnabled"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["isCustomDateEnabled"]);


                    rCritria.iTripType = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["iTripType"].ToString()) ? 0 : Convert.ToInt16(ds.Tables[0].Rows[0]["iTripType"]);

                    rCritria.vSpeedType = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["vSpeedType"].ToString()) ? "" : Convert.ToString(ds.Tables[0].Rows[0]["vSpeedType"]);

                    rCritria.WebHookGroupingId = Convert.ToInt32(ds.Tables[0].Rows[0]["webHookGroupingId"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["webHookGroupingId"]);

                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        rCritria.iCreatedBy = Convert.ToString(ds.Tables[4].Rows[0]["iCreatedBy"]);
                        rCritria.iCreatedOn = UserSettings.ConvertUTCDateTimeToProperLocalDateTime(Convert.ToDateTime(ds.Tables[4].Rows[0]["dCreatedOn"]), TimeZoneId);
                    }
                    else
                    {
                        rCritria.iCreatedBy = "";
                        rCritria.iCreatedOn = "";
                    }


                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        rCritria.iUpdatedBy = Convert.ToString(ds.Tables[5].Rows[0]["iUpdatedBy"]);
                        rCritria.iUpdatedOn = UserSettings.ConvertUTCDateTimeToProperLocalDateTime(Convert.ToDateTime(ds.Tables[5].Rows[0]["dLastAccessed"]), TimeZoneId);
                    }
                    else
                    {
                        rCritria.iUpdatedBy = "";
                        rCritria.iUpdatedOn = "";
                    }

                    DateTime dval;

                    if (DateTime.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["dEndDate"]), out dval))
                        rCritria.EndDate = dval.ToString("dd-MM-yyyy");
                    else
                        rCritria.EndDate = "";



                    clsAlert oa = new clsAlert();

                    if (ds.Tables[0].Rows[0]["iAlertId"].ToString() != "0")
                    {
                        rCritria.DeviceOrGroupName = oa.GetAlertName(ds.Tables[0].Rows[0]["iAlertId"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["vfkDeviceID"].ToString() != "0")
                    {
                        rCritria.DeviceOrGroupName = oa.GetGroup_DeviceName(0, ds.Tables[0].Rows[0]["vfkDeviceID"].ToString(), 0);
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["ifkGroupMID"].ToString()) != 0)
                    {
                        rCritria.DeviceOrGroupName = oa.GetGroup_DeviceName(Convert.ToInt32(ds.Tables[0].Rows[0]["ifkGroupMID"].ToString()), "0", 0);
                        rCritria.GroupMID = Convert.ToInt32(ds.Tables[0].Rows[0]["ifkGroupMID"].ToString());
                    }


                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["vZone"])))
                    {
                        clsAlert objAlert = new clsAlert();
                        objAlert.Operation = 18;
                        rCritria.Zone = objAlert.vZone = ds.Tables[0].Rows[0]["vZone"].ToString();
                        rCritria.ZoneType = objAlert.vZoneType = ds.Tables[0].Rows[0]["vZoneType"].ToString();
                        rCritria.GeoMID = objAlert.ifkGeoMID = Convert.ToInt32(ds.Tables[0].Rows[0]["ifkGeoMID"]);
                        rCritria.HTML = objAlert.GetZoneName();
                    }
                    else if (Convert.ToInt32(ds.Tables[0].Rows[0]["iTriggeredEventID"].ToString()) != 0)
                    {
                        rCritria.TriggeredEventID = Convert.ToInt32(ds.Tables[0].Rows[0]["iTriggeredEventID"].ToString());
                        rCritria.TriggeredEventStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["bTriggeredEventStatus"]);
                        rCritria.vEventName = oa.GetEventName(Convert.ToInt32(ds.Tables[0].Rows[0]["iTriggeredEventID"].ToString()), Convert.ToBoolean(ds.Tables[0].Rows[0]["bTriggeredEventStatus"]));
                    }
                    rCritria.vDigitalEvent = Convert.ToString(ds.Tables[0].Rows[0]["vDigitalEvent"]);
                    rCritria.DigitalEventStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["bDigitalEventStatus"]);
                    rCritria.AnaloglEventName = Convert.ToString(ds.Tables[0].Rows[0]["vAnaloglEvent"]);
                    rCritria.AnalogEventStatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["bAnalogEventStatus"]);
                    rCritria.IsTemeperatureViolation = Convert.ToBoolean(ds.Tables[0].Rows[0]["bTemeperatureViolation"]);
                    rCritria.IsOverspeed = Convert.ToBoolean(ds.Tables[0].Rows[0]["bOverspeed"]);
                    rCritria.IsExcessiveIdle = Convert.ToBoolean(ds.Tables[0].Rows[0]["bExcessiveIdle"]);
                    rCritria.Trip = Convert.ToString(ds.Tables[0].Rows[0]["vTrip"]);
                    rCritria.Reminder = Convert.ToString(ds.Tables[0].Rows[0]["vReminder"]);
                    rCritria.Frequency = Convert.ToString(ds.Tables[0].Rows[0]["Frequency"]);
                    rCritria.Description = Convert.ToString(ds.Tables[0].Rows[0]["vDescription"]);
                    rCritria.IsScheduled = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsScheduleReport"]);


                    rCritria.Attributes = Convert.ToString(ds.Tables[0].Rows[0]["Attributes"]);
                    rCritria.vAdditionalParams = Convert.ToString(ds.Tables[0].Rows[0]["vAdditionalParams"]);
                    //Added on 25th June 2013
                    rCritria.report_type_id = Convert.ToInt32(ds.Tables[0].Rows[0]["ifkReportTypeId"]);
                    rCritria.ShowMeDataFor = Convert.ToString(ds.Tables[0].Rows[0]["vShowMeDataFor"]);
                    rCritria.AlertId = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["iAlertId"])) ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["iAlertId"]);
                    // rCritria.IgnitionState = Convert.ToString(ds.Tables[0].Rows[0]["vIgnitionState"]);
                    // rCritria.BusinessInput = Convert.ToString(ds.Tables[0].Rows[0]["vBusinessInput"]);
                    // rCritria.DigitalType = Convert.ToString(ds.Tables[0].Rows[0]["vDigitalType"]);
                    // rCritria.IdlingReportMaxDuration = Convert.ToString(ds.Tables[0].Rows[0]["vIdlingReportMaxDuration"]);
                    // rCritria.OverSpeedMode = Convert.ToString(ds.Tables[0].Rows[0]["vOverSpeedMode"]);
                    // rCritria.OverSpeedMaxSpeed = Convert.ToString(ds.Tables[0].Rows[0]["vOverSpeedMaxSpeed"]);
                    // rCritria.StopReportType = Convert.ToString(ds.Tables[0].Rows[0]["vStopReportType"]);
                    //rCritria.StopReportTypeMinDuration = Convert.ToString(ds.Tables[0].Rows[0]["vStopReportTypeMinDuration"]);



                    //  rCritria.TripListingMinDistance = Convert.ToString(ds.Tables[0].Rows[0]["vTripListingMinDistance"]);
                    rCritria.SendReportDays = Convert.ToString(ds.Tables[0].Rows[0]["vSendReportDays"]);
                    rCritria.AtThisTime = Convert.ToString(ds.Tables[0].Rows[0]["vAtThisTime"]);

                    rCritria.TimeRange = Convert.ToString(ds.Tables[0].Rows[0]["vCustomTime"]);

                    rCritria.IncludeMini_ResellerClients = Convert.ToBoolean(ds.Tables[0].Rows[0]["bIncludeMini_ResellerClients"]);



                    reportValueList.Add(rCritria);

                }
                else
                {
                    reportValueList.Add(null);
                }
                #endregion
                #region Report Time Criteria
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ClsReport ort = new ClsReport();

                    ort.lstTimePeriodContent = new List<ClsReport>();
                    int timeArrayCount = 0;
                    foreach (DataRow drTime in ds.Tables[1].Rows)
                    {
                        string inOutTime = String.Empty;
                        string startTime = (drTime["dStartDate"].ToString()).Substring(0, (drTime["dStartDate"].ToString()).Length - 3);

                        string endTime = "";
                        if (drTime["dEndDate"].ToString() != "")
                        {
                            endTime = (drTime["dEndDate"].ToString()).Substring(0, (drTime["dEndDate"].ToString()).Length - 3);
                        }


                        inOutTime = "Inside the hours of : ";

                        //if (Convert.ToBoolean(drTime["bHoursType"].ToString()) == true)
                        //{
                        //    inOutTime = "Inside the hours of : ";
                        //}
                        //else
                        //{
                        //    inOutTime = "Outside the hours of : ";
                        //}

                        StringBuilder sbTimePeriod = new StringBuilder();
                        sbTimePeriod.Append("<span class='selected_text' style='width: 170px;' id=\"" + (inOutTime + "-" + startTime + "-and-" + endTime + "-on-" + drTime["vSendReportDays"]).ToString().Replace(" ", "-") + "\">");
                        sbTimePeriod.Append("<img name='Off' class='img_closeEvent' src='../images/notifyclose.jpg' ");
                        sbTimePeriod.Append(" onclick='RemoveTimePeriods(" + timeArrayCount + ",\"" + (inOutTime + "-" + startTime + "-and-" + endTime + "-on-" + drTime["vSendReportDays"]).ToString().Replace(" ", "-") + "\",\"report\");' /> ");
                        sbTimePeriod.Append("<a href='#' style='color:#95B22D;width:190px;' ");
                        //sbTimePeriod.Append(" onclick='EditTimePeriods(" + timeArrayCount + ",\"" + Convert.ToBoolean(drTime["bHoursType"]).ToString().ToLower() + "\",\"" + startTime + "\",\"" + endTime + "\",\"" + drTime["vSendReportDays"].ToString() + "\",\"" + (inOutTime + "-" + startTime + "-and-" + endTime + "-on-" + drTime["vSendReportDays"]).ToString().Replace(" ", "-") + "\",\"report\");' >");
                        sbTimePeriod.Append(" onclick='EditTimePeriods(" + timeArrayCount + ",\true\",\"" + startTime + "\",\"" + endTime + "\",\"" + drTime["vSendReportDays"].ToString() + "\",\"" + (inOutTime + "-" + startTime + "-and-" + endTime + "-on-" + drTime["vSendReportDays"]).ToString().Replace(" ", "-") + "\",\"report\");' >");
                        sbTimePeriod.Append(inOutTime + " " + startTime + " and " + endTime + " on " + drTime["vSendReportDays"] + "  ");
                        sbTimePeriod.Append("</a>");
                        sbTimePeriod.Append("</span>");

                        //ort.lstTimePeriodContent.Add(new ClsReport(Convert.ToBoolean(drTime["bHoursType"]), Convert.ToString(drTime["dStartDate"]), Convert.ToString(drTime["dEndDate"]),
                        ort.lstTimePeriodContent.Add(new ClsReport(false, Convert.ToString(drTime["dStartDate"]), Convert.ToString(drTime["dEndDate"]),
                            Convert.ToString(drTime["vSendReportDays"]), sbTimePeriod.ToString()));
                        timeArrayCount++;

                        reportValueList.Add(ort);
                    }
                }
                else
                {
                    reportValueList.Add(null);
                }
                #endregion
                #region Report Other Criteria
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ClsReport oro = new ClsReport();

                    StringBuilder sbOtherCriteria = new StringBuilder();
                    foreach (DataRow drOther in ds.Tables[2].Rows)
                    {
                        string onOff = String.Empty;
                        onOff = Convert.ToString(drOther["status"]);
                        sbOtherCriteria.Append("<span class='selected_text' title='" + drOther["ifkDigitalMasterID"].ToString() + "," + drOther["bEventStatus"].ToString() + "' id='" + drOther["vEventName"].ToString().Replace(" ", "") + "'>");
                        sbOtherCriteria.Append("<img onclick='RemoveDigitalEvents(\"" + drOther["vEventName"].ToString().Replace(" ", "") + "\");' src='../images/notifyclose.jpg' ");
                        sbOtherCriteria.Append(" class='img_closeEvent' name='" + onOff + "'>");
                        sbOtherCriteria.Append(drOther["vEventName"].ToString() + " is " + onOff);
                        sbOtherCriteria.Append("</span>");
                    }
                    oro.HTML = sbOtherCriteria.ToString();
                    reportValueList.Add(oro);

                }
                else
                {
                    reportValueList.Add(null);
                }
                #endregion
                #region Report Notify People
                if (ds.Tables[3].Rows.Count > 0)
                {
                    ClsReport orn = new ClsReport();

                    foreach (DataRow _dr in ds.Tables[3].Rows)
                    {
                        StringBuilder sbNotify = new StringBuilder();
                        clsAlert oa = new clsAlert();

                        oa.vUserIDs = (!String.IsNullOrEmpty(_dr["iUserID"].ToString())) ? _dr["iUserID"].ToString() : "0";
                        oa.vRoleIDs = (!String.IsNullOrEmpty(_dr["vRoleIDs"].ToString())) ? _dr["vRoleIDs"].ToString() : "0";


                        using (DataSet dsUserRole = oa.GetUser_RoleName())
                        {
                            if (dsUserRole.Tables.Count > 0)
                            {
                                if (dsUserRole.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow drUsers in dsUserRole.Tables[0].Rows)
                                    {
                                        sbNotify.Append("<div class='NotifyContent' id='divNotifyName" + Convert.ToInt32(drUsers["pkUserID"].ToString()) + "'>");
                                        sbNotify.Append("<img style='float:left;' src='../images/user.jpg' alt='' />");
                                        sbNotify.Append("<div style='float: right; width:90%; position:relative;'>");
                                        sbNotify.Append("<img onclick='RemoveNotifyContent(\"divNotifyName" + Convert.ToInt32(drUsers["pkUserID"].ToString()) + "\");' ");
                                        sbNotify.Append(" class='img_notifyClose' src='../images/notifyclose.jpg' alt=''>");
                                        sbNotify.Append("<span class='span_notify' title='User' id='" + Convert.ToInt32(drUsers["pkUserID"].ToString()) + "'> ");
                                        sbNotify.Append(drUsers["vName"].ToString());
                                        sbNotify.Append("</span>");
                                        sbNotify.Append("<br><a href='#'>User</a></div><div></div></div>");
                                    }
                                }
                                if (dsUserRole.Tables[1].Rows.Count > 0)
                                {
                                    foreach (DataRow drRole in dsUserRole.Tables[1].Rows)
                                    {
                                        sbNotify.Append("<div class='NotifyContent' id='divNotifyTypeName" + Convert.ToInt32(drRole["ipkContactTypeID"].ToString()) + "''>");
                                        sbNotify.Append("<img style='float:left;' src='../images/user_role.jpg' alt='' />");
                                        sbNotify.Append("<div style='float: right; width:90%; position:relative;'>");
                                        sbNotify.Append("<img onclick='RemoveNotifyContent(\"divNotifyTypeName" + Convert.ToInt32(drRole["ipkContactTypeID"].ToString()) + "\");' ");
                                        sbNotify.Append(" class='img_notifyClose' src='../images/notifyclose.jpg' alt=''>");
                                        sbNotify.Append("<span class='span_notify' title='User Role' id='" + Convert.ToInt32(drRole["ipkContactTypeID"].ToString()) + "''>");
                                        sbNotify.Append(drRole["vTypeName"].ToString());
                                        sbNotify.Append("</span>");
                                        sbNotify.Append("<br><a href='#'>User Role</a></div><div></div></div>");
                                    }
                                }
                            }
                        }


                        orn.HTML += sbNotify.ToString();
                        reportValueList.Add(orn);

                    }
                }
                else
                {
                    reportValueList.Add(null);
                }
                #endregion

                return reportValueList;
            }
            else
            {
                return null;
            }
           
        }





        public DataTable ViewReport()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[1] = new SqlParameter("@ReportID", SqlDbType.Int);
                param[1].Value = ReportID;

                param[2] = new SqlParameter("@ReportTypeId", SqlDbType.Int);
                param[2].Value = report_type_id;

                param[3] = new SqlParameter("@CompanyId", SqlDbType.Int);
                param[3].Value = companyid;

                DataSet ds = SqlHelper.ExecuteDataset(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);

                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "ViewReport()", ex.Message  + ex.StackTrace);
                return null;
            }
        }

        public DataSet ViewReportInds()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[1] = new SqlParameter("@ReportID", SqlDbType.Int);
                param[1].Value = ReportID;

                param[2] = new SqlParameter("@ReportTypeId", SqlDbType.Int);
                param[2].Value = report_type_id;

                param[3] = new SqlParameter("@CompanyId", SqlDbType.Int);
                param[3].Value = companyid;

                DataSet ds = SqlHelper.ExecuteDataset(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);

                return ds;

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "ViewReportInds()", ex.Message  + ex.StackTrace);
                return null;
            }
        }

        public DataSet ViewReport(int startIndex, int numberOfRows, string sortExpressions)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = operation;

                param[1] = new SqlParameter("@ReportID", SqlDbType.Int);
                param[1].Value = ReportID;

                param[2] = new SqlParameter("@startIndex", SqlDbType.Int);
                param[2].Value = startIndex;

                param[3] = new SqlParameter("@numberOfRows", SqlDbType.Int);
                param[3].Value = numberOfRows;

                param[4] = new SqlParameter("@sortExpressions", SqlDbType.NVarChar);
                param[4].Value = sortExpressions;


                DataSet ds = SqlHelper.ExecuteDataset(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);

                return ds;
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "ViewReport()", ex.Message  + ex.StackTrace);
                return null;
            }
        }

        public void RollbackReportData(clsRegistration objclsRegistration)
        {
            operation = 8;
            SaveCustomReports(objclsRegistration);
        }

        public string PrepareNotifyPeopleList()
        {
            string result = "";
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 15;

                param[1] = new SqlParameter("@ReportID", SqlDbType.Int);
                param[1].Value = ReportID;

                DataSet ds = SqlHelper.ExecuteDataset(Connectionstring.ToString(), CommandType.StoredProcedure, "sp_Reports", param);

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            result = result + ";" + Convert.ToString(dr["vEmail"]);
                        }
                    }
                    result = result.Substring(1, result.Length - 1);
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReport.cs", "PrepareNotifyPeopleList()", ex.Message  + ex.StackTrace);
                result = "";
            }
            return result;
        }

        #endregion

        #region Destructor

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }
        #endregion
    }

}
