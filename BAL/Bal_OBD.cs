using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_OBD
    {
        public string GetOBDData(EL_GenericData el)
        {
            string result = "";

            try
            {
                DataSet ds = new DataSet();             

                DataTable _OBD = new DataTable();

                //_OBD.Columns.Add("RPM", typeof(string));
                //_OBD.Columns.Add("CTemp", typeof(string));
                //_OBD.Columns.Add("FuelL", typeof(string));
                //_OBD.Columns.Add("FuelT", typeof(string));
                //_OBD.Columns.Add("AirFlw", typeof(string));
                //_OBD.Columns.Add("IaT", typeof(string));
                //_OBD.Columns.Add("TPos", typeof(string));
                //_OBD.Columns.Add("EngHrs", typeof(string));
                //_OBD.Columns.Add("MStatus", typeof(string));
                _OBD.Columns.Add("vTextMessage", typeof(string));
                _OBD.Columns.Add("dGPSDateTime", typeof(DateTime));
                _OBD.Columns.Add("vReportID", typeof(int));


                DAL_OBD dAL_OBD = new DAL_OBD();

                ds = dAL_OBD.GetOBDData(el);

                //Organize data
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["ObdData"]) != "")
                    {                      

                        var jObj = JsonConvert.DeserializeObject<ExpandoObject>(Convert.ToString(dr["ObdData"]));

                        var _drNewSource = _OBD.NewRow();

                        foreach (KeyValuePair<string, object> prop in jObj)
                            {
                                if (!_OBD.Columns.Contains(prop.Key))
                                    _OBD.Columns.Add(prop.Key, typeof(string));

                                _drNewSource[prop.Key] = prop.Value;
                               
                            }


                        _drNewSource["vTextMessage"] = dr["vTextMessage"];
                        _drNewSource["dGPSDateTime"] = Convert.ToDateTime(UserSettings.ConvertUTCDateTimeToProperLocalDateTime(Convert.ToDateTime(dr["dGPSDateTime"]), el.TimeZone));
                        _drNewSource["vReportID"] = Convert.ToInt32(dr["vReportID"]);

                        _OBD.Rows.Add(_drNewSource);                     

                    
                    }
                }


                var data = new
                {
                    OBD = _OBD
                };

                result = JsonConvert.SerializeObject(data, Formatting.Indented);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_OBD.cs", "GetOBDData()", ex.Message  + ex.StackTrace);
            }


            return result;
        }

        public string GetOBDData_DELETE_ME(EL_GenericData el)
        {
            string result = "";

            try
            {
                DataSet ds = new DataSet();

                //List<EL_OBDData> list = new List<EL_OBDData>();

                DataTable _OBD = new DataTable();

                _OBD.Columns.Add("RPM", typeof(string));
                _OBD.Columns.Add("CTemp", typeof(string));
                _OBD.Columns.Add("FuelL", typeof(string));
                _OBD.Columns.Add("FuelT", typeof(string));
                _OBD.Columns.Add("AirFlw", typeof(string));
                _OBD.Columns.Add("IaT", typeof(string));
                _OBD.Columns.Add("TPos", typeof(string));
                _OBD.Columns.Add("EngHrs", typeof(string));
                _OBD.Columns.Add("MStatus", typeof(string));
                _OBD.Columns.Add("vTextMessage", typeof(string));
                _OBD.Columns.Add("dGPSDateTime", typeof(DateTime));
                _OBD.Columns.Add("vReportID", typeof(int));


                DAL_OBD dAL_OBD = new DAL_OBD();

                ds = dAL_OBD.GetOBDData(el);

                //Organize data
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["ObdData"]) != "")
                    {
                        EL_OBDData eL_OBD = JsonConvert.DeserializeObject<EL_OBDData>(Convert.ToString(dr["ObdData"]));

                        //list.Add(eL_OBD);

                        _OBD.Rows.Add(
                               eL_OBD.RPM == null ? "-" : eL_OBD.RPM,
                               eL_OBD.CTemp == null ? "-" : eL_OBD.CTemp,
                               eL_OBD.FuelL == null ? "-" : eL_OBD.FuelL,
                               eL_OBD.FuelT == null ? "-" : eL_OBD.FuelT,
                               eL_OBD.AirFlw == null ? "-" : eL_OBD.AirFlw,
                               eL_OBD.IaT == null ? "-" : eL_OBD.IaT,
                               eL_OBD.TPos == null ? "-" : eL_OBD.TPos,
                               eL_OBD.EngHrs == null ? "-" : eL_OBD.EngHrs,
                               eL_OBD.MStatus == null ? "-" : eL_OBD.MStatus,
                               dr["vTextMessage"],
                               Convert.ToDateTime(UserSettings.ConvertUTCDateTimeToProperLocalDateTime(Convert.ToDateTime(dr["dGPSDateTime"]), el.TimeZone)),
                               Convert.ToInt32(dr["vReportID"])
                           );
                    }
                }


                var data = new
                {
                    OBD = _OBD
                };

                result = JsonConvert.SerializeObject(data, Formatting.Indented);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_OBD.cs", "GetOBDData()", ex.Message + ex.StackTrace);
            }


            return result;
        }

    }
}
