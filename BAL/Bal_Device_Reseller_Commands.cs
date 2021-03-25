using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using Newtonsoft.Json;

namespace WLT.BusinessLogic.BAL
{
    public  class Bal_Device_Reseller_Commands
    {
       public int ifkResellerId { get; set; }

        public string GetAllResellerDeviceCommands(El_ResellerDevicesCommand _El_ResellerDevicesCommand )
        {
            var dataAccess = new  DAL_Admin();

            var _deviceTypes = dataAccess.ResellerDevicesCommands(_El_ResellerDevicesCommand);

            var _devices = new List<El_ResellerDevicesCommand>();


            var _El_ResellerDevicesCommandObj = new El_ResellerDevicesCommand();




            foreach (DataRow dr in  _deviceTypes.Tables[0].Rows)
            {
                _devices.Add(new El_ResellerDevicesCommand {
                    DeviceName = Convert.ToString(dr["vDeviceName"]),
                    ifkDeviceTypeID = Convert.ToInt32(dr["ipkDeviceID"]),
                    CommandType = Convert.ToString(dr["CommandType"]),
                    CommandText = Convert.ToString(dr["CommandText"]),
                    CommandDescription = Convert.ToString(dr["CommandDescription"]),
                    CommandID = Convert.ToString(dr["CommandID"]) == "" ? 0 : Convert.ToInt32(dr["CommandID"]),
                    isShowOnAssetInfo = Convert.ToBoolean(dr["ishowOnAssetInfo"]),
                    
                });
            }
            _El_ResellerDevicesCommandObj.List.Add(_devices);

            _devices = new List<El_ResellerDevicesCommand>();

            foreach (DataRow dr in _deviceTypes.Tables[1].Rows)
            {
                _devices.Add(new El_ResellerDevicesCommand
                {
                    DeviceName = Convert.ToString(dr["vDeviceName"]),
                    ifkDeviceTypeID = Convert.ToInt32(dr["ipkDeviceID"]),
                    CommandType = Convert.ToString(dr["CommandType"]),
                    CommandText = Convert.ToString(dr["CommandText"]),
                    CommandDescription = Convert.ToString(dr["CommandDescription"]),
                    CommandID = Convert.ToInt32(dr["CommandID"]),
                    isShowOnAssetInfo = Convert.ToBoolean(dr["ishowOnAssetInfo"]),

                });
            }

            _El_ResellerDevicesCommandObj.List.Add(_devices);

            var _json = JsonConvert.SerializeObject(_El_ResellerDevicesCommandObj);

            return _json;
        }


        public string UpdateResellerHardwareCommandItem(El_ResellerDevicesCommand _El_ResellerDevicesCommand)
        {
            var dataAccess = new DAL_Admin();

            var _deviceTypes = dataAccess.ResellerDevicesCommands(_El_ResellerDevicesCommand);

            var _json = JsonConvert.SerializeObject(_deviceTypes);

            return _json;
        }


        public string GetCustomResellerDeviceCommands(El_ResellerDevicesCommand _El_ResellerDevicesCommand)
        {
            var dataAccess = new DAL_Admin();

            var _deviceTypes = dataAccess.ResellerDevicesCommands(_El_ResellerDevicesCommand);

            var _devices = new List<El_ResellerDevicesCommand>();

            foreach (DataRow dr in _deviceTypes.Tables[0].Rows)
            {
                _devices.Add(new El_ResellerDevicesCommand
                {
                    DeviceName = Convert.ToString(dr["vDeviceName"]),
                    ifkDeviceTypeID = Convert.ToInt32(dr["ipkDeviceID"]),
                    CommandType = Convert.ToString(dr["CommandType"]),
                    CommandText = Convert.ToString(dr["CommandText"]),
                    CommandDescription = Convert.ToString(dr["CommandDescription"]),
                    CommandID = Convert.ToInt32(dr["CommandID"]),
                    isShowOnAssetInfo = Convert.ToBoolean(dr["ishowOnAssetInfo"]),
                    IsSMSCmd = Convert.ToBoolean(dr["IsSMSCmd"])

                });
            }
          
            var _json = JsonConvert.SerializeObject(_devices);

            return _json;
        }

        public string UpdateResellerHardwareCommands(El_ResellerDevicesCommand _El_ResellerDevicesCommand)
        {
            var dataAccess = new DAL_Admin();

            var _deviceTypes = dataAccess.ResellerDevicesCommands(_El_ResellerDevicesCommand);

            return string.Empty;
        }

        public string SaveResellerHardwareCommands(El_ResellerDevicesCommand _El_ResellerDevicesCommand)
        {
            var dataAccess = new DAL_Admin();

            var _deviceTypes = dataAccess.ResellerDevicesCommands(_El_ResellerDevicesCommand);

            return string.Empty;
        }
    }
}
