using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_LiveLocation
    {

        public DataSet GetDeviceDetails(EL_LiveLocation eL_LiveLocation)
        {
            DataSet ds = new DataSet();

            DAL_LiveLocation dAL_LiveLocation = new DAL_LiveLocation();

            ds = dAL_LiveLocation.GetDeviceDetails(eL_LiveLocation);

            return ds;
        }


        public DataSet GetCompanyDetails(EL_LiveLocation eL_LiveLocation)
        {
            DataSet ds = new DataSet();

            DAL_LiveLocation dAL_LiveLocation = new DAL_LiveLocation();

            ds = dAL_LiveLocation.GetCompanyDetails(eL_LiveLocation);

            return ds;
        }

        public DataSet GetDeviceLiveData(EL_LiveLocation eL_LiveLocation)
        {
           
            DataSet ds = new DataSet();

            DAL_LiveLocation dAL_LiveLocation = new DAL_LiveLocation();

            ds = dAL_LiveLocation.GetDeviceLiveData(eL_LiveLocation);            

            return ds;
        }

        public string SaveLiveLocExpiryTime(EL_LiveLocation eL_LiveLocation)
        {
            string result = "";

            DAL_LiveLocation dAL_LiveLocation = new DAL_LiveLocation();

            result = dAL_LiveLocation.SaveLiveLocExpiryTime(eL_LiveLocation);

            return result;
        }
    }
}
