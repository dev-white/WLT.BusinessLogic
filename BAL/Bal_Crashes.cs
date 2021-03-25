using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;
using WLT.ErrorLog;
using System.Data;
using Newtonsoft.Json;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_Crashes
    {
        public string GetImpactDetections_deleteme(EL_Crashes eL_Crashes)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _Impacts = new DataTable();

            DAL_Crashes dal_Crashes = new DAL_Crashes();

            ds = dal_Crashes.GetImpactDetections(eL_Crashes);

            _Impacts = ds.Tables[0].Copy();

            var data = new
            {
                Impacts = _Impacts
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }


        public string GetImpactDetections(EL_Crashes eL_Crashes)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _Impacts = new DataTable();

            DAL_Crashes dal_Crashes = new DAL_Crashes();

            ds = dal_Crashes.GetImpactDetections(eL_Crashes);

            var tblCommonData = new List<EL_CrashesTrackingData>();

            var tblEventsAssetDetails = new List<EL_CrashesAssetData>();

            var tbl_count = 0;

            foreach (DataTable dt in ds.Tables)
            {
                if (tbl_count == 0)
                    foreach (DataRow dr in dt.Rows)
                        tblCommonData.Add(new EL_CrashesTrackingData
                        {
                            dGPSDateTime = Convert.ToDateTime(dr["dGPSDateTime"]),
                            ipkCommanTrackingID = Convert.ToInt64(dr["ipkCommanTrackingID"]),
                            vTextMessage = Convert.ToString(dr["vTextMessage"]),
                            vVehicleSpeed = Convert.ToDouble(dr["vVehicleSpeed"]),
                            vpkDeviceID = Convert.ToInt64(dr["vpkDeviceID"]),


                             bIsIgnitionOn = Convert.ToBoolean(dr["bIsIgnitionOn"]),
                            vLatitude = Convert.ToDouble(dr["vLatitude"]),
                            vLongitude = Convert.ToDouble(dr["vLongitude"]),
                            vRoadSpeed = Convert.ToDouble(dr["vRoadSpeed"]),
                            vHeading = Convert.ToInt32(dr["vHeading"]),
                        });


                if (tbl_count == 1)
                    foreach (DataRow dr in dt.Rows)
                        tblEventsAssetDetails.Add(new EL_CrashesAssetData
                        {
                            vDeviceName = Convert.ToString(dr["vDeviceName"]),
                            CommentsCount = Convert.ToInt32(dr["CommentsCount"]),
                            Make = Convert.ToString(dr["Make"]),
                            ifkCommanTrackingID = Convert.ToInt64(dr["ifkCommanTrackingID"]),
                            isLogDismissed = Convert.ToBoolean(dr["isLogDismissed"]),
                            vpkDeviceID = Convert.ToInt64(dr["vpkDeviceID"])
                        });



                tbl_count += 1;
            }


            dynamic _LoggedCrashes;

            if (tblCommonData.Count() > 0)
            {
                _LoggedCrashes = from asset in tblEventsAssetDetails
                                 join common_data in tblCommonData on asset.vpkDeviceID equals common_data.vpkDeviceID into temp
                                 from _common in temp.DefaultIfEmpty()

                                 select new
                                 {
                                     _common?.dGPSDateTime,
                                     _common?.ipkCommanTrackingID,
                                     _common?.vTextMessage,
                                     _common?.vVehicleSpeed,
                                     _common?.bIsIgnitionOn,
                                     _common?.vLatitude,
                                     _common?.vLongitude,
                                     _common?.vRoadSpeed,
                                     _common?.vHeading,

                                     asset.vDeviceName,
                                     asset.CommentsCount,
                                     asset.Make,
                                     asset.ifkCommanTrackingID
                                 };
            }
            else            
               _LoggedCrashes = new List<object>();
               

            
          


            var data = new
            {
                Impacts = _LoggedCrashes
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }

        public string GetLoggedCrashes_deleteme(EL_Crashes eL_Crashes)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _LoggedCrashes = new DataTable();

            DAL_Crashes dal_Crashes = new DAL_Crashes();

            ds = dal_Crashes.GetLoggedCrashes(eL_Crashes);

            _LoggedCrashes = ds.Tables[0].Copy();

            var data = new
            {
                LoggedCrashes = _LoggedCrashes
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }
        public string GetLoggedCrashes(EL_Crashes eL_Crashes)
        {
            string result = "";           

            DataSet ds = new DataSet();        

            DAL_Crashes dal_Crashes = new DAL_Crashes();

            ds = dal_Crashes.GetLoggedCrashes(eL_Crashes);

            var tblCommonData = new List<EL_CrashesTrackingData>();
            
            var tblEventsAssetDetails = new List<EL_CrashesAssetData>();

            var tbl_count =0;                   

            foreach (DataTable dt in ds.Tables)
            {
                if (tbl_count == 0)                
                    foreach (DataRow dr in dt.Rows)
                        tblCommonData.Add(new EL_CrashesTrackingData
                        {
                            dGPSDateTime = Convert.ToDateTime(dr["dGPSDateTime"]),
                            ipkCommanTrackingID = Convert.ToInt64(dr["ipkCommanTrackingID"]),
                            vTextMessage = Convert.ToString(dr["vTextMessage"]),
                            vVehicleSpeed = Convert.ToDouble(dr["vVehicleSpeed"]),

                            bIsIgnitionOn = Convert.ToBoolean(dr["bIsIgnitionOn"]),
                            vLatitude = Convert.ToDouble(dr["vLatitude"]),
                            vLongitude = Convert.ToDouble(dr["vLongitude"]),
                            vRoadSpeed = Convert.ToDouble(dr["vRoadSpeed"]),
                            vHeading = Convert.ToInt32(dr["vHeading"]),
                        });
                

                if (tbl_count == 1)      
                    foreach (DataRow dr in dt.Rows)
                        tblEventsAssetDetails.Add(new EL_CrashesAssetData
                        {
                            vDeviceName = Convert.ToString(dr["vDeviceName"]),
                            CommentsCount = Convert.ToInt32(dr["CommentsCount"]),
                            Make = Convert.ToString(dr["Make"]),
                            ifkCommanTrackingID = Convert.ToInt64(dr["ifkCommanTrackingID"]),
                            ipkCrashLogID = Convert.ToInt64(dr["ipkCrashLogID"]),

                        
                        });               



                tbl_count += 1;
            }





                var _LoggedCrashes = from asset in tblEventsAssetDetails
                           join common_data in tblCommonData on asset.ifkCommanTrackingID  equals common_data.ipkCommanTrackingID into temp
                           from _common in temp.DefaultIfEmpty()
                           select new {

                               _common?.dGPSDateTime,
                               _common?.ipkCommanTrackingID,
                               _common?.vTextMessage,
                               _common?.vVehicleSpeed,
                               _common? .bIsIgnitionOn ,
                               _common?.vLatitude ,
                               _common?.vLongitude ,
                               _common?.vRoadSpeed ,
                               _common?.vHeading,




                               asset.vDeviceName,
                               asset.CommentsCount,
                               asset.Make,
                               asset.ifkCommanTrackingID,
                               asset.ipkCrashLogID
                           };
          

          //  _LoggedCrashes = ds.Tables[0].Copy();

            var data = new
            {
              LoggedCrashes = _LoggedCrashes
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }
   

        public string GetDismissedCrashes_deleteme(EL_Crashes eL_Crashes)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _DismissedCrashes = new DataTable();

            DAL_Crashes dal_Crashes = new DAL_Crashes();

            ds = dal_Crashes.GetLoggedCrashes(eL_Crashes);

            _DismissedCrashes = ds.Tables[0].Copy();

            var data = new
            {
                DismissedCrashes = _DismissedCrashes
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }


        public string GetDismissedCrashes(EL_Crashes eL_Crashes)
        {
            string result = "";

            eL_Crashes.isLogDismissed = true;

            DataSet ds = new DataSet();

            DataTable _DismissedCrashes = new DataTable();

            DAL_Crashes dal_Crashes = new DAL_Crashes();

            ds = dal_Crashes.GetLoggedCrashes(eL_Crashes);

            var tblCommonData = new List<EL_CrashesTrackingData>();

            var tblEventsAssetDetails = new List<EL_CrashesAssetData>();

            var tbl_count = 0;

            foreach (DataTable dt in ds.Tables)
            {
                if (tbl_count == 0)
                    foreach (DataRow dr in dt.Rows)
                        tblCommonData.Add(new EL_CrashesTrackingData
                        {
                            dGPSDateTime = Convert.ToDateTime(dr["dGPSDateTime"]),
                            ipkCommanTrackingID = Convert.ToInt64(dr["ipkCommanTrackingID"]),
                            vTextMessage = Convert.ToString(dr["vTextMessage"]),
                            vVehicleSpeed = Convert.ToDouble(dr["vVehicleSpeed"]),


                            bIsIgnitionOn = Convert.ToBoolean(dr["bIsIgnitionOn"]),
                            vLatitude = Convert.ToDouble(dr["vLatitude"]),
                            vLongitude = Convert.ToDouble(dr["vLongitude"]),
                            vRoadSpeed = Convert.ToDouble(dr["vRoadSpeed"]),
                            vHeading = Convert.ToInt32(dr["vHeading"]),
                        });


                if (tbl_count == 1)
                    foreach (DataRow dr in dt.Rows)
                        tblEventsAssetDetails.Add(new EL_CrashesAssetData
                        {
                            vDeviceName = Convert.ToString(dr["vDeviceName"]),
                            CommentsCount = Convert.ToInt32(dr["CommentsCount"]),
                            Make = Convert.ToString(dr["Make"]),
                            ifkCommanTrackingID = Convert.ToInt64(dr["ifkCommanTrackingID"]),
                            isLogDismissed = Convert.ToBoolean(dr["isLogDismissed"]),
                            ipkCrashLogID = Convert.ToInt64(dr["ipkCrashLogID"]),

                        
                        });



                tbl_count += 1;
            }

            var _LoggedCrashes = from asset in tblEventsAssetDetails
                                 join common_data in tblCommonData on asset.ifkCommanTrackingID equals common_data.ipkCommanTrackingID into temp
                                 from _common in temp.DefaultIfEmpty()
                                 where asset.isLogDismissed
                                 select new
                                 {
                                     _common?.dGPSDateTime,
                                     _common?.ipkCommanTrackingID,
                                     _common?.vTextMessage,
                                     _common?.vVehicleSpeed,
                                     _common?.bIsIgnitionOn,
                                     _common?.vLatitude,
                                     _common?.vLongitude,
                                     _common?.vRoadSpeed,
                                     _common?.vHeading,

                                     asset.vDeviceName,
                                     asset.CommentsCount,
                                     asset.Make,
                                     asset.ifkCommanTrackingID,
                                     asset.ipkCrashLogID
                                 };




            var data = new
            {
                DismissedCrashes = _LoggedCrashes
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }


        public string LogACrash(EL_Crashes eL_Crashes)
        {

            string result = "";
            DAL_Crashes dal_Crashes = new DAL_Crashes();

            result = dal_Crashes.LogACrash(eL_Crashes);
            return result;
        }

        public string DismissACrash(EL_Crashes eL_Crashes)
        {

            string result = "";
            DAL_Crashes dal_Crashes = new DAL_Crashes();

            result = dal_Crashes.DismissACrash(eL_Crashes);
            return result;
        }
        public string AddLogComment(EL_Crashes eL_Crashes)
        {

            string result = "";
            DAL_Crashes dal_Crashes = new DAL_Crashes();

            result = dal_Crashes.AddLogComment(eL_Crashes);
            return result;
        }

        public string GetCrashLogDetails(EL_Crashes eL_Crashes)
        {

            string result = "";

            DataSet ds = new DataSet();
            DataTable _CrashesLogDetails = new DataTable();
            DataTable _CrashesLog = new DataTable();
            DataTable _CrashesLogComments = new DataTable();
            DataTable _CrashesLogImages = new DataTable();

            DAL_Crashes dal_Crashes = new DAL_Crashes();

            ds = dal_Crashes.GetCrashLogDetails(eL_Crashes);

            //Add EventName
            Bal_CommonEvents bal = new Bal_CommonEvents();
            DataSet ds1 = new DataSet();

            var temp_tbl = ds.Tables[1].Copy();

            ds1.Tables.Add(temp_tbl);

            _CrashesLog = bal.GetDataTableWithEventName(ds1, "vEventName", "vReportID");

            _CrashesLogDetails = ds.Tables[0].Copy();
            _CrashesLog = ds.Tables[1].Copy();
            _CrashesLogComments = ds.Tables[2].Copy();
            _CrashesLogImages = ds.Tables[3].Copy();

            var data = new
            {
                CrashesLogDetails = _CrashesLogDetails,
                CrashesLog = _CrashesLog,
                CrashesLogComments = _CrashesLogComments,
                CrashesLogImages = _CrashesLogImages
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);
            return result;
        }

        public string GetEventsOnCrash(EL_Crashes eL_Crashes)
        {

            string result = "";

            DataSet ds = new DataSet();
            DataTable _CrashesEvents = new DataTable();
           


            DAL_Crashes dal_Crashes = new DAL_Crashes();

            ds = dal_Crashes.GetEventsOnCrash(eL_Crashes);

            //Add EventName
            Bal_CommonEvents bal = new Bal_CommonEvents();
            _CrashesEvents = bal.GetDataTableWithEventName(ds, "vEventName", "vReportID");


            var data = new
            {
                CrashesEvents = _CrashesEvents
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);
            return result;
        }

        public string MarkAsImpactEvent(EL_Crashes eL_Crashes)
        {

            string result = "";
            DAL_Crashes dal_Crashes = new DAL_Crashes();

            result = dal_Crashes.MarkAsImpactEvent(eL_Crashes);
            return result;
        }

    }
}
