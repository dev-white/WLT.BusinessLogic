using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WLT.DataAccessLayer.DAL_Report;
using WLT.EntityLayer;
using WLT.EntityLayer.EL_Reports;

namespace WLT.BusinessLogic.Bal_Reports
{
   public class Bal_SpeedEpisode
    {

        public Bal_SpeedEpisode()
        {
            raw_episodes = new List<EL_SpeedEpisode>();

             processed_items_disctionary = new Dictionary<long, List<El_EpisodeItem>>();
        }
       public List<EL_SpeedEpisode> raw_episodes { get; set; }

        public Dictionary<long, List<El_EpisodeItem>> processed_items_disctionary { get; set; }
        public Hashtable SpeedEpisodeDataSource(El_Report _El_Report)
        {
            var return_sourceHashTable = new  Hashtable();

            var ds = new DAL_SpeedEpisode().GetSpeedEpisodes(_El_Report);          

            var table_count = 0;

            foreach (DataTable dt in ds.Tables)
            {
                if (table_count==0)
                {
                    foreach (DataRow reader in dt.Rows)                    
                        raw_episodes.Add(new EL_SpeedEpisode
                        {
                            reseller_id = Convert.ToInt32(reader["reseller_id"]),
                            imei_number = Convert.ToInt64(reader["imei_number"]),
                            gps_datetime = Convert.ToDateTime(reader["gps_datetime"]),
                            event_id = Convert.ToInt32(reader["event_id"]),
                            speed = Convert.ToDouble(reader["speed"])
                        });
                    

                }

                if (table_count == 1)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        _El_Report.dStartDate = Convert.ToDateTime(dr["firstdate"]);
                        _El_Report.dEndDate = Convert.ToDateTime(dr["lastdate"]);
                        _El_Report.AssetName = Convert.ToString(dr["vasset"]);
                        _El_Report.ReportId = Convert.ToInt32(dr["overspeed_limit_type"]);
                    }

                    

                }
                if (table_count == 2)
                {



                }


                table_count += 1;
            }


            GetEpisodesTelemetry(_El_Report);


            return return_sourceHashTable;
        }
        public void GetEpisodesTelemetry ( El_Report _El_Report)
        {

            var _El_EpisodeItem = El_EpisodeItem.EpisodeGetCategory(_El_Report.ReportId);
       

            
            raw_episodes.Sort((x, y) => x.gps_datetime.CompareTo(y.gps_datetime));

            ///  //device specifc episodes 
            var extracted_devices = raw_episodes.Select(n => n.imei_number).Distinct();

           

             foreach(var device_imei in extracted_devices)
            {

                processed_items_disctionary.Add(device_imei, new List<El_EpisodeItem> ());

                //device specific episodes 
                var _device_episodes = raw_episodes.Where(p => p.imei_number == device_imei);

                var _previous_event_id = 0;

                var _El_ReportRecord = new El_EpisodeItem();

                foreach ( var record in _device_episodes)
                {
                  
                    // avoid records that start with  episode ends 
                    if (_previous_event_id == 0 &&  _El_EpisodeItem.episode_end_codes.Any(n=>n == record.event_id))
                        continue;



                    if (_El_EpisodeItem.episode_start_codes.Any(n => n == record.event_id))
                    {            
                        if(!_El_ReportRecord.active)
                        {
                            _El_ReportRecord.start_date = record.gps_datetime;
                            _El_ReportRecord.active = true;
                        }
                    }


                    if (_El_EpisodeItem.episode_end_codes.Any(n => n == record.event_id))
                    {

                        if (_El_ReportRecord.active)
                        {
                            _El_ReportRecord.end_date = record.gps_datetime;
                            _El_ReportRecord.active = false;

                            processed_items_disctionary[device_imei].Add(new El_EpisodeItem { 
                                start_date = _El_ReportRecord.start_date,
                                end_date = _El_ReportRecord.end_date });
                        }

                    }



                    _previous_event_id = record.event_id;
                }





            }




          

           


        }

        public string ConstructEpisodesPgSqlStatement()
        {
            var _string = string.Empty;








            return _string;
        }
    }

    public class  El_EpisodeItem
    {

        public El_EpisodeItem()
        {
            episode_start_codes = new List<int>();
            episode_end_codes = new List<int>();
        }
        public  List<int> episode_start_codes { get; set; }
        public List<int> episode_end_codes { get; set; }

        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

        public bool active { get; set; }
        public long imei { get; set; }

        public  static El_EpisodeItem EpisodeGetCategory( long event_id  )
        {
            var _El_EpisodeItem = new El_EpisodeItem();
            if (event_id == 1)
            {
               
                _El_EpisodeItem.episode_start_codes.AddRange(new int[] { 1});
                _El_EpisodeItem.episode_end_codes.AddRange(new int[] { 1 });

            }

            if (event_id == 821)
            {
               
                _El_EpisodeItem.episode_start_codes.AddRange(new int[] { 820, 821 });
                _El_EpisodeItem.episode_end_codes.AddRange(new int[] { 822 });
            }

            if (event_id == 824)
            {
               
                _El_EpisodeItem.episode_start_codes.AddRange(new int[] { 823, 824 });
                _El_EpisodeItem.episode_end_codes.AddRange(new int[] { 825 });
            }
            return _El_EpisodeItem;
        }
    }
}
