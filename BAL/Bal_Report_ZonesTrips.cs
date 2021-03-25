using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using WLT.DataAccessLayer.DAL;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
public    class Bal_Report_ZonesTrips
    {
       public int Userid { get; set; }
        public int ReportId { get; set; }
        public string  TimeZoneID { get; set; }



        public DataSet  GetZoneTrips()
        {

            var _DAL_Reports = new DAL_Reports();

            var dt = SourceDefinition().Clone();

            //Fetch row data
            var ds = _DAL_Reports.GetZoneTrips(Userid, ReportId);

            //create a lookup dictionary 
            var lkUpdict = new Dictionary<int, List<string>>();

            int length = ds.Tables[0].Rows.Count;
           
           
            var data = ds.Tables[0];

            try
            {
                if (length != 0)
                {

                    var unit = (int)ds.Tables[1].Rows[0]["Unit"];

                    var isAssetGrouped = Convert.ToBoolean(ds.Tables[1].Rows[0]["GroupByAsset"]);

                    for (int i = 0; i < length; i++)
                    {

                        var code = -1;

                        if (!isAssetGrouped)
                        {
                            code = ApplyCode(data.Rows[i], lkUpdict, i);
                        }
                        var Time = TimeSpan.FromSeconds(Convert.ToInt32(data.Rows[i]["Duration"]));

                        var AvgSpeed = 0.0;

                        if (Time.TotalSeconds != 0)
                        {
                            AvgSpeed = Convert.ToDouble(data.Rows[i]["Distance"]) / Time.TotalHours;
                        }

                        DataRow dr = dt.NewRow();

                        dr["Asset"] = data.Rows[i]["Asset"];

                        dr["Lat"] = data.Rows[i]["vLatitude"];
                        dr["Lon"] = data.Rows[i]["vLongitude"];

                        dr["startLocation"] = data.Rows[i]["StartLocation"];
                        dr["Destination"] = data.Rows[i]["Destination"];
                        dr["StartTime"] = Convert.ToDateTime((data.Rows[i]["StartTime"]));// ToString("dd MMM yyyy HH:mm:ss");
                        dr["EndTime"] = Convert.ToDateTime(data.Rows[i]["EntryTime"]);//.ToString("dd MMM yyyy HH:mm:ss");
                        dr["Duration"] = Time;
                        dr["TotalDuration"] = Time;
                        dr["Trip"] = code;
                        if (code != -1)
                        {
                            dr["LocationPair"] = lkUpdict[code][0] + "  to  " + lkUpdict[code][1];
                        }
                        dr["Distance"] = UserSettings.ConvertKMsToXxOdoMeter(unit, data.Rows[i]["Distance"].ToString(), true, 2);
                        dr["AvgSpeed"] = UserSettings.ConvertKMsToXx(unit, AvgSpeed.ToString(), true, 2);


                        dt.Rows.Add(dr);



                    }
                }
            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( " WLT.BusinessLogic.BAL.Bal_Report_ZonesTrips.cs", "GetZoneTrips()", ex.Message  + ex.StackTrace);

            }

            //  var singleAssetdata = SpecificAssetData(ds.Tables[0],lkUpdict);


            ds.Tables.RemoveAt(0);

            ds.Tables.Add(dt);

            return ds;
        }
         
 
        public DataTable SourceDefinition  () {


            var dt = new DataTable();
            dt.Columns.Add("Asset", typeof(string));
            dt.Columns.Add("startLocation",typeof (string ));
            dt.Columns.Add("Destination", typeof(string));
            dt.Columns.Add("StartTime", typeof(DateTime));
            dt.Columns.Add("EndTime", typeof(DateTime));
            dt.Columns.Add("Duration", typeof(TimeSpan));
            dt.Columns.Add("TotalDuration", typeof(string));
            dt.Columns.Add("Trip", typeof(int));
            dt.Columns.Add("LocationPair", typeof(string));
            dt.Columns.Add("Distance", typeof(string));
            dt.Columns.Add("AvgSpeed", typeof(string));
            dt.Columns.Add("Lon", typeof(double));
            dt.Columns.Add("Lat", typeof(double));

            return dt;

        }


        public int ApplyCode( object o, Dictionary<int, List<string>> codeDictionary,int i)
        {
            //var codeDictionary = new Dictionary<int , keyValueZoneTripsObject>();
            var code = i;

            var row = (DataRow)o;


            var startLoc = row["StartLocation"].ToString();

            var endLoc =  row["Destination"].ToString();

           
            if (codeDictionary.Count > 0)
            {

                 var key = -1;
                int l = 0;
                foreach (var item in codeDictionary.Values)
                {                    
                    if (item.Any(x => x == startLoc) && item.Any(x => x == endLoc))
                    {
                        key = getKey(codeDictionary, item);
                        break;
                    }
                    l++;
                }

                if (!codeDictionary.ContainsKey(key))
                {
                    codeDictionary.Add(i, new List<string> { startLoc, endLoc });
                }
                else
                {
                    
                    code = key;
                }
            }
            else
            {
                codeDictionary.Add(i, new List<string> { startLoc, endLoc });

            }

            return code;
        }

        private static int getKey(Dictionary<int, List<string>> codeDictionary, List<string> item)
        {
            int key;
            {
                key = codeDictionary.FirstOrDefault(x => x.Value == item).Key;
            }

            return key;
        }
    }
    //public class keyValueZoneTripsObject {

    //    public string StartLocation { get; set; }
    //    public string Destination { get; set; }
    //    public string vpkReportID { get; set; }

    //}


    }

