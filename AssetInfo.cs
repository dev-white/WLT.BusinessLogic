using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.ErrorLog;

namespace Whitelabeltracking.BusinessLogic
{
    public class AssetInfo
    {
        LogError logError = new LogError();

        public string GetCurrentStats(int AssetID)
        {
            DataSet ds = new DataSet();
            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.GetCurrentStats(AssetID);

            DataTable dt = new DataTable();
            DataTable _TripStart = new DataTable();
            DataTable _TripCur = new DataTable();

            if (ds.Tables.Count > 0)
            {
                _TripStart = ds.Tables[0].Copy();
                _TripCur = ds.Tables[1].Copy();
            }

            var data = new
            {
                TripStart = _TripStart,
                TripCur = _TripCur
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);


        }

        public string ShowIOCurrentStatus(int AssetID)
        {
            DataSet ds = new DataSet();
            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.ShowIOCurrentStatus(AssetID);

            DataTable _AnalogInput = new DataTable();
            DataTable _DigitalIO = new DataTable();

            if (ds.Tables.Count > 0)
            {
                _AnalogInput = ds.Tables[0].Copy();
                _DigitalIO = ds.Tables[1].Copy();
            }

            var data = new
            {
                AnalogInput = _AnalogInput,
                DigitalIO = _DigitalIO
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);

        }
    }


}
