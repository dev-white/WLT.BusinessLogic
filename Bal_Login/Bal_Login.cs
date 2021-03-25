using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WLT.BusinessLogic.Admin_Classes;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer;
using WLT.DataAccessLayer.DAL_Login;
using WLT.EntityLayer;
using WLT.EntityLayer.GPSOL;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.wlt_Login
{
    public class Bal_Login
    {
        private readonly wlt_Config _wlt_AppConfig;

        private readonly string Connectionstring;
        public Bal_Login()
        {
            

            _wlt_AppConfig = AppConfiguration.GetAppSettings<wlt_Config>("wlt_config");

            Connectionstring = AppConfiguration.GetAppSettings<wlt_Config>("ConnectionStrings").wlt_WebAppConnectionString;
        }

        public wlt_SessionPOCO DoLogin(clsRegistration objRegisteration, string lang, string absoluteUrl)
        {
            //create object for dataset and class
            DataSet ds = new DataSet();

            var _DAL_Login = new DAL_Login();

            var wlt_Session = new wlt_SessionPOCO();

            //TODO do something here 
            bool isLoginLogEnabled = _wlt_AppConfig.LoginLogStatus == "true" ? true : false;

            try
            {
                ds = _DAL_Login.DoLogin(objRegisteration);

                // putting ds to poco;
                wlt_Session.ds = ds;

                int x = ds.Tables.Count;

                string strMsg = "";

                //Set Session for isMiniReseller
                wlt_Session.IsMiniResellerSession = false;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["isMiniReseller"]))
                    {
                        wlt_Session.IsMiniResellerSession = true;
                    }

                }

                //Change Password on First Login. If on Trial and it is expired don't prompt change
                if (ds.Tables.Count > 6)
                {
                    if (ds.Tables[4].Rows[0]["passwordChangeEnabled"] != DBNull.Value && Convert.ToBoolean(ds.Tables[4].Rows[0]["passwordChangeEnabled"]) && Convert.ToBoolean(ds.Tables[7].Rows[0]["IsAllowedToLogin"]))
                    {
                        wlt_Session.RedirectUrl = ChangePassword(ds, absoluteUrl);

                        wlt_Session.PasswordChangeEnabled = true;

                        return wlt_Session;

                    }
                }

                if (!string.IsNullOrEmpty(lang))
                {
                    wlt_Session.UserLanguage = lang;
                }
                else
                {
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[5].Rows)
                        {
                            wlt_Session.UserLanguage = Convert.ToString(dr["VLanguageCode"]);
                        }
                    }
                }

                if (ds.Tables.Count > 6)
                {
                    var googleMapVersion = _wlt_AppConfig.GoogleMapVersion;

                    wlt_Session.GoogleMapURL = "";
                    wlt_Session.MapBoxStreetsApikey = "";
                    wlt_Session.MapBoxTerrainApikey = "";
                    wlt_Session.MapBoxSatelliteApikey = "";

                    //Store Map APIs in Session
                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[6].Rows)
                        {
                            if (Convert.ToString(dr["vMapDisplayName"]) == "Google Hybrid")
                            {
                                if (Convert.ToString(dr["vApiKey"]) != "")
                                {

                                    wlt_Session.GoogleMapURL = "https://maps.googleapis.com/maps/api/js?v=" + googleMapVersion + "&key=" + Convert.ToString(dr["vApiKey"]);

                                    wlt_Session.ResellerGMapKey = Convert.ToString(dr["vApiKey"]);
                                }
                                else
                                {

                                    wlt_Session.GoogleMapURL = "https://maps.google.com/maps/api/js?v=" + googleMapVersion;

                                    wlt_Session.ResellerGMapKey = "";
                                }
                            }
                            else if (Convert.ToString(dr["vMapDisplayName"]) == "Mapbox Streets")
                            {
                                wlt_Session.MapBoxStreetsApikey = Convert.ToString(dr["vApiKey"]);
                            }
                            else if (Convert.ToString(dr["vMapDisplayName"]) == "Mapbox Terrain")
                            {
                                wlt_Session.MapBoxTerrainApikey = Convert.ToString(dr["vApiKey"]);
                            }
                            else if (Convert.ToString(dr["vMapDisplayName"]) == "MapBox Satellite")
                            {
                                wlt_Session.MapBoxSatelliteApikey = Convert.ToString(dr["vApiKey"]);
                            }

                        }
                    }
                    else
                    {
                        wlt_Session.GoogleMapURL = "https://maps.google.com/maps/api/js?v=" + googleMapVersion;

                        wlt_Session.ResellerGMapKey = "";
                    }
                }



                if (ds.Tables.Count > 7)
                {

                    var IsAllowedToLogin = Convert.ToBoolean(ds.Tables[7].Rows[0]["IsAllowedToLogin"]);

                    if (!IsAllowedToLogin)
                        wlt_Session.LoginErrorMessage = @"There is a probem with this account";
                }                   
                

                }
            catch (Exception ex)
            {

                wlt_Session.LoginErrorMessage = @"Login failed. An error occurred when login.";

                LogError.RegisterErrorInLogFile("/Account/Login", "Login()", ex.Message  + ex.StackTrace);

                if (isLoginLogEnabled)//Log User Logged in
                {
                    // var IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    var IPAddress = "";
                    LogError.RegisterLoginLog(_wlt_AppConfig.LoginLog, objRegisteration.vEmail, IPAddress, "Failed Login. Error: " + ex.Message  + ex.StackTrace);
                }
            }

            return wlt_Session;
        }

        public clsRegistration GetUser(int pkUserID)
        {

            DataSet ds = new DataSet();
            clsRegistration obj = new clsRegistration();
            try
            {

                // var hostName = configuration.GetSection("Config:Production:RedisAddress:Hostname").Value;              

                string Query = "select * from tblUser_Master where ipkUserID=" + pkUserID;



                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.Text, Query);

                obj = new clsRegistration(Convert.ToInt32(ds.Tables[0].Rows[0]["pkUserID"].ToString()),
                                                       ds.Tables[0].Rows[0]["vName"].ToString(),
                                                       ds.Tables[0].Rows[0]["vEmail"].ToString(),
                                                       ds.Tables[0].Rows[0]["vPassword"].ToString(),
                                                       ds.Tables[0].Rows[0]["vPasswordQuestion"].ToString(),
                                                       ds.Tables[0].Rows[0]["vPasswordAnswer"].ToString(),
                                                       ds.Tables[0].Rows[0]["vOfficeTel"].ToString(),
                                                       ds.Tables[0].Rows[0]["vMobile"].ToString(),
                                                       Convert.ToInt32(ds.Tables[0].Rows[0]["ifkLanguageID"].ToString()),
                                                       ds.Tables[0].Rows[0]["vTimeZoneID"].ToString(),
                                                       Convert.ToInt32(ds.Tables[0].Rows[0]["ifkRoleID"].ToString()),
                                                       Convert.ToInt32(ds.Tables[0].Rows[0]["ifkCompanyID"].ToString()),
                                                       Convert.ToInt32(ds.Tables[0].Rows[0]["iParent"].ToString()),
                                                       Convert.ToBoolean(ds.Tables[0].Rows[0]["bStatus"].ToString()),
                                                       Convert.ToDateTime(ds.Tables[0].Rows[0]["dEntryDate"].ToString()),
                                                       "",
                                                       Convert.ToInt32(ds.Tables[0].Rows[0]["ifkUserTypeID"].ToString()),
                                                       Convert.ToInt32(ds.Tables[0].Rows[0]["ifkDefaultClient"].ToString()),
                                                       Convert.ToBoolean(ds.Tables[0].Rows[0]["IsShowAllAsset"].ToString()),
                                                       ds.Tables[0].Rows[0]["vTimeOffset"].ToString(),
                                                       ds.Tables[0].Rows[0]["vThemeName"].ToString(),
                                                       Convert.ToInt32(ds.Tables[0].Rows[0]["ifkCompanyID"].ToString()),
                                                       Convert.ToInt32(ds.Tables[0].Rows[0]["ifkMeasurementUnit"].ToString()),
                                                       "0",
                                                       false,
                                                       false,
                                                       false,
                                                       Convert.ToString(ds.Tables[0].Rows[0]["PhotoName"]) == "" ? "/Images/man.svg" : Convert.ToString(ds.Tables[0].Rows[0]["PhotoName"]),
                                                       Convert.ToInt32(ds.Tables[0].Rows[0]["ifkFuelMeasurementUnit"].ToString())
                                                       );
            }
            catch (Exception ex)
            {




                LogError.RegisterErrorInLogFile("registration.cs", "GetUser()", ex.Message  + ex.StackTrace);
            }
            return obj;
        }

        public string GetHelpUrl(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();
            string helpUrl = "";

            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 22;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        helpUrl = row["vHelpUrl"].ToString();

                    }
                }

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "GetHelpUrl()", ex.Message  + ex.StackTrace);
                helpUrl = "Error in getting Help Url " + ex.Message;
            }
            return helpUrl;
        }

        public DataSet SelectLoginDateForSpan(int dayCount, int CompanyID, int loginType, clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[7];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@MinusDays", SqlDbType.Int);
                param[2].Value = dayCount;

                param[3] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[3].Value = CompanyID;

                param[4] = new SqlParameter("@vTimeOffset", SqlDbType.VarChar);
                param[4].Value = _clsRegistration.vTimeOffset;

                param[5] = new SqlParameter("@UserDateTime", SqlDbType.DateTime);
                param[5].Value = _clsRegistration.UserDateTime;

                param[6] = new SqlParameter("@LoginType", SqlDbType.Int);
                param[6].Value = loginType;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Login.cs", "SelectLoginDateForSpan()", ex.Message  + ex.StackTrace);
            }
            return ds;

        }

        public string GetCustomLinks(clsRegistration _clsRegistration)
        {
            string _str = "";
            DataSet ds = new DataSet();
            //string helpUrl = "";

            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 23;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_Company", param);
                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (Convert.ToString(row["CustomResellerLink1"]) != "")
                        {
                            _str += "<p><a  href='" + Convert.ToString(row["CustomResellerLink1"]) + "' target='_blank' rel='noopener' title='" + Convert.ToString(row["CustomLink1ToolTip"]) + "'><i class='' style='font-size: 16px; padding-right: 30px; top:2px; position:relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M295.5 732.5Q305 742 318 742q7 0 12.5-2.5T341 732l388-390q9-10 9-23t-9.5-22.5T706 287t-23 10L296 687q-10 10-10 23t9.5 22.5zM476 664q8 36-1 69t-36 60L310 918q-42 42-102 42t-102-42t-42-102t42-101l126-131q42-42 102-42q17 0 38 6l50-50q-42-20-88-20q-86 0-147 61L61 669Q0 730 0 816t61 147t147 61t147-61l129-125q46-46 57-107t-14-117zM963 61q-30-30-68-45.5T816 0q-86 1-147 61L540 186q-48 47-58.5 113.5T500 424l50-50q-12-36-2.5-75t37.5-68l129-125q10-10 22-18t25-13.5t27-8t28-2.5q60 0 102 42t42 102t-42 101L789 439q-20 20-46.5 31T687 481q-23 0-28-1l-51 51q13 5 26.5 8t26 4.5T687 545q41 0 79-15.5t68-45.5l129-129q68-68 60-165q-7-76-60-129z' fill='#c1bfd0'/></svg></i>" + Convert.ToString(row["CustomResellerLink1DisplayName"]) + "</a></p>";
                        }
                        if (Convert.ToString(row["CustomResellerLink2"]) != "")
                        {
                            _str += "<p><a  href='" + Convert.ToString(row["CustomResellerLink2"]) + "' target='_blank' rel='noopener' title='" + Convert.ToString(row["CustomLink2ToolTip"]) + "'><i class='' style='font-size: 16px; padding-right: 30px; top:2px; position:relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M295.5 732.5Q305 742 318 742q7 0 12.5-2.5T341 732l388-390q9-10 9-23t-9.5-22.5T706 287t-23 10L296 687q-10 10-10 23t9.5 22.5zM476 664q8 36-1 69t-36 60L310 918q-42 42-102 42t-102-42t-42-102t42-101l126-131q42-42 102-42q17 0 38 6l50-50q-42-20-88-20q-86 0-147 61L61 669Q0 730 0 816t61 147t147 61t147-61l129-125q46-46 57-107t-14-117zM963 61q-30-30-68-45.5T816 0q-86 1-147 61L540 186q-48 47-58.5 113.5T500 424l50-50q-12-36-2.5-75t37.5-68l129-125q10-10 22-18t25-13.5t27-8t28-2.5q60 0 102 42t42 102t-42 101L789 439q-20 20-46.5 31T687 481q-23 0-28-1l-51 51q13 5 26.5 8t26 4.5T687 545q41 0 79-15.5t68-45.5l129-129q68-68 60-165q-7-76-60-129z' fill='#c1bfd0'/></svg></i>" + Convert.ToString(row["CustomResellerLink2DisplayName"]) + "</a></p>";
                        }
                        if (Convert.ToString(row["CustomResellerLink3"]) != "")
                        {
                            _str += "<p><a  href='" + Convert.ToString(row["CustomResellerLink3"]) + "' target='_blank' rel='noopener' title='" + Convert.ToString(row["CustomLink3ToolTip"]) + "'><i class=''style='font-size: 16px; padding-right: 30px; top:2px; position:relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M295.5 732.5Q305 742 318 742q7 0 12.5-2.5T341 732l388-390q9-10 9-23t-9.5-22.5T706 287t-23 10L296 687q-10 10-10 23t9.5 22.5zM476 664q8 36-1 69t-36 60L310 918q-42 42-102 42t-102-42t-42-102t42-101l126-131q42-42 102-42q17 0 38 6l50-50q-42-20-88-20q-86 0-147 61L61 669Q0 730 0 816t61 147t147 61t147-61l129-125q46-46 57-107t-14-117zM963 61q-30-30-68-45.5T816 0q-86 1-147 61L540 186q-48 47-58.5 113.5T500 424l50-50q-12-36-2.5-75t37.5-68l129-125q10-10 22-18t25-13.5t27-8t28-2.5q60 0 102 42t42 102t-42 101L789 439q-20 20-46.5 31T687 481q-23 0-28-1l-51 51q13 5 26.5 8t26 4.5T687 545q41 0 79-15.5t68-45.5l129-129q68-68 60-165q-7-76-60-129z' fill='#c1bfd0'/></svg></i>" + Convert.ToString(row["CustomResellerLink3DisplayName"]) + "</a></p>";
                        }

                    }


                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        if (Convert.ToString(row["CustomClientLink1"]) != "")
                        {
                            _str += "<p><a  href='" + Convert.ToString(row["CustomClientLink1"]) + "' target='_blank' rel='noopener' title='" + Convert.ToString(row["CustomLink1ToolTip"]) + "'><i class='' style='font-size: 16px; padding-right: 30px; top:2px; position:relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M295.5 732.5Q305 742 318 742q7 0 12.5-2.5T341 732l388-390q9-10 9-23t-9.5-22.5T706 287t-23 10L296 687q-10 10-10 23t9.5 22.5zM476 664q8 36-1 69t-36 60L310 918q-42 42-102 42t-102-42t-42-102t42-101l126-131q42-42 102-42q17 0 38 6l50-50q-42-20-88-20q-86 0-147 61L61 669Q0 730 0 816t61 147t147 61t147-61l129-125q46-46 57-107t-14-117zM963 61q-30-30-68-45.5T816 0q-86 1-147 61L540 186q-48 47-58.5 113.5T500 424l50-50q-12-36-2.5-75t37.5-68l129-125q10-10 22-18t25-13.5t27-8t28-2.5q60 0 102 42t42 102t-42 101L789 439q-20 20-46.5 31T687 481q-23 0-28-1l-51 51q13 5 26.5 8t26 4.5T687 545q41 0 79-15.5t68-45.5l129-129q68-68 60-165q-7-76-60-129z' fill='#c1bfd0'/></svg></i>" + Convert.ToString(row["CustomClientLink1DisplayName"]) + "</a></p>";
                        }
                        if (Convert.ToString(row["CustomClientLink2"]) != "")
                        {
                            _str += "<p><a  href='" + Convert.ToString(row["CustomClientLink2"]) + "' target='_blank' rel='noopener' title='" + Convert.ToString(row["CustomLink2ToolTip"]) + "'><i class='' style='font-size: 16px; padding-right: 30px; top:2px; position:relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M295.5 732.5Q305 742 318 742q7 0 12.5-2.5T341 732l388-390q9-10 9-23t-9.5-22.5T706 287t-23 10L296 687q-10 10-10 23t9.5 22.5zM476 664q8 36-1 69t-36 60L310 918q-42 42-102 42t-102-42t-42-102t42-101l126-131q42-42 102-42q17 0 38 6l50-50q-42-20-88-20q-86 0-147 61L61 669Q0 730 0 816t61 147t147 61t147-61l129-125q46-46 57-107t-14-117zM963 61q-30-30-68-45.5T816 0q-86 1-147 61L540 186q-48 47-58.5 113.5T500 424l50-50q-12-36-2.5-75t37.5-68l129-125q10-10 22-18t25-13.5t27-8t28-2.5q60 0 102 42t42 102t-42 101L789 439q-20 20-46.5 31T687 481q-23 0-28-1l-51 51q13 5 26.5 8t26 4.5T687 545q41 0 79-15.5t68-45.5l129-129q68-68 60-165q-7-76-60-129z' fill='#c1bfd0'/></svg></i>" + Convert.ToString(row["CustomClientLink2DisplayName"]) + "</a></p>";
                        }
                        if (Convert.ToString(row["CustomClientLink3"]) != "")
                        {
                            _str += "<p><a  href='" + Convert.ToString(row["CustomClientLink3"]) + "' target='_blank' rel='noopener' title='" + Convert.ToString(row["CustomLink3ToolTip"]) + "'><i class=''style='font-size: 16px; padding-right: 30px; top:2px; position:relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M295.5 732.5Q305 742 318 742q7 0 12.5-2.5T341 732l388-390q9-10 9-23t-9.5-22.5T706 287t-23 10L296 687q-10 10-10 23t9.5 22.5zM476 664q8 36-1 69t-36 60L310 918q-42 42-102 42t-102-42t-42-102t42-101l126-131q42-42 102-42q17 0 38 6l50-50q-42-20-88-20q-86 0-147 61L61 669Q0 730 0 816t61 147t147 61t147-61l129-125q46-46 57-107t-14-117zM963 61q-30-30-68-45.5T816 0q-86 1-147 61L540 186q-48 47-58.5 113.5T500 424l50-50q-12-36-2.5-75t37.5-68l129-125q10-10 22-18t25-13.5t27-8t28-2.5q60 0 102 42t42 102t-42 101L789 439q-20 20-46.5 31T687 481q-23 0-28-1l-51 51q13 5 26.5 8t26 4.5T687 545q41 0 79-15.5t68-45.5l129-129q68-68 60-165q-7-76-60-129z' fill='#c1bfd0'/></svg></i>" + Convert.ToString(row["CustomClientLink3DisplayName"]) + "</a></p>";
                        }

                    }

                }
                else if (ds.Tables.Count == 1)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (Convert.ToString(row["CustomResellerLink1"]) != "")
                        {
                            _str += "<p><a  href='" + Convert.ToString(row["CustomResellerLink1"]) + "' target='_blank' rel='noopener' title='" + Convert.ToString(row["CustomLink1ToolTip"]) + "'><i class='' style='font-size: 16px; padding-right: 30px; top:2px; position:relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M295.5 732.5Q305 742 318 742q7 0 12.5-2.5T341 732l388-390q9-10 9-23t-9.5-22.5T706 287t-23 10L296 687q-10 10-10 23t9.5 22.5zM476 664q8 36-1 69t-36 60L310 918q-42 42-102 42t-102-42t-42-102t42-101l126-131q42-42 102-42q17 0 38 6l50-50q-42-20-88-20q-86 0-147 61L61 669Q0 730 0 816t61 147t147 61t147-61l129-125q46-46 57-107t-14-117zM963 61q-30-30-68-45.5T816 0q-86 1-147 61L540 186q-48 47-58.5 113.5T500 424l50-50q-12-36-2.5-75t37.5-68l129-125q10-10 22-18t25-13.5t27-8t28-2.5q60 0 102 42t42 102t-42 101L789 439q-20 20-46.5 31T687 481q-23 0-28-1l-51 51q13 5 26.5 8t26 4.5T687 545q41 0 79-15.5t68-45.5l129-129q68-68 60-165q-7-76-60-129z' fill='#c1bfd0'/></svg></i>" + Convert.ToString(row["CustomResellerLink1DisplayName"]) + "</a></p>";
                        }
                        if (Convert.ToString(row["CustomResellerLink2"]) != "")
                        {
                            _str += "<p><a  href='" + Convert.ToString(row["CustomResellerLink2"]) + "' target='_blank' rel='noopener' title='" + Convert.ToString(row["CustomLink2ToolTip"]) + "'><i class=''  style='font-size: 16px; padding-right: 30px; top:2px; position:relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M295.5 732.5Q305 742 318 742q7 0 12.5-2.5T341 732l388-390q9-10 9-23t-9.5-22.5T706 287t-23 10L296 687q-10 10-10 23t9.5 22.5zM476 664q8 36-1 69t-36 60L310 918q-42 42-102 42t-102-42t-42-102t42-101l126-131q42-42 102-42q17 0 38 6l50-50q-42-20-88-20q-86 0-147 61L61 669Q0 730 0 816t61 147t147 61t147-61l129-125q46-46 57-107t-14-117zM963 61q-30-30-68-45.5T816 0q-86 1-147 61L540 186q-48 47-58.5 113.5T500 424l50-50q-12-36-2.5-75t37.5-68l129-125q10-10 22-18t25-13.5t27-8t28-2.5q60 0 102 42t42 102t-42 101L789 439q-20 20-46.5 31T687 481q-23 0-28-1l-51 51q13 5 26.5 8t26 4.5T687 545q41 0 79-15.5t68-45.5l129-129q68-68 60-165q-7-76-60-129z' fill='#c1bfd0'/></svg></i>" + Convert.ToString(row["CustomResellerLink2DisplayName"]) + "</a></p>";
                        }
                        if (Convert.ToString(row["CustomResellerLink3"]) != "")
                        {
                            _str += "<p><a  href='" + Convert.ToString(row["CustomResellerLink3"]) + "' target='_blank' rel='noopener' title='" + Convert.ToString(row["CustomLink3ToolTip"]) + "'><i class='' style='font-size: 16px; padding-right: 30px; top:2px; position:relative;'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' aria-hidden='true' focusable='false' width='1em' height='1em' style='-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);' preserveAspectRatio='xMidYMid meet' viewBox='0 0 1024 1024'><path d='M295.5 732.5Q305 742 318 742q7 0 12.5-2.5T341 732l388-390q9-10 9-23t-9.5-22.5T706 287t-23 10L296 687q-10 10-10 23t9.5 22.5zM476 664q8 36-1 69t-36 60L310 918q-42 42-102 42t-102-42t-42-102t42-101l126-131q42-42 102-42q17 0 38 6l50-50q-42-20-88-20q-86 0-147 61L61 669Q0 730 0 816t61 147t147 61t147-61l129-125q46-46 57-107t-14-117zM963 61q-30-30-68-45.5T816 0q-86 1-147 61L540 186q-48 47-58.5 113.5T500 424l50-50q-12-36-2.5-75t37.5-68l129-125q10-10 22-18t25-13.5t27-8t28-2.5q60 0 102 42t42 102t-42 101L789 439q-20 20-46.5 31T687 481q-23 0-28-1l-51 51q13 5 26.5 8t26 4.5T687 545q41 0 79-15.5t68-45.5l129-129q68-68 60-165q-7-76-60-129z' fill='#c1bfd0'/></svg></i>" + Convert.ToString(row["CustomResellerLink3DisplayName"]) + "</a></p>";
                        }

                    }
                }

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "GetHelpUrl()", ex.Message  + ex.StackTrace);
                _str = "Error in getting Help Url " + ex.Message;
            }
            return _str;
        }

        public Reseller GetClientResellerParent(int ClientID)
        {
            var ds = new DataSet();

            var ResellerModel = new Reseller();

            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("Operation", SqlDbType.Int);

                param[0].Value = 231;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);

                param[1].Value = ClientID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);

                if(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ResellerModel.ResellerID = Convert.ToInt32(ds.Tables[0].Rows[0]["ResellerId"]);
                    ResellerModel.ResellerName = Convert.ToString(ds.Tables[0].Rows[0]["vCompanyName"]);
                    ResellerModel.IsMiniReseller = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsMiniReseller"]);
                };

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "GetClientResellerParent()", ex.Message  + ex.StackTrace);
            }
            return ResellerModel;
        }

        public DataSet GetClientUrlsForReseller(int iParent)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 18;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "GetClientUrlsForReseller()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetClientUrl(int iParent)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 19;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "GetClientUrl()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        private string ChangePassword(DataSet ds, string url)
        {

            try
            {
                //start create random string
                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                StringBuilder res = new StringBuilder();
                Random rnd = new Random();
                int intCount = 5;
                while (0 < intCount)
                {
                    res.Append(valid[rnd.Next(valid.Length)]);
                    intCount--;
                }
                //end create random string

                string strUserId = "0";
                string bUseHttps = "";
                string strEmail = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strUserId = Convert.ToString(dr["pkUserID"]);
                    bUseHttps = Convert.ToString(dr["bUseHttps"]);
                    strEmail = Convert.ToString(dr["vEmail"]);
                }

                string RequestId = GenerateRequestID() + strUserId + res.ToString();

                clsChangePassword ocp = new clsChangePassword();
                ocp.RequestId = RequestId;
                ocp.Email = strEmail;

                url = url + "/Account/ChangePassword?requestid=" + RequestId;

                if (bUseHttps == "True")
                {
                    url = "https://" + url;
                }
                else
                {
                    url = "http://" + url;
                }

                int intRet = ocp.InsertSessionId();

                return url;

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Login.cs", "ChangePassword()", ex.Message  + ex.StackTrace);

            }
            return "";
        }

        public string GenerateRequestID()
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";

            char[] chars = new char[36];

            Random rd = new Random();

            for (int i = 0; i < 36; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
