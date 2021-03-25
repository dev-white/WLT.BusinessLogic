

using GeoCoordinatePortable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using WLT.BusinessLogic.BAL.DBSCAN;
using WLT.EntityLayer;
using WLT.EntityLayer.API_Entities;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;
using static WLT.BusinessLogic.BAL.Bal_GeoCoding;

namespace wlt.businesslogic.bal
{
    public class Bal_Report_Historical_Location_Probability
    {

        public string ReverseGeocode(string lat, string lon, int zoom = 18)
        {

            string xresult = string.Empty;

            var nominatimUrl = AppConfiguration.Configuration().NominatimUrl;

            Uri address = new Uri(nominatimUrl + "reverse?format=json&lat=" + lat + "&lon=" + lon + "&zoom=" + zoom + "&addressdetails=1&extratags=1");
            HttpWebRequest request;
            StreamReader reader;
            StringBuilder sbSource;

            // Create and initialize the web request  
            request = WebRequest.Create(address) as HttpWebRequest;
            //request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "GET";
            request.UserAgent = "WebApp";
            request.KeepAlive = true;

            request.Timeout = 15 * 1000;

            // Get response                

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                if (request.HaveResponse == true && response != null)
                {
                    try
                    {
                        // Get the response stream  
                        reader = new StreamReader(response.GetResponseStream());
                      
                        var objText = reader.ReadToEnd();

                        NominatimResult myojb = JsonConvert.DeserializeObject<NominatimResult>(objText);//  (NominatimResult)js.Deserialize(objText, typeof(NominatimResult));


                        xresult = JsonConvert.SerializeObject(myojb);

                        //convert to an xml stream
                        reader.Close();
                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogError.RegisterErrorInLogFile("Bal_Report_Historical_Location_Probability.cs", "ReverseGeocode()", ex.Message  + ex.StackTrace);

                    }
                }

            }


            return xresult;
        }

        public string GetReportDataAsync(El_Report El_Report)
        {

            var ds = new DataSet();

            var _DAL_Reports = new  WLT.DataAccessLayer.DAL.DAL_Reports();

            ds = _DAL_Reports.Get_Report_Historical_Location_ProbabilityData(El_Report);

            var HeaderRow = ds.Tables[1].Copy();

            string display_name = "";

            DatasetItem[] featureData = { };

            List<DatasetItem> _rawPoints = new List<DatasetItem>();

            foreach (DataRow _row in ds.Tables[0].Rows)
            {
                _rawPoints.Add(new DatasetItem(Convert.ToDouble(_row["lat"]), Convert.ToDouble(_row["lon"])));
            }


            featureData = _rawPoints.ToArray();

            HashSet<DatasetItem[]> clusters;

            //    var dbs = new DbscanAlgorithm<MyCustomDatasetItem>((x, y) => Math.Sqrt(((x.X - y.X) * (x.X - y.X)) + ((x.Y - y.Y) * (x.Y - y.Y))));

            var dbs1 = new Dbscan<DatasetItem>((x, y) => new GeoCoordinate(x.X, x.Y).GetDistanceTo(new GeoCoordinate(y.X, y.Y)));

            dbs1.ComputeClusterDbscan(allPoints: featureData, epsilon: 400, minPts: 5, clusters: out clusters);

            var Centroid = new DataTable();

            Centroid.Columns.Add("Cluster", typeof(int));
            Centroid.Columns.Add("Location", typeof(string));
            Centroid.Columns.Add("lat", typeof(double));
            Centroid.Columns.Add("lon", typeof(double));
            Centroid.Columns.Add("NoOfPoints", typeof(int));
            Centroid.Columns.Add("parcentage", typeof(double));

            int i = 0;

            foreach (var Cluster in clusters)
            {

                var _ResultCoodinates = GetCentralGeoCoordinate(Cluster);

                var result = ReverseGeocode(_ResultCoodinates.Latitude.ToString(), _ResultCoodinates.Longitude.ToString());

                if (!result.StartsWith("{"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    display_name = doc?.InnerText;
                }
                else
                {
                    var obj = JsonConvert.DeserializeObject<NominatimRootobject>(result);
                    display_name = obj.display_name;
                }


                Centroid.Rows.Add(i,
                                  display_name,//Location Name placeholder
                                 _ResultCoodinates.Latitude,
                                 _ResultCoodinates.Longitude,
                                 Cluster.Count(),
                                 0
                                 );
                i += 1;
            }

            Centroid.DefaultView.Sort = "NoOfPoints desc";

            Centroid = Centroid.DefaultView.ToTable();

            var ResultObject = new
            {
                HeaderRow,
                clusters,
                Centroid
            };

            return JsonConvert.SerializeObject(ResultObject);
        }

        public static GeoCoordinate GetCentralGeoCoordinate(DatasetItem[] _clusterPoints)
        {
            var geoCoordinates = new List<GeoCoordinate>();

            foreach (var itemPoint in _clusterPoints)
            {
                geoCoordinates.Add(new GeoCoordinate(itemPoint.X, itemPoint.Y));
            }

            if (geoCoordinates.Count == 1)
            {
                return geoCoordinates.Single();
            }

            double x = 0;
            double y = 0;
            double z = 0;

            foreach (var geoCoordinate in geoCoordinates)
            {
                var latitude = geoCoordinate.Latitude * Math.PI / 180;
                var longitude = geoCoordinate.Longitude * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
            }

            var total = geoCoordinates.Count;

            x = x / total;
            y = y / total;
            z = z / total;

            var centralLongitude = Math.Atan2(y, x);
            var centralSquareRoot = Math.Sqrt(x * x + y * y);
            var centralLatitude = Math.Atan2(z, centralSquareRoot);

            return new GeoCoordinate(centralLatitude * 180 / Math.PI, centralLongitude * 180 / Math.PI);
        }
    }


}



