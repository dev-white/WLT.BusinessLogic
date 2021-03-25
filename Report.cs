using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.ErrorLog;

namespace WLT.BusinessLogic {

    public static class Non_Telerik_Report
    {





        public static string FuelData(int Reportid, int UserId, string StrTimeZoneID)
        {

            List<FuelModel> dataList = new List<FuelModel>();
            clsReports_Project cls = new clsReports_Project();
            DataSet TempDS = new DataSet();
            TempDS = cls.Get_AnalogReport(StrTimeZoneID, UserId, 50, Reportid, 2, 0,1);




            //Get logo

            string base64String = null;
            if (TempDS.Tables[3].Rows.Count > 0)
            {
                byte[] bytes = (byte[])TempDS.Tables[3].Rows[0]["vLogo"];
                byte[] imageSource = bytes;
                base64String = Convert.ToBase64String(imageSource);

            }


            try
            {
                if (TempDS.Tables.Count == 0 || TempDS.Tables[1].Rows.Count == 0 || TempDS.Tables[0].Rows.Count == 0)
                {
                    dataList.Add(new FuelModel { errorFlag = 1 });
                }
                else
                {
                    var row = TempDS.Tables[3].Rows[0];
                    dataList.Add(new FuelModel { startDate = Convert.ToDateTime(row["startDate"]).ToString("dd MMM, yyyy HH:mm:ss"), endDate = Convert.ToDateTime(row["endDate"]).ToString("dd MMM, yyyy HH:mm:ss"), Asset = (string)TempDS.Tables[3].Rows[0]["AssetName"], Logo = base64String, max = (double)TempDS.Tables[1].Rows[0]["iMaxValue"], min = (double)TempDS.Tables[1].Rows[0]["iMinValue"], DateOfQuery = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, StrTimeZoneID).ToString("dd-MMM-yyyy HH:mm"), analogType = Convert.ToString(TempDS.Tables[0].Rows[0]["vName"]), Title = Convert.ToString(TempDS.Tables[0].Rows[0]["vUnitText"]) });
                }
                foreach (DataRow row in TempDS.Tables[2].Rows)
                {

                    dataList.Add(new FuelModel { FuelData = Convert.ToDouble(row["vAnalog1"]), Date = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["dGPSDateTime"]), StrTimeZoneID) });



                }
                if (dataList.Count == 1)
                {


                    dataList.Add(new FuelModel { FuelData = null, Date = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, StrTimeZoneID) });


                }

                DataTable dts = new DataTable();
                dts.Columns.Add("DataFuel", typeof(double));
                dts.Columns.Add("date", typeof(DateTime));
                dataList = dataList.OrderBy(x => x.Date).ToList();
            }
            catch (Exception ex) {

                LogError.RegisterErrorInLogFile( "Non_Telerik_Report.cs", "FuelData()", ex.Message  + ex.StackTrace);

            }
            string DataList = JsonConvert.SerializeObject(dataList);
            return JsonConvert.SerializeObject(dataList);

        }
    }
    

    public class Bal_Historic_Geozone_Scan_Report
    {
        public EntityLayer.El_Report _El_Report { get; set; }
        public Bal_Historic_Geozone_Scan_Report()
        {

        }

        public EntityLayer.El_Report GetViolatingDevicesDetails()
        {
            var _dal_report = new DAL_Reports();

          var _reportData =  _dal_report.Report_Historic_Geozone_Scan(_El_Report);

            Header(_reportData);

            Body(_reportData);

            return _El_Report;

        }

        public void Header(DataSet _ds)
        {
            if (_ds.Tables.Count >1)
            {
                foreach (DataRow dr in _ds.Tables[1].Rows)
                {
                    _El_Report.ReportName = Convert.ToString(dr["vReportName"]);
                    _El_Report.AssetName = Convert.ToString(dr["vAsset"]);
                    _El_Report.dStartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( Convert.ToDateTime(dr["FirstDate"]), _El_Report.TimeZoneID);
                    _El_Report.dEndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["LastDate"]), _El_Report.TimeZoneID);
                    _El_Report.CompanyLogo = Convert.ToString(dr["vLogo"]);
                    _El_Report._EL_DatesFilter = JsonConvert.DeserializeObject<EL_DatesFilter>(Convert.ToString(dr["timeRange"]));
                    _El_Report._EL_DatesFilter.iTimeFilterType = Convert.ToInt32(dr["iEnabledDateType"]);
                    _El_Report._EL_DatesFilter.bAllowFilter = Convert.ToBoolean(dr["isCustomTimeEnabled"]);

                }
            }
        }

        public void Body(DataSet _ds)
        {
            var _dt = new DataTable();

            if (_ds.Tables.Count >0)
            {
                _El_Report.DataSource = _ds.Tables[0];

                //foreach (DataRow dr in _ds.Tables[0].Rows)
                //{

                //}
            }
        }
    } 

}