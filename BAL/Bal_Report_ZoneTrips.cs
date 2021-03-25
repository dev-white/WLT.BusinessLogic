//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Threading.Tasks;
//using Whitelabeltracking.DataAccessLayer.DAL;

//namespace Whitelabeltracking.BusinessLogic.BAL
//{
//    class Bal_report_ZoneTrips
//    {


//        public void getTrips()
//        {
//            int userid = 0,  reportId =0;

//            //get all data first 
//            var ds = ReportsDataAccess.GetZoneTrips(userid, reportId);


//            //get distinct device List

//            var DeviceList = (from q in ds.Tables[0].AsEnumerable()
//                              select q["vpkDeviceID"]).Distinct();

//            // filter trips  per asset data

//            //foreach (var ID in deviceList)
//            //{
//            //    //Filter the database value and get data per asset  int variable @deviceList
//            //    var data = ds.Tables[0].Select("vpkDeviceID = " + ID);

//            //    //merge the processed data to current main table
//            //    dt.Merge(GetAssetSpecificData(data));

//            //    //now get the unfinished  data from the dictionary( the data without end times)               
//            //    foreach (var incompleteData in geoZoneLookUpDictionary)
//            //    {

//            //        dt.Rows.Add(DatarowCreator(incompleteData.Key, geoZoneLookUpDictionary).ItemArray);


//            //    }
//            //    geoZoneLookUpDictionary.Clear();

//            }




//        }
//    }
//}
