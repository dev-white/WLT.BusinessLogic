using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WLT.BusinessLogic.BAL;
using WLT.DataAccessLayer.Classes;
using WLT.DataAccessLayer.GPSOL;
using WLT.EntityLayer;
using WLT.EntityLayer.GPSOL;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Admin_Classes
{
    public class BAL_MyAccount
    {
        private readonly wlt_Config _wlt_AppConfig;



        public BAL_MyAccount()
        {


            _wlt_AppConfig = AppConfiguration.GetAppSettings<wlt_Config>("wlt_config");

            //Connectionstring = _AppConfigurationService.GetAppSettings<wlt_Config>("ConnectionStrings").wlt_WebAppConnectionString;
        }

        public Dictionary<string, EL_CurrentTrackerData> GetAllTrackerPointDataInJSON(clsCurrentTrackPoint objclsTrackPoint, clsRegistration objclsRegistration)
        {
            Dictionary<string, EL_CurrentTrackerData> jsondic = new Dictionary<string, EL_CurrentTrackerData>();

            DataTable dsAllDevices = new DataTable();
            string vDatetime = "";
            string TimeZoneID = "";
            //string filePath = ConfigurationManager.AppSettings["AssetPhotoFolderPath"].ToString();
            //string resizeFilePath = ConfigurationManager.AppSettings["AssetPhotoResizeFolderPath"].ToString();
            //string uploadedLogo = "";
            string DriverName = "";
            //string DriverPhoto = "";
            int SpeedTypeId = 1;


            try
            {

                TimeZoneID = objclsRegistration.vTimeZoneID;
                SpeedTypeId = objclsRegistration.ifkMeasurementUnit;

                BAL_MyAccount _objBALMyAccount = new BAL_MyAccount();
                EL_MyAccount _objELMyAccount = new EL_MyAccount();
               

                _objELMyAccount.UserId = objclsRegistration.pkUserID;
                _objELMyAccount.UserTypeId = objclsRegistration.ifkUserTypeID;
                _objELMyAccount.ifk_ClientId = objclsRegistration.ifkCompanyUniqueID;


                if (objclsTrackPoint.operation == 1)
                {
                    dsAllDevices = _objBALMyAccount.GetTrackerPointData(_objELMyAccount, true);
                }
                else if (objclsTrackPoint.operation == 2)
                {
                    _objELMyAccount.op = 1;
                    _objELMyAccount.ifk_ClientId = objclsTrackPoint.ClientID;
                    dsAllDevices = _objBALMyAccount.GetTrackerPointData(_objELMyAccount, false);
                }
                else
                {
                    //Cache newCache = new Cache();                                               

                    //if (newCache["data"] == null)
                    //{
                    //    newCache.Add("data", _objBALMyAccount.GetTrackerPointData(_objELMyAccount, false), null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                    //}

                    //dsAllDevices = (DataSet)newCache["data"];

                    dsAllDevices = _objBALMyAccount.GetTrackerPointData(_objELMyAccount, false);

                }

                //if (dsAllDevices.Tables.Count > 0)
                //{
                //    _objMyAccount.ViewState["trackingData"] = dsAllDevices.Tables[0];
                //}


                jsondic = new Dictionary<string, EL_CurrentTrackerData>(dsAllDevices.Rows.Count);


                var _DAL_TrackerPoint = new DAL_TrackerPoint();

                if (dsAllDevices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsAllDevices.Rows)
                    {
                        objclsTrackPoint.vpkDeviceID = Convert.ToString(dr["vpkDeviceID"]);
                        objclsTrackPoint.Heading = Convert.ToInt32(dr["vHeading"]);
                        objclsTrackPoint.vVehicleSpeed = UserSettings.ConvertKMsToXx(SpeedTypeId, Convert.ToString(dr["vVehicleSpeed"]), false, 1);
                        objclsTrackPoint.vRoadSpeed = UserSettings.ConvertKMsToXx(SpeedTypeId, Convert.ToString(dr["vRoadSpeed"]), false, 1);
                        objclsTrackPoint.bIsIgnitionOn = Convert.ToString(dr["bIsIgnitionOn"]);
                        objclsTrackPoint.IsGSMDevice = false;
                        objclsTrackPoint.EventName = Convert.ToString(dr["vEventName"]);
                        objclsTrackPoint.iTrackerType = Convert.ToInt32(dr["iTrackerType"]);
                        objclsTrackPoint.isTripReplay = true;

                        string maparrow = _DAL_TrackerPoint.MapArrow(objclsTrackPoint);
                        string ignitionStatus = _DAL_TrackerPoint.IgnitionStatus(objclsTrackPoint);

                        string menuicon = _DAL_TrackerPoint.MenuIcon(objclsTrackPoint);
                        string menuiconpath = _DAL_TrackerPoint.Menuicon();

                        DriverName = "";

                        string _strDriverId = Convert.ToString(dr["ifkDriverID"]);

                        DriverName = Convert.ToString(dr["DriverName"]);

                        if ((TimeZoneID != "") && (TimeZoneID != null))
                        {
                            if (!DBNull.Value.Equals(dr["dGPSDateTime"]))
                            {
                                vDatetime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(dr["dGPSDateTime"]), "UTC", TimeZoneID)).ToString("MM/dd/yyyy HH:mm:ss");

                            }
                            else
                            {
                                vDatetime = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime("18/05/1978 12:00:00"), "UTC", TimeZoneID)).ToString("MM/dd/yyyy HH:mm:ss");

                            }

                            string ifk_CommonEventId_Opposite = "";
                            string cInput1 = "";
                            string cInput2 = "";
                            string cInput3 = "";
                            string cInput4 = "";
                            string cInput5 = "";
                            string cInput6 = "";


                            jsondic[Convert.ToString(dr["ipkDeviceID"])] = new EL_CurrentTrackerData
                            {
                                ipkDeviceID = Convert.ToString(dr["ipkDeviceID"]),
                                vpkDeviceID = Convert.ToString(dr["vpkDeviceID"]),
                                ifk_DeviceTypeId = Convert.ToString(dr["ifk_DeviceTypeId"]),
                                vLatitude = Convert.ToString(dr["vLatitude"]),
                                vLongitude = Convert.ToString(dr["vLongitude"]),
                                nAltitude = Convert.ToString(dr["nAltitude"]),
                                dGPSDateTime = vDatetime,
                                vOdometer = Convert.ToString(dr["vOdometer"]),
                                Heading = Convert.ToInt32(dr["vHeading"]),
                                vVehicleSpeed = UserSettings.ConvertKMsToXx(SpeedTypeId, Convert.ToString(dr["vVehicleSpeed"]), false, 0),
                                vRoadSpeed = UserSettings.ConvertKMsToXx(SpeedTypeId, Convert.ToString(dr["vRoadSpeed"]), false, 0),
                                EventName = Convert.ToString(dr["vEventName"]),
                                vDeviceName = Convert.ToString(dr["vDeviceName"]),
                                vTextMessage = Convert.ToString(dr["vTextMessage"]),
                                vImage = maparrow,
                                bIsIgnitionOn = ignitionStatus,
                                DriverId = Convert.ToString(dr["ifkDriverID"]),
                                AssetPhotoName = Convert.ToString(dr["AssetPhotoName"]),
                                iTrackerType = Convert.ToInt32(Convert.ToString(dr["iTrackerType"])),
                                Drivername = DriverName,
                                DriverPhoto = Convert.ToString(dr["DvLogoName"]),
                                vIcon = menuiconpath + menuicon,
                                vRadius = "",
                                vStartAngle = "",
                                vEndAngle = "",
                                iBatteryBackup = Convert.ToString(dr["iBatteryBackup"]),
                                IsGSMDevice = false,
                                bInverted = "",
                                vName = "",
                                vOnText = "",
                                vOffText = "",
                                ifk_CommonEventId = Convert.ToString(dr["vReportID"]),
                                ifk_CommonEventId_Opposite = ifk_CommonEventId_Opposite,
                                cInput1 = cInput1,
                                cInput2 = cInput2,
                                cInput3 = cInput3,
                                cInput4 = cInput4,
                                cInput5 = cInput5,
                                cInput6 = cInput6,
                                cUnit = UserSettings.GetOdometerUnitName(SpeedTypeId),
                                strParent = Convert.ToString(dr["iParent"]),
                                Attributes = Convert.ToString(dr["Attributes"])

                            };

                        }
                    }
                }
                else
                {
                    //jsondic["login"] = new EL_CurrentTrackerData("");
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("MyAccount.cs", "GetAllTrackerPointDataInJSON()", ex.ToString() + " " + ex.StackTrace.ToString());
            }


            return jsondic;
        }

        public DataTable GetTrackerPointData(EL_MyAccount p_objELMyAccount, bool IsAllData)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                ds = _objDALMyAccount.GetTrackerPointData(p_objELMyAccount, IsAllData);

                //Add vEventName to the query
                dt = ds.Tables[0].Copy();

                //Add EventName
                Bal_CommonEvents bal = new Bal_CommonEvents();
                dt = bal.GetDataTableWithEventName(ds, "vEventName", "vReportID");


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetTrackerPointData()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return dt;
        }

        public async Task<DataTable> UpdateExpandedClient(string ClientID)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                ds = await _objDALMyAccount.UpdateExpandedClient(ClientID);

                //Add EventName
                Bal_CommonEvents bal = new Bal_CommonEvents();
                dt = bal.GetDataTableWithEventName(ds, "vEventName", "vReportID");

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "UpdateExpandedClient()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return dt;
        }

        public async Task<Hashtable> GetCurrentBubbleExtraDetails(string asset_id)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();

            DataSet ds = new DataSet();

            var hashtable = new Hashtable();

            double? FuelValue = 0.0;

            try
            {
                ds = await _objDALMyAccount.GetCurrentFuelLevel(asset_id);

                var _lstEL_Fuel = new List<EL_Fuel>();

                hashtable[$"Map-{asset_id}"] = _lstEL_Fuel;

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    var AssetFuelLMappings = ds.Tables[0];

                    var _lastRow = ds.Tables[1].Rows[0];

                    foreach (DataRow _dr in AssetFuelLMappings.Rows)
                    {
                        var eL_Fuel = new EL_Fuel();

                        eL_Fuel.iAnalogNumber = Convert.ToInt32(_dr["iAnalogNumber"]);
                        eL_Fuel.sensorType = Convert.ToInt32(_dr["SensorType"]);
                        eL_Fuel.canBusType = Convert.ToInt32(_dr["ifkCanBusType"]);
                        eL_Fuel.oneWireID = Convert.ToString(_dr["DeviceId1Wire"]);
                        eL_Fuel.MappedName = Convert.ToString(_dr["vName"]);
                        eL_Fuel.AnalogType = Convert.ToInt32(_dr["AnalogType"]);

                        _lstEL_Fuel.Add(eL_Fuel);
                    }

                    var bal_temperature = new Bal_Temperature();

                    foreach (var item in _lstEL_Fuel)
                    {
                        var sensorType = item.sensorType;

                        var iAnalogNumber = item.iAnalogNumber;

                        var canBusType = item.canBusType;

                      

                        //handle fuel only
                        if ((sensorType == 7 && item.canBusType == 1) || item.AnalogType == 1)
                        {

                            if (sensorType != 7 && Convert.ToString(_lastRow["SensorData"]) != "")
                            {
                                 FuelValue = Bal_FuelUtils.GetFuelValue(Convert.ToString(_lastRow["SensorData"]), sensorType, iAnalogNumber);

                            }
                            else if (sensorType == 7 && Convert.ToString(_lastRow["ObdData"]) != "")
                            {
                             
                                FuelValue = Bal_FuelUtils.GetFuelValue(Convert.ToString(_lastRow["ObdData"]), sensorType, iAnalogNumber);

                            }

                            if (hashtable.ContainsKey($"Fuel-{item.MappedName}"))
                                item.MappedName = item.MappedName + "1";

                            hashtable[$"Fuel-{item.MappedName}"] = FuelValue;
                        }




                        //handle temperature
                        if ((sensorType == 7 && item.canBusType == 2) || item.AnalogType == 2)
                        {
                            var value = bal_temperature.DecipherValue(item, _lastRow) ?? 0.0;

                            hashtable[$"Temperature-{item.MappedName}"] = value;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetCurrentBubbleExtraDetails()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return hashtable;
        }
            public async Task<Hashtable> GetCurrentBubbleExtraDetails1(string asset_id )
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();

            DataSet ds = new DataSet();

            var hashtable = new Hashtable();

            double? FuelValue = 0.0;

            try
            {
                ds = await _objDALMyAccount.GetCurrentFuelLevel(asset_id);

                var _lstEL_Fuel = new List<EL_Fuel>();

                hashtable[$"Map-{asset_id}"] = _lstEL_Fuel;

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    var AssetFuelLMappings = ds.Tables[0];

                    var _lastRow = ds.Tables[1].Rows[0];

                    foreach (DataRow _dr in AssetFuelLMappings.Rows)
                    {
                        var eL_Fuel = new EL_Fuel();

                        eL_Fuel.iAnalogNumber = Convert.ToInt32(_dr["iAnalogNumber"]);
                        eL_Fuel.sensorType = Convert.ToInt32(_dr["SensorType"]);
                        eL_Fuel.canBusType = Convert.ToInt32(_dr["ifkCanBusType"]);
                        eL_Fuel.oneWireID = Convert.ToString(_dr["DeviceId1Wire"]);
                        eL_Fuel.MappedName = Convert.ToString(_dr["vName"]);
                        eL_Fuel.AnalogType = Convert.ToInt32(_dr["AnalogType"]);

                        _lstEL_Fuel.Add(eL_Fuel);
                    }

                    var bal_temperature = new Bal_Temperature();

                    foreach (var item in _lstEL_Fuel)
                    {
                        var sensorType = item.sensorType;

                        var iAnalogNumber = item.iAnalogNumber;

                        var canBusType = item.canBusType;                     


                        //handle fuel only
                        if ((sensorType == 7 && item.canBusType == 1) || item.AnalogType == 1)
                        {

                            if (sensorType != 7 && Convert.ToString(_lastRow["SensorData"]) != "")
                            {
                                var elAnalogSensor = JsonConvert.DeserializeObject<EL_AnalogSensorData>(Convert.ToString(_lastRow["SensorData"]));

                                if (sensorType == 1)
                                {
                                    if (iAnalogNumber == 1)
                                        FuelValue = elAnalogSensor.An1;


                                    if (iAnalogNumber == 2)
                                        FuelValue = elAnalogSensor.An2;

                                    if (iAnalogNumber == 3)
                                        FuelValue = elAnalogSensor.An3;


                                    if (iAnalogNumber == 4)
                                        FuelValue = elAnalogSensor.An4;

                                    if (iAnalogNumber == 5)
                                        FuelValue = elAnalogSensor.An5;

                                    if (iAnalogNumber == 6)
                                        FuelValue = elAnalogSensor.An6;
                                }

                                if (sensorType == 2)
                                {
                                    FuelValue = iAnalogNumber == 1 ? elAnalogSensor.S1 : elAnalogSensor.S2;
                                }
                                if (sensorType == 3)
                                {
                                    //   FuelValue = elAnalogSensor.OneW[0];
                                }
                            }
                            else if (sensorType == 7 && Convert.ToString(_lastRow["ObdData"]) != "")
                            {
                                var canBus = JsonConvert.DeserializeObject<EL_OBDData>(Convert.ToString(_lastRow["ObdData"]));

                                if ((canBusType == 1 || canBusType == 4) &&
                                    !string.IsNullOrEmpty(canBus.FuelL))
                                {
                                    FuelValue = Convert.ToDouble(canBus.FuelL.Split(' ')[0]);

                                }
                                else
                                {
                                    FuelValue = 0.0;

                                    LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetCurrentBubbleExtraDetails()", "There was a problem parsing fuel value");
                                }


                            }

                            if (hashtable.ContainsKey($"Fuel-{item.MappedName}"))
                                item.MappedName = item.MappedName + "1";

                            hashtable[$"Fuel-{item.MappedName}"] = FuelValue;
                        }




                        //handle temperature
                        if ((sensorType == 7 && item.canBusType == 2) || item.AnalogType == 2)
                        {
                            var value = bal_temperature.DecipherValue(item, _lastRow) ?? 0.0;

                            hashtable[$"Temperature-{item.MappedName}"] = value;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetCurrentBubbleExtraDetails()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return hashtable;
        }
        

        public DataSet GetAssetMenuForAdmin(EL_MyAccount p_objELMyAccount)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();
            try
            {
                ds = _objDALMyAccount.GetAssetMenuForAdmin(p_objELMyAccount);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetAssetMenuForAdmin()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

        public DataSet LoadMoreClients(EL_MyAccount p_objELMyAccount)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();
            try
            {
                ds = _objDALMyAccount.LoadMoreClients(p_objELMyAccount);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "LoadMoreClients()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

        public DataSet LoadMoreResellers(EL_MyAccount p_objELMyAccount)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();
            try
            {
                ds = _objDALMyAccount.LoadMoreResellers(p_objELMyAccount);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "LoadMoreResellers()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

        public DataSet GetUnrenderedClients(EL_MyAccount p_objELMyAccount)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();
            try
            {
                ds = _objDALMyAccount.GetUnrenderedClients(p_objELMyAccount);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetUnrenderedClient()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

        public DataSet GetClientGroups(EL_MyAccount p_objELMyAccount)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();

            try
            {
                ds = _objDALMyAccount.GetClientGroups(p_objELMyAccount);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetClientGroups()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

        public DataSet GetColumnMaster(EL_MyAccount p_objELMyAccount)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();

            try
            {
                ds = _objDALMyAccount.GetColumnMaster(p_objELMyAccount);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetColumnMaster()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

        public DataSet GetTrackerPointDataPerDevice(EL_MyAccount p_objELMyAccount)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();

            try
            {
                ds = _objDALMyAccount.GetTrackerPointDataPerDevice(p_objELMyAccount);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetTrackerPointDataPerDevice()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

        public DataSet GetScheduledReportData()
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();

            try
            {
                ds = _objDALMyAccount.GetScheduledReportData();
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetScheduledReportData()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

        public DataSet UpdateScheduledReportData(EL_MyAccount p_objELMyAccount)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();

            try
            {
                ds = _objDALMyAccount.UpdateScheduledReportData(p_objELMyAccount);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "UpdateScheduledReportData()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

        public DataSet CheckSheduleReportHistory(EL_MyAccount ObjParams)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();

            DataSet ds = new DataSet();

            try
            {
                ds = _objDALMyAccount.CheckSheduleReportHistory(ObjParams);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "UpdateScheduledReportData()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }


        public List<El_NewGroupDDL> ShowTripReplayGroupWithKendoUi(int TripCmpid, string TripORReport, int CompanyUniqueID, int pkUserID, int ifkUserTypeID)
        {

            DataSet ds = new DataSet();

            List<El_NewGroupDDL> objChildGroup = new List<El_NewGroupDDL>();

            DAL_MyAccount objclsNewGroup = new DAL_MyAccount();

            int iParentID = 0;
            int ifkGroupMID = 0;
            int iParentGMID = 0;

            if (ifkUserTypeID == 1)
            {
                if (TripCmpid != -1 || TripCmpid != 0)
                {
                    iParentID = TripCmpid;
                    objclsNewGroup.Operation = 77;
                }
                else
                {
                    objclsNewGroup.Operation = 7;
                }

            }
            else if (ifkUserTypeID == 2)
            {
                if (TripCmpid != -1 || TripCmpid != 0)
                {
                    iParentID = TripCmpid;
                    objclsNewGroup.Operation = 77;
                }
                else
                {
                    objclsNewGroup.Operation = 8;
                    iParentID = CompanyUniqueID;
                }

            }
            else if (ifkUserTypeID == 3)
            {
                if (TripCmpid != -1 || TripCmpid != 0)
                {
                    iParentID = TripCmpid;
                    objclsNewGroup.Operation = 9;
                }
            }

            ds = objclsNewGroup.GetGroups(iParentGMID, iParentID);

            if ((ds.Tables.Count) > 0 && (ds.Tables[0].Rows.Count) > 0)
            {
                int count = 0;

                //objclsNewGroup.Operation = 145;
                //iParentID = TripCmpid;
                //count = objclsNewGroup.CountDeviceInAllGroup(iParentID, pkUserID);

                //Parent Group - Parent Level
                int ParentCount = ds.Tables[0].Rows.Count;
                for (int r = 0; r < ParentCount; r++)
                {
                    ifkGroupMID = Convert.ToInt32(ds.Tables[0].Rows[r]["ipkGroupMID"].ToString());
                    count = objclsNewGroup.CountDeviceInGroup(ifkGroupMID);

                    objChildGroup.Add(new El_NewGroupDDL(
                                Convert.ToString(ds.Tables[0].Rows[r]["vpkGroupName"].ToString() + "(" + count + ")"),
                                Convert.ToInt32(ds.Tables[0].Rows[r]["ipkGroupMID"].ToString())
                            )
                        );

                    DataSet dsParent = new DataSet();

                    iParentGMID = Convert.ToInt32(ds.Tables[0].Rows[r]["ipkGroupMID"].ToString());
                    iParentID = TripCmpid;
                    objclsNewGroup.Operation = 11;
                    dsParent = objclsNewGroup.GetGroups(iParentGMID, iParentID);

                    //Child Groups - First Inner Level
                    if ((dsParent.Tables.Count) > 0 && (dsParent.Tables[0].Rows.Count) > 0)
                    {
                        int ChildCount = dsParent.Tables[0].Rows.Count;
                        for (int j = 0; j < ChildCount; j++)
                        {
                            ifkGroupMID = Convert.ToInt32(dsParent.Tables[0].Rows[j]["ipkGroupMID"].ToString());
                            count = objclsNewGroup.CountDeviceInGroup(ifkGroupMID);

                            objChildGroup.Add(new El_NewGroupDDL(
                                        Convert.ToString((Convert.ToString(dsParent.Tables[0].Rows[j]["vpkGroupName"].ToString() + "[" + count + "]"))),
                                        Convert.ToInt32(dsParent.Tables[0].Rows[j]["ipkGroupMID"].ToString())
                                    )
                                );

                            DataSet dsChildGroup = new DataSet();

                            iParentGMID = Convert.ToInt32(dsParent.Tables[0].Rows[j]["ipkGroupMID"].ToString());
                            iParentID = TripCmpid;
                            objclsNewGroup.Operation = 11;
                            dsChildGroup = objclsNewGroup.GetGroups(iParentGMID, iParentID);

                            //Grand-Children Groups - Second Inner Level
                            if ((dsChildGroup.Tables.Count) > 0 && (dsChildGroup.Tables[0].Rows.Count) > 0)
                            {
                                int GrandChildCount = dsChildGroup.Tables[0].Rows.Count;
                                for (int c = 0; c < GrandChildCount; c++)
                                {
                                    ifkGroupMID = Convert.ToInt32(dsChildGroup.Tables[0].Rows[c]["ipkGroupMID"].ToString());
                                    count = objclsNewGroup.CountDeviceInGroup(ifkGroupMID);

                                    objChildGroup.Add(new El_NewGroupDDL(
                                                Convert.ToString((Convert.ToString(dsChildGroup.Tables[0].Rows[c]["vpkGroupName"].ToString() + "{" + count + "}"))),
                                                Convert.ToInt32(dsChildGroup.Tables[0].Rows[c]["ipkGroupMID"].ToString())
                                            )
                                        );

                                    DataSet dssubchildGroup = new DataSet();

                                    iParentGMID = Convert.ToInt32(dsChildGroup.Tables[0].Rows[c]["ipkGroupMID"].ToString());
                                    iParentID = TripCmpid;
                                    objclsNewGroup.Operation = 11;
                                    dssubchildGroup = objclsNewGroup.GetGroups(iParentGMID, iParentID);

                                    //Great Grand-Children Groups - Third Inner Level
                                    if ((dssubchildGroup.Tables.Count) > 0 && (dssubchildGroup.Tables[0].Rows.Count) > 0)
                                    {
                                        int GreatGrandChildCount = dssubchildGroup.Tables[0].Rows.Count;
                                        for (int d = 0; d < GreatGrandChildCount; d++)
                                        {
                                            ifkGroupMID = Convert.ToInt32(dssubchildGroup.Tables[0].Rows[d]["ipkGroupMID"].ToString());
                                            count = objclsNewGroup.CountDeviceInGroup(ifkGroupMID);

                                            objChildGroup.Add(new El_NewGroupDDL(
                                                          Convert.ToString((Convert.ToString(dssubchildGroup.Tables[0].Rows[d]["vpkGroupName"].ToString() + "[{" + count + "}]"))),
                                                          Convert.ToInt32(dssubchildGroup.Tables[0].Rows[d]["ipkGroupMID"].ToString())
                                                      )
                                                  );
                                        }
                                    }
                                }
                            }

                        }
                    }

                }
            }
            return objChildGroup;
        }


        public string SaveTermsAndConditions(int iParent, int ipkUserID)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            string returnStr = "";

            try
            {
                returnStr = _objDALMyAccount.SaveTermsAndConditions(iParent, ipkUserID);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "SaveTermsAndConditions()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return returnStr;
        }

        public string ShowDeviceSMSConfig(int ifkAssetID, int Operation)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            _objDALMyAccount.ifkAssetID = ifkAssetID;
            _objDALMyAccount.Operation = Operation;

            string _Port = "0";
            DataTable _Config = new DataTable();

            try
            {
                DataSet ds = new DataSet();
                ds = _objDALMyAccount.ShowDeviceSMSConfig();

                if (ds.Tables.Count > 1)
                {
                    _Config = ds.Tables[0].Copy();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        _Port = Convert.ToString(ds.Tables[1].Rows[0]["vPort"]);
                    }
                }
                else if (ds.Tables.Count == 1)
                {
                    _Port = Convert.ToString(ds.Tables[0].Rows[0]["vPort"]);
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "ShowDeviceSMSConfig()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            var data = new
            {
                Config = _Config,
                Port = _Port

            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public string ShowOdoReading(int ifkAssetID, int Operation)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            _objDALMyAccount.ifkAssetID = ifkAssetID;
            _objDALMyAccount.Operation = Operation;
            string result = "";

            try
            {
                DataSet ds = new DataSet();
                ds = _objDALMyAccount.ShowOdoReading();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "ShowOdoReading()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return result;

        }
        public string UpdateOdoReading(int ifkAssetID, int odometer, DateTime odometerDate, int Operation)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            _objDALMyAccount.ifkAssetID = ifkAssetID;
            _objDALMyAccount.Operation = Operation;
            _objDALMyAccount.odometerReading = odometer;
            _objDALMyAccount.odometerDate = odometerDate;

            string result = "";

            try
            {
                DataSet ds = new DataSet();
                result = _objDALMyAccount.UpdateOdoReading();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "UpdateOdoReading()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return result;

        }

        public string GetDeviceDigitalOutputs(int ifkAssetID, int Operation)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            _objDALMyAccount.ifkAssetID = ifkAssetID;
            _objDALMyAccount.Operation = Operation;

            string result = "";

            try
            {
                DataSet ds = new DataSet();
                ds = _objDALMyAccount.GetDeviceDigitalOutputs();

                DataTable dt = ds.Tables[0].Copy();

                var data = new
                {
                    data = dt
                };

                result = JsonConvert.SerializeObject(data, Formatting.Indented);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetDeviceDigitalOutputs()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return result;

        }

        public void UpdateScheduledReports(int Operation, int ifkReportID, bool Success, string vErrorMessage, int _ifkresellerId, int _ifkClientId, bool _bShowtoreseller)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            _objDALMyAccount.ifkReportID = ifkReportID;
            _objDALMyAccount.Operation = Operation;
            _objDALMyAccount.bSuccess = Success;
            _objDALMyAccount.vErrorMessage = vErrorMessage;

            _objDALMyAccount.ifkResellerID = _ifkresellerId;
            _objDALMyAccount.ifkClientID = _ifkClientId;
            _objDALMyAccount.bShowtoReseller = _bShowtoreseller;



            _objDALMyAccount.UpdateScheduledReportsLogs();


        }
        public string CheckPWAStatus(string AppUrl, int UserID)
        {

            string result = "";
            var isPwaEnabled = false;

            try
            {
                DataSet ds = new DataSet();

                DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
                ds = _objDALMyAccount.CheckPWAStatus(AppUrl, UserID);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    isPwaEnabled = Convert.ToBoolean(ds.Tables[0].Rows[0]["PwaIsEnabled"]);
                }

                var data = new
                {
                    PwaIsEnabled = isPwaEnabled
                };

                result = JsonConvert.SerializeObject(data, Formatting.Indented);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "CheckPWAStatus()", ex.Message + ex.StackTrace);

            }
            finally
            {

            }

            return result;

        }
        public string SaveMapTelemetrySettings(DAL_MyAccount _objDALMyAccount)
        {

            string result = "";

            try
            {
                DataSet ds = new DataSet();
                result = _objDALMyAccount.SaveMapTelemetrySettings();

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "SaveMapTelemetrySettings()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return result;

        }

        public string FetchMapTelemetrySettings(DAL_MyAccount _objDALMyAccount)
        {

            string result = "";

            try
            {
                DataSet ds = new DataSet();
                ds = _objDALMyAccount.FetchMapTelemetrySettings();

                DataTable dt = ds.Tables[0].Copy();

                var data = new
                {
                    data = dt
                };

                result = JsonConvert.SerializeObject(data, Formatting.Indented);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "FetchMapTelemetrySettings()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return result;

        }
        public DataSet GetUnloadedClient(EL_MyAccount p_objELMyAccount)
        {
            DAL_MyAccount _objDALMyAccount = new DAL_MyAccount();
            DataSet ds = new DataSet();
            try
            {
                ds = _objDALMyAccount.GetUnloadedClient(p_objELMyAccount);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_MyAccount.cs", "GetUnloadedClient()", ex.Message + ex.StackTrace);

            }
            finally
            {
                _objDALMyAccount = null;
            }

            return ds;
        }

    }
}
