using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_GeoCoding
    {       

        public string SearchPlaces(EL_GeoSearch eL_GeoSearch)
        {

            string xresult = string.Empty;

            var nominatimUrl = AppConfiguration.Configuration().NominatimSearchUrl;

            Uri address = new Uri(nominatimUrl + "search?q=" + eL_GeoSearch.placeName + "&format=json&limit=" + eL_GeoSearch.maxRows);
            HttpWebRequest request;
            StreamReader reader;
            StringBuilder sbSource;

            // Create and initialize the web request  
            request = WebRequest.Create(address) as HttpWebRequest;
            request.UserAgent = "WebApp";
            request.KeepAlive = false;

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

                        // Read it into a StringBuilder  
                        sbSource = new StringBuilder(reader.ReadToEnd());

                        xresult = sbSource.ToString();

                        //convert to an xml stream
                        reader.Close();
                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogError.RegisterErrorInLogFile( "Bal_GeoCoding.cs", "SearchPlaces()", ex.Message  + ex.StackTrace);

                    }
                }

            }

            return xresult;
        }

        public string ReverseGeocode(EL_GeoSearch eL_GeoSearch)
        {

            string xresult = string.Empty;

            var nominatimUrl = AppConfiguration.Configuration().NominatimSearchUrl;

            Uri address = new Uri(nominatimUrl + "reverse?q=format=html&lat=" + eL_GeoSearch.lat + "&lon=" + eL_GeoSearch.lon + "&zoom=" + eL_GeoSearch.zoom + "&addressdetails=1&extratags=1");
            HttpWebRequest request;
            StreamReader reader;
            StringBuilder sbSource;

            // Create and initialize the web request  
            request = WebRequest.Create(address) as HttpWebRequest;
            //request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "GET";
            request.UserAgent = "WebApp";
            request.KeepAlive = false;

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

                       
                        NominatimResult myojb = JsonConvert.DeserializeObject<NominatimResult>(objText);

                        xresult = JsonConvert.SerializeObject(myojb, Formatting.Indented);


                        //convert to an xml stream
                        reader.Close();
                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogError.RegisterErrorInLogFile( "Bal_GeoCoding.cs", "ReverseGeocode()", ex.Message  + ex.StackTrace);

                    }
                }

            }


            return xresult;
        }

        public class Address
        {
            public string suburb { get; set; }
            public string county { get; set; }
            public string state_district { get; set; }
            public string state { get; set; }
            public string postcode { get; set; }
            public string country { get; set; }
            public string country_code { get; set; }
        }

        public class Extratags
        {
            public string maxspeed { get; set; }
        }

        public class NominatimResult
        {
            public string place_id { get; set; }
            public string licence { get; set; }
            public string osm_type { get; set; }
            public string osm_id { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string display_name { get; set; }
            public Address address { get; set; }
            public Extratags extratags { get; set; }
            public List<string> boundingbox { get; set; }
        }


    }

}
