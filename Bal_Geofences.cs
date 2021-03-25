using Hangfire.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using WLT.EntityLayer;
using System.Linq;
using System.Text;
using WLT.DataAccessLayer.DAL;
using Newtonsoft.Json;
using System.Dynamic;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.EntityLayer.Utilities;
using System.Reflection.Metadata.Ecma335;

namespace WLT.BusinessLogic
{
    public class Bal_Geofences
    {

        public Dictionary<long, EL_GeofenceItem> GeofenceItem { get; set; } = new Dictionary<long, EL_GeofenceItem>();

        public Dictionary<int, int> dctGeofenceEventIds { get; set; }
        public EL_ReportSource ReportSource { get; set; } = new EL_ReportSource();
        public Bal_Geofences(El_Report _El_Report)
        {
            dctGeofenceEventIds = GetGeofenceEventIds();

            ReportSource = new EL_ReportSource();

            ReportSource.Report = _El_Report;
        }
        public static List<TTo> Cast<TFrom, TTo>(List<TFrom> fromlist)
       where TFrom : class
         where TTo : class
        {
            return fromlist.ConvertAll(x => JsonConvert.DeserializeObject<TTo>(JsonConvert.SerializeObject(x)));
        }
        public List<EL_GeofenceRawEntry> SplitZonesTreversalPerTrip(clsPopulateTripSummary _trip, EL_DeviceZonesSummaryEntry _EL_DeviceZonesSummaryEntry)
        {
            var _lstEL_GeofenceItem = new List<EL_GeofenceRawEntry>();





            foreach (var items in _EL_DeviceZonesSummaryEntry.EL_GeofenceItem)
            {
                var ParticularZoneItems = items.Value;

                var defaultDate = Convert.ToDateTime("01/01/0001");

                var startdate = Convert.ToDateTime("2020-05-14 06:45:25");

                foreach (var ProcessedZonesDataItem in ParticularZoneItems.ProcessedZonesDataItems)
                {
                    var _EL_GeofenceRawEntry = new EL_GeofenceRawEntry();

                    _EL_GeofenceRawEntry.zone_id = ProcessedZonesDataItem.zone_id;

                    _EL_GeofenceRawEntry.zone_name = ProcessedZonesDataItem.zone_name;



                    ///
                    ///  make sure that the zones entries aren within the bounds of this trip  before looping on 
                    ///

                    if (_trip.StartDate <= ProcessedZonesDataItem.start_date && ProcessedZonesDataItem.start_date <= _trip.EndDate
                     || _trip.StartDate <= ProcessedZonesDataItem.end_date && ProcessedZonesDataItem.end_date <= _trip.EndDate
                     )
                    {


                        if (ProcessedZonesDataItem.start_date == default(DateTime))
                        {
                            var sds = "";
                        }
                        //
                        ///  scenerio 1
                        // if the trip is  inside the entry  and exit of location
                        if (ProcessedZonesDataItem.start_date <= _trip.StartDate && _trip.EndDate <= ProcessedZonesDataItem.end_date)
                        {
                            _EL_GeofenceRawEntry.duration_in = _trip.Duration.TotalSeconds;

                            _EL_GeofenceRawEntry.duration_out = 0;

                            _EL_GeofenceRawEntry.distance_in = _trip.Distance;

                            _EL_GeofenceRawEntry.distance_out = 0;

                            _EL_GeofenceRawEntry.InitialCoordinates = new Coordinates
                            {
                                Lat = _trip.FirstLocationLattitude,

                                Lng = _trip.FirstLocationLongtude,
                            };

                            _EL_GeofenceRawEntry.FinalCoordinates = new Coordinates
                            {
                                Lat = _trip.SecondLocationLattitude,

                                Lng = _trip.SecondLocationLongtude,
                            };

                            _lstEL_GeofenceItem.Add(_EL_GeofenceRawEntry);
                        }
                        ///  scenerio 2
                        // if the trip  started outside  the entry   but ended  before exit  of location
                        if (_trip.StartDate < ProcessedZonesDataItem.start_date && _trip.EndDate < ProcessedZonesDataItem.end_date)
                        {
                            _EL_GeofenceRawEntry.duration_in = _trip.EndDate.Subtract(ProcessedZonesDataItem.start_date).TotalSeconds;

                            _EL_GeofenceRawEntry.duration_out = _trip.Duration.TotalSeconds - _EL_GeofenceRawEntry.duration_in;

                            _EL_GeofenceRawEntry.distance_in = _trip.EndOdometer - ProcessedZonesDataItem.initial_odometer;

                            _EL_GeofenceRawEntry.distance_out = _trip.Distance - _EL_GeofenceRawEntry.distance_in;

                            _EL_GeofenceRawEntry.InitialCoordinates = new Coordinates
                            {
                                Lat = _trip.FirstLocationLattitude,

                                Lng = _trip.FirstLocationLongtude,
                            };

                            _EL_GeofenceRawEntry.FinalCoordinates = ProcessedZonesDataItem.FinalCoordinates;

                            _lstEL_GeofenceItem.Add(_EL_GeofenceRawEntry);
                        }

                        ///  scenerio 3
                        // if the trip  started outside  the entry   and  ended  after  exit  of location and there  all entires are present
                        if (_trip.StartDate <= ProcessedZonesDataItem.start_date && ProcessedZonesDataItem.end_date <= _trip.EndDate && ProcessedZonesDataItem.start_date < ProcessedZonesDataItem.end_date)
                        {

                            _EL_GeofenceRawEntry.duration_in = ProcessedZonesDataItem.end_date.Subtract(ProcessedZonesDataItem.start_date).TotalSeconds;

                            _EL_GeofenceRawEntry.duration_out = _trip.Duration.TotalSeconds - _EL_GeofenceRawEntry.duration_in;

                            _EL_GeofenceRawEntry.distance_in = ProcessedZonesDataItem.final_odometer - ProcessedZonesDataItem.initial_odometer;

                            _EL_GeofenceRawEntry.distance_out = _trip.Distance - _EL_GeofenceRawEntry.distance_in;

                            _EL_GeofenceRawEntry.InitialCoordinates = ProcessedZonesDataItem.InitialCoordinates;

                            _EL_GeofenceRawEntry.FinalCoordinates = ProcessedZonesDataItem.FinalCoordinates;

                            _lstEL_GeofenceItem.Add(_EL_GeofenceRawEntry);

                        }

                        ///  scenerio 4                                               
                        //missing  exits in this case
                        ////

                        if (_trip.StartDate <= ProcessedZonesDataItem.start_date && default(DateTime) == ProcessedZonesDataItem.end_date)
                        {
                            // missing exit

                            _EL_GeofenceRawEntry.duration_in = _trip.EndDate.Subtract(ProcessedZonesDataItem.start_date).TotalSeconds;

                            _EL_GeofenceRawEntry.duration_out = _trip.Duration.TotalSeconds - _EL_GeofenceRawEntry.duration_in;

                            _EL_GeofenceRawEntry.distance_in = _trip.EndOdometer - ProcessedZonesDataItem.initial_odometer;

                            _EL_GeofenceRawEntry.distance_out = _trip.Distance - _EL_GeofenceRawEntry.distance_in;

                            _EL_GeofenceRawEntry.InitialCoordinates = ProcessedZonesDataItem.InitialCoordinates;

                            _EL_GeofenceRawEntry.FinalCoordinates = new Coordinates
                            {
                                Lng = _trip.SecondLocationLongtude,
                                Lat = _trip.SecondLocationLattitude,
                            };


                            _lstEL_GeofenceItem.Add(_EL_GeofenceRawEntry);

                        }



                        ///
                        ///  scenerio 5
                        /// missing entry point on the zone 
                        ///
                        if (_trip.StartDate <= ProcessedZonesDataItem.end_date && default(DateTime) == ProcessedZonesDataItem.start_date)
                        {
                            // missing exit

                            _EL_GeofenceRawEntry.duration_in = ProcessedZonesDataItem.end_date.Subtract(_trip.StartDate).TotalSeconds;

                            _EL_GeofenceRawEntry.duration_out = _trip.Duration.TotalSeconds - _EL_GeofenceRawEntry.duration_in;

                            _EL_GeofenceRawEntry.distance_in = ProcessedZonesDataItem.final_odometer - _trip.StartOdometer;

                            _EL_GeofenceRawEntry.distance_out = _trip.Distance - _EL_GeofenceRawEntry.distance_in;

                            _EL_GeofenceRawEntry.InitialCoordinates = new Coordinates
                            {
                                Lat = _trip.FirstLocationLattitude,
                                Lng = _trip.FirstLocationLongtude,
                            };

                            _EL_GeofenceRawEntry.FinalCoordinates = ProcessedZonesDataItem.FinalCoordinates;


                            _lstEL_GeofenceItem.Add(_EL_GeofenceRawEntry);



                        }

                    }

                    else
                    {
                        if (ProcessedZonesDataItem.is_still_in_zone && !ProcessedZonesDataItem.is_range_has_sub_entries)
                        {
                            _EL_GeofenceRawEntry.duration_in = _trip.Duration.TotalSeconds;

                            _EL_GeofenceRawEntry.duration_out = 0;

                            _EL_GeofenceRawEntry.distance_in = _trip.Distance;

                            _EL_GeofenceRawEntry.distance_out = 0;

                            _EL_GeofenceRawEntry.InitialCoordinates = new Coordinates
                            {
                                Lat = _trip.FirstLocationLattitude,

                                Lng = _trip.FirstLocationLongtude,
                            };

                            _EL_GeofenceRawEntry.FinalCoordinates = new Coordinates
                            {
                                Lat = _trip.SecondLocationLattitude,

                                Lng = _trip.SecondLocationLongtude,
                            };

                            _lstEL_GeofenceItem.Add(_EL_GeofenceRawEntry);
                        }
                    }

                    _EL_GeofenceRawEntry.distance_in = Convert.ToDouble(UserSettings.ConvertKMsToXx(ReportSource.Report.intMeasurementUnit, _EL_GeofenceRawEntry.distance_in.ToString(), false, 2));
                    _EL_GeofenceRawEntry.distance_out = Convert.ToDouble(UserSettings.ConvertKMsToXx(ReportSource.Report.intMeasurementUnit, _EL_GeofenceRawEntry.distance_out.ToString(), false, 2));


                    //16:18:40
                    if (_trip.StartDate == Convert.ToDateTime("2020-05-22 15:58:40"))
                    {

                    }



                }


            }

            return _lstEL_GeofenceItem;

        }
        public EL_DeviceZonesSummaryEntry GetDeviceZonesStats(long  vpkDeviceId)
        {
            var _EL_DeviceZonesSummaryEntry = new EL_DeviceZonesSummaryEntry();


            foreach (var zone_ID in ReportSource.ZoneListItems)
            {
                var DeviceZoneGroupName = $"{zone_ID}_{vpkDeviceId}";

                if (ReportSource.dctZonesItems.ContainsKey(DeviceZoneGroupName))
                {
                    var deviceSpecificGeofences = ReportSource.dctZonesItems[DeviceZoneGroupName];

                    var ZonesStatsDetails = SynthesizeGeofenceEntries(deviceSpecificGeofences).SummarizeSpecificZoneStats();

                    _EL_DeviceZonesSummaryEntry.EL_GeofenceItem.Add(zone_ID, ZonesStatsDetails);
                }

            }

            return _EL_DeviceZonesSummaryEntry;
        }

