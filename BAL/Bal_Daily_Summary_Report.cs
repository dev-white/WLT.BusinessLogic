using System;
using System.Collections.Generic;
using System.Data;
using Whitelabeltracking.DataAccessLayer.DAL;
using System.Text;
using WLT.EntityLayer;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer.DAL;

namespace Whitelabeltracking.BusinessLogic.BAL
{
   public  static class Bal_Daily_Summary_Report
    {
        public static List<clsPopulateTripSummary> Trips_lst = new List<clsPopulateTripSummary>();
        public static DateTime _startDate;
        public static DateTime _endDate;        
        public static int _ClientId;
        public static string _Assetname;
        public static string _TimeZoneID;



        public static  Tuple<object, List<ClsPopulateTripSummaryExtended>> GetDailySummaryInfo(string TimeZoneID, int ReportId, int UserId)
        {
            //return  DataAccessLayer.DAL.DAL_DailySummaryReport.GetDailySummaryInfo(TimeZoneID, ReportId, UserId);

            var _TripsObject = new Trips();

            //get devices deviceCsv 
            var _devicesCSVObject = DAL_DailySummaryReport.FetchAssetOverallData(ReportId, UserId);


            var tbl1Header = _devicesCSVObject.Tables[0];

            var _logo = _devicesCSVObject.Tables[1].Rows[0]["vLogo"];

            // get parameters 
            _startDate = Convert.ToDateTime(tbl1Header.Rows[0]["StartDate"]);
             _endDate = Convert.ToDateTime(tbl1Header.Rows[0]["EndDate"]);
            _ClientId = Convert.ToInt32(tbl1Header.Rows[0]["CompanyId"]);
            _Assetname = Convert.ToString(tbl1Header.Rows[0]["AssetName"]);
            _TimeZoneID = TimeZoneID;

            //get assets 

            var _deviceSVC =  CreateDeviceSVC(_devicesCSVObject.Tables[2]);

            //get raw trips 
            var _rawDataTrips = _TripsObject.GetDirtyTripSummaryForMultipleAssets(_deviceSVC.Item1, _startDate, _endDate, TimeZoneID);

            //filter the trips per asset
          

            var _filteredData = GetTripsList(_rawDataTrips.Tables[0].Copy(), _deviceSVC.Item2);

            
            return new Tuple<object, List<ClsPopulateTripSummaryExtended>>(_logo, _filteredData);

        }


        //Spit Out list 

        private static List<ClsPopulateTripSummaryExtended> GetTripsList(DataTable dt, List<Asset> Assets)
        {
            var _TripsObject = new Trips();

            var _ClsPopulateTripSummaryExtended = new List<ClsPopulateTripSummaryExtended>();

            foreach ( var Asset in Assets) {

                var _AssetSpecificData = dt.Select("vpkDeviceID = " + Asset.vpkDeviceID);


                if (_AssetSpecificData.Length > 0)
                {
                    var _ds = new DataSet();

                    //convert Rows to Datatable
                    _ds.Tables.Add(_AssetSpecificData.CopyToDataTable());

                    //create trips                   

                    var _cleanTrips = _TripsObject.CleanDirtyTripSummary(_ds, _ClientId, Asset.vpkDeviceID, _startDate, _endDate, _TimeZoneID, true);

                    var counter = _cleanTrips.Count;
                    if (counter > 0)
                    {


                        _ClsPopulateTripSummaryExtended.Add(new ClsPopulateTripSummaryExtended
                        {

                            Device_Name = Asset.DeviceName,
                            StartDate = _cleanTrips [0].StartDate,
                            EndDate = _cleanTrips[counter-1].EndDate,
                            Duration = _cleanTrips[counter - 1].EndDate.Subtract(_cleanTrips[0].StartDate),
                            Distance = _cleanTrips[counter - 1].EndOdometer - _cleanTrips[0].StartOdometer,
                            LocationStart = _cleanTrips[0].LocationStart,
                            LocationEnd = _cleanTrips[counter - 1].LocationEnd,
                            IsMain = 1


                        });


                        foreach (var parent in _cleanTrips)
                        {
                            _ClsPopulateTripSummaryExtended.Add(new ClsPopulateTripSummaryExtended
                            {

                                Device_Name = Asset.DeviceName,
                                StartDate = parent.StartDate,
                                EndDate = parent.EndDate,
                                Duration = parent.Duration,
                                Distance = parent.Distance,
                                LocationStart = parent.LocationStart,
                                LocationEnd = parent.LocationEnd,
                                IsMain = 0


                            });

                        }
                    }
                    

                }
                
            }



            return _ClsPopulateTripSummaryExtended;
        }

        //get csv
        private  static Tuple<string,List<Asset>> CreateDeviceSVC( DataTable dt) {


            var _deviceSVC = new StringBuilder();
            var _vpkDeviceIDList = new List<Asset>();
            foreach (DataRow item in dt.Rows)
            {

                _deviceSVC.Append(Convert.ToString(item["vpkDeviceID"]) + ",");

                _vpkDeviceIDList.Add( new Asset {  vpkDeviceID =Convert.ToInt64(item["vpkDeviceID"]), DeviceName =Convert.ToString(item["vDeviceName"]) });


            }

            _deviceSVC.Append("0");

            return  new Tuple<string ,List<Asset>>(_deviceSVC.ToString(),_vpkDeviceIDList);
        }

      

    }

    public class ClsPopulateTripSummaryExtended : clsPopulateTripSummary
    {
        //Note we do not have a copy of an ExternalClass object here.
        //This class itself will now have all of its instance members.
        public int IsMain { get; set; }
        public string Trips { get; set; }

        //Base will call the constructor for the inherited class.
        //If it has parameters include those parameters in NewClass() and add them to base().
        //This is important so we don't have to write all the properties ourself.
        //In some case}s it's even impossible to write to those properties making this approach mandatory.
        public ClsPopulateTripSummaryExtended() : base()
        {

        }
    }
        public class Asset
        {
            public String DeviceName { get; set; }
            public long vpkDeviceID { get; set; }
        public Asset() {

        }

    }
    }

