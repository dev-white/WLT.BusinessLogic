using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic
{
    public class Bal_TripsTelemetry
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long vpkDeviceID { get; set; }
        public string TimezoneID { get; set; }        

        public List<El_MultiAssetTrips> GetTelemetry()
        {
            var ds = new DataSet();

            var _DAL_TripsTelemetry = new DAL_TripsTelemetry();

            StartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(StartDate, TimezoneID);

            EndDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(EndDate, TimezoneID);

            var resultTelDataset = _DAL_TripsTelemetry.GetTelemetry(StartDate, EndDate, vpkDeviceID);

            var TelemetryArray = new List<El_MultiAssetTrips>();

            if (resultTelDataset.Tables.Count > 0)
            {
                foreach (DataRow record in resultTelDataset.Tables[0].Rows)
                {

                    TelemetryArray.Add(new El_MultiAssetTrips
                    {
                        Longitude = Convert.ToDouble(record["vLongitude"]),
                        Latitude = Convert.ToDouble(record["vLatitude"]),
                        gpsDatetime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(record["dGPSDateTime"]), TimezoneID),
                        TextLocation = Convert.ToString(record["vTextMessage"]),
                        ignitionStatus = Convert.ToBoolean(record["ignitionStatus"]),
                        odometer = Convert.ToString(record["odometer"]),
                        speed = Convert.ToString(record["speed"]),
                        Heading = Convert.ToDouble(record["vHeading"]),
                        Roadspeed = Convert.ToDouble(record["vRoadSpeed"]),
                        ifkDriverID = Convert.ToInt32(record["ifkDriverID"]),



                    });
                }

            }


            return TelemetryArray;

        }
    }
}