        public Tuple<int, int> GetZoneEventIdByType(string zone_type)
        {


            if (zone_type == "Keep-In")
                return new Tuple<int, int>(804, 805);

            else if (zone_type == "No-Go")
                return new Tuple<int, int>(802, 803);

            else if (zone_type == "Location")
                return new Tuple<int, int>(800, 801);
            else
                return new Tuple<int, int>(0, 0);
        }
        public EL_GeofenceItem SynthesizeGeofenceEntries(EL_GeofenceItem _EL_GeofenceItem)
        {
            //All rows for specific zone id and asset  

            var TotalZoneRecords = _EL_GeofenceItem.ZonesRawData.Count;

            for (int i = 0; i < TotalZoneRecords; i++)
            {
                var ZoneRecordItem = _EL_GeofenceItem.ZonesRawData[i];


                var zone_id = Convert.ToInt64(ZoneRecordItem["ifkZoneId"] is DBNull ? 0 : ZoneRecordItem["ifkZoneId"]);

                var event_id = Convert.ToInt32(ZoneRecordItem["eventid"] is DBNull ? 0 : ZoneRecordItem["eventid"]);



                var dgpsDateTime = Convert.ToDateTime(ZoneRecordItem["gps_datetime"] is DBNull ? default(DateTime) : ZoneRecordItem["gps_datetime"]);

                var dOdometer = Convert.ToDouble(ZoneRecordItem["Odometer"] is DBNull ? 0 : ZoneRecordItem["Odometer"]);


                var lng = Convert.ToDouble(ZoneRecordItem["lon"] is DBNull ? 0 : ZoneRecordItem["lon"]);

                var lat = Convert.ToDouble(ZoneRecordItem["lat"] is DBNull ? 0 : ZoneRecordItem["lat"]);

                _EL_GeofenceItem.zone_name = Convert.ToString(ZoneRecordItem["vgeoname"]);

                Func<bool> IsLastRecord = () => { return TotalZoneRecords - i == 1; };

                Func<bool> IsClosingEvent = () => { return !dctGeofenceEventIds.ContainsKey(event_id); };


                var is_range_has_sub_entries = Convert.ToBoolean(ZoneRecordItem["is_range_has_sub_entries"]);

                if (Convert.ToBoolean(ZoneRecordItem["is_still_in_zone"]) && !is_range_has_sub_entries)
                {
                    _EL_GeofenceItem.ProcessedZonesDataItems.Push(new EL_GeofenceRawEntry
                    {
                        is_still_in_zone = true,
                        zone_name = _EL_GeofenceItem.zone_name,
                        zone_id = zone_id
                    });

                    continue;
                }

                if (_EL_GeofenceItem.ProcessedZonesDataItems.Count > 0)
                {
                    var previousGeofenceEntryItem = _EL_GeofenceItem.ProcessedZonesDataItems.Peek();

                    if (IsClosingEvent())
                    {
                        if (previousGeofenceEntryItem.is_closing_round)
                        {
                            previousGeofenceEntryItem.end_date = dgpsDateTime;
                            previousGeofenceEntryItem.final_odometer = dOdometer;
                        }
                        else
                            previousGeofenceEntryItem.is_closing_round = true;


                        previousGeofenceEntryItem.Summarize();

                    }
                    else
                    {

                        if (previousGeofenceEntryItem.is_closing_round)
                        {

                            if (previousGeofenceEntryItem.start_date == default(DateTime))
                            {
                                previousGeofenceEntryItem.message = "Could not find entry ";

                            }

                            _EL_GeofenceItem.ProcessedZonesDataItems.Push(new EL_GeofenceRawEntry { InitialCoordinates = new Coordinates { Lat = lat, Lng = lng }, zone_name = _EL_GeofenceItem.zone_name, event_id = event_id, zone_id = zone_id, start_date = dgpsDateTime, initial_odometer = dOdometer });

                            previousGeofenceEntryItem = _EL_GeofenceItem.ProcessedZonesDataItems.Peek();
                        }

                        if (IsLastRecord())
                        {
                            if (previousGeofenceEntryItem.end_date == default(DateTime))
                                previousGeofenceEntryItem.message = "Could not find exit";


                            if (previousGeofenceEntryItem.start_date == default(DateTime))
                                previousGeofenceEntryItem.message = "Could not find entry";

                        }
                    }


                }
                else
                {
                    if (!IsClosingEvent())
                        _EL_GeofenceItem.ProcessedZonesDataItems.Push(new EL_GeofenceRawEntry { InitialCoordinates = new Coordinates { Lat = lat, Lng = lng }, zone_name = _EL_GeofenceItem.zone_name, event_id = event_id, zone_id = zone_id, start_date = dgpsDateTime, initial_odometer = dOdometer });
                    else
                        _EL_GeofenceItem.ProcessedZonesDataItems.Push(new EL_GeofenceRawEntry { FinalCoordinates = new Coordinates { Lat = lat, Lng = lng }, zone_name = _EL_GeofenceItem.zone_name, event_id = event_id, zone_id = zone_id, end_date = dgpsDateTime, is_closing_round = true, final_odometer = dOdometer });
                }





            }


            return _EL_GeofenceItem;
        }



