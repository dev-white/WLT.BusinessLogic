using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_NTSA
    {

        static string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        public static DataTable pg_GetNtsa_Telemetry_and_Violations(string _commandText)
        {
            var ds = new DataSet();

            try
            {

                var result = JsonConvert.SerializeObject("{}");

                NpgsqlConnection conn = new NpgsqlConnection(AppConfiguration.Configuration().PgsqlConnection);

                NpgsqlCommand command = new NpgsqlCommand();

                command.Connection = conn;

                conn.Open();

                command.CommandText = _commandText;

                command.CommandTimeout = 500;

             NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);

                da.Fill(ds);

                conn.Close();

                da.Dispose();


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_NTSA.cs", "pg_GetNtsa_Telemetry_and_Violations()", ex.Message  + ex.StackTrace);

            }
            finally
            {
                
            }
            var dt = new DataTable();

            foreach (DataTable _dt in ds.Tables)
                dt = _dt;

            return dt;

        }
        public static DataTable pg_GetTelemetry(string _commandText)
        {
            var ds = new DataSet();

            try
            {

                NpgsqlConnection conn = new NpgsqlConnection(AppConfiguration.Configuration().PgsqlConnection);

                NpgsqlCommand command = new NpgsqlCommand();

                command.Connection = conn;

                command.CommandTimeout = 500;

                conn.Open();

                command.CommandText = _commandText;

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);

                da.Fill(ds);

                conn.Close();

                da.Dispose();


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_NTSA.cs", "pg_GetTelemetry()", ex.Message  + ex.StackTrace);

            }

            var dt = new DataTable();

            foreach (DataTable _dt in ds.Tables)
                dt = _dt;

            return dt;

        }

        public static Dictionary<int, string> GetEvents()
        {
            var eventDictionary = new Dictionary<int, string>();

            SqlParameter[] param = new SqlParameter[1];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 6;

                var ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_LiveTelemetry", param);

                foreach (DataTable dt in ds.Tables)
                    foreach (DataRow dr in dt.Rows)
                        eventDictionary.Add(Convert.ToInt32(dr["ipkCommonEventLookupId"]), Convert.ToString(dr["vEventName"]));


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_NTSA.cs", "GetResellerClients()", ex.Message  + ex.StackTrace);


            }

            return eventDictionary;

        }

    }
}
