using Microsoft.AspNetCore.Mvc;
using  Newtonsoft.Json;
using System.Linq.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer;
using WLT.DataAccessLayer.GPSOL;
using WLT.EntityLayer;
using WLT.EntityLayer.Reports;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using ServiceStack.Text;
using WLT.BusinessLogic.Util;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_Reports
    {

        public ClsReport SaveCustomReports(ClsReport objReport, wltAppState clsobj)
        {
            var _resultModel = new ClsReport();
            try
            {
                _resultModel.ActivityId = 45;

                if (objReport.operation == 11)
                {
                    _resultModel.ActivityId = 46;
                }


                if (objReport.companyid == 0)
                {
                    objReport.bIsForAllCompanies = true;
                }
                else
                {
                    objReport.bIsForAllCompanies = false;
                }
                if (objReport.TriggeredEventStatus == true)
                {
                    objReport.FilterCode = objReport.TriggeredEventID;
                }
                else if (objReport.IsOverspeed == true)
                {
                    objReport.FilterCode = 2;
                }
                else if (objReport.IsTemeperatureViolation == true)
                {
                    //objclsAlert.FilterCode = 9;
                }
                else if (objReport.IsExcessiveIdle == true)
                {
                    objReport.FilterCode = 4;
                }
                else
                {
                    objReport.FilterCode = 0;
                }

                objReport.iCreatedByID = clsobj.pkUserID;
                objReport.iUpdatedByID = clsobj.pkUserID;

                _resultModel.ResponseMessage = objReport.SaveCustomReports(clsobj);



                if (_resultModel.ResponseMessage != "")
                {

                    if (objReport.report_type_id == 2)
                    {

                        foreach (ClsReport objOther in objReport.lstOtherContent)
                        {
                            objReport.operation = 5;
                            objReport.ReportID = Convert.ToInt32(_resultModel.ResponseMessage);
                            objReport.ifkDigitalMasterID = objOther.ifkDigitalMasterID;
                            objReport.bEventStatus = objOther.bEventStatus;
                            if (objReport.SaveReportOtherCriteria() == "-1")
                            {
                                objReport.RollbackReportData(clsobj);
                                return _resultModel;
                            }
                        }
                    }

                    if (objReport.IsScheduled == true)
                    {
                        string[] _strCommanSepArray = Convert.ToString(objReport.UserIDs).Split(',');
                        int _intCount = 0;
                        string _intReportId = _resultModel.ResponseMessage;
                        while (_intCount < _strCommanSepArray.Length)
                        {
                            objReport.operation = 6;
                            objReport.ReportID = Convert.ToInt32(_intReportId);
                            objReport.UserIDs = _strCommanSepArray[_intCount].Split('_')[0];

                            _resultModel.ResponseMessage = objReport.SaveReportNotifyUsers();

                            _intCount++;
                        }

                    }

                }
                else
                {
                    _resultModel.ResponseMessage = "Error while data saving.";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Reports", "SaveCustomReports()", ex.Message + ex.StackTrace);

                _resultModel.ResponseMessage = ex.Message + ex.StackTrace;
            }


            return _resultModel;

        }


        public El_Heatmap ReportHeatmap(El_Report _El_Report)
        {
            var _El_Heatmap = new El_Heatmap();

            try
            {
                var _Bal_Heatmap = new Bal_Heatmap();

                var operation = 0;

                _El_Heatmap = _Bal_Heatmap.Getmapinfo(operation, _El_Report.ReportId, _El_Report.UserId, _El_Report.TimeZoneID, _El_Report.CultureID);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Reports", "ReportHeatmap()", ex.Message + ex.StackTrace);
            }

            return _El_Heatmap;

        }
        public string ReportLocationProbility(El_Report el_report)
        {
            var _reportPriobabilityInstance = new wlt.businesslogic.bal.Bal_Report_Historical_Location_Probability();

            el_report.MinuteCount = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, el_report.TimeZoneID).Subtract(DateTime.UtcNow).TotalMinutes;

            var reportsdataResultSet = _reportPriobabilityInstance.GetReportDataAsync(el_report);

            return reportsdataResultSet;


        }
        public El_Report LoadSpeedingData(EL_Speeding _EL_Speeding)
        {
            var ds = new DataSet();

            var _El_Report = new El_Report();



            try
            {
                SqlParameter[] param = new SqlParameter[4];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _EL_Speeding.Operation;

                param[1] = new SqlParameter("@ReportTypeId", SqlDbType.Int);
                param[1].Value = _EL_Speeding.ipkReportTypeId;

                param[2] = new SqlParameter("@ReportId", SqlDbType.Int);
                param[2].Value = _EL_Speeding.ReportId;

                param[3] = new SqlParameter("@User_ID", SqlDbType.Int);
                param[3].Value = _EL_Speeding.UserId;

                ds = SqlHelper.ExecuteDataset(AppConfiguration.Getwlt_WebAppConnectionString(), CommandType.StoredProcedure, "sp_reports_Speeding", param);

                var _EL_DatesFilter = new EL_DatesFilter();

                var _drReportCriteria = ds.Tables[1].Rows[0];

                if (_drReportCriteria["vCustomTime"].ToString() != null && _drReportCriteria["vCustomTime"].ToString() != string.Empty)
                {
                    _EL_DatesFilter = JsonConvert.DeserializeObject<EL_DatesFilter>(_drReportCriteria["vCustomTime"].ToString());

                    _EL_DatesFilter.bAllowFilter = Convert.ToBoolean(_drReportCriteria["isCustomDateEnabled"]);

                    _EL_DatesFilter.iTimeFilterType = Convert.ToInt32(_drReportCriteria["iEnabledDateType"]);

                }

                if (ds.Tables.Count > 0)
                {
                    _El_Report.ReportDataTable = ds.Tables[0].Clone();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        DateTime dDeviceSentDate = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(row["Date"]), "UTC", _EL_Speeding.TimeZoneID));

                        row["speed"] = UserSettings.ConvertKMsToXx(_EL_Speeding.ifkMeasurementUnit, row["speed"].ToString(), false, 2);
                        row["Date"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["Date"]), _EL_Speeding.TimeZoneID);

                        if (_EL_DatesFilter.bAllowFilter)
                        {
                            var _Today = dDeviceSentDate;

                            var startDt = Convert.ToDateTime(_Today.ToString("yyyy-MM-dd ") + _EL_DatesFilter.startTime);

                            var EndDt = Convert.ToDateTime(_Today.ToString("yyyy-MM-dd ") + _EL_DatesFilter.endTime);

                            if ((_Today < startDt || _Today > EndDt) && _EL_DatesFilter.iTimeFilterType == 1)
                            {
                                continue;
                            }

                            if ((_Today >= startDt && _Today <= EndDt) && _EL_DatesFilter.iTimeFilterType == 2)
                            {
                                continue;
                            }
                            var newRow = _El_Report.ReportDataTable.NewRow();
                            newRow["speed"] = row["speed"];
                            newRow["Date"] = row["Date"];
                            _El_Report.ReportDataTable.Rows.Add(newRow);
                        }

                    }

                    if (!_EL_DatesFilter.bAllowFilter)
                        _El_Report.ReportDataTable = ds.Tables[0];

                    var _translatedReportName = ReportExtensions.TranslateReportName(_EL_Speeding.CultureID, _EL_Speeding.ipkReportTypeId);

                    foreach (DataRow row in ds.Tables[1].Rows)
                    {

                        _El_Report.ReportName = _translatedReportName ?? Convert.ToString(row["vReportName"]);

                        _El_Report.dStartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["FirstDate"]), _EL_Speeding.TimeZoneID);

                        _El_Report.dEndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["LastDate"]), _EL_Speeding.TimeZoneID);

                        _El_Report.AssetName = ReportExtensions.ChangeAssetHeaderLanguageString(_El_Report.CultureID, Convert.ToString(row["vAsset"]), 131);

                        _El_Report.CompanyLogo = Convert.ToString(row["vLogo"]);

                        _El_Report.GeneratedDate = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "UTC", _EL_Speeding.TimeZoneID));

                        _El_Report.MaxSpeed = Convert.ToDouble(UserSettings.ConvertKMsToXx(_EL_Speeding.ifkMeasurementUnit, (500).ToString(), false, 2));

                        _El_Report.UnitType = UserSettings.GetSpeedUnitName(_EL_Speeding.ifkMeasurementUnit);

                        if (_EL_DatesFilter.bAllowFilter)
                        {
                            _El_Report.dStartDate = Convert.ToDateTime(_El_Report.dStartDate.ToString("yyyy-MM-dd ") + _EL_DatesFilter.startTime);
                            _El_Report.dEndDate = Convert.ToDateTime(_El_Report.dEndDate.ToString("yyyy-MM-dd ") + _EL_DatesFilter.startTime);
                            _El_Report.txtDateTypeMessage = _EL_DatesFilter.iTimeFilterType == 1 ? "Showing data inside  time range" : "Showing data outside  time range";

                            //timeStart.Value = String.Format("Starting at: {0}", _EL_DatesFilter.startTime);

                            //timeEnd.Value = String.Format("Ending at : {0}", _EL_DatesFilter.endTime);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("HomeCtrl", "LoadSpeedingData()", ex.Message + " : " + ex.StackTrace);

            }

            return _El_Report;

        }


        public string GetReportExportFormat(long reportId)
        {
            var format = SqlHelper.ExecuteScalar(AppConfiguration.Getwlt_WebAppConnectionString(), CommandType.Text, $"select reportformat from wlt_tblreport_criteria_details where ifkreportid = {reportId}");

            if (Convert.ToString(format).ToLower() == "xls")
                format = "XLSX";

            return Convert.ToString(format).ToUpper();

        }

        public El_Report GetGeneralReportHeader(El_Report el_report)
        {

            el_report.Operation = 5;

            var _DAL_NTSA_DeviceSummary = new DAL_NTSA_DeviceSummary();

            var ds = _DAL_NTSA_DeviceSummary.GetNTSADeviceInstallationDetails(el_report);

            var reportCriteria = new El_Report();

            foreach (DataTable dt in ds.Tables)
                foreach (DataRow dr in dt.Rows)
                {
                    reportCriteria.dStartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["startDate"]), el_report.TimeZoneID);
                    reportCriteria.dEndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["endDate"]), el_report.TimeZoneID);
                    reportCriteria.CompanyLogo = Convert.ToString(dr["vLogo"]);

                    reportCriteria.ReportName = Convert.ToString(dr["ReportName"]);
                    reportCriteria.AssetName = Convert.ToString(dr["reportAssetName"]);

                    reportCriteria.GeneratedDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, el_report.TimeZoneID);

                }

            return reportCriteria;
        }

        public IEnumerable<El_NtsaViolationCommonData> GetNtsaViolationReportData(El_Report el_report)
        {

            IEnumerable<El_NtsaViolationCommonData> _ntsaViolations = new List<El_NtsaViolationCommonData>();

            LogError.RegisterErrorInLogFile("Bal_Reports", "Trying to generate violation records", "");

            try
            {
                var _DAL_NTSA_DeviceSummary = new DAL_NTSA_DeviceSummary();

                if (el_report.Operation == 0)
                    el_report.Operation = 6;

                var ds = _DAL_NTSA_DeviceSummary.GetNTSADeviceInstallationDetails(el_report);

                var tbl_count = 0;

                var resellerDevices = new List<El_NtsaViolationCommonData>();

                var violationCommonData = new List<El_NtsaViolationCommonData>();

                var _nCriteria = new El_NtsaViolationCriteria();

                var reportCriteria = new El_Report();

                foreach (DataTable dt in ds.Tables)
                {

                    if (tbl_count == 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            _nCriteria.event_ids = JsonConvert.DeserializeObject<List<int>>(Convert.ToString(dr["Violations"])); ;
                            _nCriteria.ResellerID = Convert.ToInt32(dr["ResellerId"]);
                            _nCriteria.StartDate = Convert.ToDateTime(dr["startDate"]);
                            _nCriteria.EndDate = Convert.ToDateTime(dr["endDate"]);

                        }

                    }

                    if (tbl_count == 1)
                    {

                        dt.TableName = "reseller devices ";

                        foreach (DataRow dr in dt.Rows)
                            resellerDevices.Add(new El_NtsaViolationCommonData
                            {
                                IMEI = Convert.ToInt64(dr["ImeiNumber"]),
                                Res = Convert.ToString(dr["Reseller"]),

                                ResId = Convert.ToInt32(dr["ResellerId"]),
                                isMini = Convert.ToBoolean(dr["isMiniReseller"]),
                                Reg = Convert.ToString(dr["RegNo"]),

                                //Model = Convert.ToString(dr["Model"]),
                                //Make = Convert.ToString(dr["Make"]),

                                Owner = Convert.ToString(dr["Owner"]),

                                OTel = Convert.ToString(dr["OwnerTell"]),
                                Action = Convert.ToString(dr["ActionTaken"]),


                            });

                    }

                    tbl_count += 1;
                }


                //_nCriteria.event_ids.Add(124);



                var ImeiCsv = String.Join(",", resellerDevices.Select(p => p.IMEI).Distinct());
                var eventCsv = String.Join(",", _nCriteria.event_ids.Distinct());

                var npSql = $"SELECT * FROM tracking_data where  event_id in ({eventCsv}) and gps_datetime  between '{_nCriteria.StartDate.ToString("yyyy-MM-dd HH:mm:ss")}' and '{_nCriteria.EndDate.ToString("yyyy-MM-dd HH:mm:ss")}' and imei_number  in ({ImeiCsv}) order by gps_datetime desc; ";

                var ViolationsDs = _DAL_NTSA_DeviceSummary.GetNTSADeviceInstallationDetailsPG(npSql);


                foreach (DataTable dt in ViolationsDs.Tables)
                    foreach (DataRow dr in dt.Rows)
                        violationCommonData.Add(new El_NtsaViolationCommonData
                        {
                            DVCID = Convert.ToInt64(dr["imei_number"]),
                            Date = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["gps_datetime"]), el_report.TimeZoneID),
                            EID = Convert.ToInt32(dr["event_id"]),

                            SPD = Convert.ToString(dr["speed"]),
                            LOC = Convert.ToString(dr["road"]),
                            LNG = Convert.ToString(dr["longitude"]),
                            LAT = Convert.ToString(dr["latitude"])

                        });


                var _Bal_CommonEventsLookUp = new Bal_CommonEventsLookUp();

                _ntsaViolations = from asset in resellerDevices
                                  join common_data in violationCommonData on asset.IMEI equals common_data.DVCID
                                  select new El_NtsaViolationCommonData
                                  {

                                      Date = common_data?.Date,
                                      EID = common_data?.EID,
                                      DVCID = common_data?.DVCID,
                                      //EVNT = _Bal_CommonEventsLookUp.GetCommonEventName(Convert.ToInt32(common_data.EID), 0, 0, ""),
                                      EVNT = new Bal_CommonEvents().GetCommonEventName(Convert.ToInt32(common_data.EID)),

                                      LAT = common_data?.LAT,
                                      LOC = common_data?.LOC,
                                      LNG = common_data?.LNG,
                                      SPD = common_data?.SPD,



                                      Reg = asset.Reg,
                                      IMEI = asset?.IMEI,
                                      //Make = asset.Make,
                                      //Model = asset.Model,
                                      Res = asset.Res,
                                      ResId = asset?.ResId,
                                      Owner = asset?.Owner,
                                      OTel = asset.OTel,
                                      Action = asset.Action,
                                  };



            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Reports", "GetNtsaViolationReportData()", ex.Message + " : " + ex.StackTrace);
            }


            return _ntsaViolations;
        }


        public DataTableResultSet NtsaDeviceViolationSummaryReportServerSideProcessing(object parameters, IEnumerable<El_NtsaViolationCommonData> _ViolationRecords)
        {

            var req = DataTableParameters.Get(parameters);

            var resultSet = new DataTableResultSet();

            resultSet.draw = req.Draw;

            resultSet.recordsTotal = _ViolationRecords.Count();

            resultSet.recordsFiltered = resultSet.recordsTotal;

            if (!string.IsNullOrEmpty(req.SearchValue))
                _ViolationRecords = _ViolationRecords.Where(m => m.IMEI.ToString().Contains(req.SearchValue)
                                           || m.Reg.Contains(req.SearchValue)
                                           || m.Owner.Contains(req.SearchValue)
                                           || m.OTel.Contains(req.SearchValue)
                                           || m.EVNT.Contains(req.SearchValue)
                                           || m.Action.Contains(req.SearchValue)

                                           || m.SPD.Contains(req.SearchValue)
                                           || m.LOC.Contains(req.SearchValue)
                                           || m.LNG.Contains(req.SearchValue)
                                           || m.LAT.Contains(req.SearchValue)
                                           );




            foreach (var item in req.Order)
                if (resultSet.recordsTotal > 0 && !string.IsNullOrEmpty(req.Columns[item.Value.Column].Name))
                {
                    if (req.Columns[item.Value.Column].Name.ToLower() == "date")
                        _ViolationRecords = item.Value.Direction == "ASC" ? _ViolationRecords.AsQueryable().OrderBy(p => p.Date) : _ViolationRecords.AsQueryable().OrderByDescending(p => p.Date);
                    else
                        _ViolationRecords = _ViolationRecords.AsQueryable().OrderBy($"{req.Columns[item.Value.Column].Name} {item.Value.Direction}");
                }



            _ViolationRecords = _ViolationRecords.Skip(req.Start).Take(req.Length);

            var i = 0;

            foreach (var record in _ViolationRecords)
            {
                i += 1;

                var columns = new List<string>();

                if (req.CustomText == "ntsa")
                {
                    columns.Add(record.Date?.ToString("yyyy MMM dd")); //0
                    columns.Add(record.Date?.ToString("HH:mm:ss")); //1
                    columns.Add(record.EVNT); //2                              
                    columns.Add(record.LNG); //3
                    columns.Add(record.LAT); //4
                    columns.Add(record.LOC); //5
                    columns.Add(record.SPD); //6

                }
                else
                {

                    columns.Add((i).ToString()); //0
                    columns.Add(record.IMEI.ToString()); //1
                    columns.Add(record.Reg); //2
                    columns.Add(record.Owner); //3
                    columns.Add(record.OTel); //4
                    columns.Add(record.EVNT); //5
                    columns.Add(record.Date?.ToString("yyyy MM dd HH:mm:ss")); //6
                    columns.Add(record.Action); //7
                }

                resultSet.data.Add(columns);




            }


            return resultSet;
        }



        public Hashtable GetNtsaDeactivationData(El_Report _El_Report)
        {
            var resultDataSet = new Hashtable();

            try
            {
                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _El_Report.Operation;

                param[1] = new SqlParameter("@ipkReportId", SqlDbType.Int);
                param[1].Value = _El_Report.ReportId;

                param[2] = new SqlParameter("@UserId", SqlDbType.Int);
                param[2].Value = _El_Report.UserId;

                var ds = SqlHelper.ExecuteDataset(AppConfiguration.Getwlt_WebAppConnectionString(), CommandType.StoredProcedure, "sp_report_ntsa_deactivation", param);

                var _EL_DatesFilter = new EL_DatesFilter();


                var dt_count = 0;

                var _reportCriteriaReport = new El_Report();

                foreach (DataTable dt in ds.Tables)
                {
                    if (dt_count == 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            _reportCriteriaReport.ReportName = Convert.ToString(dr["ReportName"]);
                            _reportCriteriaReport.GeneratedDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, _El_Report.TimeZoneID);
                            _reportCriteriaReport.dStartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["startDate"]), _El_Report.TimeZoneID);
                            _reportCriteriaReport.dEndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["endDate"]), _El_Report.TimeZoneID);
                            _reportCriteriaReport.AssetName = Convert.ToString(dr["reportAssetName"]);
                            _reportCriteriaReport.CompanyLogo = Convert.ToString(dr["vlogo"]);
                        }
                    }


                    if (dt_count == 1)
                    {
                        foreach (DataRow dr in dt.Rows)
                            dr["deactivation_date"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["deactivation_date"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(dr["deactivation_date"])), _El_Report.TimeZoneID);


                        resultDataSet["_deactivationResultSet"] = dt;
                    }


                    dt_count += 1;
                }


                resultDataSet["_reportCriteriaReport"] = _reportCriteriaReport;

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Reports", "GetNtsaDeactivationData()", ex.Message + " : " + ex.StackTrace);

            }

            return resultDataSet;

        }


        public El_ReportExportResult NtsaViolationsReportExport(El_Report _El_Report, IEnumerable<El_NtsaViolationCommonData> _ViolationsRecords = null)
        {
            var _Bal_Reports = new Bal_Reports();

            var reportResult = new El_ReportExportResult();

            var reportDataSet = _ViolationsRecords ?? _Bal_Reports.GetNtsaViolationReportData(_El_Report);

            reportResult.ReportCriteria = _Bal_Reports.GetGeneralReportHeader(_El_Report);

            var _recordCount = reportDataSet.Count();

            var threshHold = 200000;

            if (_El_Report.isChunked && _recordCount > threshHold)
            {

                var remainder = _recordCount;

                var i = 0;

                while (remainder > 0)
                {
                    var chunkDataSet = new List<El_NtsaViolationCommonData>();

                    if (remainder < threshHold)
                        chunkDataSet = reportDataSet.Skip(i).Take(remainder).ToList();
                    else
                        chunkDataSet = reportDataSet.Skip(i).Take(threshHold).ToList();


                    // string csv = CsvSerializer.SerializeToCsv(chunkDataSet);

                    var csv = CSV.Get(chunkDataSet);


                    var csvBytes = new UTF8Encoding().GetBytes(csv);

                    reportResult.ReportResults.Add(new FileContentResult(csvBytes, "text/csv")
                    {
                        FileDownloadName = _El_Report.ReportName,
                    });


                    //iterating variables
                    remainder -= threshHold;

                    i += threshHold;
                }
            }
            else
            {
                //string csv = CsvSerializer.SerializeToCsv(reportDataSet);

                var csv = CSV.Get(reportDataSet);

                var csvBytes = new UTF8Encoding().GetBytes(csv);

                reportResult.ReportResult = new FileContentResult(csvBytes, "text/csv")
                {
                    FileDownloadName = _El_Report.ReportName,
                };
            }

            reportResult.exportFormat = ".csv";

            return reportResult;
        }

        public El_ReportExportResult ReportsProfileChecker(El_Report _El_Report)
        {
            var lstReportsCustom = new int[] { 139 };

            if (!lstReportsCustom.Any(p => p == _El_Report.ReportTypeID))
                return new El_ReportExportResult { };

            var reportResult = new El_ReportExportResult();

            if (_El_Report.ReportTypeID == 139)
            {
                reportResult = NtsaViolationsReportExport(_El_Report);
            }


            reportResult.IsCustomGenerated = true;

            return reportResult;

        }


        public List<El_Event> GetReportEvents()
        {

            var events = new List<El_Event>();

            try
            {
                var _DAL_Reports = new DAL_Reports();             

                var ds = _DAL_Reports.GetReportEvents();

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        events.Add(new El_Event { Name = Convert.ToString(dr["vEventName"]), Id = Convert.ToInt32(dr["ipkCommonEventLookupId"])});
                    }

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Reports", "GetReportEvents()", ex.Message + ex.StackTrace);
            }

            return events;

        }
    }
}
