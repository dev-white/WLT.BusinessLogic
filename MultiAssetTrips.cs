using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using Newtonsoft.Json;
using System.Data;
using WLT.BusinessLogic.Bal_GPSOL;

namespace WLT.BusinessLogic
{
    public class MultiAssetTrips
    {


        public El_Device Devices { get; set; }

        public List<El_MultiAssetTrips> TripsArray { get; set; }
        public string TimeZoneID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public MultiAssetTrips()
        {

            TripsArray = new List<El_MultiAssetTrips>();
        }

        public List<El_MultiAssetTrips> GetMultiAssetTrips()
        {
            var listDevices = JsonConvert.DeserializeObject<List<long>>(Devices.vpkDeviceIDCSV);

            var TripObject = new Trips();

            var csv = string.Join(",", listDevices.ToArray());

            var dirtyTripsDataSet = TripObject.GetDirtyTripSummaryForMultipleAssets(csv, startDate, endDate, TimeZoneID);

            foreach (var vpkDeviceID in listDevices)
            {
                var rawDeviceSpecificData = new DataTable();

                var FinalList = new List<clsPopulateTripSummary>();

                if (dirtyTripsDataSet.Tables.Count > 0)
                {
                    rawDeviceSpecificData = dirtyTripsDataSet.Tables[0].Copy();

                    rawDeviceSpecificData.DefaultView.RowFilter = "vpkDeviceID  =" + vpkDeviceID;

                    rawDeviceSpecificData = rawDeviceSpecificData.DefaultView.ToTable();

                    var ds = new DataSet();

                    ds.Tables.Add(rawDeviceSpecificData);

                    var CleansummeryList = new List<clsPopulateTripSummary>();

                    CleansummeryList = TripObject.CleanDirtyTripSummary(ds, Devices.CompanyId, vpkDeviceID, startDate, endDate, TimeZoneID, false);

                    FinalList = TripObject.RemoveZeroDistanceFromCleanedTripSummary(0.01, CleansummeryList, TimeZoneID);



                    foreach (var item in FinalList)
                    {
                        var _TripsTelemetry = new Bal_TripsTelemetry();

                        _TripsTelemetry.StartDate = item.StartDate;

                        _TripsTelemetry.EndDate = item.EndDate;

                        _TripsTelemetry.TimezoneID = TimeZoneID;

                        _TripsTelemetry.vpkDeviceID = item.VpkDeviceID;

                        item.Telemetry = _TripsTelemetry.GetTelemetry();
                    }



                }

                var _El_MultiAssetTrips = new El_MultiAssetTrips()
                {

                    Trips = FinalList.OrderBy(n => n.StartDate),
                    vpkDeviceID = vpkDeviceID
                };

                TripsArray.Add(_El_MultiAssetTrips);
            }

            return TripsArray;
        }

        public List<El_MultiAssetTrips> GetMultiAssetTrails()
        {
            var listDevices = JsonConvert.DeserializeObject<List<long>>(Devices.vpkDeviceIDCSV);

            var TripObject = new Trips();

            var csv = string.Join(",", listDevices.ToArray());

            var dirtyTripsDataSet = TripObject.GetDirtyTripSummaryForMultipleAssets(csv, startDate, endDate, TimeZoneID);

            foreach (var vpkDeviceID in listDevices)
            {
                var rawDeviceSpecificData = new DataTable();

                var FinalList = new List<clsPopulateTripSummary>();

                if (dirtyTripsDataSet.Tables.Count > 0)
                {
                    rawDeviceSpecificData = dirtyTripsDataSet.Tables[0].Copy();

                    rawDeviceSpecificData.DefaultView.RowFilter = "vpkDeviceID  =" + vpkDeviceID;

                    rawDeviceSpecificData = rawDeviceSpecificData.DefaultView.ToTable();

                    var ds = new DataSet();

                    ds.Tables.Add(rawDeviceSpecificData);

                    var CleansummeryList = new List<clsPopulateTripSummary>();

                    CleansummeryList = TripObject.CleanDirtyTripSummary(ds, Devices.CompanyId, vpkDeviceID, startDate, endDate, TimeZoneID, false);

                    FinalList = TripObject.RemoveZeroDistanceFromCleanedTripSummary(0.01, CleansummeryList, TimeZoneID);

                    var StartDate = new DateTime();
                    var EndDate = new DateTime();

                    Boolean IsInProgress = false;
                    
                    foreach (var item in FinalList)
                    {
                      
                        StartDate = item.StartDate;

                        EndDate = item.EndDate;                        

                        IsInProgress = item.IsInProgress;

                        break;
                    }

                    if (FinalList.Count > 0)
                    {                    

                        var TripsTelemetry = new Bal_TripsTelemetry();

                        TripsTelemetry.StartDate = StartDate;

                        TripsTelemetry.EndDate = EndDate;

                        TripsTelemetry.TimezoneID = TimeZoneID;

                        TripsTelemetry.vpkDeviceID = vpkDeviceID;


                        var _El_MultiAssetTrips = new El_MultiAssetTrips()
                        {

                            Telemetry = TripsTelemetry.GetTelemetry(),
                            vpkDeviceID = vpkDeviceID,
                            IsInProgress = IsInProgress

                        };

                        TripsArray.Add(_El_MultiAssetTrips);

                    }

                }


            }

            return TripsArray;
        }
    }
}
