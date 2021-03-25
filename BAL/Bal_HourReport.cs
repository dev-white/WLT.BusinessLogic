using System;
using System.Data;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
    public static class Bal_HourReport
    {


        public static cls_Report GetHourReport(El_Report el_report)
        {
            var _DAL_Reports = new DAL_Reports();

            var _cls_Report = new cls_Report();

            var  data = _DAL_Reports.GetHourReport(el_report);

             Header(_cls_Report,data, el_report.TimeZoneID);

            _cls_Report.DataSource = Body(data,el_report.TimeZoneID);

            return  _cls_Report;
        }


       private static void Header(cls_Report _report, DataSet ds,string TimeZoneID)
        {
            var rows = ds.Tables[1].Rows;

            foreach (DataRow row in rows)
            {
                _report.StartTime = Convert.ToDateTime(row["FirstDate"]);
                _report.EndTime = Convert.ToDateTime(row["LastDate"]);
                _report.ReportName = Convert.ToString(row["vReportName"]);
                _report.AssetName = Convert.ToString(row["vAsset"]);

                DateTime currentdate = DateTime.UtcNow;
                DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(currentdate), "UTC", TimeZoneID);

                _report.GenaratedDate = dt;

            }
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                _report.CompanyLogo = Convert.ToString(row["vLogo"]);

            }
        }
        private static DataTable Body(DataSet _ds,string TimeZoneID)
        {
            var dt = new DataTable("tbl");
            dt.Columns.Add("AssetName", typeof(string));
            dt.Columns.Add("startHours", typeof(DateTime));
            dt.Columns.Add("endHours", typeof(DateTime));
            dt.Columns.Add("TotalHours", typeof(double));
            dt.Columns.Add("LogoName", typeof(string));
            dt.Columns.Add("vpkDeviceID", typeof(long));
            dt.Columns.Add("OvarallHours", typeof(long));


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

                    dt.Rows.Add(_drSource);
                }
            }
            return dt;
        }

    }

    public class cls_Report {
          public  DataTable DataSource { get; set; }
          public string  AssetName { get; set; }
          public string ReportName { get; set; }
          public DateTime GenaratedDate { get; set; }
          public DateTime StartTime { get; set; }
          public DateTime EndTime { get; set; }
        public int MeasurementId { get; set; }

        public string Unit { get; set; }
        public string CompanyLogo { get; set; }

    }
}
