using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
  public  class Bal_Heatmap
    {
      public  Bal_Heatmap() { }



        public El_Heatmap Getmapinfo(int operation, long reportId, int userID,string TimeZoneID,string CultureID)
        {
            var _DAL_Heatmap = new DAL_Heatmap();

            var ds = _DAL_Heatmap.FetchData( operation,  reportId,  userID);

            var _GeoJson = new El_Heatmap();

            var _header = ds.Tables[0].Rows[0];

            DateTime currentdate = DateTime.UtcNow;

            DateTime dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(currentdate), "UTC", TimeZoneID);

            _GeoJson.ReportName = Convert.ToString(_header["reportName"]);                          

            _GeoJson.Reporting_Date = dt.ToString("dddd dd MMMM yyyy HH:mm:ss", new CultureInfo(CultureID));

            _GeoJson.startDate = Convert.ToDateTime(_header["dStartDate"]).ToString("dddd dd MMMM yyyy HH:mm:ss", new CultureInfo(CultureID));

            _GeoJson.EndDate = Convert.ToDateTime(_header["dEndDate"]).ToString("dddd dd MMMM yyyy HH:mm:ss", new CultureInfo(CultureID));

            _GeoJson.logo = ReturnImage(ds.Tables[1].Rows[0]["vLogo"]);

            _GeoJson.Asset = ReportExtensions.ChangeAssetHeaderLanguageString(CultureID, Convert.ToString(_header["Asset"]), 59);    

            foreach (DataRow row in ds.Tables[2].Rows)
            {
                _GeoJson.List.Add( new El_Heatmap {

                    Lon = Convert.ToString(row["vLongitude"]),
                    Lat = Convert.ToString(row["vLatitude"]),
                   Weight =  Convert.ToDouble(row["Weight"]),
                   
                });


            }


            return _GeoJson;
        }

        public string ReturnImage(object logo)
        {

            string _image = string.Empty;

            if (logo != DBNull.Value)
            {
                var _imageUrl = logo.ToString();

                _image = string.Format("<img   class = 'serverImage' src = '{0}' />", _imageUrl);

            }

            return _image;
        }

    }
}
