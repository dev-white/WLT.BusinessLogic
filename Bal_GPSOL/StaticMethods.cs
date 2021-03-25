using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public static class StaticMethods
    {

        static string f_strConnectionString  = AppConfiguration.Getwlt_WebAppConnectionString();
        public static Image ScaleImageInAsset(Image image, int maxHeight)
        {

            //if (image.Width < image.Height)
            //{
            //    var width = Math.Round(image.Width / (image.Height / maxHeight));
            //}
            //else if (image.Height < image.Width)
            //{
            //    var height = Math.round(image.Height / (image.Width / maxHeight));
            //}

            int sourceWidth = image.Width;
            int sourceHeight = image.Height;

            float scale = 0;

            scale = (maxHeight / (float)sourceHeight);

            int newWidth = (int)(sourceWidth * scale);
            int newHeight = (int)(sourceHeight * scale);


            //var ratio = (double)maxHeight / image.Height;

            //var newWidth = (int)(image.Width * ratio);
            //var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                // gr.DrawImage(newImage, new Rectangle(0, 0, NewWidth, NewHeight));                
                gr.DrawImage(image, new Rectangle(0, 0, newWidth, maxHeight));
                gr.Dispose();
            }

            return newImage;
        }
        public static DateTime ConvertstrDate(string date) // i.e  20131021112929[yyyyMMddhhmmss]
        {
            string str = date.Substring(0, 4);
            string str1 = date.Substring(4, 2);
            string str2 = date.Substring(6, 2);
            string str3 = date.Substring(8, 2);
            string str4 = date.Substring(10, 2);
            string str5 = date.Substring(12, 2);
            string[] strArrays = new string[] { str2, "/", str1, "/", str, " ", str3, ":", str4, ":", str5 };
            return DateTime.Parse(string.Concat(strArrays));
        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }
        
        public static string ChangeDateFormat(string date, bool isYear)
        {
            DateTime changedate = new DateTime();
            changedate = Convert.ToDateTime(date);
            string dday = Convert.ToString(changedate.Day);

            int ddday = Convert.ToInt32(dday.Substring(dday.Length - 1, 1));
            if (isYear)
            {
                if (Convert.ToInt32(dday) >= 10 && Convert.ToInt32(dday) <= 19)
                {
                    return changedate.Day + "th " + GetMonthinWord(changedate.Month) + " " + changedate.Year + " " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));
                }
                else
                {
                    switch (ddday)
                    {
                        case 0:
                            return changedate.Day + " " + GetMonthinWord(changedate.Month) + " " + changedate.Year + " " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));

                        case 1:
                            return changedate.Day + "st " + GetMonthinWord(changedate.Month) + " " + changedate.Year + " " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));

                        case 2:
                            return changedate.Day + "nd " + GetMonthinWord(changedate.Month) + " " + changedate.Year + " " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));

                        case 3:
                            return changedate.Day + "rd " + GetMonthinWord(changedate.Month) + " " + changedate.Year + " " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));

                        default:
                            return changedate.Day + "th " + GetMonthinWord(changedate.Month) + " " + changedate.Year + " " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));
                    }

                }
            }
            else
            {

                if (Convert.ToInt32(dday) >= 10 && Convert.ToInt32(dday) <= 19)
                {
                    return changedate.Day + "th " + GetMonthinWord(changedate.Month) + "  " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));
                }
                else
                {
                    switch (ddday)
                    {
                        case 0:
                            return changedate.Day + " " + GetMonthinWord(changedate.Month) + " " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));

                        case 1:
                            return changedate.Day + "st " + GetMonthinWord(changedate.Month) + "  " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));

                        case 2:
                            return changedate.Day + "nd " + GetMonthinWord(changedate.Month) + "  " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));

                        case 3:
                            return changedate.Day + "rd " + GetMonthinWord(changedate.Month) + "  " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));

                        default:
                            return changedate.Day + "th " + GetMonthinWord(changedate.Month) + "  " + (changedate.Hour.CompareTo(10) >= 0 ? Convert.ToString(changedate.Hour) : Convert.ToString("0" + Convert.ToString(changedate.Hour))) + ":" + (changedate.Minute.CompareTo(10) >= 0 ? Convert.ToString(changedate.Minute) : Convert.ToString("0" + Convert.ToString(changedate.Minute))) + ":" + (changedate.Second.CompareTo(10) >= 0 ? Convert.ToString(changedate.Second) : Convert.ToString("0" + Convert.ToString(changedate.Second)));
                    }
                }
            }
        }
        public static string GetMonthinWord(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan";

                case 2:
                    return "Feb";

                case 3:
                    return "Mar";

                case 4:
                    return "Apr";

                case 5:
                    return "May";

                case 6:
                    return "Jun";

                case 7:
                    return "Jul";

                case 8:
                    return "Aug";

                case 9:
                    return "Sept";
                case 10:
                    return "Oct";

                case 11:
                    return "Nov";

                case 12:
                    return "Dec";

                default:
                    return "";
            }

        }
        public static void GenerateImagesforAssetsnDrivers()
        {
            DataSet ds = new DataSet();
            string filePath = AppConfiguration.Configuration().AssetPhotoFolderPath;
            string resizeFilePath = AppConfiguration.Configuration().AssetPhotoResizeFolderPath;

            ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, "SELECT * FROM wlt_tblAssets A inner join wlt_tblDevices B on A.Id=ifk_AssignedAssetId where vLogo IS NOT NULL AND vLogoName IS NOT NULL");

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string filename = row["vLogoName"].ToString();

                    //if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(resizeFilePath + "\\" + filename)))
                    //{
                    //    using (System.IO.FileStream fs = new System.IO.FileStream(System.Web.HttpContext.Current.Server.MapPath(resizeFilePath + "\\" + filename), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    //    {
                    //        Byte[] logobytes = (Byte[])row["vLogo"];
                    //        fs.Write(logobytes, 0, logobytes.Length);
                    //        fs.Close();
                    //    }
                    //}
                }
            }

            ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.Text, "SELECT * FROM wlt_tblAssets_Driver where vLogo IS NOT NULL AND vLogoName IS NOT NULL");

            if (ds.Tables.Count > 0)
            {

                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    string filename = row["vLogoName"].ToString();

                    //if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(resizeFilePath + "\\" + filename)))
                    //{
                    //    using (System.IO.FileStream fs = new System.IO.FileStream(System.Web.HttpContext.Current.Server.MapPath(resizeFilePath + "\\" + filename), System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    //    {
                    //        Byte[] logobytes = (Byte[])row["vLogo"];
                    //        fs.Write(logobytes, 0, logobytes.Length);
                    //        fs.Close();
                    //    }
                    //}
                }
            }


        }
    }
}