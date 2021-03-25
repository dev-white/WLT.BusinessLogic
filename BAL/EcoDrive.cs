using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class EcoDrive
    {
        float harshBrakePointsPerViolation = 20;
        float harshAccPointsPerViolation = 20;
        float harshCornerPointsPerViolation = 20;
        float idlePointsPerViolation = 5;
        float softwareOverspeedPointsPerEpisode = 20;
        float hardwareOverspeedPointsPerViolation = 5;



      
        float maxNoOfHarshBrakesIn24Hours  = 10;

        float maxNoOfHarshAccIn24Hours     = 30;

        float MaxNoOfHarshCornerIn24Hours  = 10;

        float MaxOverspeedDifference = 70;

     

        public El_Eco_Drive_Model GetEcoDriveScore(DataTable _Violations, DateTime startDate, DateTime endDateTime, long imei ,string TimeZoneId)
        {
            var _eco_Obj = new El_Eco_Drive_Model();

            //Trip Score Beta by MD
            //1 to 20 points per EVENT violation is a good start for non overspeed/idle events (time based events logic needs to be different)

            //1 Do DB lookup and get all violation data for time period
            DataSet dsViolations = new DataSet();



            dsViolations.Tables.Add(_Violations);       //GetAllTripScoreViolationsForTimeRange_SingleAsset(imei, startDate, endDateTime, TimeZoneId);

            //DataSet dsMinMaxOdo = new DataSet();
            double distance = GetDistanceTravelledForEcoScore(imei, startDate, endDateTime, TimeZoneId);

            //DataTable violations = dsViolations.Tables["tblCommonTrackingData"];

            DataTable violations = dsViolations.Tables[0].Copy();

            //2 Work out INSTANCE based score

            float noOfHarshBrakes = (from myRow in violations.AsEnumerable()
                                     where myRow.Field<int>("vReportId") == 2
                                     select myRow).Count(); //select count(*) from dsViolations where reportID = 2

            float harshBrakeScore = noOfHarshBrakes * harshBrakePointsPerViolation;

            float noOfHarshAcc = (from myRow in violations.AsEnumerable()
                                  where myRow.Field<int>("vReportId") == 3
                                  select myRow).Count();//select count(*) from dsViolations where reportID = 3

            float harshAccScore = noOfHarshAcc * harshAccPointsPerViolation;

            float noOfHarshCorner = (from myRow in violations.AsEnumerable()
                                     where myRow.Field<int>("vReportId") == 14
                                     select myRow).Count();

            float harshCornerScore = noOfHarshCorner * harshCornerPointsPerViolation;

            float noOfOverSpeedSoftwareEpisodes = (from myRow in violations.AsEnumerable()
                                                   where myRow.Field<int>("vReportId") == 820
                                                       || myRow.Field<int>("vReportId") == 824
                                               
                                                   select myRow).Count(); 



            float noOfOverSpeedsFromHardware = (from myRow in violations.AsEnumerable()
                                                where myRow.Field<int>("vReportId") == 1
                                                select myRow).Count();

            float OverSpeedScoreSoftware = noOfOverSpeedSoftwareEpisodes * softwareOverspeedPointsPerEpisode;


            float OverSpeedScoreFromHardware = noOfOverSpeedsFromHardware * hardwareOverspeedPointsPerViolation;


            float totalScore = harshBrakeScore + harshAccScore + harshCornerScore + OverSpeedScoreSoftware + OverSpeedScoreFromHardware;


            float subscore = totalScore / (float)distance;

            float score = 100 - subscore;

            if (score < 1) score = 0;

            _eco_Obj.Score = score;

            _eco_Obj.Harsh_Acceleration_score = noOfHarshAcc;
            _eco_Obj.Harsh_Braking_score = noOfHarshBrakes;
            _eco_Obj.Harsh_Conering_score = noOfHarshCorner;

             _eco_Obj.Overspeed_score = OverSpeedScoreSoftware + OverSpeedScoreFromHardware;

            _eco_Obj.SWOverspeedCount =(int) noOfOverSpeedSoftwareEpisodes;

            _eco_Obj.HWOverspeedCount = (int)noOfOverSpeedsFromHardware;

            _eco_Obj.HWOverspeedCount = (int)noOfOverSpeedsFromHardware;

            return _eco_Obj;
        }

         public El_Eco_Drive_Model GetEcoDriveScore(EL_EcoScoreModel _EL_EcoScoreModel)
        {

            var _El_Eco_ScoreConfig = GetAssetEcoConfig(_EL_EcoScoreModel.AssetId);


            var _eco_Obj = new El_Eco_Drive_Model();

            DataSet dsViolations = new DataSet();

            dsViolations.Tables.Add(_EL_EcoScoreModel._Violations);

            TimeSpan timeDuration = _EL_EcoScoreModel.endDateTime.Subtract(_EL_EcoScoreModel.startDate);

            var allowableHarshbrakes = _El_Eco_ScoreConfig.maxNoOfHarshBrakesIn24Hours * timeDuration.TotalHours / 24;

            var allowableHarshAcc = _El_Eco_ScoreConfig.maxNoOfHarshAccIn24Hours * timeDuration.TotalHours / 24;

            var allowableHarshCorner = _El_Eco_ScoreConfig.MaxNoOfHarshCornerIn24Hours * timeDuration.TotalHours / 24;

            double distance = GetDistanceTravelledForEcoScore(_EL_EcoScoreModel.imei, _EL_EcoScoreModel.startDate, _EL_EcoScoreModel.endDateTime, _EL_EcoScoreModel.TimeZoneId);

            DataTable violations = dsViolations.Tables[0].Copy();

            int noOfHarshBrakes = 0;

            int noOfHarshAcc = 0;

            int noOfHarshCorner = 0;

            int noOfOverSpeedsFromHardware = 0;

            int noOfOverSpeedsFromRoadSpeedLimit = 0;

            int timeSpentOverRoadSpeedLimit = 0;


            var acummulatedSoftWareSpeedDifference = 0.0;

            var acummulatedRoadSpeedDifference = 0.0;

            float noOfOverSpeedsFromSoftwareLimit = 0;

            int timeSpentOverSoftwareLimit = 0;


            var iterations = 0;

            foreach (DataRow row in violations.Rows)
            {
                if (Convert.ToInt16(row["vReportID"]) == 2) noOfHarshBrakes += 1;

                if (Convert.ToInt16(row["vReportID"]) == 3) noOfHarshAcc += 1;

                if (Convert.ToInt16(row["vReportID"]) == 14) noOfHarshCorner += 1;

                if (Convert.ToInt16(row["vReportID"]) == 821)
                {
                    var speedLimit = 0.0;

                    if (int.TryParse(row["additionalEventInfo"].ToString(), out _))
                        timeSpentOverRoadSpeedLimit += Convert.ToInt32(row["additionalEventInfo"]);

                    double.TryParse(row["vRoadSpeed"].ToString(), out speedLimit);

                    noOfOverSpeedsFromRoadSpeedLimit += 1;

                    if (speedLimit > 0)
                    {
                        var speedDifference = Convert.ToDouble(row["vVehicleSpeed"]) - speedLimit;

                        acummulatedRoadSpeedDifference += speedDifference;
                    }

                }

                if (Convert.ToInt16(row["vReportID"]) == 824)
                {

                    if (int.TryParse(row["additionalEventInfo"].ToString(), out _))
                        timeSpentOverSoftwareLimit += Convert.ToInt32(row["additionalEventInfo"]);

                    noOfOverSpeedsFromSoftwareLimit += 1;

                    var speedLimit = GetAssetSpeedLimit(_EL_EcoScoreModel.imei);

                    if (speedLimit > 0)
                    {
                        var speedDifference = Convert.ToDouble(row["vVehicleSpeed"]) - speedLimit;

                        acummulatedSoftWareSpeedDifference += speedDifference;
                    }
                }

                iterations += 1;
            }

            Func<double, double> Clamp = (value) =>
            {
                var max = 1;

                return value > max ? max : value;
            };



            double harshBrakeScore = Clamp(noOfHarshBrakes / allowableHarshbrakes);

            double harshAccScore = Clamp(noOfHarshAcc / allowableHarshAcc);

            double harshCornerScore = Clamp(noOfHarshCorner / allowableHarshCorner);

            var SpeedScore = 0.0;

            if (timeSpentOverSoftwareLimit > 0)
            {               
                _eco_Obj.SW_AVG_TimeSpentOverspeeding = timeSpentOverSoftwareLimit / noOfOverSpeedsFromSoftwareLimit;
                _eco_Obj.SW_AVG_SpeedDifference = acummulatedSoftWareSpeedDifference / noOfOverSpeedsFromSoftwareLimit;

                //add time penalty weight to avarage speed difference in minutes
                _eco_Obj.SW_AVG_SpeedDifference = TimeSpan.FromSeconds(_eco_Obj.SW_AVG_TimeSpentOverspeeding).TotalMinutes + _eco_Obj.SW_AVG_SpeedDifference;
                _eco_Obj.SWOverspeedScore = Clamp(_eco_Obj.SW_AVG_SpeedDifference / _El_Eco_ScoreConfig.MaxOverspeedDifference);
            }
        
                                



            if (timeSpentOverRoadSpeedLimit > 0)
            {
                _eco_Obj.Road_AVG_TimeSpentOverspeeding = timeSpentOverRoadSpeedLimit / noOfOverSpeedsFromRoadSpeedLimit;
                _eco_Obj.Road_AVG_SpeedDifference = acummulatedRoadSpeedDifference / noOfOverSpeedsFromRoadSpeedLimit;

                //add time penalty weight to avarage speed difference in minutes
                _eco_Obj.Road_AVG_SpeedDifference = TimeSpan.FromSeconds(_eco_Obj.Road_AVG_SpeedDifference).TotalMinutes + _eco_Obj.Road_AVG_SpeedDifference;
                              
                _eco_Obj.RoadOverspeedScore = Clamp(_eco_Obj.Road_AVG_SpeedDifference / _El_Eco_ScoreConfig.MaxOverspeedDifference);
            }



            _eco_Obj.Road_AVG_SpeedDifference = Convert.ToDouble(UserSettings.ConvertKMsToXxOdoMeter(_EL_EcoScoreModel.Unit, _eco_Obj.Road_AVG_SpeedDifference.ToString(), false, 2));

            _eco_Obj.SW_AVG_SpeedDifference = Convert.ToDouble(UserSettings.ConvertKMsToXxOdoMeter(_EL_EcoScoreModel.Unit, _eco_Obj.SW_AVG_SpeedDifference.ToString(), false, 2));


            if (_EL_EcoScoreModel.overspeedType == 824)
                SpeedScore = _eco_Obj.SWOverspeedScore;
            else
                SpeedScore = _eco_Obj.RoadOverspeedScore;

            var scoreArray = new double[] { SpeedScore , harshCornerScore , harshAccScore , harshBrakeScore };

            var score = scoreArray.Max();

            _eco_Obj.Score  = (1- score) *100;

            //`````````````````
            _eco_Obj.Harsh_Acceleration_score = (1 - harshAccScore) * 100 ;

            _eco_Obj.Harsh_Braking_score = (1 - harshBrakeScore) * 100  ;

            _eco_Obj.Harsh_Conering_score = (1 - harshCornerScore) * 100;

            _eco_Obj.RoadOverspeedScore = (1 - harshCornerScore) * 100;

            _eco_Obj.SWOverspeedScore = (1 - harshCornerScore) * 100;

            //`````````````````

            _eco_Obj.NoOfHarshAcc = noOfHarshAcc;

            _eco_Obj.NoOfHarshBrakes = noOfHarshBrakes;

            _eco_Obj.NoOfHarshCorner = noOfHarshCorner;

            _eco_Obj.Overspeed_score = (1 - SpeedScore ) * 100;

            _eco_Obj.SWOverspeedCount = (int)noOfOverSpeedsFromSoftwareLimit;

            _eco_Obj.HWOverspeedCount = (int)noOfOverSpeedsFromHardware;

            _eco_Obj.RoadOverspeedCount = (int)noOfOverSpeedsFromRoadSpeedLimit;

            return _eco_Obj;
        }
        public El_Eco_Drive_Model GetEcoDriveScore(EL_EcoScoreModel _EL_EcoScoreModel,string _IMEIsCSV)
        {
            El_Eco_Drive_Model _El_Eco_Drive_Model = new El_Eco_Drive_Model();

            try
            {
                Stack<El_EcoOverspeedEpisodeItem> hardwarespeedEpisodesStack = new Stack<El_EcoOverspeedEpisodeItem>();

                Stack<El_EcoOverspeedEpisodeItem> softwareEpisodesStack = new Stack<El_EcoOverspeedEpisodeItem>();

                Stack<El_EcoOverspeedEpisodeItem> roadspeedEpisodesStack = new Stack<El_EcoOverspeedEpisodeItem>();



                _El_Eco_Drive_Model.OveralPeriodDistance = GetDistanceTravelledForEcoScore(_EL_EcoScoreModel.imei, _EL_EcoScoreModel.startDate, _EL_EcoScoreModel.endDateTime, _EL_EcoScoreModel.TimeZoneId);



                var softWareSpeedLimit = GetAssetSpeedLimit(_EL_EcoScoreModel.imei);

                var totalPeriodMinutes = _EL_EcoScoreModel.endDateTime .Subtract(_EL_EcoScoreModel.startDate).TotalMinutes;

                 var violation = _EL_EcoScoreModel._Violations;

                var iterations = 0;

                var rowCount = violation.Rows.Count;

                var _isLastRecord = false;

                for (int i = 0; i < rowCount; i++)
                {
                    _isLastRecord = i == (rowCount - 1);

                    var row = violation.Rows[i];

                    if (Convert.ToInt16(row["vReportID"]) == 2) _El_Eco_Drive_Model.NoOfHarshBrakes += 1;

                    if (Convert.ToInt16(row["vReportID"]) == 3) _El_Eco_Drive_Model.NoOfHarshAcc += 1;

                    if (Convert.ToInt16(row["vReportID"]) == 14) _El_Eco_Drive_Model.NoOfHarshCorner += 1;

                   // if (Convert.ToInt16(row["vReportID"]) == 1)  _El_Eco_Drive_Model.HWOverspeedCount += 1;



                    var roadSpeedLimit = -1.0;

                    double.TryParse(row["vRoadSpeed"].ToString(), out roadSpeedLimit);

                    if (_isLastRecord)
                    {

                    }

                    var ss = violation.Rows.OfType<DataRow>().Any(p=> Convert.ToDouble(p["vVehicleSpeed"]) -  Convert.ToDouble(p["vVehicleSpeed"])> 0);

                    if (roadSpeedLimit > -1.0)
                    {

                        InsertEpisodeItemdeOnStack(_isLastRecord, Convert.ToDouble(row["vVehicleSpeed"]), roadSpeedLimit, Convert.ToDateTime(row["dGPSDatetime"]), Convert.ToDouble(row["vOdometer"]), roadspeedEpisodesStack);

                    }
                    if (softWareSpeedLimit > 0 )
                        InsertEpisodeItemdeOnStack(_isLastRecord, Convert.ToDouble(row["vVehicleSpeed"]), softWareSpeedLimit, Convert.ToDateTime(row["dGPSDatetime"]), Convert.ToDouble(row["vOdometer"]), softwareEpisodesStack);

                    if(_EL_EcoScoreModel.HardwareSpeedLimit > -1)
                        InsertEpisodeItemdeOnStack(_isLastRecord, Convert.ToDouble(row["vVehicleSpeed"]), _EL_EcoScoreModel.HardwareSpeedLimit, Convert.ToDateTime(row["dGPSDatetime"]), Convert.ToDouble(row["vOdometer"]), hardwarespeedEpisodesStack);

                    

                iterations += 1;

                }

                var _hardwareEpisodeScoreTotal = 0.0;
                var _hardwareSpeedDifferenceTotal = 0.0;
                var _hardwareEpisodeDistanceTotal = 0.0;


                var _softwareEpisodeScoreTotal = 0.0;
                var _softwareSpeedDifferenceTotal = 0.0;
                var _softwareEpisodeDistanceTotal = 0.0;



                var _roadEpisodeScoreTotal = 0.0;
                var _roadSpeedDifferenceTotal = 0.0;
                var _roadEpisodeDistanceTotal = 0.0;





                //hardareEpisodesStack CALCULATIONS 
                if (hardwarespeedEpisodesStack.Count > 0)
                {
                    var count = hardwarespeedEpisodesStack.Count;

                    hardwarespeedEpisodesStack.ToList().ForEach(obj =>
                    {
                        _hardwareEpisodeScoreTotal += obj.EpisodePenaltyScore;
                        _hardwareSpeedDifferenceTotal += obj.AVGSpeedDifference;
                        _hardwareEpisodeDistanceTotal += Math.Abs(obj.EpisodeDistance);

                        //_El_Eco_Drive_Model.SWOverspeedCount += obj.OverspeedRecordCount;
                        _El_Eco_Drive_Model.Total_HW_TimeSpentOverspeeding += obj.TotalMinutes;
                    });

                    _El_Eco_Drive_Model.HWOverspeedCount = count;

                    _El_Eco_Drive_Model.HW_AVG_SpeedDifference = Math.Round(_hardwareSpeedDifferenceTotal / count, 2);
                    _El_Eco_Drive_Model.HW_AVG_TimeSpentOverspeeding = Math.Round(_El_Eco_Drive_Model.Total_HW_TimeSpentOverspeeding / count, 2) * 60;

                    var _penaltyTotal = Math.Round(_hardwareEpisodeScoreTotal / count, 2);

                    var ratio = (_hardwareEpisodeDistanceTotal / _El_Eco_Drive_Model.OveralPeriodDistance);

                    if (ratio == 0)
                        ratio = _El_Eco_Drive_Model.Total_HW_TimeSpentOverspeeding / totalPeriodMinutes;

                    var _HWOverspeedScore = ((Double.IsNaN(ratio) || Double.IsInfinity(ratio)) ? 1 : ratio) * _penaltyTotal;

                    _El_Eco_Drive_Model.HWOverspeedScore = 100 - _HWOverspeedScore;

                }
                else _El_Eco_Drive_Model.HWOverspeedScore = 100;



                //softwareEpisodesStack CALCULATIONS 
                if (softwareEpisodesStack.Count > 0)
                {
                    var count = softwareEpisodesStack.Count;

                    softwareEpisodesStack.ToList().ForEach(obj =>
                    {
                        _softwareEpisodeScoreTotal += obj.EpisodePenaltyScore;
                        _softwareSpeedDifferenceTotal += obj.AVGSpeedDifference;
                        _softwareEpisodeDistanceTotal += Math.Abs(obj.EpisodeDistance);

                        //_El_Eco_Drive_Model.SWOverspeedCount += obj.OverspeedRecordCount;
                        _El_Eco_Drive_Model.Total_SW_TimeSpentOverspeeding += obj.TotalMinutes;
                    });

                    _El_Eco_Drive_Model.SWOverspeedCount = count;

                    _El_Eco_Drive_Model.SW_AVG_SpeedDifference = Math.Round(_softwareSpeedDifferenceTotal / count, 2);
                    _El_Eco_Drive_Model.SW_AVG_TimeSpentOverspeeding = Math.Round(_El_Eco_Drive_Model.Total_SW_TimeSpentOverspeeding / count, 2) * 60;

                    var _penaltyTotal = Math.Round(_softwareEpisodeScoreTotal / count, 2);

                    var ratio = (_softwareEpisodeDistanceTotal / _El_Eco_Drive_Model.OveralPeriodDistance);

                    if (ratio == 0)
                        ratio = _El_Eco_Drive_Model.Total_SW_TimeSpentOverspeeding / totalPeriodMinutes;

                    var _SWOverspeedScore = ((Double.IsNaN(ratio) || Double.IsInfinity(ratio)) ? 1 : ratio) * _penaltyTotal;

                    _El_Eco_Drive_Model.SWOverspeedScore = 100 - _SWOverspeedScore;

                }
                else _El_Eco_Drive_Model.SWOverspeedScore = 100;



                //softwareEpisodesStack CALCULATIONS 
                if (roadspeedEpisodesStack.Count > 0)
                {

                    var count = roadspeedEpisodesStack.Count;

                    roadspeedEpisodesStack.ToList().ForEach(obj => {
                        _roadEpisodeScoreTotal += obj.EpisodePenaltyScore;
                        _roadSpeedDifferenceTotal += obj.AVGSpeedDifference;
                        _roadEpisodeDistanceTotal += Math.Abs(obj.EpisodeDistance);

                        //_El_Eco_Drive_Model.RoadOverspeedCount += obj.OverspeedRecordCount;
                        _El_Eco_Drive_Model.Total_Road_TimeSpentOverspeeding += obj.TotalMinutes;
                    });

                    _El_Eco_Drive_Model.RoadOverspeedCount = count;

                    _El_Eco_Drive_Model.Road_AVG_SpeedDifference = Math.Round(_roadSpeedDifferenceTotal / count, 2);
                    _El_Eco_Drive_Model.Road_AVG_TimeSpentOverspeeding = Math.Round(_El_Eco_Drive_Model.Total_Road_TimeSpentOverspeeding / count, 2) * 60;


                    var _penaltyTotal = Math.Round(_roadEpisodeScoreTotal / count, 2);

                    var ratio = (_roadEpisodeDistanceTotal / _El_Eco_Drive_Model.OveralPeriodDistance);

                    if (ratio == 0)
                        ratio = _El_Eco_Drive_Model.Total_Road_TimeSpentOverspeeding / totalPeriodMinutes;

                    var _RoadOverspeedScore = ((Double.IsNaN(ratio) || Double.IsInfinity(ratio)) ? 1 : ratio) * _penaltyTotal;

                    _El_Eco_Drive_Model.RoadOverspeedScore = 100 - _RoadOverspeedScore;


                }
                else                
                    _El_Eco_Drive_Model.RoadOverspeedScore = 100;



                
                _El_Eco_Drive_Model.CalculateScoreAvarages();

                _El_Eco_Drive_Model.CalculateOverallScore(_EL_EcoScoreModel.overspeedType);      

                _El_Eco_Drive_Model.OveralPeriodDistance = Math.Round(_El_Eco_Drive_Model.OveralPeriodDistance, 2);

            }
            catch(Exception ex)
            {

            }



            return _El_Eco_Drive_Model;
        }



        public El_EcoOverspeedEpisodeItem InsertEpisodeItemdeOnStack (bool _isLastRecord,double vehicleSpeed ,double speedLimit,DateTime date,double odometer, Stack<El_EcoOverspeedEpisodeItem> StackContainer)
        {
            var _El_EcoOverspeedEpisodeItem = new El_EcoOverspeedEpisodeItem();

            var speedDifference =  vehicleSpeed - speedLimit;

        

            var episodesCount = StackContainer.Count;

            if (speedDifference <= 0 && StackContainer.Count == 0 ||( _isLastRecord && StackContainer.Count == 0))
                return _El_EcoOverspeedEpisodeItem;

            Action CreateSpeedEpisodeItem = () =>
            {
                StackContainer.Push(new El_EcoOverspeedEpisodeItem
                {
                    StartTime = date,
                    StartOdometer = odometer
                });
            };


            if (episodesCount == 0)
                CreateSpeedEpisodeItem();

             _El_EcoOverspeedEpisodeItem = StackContainer.Peek();

            if (speedDifference <= 0  || (_isLastRecord  && speedDifference >= 0))
            {
                if (!_El_EcoOverspeedEpisodeItem.IsEpisodeComplete)
                {
                    _El_EcoOverspeedEpisodeItem.IsEpisodeComplete = true;

                    _El_EcoOverspeedEpisodeItem.EndTime = date;
                    _El_EcoOverspeedEpisodeItem.EndOdometer = odometer;

                    _El_EcoOverspeedEpisodeItem.CalculateScore();
                }
            }
            else
            {
                if (_El_EcoOverspeedEpisodeItem.IsEpisodeComplete)      
                    CreateSpeedEpisodeItem();                   
                
                else
                {
                    _El_EcoOverspeedEpisodeItem.TotalSpeedDifference += speedDifference;
                    _El_EcoOverspeedEpisodeItem.OverspeedRecordCount += 1;                


                }


            }

            return _El_EcoOverspeedEpisodeItem;
        }
       
        
        
        
        public DataSet GetAllTripScoreViolationsForTimeRange(String imeiCSV, DateTime tripStartDateTimeLocal, DateTime tripEndDateTimeLocal, string timeZoneId)
        {
            DataSet allViolations = new DataSet();

            DAL_EcoDriveData objDal_EcoDrive = new DAL_EcoDriveData();

            DateTime todaysDate = System.DateTime.UtcNow;

            try
            {

                string _strStartDateRange = "";
                string _strEndDateRange = "";

                if (tripStartDateTimeLocal.ToString() == "1/1/0001 12:00:00 AM")
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate.AddHours(24).AddSeconds(-1), timeZoneId);
                }
                else
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripStartDateTimeLocal, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripEndDateTimeLocal, timeZoneId);
                }

                allViolations = objDal_EcoDrive.GetViolationsForEcoScore(imeiCSV, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange));
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("EcoDrive.cs", "GetAllTripScoreViolationsForTimeRange_SingleAsset()", ex.Message  + ex.StackTrace);
            }

            return allViolations;

        }

        public DataSet GetAllTripScoreViolationsForTimeRange(String imeiCSV, DateTime tripStartDateTimeLocal, DateTime tripEndDateTimeLocal, string timeZoneId, int operation)
        {
            DataSet allViolations = new DataSet();

            DAL_EcoDriveData objDal_EcoDrive = new DAL_EcoDriveData();

            DateTime todaysDate = System.DateTime.UtcNow;

            try
            {

                string _strStartDateRange = "";
                string _strEndDateRange = "";

                if (tripStartDateTimeLocal.ToString() == "1/1/0001 12:00:00 AM")
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate.AddHours(24).AddSeconds(-1), timeZoneId);
                }
                else
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripStartDateTimeLocal, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripEndDateTimeLocal, timeZoneId);
                }

                allViolations = objDal_EcoDrive.GetViolationsForEcoScore(imeiCSV, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange),operation);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("EcoDrive.cs", "GetAllTripScoreViolationsForTimeRange_SingleAsset()", ex.Message + ex.StackTrace);
            }

            return allViolations;

        }


        public double GetDistanceTravelledForEcoScore(long deviceDbId, DateTime tripStartDateTimeLocal, DateTime tripEndDateTimeLocal, string timeZoneId)
        {
            //DataSet minMaxOdo = new DataSet();
            double distance = 0;
            DAL_EcoDriveData objDal_EcoDrive = new DAL_EcoDriveData();
            DateTime todaysDate = System.DateTime.UtcNow;

            try
            {

                string _strStartDateRange = "";
                string _strEndDateRange = "";

                if (tripStartDateTimeLocal.ToString() == "1/1/0001 12:00:00 AM")
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate.AddHours(24).AddSeconds(-1), timeZoneId);
                }
                else
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripStartDateTimeLocal, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripEndDateTimeLocal, timeZoneId);
                }

                distance = Convert.ToDouble(objDal_EcoDrive.GetMinMaxOdoForEcoScore(deviceDbId, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange)));
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "EcoDrive.cs", "minMaxOdo()", ex.Message  + ex.StackTrace);

            }

            return distance;

        }

        public double GetAssetSpeedLimit(long imei )
        {
            var speedLimit = 0.0;
            try
            {

                DAL_EcoDriveData objDal_EcoDrive = new DAL_EcoDriveData();
                speedLimit = Convert.ToDouble(objDal_EcoDrive.GetSoftWareSpeedLimit(imei));
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("EcoDrive.cs", "GetAssetSpeedLimit()", ex.Message + ex.StackTrace);

            }
            return speedLimit;
        }


        public El_Eco_ScoreConfig GetAssetEcoConfig(int AssetId)
        {
            El_Eco_ScoreConfig El_Eco_ScoreConfig = new El_Eco_ScoreConfig();
            try
            {

                DAL_EcoDriveData objDal_EcoDrive = new DAL_EcoDriveData();

                var config = objDal_EcoDrive.GetEcoScoreConfig(AssetId);

                if (config is { })
                    El_Eco_ScoreConfig = new El_Eco_ScoreConfig {
                          maxNoOfHarshAccIn24Hours = Convert.ToInt32(config["max_no_ofharsh_acc_24hours"]  == DBNull.Value? 0: config["max_no_ofharsh_acc_24hours"]),
                          maxNoOfHarshBrakesIn24Hours = Convert.ToInt32(config["max_no_ofharsh_brakes_24hours"] == DBNull.Value ? 0 : config["max_no_ofharsh_brakes_24hours"]),
                          MaxNoOfHarshCornerIn24Hours = Convert.ToInt32(config["max_no_ofharsh_cornering_24hours"] == DBNull.Value ? 0 : config["max_no_ofharsh_cornering_24hours"]),
                          MaxOverspeedDifference = Convert.ToDouble(config["max_overspeed_difference"] == DBNull.Value ? 0 : config["max_overspeed_difference"])
                    };

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("EcoDrive.cs", "GetAssetEcoConfig()", ex.Message + ex.StackTrace);

            }
            return El_Eco_ScoreConfig;
        }

    }
}
