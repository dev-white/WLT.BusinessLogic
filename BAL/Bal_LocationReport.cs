using System;
using System.Data;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic.BAL
{
  public  class Bal_LocationReport
    {
        public int UserId { get; set; }
        public int ifkReportId { get; set; }

        public string TimezoneID { get; set; }

        public int MeasurementId { get; set; }

        public clsLocationReport _report = new clsLocationReport();

        public clsLocationReport GetrawLocationData()
        {
            var _dal_LocationReport = new DAL_LocationReport();

            var tblRowData = _dal_LocationReport.GetLocationReportData(ifkReportId, UserId);                     

             DataSource(tblRowData);          

            return _report;
        }

        public  void DataSource(DataSet _ds)
        {
            var dt = new DataTable();


            dt.Columns.Add("Asset", typeof(string));

            dt.Columns.Add("Location", typeof(string));

            dt.Columns.Add("Odometer", typeof(string));

            dt.Columns.Add("IgnitionStatus", typeof(bool));

            dt.Columns.Add("Latitude", typeof(double));

            dt.Columns.Add("Longitude", typeof(double));

            dt.Columns.Add("Speed", typeof(string));

            dt.Columns.Add("LastReported", typeof(DateTime));


            if (_ds.Tables.Count > 0) {


                if (_ds.Tables[1].Rows.Count > 0)
                {
                    MeasurementId = Convert.ToInt32(_ds.Tables[1].Rows[0]["measurement_code"]);

                    var _hdt = _ds.Tables[1];
                    foreach (DataRow _dr in _hdt.Rows)
                    {
                        _report.GeneratedDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, TimezoneID);
                        _report.StartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_dr["FirstDate"]), TimezoneID);
                        _report.EndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_dr["LastDate"]), TimezoneID);
                        _report.Logo = Convert.ToString(_dr["vLogo"]);
                        _report.ReportName = Convert.ToString(_dr["vReportName"]);
                        _report.Asset = Convert.ToString(_dr["vAsset"]);
                    }
                }

                if (_ds.Tables[0].Rows.Count > 0)
                {
                    var _rows = _ds.Tables[0].Rows;

                    foreach (DataRow row in _rows)
                    {
                        var dr = dt.NewRow();
                        dr["Asset"] = Convert.ToString(row["Asset"]);
                        dr["Location"] = Convert.ToString(row["Location"]);
                        dr["Odometer"] = Convert.ToDouble(row["Odometer"]);
                        dr["IgnitionStatus"] = Convert.ToBoolean(row["IgnitionStatus"]);
                        dr["Latitude"] = Convert.ToDouble(row["Latitude"]);
                        dr["Longitude"] = Convert.ToDouble(row["Longitude"]);
                        dr["Speed"] =  UserSettings.ConvertKMsToXx(MeasurementId, Convert.ToDouble(row["speed"]).ToString(),true,2);
                        dr["LastReported"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( Convert.ToDateTime(row["dGPSDateTime"]),TimezoneID);
                        dt.Rows.Add(dr);
                    }

                    _report.Datasource = dt;

                }

             

            }

            
        }

        
    }
    public class clsLocationReport
    {
     

        public DataTable Datasource { get; set; }       
        public  DateTime GeneratedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Asset { get; set; }
        public string Logo { get; set; }
        public string ReportName { get; set; }

    }
}