        public void GetZonesRawData()
        {
            
            ReportSource.Report.Operation=2;

            var ds = DAL_Geofence.GetGeofenceInfo(ReportSource.Report);

            foreach (DataTable _dt in ds.Tables)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    dr["gps_datetime"] = dr["gps_datetime"] is DBNull ? default(DateTime) : UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["gps_datetime"]), ReportSource.Report.TimeZoneID);


                    var zone_ID = Convert.ToInt64(dr["ifkZoneId"]);                  
                  
                        var _rawDeviceId = Convert.ToString(dr["device_id"]);

                        if (string.IsNullOrWhiteSpace(Convert.ToString(_rawDeviceId)))
                            continue;

                        var DeviceZoneGroupName = $"{zone_ID}_{_rawDeviceId}";

                        if (ReportSource.dctZonesItems.ContainsKey(DeviceZoneGroupName))
                            ReportSource.dctZonesItems[DeviceZoneGroupName].ZonesRawData.Add(dr);
                        else
                            ReportSource.dctZonesItems.Add(DeviceZoneGroupName, new EL_GeofenceItem { ZonesRawData = new List<DataRow> { dr } });

                        ReportSource.ZoneListItems.Add(zone_ID);
                    
                   
                }            


            }
            
        }
     
        public Dictionary<int, int> GetGeofenceEventIds()
        {
            var dictGeofenceEventIds = new Dictionary<int, int>();

            dictGeofenceEventIds.Add(800, 801);  // entered locations 

            dictGeofenceEventIds.Add(802, 803);  // entered no go zone

            dictGeofenceEventIds.Add(804, 805);  // exited keep in locations

            return dictGeofenceEventIds;
        }


    
        public EL_ReportSource GetGeofenceInfo()
        {
            var ds = DAL_Geofence.GetGeofenceInfo(ReportSource.Report);

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                var dt = ds.Tables[i];

                foreach (DataRow dr in dt.Rows)
                {  
                    var _Devices = Convert.ToString(dr["devices"]);

                    ReportSource.Devices = JsonConvert.DeserializeObject<List<El_Device>>(_Devices);
                }
            }


            return ReportSource;
        }

    }

}
