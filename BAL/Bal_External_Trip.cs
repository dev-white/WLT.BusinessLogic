using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_External_Trip
    {
        private static ObjectCache cache = MemoryCache.Default;

        private CacheItemPolicy policy = null;

        private CacheEntryRemovedCallback callback = null;


        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();

        public List<TripItem> GetTrips_API( long _vpkDeviceID )
        {

          //  var _companyDetails = GetCompanyIdByIMEI(_vpkDeviceID );

            var _EL_External_Trip = new EL_External_Trip();

            _EL_External_Trip.ICompanyid = 3;

            _EL_External_Trip.StartDate = DateTime.Now.Date;

            _EL_External_Trip.EndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            _EL_External_Trip.TimeZoneID = "E. Africa Standard Time";

            _EL_External_Trip.vpkDeviceID = _vpkDeviceID;


            Trips trips = new Trips();

            var _dirtyRawData = trips.GetDirtyTripSummary(_EL_External_Trip.vpkDeviceID, _EL_External_Trip.StartDate, _EL_External_Trip.EndDate, _EL_External_Trip.TimeZoneID, _EL_External_Trip.ITrackerType);

           var  CleansummeryList = trips.CleanDirtyTripSummary(_dirtyRawData, _EL_External_Trip.ICompanyid, _EL_External_Trip.vpkDeviceID, _EL_External_Trip.StartDate, _EL_External_Trip.EndDate, _EL_External_Trip.TimeZoneID, false);

           var  lstCleanedTripSummaryWithNoZeroDistance = trips.RemoveZeroDistanceFromCleanedTripSummary(0.01, CleansummeryList, _EL_External_Trip.TimeZoneID);

            CleansummeryList = trips.AddStoppagesToCleanedTripSummary(lstCleanedTripSummaryWithNoZeroDistance, _EL_External_Trip.TimeZoneID);
            
            // remove previous data store 
            cache.Remove(_EL_External_Trip.vpkDeviceID.ToString()); 
           
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();

            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);


            // add trip items to the cache with imei number as a key 
            cache.Add(_EL_External_Trip.vpkDeviceID.ToString(), CleansummeryList, cacheItemPolicy);

            var _TripItemList = new List<TripItem>();

            foreach (var _Trip in CleansummeryList)
                _TripItemList.Add(new TripItem { EventType = _Trip.IsStoppageItem ? "parked" : "journey" , TripId = _Trip.IdStart});

            return _TripItemList;

        }

        public clsPopulateTripSummary GetTripDetails_API( long TripId, long vpkDeviceID )
        {
            var _trips = new List<clsPopulateTripSummary>();
           
            if (_trips ==null)            
                GetTrips_API(vpkDeviceID);

            _trips = cache[vpkDeviceID.ToString()] as List<clsPopulateTripSummary>;

            var specificTrip = _trips.Where(item => item.IdStart == TripId).FirstOrDefault();

            return specificTrip;
        }
        
        public void GetCompanyIdByIMEI( long _vpkDeviceID )
        {
            DataRow _DataRow;            

            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@imei", SqlDbType.BigInt);

                param[0].Value = _vpkDeviceID;           
                
                var ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_GetCompanyDetailsFromImei ", param);

                foreach (DataRow row in ds.Tables) 
                    _DataRow = row;


            }
            catch (Exception ex)
            {                
               LogError.RegisterErrorInLogFile( "Bal_External_Trip.cs", "GetCompanyIdByIMEI()", ex.Message  + ex.StackTrace);
                
            }

           // return _DataRow;


        }

    }
}
