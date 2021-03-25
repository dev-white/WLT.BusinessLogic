using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic
{
    public class Bal_Battery
    {

        public EL_Battery GetBatteryLowStatus(EL_Battery ReportSource)
        {
            var ds = DAL_Battery.GetBatteryLowStatus(ReportSource.Report);

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                var dt = ds.Tables[i];

                if (i == 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        ReportSource.Report.CompanyLogo = Convert.ToString(dr["vLogo"]);
                        ReportSource.Report.ReportName = Convert.ToString(dr["vReportName"]);
                        ReportSource.Report.AssetName = Convert.ToString(dr["vAsset"]);
                        ReportSource.Report.BatteryCharge = Convert.ToDouble(dr["BatteryCharge"]);                       
                        ReportSource.BatterySource = Convert.ToString(dr["selected_battery_source"]);


                        if (dr["showZeroDistance"].ToString() != null && dr["showZeroDistance"].ToString() != string.Empty)
                        {
                            ReportSource.Report.ShowZeroDistance = Convert.ToBoolean(dr["showZeroDistance"]);
                        }

                        if (dr["TimeRange"].ToString() != null && dr["TimeRange"].ToString() != string.Empty)
                        {
                            var _filter_O = JsonConvert.DeserializeObject<EL_DatesFilter>(dr["TimeRange"].ToString());
                            _filter_O.bAllowFilter = Convert.ToBoolean(dr["isCustomTimeEnabled"]);
                            _filter_O.iTimeFilterType = Convert.ToInt32(dr["iEnabledDateType"]);
                            ReportSource._EL_DatesFilter = _filter_O;


                        }
                        else
                        {
                            ReportSource._EL_DatesFilter = new EL_DatesFilter();
                        }

                        var _Devices = Convert.ToString(dr["devices"]);

                    }

                }
                if (i == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                        dr["last_reported_date"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["last_reported_date"]), ReportSource.Report.TimeZoneID);

                    ReportSource.RawBatterInfo = dt.Copy();

                }
            }


            return ReportSource;

        }

    }
}
