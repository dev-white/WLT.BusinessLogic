using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;
using System.Data;
using WLT.BusinessLogic.Bal_GPSOL;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_TrailPoints
    {
        public List<EL_TrailPoints> GetTrailPoints(int ipkAssetID, DateTime TripSelectedDate, int iTrackerType, string TimeZoneID, int ifkCompanyID)
        {
            List<EL_TrailPoints> list = new List<EL_TrailPoints>();
            DataSet ds = new DataSet();

            DAL_TrailPoints dal_Trails = new DAL_TrailPoints();
            EL_TrailPoints el_Trails = new EL_TrailPoints();


            DataSet dsTripSummary = new DataSet();
            List<clsPopulateTripSummary> lstCleanedTripSummary = new List<clsPopulateTripSummary>();

            /**************************************************************************************/
            Trips objTrips = new Trips();


            dsTripSummary = objTrips.GetDirtyTripSummary(Convert.ToInt64(ipkAssetID), TripSelectedDate, TripSelectedDate.AddHours(24).AddSeconds(-1), TimeZoneID, iTrackerType);

            lstCleanedTripSummary = objTrips.CleanDirtyTripSummary(dsTripSummary, Convert.ToInt32(ifkCompanyID), Convert.ToInt64(ipkAssetID), TripSelectedDate, TripSelectedDate.AddHours(24).AddSeconds(-1), TimeZoneID, true);


            lstCleanedTripSummary = objTrips.RemoveZeroDistanceFromCleanedTripSummary(0.01, lstCleanedTripSummary, TimeZoneID);


            //lstCleanedTripSummary = objTrips.AddStoppagesToCleanedTripSummary(lstCleanedTripSummary, TimeZoneID);

            var StartDate = new DateTime();
            var EndDate = new DateTime();
            long vpkDeviceID = 0;
            Boolean IsInProgress = false;

            foreach (var tripsList in lstCleanedTripSummary)
            {
                vpkDeviceID = tripsList.VpkDeviceID;
                StartDate = tripsList.StartDateUtc;
                EndDate = tripsList.EndDateUtc;
                IsInProgress = tripsList.IsInProgress;

                break;
            }

            if (lstCleanedTripSummary.Count > 0)
            {
                ds = dal_Trails.GetTrailPoints(vpkDeviceID, StartDate, EndDate);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //Process Trail
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(new EL_TrailPoints { vLatitude = dr["vLatitude"].ToString(), vLongitude = dr["vLongitude"].ToString(), IsInProgress = IsInProgress });
                    }


                }
            }


            return list;
        }
    }
}
