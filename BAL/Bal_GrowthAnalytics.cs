using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;

namespace WLT.BusinessLogic
{
    public class Bal_GrowthAnalytics
    {


        public DataTable Analytics_Growth_DevicesReporting_Growth_Analysis_By_Day()
        {
            var _DAL_GrowthAnalytics = new DAL_GrowthAnalytics();

            var _dtsLst = new DataTable("daily_analytics");

            var ds = _DAL_GrowthAnalytics.Analytics_Growth_DevicesReporting_Growth_Analysis_By_Day(DateTime.UtcNow, DateTime.UtcNow);

            foreach (DataTable _dtItem in ds.Tables)
                _dtsLst = _dtItem;

            return _dtsLst;

        }

        public DataTable Analytics_Growth_DevicesReporting_Growth_Analysis_By_Month()
        {
            var _DAL_GrowthAnalytics = new DAL_GrowthAnalytics();

            var _dtsLst = new DataTable("daily_analytics");

            var ds = _DAL_GrowthAnalytics.Analytics_Growth_DevicesReporting_Growth_Analysis_By_Month(DateTime.UtcNow, DateTime.UtcNow);

            foreach (DataTable _dtItem in ds.Tables)
                _dtsLst = _dtItem;

            return _dtsLst;

        }



        public EL_GrowthStats Analytics_NewGrowth(EL_GrowthStats _EL_GrowthStats)
        {
            var _time_variable = _EL_GrowthStats.time_query_value;

            var _DAL_GrowthAnalytics = new DAL_GrowthAnalytics();

            var _dtsLst = new DataTable("daily_analytics");

            Action GetRangeDates = delegate ()
           {
               var final_date = _EL_GrowthStats.last_date.Date;


               if (_time_variable == "day")
               {
                   _EL_GrowthStats.last_date = final_date;
                   _EL_GrowthStats.first_date = final_date.AddSeconds(-1).Date;
               }

               if (_time_variable == "week")
               {
                   _EL_GrowthStats.last_date = final_date;
                   _EL_GrowthStats.first_date = final_date.AddDays(-6).Date;
               }

               if (_time_variable == "month")
               {
                   _EL_GrowthStats.last_date = new DateTime(final_date.Year, final_date.Month, 1);
                   _EL_GrowthStats.first_date = _EL_GrowthStats.last_date.AddSeconds(-1).Date;
               }


           };

            Func<DataSet, DataSet, List<EL_GrowthStatsReseller>> _dtMergeResults = delegate (DataSet fir_ds, DataSet sec_ds)
            {
                var _earliestDt = new DataTable();

                var _latestDt = new DataTable();

                foreach (DataTable dt in fir_ds.Tables)
                    _earliestDt = dt;

                foreach (DataTable dt in sec_ds.Tables)
                    _latestDt = dt;


                if (_latestDt.Rows.Count == 0 && _earliestDt.Rows.Count == 0)
                    return new List<EL_GrowthStatsReseller>();

                var _lstEL_GrowthStatsReseller = new List<EL_GrowthStatsReseller>();

                var _fullJoinList = _earliestDt.AsEnumerable().FullOuterJoin(_latestDt.AsEnumerable(), late => late.Field<int>("reseller_id"), early => early.Field<int>("reseller_id"), (earlyDr, lateDr, id) => new JoinDr(earlyDr, lateDr));


                _lstEL_GrowthStatsReseller= _fullJoinList.Select(p => new EL_GrowthStatsReseller {

                    comp = p.EarliestDr == null ? p.LatestDr.Field<string>("company_name") : p.EarliestDr.Field<string>("company_name"),
                    res_id = p.EarliestDr == null ? p.LatestDr.Field<int>("reseller_id") : p.EarliestDr.Field<int>("reseller_id"),


                    f_read_dt = ( p.EarliestDr == null  || !DateTime.TryParse(p.EarliestDr["reading_date"].ToString(),out _)) ? default(DateTime) : p.EarliestDr.Field<DateTime>("reading_date"),
                    l_read_dt = (p.LatestDr == null || !DateTime.TryParse(p.LatestDr["reading_date"].ToString(), out _)) ? default(DateTime) : p.LatestDr.Field<DateTime>("reading_date"),



                    f_read =  (p.EarliestDr == null || !int.TryParse(p.EarliestDr["reading"].ToString(), out _)) ? 0 : p.EarliestDr.Field<int>("reading"),
                    l_read =  (p.LatestDr == null || !int.TryParse(p.LatestDr["reading"].ToString(), out _)) ? 0 : p.LatestDr.Field<int>("reading"),
                  

                    dif = ((p.LatestDr == null || !int.TryParse(p.LatestDr["reading"].ToString(), out _)) ? 0 : p.LatestDr.Field<int>("reading")) - ((p.EarliestDr == null || !int.TryParse(p.EarliestDr["reading"].ToString(), out _)) ? 0 : p.EarliestDr.Field<int>("reading"))

                }).OrderByDescending(p=>p.l_read).ToList();


                return _lstEL_GrowthStatsReseller;
            };

            // fix date ranges 
            GetRangeDates();

            if (_EL_GrowthStats.query_type == "live_growth")
            {
                _EL_GrowthStats.first_date = new DateTime(_EL_GrowthStats.last_date.Year, _EL_GrowthStats.last_date.Month,1);

                var _live_growthDs = _DAL_GrowthAnalytics.Analytics_NewGrowth(_EL_GrowthStats);

                foreach( DataTable dt in _live_growthDs.Tables)
                    foreach (DataRow dr in dt.Rows)
                    {
                        _EL_GrowthStats._lstEL_GrowthStatsReseller.Add(new EL_GrowthStatsReseller
                        {
                            comp = Convert.ToString( dr["company_name"]),
                            res_id = Convert.ToInt32(dr["reseller_id"]),

                            //new_devs = Convert.ToInt32(dr["new_devices"]),
                            new_devs_rpng = Convert.ToInt32(dr["AddedThisMonthAndReporting"]),
                            new_devs_nt_rpng = Convert.ToInt32(dr["AddedThisMonthMaybeNotReporting"]) 


                        });
                    }

            }

            else
            {


                if (_time_variable == "day")
                {

                    _EL_GrowthStats.specific_date = _EL_GrowthStats.last_date;
                    var latestDsPortion = _DAL_GrowthAnalytics.Analytics_NewGrowth(_EL_GrowthStats);

                    _EL_GrowthStats.specific_date = _EL_GrowthStats.first_date;
                    var initialDsPortion = _DAL_GrowthAnalytics.Analytics_NewGrowth(_EL_GrowthStats);

                    _EL_GrowthStats._lstEL_GrowthStatsReseller = _dtMergeResults(initialDsPortion, latestDsPortion);

                }

                if (_time_variable == "week")
                {


                    // first portion of the query
                    var latestDsPortion = _DAL_GrowthAnalytics.Analytics_NewGrowth(_EL_GrowthStats);


                    var _clone__EL_GrowthStats = JsonConvert.DeserializeObject<EL_GrowthStats>(JsonConvert.SerializeObject(_EL_GrowthStats));

                    // second portion
                    _clone__EL_GrowthStats.last_date = _clone__EL_GrowthStats.first_date.AddSeconds(-1).Date;
                    _clone__EL_GrowthStats.first_date = _clone__EL_GrowthStats.last_date.AddDays(-6).Date;

                    var initialDsPortion = _DAL_GrowthAnalytics.Analytics_NewGrowth(_clone__EL_GrowthStats);



                    _EL_GrowthStats._lstEL_GrowthStatsReseller = _dtMergeResults(initialDsPortion, latestDsPortion);

                }

                if (_time_variable == "month")
                {


                    // first portion of the query
                    _EL_GrowthStats.specific_date = _EL_GrowthStats.first_date;
                    var initialDsPortion = _DAL_GrowthAnalytics.Analytics_NewGrowth(_EL_GrowthStats);



                    // second portion
                    _EL_GrowthStats.specific_date = _EL_GrowthStats.last_date;
                    var latestDsPortion = _DAL_GrowthAnalytics.Analytics_NewGrowth(_EL_GrowthStats);

                    _EL_GrowthStats._lstEL_GrowthStatsReseller = _dtMergeResults(initialDsPortion, latestDsPortion);


                }

            }
            return _EL_GrowthStats;

        }






      
    }
    public static class ExtensionsHelper
    {
     
      internal static IEnumerable<TResult> FullOuterJoin<TA, TB, TKey, TResult>(
      this IEnumerable<TA> a,
      IEnumerable<TB> b,
      Func<TA, TKey> selectKeyA,
      Func<TB, TKey> selectKeyB,
      Func<TA, TB, TKey, TResult> projection,
      TA defaultA = default(TA),
      TB defaultB = default(TB),
      IEqualityComparer<TKey> cmp = null)
            {
                cmp = cmp ?? EqualityComparer<TKey>.Default;
                var alookup = a.ToLookup(selectKeyA, cmp);
                var blookup = b.ToLookup(selectKeyB, cmp);

                var keys = new HashSet<TKey>(alookup.Select(p => p.Key), cmp);
                keys.UnionWith(blookup.Select(p => p.Key));

                var join = from key in keys
                           from xa in alookup[key].DefaultIfEmpty(defaultA)
                           from xb in blookup[key].DefaultIfEmpty(defaultB)
                           select projection(xa, xb, key);

                return join;
            }
             
    }
}
