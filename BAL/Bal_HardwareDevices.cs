using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;
using WLT.ErrorLog;


namespace WLT.BusinessLogic.BAL
{
    public class Bal_HardwareDevices
    {
        public string ApplyDeviceSettings(EL_HardwareDevices EL_HardwareDevices)
        {
            string results = "";

            DAL_HardwareDevices DAL_HardwareDevices = new DAL_HardwareDevices();

            results = DAL_HardwareDevices.ApplyDeviceSettings(EL_HardwareDevices);

            return results;
        }
    }
}
