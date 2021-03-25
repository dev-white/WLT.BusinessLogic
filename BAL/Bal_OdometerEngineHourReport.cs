using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
   public class Bal_OdometerEngineHourReport
    {
        public static cls_Report GetOdomerEngineHourReport(El_Report el_report)
        {
            var _cls_Report = new cls_Report();

            try
            {
                var _DAL_Reports = new DAL_Reports();

                var data = _DAL_Reports.GetOdometerEngineHourReport(el_report);

                Header(_cls_Report, data, el_report.TimeZoneID);

                _cls_Report.DataSource = Body(data, el_report.TimeZoneID, _cls_Report);
            }
            catch (Exception ex)
            {

            }
         
         

            return _cls_Report;
        }


        private static void Header(cls_Report _report, DataSet ds, string TimeZoneID)
        {
            var rows = ds.Tables[1].Rows;

            foreach (DataRow row in rows)
            {
                _report.StartTime = Convert.ToDateTime(row["FirstDate"]);
                _report.EndTime = Convert.ToDateTime(row["LastDate"]);
                _report.ReportName = Convert.ToString(row["vReportName"]);
                _report.AssetName = Convert.ToString(row["vAsset"]);
                _report.MeasurementId = Convert.ToInt32(row["ifkMeasurementUnit"]);
                

                DateTime currentdate = DateTime.UtcNow;
                DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(currentdate), "UTC", TimeZoneID);

                _report.GenaratedDate = dt;

            }
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                _report.CompanyLogo = Convert.ToString(row["vLogo"]);

            }
        }
        private static DataTable Body(DataSet _ds, string TimeZoneID, cls_Report _cls_Report )
        {

            //= Fields.

            var dt = new DataTable("tbl");
            dt.Columns.Add("AssetName", typeof(string));
            dt.Columns.Add("startHours", typeof(DateTime));
            dt.Columns.Add("endHours", typeof(DateTime));
            dt.Columns.Add("TotalHours", typeof(double));
            dt.Columns.Add("LogoName", typeof(string));
            dt.Columns.Add("vpkDeviceID", typeof(long));
            dt.Columns.Add("OvarallHours", typeof(long));
            dt.Columns.Add("Distance_Covered", typeof(double));
            dt.Columns.Add("Lifetime_Covered", typeof(double));




            _cls_Report.Unit = UserSettings.GetOdometerUnitName(_cls_Report.MeasurementId);

            if (_ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in _ds.Tables[0].Rows)
                {
                    var _drSource = dt.NewRow();

                    var startDate = new DateTime(Convert.ToInt64(dr["startHours"]));
                    var endDate = new DateTime(Convert.ToInt64(dr["endHours"]));

                    _drSource["AssetName"] = Convert.ToString(dr["AssetName"]);
                    _drSource["startHours"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(startDate, TimeZoneID);
                    _drSource["endHours"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(endDate, TimeZoneID);
                    _drSource["TotalHours"] = Convert.ToDouble(dr["TotalHours"]);
                    _drSource["LogoName"] = Convert.ToString(dr["LogoName"]);
                    _drSource["OvarallHours"] = Convert.ToDouble(dr["endHours"]);
                    _drSource["Distance_Covered"] = UserSettings.ConvertKMsToXxOdoMeter(_cls_Report.MeasurementId, Convert.ToString(dr["OdometerDifference"]),false,2);
                    _drSource["Lifetime_Covered"] = UserSettings.ConvertKMsToXxOdoMeter(_cls_Report.MeasurementId, Convert.ToString(dr["CurrentOdometerReading"]), false, 2);

                  
              



                    dt.Rows.Add(_drSource);
                }
            }
            return dt;
        }

    }
}

