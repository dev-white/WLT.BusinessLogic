using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Text;
using WLT.DataAccessLayer;
using WLT.EntityLayer;
using WLT.EntityLayer.GPSOL;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_AdminHelper
    {

      
        private readonly wlt_Config _wlt_AppConfig;

        private readonly string Connectionstring;


        public Bal_AdminHelper()
        {
           

            _wlt_AppConfig = AppConfiguration.GetAppSettings<wlt_Config>("wlt_config");

            Connectionstring = AppConfiguration.GetAppSettings<wlt_Config>("ConnectionStrings").wlt_WebAppConnectionString;
        }     

        public DataSet GetTreedata(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = new SqlParameter[3];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@iParentID", SqlDbType.Int);
                param[1].Value = _clsRegistration.ResellerID;

                param[2] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[2].Value = _clsRegistration.ClientID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_SubTree", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("registration.cs", "GetTreedata()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        public DataSet GetAdminTreedata(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = new SqlParameter[3];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@iParentID", SqlDbType.Int);
                param[1].Value = _clsRegistration.iParent;

                param[2] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[2].Value = _clsRegistration.CompanyUniqueID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_AdminSubTree", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("registration.cs", "GetTreedata()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }


        public string SaveGroup(clsRegistration _clsRegistration)
        {
            ATPL_CRYPTO.SymmCrypto objEncryDescr = new ATPL_CRYPTO.SymmCrypto();

            string returnstring = "";
            SqlParameter[] param = new SqlParameter[4];      
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

           
                param[2] = new SqlParameter("@ifkGroupMID", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.ifkGroupMID;

                param[3] = new SqlParameter("@iParent", SqlDbType.VarChar);
                param[3].Value = _clsRegistration.iParent;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "save()", ex.Message  + ex.StackTrace);
                returnstring = "Error in saving User group " + ex.Message;
            }
            return returnstring;
        }

        public string Save(clsRegistration _clsRegistration)
        {
             ATPL_CRYPTO.SymmCrypto objEncryDescr = new ATPL_CRYPTO.SymmCrypto();

            string returnstring = "";
            SqlParameter[] param = new SqlParameter[28];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@vName", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.vName;

                param[3] = new SqlParameter("@vPassword", SqlDbType.VarChar);
                param[3].Value = objEncryDescr.Encrypting(_clsRegistration.vPassword);

                param[4] = new SqlParameter("@vEmail", SqlDbType.VarChar);
                param[4].Value = _clsRegistration.RemoveSpecialCharacters(_clsRegistration.vEmail);

                param[5] = new SqlParameter("@vPasswordQuestion", SqlDbType.VarChar);
                param[5].Value = _clsRegistration.vPasswordQuestion;

                param[6] = new SqlParameter("@vPasswordAnswer", SqlDbType.VarChar);
                param[6].Value = _clsRegistration.vPasswordAnswer;

                param[7] = new SqlParameter("@vOfficeTel", SqlDbType.VarChar);
                param[7].Value = _clsRegistration.vOfficeTel;

                param[8] = new SqlParameter("@vMobile", SqlDbType.VarChar);
                param[8].Value = _clsRegistration.vMobile;

                param[9] = new SqlParameter("@ifkLanguageID", SqlDbType.Int);
                param[9].Value = _clsRegistration.ifkLanguageID;

                param[10] = new SqlParameter("@vTimeZoneID", SqlDbType.VarChar);
                param[10].Value = _clsRegistration.vTimeZoneID;

                param[11] = new SqlParameter("@ifkRoleID", SqlDbType.Int);
                param[11].Value = _clsRegistration.ifkRoleID;

                param[12] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[12].Value = _clsRegistration.ifkCompanyUniqueID; //CompanyUniqueID;

                param[13] = new SqlParameter("@iParent", SqlDbType.Int);
                param[13].Value = _clsRegistration.iParent;

                param[14] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[14].Value = _clsRegistration.bStatus;

                param[15] = new SqlParameter("@Error", SqlDbType.Int);
                param[15].Direction = ParameterDirection.Output;

                param[16] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[16].Value = _clsRegistration.iCreatedBy;

                param[17] = new SqlParameter("@ifkUserTypeID", SqlDbType.Int);
                param[17].Value = _clsRegistration.ifkUserTypeID;

                param[18] = new SqlParameter("@ifkDefaultClient", SqlDbType.Int);
                param[18].Value = _clsRegistration.ifkDefaultClient;

                param[19] = new SqlParameter("@IsShowAllAsset", SqlDbType.Bit);
                param[19].Value = _clsRegistration.IsShowAllAsset;

                param[20] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[20].Value = _clsRegistration.ifkUserID;

                param[21] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[21].Value = _clsRegistration.ifkGroupMID;

                param[22] = new SqlParameter("@vTimeZoneName", SqlDbType.VarChar);
                param[22].Value = _clsRegistration.vTimeZoneName;

                param[23] = new SqlParameter("@vTimeOffset", SqlDbType.NVarChar);
                param[23].Value = _clsRegistration.vTimeZoneOffset;


                param[24] = new SqlParameter("@ifkMeasurementUnit", SqlDbType.Int);
                param[24].Value = _clsRegistration.ifkMeasurementUnit;

                param[25] = new SqlParameter("@quickAlertsEnabled", SqlDbType.Bit);
                param[25].Value = _clsRegistration.quickAlertsEnabled == 1 ? true : false;

                param[26] = new SqlParameter("@passwordChangeEnabled", SqlDbType.Bit);
                param[26].Value = _clsRegistration.passwordChangeEnabled == 1 ? true : false;

                param[27] = new SqlParameter("@ifkFuelMeasurementUnit", SqlDbType.Int);
                param[27].Value = _clsRegistration.ifkFuelMeasurementUnit;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);

                if (param[15].Value.ToString() == "-1")
                {
                    returnstring = "-1";
                }
                else if (param[15].Value.ToString() != "-1" && param[15].Value.ToString() != "-2")
                {
                    returnstring = param[15].Value.ToString();
                }
                else if (param[15].Value.ToString() == "-2")
                {
                    returnstring = "-2";
                }
                else if (param[15].Value.ToString() == "6" || param[11].Value.ToString() == "8")
                {
                    returnstring = "Delete successful";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "save()", ex.Message  + ex.StackTrace);
                returnstring = "Error in saving User " + ex.Message;
            }
            return returnstring;
        }

        public string SaveAddPersonNotify(clsRegistration _clsRegistration)
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[8];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@CanUserLogin", SqlDbType.Bit);
                param[1].Value = _clsRegistration.CanUserLogin;

                param[2] = new SqlParameter("@vName", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.vName;

                param[3] = new SqlParameter("@vEmail", SqlDbType.VarChar);
                param[3].Value = _clsRegistration.RemoveSpecialCharacters(_clsRegistration.vEmail);

                param[4] = new SqlParameter("@vMobile", SqlDbType.VarChar);
                param[4].Value = _clsRegistration.vMobile;

                param[5] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[5].Value = _clsRegistration.ifkCompanyUniqueID;

                param[6] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[6].Value = _clsRegistration.iCreatedBy;

                param[7] = new SqlParameter("@Error", SqlDbType.Int);
                param[7].Direction = ParameterDirection.Output;



                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);

                if (param[7].Value.ToString() == "1")
                {
                    returnstring = "1";
                }

                else if (param[7].Value.ToString() == "-1")
                {
                    returnstring = "-1";
                }
                else
                {
                    returnstring = param[7].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "save()", ex.Message  + ex.StackTrace);
                returnstring = "Error in saving User " + ex.Message;
            }
            return returnstring;
        }

        public string SaveAssetGroupID(clsRegistration _clsRegistration)
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[8];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@iParent", SqlDbType.Int);
                param[2].Value = _clsRegistration.iParent;

                param[3] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[3].Value = _clsRegistration.bStatus;

                param[4] = new SqlParameter("@Error", SqlDbType.Int);
                param[4].Direction = ParameterDirection.Output;

                param[5] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[5].Value = _clsRegistration.iCreatedBy;

                param[6] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[6].Value = _clsRegistration.ifkUserID;

                param[7] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[7].Value = _clsRegistration.ifkGroupMID;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);


                if (param[4].Value.ToString() != "-1" && param[4].Value.ToString() != "-2")
                {
                    returnstr = "Save successful !";
                }
                else if (param[4].Value.ToString() == "-2")
                {
                    returnstr = "-2";
                }


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "save()", ex.Message  + ex.StackTrace);
                returnstr = "Error in saving User " + ex.Message;
            }
            return returnstr;

        }

        public DataSet GetResellerUserList(clsRegistration _clsRegistration)
        {

            SqlParameter[] param = new SqlParameter[5];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = _clsRegistration.iParent;

                param[2] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[2].Value = _clsRegistration.pkUserID;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = _clsRegistration.ifkUserID;

                param[4] = new SqlParameter("@ifkGroupMID", SqlDbType.Int);
                param[4].Value = _clsRegistration.ifkGroupMID;


                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "GetResellerUserList()", ex.Message  + ex.StackTrace);
            }
            return ds;


        }

        public List<clsRegistration> SelectSupperAdmin(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();

            List<clsRegistration> lstSupperAdmin = new List<clsRegistration>();

            //  ATPL_CRYPTO.SymmCrypto objEncryDescr = new ATPL_CRYPTO.SymmCrypto();

            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);


                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstSupperAdmin.Add(new clsRegistration
                        {

                            pkUserID = Convert.ToInt32(row["pkUserID"].ToString()),
                            vName = row["vName"].ToString(),
                            vEmail = row["vEmail"].ToString(),
                            vPassword = AppConfiguration.UNCHANGED_PASSWORD,
                            vMobile = row["vMobile"].ToString(),
                            ifkLanguageID = Convert.ToInt32(row["ifkLanguageID"].ToString()),
                            vTimeZoneID = row["vTimeZoneID"].ToString(),
                            ifkDefaultClient = Convert.ToInt32(row["ifkDefaultClient"].ToString()),
                            IsShowAllAsset = true,
                            vLanguageName = row["vLanguageName"].ToString(),
                            vCompanyName = row["vCompanyName"].ToString(),
                            bStatus = Convert.ToBoolean(row["bStatus"].ToString()),
                            vTimeZoneName = row["vTimeZoneName"].ToString(),
                            ifkMeasurementUnit = Convert.ToInt32(row["ifkMeasurementUnit"].ToString()),
                            MeasurementUnit = row["MeasurementUnit"].ToString(),
                            quickAlertsEnabled = Convert.ToString(row["quickAlertsEnabled"]) == "" ? 0 : Convert.ToString(row["quickAlertsEnabled"]) == "False" ? 0 : 1,
                            passwordChangeEnabled = Convert.ToString(row["passwordChangeEnabled"]) == "" ? 0 : Convert.ToString(row["passwordChangeEnabled"]) == "False" ? 0 : 1,
                            PhotoName = Convert.ToString(row["PhotoName"]) == "" ? "/Images/man.svg" : Convert.ToString(row["PhotoName"]),
                            ifkFuelMeasurementUnit = Convert.ToInt32(row["ifkFuelMeasurementUnit"].ToString()),
                            FuelMeasurementUnit = row["FuelMeasurementUnit"].ToString(),


                            is_primary_owner = row["is_primary_owner"].ToString() == "" ? false : Convert.ToBoolean(row["is_primary_owner"]),
                            is_billing = row["is_billing"].ToString() == "" ? false : Convert.ToBoolean(row["is_billing"]),
                            is_technician = row["is_technician"].ToString() == "" ? false : Convert.ToBoolean(row["is_technician"].ToString()),


                        });
                    }
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "SelectSupperAdmin()", ex.Message  + ex.StackTrace);
            }


            return lstSupperAdmin;
        }

        public List<clsRegistration> SelectAdminUserData(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();
            List<clsRegistration> lstAdminUserData = new List<clsRegistration>();

            //   ATPL_CRYPTO.SymmCrypto objEncryDescr = new ATPL_CRYPTO.SymmCrypto();

            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstAdminUserData.Add(new clsRegistration
                        {
                            pkUserID = Convert.ToInt32(row["pkUserID"].ToString()),
                            vName = row["vName"].ToString(),
                            vEmail = row["vEmail"].ToString(),
                            vPassword = AppConfiguration.UNCHANGED_PASSWORD,
                            vMobile = row["vMobile"].ToString(),
                            ifkLanguageID = Convert.ToInt32(row["ifkLanguageID"].ToString()),
                            vTimeZoneID = row["vTimeZoneID"].ToString(),
                            vLanguageName = row["vLanguageName"].ToString(),
                            vTimeZoneName = row["vTimeZoneName"].ToString(),
                            bStatus = Convert.ToBoolean(row["bStatus"].ToString()),
                            ifkMeasurementUnit = Convert.ToInt32(ds.Tables[0].Rows[0]["ifkMeasurementUnit"].ToString()),
                            MeasurementUnit = row["MeasurementUnit"].ToString(),
                            quickAlertsEnabled = Convert.ToString(row["quickAlertsEnabled"]) == "" ? 0 : Convert.ToString(row["quickAlertsEnabled"]) == "False" ? 0 : 1,
                            passwordChangeEnabled = Convert.ToString(row["passwordChangeEnabled"]) == "" ? 0 : Convert.ToString(row["passwordChangeEnabled"]) == "False" ? 0 : 1,
                            PhotoName = Convert.ToString(row["PhotoName"]) == "" ? "/Images/man.svg" : Convert.ToString(row["PhotoName"]),
                            ifkFuelMeasurementUnit = Convert.ToInt32(ds.Tables[0].Rows[0]["ifkFuelMeasurementUnit"].ToString()),
                            FuelMeasurementUnit = row["FuelMeasurementUnit"].ToString()

                        }); ;
                    }
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "SelectAdminUserData()", ex.Message  + ex.StackTrace);
            }

            return lstAdminUserData;
        }

        public DataSet NewFrontData(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[2].Value = _clsRegistration.CompanyUniqueID;

                param[3] = new SqlParameter("@iParent", SqlDbType.Int);
                param[3].Value = _clsRegistration.ResellerID;

                param[4] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[4].Value = _clsRegistration.iTrackerType;

                param[5] = new SqlParameter("@IMEI", SqlDbType.VarChar);
                param[5].Value = _clsRegistration.SelectedDevice;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "NewFrontData_sp", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("registration.cs", "NewFrontData_sp()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public DataSet NewFrontData2(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[2].Value = _clsRegistration.CompanyUniqueID;

                param[3] = new SqlParameter("@iParent", SqlDbType.Int);
                param[3].Value = _clsRegistration.ResellerID;

                param[4] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[4].Value = _clsRegistration.iTrackerType;

                param[5] = new SqlParameter("@DeviceIDS", SqlDbType.VarChar);
                param[5].Value = _clsRegistration.SelectedDevice;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "NewFrontData_sp", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("registration.cs", "NewFrontData_sp()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetConnectedSensorEvents(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@DeviceIDS", SqlDbType.VarChar);
                param[1].Value = _clsRegistration.SelectedDevice;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "NewFrontData_sp", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("registration.cs", "GetConnectedSensorEvents()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public string SaveCurrentLoginDate(clsRegistration _clsRegistration)
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@CurrentLoginDate", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.dCurrentLoginDate;

                param[3] = new SqlParameter("@vEmail", SqlDbType.VarChar);
                param[3].Value = _clsRegistration.vEmail;

                param[4] = new SqlParameter("@vBrowser", SqlDbType.VarChar);
                param[4].Value = _clsRegistration.vBrowser;

                param[5] = new SqlParameter("@vIPAddress", SqlDbType.VarChar);
                param[5].Value = _clsRegistration.IPAddress;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_AdminHelper.cs", "SaveCurrentLoginDate()", ex.Message  + ex.StackTrace);
                returnstr = "Error in saving UserCurrentDate " + ex.Message;
            }
            return returnstr;

        }

        public DataSet SelectLastLoginDate(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_AdminHelper.cs", "SelectLastLoginDate()", ex.Message  + ex.StackTrace);
            }
            return ds;

        }

        public string SaveLastLoginDate(clsRegistration _clsRegistration)
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[3];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@LastLoginDate", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.dLastLoginDate;

                returnstr = SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param).ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_AdminHelper.cs", "SaveLastLoginDate()", ex.Message  + ex.StackTrace);
            }
            return returnstr;

        }
      

        public void TimeZoneDiffrence(clsRegistration _clsRegistration)
        {

            var UserTimeZone = TimeZoneInfo.FindSystemTimeZoneById(_clsRegistration.vTimeZoneID);

            var now = DateTimeOffset.UtcNow;
            TimeSpan UserOffset = UserTimeZone.GetUtcOffset(now);

            ///New code
            TimeSpan UserOffset1 = UserTimeZone.BaseUtcOffset;
            ///
            if (UserOffset.Hours > 0)
            {
                _clsRegistration.vTimeZoneOffset = "+" + UserOffset.ToString();
                _clsRegistration.vTimeZoneOffset = _clsRegistration.vTimeZoneOffset.Substring(0, _clsRegistration.vTimeZoneOffset.Length - 3);
            }
            else
            {
                _clsRegistration.vTimeZoneOffset = UserOffset.ToString();
                _clsRegistration.vTimeZoneOffset = _clsRegistration.vTimeZoneOffset.Substring(0, _clsRegistration.vTimeZoneOffset.Length - 3);
            }
        }

        public string DeleteInputOutputDevices(clsRegistration _clsRegistration)
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[5];
            try
            {
                param[0] = new SqlParameter("@id", SqlDbType.Int);
                param[0].Value = _clsRegistration.intId;

                param[1] = new SqlParameter("@type", SqlDbType.Char, 1);
                param[1].Value = _clsRegistration.strType.Substring(0, 1);

                param[2] = new SqlParameter("@operation", SqlDbType.Int);
                param[2].Value = _clsRegistration.Operation;

                param[3] = new SqlParameter("@ImeiNumber", SqlDbType.BigInt);
                param[3].Direction = ParameterDirection.Output;

                param[4] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[4].Value = _clsRegistration.ipkDeviceID;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "sp_DeleteInputOutputDevices", param);

                if (param[3].Value.ToString() == "1")
                {
                    returnstr = "1";
                }

                else if (param[3].Value.ToString() == "-1")
                {
                    returnstr = "-1";
                }

                else
                {
                    returnstr = param[3].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_AdminHelper.cs", "DeleteInputOutputDevices()", ex.Message  + ex.StackTrace);
            }
            return returnstr;

        }

        public string SaveInputOutputDevices(clsRegistration _clsRegistration)
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[9];
            try
            {
                param[0] = new SqlParameter("@type", SqlDbType.Char, 1);
                param[0].Value = _clsRegistration.strType;

                param[1] = new SqlParameter("@ifk_CompanyId", SqlDbType.BigInt);
                param[1].Value = _clsRegistration.intCompanyId;

                param[2] = new SqlParameter("@vName", SqlDbType.VarChar, 50);
                param[2].Value = _clsRegistration.strName;

                param[3] = new SqlParameter("@vUnitText", SqlDbType.VarChar, 50);
                param[3].Value = _clsRegistration.strUnit;

                //param[4] = new SqlParameter("@iAveraging", SqlDbType.SmallInt);
                //param[4].Value = intAveraging;

                param[4] = new SqlParameter("@bIsTemperature", SqlDbType.Bit);
                param[4].Value = _clsRegistration.intTemperature;

                param[5] = new SqlParameter("@bIsInput", SqlDbType.Bit);
                param[5].Value = _clsRegistration.intInput;

                param[6] = new SqlParameter("@vOnText", SqlDbType.VarChar, 50);
                param[6].Value = _clsRegistration.strOnText;

                param[7] = new SqlParameter("@vOffText", SqlDbType.VarChar, 50);
                param[7].Value = _clsRegistration.strOffText;

                param[8] = new SqlParameter("@strAnalogType", SqlDbType.Int);
                param[8].Value = _clsRegistration.strAnalogType;

                returnstr = SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "sp_AddInputOutputDevices", param).ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "SaveInputOutputDevices()", ex.Message  + ex.StackTrace);
            }
            return returnstr;

        }

        public string UpdateInputOutputDevices(clsRegistration _clsRegistration)
        {
            string returnstr = "";
            SqlParameter[] param = new SqlParameter[11];
            try
            {
                param[0] = new SqlParameter("@type", SqlDbType.Char, 1);
                param[0].Value = _clsRegistration.strType;

                param[1] = new SqlParameter("@ifk_CompanyId", SqlDbType.BigInt);
                param[1].Value = _clsRegistration.intCompanyId;

                param[2] = new SqlParameter("@vName", SqlDbType.VarChar, 50);
                param[2].Value = _clsRegistration.strName;

                param[3] = new SqlParameter("@vUnitText", SqlDbType.VarChar, 50);
                param[3].Value = _clsRegistration.strUnit;

                param[4] = new SqlParameter("@iAveraging", SqlDbType.SmallInt);
                param[4].Value = _clsRegistration.intAveraging;

                param[5] = new SqlParameter("@bIsTemperature", SqlDbType.Bit);
                param[5].Value = _clsRegistration.intTemperature;

                param[6] = new SqlParameter("@bIsInput", SqlDbType.Bit);
                param[6].Value = _clsRegistration.intInput;

                param[7] = new SqlParameter("@vOnText", SqlDbType.VarChar, 50);
                param[7].Value = _clsRegistration.strOnText;

                param[8] = new SqlParameter("@vOffText", SqlDbType.VarChar, 50);
                param[8].Value = _clsRegistration.strOffText;

                param[9] = new SqlParameter("@id", SqlDbType.BigInt);
                param[9].Value = _clsRegistration.intId;

                param[10] = new SqlParameter("@strAnalogType", SqlDbType.Int);
                param[10].Value = _clsRegistration.strAnalogType;


                returnstr = SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "sp_UpdateInputOutputDevices", param).ToString();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "SaveInputOutputDevices()", ex.Message  + ex.StackTrace);
            }
            return returnstr;

        }

        public List<clsRegistration> GetInputOutputDeviceDetails(clsRegistration _clsRegistration)
        {
            DataSet ds = new DataSet();
            List<clsRegistration> lstDetails = new List<clsRegistration>();

            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@id", SqlDbType.Int);
                param[0].Value = _clsRegistration.intId;

                param[1] = new SqlParameter("@type", SqlDbType.Char, 1);
                param[1].Value = _clsRegistration.strType;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "sp_GetInputOutputDeviceDetails", param);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstDetails.Add(new clsRegistration(Convert.ToInt32(row["id"].ToString()),
                                                      Convert.ToInt32(row["ifk_CompanyId"].ToString()),
                                                      row["vName"].ToString(),
                                                      row["vUnitText"].ToString(),
                                                      Convert.ToInt32(row["bIsTemperature"].ToString()),
                                                      row["bIsInput"].ToString(),
                                                      row["vOnText"].ToString(),
                                                      row["vOffText"].ToString(),
                                                      row["type"].ToString(),
                                                      row["strAnalogType"].ToString() == "" ? 0 : Convert.ToInt32(row["strAnalogType"].ToString())
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "SelectSupperAdmin()", ex.Message  + ex.StackTrace);
            }

            return lstDetails;
        }

        public string SaveSupperAdminUserTimeZone(clsRegistration _clsRegistration)
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@vTimeZoneID", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.vTimeZoneID;

                param[3] = new SqlParameter("@vTimeZoneName", SqlDbType.VarChar);
                param[3].Value = _clsRegistration.vTimeZoneName;

                param[4] = new SqlParameter("@vTimeOffset", SqlDbType.NVarChar);
                param[4].Value = _clsRegistration.vTimeZoneOffset;

                param[5] = new SqlParameter("@Error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;





                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);


                if (param[5].Value.ToString() != "-1" && param[5].Value.ToString() != "-2")
                {
                    returnstring = param[5].Value.ToString();
                }
                else if (param[5].Value.ToString() == "-2")
                {
                    returnstring = "-2";
                }


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "save()", ex.Message  + ex.StackTrace);
                returnstring = "Error in saving User " + ex.Message;
            }
            return returnstring;
        }

        public string SaveSupperAdminUserLanguage(clsRegistration _clsRegistration)
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@ifkLanguageID", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.ifkLanguageID;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);


                if (param[3].Value.ToString() != "-1" && param[3].Value.ToString() != "-2")
                {
                    returnstring = param[3].Value.ToString();
                }
                else if (param[3].Value.ToString() == "-2")
                {
                    returnstring = "-2";
                }


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "SaveSupperAdminUserLanguage()", ex.Message  + ex.StackTrace);
                returnstring = "Error in saving User " + ex.Message;
            }
            return returnstring;
        }

        public string SaveUserSpeed(clsRegistration _clsRegistration)
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@ifkMeasurementUnit", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.ifkMeasurementUnit;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);


                if (param[3].Value.ToString() != "-1" && param[3].Value.ToString() != "-2")
                {
                    returnstring = param[3].Value.ToString();
                }
                else if (param[3].Value.ToString() == "-2")
                {
                    returnstring = "-2";
                }


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "SaveUserSpeed()", ex.Message  + ex.StackTrace);
                returnstring = "Error in saving User " + ex.Message;
            }
            return returnstring;
        }

        public string SaveUserFuelUnits(clsRegistration _clsRegistration)
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@ifkFuelMeasurementUnit", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.ifkFuelMeasurementUnit;

                param[3] = new SqlParameter("@Error", SqlDbType.Int);
                param[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);


                if (param[3].Value.ToString() != "-1" && param[3].Value.ToString() != "-2")
                {
                    returnstring = param[3].Value.ToString();
                }
                else if (param[3].Value.ToString() == "-2")
                {
                    returnstring = "-2";
                }


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "SaveUserSpeed()", ex.Message  + ex.StackTrace);
                returnstring = "Error in saving User " + ex.Message;
            }
            return returnstring;
        }       

       

        public string PasswordExist(clsRegistration _clsRegistration)
        {
            try
            {
                //  ATPL_CRYPTO.SymmCrypto objCrypto = new ATPL_CRYPTO.SymmCrypto();

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 223;

                param[1] = new SqlParameter("@Error", SqlDbType.Int);
                param[1].Direction = ParameterDirection.Output;

                param[2] = new SqlParameter("@vPassword", SqlDbType.NVarChar);
                param[2].Value = "";// objCrypto.Encrypting(vPassword);

                param[3] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[3].Value = _clsRegistration.pkUserID;

                SqlHelper.ExecuteNonQuery(Connectionstring.ToString(), CommandType.StoredProcedure, "Newsp_registration", param);

                var op = param[1].Value.ToString();

                //return Convert.ToInt32(param[1].Value);
                return op;

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "PasswordExist()", ex.Message  + ex.StackTrace);
                return "";
            }
        }

        public string SavePersonalDetails(clsRegistration _clsRegistration)
        {
            string result = string.Empty;

            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 224;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@vName", SqlDbType.NVarChar);
                param[3].Value = _clsRegistration.vName;

                param[4] = new SqlParameter("@vMobile", SqlDbType.NVarChar);
                param[4].Value = _clsRegistration.vMobile;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);

                if (Convert.ToInt32(param[2].Value) == 1)
                {
                    result = "Personal Details saved successfully!";
                }
                if (Convert.ToInt32(param[2].Value) == -1)
                {
                    result = "-1";
                }
                if (Convert.ToInt32(param[2].Value) == -2)
                {
                    result = "-2";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsChangePassword.cs", "SavePersonalDetails()", ex.Message  + ex.StackTrace);
                result = "Internal Execution Error :" + ex.Message;
            }

            return result;
        }

        public string PasswordChangeEnabled(clsRegistration _clsRegistration)
        {
            string result = string.Empty;

            try
            {
                //   ATPL_CRYPTO.SymmCrypto objCrypto = new ATPL_CRYPTO.SymmCrypto();

                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = 226;

                param[1] = new SqlParameter("@vPassword", SqlDbType.NVarChar);
                param[1].Value = "";// objCrypto.Encrypting(vNewPassword);

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@vEmail", SqlDbType.NVarChar);
                param[3].Value = _clsRegistration.vEmail;

                param[4] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[4].Value = _clsRegistration.pkUserID;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "Newsp_registration", param);

                if (Convert.ToInt32(param[2].Value) == 1)
                {
                    result = "1";
                }
                if (Convert.ToInt32(param[2].Value) == -1)
                {
                    result = "-1";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsChangePassword.cs", "PasswordChangeEnabled()", ex.Message  + ex.StackTrace);
                result = "Internal Execution Error :" + ex.Message;
            }

            return result;
        }

        public DataSet GetNonLoginContactList(clsRegistration _clsRegistration)
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = _clsRegistration.iParent;

                param[2] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[2].Value = _clsRegistration.ifkUserID;

                ds = SqlHelper.ExecuteDataset(Connectionstring, CommandType.StoredProcedure, "New_ManageContacts", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "GetNonLoginContactList()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }
        public string SaveNonLoginContacts(clsRegistration _clsRegistration)
        {
            SqlParameter[] param = new SqlParameter[11];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@vName", SqlDbType.VarChar);
                param[2].Value = _clsRegistration.vName;

                param[3] = new SqlParameter("@vEmail", SqlDbType.NVarChar);
                param[3].Value = _clsRegistration.vEmail;

                param[4] = new SqlParameter("@vMobile", SqlDbType.NVarChar);
                param[4].Value = _clsRegistration.vMobile;

                param[5] = new SqlParameter("@Error", SqlDbType.Int);
                param[5].Direction = ParameterDirection.Output;

                param[6] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[6].Value = _clsRegistration.iCreatedBy;

                param[7] = new SqlParameter("@iParent", SqlDbType.Int);
                param[7].Value = _clsRegistration.iParent;

                param[8] = new SqlParameter("@vTimeZoneID", SqlDbType.VarChar);
                param[8].Value = _clsRegistration.vTimeZoneID;

                param[9] = new SqlParameter("@vTimeZoneName", SqlDbType.VarChar);
                param[9].Value = _clsRegistration.vTimeZoneName;

                param[10] = new SqlParameter("@vTimeOffset", SqlDbType.NVarChar);
                param[10].Value = _clsRegistration.vTimeZoneOffset;


                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "New_ManageContacts", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "SaveNonLoginContacts()", ex.Message  + ex.StackTrace);
            }
            return param[5].Value.ToString();
        }

        public string DeleteNonLoginContacts(clsRegistration _clsRegistration)
        {
            SqlParameter[] param = new SqlParameter[3];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;
                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;
                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "New_ManageContacts", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsRegistration.cs", "DeleteNonLoginContacts()", ex.Message  + ex.StackTrace);
            }
            return param[2].Value.ToString();
        }

        public string SaveUserUploadedImage(clsRegistration _clsRegistration)
        {
            SqlParameter[] param = new SqlParameter[5];
            string returnstring = "";

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = _clsRegistration.Operation;

                param[1] = new SqlParameter("@pkUserID", SqlDbType.Int);
                param[1].Value = _clsRegistration.pkUserID;

                param[2] = new SqlParameter("@Error", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[3].Value = _clsRegistration.iCreatedBy;

                param[4] = new SqlParameter("@PhotoName", SqlDbType.VarChar);
                param[4].Value = _clsRegistration.PhotoName;

                SqlHelper.ExecuteNonQuery(Connectionstring, CommandType.StoredProcedure, "UserProfile", param);

                if (param[2].Value.ToString() == "1")
                {
                    returnstring = "Saved successful";
                }

                else
                {
                    returnstring = "Internal Execution Error!";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsClientDevice.cs", "SaveUserUploadedImage()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error:" + ex.Message;
            }
            return returnstring;
        }
    }

    //[Serializable()]
    //public class Reseller
    //{
    //    public string ResellerName { get; set; }

    //    public int ResellerID { get; set; }


    //}
}
