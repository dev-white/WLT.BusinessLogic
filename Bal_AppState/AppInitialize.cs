using CloudinaryDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.BusinessLogic.wlt_Login;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_AppState
{
    public class AppInitialize
    {

        private readonly wlt_Config _wlt_AppConfig;
        private string _ContentRoot;

        public AppInitialize(string ContentRoot)
        {


            _wlt_AppConfig = AppConfiguration.GetAppSettings<wlt_Config>("wlt_config");

            _ContentRoot = ContentRoot;
        }

        public void InitiliazePage(wlt_SessionPOCO _wlt_SessionPOCO, ref wltAppState objRegisteration, string vThemeName)
        {
            try
            {
                ObjectCache Cache = MemoryCache.Default;

                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();

                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1);
            

                if (vThemeName != "")
                {
                    //set the attributes for dynamic css object
                    //string theme = objRegisteration.vThemeName;

                    StringBuilder themeUrl = new StringBuilder();

                    themeUrl.Append("App_Themes/");
                    themeUrl.Append(vThemeName);
                    themeUrl.Append("/");
                    themeUrl.Append(vThemeName);
                    themeUrl.Append(".min.css");

                    objRegisteration.ThemeUrl = "/" + themeUrl + "?v=" + _wlt_AppConfig.AppVersion;

                }
                //if theme session doesn't contain any value
                else
                {
                    objRegisteration.ThemeUrl = "/css/KenDoUi/kendo.metro.min.css";
                }

                //PWA Manifest Settings
                int resellerId = _wlt_SessionPOCO.Reseller.ResellerID;


                bool isManifestExists = false;
                bool isPwaEnabled = false;
                var contentRoot = _ContentRoot;


                isManifestExists = File.Exists(contentRoot + "/manifest/manifest_" + resellerId + ".json");

                DataSet ds1 = new DataSet();

                var clsClientDevice = new clsClientDevice();

                clsClientDevice.ifkResellerID = resellerId;
                clsClientDevice.Operation = 2;


                //PWA Settings Cache
                if (Cache["pwa_" + resellerId] == null)
                {

                    Cache.Add("pwa_" + resellerId, clsClientDevice.GetPwaSettings(), cacheItemPolicy);
                }

                ds1 = (DataSet)Cache["pwa_" + resellerId];


                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0) //Get to Know if PWA is enabled
                {
                    isPwaEnabled = Convert.ToBoolean(ds1.Tables[0].Rows[0]["PwaIsEnabled"]);
                }

                if (objRegisteration.ifkUserTypeID == 2 || !isManifestExists) //Create Manifest only when a Reseller User logs in
                {
                    //Create PWA Web Manifest File
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {

                        if (isPwaEnabled)
                        {
                            try
                            {
                                Account account = new Account(
                                    "tracking",
                                    "238284973827697",
                                    "aQSyor0x8vcmcB2ZVXQJg1tFU54");

                                Cloudinary cloudinary = new Cloudinary(account);

                                var image = "pwalogos/" +
                                            Convert.ToString(ds1.Tables[0].Rows[0]["PwaLogoUrl"]).Split('/')[8];

                                var url48 = cloudinary.Api.UrlImgUp.Secure(true)
                                           .Transform(new Transformation().Width(48).Height(48).Crop("scale"))
                                           .BuildImageTag(image);
                                var url72 = cloudinary.Api.UrlImgUp.Secure(true)
                                 .Transform(new Transformation().Width(72).Height(72).Crop("scale"))
                                 .BuildImageTag(image);
                                var url96 = cloudinary.Api.UrlImgUp.Secure(true)
                                    .Transform(new Transformation().Width(96).Height(96).Crop("scale"))
                                    .BuildImageTag(image);
                                var url120 = cloudinary.Api.UrlImgUp.Secure(true)
                                    .Transform(new Transformation().Width(120).Height(120).Crop("scale"))
                                    .BuildImageTag(image);
                                var url128 = cloudinary.Api.UrlImgUp.Secure(true)
                                    .Transform(new Transformation().Width(128).Height(128).Crop("scale"))
                                    .BuildImageTag(image);
                                var url152 = cloudinary.Api.UrlImgUp.Secure(true)
                                    .Transform(new Transformation().Width(152).Height(152).Crop("scale"))
                                    .BuildImageTag(image);
                                var url144 = cloudinary.Api.UrlImgUp.Secure(true)
                                    .Transform(new Transformation().Width(144).Height(144).Crop("scale"))
                                    .BuildImageTag(image);
                                var url180 = cloudinary.Api.UrlImgUp.Secure(true)
                                   .Transform(new Transformation().Width(180).Height(180).Crop("scale"))
                                   .BuildImageTag(image);
                                var url192 = cloudinary.Api.UrlImgUp.Secure(true)
                                    .Transform(new Transformation().Width(192).Height(192).Crop("scale"))
                                    .BuildImageTag(image);
                                var url384 = cloudinary.Api.UrlImgUp.Secure(true)
                                    .Transform(new Transformation().Width(384).Height(384).Crop("scale"))
                                    .BuildImageTag(image);
                                var url512 = cloudinary.Api.UrlImgUp.Secure(true)
                                 .Transform(new Transformation().Width(512).Height(512).Crop("scale"))
                                 .BuildImageTag(image);

                                List<Icons> icons = new List<Icons>();

                                icons.Add(new Icons(url48.Split('"')[1], "image/png", "48x48"));
                                icons.Add(new Icons(url72.Split('"')[1], "image/png", "72x72"));
                                icons.Add(new Icons(url96.Split('"')[1], "image/png", "96x96"));
                                icons.Add(new Icons(url120.Split('"')[1], "image/png", "120x120"));
                                icons.Add(new Icons(url128.Split('"')[1], "image/png", "128x128"));
                                icons.Add(new Icons(url144.Split('"')[1], "image/png", "144x144"));
                                icons.Add(new Icons(url152.Split('"')[1], "image/png", "152x152"));
                                icons.Add(new Icons(url180.Split('"')[1], "image/png", "180x180"));
                                icons.Add(new Icons(url192.Split('"')[1], "image/png", "192x192"));
                                icons.Add(new Icons(url384.Split('"')[1], "image/png", "384x384"));
                                icons.Add(new Icons(url512.Split('"')[1], "image/png", "512x512"));

                                ManifestData data = new ManifestData();

                                data.short_name = Convert.ToString(ds1.Tables[0].Rows[0]["PwaName"]);
                                data.name = Convert.ToString(ds1.Tables[0].Rows[0]["PwaName"]);
                                data.icons = icons;
                                data.start_url = "/";
                                data.display = "standalone";
                                data.background_color =
                                    Convert.ToString(ds1.Tables[0].Rows[0]["PwaBackgroundColor"]);
                                data.theme_color = Convert.ToString(ds1.Tables[0].Rows[0]["PwaThemeColor"]);
                                data.description = Convert.ToString(ds1.Tables[0].Rows[0]["PwaDesc"]);
                                data.orientation = "any";

                                string json = JsonConvert.SerializeObject(data);

                                File.WriteAllText(contentRoot + "\\manifest\\manifest_" + resellerId + ".json", json);

                                isManifestExists = File.Exists(contentRoot + "\\manifest\\manifest_" + resellerId + ".json");

                                //Fix for ios 14
                                objRegisteration.PwaIcon = url192.Split('"')[1];
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }

                    //End of creating Web Manifest
                }

                if (isPwaEnabled && isManifestExists) //If Manifest File exist and PWA is Enabled
                {

                    objRegisteration.ManifestUrl = "\\manifest\\manifest_" + resellerId + ".json" + "?v=" + DateTime.Now.Ticks;

                }
                else
                {
                    objRegisteration.ManifestUrl = "";
                    objRegisteration.PwaIcon = "";
                }


                objRegisteration.hndifkMeasurementUnit = Convert.ToString(objRegisteration.ifkMeasurementUnit);

                objRegisteration.hndUserType = Convert.ToString(objRegisteration.ifkUserTypeID);

                objRegisteration.hndDefaultClient = Convert.ToString(objRegisteration.ifkDefaultClient);

                objRegisteration.hndCompanyID = Convert.ToString(objRegisteration.iParent);

                objRegisteration.hndLangugageCode = Convert.ToString(objRegisteration.ifkLanguageID);


                objRegisteration.CultureID = objRegisteration.LanguageDetails != null ? Convert.ToString(objRegisteration.LanguageDetails.CultureID) : "en-US";

                objRegisteration.hndResellerID = "";



                // all dependant on session
                objRegisteration.imgCmpHeaderLogo = Convert.ToString(_wlt_SessionPOCO.CompanyLogoName);
                objRegisteration.imgCmpFavicon = Convert.ToString(_wlt_SessionPOCO.CompanyFavicon);
                objRegisteration.PageHeading = Convert.ToString(_wlt_SessionPOCO.CompanyPageHeading);


                //Set Global Timezone Offset
                var UserTimeZone = TimeZoneInfo.FindSystemTimeZoneById(objRegisteration.vTimeZoneID);
                var UserTimeZoneOffset = "+00:00";

                var now = DateTimeOffset.UtcNow;
                TimeSpan UserOffset = UserTimeZone.GetUtcOffset(now);

                if (UserOffset.Hours > 0)
                {
                    UserTimeZoneOffset = "+" + UserOffset.ToString();
                    UserTimeZoneOffset = UserTimeZoneOffset.Substring(0, UserTimeZoneOffset.Length - 3);
                }
                else
                {
                    UserTimeZoneOffset = UserOffset.ToString();
                    UserTimeZoneOffset = UserTimeZoneOffset.Substring(0, UserTimeZoneOffset.Length - 3);
                }

                objRegisteration.hndUserTimeOffset = UserTimeZoneOffset;

                //End of Global TimeZone Offset

                DateTime dt = new DateTime();
                string helpUrl = "";
                //this variable is not been used in this block
                string GMTTIME = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("UTC")).ToString(); //Coordinated Universal Time

                //display name of person who has logged in
                //aLoginUserName.InnerText = objRegisteration.vName;

                //get Help Url
                var balLogin = new Bal_Login();

                if (Cache["help_" + objRegisteration.pkUserID] == null)
                {

                    Cache.Add("help_" + objRegisteration.pkUserID, balLogin.GetHelpUrl(objRegisteration), cacheItemPolicy);
                }

                helpUrl = Convert.ToString(Cache["help_" + objRegisteration.pkUserID]);


                //get the last login date to display in account settings popup
                DataSet ds = new DataSet();
                objRegisteration.Operation = 153;
                objRegisteration.pkUserID = objRegisteration.pkUserID;
                objRegisteration.ipkCompanyID = objRegisteration.ipkCompanyID;

                ds = balLogin.SelectLoginDateForSpan(1, 0, 0, objRegisteration);

                //if there is record for login
                if (ds.Tables.Count > 0)
                {
                    //if last login date is not there - if it's a first login
                    if (ds.Tables[0].Rows[0]["dLastLoginDate"].ToString() == "")
                    {
                        //if current login date is present - show current login datetime at the place of last login datetime in account settings popup
                        if ((ds.Tables[0].Rows[0]["dCurrentLoginDate"].ToString() != "") && (ds.Tables[0].Rows[0]["dCurrentLoginDate"].ToString() != null))
                        {
                            dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["dCurrentLoginDate"].ToString());
                        }
                        //show current time at the place of last login datetime in account settings popup
                        else
                        {
                            dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, objRegisteration.vTimeZoneID);
                        }
                    }
                    //if last login date is there
                    else
                    {
                        //if last login date is present - logically need not checked this again as it's been already checked in previous if block
                        if ((ds.Tables[0].Rows[0]["dLastLoginDate"].ToString() != "") && (ds.Tables[0].Rows[0]["dLastLoginDate"].ToString() != null))
                        {
                            dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["dLastLoginDate"].ToString());
                        }
                        else
                        {
                            dt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, objRegisteration.vTimeZoneID);
                        }

                    }

                }

                if (helpUrl != "")
                {
                    StringBuilder url = new StringBuilder();
                    url.AppendFormat("<a href='{0}' target='_blank' rel='noopener'><i class='' style='font-size: 16px; padding-right: 30px; top: 2px; position: relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M512 0Q373 0 255 68.5T68.5 255T0 512t68.5 257T255 955.5t257 68.5t257-68.5T955.5 769t68.5-257t-68.5-257T769 68.5T512 0zm128 83q54 16 102 45t86.5 67.5T896 282t45 102H733q-34-59-93-93V83zm64 429q0 80-56.5 136T512 704q-52 0-96-25.5t-70-70t-26-96.5q0-79 56.5-135.5T512 320q52 0 96 26t70 70t26 96zM448 69q32-5 64-5t64 5v196q-32-9-63.5-9t-64.5 9V69zm-64 14v208q-29 17-52.5 40.5T291 384H83q45-151 175-240q59-41 126-61zM64 512q0-8 .5-16l1-16l1.5-16l2-16h196q-9 32-9 64t8 64H69q-5-34-5-64zm320 429q-108-32-188.5-112.5T83 640h208q34 59 93 93v208zm192 14q-32 5-64 5t-64-5V760q33 8 64.5 8t63.5-8v195zm64-14V733q59-34 93-93h208q-22 72-65 132.5t-103.5 104T640 941zm120-365q4-16 6-32t2-32q0-32-9-64h196q5 34 5 64t-5 64H760z' fill='#c1bfd0'/></svg></i><span data-localize='HelpSupport'>Help & Support</span></a>", helpUrl);
                    objRegisteration.lblhelpUrl = url.ToString();
                    objRegisteration.lblhelpUrl2 = url.ToString();
                }

                if (Cache["CustomLinks_" + objRegisteration.pkUserID] == null)
                {

                    Cache.Add("CustomLinks_" + objRegisteration.pkUserID, balLogin.GetCustomLinks(objRegisteration), cacheItemPolicy);
                }

                string CustomLinks = Convert.ToString(Cache["CustomLinks_" + objRegisteration.pkUserID]);

                if (CustomLinks != "")
                {
                    //StringBuilder url = new StringBuilder();
                    //url.AppendFormat("<a href='{0}' target='_blank' rel='noopener'><span data-localize='HelpSupport'>Custom Links</span></a></div>", CustomLinks);
                    objRegisteration.logincustomlinks = CustomLinks; // url.ToString();
                    objRegisteration.logincustomlinks2 = CustomLinks;
                }


                objRegisteration.spanLoginUserName = objRegisteration.vName;
                objRegisteration.spanLoginUserEmail = objRegisteration.vEmail;
                objRegisteration.spanLoginUserName2 = objRegisteration.vName;
                objRegisteration.spanLoginUserEmail2 = objRegisteration.vEmail;

                Account cloudAccount = new Account(
                    "tracking",
                    "238284973827697",
                    "aQSyor0x8vcmcB2ZVXQJg1tFU54");

                Cloudinary cloud = new Cloudinary(cloudAccount);

                var userProfileImage = "/Images/man.svg";

                string firstLetters = "";

                foreach (var part in objRegisteration.vName.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries))
                {
                    firstLetters += part.Substring(0, 1);
                }

                if (objRegisteration.PhotoName.Split('/').Length > 6)
                {
                    var imageurl = "user/" +
                                   objRegisteration.PhotoName.Split('/')[8];

                    var transImage = cloud.Api.UrlImgUp.Secure(true)
                        .Transform(new Transformation().Width(60).Height(60).Crop("thumb").Radius("max").FetchFormat("png").Gravity("face"))
                        .BuildImageTag(imageurl);

                    userProfileImage = transImage.Split('"')[1];
                    objRegisteration.spanUserPhoto = "<img class='img-circle' src='" + userProfileImage + "' />";
                    objRegisteration.spanUserPhoto1 = "<img class='img-circle' src='" + userProfileImage + "' />";
                }
                else
                {
                    objRegisteration.spanUserPhoto = "<span class='user-badge2'>" + firstLetters + "</span>";
                    objRegisteration.spanUserPhoto1 = "<span class='user-badge'>" + firstLetters + "</span>";
                }



                string monthTime = dt.ToString("MMMM");
                //string monthTime = "September";

                if (monthTime == "January")
                {
                    monthTime = "Jan";
                }
                if (monthTime == "February")
                {
                    monthTime = "Feb";
                }
                if (monthTime == "September")
                {
                    monthTime = "Sept";
                }
                if (monthTime == "October")
                {
                    monthTime = "Oct";
                }
                if (monthTime == "November")
                {
                    monthTime = "Nov";
                }
                if (monthTime == "December")
                {
                    monthTime = "Dec";
                }

                string dayTime = dt.ToString("ddddd");

                if (dayTime == "Monday")
                {
                    dayTime = "Mon";
                }

                if (dayTime == "Tuesday")
                {
                    dayTime = "Tues";
                }

                if (dayTime == "Wednesday")
                {
                    dayTime = "Weds";
                }

                if (dayTime == "Thursday")
                {
                    dayTime = "Thurs";
                }

                if (dayTime == "Friday")
                {
                    dayTime = "Fri";
                }

                if (dayTime == "Saturday")
                {
                    dayTime = "Sat";
                }

                if (dayTime == "Sunday")
                {
                    dayTime = "Sun";
                }


                objRegisteration.lblLastLogin = "<span data-localize='al_" + dt.ToString("ddddd") + "'>" + dayTime + "</span>&nbsp;" + dt.ToString("dd") + "&nbsp;<span data-localize='ac_" + dt.ToString("MMMM") + "'>" + monthTime + "</span>&nbsp;" + dt.ToString("HH:mm");
                //lbltimeZone.InnerHtml = "UTC " + (objRegisteration.vTimeZoneOffset == "" ? "Please update your timezone" : "" + objRegisteration.vTimeZoneOffset + "");
                objRegisteration.lbltimeZone = "UTC " + (UserTimeZoneOffset == "" ? "Please update your timezone" : "" + UserTimeZoneOffset + "");

                objRegisteration.hndTimeZoneID = objRegisteration.vTimeZoneID;

                objRegisteration.hndComanyCount = objRegisteration.hndDefaultClient;

                List<El_Mappings> lstCompany = new List<El_Mappings>();

                clsCompany objcls = new clsCompany();

                if (objRegisteration.ifkUserTypeID == 3)
                {
                    objcls.Operation = 2;
                }
                else
                {
                    objcls.Operation = 5;
                }

                objcls.ipkCompanyID = objRegisteration.ifkCompanyUniqueID;

                //Map Data Cache
                if (Cache["map_" + objcls.Operation + "_" + objcls.ipkCompanyID] == null)
                {
                    Cache.Add("map_" + objcls.Operation + "_" + objcls.ipkCompanyID, objcls.GetMapDataInfo(), cacheItemPolicy);
                }

                lstCompany = (List<El_Mappings>)Cache["map_" + objcls.Operation + "_" + objcls.ipkCompanyID];

                var MapDataInfo = JsonConvert.SerializeObject(lstCompany);
                objRegisteration.MapInfoData = MapDataInfo;
                objRegisteration.ihndResellerID = objRegisteration.CompanyUniqueID;

                objRegisteration.hndlat = _wlt_SessionPOCO.latlong.vlat;
                objRegisteration.hndlong = _wlt_SessionPOCO.latlong.vlong;
                objRegisteration.hndzoomlevel = Convert.ToString(_wlt_SessionPOCO.latlong.izoomleval);

                //objRegisteration.SkobblerApikey = "";
                
                objRegisteration.MapBoxStreetsApikey = _wlt_SessionPOCO.MapBoxStreetsApikey;
                objRegisteration.MapBoxTerrainApikey = _wlt_SessionPOCO.MapBoxTerrainApikey;
                objRegisteration.MapBoxSatelliteApikey = _wlt_SessionPOCO.MapBoxSatelliteApikey;


            }
            catch(Exception ex)
            {
                LogError.RegisterErrorInLogFile("AppInitialize", "InitiliazePage()", ex.Message + " : " + ex.StackTrace);
            }
        }
    }
}
