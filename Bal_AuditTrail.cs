using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;
using System.Data;
using Newtonsoft.Json;

namespace WLT.BusinessLogic
{
    public class Bal_AuditTrail
    {
        public void CreateOrUpdateAuditTrail(EL_AuditTrail _EL_AuditTrail)
        {

        }

        public void SaveAuditTrail(EL_AuditTrail el)
        {

            DAL_AuditTrail dal = new DAL_AuditTrail();
            dal.SaveAuditTrail(el);
        }

        public List<EL_AuditTrail> GetAllAuditTrail(EL_AuditTrail el)
        {
            List<EL_AuditTrail> list = new List<EL_AuditTrail>();

            DAL_AuditTrail dal = new DAL_AuditTrail();

            DataSet ds = new DataSet();

            ds = dal.GetAllAuditTrail(el);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var crudId = Convert.ToInt32(dr["crud_id"]);
                    list.Add(new EL_AuditTrail
                    {
                        ActivityDatetime = Convert.ToDateTime(UserSettings.ConvertUTCDateTimeToProperLocalDateTime(Convert.ToDateTime(dr["activity_datetime"]), el.vTimeZone)),
                        ActivityDbId = Convert.ToString(dr["activity_db_id"]) == "" ? 0 : Convert.ToInt32(dr["activity_db_id"]),
                        vAdditionalInfo = Convert.ToString(dr["additional_info"]),
                        AssetId = Convert.ToString(dr["asset_id"]) == "" ? 0 : Convert.ToInt32(dr["asset_id"]),
                        DeviceId = Convert.ToString(dr["device_id"]) == "" ? 0 : Convert.ToInt32(dr["device_id"]),
                        vIpAddress = Convert.ToString(dr["ip_address"]),
                        CrudName = crudId == 1 ? "Create" : crudId == 2 ? "Read" : crudId == 3 ? "Update" : crudId == 4 ? "Delete" : "",
                        UserName = Convert.ToString(dr["vName"]),
                        vMobile = Convert.ToString(dr["vMobile"]),
                        vEmail = Convert.ToString(dr["vEmail"]),
                        vActivity = Convert.ToString(dr["activity"])
                    });

                }
            }

            return list;
        }

        public List<EL_AuditTrail> GetAuditTrailByFilter(EL_AuditTrail el)
        {
            List<EL_AuditTrail> list = new List<EL_AuditTrail>();

            el.StartTime = el.ActivityDatetime;
            el.EndTime = el.ActivityDatetime.AddHours(24).AddSeconds(-1);

            //Convert to UTC time
            el.StartTime = Convert.ToDateTime(UserSettings.ConvertLocalDateTimeToUTCDateTime(el.StartTime, el.vTimeZone));
            el.EndTime = Convert.ToDateTime(UserSettings.ConvertLocalDateTimeToUTCDateTime(Convert.ToDateTime(el.EndTime), el.vTimeZone));

            DAL_AuditTrail dal = new DAL_AuditTrail();

            DataSet ds = new DataSet();

            ds = dal.GetAuditTrailByFilter(el);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var crudId = Convert.ToInt32(dr["crud_id"]);

                    list.Add(new EL_AuditTrail
                    {
                        ActivityDatetime = Convert.ToDateTime(UserSettings.ConvertUTCDateTimeToProperLocalDateTime(Convert.ToDateTime(dr["activity_datetime"]), el.vTimeZone)),
                        ActivityDbId = Convert.ToString(dr["activity_db_id"]) == "" ? 0 : Convert.ToInt32(dr["activity_db_id"]),
                        vAdditionalInfo = Convert.ToString(dr["additional_info"]),
                        AssetId = Convert.ToString(dr["asset_id"]) == "" ? 0 : Convert.ToInt32(dr["asset_id"]),
                        DeviceId = Convert.ToString(dr["device_id"]) == "" ? 0 : Convert.ToInt32(dr["device_id"]),
                        vIpAddress = Convert.ToString(dr["ip_address"]),
                        CrudName = crudId == 1 ? "Create" : crudId == 2 ? "Read" : crudId == 3 ? "Update" : crudId == 4 ? "Delete" : "",
                        UserName = Convert.ToString(dr["vName"]),
                        vMobile = Convert.ToString(dr["vMobile"]),
                        vEmail = Convert.ToString(dr["vEmail"]),
                        vActivity = Convert.ToString(dr["activity"])
                    });
                }
            }

            return list;
        }

        public string GetAuditTrailFeature()
        {
            DAL_AuditTrail dal = new DAL_AuditTrail();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds = dal.GetAuditTrailFeature();

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();
            }

            var data = new
            {
                features = dt
            };

            return JsonConvert.SerializeObject(data);

        }

        public string GetAuditTrailUsers(EL_AuditTrail el)
        {
            DAL_AuditTrail dal = new DAL_AuditTrail();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds = dal.GetAuditTrailUsers(el);

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();
            }

            var data = new
            {
                users = dt
            };

            return JsonConvert.SerializeObject(data);
        }
    }
}
