using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic
{
    public class Bal_CommonEvents
    {
        public List<EL_CommonEventsLookup> GetCommonEvents()
        {
            //Get from Cache
            ObjectCache cache = MemoryCache.Default;

            List<EL_CommonEventsLookup> cachedList = cache["CommonEvents"] as List<EL_CommonEventsLookup>;

            try
            {
                DAL_CommonEvents dal = new DAL_CommonEvents();

                DataSet ds = new DataSet();

                if (cachedList == null)
                {
                    ds = dal.GetCommonEvents();

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        //foreach (DataRow dr in ds.Tables[0].Rows)
                        //{
                        //    cachedList.Add(new EL_CommonEventsLookup
                        //    {
                        //        ipkCommonEventLookupId = Convert.ToInt32(dr["ipkCommonEventLookupId"]),
                        //        vEventName = dr["vEventName"].ToString()

                        //    });
                        //}

                        cachedList = ds.Tables[0].AsEnumerable()
                          .Select(row => new EL_CommonEventsLookup
                          {
                              ipkCommonEventLookupId = row.Field<int>(0),
                              vEventName = row.Field<string>(1)
                          }).ToList();

                    }

                    //Cache Data
                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.SlidingExpiration = TimeSpan.FromHours(10);

                    cache.Set("CommonEvents", cachedList, policy);
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_CommonEvents.cs", "GetCommonEvents()", ex.Message + ex.StackTrace);
            }

            return cachedList;
        }


        //public string GetCustomEventName( int asset_id, int event_id )
        //{   
        //        DAL_CommonEvents dal = new DAL_CommonEvents();             

        //        return dal.GetCustomEventName(asset_id, event_id);
        //}

        public string GetCustomEventName(int asset_id, int event_id)
        {
            var dict = GetDigitalEventsMapping(asset_id);

            return dict.ContainsKey(event_id) ? dict[event_id] : "";
        }
        public DataSet GetCommonEventsTable()
        {
            //Get from Cache
            ObjectCache cache = MemoryCache.Default;

            DataSet CommonEvents = cache["CommonEvents"] as DataSet;

            try
            {
                DAL_CommonEvents dal = new DAL_CommonEvents();

                DataSet ds = new DataSet();

                if (CommonEvents == null)
                {
                    CommonEvents = dal.GetCommonEvents();                 

                     //Cache Data
                     CacheItemPolicy policy = new CacheItemPolicy();
                    policy.SlidingExpiration = TimeSpan.FromHours(10);

                    cache.Set("CommonEvents", CommonEvents, policy);
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_CommonEvents.cs", "GetCommonEventsTable()", ex.Message + ex.StackTrace);
            }

            return CommonEvents;
        }


        public Dictionary<int, string> GetDigitalEventsMapping( int device_id )  // device identity id
        {
            //Get from Cache
            ObjectCache cache = MemoryCache.Default;                     

            var CommonEvents = cache["DigitalMappingEvents"] as Tuple<int, Dictionary<int, string>>;

            try
            {
                DAL_CommonEvents dal = new DAL_CommonEvents();
                

                if (CommonEvents == null  || CommonEvents.Item1!= device_id)
                {

                    CommonEvents = new Tuple<int, Dictionary<int, string>>(device_id, new Dictionary<int, string>());

                    var ds  = dal.GetDigitalEventsMapping (device_id);

                    foreach (DataTable dt in ds.Tables)
                        foreach (DataRow dr in dt.Rows)
                            if (!CommonEvents.Item2.ContainsKey(Convert.ToInt32(dr["ifk_CommonEventId"])))
                            {
                                CommonEvents.Item2.Add(Convert.ToInt32(dr["ifk_CommonEventId"]),$"{ Convert.ToString(dr["vName"])} (on) ");
                                CommonEvents.Item2.Add(Convert.ToInt32(dr["ifk_CommonEventId_Opposite"]), $"{ Convert.ToString(dr["vName"])}  (off) ");
                            }
                                                      


                        //Cache Data
                    CacheItemPolicy policy = new CacheItemPolicy();

                    policy.SlidingExpiration = TimeSpan.FromHours(10);

                    cache.Set("DigitalMappingEvents", CommonEvents, policy);
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_CommonEvents.cs", "GetDigitalEventsMapping()", ex.Message + ex.StackTrace);
            }

            return CommonEvents.Item2;
        }


        public string GetCommonEventName(int EventId)
        {
            string EventName = "";

            try
            {
                List<EL_CommonEventsLookup> List = new List<EL_CommonEventsLookup>();
                List = GetCommonEvents();

                if (List.Count > 0)
                {
                    EventName = List.Find(x => x.ipkCommonEventLookupId == EventId).vEventName;
                }
            }
            catch (Exception ex)
            {                
                EventName = "Event not configured";
            }
            if ("Digital Input 1 Off" == EventName)
            {

            }
            return EventName;
           
        }




        public string GetCommonEventName(int EventId, int  assetId )
        {
            string EventName = "";

            try
            {
                List<EL_CommonEventsLookup> List = new List<EL_CommonEventsLookup>();
                List = GetCommonEvents();

                if (List.Count > 0)
                {
                    EventName = List.Find(x => x.ipkCommonEventLookupId == EventId).vEventName;
                }
            }
            catch (Exception ex)
            {
                EventName = "Event not configured";
            }
            if ("Digital Input 1 Off" == EventName)
            {

            }
            return EventName;

        }



        public DataTable GetDataTableWithEventName(DataSet ds, string EventName, string ReportIdName)
        {
            DataTable dt = new DataTable();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0].Copy();
                dt.Columns.Add(EventName, typeof(System.String));

                foreach (DataRow dr in dt.Rows)
                {
                    var eventName = GetCommonEventName(Convert.ToInt32(dr[ReportIdName]));

                    dr[EventName] = eventName;

                }
            }

            return dt;
        }

 
    }
}
