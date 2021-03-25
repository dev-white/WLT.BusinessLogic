using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WLT.BusinessLogic.BAL;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.Bal_Reports
{
   public class Bal_MultiAssetFuelReport
    {
        public FuelModel ReportHeaderDetails { get; set; } = new FuelModel();

        public List<EL_FuelAssetItem> GetMultipleAssetsForFuelWithMappings(El_Report _El_Report)
        {
            _El_Report.Operation = 1;

            var ds =  DAL_Reports.GetMultipleAssetsForFuelWithMappings(_El_Report);

            var devicesDetails = new List<EL_FuelAssetItem>();

            var tbl_index = 0;

            var index = 0;

            Dictionary<int, int> dict_RowIndex = new Dictionary<int, int>();
 
            foreach ( DataTable dt in ds.Tables)
            {
                // common header for the  report
                if (tbl_index == 0)
                    foreach (DataRow dr in dt.Rows)                    
                        ReportHeaderDetails = new FuelModel
                        {
                            Asset = Convert.ToString(dr["Asset"]),
                            startDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["startDate"]), _El_Report.TimeZoneID).ToString("dd MMM yyyy   HH:mm:ss "),
                            endDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["endDate"]), _El_Report.TimeZoneID).ToString("dd MMM yyyy   HH:mm:ss "),
                            DateOfQuery = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, _El_Report.TimeZoneID).ToString("dd MMM yyyy HH:mm:ss"),
                            Logo = dr["vLogo"].ToString(),
                          
                        };
                           
                    
                
                // asset mappings and extra details 
                if (tbl_index==1)
                foreach( DataRow dr in dt.Rows)
                {
                    var _EL_FuelAssetItem = new EL_FuelAssetItem();


                        var SensorType = Convert.ToInt32(dr["SensorType"]);

                        var ifkCanBusType = Convert.ToInt32(dr["ifkCanBusType"]);

                        var iAnalogNumber = Convert.ToInt32(dr["iAnalogNumber"]);

                        var type = SensorType == 7 ? ifkCanBusType : iAnalogNumber;

                        var _BroughtForword = Bal_FuelUtils.GetFuelValue(Convert.ToString(dr["brought_forward"]), SensorType, type);
                      
                        var _CarriedForward = Bal_FuelUtils.GetFuelValue(Convert.ToString(dr["carried_forward"]), SensorType, type);

                      

                        if (dict_RowIndex.ContainsKey(Convert.ToInt32(dr["asset_id"])))                        
                            index = dict_RowIndex[Convert.ToInt32(dr["asset_id"])];
                        
                        else
                        {
                            index += 1;

                            dict_RowIndex.Add(Convert.ToInt32(dr["asset_id"]), index);

                        }


                    _EL_FuelAssetItem.FuelModelItem = new FuelModel
                    {
                        Asset = Convert.ToString(dr["asset_name"]),                      
                        ifkDeviceId = Convert.ToInt32(dr["asset_id"]),
                        RowIndex = index,
                        SensorType = SensorType,
                        iAnalogNumber = iAnalogNumber,
                        ifkCanBusType = ifkCanBusType,
                        Tracker_Type = Convert.ToInt32(dr["tracker_type"]),
                        Title = Convert.ToString(dr["vname"]),
                        iMappingId = Convert.ToInt32(dr["Id"]),                        
                        analogType = Convert.ToString(dr["vUnitText"]),
                        BroughtForword = double.TryParse(Convert.ToString(_BroughtForword), out _) ? Convert.ToDouble(_BroughtForword) : 0,
                        CarriedForward  =  double.TryParse( Convert.ToString(_CarriedForward),out _)? Convert.ToDouble(_CarriedForward):0,
                        EngineSize = Convert.ToString(dr["Asset_Engine_Size"]),
                        FuelCapacity = Convert.ToString(dr["Tank_Capacity"])
                    };

                        _EL_FuelAssetItem.FuelItem = new EL_Fuel
                        {
                            ifkAssetID = _EL_FuelAssetItem.FuelModelItem.ifkDeviceId,
                            sensorType = _EL_FuelAssetItem.FuelModelItem.SensorType,
                            iAnalogNumber = _EL_FuelAssetItem.FuelModelItem.iAnalogNumber,
                            canBusType = _EL_FuelAssetItem.FuelModelItem.ifkCanBusType,
                            Operation = 3,
                            startDate = ReportHeaderDetails.startDate,
                            endDate = ReportHeaderDetails.endDate,
                            Tracker_Type = _EL_FuelAssetItem.FuelModelItem.Tracker_Type,
                                   


                    };

                    _EL_FuelAssetItem.DeviceDetailsMaster = new El_Device
                    {
                        deviceName = Convert.ToString(dr["asset_name"]),
                        ifkAssetId = Convert.ToInt32(dr["asset_id"]),
                        TrackerType = Convert.ToInt32(dr["tracker_type"]),
                        Model = Convert.ToString(dr["Asset_Model"]),
                        Make = Convert.ToString(dr["Asset_Make"]),
                        CompanyId= Convert.ToInt32(dr["clientid"]),
                    };
                        
                    devicesDetails.Add(_EL_FuelAssetItem);
                }


                 tbl_index +=1;
            }

            return devicesDetails;
        }

        public List<FuelModel> GetFuelDataForMultipleAssets(El_Report _El_Report)
        {
           _El_Report.Operation = 3;

            var _bal_fuel = new Bal_Fuel();

            var _device_details = GetMultipleAssetsForFuelWithMappings(_El_Report);

            var lst_assets_processed_fuel_data = new List<FuelModel>();

            foreach (var asset_and_mappings  in _device_details)
            {
              var processed_asset_data =  _bal_fuel.FuelInfo(_El_Report, asset_and_mappings);

                lst_assets_processed_fuel_data.Add(processed_asset_data);
            }
                        
            return lst_assets_processed_fuel_data;
        }
                          
    }
}
