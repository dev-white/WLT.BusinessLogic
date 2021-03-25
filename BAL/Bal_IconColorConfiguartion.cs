using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WLT.DataAccessLayer;
using WLT.EntityLayer.GPSOL;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_IconColorConfiguartion
    {

        static string  f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        public static EL_UserIconConfiguration GetClientIconColorConfigurations(int ClientID)
        {
            var lstserIconConfiguration = new List<EL_UserIconConfiguration>();

            try
            {

                var sql = $"select  ignition_on_speed_above_zero,ignition_on_speed_is_zero,ignition_off,overspeed,device_offline from Wlt_tbl_ClientIconColor_Configurations where  ifk_client_id = {ClientID} or ifk_client_id = 0 order by id ";

                var ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, sql);

                foreach (DataTable dt in ds.Tables)
                    foreach (DataRow dr in dt.Rows)
                    {
                        lstserIconConfiguration.Add(new EL_UserIconConfiguration
                        {
                            Ignition_On_Speed_Above_Zero = Convert.ToString(dr["ignition_on_speed_above_zero"]),
                            Ignition_On_Speed_Is_Zero = Convert.ToString(dr["ignition_on_speed_is_zero"]),
                            Ignition_Off = Convert.ToString(dr["ignition_off"]),
                            Overspeed = Convert.ToString(dr["overspeed"]),
                            device_Offline = Convert.ToString(dr["device_offline"]),
                        });
                    }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_IconColorConfiguartion.cs", "GetUserIconColorConfigurations()", ex.Message  + ex.StackTrace);

            }

            return lstserIconConfiguration.LastOrDefault();
        }

    }
}
