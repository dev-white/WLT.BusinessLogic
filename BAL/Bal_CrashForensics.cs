using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_CrashForensics
    {
        public string GetIncidentDetails(EL_CrashForensics eL_CrashForensics)
        {
            string results = "";

            DataSet ds = new DataSet();

            DAL_CrashForensics dAL_CrashForensics = new DAL_CrashForensics();

            DataTable _AssetDetails = new DataTable();
            DataTable _LoggedBy = new DataTable();
            DataTable _Overview = new DataTable();
            DataTable _TripStart = new DataTable();
            DataTable _TotalDistancePrior = new DataTable();
            DataTable _CurentDayDistance = new DataTable();

            ds = dAL_CrashForensics.GetForensicsDetails(eL_CrashForensics);

            _AssetDetails = ds.Tables[0].Copy();
            _LoggedBy = ds.Tables[1].Copy();
            _Overview = ds.Tables[2].Copy();
            _TripStart = ds.Tables[3].Copy();
            _TotalDistancePrior = ds.Tables[4].Copy();
            _CurentDayDistance = ds.Tables[5].Copy();

            var data = new
            {
                AssetDetails = _AssetDetails,
                LoggedBy = _LoggedBy,
                Overview = _Overview,
                TripStart = _TripStart,
                TotalDistancePrior = _TotalDistancePrior,
                CurentDayDistance = _CurentDayDistance
            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);


            return results;
        }

        public string GetDaysViolationPriorToIncident(EL_CrashForensics eL_CrashForensics)
        {
            string results = "";

            DataSet ds = new DataSet();

            DAL_CrashForensics dAL_CrashForensics = new DAL_CrashForensics();

            int _TotalOverRoadSpeed = 0;
            int _TotalOverDeviceSpeed = 0;
            int _TotalHarshAcceleration = 0;
            int _TotalHarshCornering = 0;
            int _TotalHarshBraking = 0;

            DataTable _CrashImages = new DataTable();

            ds = dAL_CrashForensics.GetForensicsDetails(eL_CrashForensics);  
            
            if(ds.Tables.Count > 0)
            {
                if(ds.Tables[0].Rows.Count > 0){
                    _TotalOverRoadSpeed = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    _TotalOverDeviceSpeed = Convert.ToInt32(ds.Tables[1].Rows[0]["count"]);
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    _TotalHarshAcceleration = Convert.ToInt32(ds.Tables[2].Rows[0]["count"]);
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    _TotalHarshCornering = Convert.ToInt32(ds.Tables[3].Rows[0]["count"]);
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    _TotalHarshBraking = Convert.ToInt32(ds.Tables[4].Rows[0]["count"]);
                }

                _CrashImages = ds.Tables[5].Copy();
            }


            var data = new
            {
                TotalOverRoadSpeed = _TotalOverRoadSpeed,
                TotalOverDeviceSpeed = _TotalOverDeviceSpeed,
                TotalHarshAcceleration = _TotalHarshAcceleration,
                TotalHarshCornering = _TotalHarshCornering,
                TotalHarshBraking= _TotalHarshBraking,
                CrashImages= _CrashImages
            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);


            return results;
        }

        public string GetOtherPartyInvolved(EL_CrashForensics eL_CrashForensics)
        {
            string results = "";

            DataSet ds = new DataSet();

            DAL_CrashForensics dAL_CrashForensics = new DAL_CrashForensics();

            DataTable _OtherParty = new DataTable();

            ds = dAL_CrashForensics.GetForensicsDetails(eL_CrashForensics);

            _OtherParty = ds.Tables[0].Copy();

             var data = new
            {
                 OtherParty= _OtherParty

             };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);


            return results;
        }

        public string GetCrashComments(EL_CrashForensics eL_CrashForensics)
        {
            string results = "";

            DataSet ds = new DataSet();

            DAL_CrashForensics dAL_CrashForensics = new DAL_CrashForensics();

            DataTable _CrashComments = new DataTable();

            ds = dAL_CrashForensics.GetForensicsDetails(eL_CrashForensics);

            _CrashComments = ds.Tables[0].Copy();

            var data = new
            {
                CrashComments = _CrashComments

            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);


            return results;
        }

        public string GetCrashTelemetry(EL_CrashForensics eL_CrashForensics)
        {
            string results = "";

            DataSet ds = new DataSet();

            DAL_CrashForensics dAL_CrashForensics = new DAL_CrashForensics();

            DataTable _CrashTelemetry = new DataTable();

            ds = dAL_CrashForensics.GetForensicsDetails(eL_CrashForensics);

            _CrashTelemetry = ds.Tables[0].Copy();

            var data = new
            {
                CrashTelemetry = _CrashTelemetry

            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);


            return results;
        }

        public string EditOtherPartyInvolved(EL_CrashForensics eL_CrashForensics)
        {
            string results = "";
            
            DAL_CrashForensics dAL_CrashForensics = new DAL_CrashForensics();          

            results = dAL_CrashForensics.EditOtherPartyInvolved(eL_CrashForensics);  

            return results;
        }

        public string AddCrashComment(EL_CrashForensics eL_CrashForensics)
        {
            string results = "";

            DAL_CrashForensics dAL_CrashForensics = new DAL_CrashForensics();

            results = dAL_CrashForensics.AddCrashComment(eL_CrashForensics);

            return results;
        }


        public DataSet GetCompanyDetails(EL_CrashForensics eL_CrashForensics)
        {           
            DataSet ds = new DataSet();

            DAL_CrashForensics dAL_CrashForensics = new DAL_CrashForensics();

            ds = dAL_CrashForensics.GetCompanyDetails(eL_CrashForensics);

            return ds;
        }


    }
}
