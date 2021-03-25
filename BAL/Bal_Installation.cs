using System;
using System.Collections.Generic;
using System.Data;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using Newtonsoft.Json;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
  
   
    public class Bal_Installation
    {
        private static Bal_Microservice _Microservice;
        public Bal_Installation()
        {
            _Microservice = new Bal_Microservice();

         
        }
        

        public List<string> GetAvalaibleIme(EL_Installation __EL_Installation)
        {

            var list = new List<string>();

            var results = DAL_Installation.ExecuteInstallationAction(__EL_Installation);


            foreach( DataTable dt in results.Tables)
                foreach( DataRow row in dt.Rows)
                {
                    list.Add(Convert.ToString(row["ImeiNumber"]));
                }

            return list;
        }

        public void  UpdateAssetWithImei(EL_Installation __EL_Installation)
        {       
          DAL_Installation.ExecuteInstallationAction(__EL_Installation);           
        }

        public EL_Installation LoadSearchWindow(EL_Installation __EL_Installation)  {
             
                  var _results = new EL_Installation();

                  __EL_Installation.Operation = 1;
     
                 __EL_Installation.InstallDate = DateTime.UtcNow;

                 var results = DAL_Installation.ExecuteInstallationAction(__EL_Installation);

                 var index = 0;

                 foreach( DataTable _table in results.Tables)
                {
                    if (index==0)
                    {
                      foreach(DataRow _row in _table.Rows)
                        {
                            _results.Assets.Add(new El_Device {
                                AssetPhoto = Convert.ToString(_row["AssetPhoto"]),
                                deviceName = Convert.ToString(_row["AssetName"]),
                                ifkAssetId = Convert.ToInt32(_row["ifk_AssignedAssetId"]),
                                vpkDeviceID = Convert.ToInt64(_row["ImeiNumber"]),
                            });
                        }
                    }
                    else
                    {
                        foreach (DataRow _row in _table.Rows)
                        {
                            _results.Installers.Add(new EL_Installer
                            {
                                InstallerId = Convert.ToInt32(_row["ifkInstallerId"]),                                
                                InstallerName = Convert.ToString(_row["InstallerName"]),
                                InstallerPhoto = Convert.ToString(_row["PhotoName"])
                             
                            });
                        }
                    }

                    index += 1;
                }
                      
                 return _results;

        }

        public EL_Installation CreateNewInstallation(EL_Installation __EL_Installation)
        {    
            __EL_Installation.InstallDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(__EL_Installation.InstallDate, __EL_Installation.vTimeZone);
                     
            var results = DAL_Installation.ExecuteInstallationAction(__EL_Installation);
            // the  very first  device status check invokation 
            new Bal_Microservice().StartDeviceStatusCheckJob(__EL_Installation);
            
            return new EL_Installation();
        }


        public List<EL_Installation> GetInstallationList(EL_Installation __EL_Installation)
        {
            var _list = new List<EL_Installation>();


            var ds = DAL_Installation.ExecuteInstallationAction(__EL_Installation).Copy();
                                                  
            foreach(DataTable dt in ds.Tables)
            {

                foreach(DataRow row in dt.Rows)
                {
                    _list.Add(new EL_Installation
                    {
                       ifkInstallationID  = Convert.ToInt32(row["installationID"]),
                        dInstallationDate = Convert.ToDateTime(row["dInstallationDate"]),
                        vAssetName = Convert.ToString(row["AssetName"]),
                        vAssetTypeName = Convert.ToString(row["vDeviceType"]),
                        IMEI = Convert.ToInt64(row["ImeiNumber"]),
                        vGsmNumber = Convert.ToString(row["GsmNumber"]),
                        ifkDeviceId = Convert.ToInt32(row["ifk_AssignedAssetId"]),
                        OwnersName = Convert.ToString(row["vOwnersName"]),
                        OwnersPhone = Convert.ToString(row["vOwnersPhone"]),
                        InstallAddress = Convert.ToString(row["vInstallAddress"]),

                        InstallerId = Convert.ToInt32(row["InstallerId"]),
                        vInstallerName = Convert.ToString(row["InstallerName"]),
                        vInstallerPhoto = Convert.ToString(row["PhotoName"]),

                        InstallDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( Convert.ToDateTime(row["dInstallDate"]),__EL_Installation.vTimeZone),
                        iStatus = Convert.ToInt32(row["iStatus"]),
                        vInstallationCriteriaIdList = JsonConvert.DeserializeObject<string[]>(Convert.ToString(row["vInstallationCriteria"]))                                          


                    });
                }

            }
            return _list;
        }
        public EL_Installation GetInstallationItem(EL_Installation __EL_Installation)
        {
            var _installationItem = new EL_Installation();

            var ExtraDeviceData = "[]";

            var ds = DAL_Installation.ExecuteInstallationAction(__EL_Installation).Copy();

            var _index = 0;

            foreach (DataTable dt in ds.Tables)
            {
                if (_index == 0)
                {
                    foreach (DataRow row in dt.Rows)  //table 1
                    {
                        _installationItem = new EL_Installation
                        {
                            ifkInstallationID = Convert.ToInt32(row["installationID"]),
                            vAssetName = Convert.ToString(row["AssetName"]),
                            vAssetTypeName = Convert.ToString(row["vDeviceType"]),
                            IMEI = Convert.ToInt64(row["ImeiNumber"]),
                            vGsmNumber = Convert.ToString(row["GsmNumber"]),

                            OwnersName = Convert.ToString(row["vOwnersName"]),
                            OwnersPhone = Convert.ToString(row["vOwnersPhone"]),
                            InstallAddress = Convert.ToString(row["vInstallAddress"]),

                            InstallerId = Convert.ToInt32(row["InstallerId"]),
                            vInstallerName = Convert.ToString(row["InstallerName"]),
                            vInstallerPhoto = Convert.ToString(row["PhotoName"]),
                            ifk_AssignedAssetId = Convert.ToInt32(row["ifk_AssignedAssetId"]),

                            InstallDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( Convert.ToDateTime(row["dInstallDate"]),__EL_Installation.vTimeZone),
                            iStatus = Convert.ToInt32(row["iStatus"]),
                            vInstallationCriteriaIdList = JsonConvert.DeserializeObject<string[]>(Convert.ToString(row["vInstallationCriteria"])),
                            bAllowOnlyComplete = Convert.ToInt32(row["bAllowOnlyComplete"])


                        };
                    }
                }
                else if (_index == 1) //  table 2 
                {
                    _installationItem.oEventsCriteriaList  = new List<InstallerEventCriteria>();

                    foreach(DataRow row in dt.Rows)
                    {
                        _installationItem.oEventsCriteriaList.Add(new InstallerEventCriteria
                        {
                            ipkCriteriaId = Convert.ToInt32(row["ipkCriteriaId"]),
                            iEventCriteriId = Convert.ToInt32(row["iEventCriteriId"]),
                            vEventCriteriaName = Convert.ToString(row["eventName"]),
                            dInstallationDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["dRecordFoundDate"]), __EL_Installation.vTimeZone),                         
                            vAddress = Convert.ToString(row["vLocation"]),
                            iStatus = Convert.ToInt32(row["iStatus"]),
                            vRaw = Convert.ToString(row["vRaw"]),
                        });
                    }
                          
                }


                else if (_index == 2) //  table 3
                {

                    ExtraDeviceData = JsonConvert.SerializeObject(dt);

                }
                _index += 1;
            }


            if (__EL_Installation.includeAssetsAndInstallers)
            {
                var _result2 = LoadSearchWindow(__EL_Installation);

                _installationItem.Assets = _result2.Assets;

                _installationItem.Installers = _result2.Installers;


            }

            _installationItem.ExtraDeviceData = ExtraDeviceData;

            return _installationItem;
        }

       public EL_Installation GetInstallationStatus(EL_Installation __EL_Installation)
        {
            var _installationItem = new EL_Installation();

            var _dates = calenderDatetimeHelper(__EL_Installation);

            __EL_Installation.dStart = _dates.Item1;

            __EL_Installation.dEnd = _dates.Item2;


            var ds = DAL_Installation.ExecuteInstallationAction(__EL_Installation).Copy();


            foreach(DataTable dt  in ds.Tables)
            {
                foreach(DataRow row in dt.Rows)
                {
                   

                    _installationItem.oEventsCriteriaList.Add(new InstallerEventCriteria
                    {
                        ipkCriteriaId = Convert.ToInt32(row["ipkCriteriaId"]),
                        iEventCriteriId = Convert.ToInt32(row["iEventCriteriId"]),
                        vEventCriteriaName = Convert.ToString(row["eventName"]),
                        dInstallationDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( Convert.ToDateTime(row["dRecordFoundDate"]), __EL_Installation.vTimeZone),
                        vAddress = Convert.ToString(row["vLocation"]),
                        iStatus = Convert.ToInt32(row["iStatus"]),
                        vRaw = Convert.ToString(row["vRaw"]),
                    });
                    _installationItem.iStatus = Convert.ToInt32(row["iStatus"]);

                }
            }
            return _installationItem;
        }

        public string UpdateInstallation(EL_Installation __EL_Installation)
        {
            __EL_Installation.Operation = 7;

            var _resultO = "seccuss";

            var _installationItem = new EL_Installation();

            try
            {

                //__EL_Installation.InstallDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(__EL_Installation.InstallDate, __EL_Installation.vTimeZone);

                var ds = DAL_Installation.ExecuteInstallationAction(__EL_Installation).Copy();
            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "Bal_Installation.cs", "UpdateInstallation()", ex.Message  + ex.StackTrace);
                _resultO = ex.Message;
            }
            return _resultO;
        }


        public InstallationStatus GetInstallationWebHookDetails( EL_Installation __EL_Installation)
        {
            var _InstallationStatus = new InstallationStatus ();

            var __Deserialized_EL_Installation = JsonConvert.DeserializeObject<EL_Installation>(JsonConvert.SerializeObject(__EL_Installation));

            __Deserialized_EL_Installation.Operation = 11;

            var ds = DAL_Installation.ExecuteInstallationAction(__Deserialized_EL_Installation);           

            foreach (DataTable dt in ds.Tables)
                foreach (DataRow row in dt.Rows)
                {
                    _InstallationStatus.vInstallationUrl = Convert.ToString(row["vInstallationUrl"]);

                    _InstallationStatus.vDe_InstallationUrl = Convert.ToString(row["vDe_InstallationUrl"]);
                }
                       
            return _InstallationStatus;
        } 

        // function overload
        public InstallationStatus GetInstallationWebHookDetails(EL_Installation __EL_Installation, int operation)
        {          

            var _installationDetails = new InstallationStatus(); 

            var __Deserialized_EL_Installation = JsonConvert.DeserializeObject<EL_Installation>(JsonConvert.SerializeObject(__EL_Installation));

            __Deserialized_EL_Installation.Operation = 13;

            var ds = DAL_Installation.ExecuteInstallationAction(__Deserialized_EL_Installation);

            int tableCount = 0;

            foreach (DataTable dt in ds.Tables)
            {
                if (tableCount == 0)
                {
                    foreach (DataRow row in dt.Rows)
                        _installationDetails.vInstallationUrl = Convert.ToString(row["vInstallationWebHookEndPoint"]);
                }

                if (tableCount == 1)
                {    
                        foreach (DataRow row in dt.Rows)
                            _installationDetails.DeviceStatusLogs.Add(new EL_DeviceStatusCheck {
                                CustomId = Convert.ToString(row["CustomId"]),
                                IMEI = Convert.ToString(row["imei"]),
                                LastReportedDate =  Convert.ToDateTime(row["dGPSDateTime"])

                            });


                  
                }

                tableCount++;
            }
                                   
            return _installationDetails;
        }
        public string    CompleteInstallation(EL_Installation __EL_Installation)
        {
            var _reurnString = "0";

         
                 
            try
            {
                __EL_Installation.InstallDate = DateTime.UtcNow;

                DAL_Installation.ExecuteInstallationAction(__EL_Installation).Copy();

             

                var _InstallationStatus = GetInstallationWebHookDetails(__EL_Installation);

                _InstallationStatus.Installation = true;

                _InstallationStatus.Imei = __EL_Installation.IMEI;

                _Microservice.StartWebHookInstallationJob(_InstallationStatus); 

                _reurnString = "1";               

            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "Bal_Installation.cs", "CompleteInstallation()", ex.Message  + ex.StackTrace);

            }
                                          
            return _reurnString;
        }

        
            public DataSet GetInstallTelemetry(EL_Installation __EL_Installation)
            {
            var totalSeconds = -1* DateTime.UtcNow.Subtract(UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, __EL_Installation.vTimeZone)).TotalSeconds;

            __EL_Installation.iSeconds = totalSeconds;

                   return DAL_Installation.ExecuteInstallationAction(__EL_Installation);
            }

            public bool  DeInstallDevice( EL_Installation __EL_Installation )
        {

         
            try
            {
                DAL_Installation.ExecuteInstallationAction(__EL_Installation).Copy();

                // start deinstallation process 
                var _InstallationStatus = GetInstallationWebHookDetails(__EL_Installation);

                _InstallationStatus.Installation = false;

                _InstallationStatus.Imei = __EL_Installation.IMEI;

                _Microservice.StartWebHookInstallationJob(_InstallationStatus);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
          
        }

        public bool DeleteInstallation(EL_Installation __EL_Installation)
        {

            try
            {
                DAL_Installation.ExecuteInstallationAction(__EL_Installation);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public DataSet  SaveTestInstallationTimeStamps(EL_Installation __EL_Installation)
        {
            __EL_Installation.dStart = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(__EL_Installation.dStart, __EL_Installation.vTimeZone);
            __EL_Installation.dEnd = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(__EL_Installation.dEnd, __EL_Installation.vTimeZone);
            return  DAL_Installation.ExecuteInstallationAction(__EL_Installation);

        }
               
        public List<EL_Installation> InstallationHistory(EL_Installation __EL_Installation)
        {
            __EL_Installation.Operation = 8;

            var _list = new List<EL_Installation>();

            var _installationItem = new EL_Installation();
            try
            {
               var ds =  DAL_Installation.ExecuteInstallationAction(__EL_Installation).Copy();
               
                foreach(DataTable dt in ds.Tables)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        _list.Add(new EL_Installation
                        {
                            ifkInstallationID = Convert.ToInt32(row["installationID"]),
                            dInstallationDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( Convert.ToDateTime(row["dInstallationDate"]), __EL_Installation.vTimeZone),
                            vAssetName = Convert.ToString(row["AssetName"]),
                            vAssetTypeName = Convert.ToString(row["vDeviceType"]),
                            IMEI = Convert.ToInt64(row["ImeiNumber"]),
                            vGsmNumber = Convert.ToString(row["GsmNumber"]),

                            iTestStatus = Convert.ToInt32(row["iTestStatus"]),

                            OwnersName = Convert.ToString(row["vOwnersName"]),
                            OwnersPhone = Convert.ToString(row["vOwnersPhone"]),
                            InstallAddress = Convert.ToString(row["vInstallAddress"]),

                            InstallerId = Convert.ToInt32(row["InstallerId"]),
                            vInstallerName = Convert.ToString(row["InstallerName"]),
                            vInstallerPhoto = Convert.ToString(row["PhotoName"]),

                            InstallDate =UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(  Convert.ToDateTime(row["dInstallDate"]),__EL_Installation.vTimeZone),
                            iStatus = Convert.ToInt32(row["iStatus"]),
                            vInstallationCriteriaIdList = JsonConvert.DeserializeObject<string[]>(Convert.ToString(row["vInstallationCriteria"]))


                        });
                    }
                }

            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "Bal_Installation.cs", "CompleteInstallation()", ex.Message  + ex.StackTrace);

            }

            return _list;
        }

        public List<EL_Installer> OpenInstallersMembers(EL_Installation __EL_Installation)
        {
            __EL_Installation.Operation = 9;

            var _list = new List<EL_Installer>();
       
            try
            {
                var ds = DAL_Installation.ExecuteInstallationAction(__EL_Installation).Copy();

                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow _row in dt.Rows)
                    {
                        _list.Add(new EL_Installer
                        {
                            InstallerId = Convert.ToInt32(_row["ifkInstallerId"]),
                            InstallerName = Convert.ToString(_row["InstallerName"]),
                            InstallerPhoto = Convert.ToString(_row["PhotoName"]),
                            vEmail = Convert.ToString(_row["vEmail"]),
                            vPhone = Convert.ToString(_row["vMobile"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "Bal_Installation.cs", "CompleteInstallation()", ex.Message  + ex.StackTrace);

            }

            return _list;
        }

        public static Tuple<DateTime, DateTime, double> calenderDatetimeHelper(EL_Installation _EL_Installation)
        {

            var i = _EL_Installation.slcReverseTimeframe;

            var TimeZoneId = _EL_Installation.vTimeZone;

            var dStartDate = new DateTime();
           
            var UtcNow = DateTime.UtcNow;           

            DateTime userTimeNow = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(UtcNow, TimeZoneId);

            var timeDifference = userTimeNow.Subtract(UtcNow);

            if (i<10) {
                switch (i)
                {
                    case -1:
                        //today 
                        dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.Date, TimeZoneId);

                        UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                        break;

                    case -2:
                        //yesterday 
                        var yesterdayEnd = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.Date, TimeZoneId).AddSeconds(-1);

                        dStartDate = yesterdayEnd.Date;

                        UtcNow = yesterdayEnd;
                        break;

                    default:
                        UtcNow = DateTime.UtcNow;

                        dStartDate = UtcNow.AddMinutes(-10);
                        break;

                }
            }
            else
            {
               
                UtcNow = DateTime.UtcNow;

                dStartDate = UtcNow.AddMinutes(-i);

            }

            return new Tuple<DateTime, DateTime, double>(dStartDate, UtcNow, timeDifference.TotalMinutes);

        }



    }



}
