using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{  
    public class Bal_Eco_Drive
    {
        public int userID { get; set; }
        public string TimezoneID { get; set; }

        public int Unit { get; set; }

        public int  ReportID { get; set; }

        public int ReportTypeId { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }
        public int IEventId { get; set; }
        public double HardwareSpeedLimit { get; private set; }

        public Bal_Eco_Drive()
        {
            var _EcoDrive = new EcoDrive();

          

         //   var ds =   _EcoDrive.GetEcoDriveScore(DateTime.Now,DateTime.Now,454454545454,"");
        }


        public El_Eco_Drive_Model Getdrivingscore()
        {
            var _reportAccess = new DAL_Reports();

            var _ds = _reportAccess.Get_Eco_preliminaries(userID, ReportID, ReportTypeId);

            var _headerRow = _ds.Tables[0].Rows[0];

            Unit = Convert.ToInt32(_headerRow["MeasurementUnit"]);

            var _EcoDrive = new EcoDrive();

            var _El_Eco_Obj = new El_Eco_Drive_Model();

            var _Devices = (from data in _ds.Tables[2].AsEnumerable()
                            select new
                            {
                                vpkDeviceID = data.Field<long>("vpkDeviceID"),
                                vDeviceName = data.Field<string>("vDeviceName"),
                                AssetId = Convert.ToInt32(data["ifkdeviceid"]),
                            }).ToList();

            var _IMEIsCSV = CreateCSV(_Devices);




           var  startTime_ = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_ds.Tables[0].Rows[0]["StartDate"]),TimezoneID);

           var  endTime_ = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_ds.Tables[0].Rows[0]["EndDate"]),TimezoneID);

            var _violation = _EcoDrive.GetAllTripScoreViolationsForTimeRange(_IMEIsCSV, startTime_, endTime_, TimezoneID,2);

            var _filter_O = JsonConvert.DeserializeObject<EL_DatesFilter>(_ds.Tables[0].Rows[0]["TimeRange"].ToString());

            _filter_O.iTimeFilterType = Convert.ToInt32(_ds.Tables[0].Rows[0]["iEnabledDateType"]);

            _filter_O.bAllowFilter = Convert.ToBoolean(_ds.Tables[0].Rows[0]["isCustomTimeEnabled"]);

            IEventId = Convert.ToInt32(_ds.Tables[0].Rows[0]["selectedOverspeedType"]);

            HardwareSpeedLimit = Convert.ToDouble(_ds.Tables[0].Rows[0]["hardwareSpeed"]);

            

            if (_filter_O.bAllowFilter)
            {
                var _newTbl = _violation.Tables[0].Clone();

                foreach (DataRow _row in _violation.Tables[0].Rows)
                {
                    var _Today = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_row["dGpsDatetime"]), TimezoneID);

                    var startDt = Convert.ToDateTime(_Today.ToString("yyyy-MM-dd ") + _filter_O.startTime);

                    var EndDt = Convert.ToDateTime(_Today.ToString("yyyy-MM-dd ") + _filter_O.endTime);

                    if ((_Today < startDt || _Today > EndDt) && _filter_O.iTimeFilterType == 1)
                    {
                        continue;
                    }

                    if ((_Today >= startDt && _Today <= EndDt) && _filter_O.iTimeFilterType == 2)
                    {
                        continue;
                    }

                    _newTbl.Rows.Add(_row.ItemArray);
                }

                _violation.Tables.RemoveAt(0);

                _violation.Tables.Add(_newTbl);


            }


       

            foreach (var _device in _Devices)
            {
                var _El_Eco_Drive_Obj = new El_Eco_Drive_Model();

                var DriverName = "";


                if (_violation.Tables.Count > 0)
            {    
                    var _deviceSpecific = _violation.Tables[0].Select("vpkDeviceID = " + _device.vpkDeviceID);

                    if (_deviceSpecific.Count() > 0)
                    {

                        DriverName = Convert.ToString(_deviceSpecific[0]["DriverName"]);

                        var _data = _deviceSpecific.CopyToDataTable();

                        var _EL_EcoScoreModel = new EL_EcoScoreModel
                        {
                            _Violations = _data,
                            startDate = startTime_,
                            endDateTime = endTime_,
                            imei = _device.vpkDeviceID,
                            AssetId = _device.AssetId,
                            TimeZoneId = TimezoneID,
                            overspeedType = IEventId,
                            Unit = Unit,
                            HardwareSpeedLimit = HardwareSpeedLimit
                        };

                        _El_Eco_Drive_Obj = _EcoDrive.GetEcoDriveScore(_EL_EcoScoreModel, _IMEIsCSV);

                        _El_Eco_Drive_Obj.Fail = 100 - _El_Eco_Drive_Obj.Score;

                        var _distanceConverted = UserSettings.ConvertKMsToXxOdoMeter(Unit, _El_Eco_Drive_Obj.OveralPeriodDistance.ToString(), false, 2);

                        _El_Eco_Drive_Obj.OveralPeriodDistance = Convert.ToDouble(_distanceConverted);

                    }
                   
                }


                _El_Eco_Drive_Obj.Asset = _device.vDeviceName;

                _El_Eco_Drive_Obj.DriverName = DriverName;

                _El_Eco_Obj.List.Add(_El_Eco_Drive_Obj);
            }

          //  _ds.Tables.RemoveAt(0);

            _El_Eco_Obj.Dataset = _ds;

            return  _El_Eco_Obj;
        }

        public El_Eco_Drive_Model Getdrivingscore(int Company_ID)
        {
            var _reportAccess = new DAL_Reports();

            var _ds = _reportAccess.Get_Eco_preliminaries(userID, Company_ID);

            var _EcoDrive = new EcoDrive();

            var _El_Eco_Obj = new El_Eco_Drive_Model();

            var _Devices = (from data in _ds.Tables[2].AsEnumerable()
                            select new
                            {
                                vpkDeviceID = data.Field<long>("vpkDeviceID"),
                                vDeviceName = data.Field<string>("vDeviceName"),

                            }).ToList();

            var _IMEIsCSV = CreateCSV(_Devices);
           
            var _violation = _EcoDrive.GetAllTripScoreViolationsForTimeRange(_IMEIsCSV, startTime, endTime, TimezoneID);

            if (_violation.Tables.Count > 0)
            {
                foreach (var _device in _Devices)
                {

                    var _deviceSpecific = _violation.Tables[0].Select("vpkDeviceID = " + _device.vpkDeviceID);

                    if (_deviceSpecific.Count() > 0)
                    {

                        var _El_Eco_Drive_Obj = _EcoDrive.GetEcoDriveScore(_deviceSpecific.CopyToDataTable(), startTime, endTime, _device.vpkDeviceID, TimezoneID);

                        _El_Eco_Drive_Obj.DriverName = Convert.ToString(_deviceSpecific[0]["DriverName"]);

                        _El_Eco_Drive_Obj.Asset = _device.vDeviceName;

                        _El_Eco_Drive_Obj.Fail = 100 - _El_Eco_Drive_Obj.Score;


                        _El_Eco_Drive_Obj.Logo = getBase64Logo(_deviceSpecific[0]["vLogo"]);




                        _El_Eco_Obj.List.Add(_El_Eco_Drive_Obj);

                    }

                }

            }

            //  _ds.Tables.RemoveAt(0);

            //_El_Eco_objataset = _ds;


            _El_Eco_Obj.List  = _El_Eco_Obj.List.OrderBy(v => v.Score).ToList();


            return _El_Eco_Obj;
        }
        private string getBase64Logo(object logo)
        {

            Byte[] logobytes = null;
            string uploadedLogo = "";
            if (logo != DBNull.Value)
            {
                logobytes = (Byte[])logo;
                string imageBase64 = Convert.ToBase64String(logobytes);
                uploadedLogo = string.Format("<img   class = 'serverImage' src = data:image/gif;base64,{0} />", imageBase64);

            }

            return uploadedLogo;
        }
        private string CreateCSV<T>(List<T> o, string seperator = ",")
        {
            var result = new StringBuilder();

            foreach (var item in o)
            {             
                result.Append(typeof(T).GetProperty("vpkDeviceID").GetValue(item, null).ToString());
                result.Append(seperator);
            }
            result.Append("0");

            return result.ToString();
        }

    }
}
