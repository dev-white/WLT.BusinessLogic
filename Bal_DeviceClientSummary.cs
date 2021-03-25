using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WLT.DataAccessLayer;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic
{
    public class Bal_DeviceClientSummary
    {
        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        public DataRow GetDeviceSummaryDetails(  El_Device  _device )
        {
            DataRow _row = null;

            try
            {
                var param = new SqlParameter[1];

                param[0] = new SqlParameter("@imei_number", SqlDbType.BigInt);

                param[0].Value = _device.vpkDeviceID;

                var ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_device_summary_report", param);

              
                foreach (DataTable dt in ds.Tables)
                    foreach (DataRow dr in dt.Rows)
                        _row = dr;

                
            }
            catch(Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_DeviceClientSummary.cs", "GetDeviceSummaryDetails()", ex.Message + ex.StackTrace);
            }
          return _row;
        }
    }
}
