using System;
using System.Data;
using System.Text;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
   
        public class Bal_Maintenance
        {
        public El_Maintenance _El_Maintenance;            

        public void AssetSaveMaintanance()
        {
            var _dal_Maintenance = new DAL_Maintenance();                         

            var _Schedular = new MaintenanceHangfireSchedular();

            var iCommandType = Convert.ToInt16(this._El_Maintenance.Command_type);

            if (iCommandType == 2)
            {
               
                var _utcScheduledDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(Convert.ToDateTime(this._El_Maintenance.Command_value), this._El_Maintenance.Timezone);

                this._El_Maintenance.Command_value = _utcScheduledDate.ToString("yyyy-MM-dd HH:mm:ss");

                var id = (int)_dal_Maintenance.AssetSaveMaintanance(this._El_Maintenance);

                var TimeDifference = _utcScheduledDate.Subtract(DateTime.UtcNow).Duration();               
               
                _Schedular.ScheduleMaintenanceBydate(id, TimeDifference);
            }
            else
            {

                var id = (int)_dal_Maintenance.AssetSaveMaintanance(this._El_Maintenance);

                if (iCommandType == 1)
                    _Schedular.ScheduleMaintenanceByodometer(4, this._El_Maintenance.Timezone);
                else
                    _Schedular.ScheduleMaintenanceByEngineHours(5, this._El_Maintenance.Timezone);

            }
        }

        public string GetMaintenanceItems(El_Maintenance _El_Maintenance, wltAppState _objRegisteration)
        {

            var _UIParam = new El_MaintenanceUIConfig();


            clsClientDevice MaintananceClass = new clsClientDevice();

            var ds = MaintananceClass.GetMaintananceCommand(_El_Maintenance);

            var dueStringBuilder = new StringBuilder();

            var ActiveStringBuilder = new StringBuilder();

            var MarkedAsCompleteStringBuilder = new StringBuilder();


            foreach (DataTable _dt in ds.Tables)
                foreach (DataRow _dr in _dt.Rows)
                {

                    var sb = "";

                    var _El_MaintenanceUIConfig = new El_MaintenanceUIConfig();

                    var _El_MaintenanceItem = new El_Maintenance {
                        ID = Convert.ToInt64(_dr["id"]),
                        Command_name = Convert.ToString(_dr["MaintenanceItemName"]),
                        Command_type = Convert.ToString(_dr["ParameterToCheck"]),
                        Command_value = Convert.ToString(_dr["ParameterValue"]),
                        Command_Email = Convert.ToString(_dr["NotifyEmailAddress"]),
                        IsActive = Convert.ToBoolean(_dr["IsActive"] ==DBNull.Value?0: _dr["IsActive"]),
                        IsMarkedAsCompleted = Convert.ToBoolean(_dr["IsMarkedAsCompleted"] == DBNull.Value ? 0 : _dr["IsMarkedAsCompleted"]),

                    };

                    // odometer 
                    if(_El_MaintenanceItem .Command_type== "1")
                    {

                        _El_MaintenanceUIConfig.parameter_command = "Odometer";

                        _El_MaintenanceUIConfig.input_string = "type ='number'";

                        _El_MaintenanceUIConfig.attribute = _El_MaintenanceUIConfig.variable_class = "";

                        var currentVla = Convert.ToDouble(_dr["current_Odometer"]);

                        var diff = (Convert.ToDouble(_El_MaintenanceItem.Command_value) - currentVla);

                        _El_MaintenanceUIConfig.RemainingValue = diff  + " Kms";

                        _El_MaintenanceItem.IsDue = diff < 0;
                      
                        _El_MaintenanceItem.Current_Value = currentVla.ToString();


                        sb = MaintenanceItem(_El_MaintenanceItem, _El_MaintenanceUIConfig);                      
                    }


                    // date  parameter
                    if (_El_MaintenanceItem.Command_type == "2")
                    {

                        var retrievedDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_El_MaintenanceItem.Command_value), _objRegisteration.vTimeZoneID);

                        _El_MaintenanceUIConfig.parameter_command = "Date";

                        _El_MaintenanceUIConfig.variable_class = "update_datepicker";
                    

                        _El_MaintenanceUIConfig.attribute = "attr_lcl";

                        var currntValue = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, _objRegisteration.vTimeZoneID);

                        var TotalRemainingSecs = Convert.ToDateTime(retrievedDate).Subtract(currntValue).TotalSeconds;

                        _El_MaintenanceUIConfig.Selected_Value = String.Format("{0:dd, MMMM, yyyy}", retrievedDate);

                        _El_MaintenanceItem.IsDue = TotalRemainingSecs < 0;

                        if (_El_MaintenanceItem.IsDue)
                        {
                            _El_MaintenanceUIConfig.RemainingValue = TimeSpanCalculator(TotalRemainingSecs);
                        }
                        else
                        {
                            _El_MaintenanceUIConfig.RemainingValue = TimeSpanCalculator(TotalRemainingSecs);
                        }

                        _El_MaintenanceItem.Current_Value = currntValue.ToString("dd MMM yyyy HH:mm:ss");

                        _El_MaintenanceItem.Command_value = retrievedDate.ToString("dd MMM yyyy HH:mm:ss");

                        sb = MaintenanceItem(_El_MaintenanceItem, _El_MaintenanceUIConfig);

                      
                    }

                    // engine hours 
                    if (_El_MaintenanceItem.Command_type == "3")
                    {

                        _El_MaintenanceUIConfig. parameter_command = "Engine Hours";

                        _El_MaintenanceUIConfig.input_string = "type ='number'";

                        _El_MaintenanceUIConfig. attribute = _El_MaintenanceUIConfig.variable_class = "";

                        var _Hours = Math.Round(TimeSpan.FromTicks(Convert.ToInt64(_El_MaintenanceItem.Command_value)).TotalHours, 2);

                        _El_MaintenanceItem.IsDue = _Hours < 0;

                        _El_MaintenanceItem.Current_Value = Math.Round(TimeSpan.FromTicks(Convert.ToInt64(Convert.ToInt64(_dr["Current_EngineHours"]).ToString())).TotalHours, 2).ToString(); 

                        _El_MaintenanceUIConfig. RemainingValue = Math.Round(_Hours - TimeSpan.FromTicks(Convert.ToInt64(_dr["Current_EngineHours"])).TotalHours, 2) + " Hours";

                        _El_MaintenanceUIConfig.Selected_Value = _Hours.ToString();

                        _El_MaintenanceItem.Command_value = _Hours.ToString();

                        sb = MaintenanceItem(_El_MaintenanceItem,  _El_MaintenanceUIConfig);

                        
                    }

                  

                    if (!_UIParam.HasMarkedAsCompleted && _El_MaintenanceItem.IsMarkedAsCompleted)
                    {
                        _UIParam.HasMarkedAsCompleted = _El_MaintenanceItem.IsMarkedAsCompleted;
                        MarkedAsCompleteStringBuilder.Append(GetMaintenanceHeader("History"));
                    }
                    else
                    {
                        if (!_UIParam.HasActive && !_El_MaintenanceItem.IsDue)
                        {
                            _UIParam.HasActive = !_El_MaintenanceItem.IsDue;
                            ActiveStringBuilder.Append(GetMaintenanceHeader("Active "));
                        }

                        if (!_UIParam.HasDue && _El_MaintenanceItem.IsDue)
                        {
                            _UIParam.HasDue = _El_MaintenanceItem.IsDue;
                            dueStringBuilder.Append(GetMaintenanceHeader("Due"));
                        }

                    }


                    if (_El_MaintenanceItem.IsMarkedAsCompleted)    
                        MarkedAsCompleteStringBuilder.Append(sb);
                    else
                    {
                        if (_El_MaintenanceItem.IsDue)
                            dueStringBuilder.Append(sb);

                        else if (!_El_MaintenanceItem.IsDue)
                            ActiveStringBuilder.Append(sb);

                    }
                        
                }

               

            var result = $"{dueStringBuilder.ToString()} {ActiveStringBuilder.ToString()} {MarkedAsCompleteStringBuilder.ToString()} ";

            return result;

        }


       public string MaintenanceItem( El_Maintenance _item , El_MaintenanceUIConfig _El_MaintenanceUIConfig)
        {

            StringBuilder sb = new StringBuilder();



            if (_item.IsMarkedAsCompleted)
            {
                //active part part
                sb.Append("<div class='col-md-4 rightmetronicpad  IsMarkedAsCompleted'>");
                sb.Append("<div class='portlet light bordered'>");
                sb.Append("<div class='portlet-title'>");
                sb.Append("<div class='caption'>");
                sb.Append("<span class='admin_Title adminVisual Settings_Title caption-subject uppercase' style='padding-top:0;'>" + _item.Command_name + "</span></div>");
                //sb.Append("<div class='actions'>");
                //sb.Append("<input id='btnSaveMainatananceCommand_" + _item.ID + "' type='button' value='Save' class='admin_SaveIcon' title='Save' onclick='SaveMaintananceUpdate(" + _item.ID + ",1);' style='display: none;' />");
                //sb.Append("<a  id='EditAssetMaintanaceCommands" + _item.ID + "' onclick='EditAssetMaintanaceCommands(" + _item.ID + ");' title='Edit' Custom_attribute = '" + _El_MaintenanceUIConfig.attribute + "' class='Edit_class EditAssetMaintanaceCommandsClass admin_EditIcon' style='margin-top:1px;'>Edit</a>");
                //sb.Append("<a style='display: none' id='aCancelMaintananceCommandRow_" + _item.ID + "' onclick='Cancel_maintanannceEdit(" + _item.ID + ");' title='Cancel' class=' admin_CancelEditIcon' >Cancel</a>");
                //sb.Append("<a id='adeleteMaintananceCommand" + _item.ID + "' onclick='deleteMaintananceCommand(" + _item.ID + ");' title='Delete' class='admin_DeleteIcon' style='margin-top:4px;'>Edit</a>");
                //sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("<div class='portlet-body'>");
                sb.Append("<table class='table' id='MaintDetailTable_" + _item.ID + "'>");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style='border:none;'><b data-localize='ai_Name'>Name</b></td>");
                sb.Append("<td style='border:none;'><span id='MaintenanceItemName_" + _item.ID + "'>" + _item.Command_name + "</span></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td><b data-localize='ai_Parameter'>Parameter</b></td>");
                sb.Append("<td><span id='AssetMaintanancetype" + _item.ID + "'>" + _El_MaintenanceUIConfig.parameter_command + "</span></td>");
                sb.Append("</tr>");

                if (_item.Command_type == "2")
                    _item.Command_value = Convert.ToDateTime(_item.Command_value).ToString("dd MMM yyyy HH:mm:ss");




                sb.Append("<tr>");
                sb.Append("<td><b data-localize='ai_ParameterValue'>Parameter Value</b></td>");
                sb.Append("<td><span id='MaintanaceParameterValue" + _item.ID + "'>" + _item.Command_value + "</span></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td><b data-localize='ai_Email'>Email</b></td>");
                sb.Append("<td><span id='MaintananceNotifyEmailAddress" + _item.ID + "'>" + _item.Command_Email + "</span></td>");
                sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td><b data-localize='ai_Remaining'>Remaining </b></td>");
                //sb.Append($"<td><span> { _item.RemainingValue } </span></td>");
                //sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");

                sb.Append("<div id='MaintDetailFields_" + _item.ID + "' style='display:none;'>");
                sb.Append("<div class='form-group form-md-line-input form-md-floating-label'>");
                sb.Append("<input type='text'  id='txtMaintannceCommandText" + _item.ID + "'  class='form-control' style='display:none; '/>");
                sb.Append("<label>Name</label>");
                sb.Append("<span class='help-block'>Maintenance Item Name goes here...</span>");
                sb.Append("</div>");
                sb.Append("<div class='form-group form-md-line-input'>");
                sb.Append("<input " + _El_MaintenanceUIConfig.input_string + "   id='txtMaintannceCommandvalue" + _item.ID + "' lclatrr = '" + _El_MaintenanceUIConfig.attribute + "'  class='form-control " + _El_MaintenanceUIConfig.variable_class + "' />");
                sb.Append("<label>Parameter Value</label>");
                sb.Append("<span class='help-block'>Parameter Value goes here...</span>");
                sb.Append("</div>");
                sb.Append("<div class='form-group form-md-line-input form-md-floating-label'>");
                sb.Append("<input type='text'    id='txtMaintanancenotify_email" + _item.ID + "'  class='form-control' style='display:none; '/>");
                sb.Append("<label>Email</label>");
                sb.Append("<span class='help-block'>Put Email here to get updates...</span>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");

            }
            else
            {

                if (!_item.IsDue)
                {
                    //active part part
                    sb.Append("<div class='col-md-4 rightmetronicpad'>");
                    sb.Append("<div class='portlet light bordered'>");
                    sb.Append("<div class='portlet-title'>");
                    sb.Append("<div class='caption'>");
                    sb.Append("<span class='admin_Title adminVisual Settings_Title caption-subject uppercase' style='padding-top:0;'>" + _item.Command_name + "</span></div>");
                    sb.Append("<div class='actions'>");
                    sb.Append("<input id='btnSaveMainatananceCommand_" + _item.ID + "' type='button' value='Save' class='admin_SaveIcon' title='Save' onclick='SaveMaintananceUpdate(" + _item.ID + ",1);' style='display: none;' />");
                    sb.Append("<a  id='EditAssetMaintanaceCommands" + _item.ID + "' onclick='EditAssetMaintanaceCommands(" + _item.ID + ");' title='Edit' Custom_attribute = '" + _El_MaintenanceUIConfig.attribute + "' class='Edit_class EditAssetMaintanaceCommandsClass admin_EditIcon' style='margin-top:1px;'>Edit</a>");
                    sb.Append("<a style='display: none' id='aCancelMaintananceCommandRow_" + _item.ID + "' onclick='Cancel_maintanannceEdit(" + _item.ID + ");' title='Cancel' class=' admin_CancelEditIcon' >Cancel</a>");
                    sb.Append("<a id='adeleteMaintananceCommand" + _item.ID + "' onclick='deleteMaintananceCommand(" + _item.ID + ");' title='Delete' class='admin_DeleteIcon' style='margin-top:4px;'>Edit</a>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div class='portlet-body'>");
                    sb.Append("<table class='table' id='MaintDetailTable_" + _item.ID + "'>");
                    sb.Append("<tbody>");
                    sb.Append("<tr>");
                    sb.Append("<td style='border:none;'><b data-localize='ai_Name'>Name</b></td>");
                    sb.Append("<td style='border:none;'><span id='MaintenanceItemName_" + _item.ID + "'>" + _item.Command_name + "</span></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td><b data-localize='ai_Parameter'>Parameter</b></td>");
                    sb.Append("<td><span id='AssetMaintanancetype" + _item.ID + "'>" + _El_MaintenanceUIConfig.parameter_command + "</span></td>");
                    sb.Append("</tr>");


                    if (_item.Command_type == "2")
                        _item.Command_value = Convert.ToDateTime(_item.Command_value).ToString("dd MMM yyyy HH:mm:ss");

                    sb.Append("<tr>");
                    sb.Append("<td><b data-localize='ai_ParameterValue'>Parameter Value</b></td>");
                    sb.Append("<td><span id='MaintanaceParameterValue" + _item.ID + "'>" + _item.Command_value + "</span></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td><b data-localize='ai_ParameterValue'>Current Value</b></td>");
                    sb.Append("<td><span id='MaintanaceParameterValue" + _item.ID + "'>" + _item.Current_Value + "</span></td>");
                    sb.Append("</tr>");





                    sb.Append("<tr>");
                    sb.Append("<td><b data-localize='ai_Email'>Email</b></td>");
                    sb.Append("<td><span id='MaintananceNotifyEmailAddress" + _item.ID + "'>" + _item.Command_Email + "</span></td>");
                    sb.Append("</tr>");
                    sb.Append("<td><b data-localize='ai_Remaining'>Remaining </b></td>");
                    sb.Append($"<td><span> { _El_MaintenanceUIConfig.RemainingValue } </span></td>");
                    sb.Append("</tr>");
                    sb.Append("</tbody>");
                    sb.Append("</table>");

                    sb.Append("<div id='MaintDetailFields_" + _item.ID + "' style='display:none;'>");
                    sb.Append("<div class='form-group form-md-line-input form-md-floating-label'>");
                    sb.Append("<input type='text'  id='txtMaintannceCommandText" + _item.ID + "'  class='form-control' style='display:none; '/>");
                    sb.Append("<label>Name</label>");
                    sb.Append("<span class='help-block'>Maintenance Item Name goes here...</span>");
                    sb.Append("</div>");
                    sb.Append("<div class='form-group form-md-line-input'>");
                    sb.Append("<input " + _El_MaintenanceUIConfig.input_string + "   id='txtMaintannceCommandvalue" + _item.ID + "' lclatrr = '" + _El_MaintenanceUIConfig.attribute + "'  class='form-control " + _El_MaintenanceUIConfig.variable_class + "' />");
                    sb.Append("<label>Parameter Value</label>");
                    sb.Append("<span class='help-block'>Parameter Value goes here...</span>");
                    sb.Append("</div>");
                    sb.Append("<div class='form-group form-md-line-input form-md-floating-label'>");
                    sb.Append("<input type='text'    id='txtMaintanancenotify_email" + _item.ID + "'  class='form-control' style='display:none; '/>");
                    sb.Append("<label>Email</label>");
                    sb.Append("<span class='help-block'>Put Email here to get updates...</span>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                }

                if (_item.IsDue)
                {
                    //due part
                    sb.Append("<div class='col-md-6 rightmetronicpad'>");
                    sb.Append("<div class='portlet light bordered'>");
                    sb.Append("<div class='portlet-title'>");
                    sb.Append("<div class='caption'>");
                    sb.Append("<span class='admin_Title adminVisual Settings_Title caption-subject uppercase' style='padding-top:0;'>" + _item.Command_name + "</span></div>");
                    sb.Append("</div>");
                    sb.Append("<div class='portlet-body'>");
                    sb.Append("<table class='table'>");
                    sb.Append("<tbody>");
                    sb.Append("<tr id='MaintenanceItemName_" + _item.ID + "'>");
                    sb.Append("<td style='border:none;'><b>Name</b></td>");
                    sb.Append("<td style='border:none;'><span>" + _item.Command_name + "</span></td>");
                    sb.Append("</tr>");


                    sb.Append("<tr id='MaintananceNotifyEmailAddress" + _item.ID + "'>");
                    sb.Append("<td><b>Email</b></td>");
                    sb.Append("<td><span>" + _item.Command_Email + "</span></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr id='AssetMaintanancetype" + _item.ID + "'>");
                    sb.Append("<td><b>Parameter</b></td>");
                    sb.Append("<td><span>" + _El_MaintenanceUIConfig.parameter_command + "</span></td>");
                    sb.Append("</tr>");
                

                    sb.Append("<tr>");
                    sb.Append("<td><b data-localize='ai_ParameterValue'>Current </b></td>");
                    sb.Append("<td><span id='MaintanaceParameterValue" + _item.ID + "'>" + _item.Current_Value + "</span></td>");
                    sb.Append("</tr>");


                    if (_item.Command_type == "2")
                        _item.Command_value = Convert.ToDateTime(_item.Command_value).ToString("dd MMM yyyy HH:mm:ss");


                    sb.Append("<tr id='MaintanaceParameterValue" + _item.ID + "'>");
                    sb.Append("<td><b>Parameter Value</b></td>");
                    sb.Append("<td><span>" + _item.Command_value + "</span></td>");
                    sb.Append("</tr>");


                    sb.Append("</tr>");
                    sb.Append("<tr id='d" + _item.ID + "'>");
                    sb.Append("<td><b>Overdue by</b></td>");
                    sb.Append("<td><span> " + _El_MaintenanceUIConfig.RemainingValue + " </span></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td><input id='setIsActive" + _item.ID + "' type='button' value='Mark as completed ' class='button buttonSmall' title='Save' onclick='SaveMaintananceUpdate(" + _item.ID + ",2);' style='margin-top: 10px' /></td>");
                    sb.Append("<td>&nbsp;</td>");
                    sb.Append("</tr>");

                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                }

            }


            return sb.ToString();

        }

        public string GetMaintenanceItems_deleteme(El_Maintenance _El_Maintenance, wltAppState _objRegisteration)
        {
            StringBuilder sb = new StringBuilder();

            DataSet ds = new DataSet();

            clsClientDevice MaintananceClass = new clsClientDevice();

            ds = MaintananceClass.GetMaintananceCommand(_El_Maintenance);



            var dueStringBuilder = new StringBuilder();

            var ActiveStringBuilder = new StringBuilder();

            var MarkedAsCompleteStringBuilder = new StringBuilder();


            


            int NumOfTables = 0;

            foreach (DataTable _tbl in ds.Tables)
            {
                sb.Append(MaintenanceRecord(ds, NumOfTables, _objRegisteration));

                NumOfTables++;

            }
            return sb.ToString();

        }
        private string GetMaintenanceHeader(string headerText)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class='set_6_in' style = 'border-bottom-style:solid;border-width:thin;border-color:#dadada;padding-bottom:5px;margin-bottom:15px;'>");
            builder.Append("<span style='width:80%'>");
            builder.Append("<strong>" + headerText + " </strong> </span>");
            builder.Append("</span>");

            builder.Append("<span class='admin_tableDiv admin_TitleTop' style='float:right;'>");

            builder.Append("</span>");
            builder.Append("</div>");

            return builder.ToString();

        }

        public string MaintenanceRecord(DataSet ds, int operation, wltAppState  objRegisteration)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                int counter = 0;

                operation++;

                if (operation == 1)
                {

                    sb.Append(GetMaintenanceHeader("Due"));
                }
                else if (operation == 2)
                {
                    sb.Append(GetMaintenanceHeader("Active"));
                }
                else if (operation == 3)
                {
                    sb.Append(GetMaintenanceHeader("History"));
                }

                foreach (DataRow _dr in ds.Tables[operation - 1].Rows)
                {
                    counter++;
                    bool status = false;
                    bool IsMarkedAsCompleted = false;
                    if (operation == 1)
                    {
                        IsMarkedAsCompleted = Convert.ToBoolean(_dr["IsMarkedAsCompleted"].ToString());
                    }
                    else if (operation == 2)
                    {
                        status = Convert.ToBoolean(_dr["IsActive"].ToString());
                    }
                    else if (operation == 3)
                    {
                        IsMarkedAsCompleted = true;
                    }
                    string Selected_Value = _dr["ParameterValue"].ToString();

                    string variable_class = null;

                    string attribute = null;

                    string input_string = null;

                    string parameter_command = null;

                    object RemainingValue = null;

                    if (int.Parse(_dr["ParameterToCheck"].ToString()) == 1)
                    {

                        parameter_command = "Odometer";

                        input_string = "type ='number'";

                        attribute = variable_class = "";

                        RemainingValue = (Convert.ToDouble(Selected_Value) - Convert.ToDouble(_dr["current_Odometer"])) + " Km/s";

                    }
                    else if (int.Parse(_dr["ParameterToCheck"].ToString()) == 2)
                    {      
                        var retrievedDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(Selected_Value), objRegisteration.vTimeZoneID);

                        parameter_command = "Date";

                        variable_class = "update_datepicker";

                        Selected_Value = String.Format("{0:dd, MMMM, yyyy}", retrievedDate);

                        attribute = "attr_lcl";

                        var TotalRemainingSecs = Convert.ToDateTime(Selected_Value).Subtract(UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, objRegisteration.vTimeZoneID)).TotalSeconds;

                       

                        RemainingValue = TimeSpanCalculator(TotalRemainingSecs);

                    }
                    else if (int.Parse(_dr["ParameterToCheck"].ToString()) == 3)
                    {

                        parameter_command = "Engine Hours";

                        input_string = "type ='number'";

                        attribute = variable_class = "";

                        var _Hours = Math.Round(TimeSpan.FromTicks(Convert.ToInt64(Selected_Value)).TotalHours, 2);

                        RemainingValue = Math.Round(_Hours - TimeSpan.FromTicks(Convert.ToInt64(_dr["Current_EngineHours"])).TotalHours, 2) + " Hours";

                        Selected_Value = _Hours.ToString();

                    }

                    if (operation == 3)
                    {

                        DateTime date = DateTime.Parse(_dr["DateMarkedAsCompleted"].ToString());

                        var DateMarked = String.Format("{0:dd, MMMM, yyyy}", date);

                        //history part
                        sb.Append("<div class='col-md-4 rightmetronicpad'>");
                        sb.Append("<div class='portlet light bordered'>");
                        sb.Append("<div class='portlet-title'>");
                        sb.Append("<div class='caption'>");
                        sb.Append("<span class='admin_Title adminVisual Settings_Title caption-subject uppercase' style='padding-top:0;'>" + _dr["MaintenanceItemName"].ToString() + "</span></div>");
                        sb.Append("</div>");
                        sb.Append("<div class='portlet-body'>");
                        sb.Append("<table class='table'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr id='MaintenanceItemName_" + Convert.ToString(_dr["Id"]) + "'>");
                        sb.Append("<td style='border:none;'><b data-localize='ai_Name'>Name</b></td>");
                        sb.Append("<td style='border:none;'><span>" + _dr["MaintenanceItemName"].ToString() + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr id='AssetMaintanancetype" + Convert.ToString(_dr["Id"]) + "'>");
                        sb.Append("<td><b data-localize='ai_Parameter'>Parameter</b></td>");
                        sb.Append("<td><span>" + parameter_command + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr id='MaintanaceParameterValue" + Convert.ToString(_dr["Id"]) + "'>");
                        sb.Append("<td><b data-localize='ai_ParameterValue'>Parameter Value</b></td>");
                        sb.Append("<td><span>" + Selected_Value + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr id='MaintananceNotifyEmailAddress" + Convert.ToString(_dr["Id"]) + "'>");
                        sb.Append("<td><b data-localize='ai_Email'>Email</b></td>");
                        sb.Append("<td><span>" + _dr["NotifyEmailAddress"].ToString() + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td><b data-localize='ai_DateMarked'>Date Marked</b></td>");
                        sb.Append("<td><span id='" + Convert.ToString(_dr["Id"]) + "'>" + DateMarked + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                    }

                    if (status)
                    {
                        //active part part
                        sb.Append("<div class='col-md-4 rightmetronicpad'>");
                        sb.Append("<div class='portlet light bordered'>");
                        sb.Append("<div class='portlet-title'>");
                        sb.Append("<div class='caption'>");
                        sb.Append("<span class='admin_Title adminVisual Settings_Title caption-subject uppercase' style='padding-top:0;'>" + _dr["MaintenanceItemName"].ToString() + "</span></div>");
                        sb.Append("<div class='actions'>");
                        sb.Append("<input id='btnSaveMainatananceCommand_" + Convert.ToString(_dr["Id"]) + "' type='button' value='Save' class='admin_SaveIcon' title='Save' onclick='SaveMaintananceUpdate(" + Convert.ToString(_dr["Id"]) + ",1);' style='display: none;' />");
                        sb.Append("<a  id='EditAssetMaintanaceCommands" + Convert.ToString(_dr["Id"]) + "' onclick='EditAssetMaintanaceCommands(" + Convert.ToString(_dr["Id"]) + ");' title='Edit' Custom_attribute = '" + attribute + "' class='Edit_class EditAssetMaintanaceCommandsClass admin_EditIcon' style='margin-top:1px;'>Edit</a>");
                        sb.Append("<a style='display: none' id='aCancelMaintananceCommandRow_" + Convert.ToString(_dr["Id"]) + "' onclick='Cancel_maintanannceEdit(" + Convert.ToString(_dr["Id"]) + ");' title='Cancel' class=' admin_CancelEditIcon' >Cancel</a>");
                        sb.Append("<a id='adeleteMaintananceCommand" + Convert.ToString(_dr["Id"]) + "' onclick='deleteMaintananceCommand(" + Convert.ToString(_dr["Id"]) + ");' title='Delete' class='admin_DeleteIcon' style='margin-top:4px;'>Edit</a>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<div class='portlet-body'>");
                        sb.Append("<table class='table' id='MaintDetailTable_" + Convert.ToString(_dr["Id"]) + "'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr>");
                        sb.Append("<td style='border:none;'><b data-localize='ai_Name'>Name</b></td>");
                        sb.Append("<td style='border:none;'><span id='MaintenanceItemName_" + Convert.ToString(_dr["Id"]) + "'>" + _dr["MaintenanceItemName"].ToString() + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td><b data-localize='ai_Parameter'>Parameter</b></td>");
                        sb.Append("<td><span id='AssetMaintanancetype" + Convert.ToString(_dr["Id"]) + "'>" + parameter_command + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td><b data-localize='ai_ParameterValue'>Parameter Value</b></td>");
                        sb.Append("<td><span id='MaintanaceParameterValue" + Convert.ToString(_dr["Id"]) + "'>" + Selected_Value + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td><b data-localize='ai_Email'>Email</b></td>");
                        sb.Append("<td><span id='MaintananceNotifyEmailAddress" + Convert.ToString(_dr["Id"]) + "'>" + _dr["NotifyEmailAddress"].ToString() + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<td><b data-localize='ai_Remaining'>Remaining </b></td>");
                        sb.Append($"<td><span> { RemainingValue.ToString() } </span></td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");

                        sb.Append("<div id='MaintDetailFields_" + Convert.ToString(_dr["Id"]) + "' style='display:none;'>");
                        sb.Append("<div class='form-group form-md-line-input form-md-floating-label'>");
                        sb.Append("<input type='text'  id='txtMaintannceCommandText" + Convert.ToString(_dr["Id"]) + "'  class='form-control' style='display:none; '/>");
                        sb.Append("<label>Name</label>");
                        sb.Append("<span class='help-block'>Maintenance Item Name goes here...</span>");
                        sb.Append("</div>");
                        sb.Append("<div class='form-group form-md-line-input'>");
                        sb.Append("<input " + input_string + "   id='txtMaintannceCommandvalue" + Convert.ToString(_dr["Id"]) + "' lclatrr = '" + attribute + "'  class='form-control " + variable_class + "' />");
                        sb.Append("<label>Parameter Value</label>");
                        sb.Append("<span class='help-block'>Parameter Value goes here...</span>");
                        sb.Append("</div>");
                        sb.Append("<div class='form-group form-md-line-input form-md-floating-label'>");
                        sb.Append("<input type='text'    id='txtMaintanancenotify_email" + Convert.ToString(_dr["Id"]) + "'  class='form-control' style='display:none; '/>");
                        sb.Append("<label>Email</label>");
                        sb.Append("<span class='help-block'>Put Email here to get updates...</span>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                    }
                    sb.Append("</div>");
                    sb.Append("</div>");
                    if (!IsMarkedAsCompleted  && !status)
                    {
                        //due part
                        sb.Append("<div class='col-md-6 rightmetronicpad'>");
                        sb.Append("<div class='portlet light bordered'>");
                        sb.Append("<div class='portlet-title'>");
                        sb.Append("<div class='caption'>");
                        sb.Append("<span class='admin_Title adminVisual Settings_Title caption-subject uppercase' style='padding-top:0;'>" + _dr["MaintenanceItemName"].ToString() + "</span></div>");
                        sb.Append("</div>");
                        sb.Append("<div class='portlet-body'>");
                        sb.Append("<table class='table'>");
                        sb.Append("<tbody>");
                        sb.Append("<tr id='MaintenanceItemName_" + Convert.ToString(_dr["Id"]) + "'>");
                        sb.Append("<td style='border:none;'><b>Name</b></td>");
                        sb.Append("<td style='border:none;'><span>" + _dr["MaintenanceItemName"].ToString() + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr id='AssetMaintanancetype" + Convert.ToString(_dr["Id"]) + "'>");
                        sb.Append("<td><b>Parameter</b></td>");
                        sb.Append("<td><span>" + parameter_command + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr id='MaintanaceParameterValue" + Convert.ToString(_dr["Id"]) + "'>");
                        sb.Append("<td><b>Parameter Value</b></td>");
                        sb.Append("<td><span>" + Selected_Value + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr id='MaintananceNotifyEmailAddress" + Convert.ToString(_dr["Id"]) + "'>");
                        sb.Append("<td><b>Email</b></td>");
                        sb.Append("<td><span>" + _dr["NotifyEmailAddress"].ToString() + "</span></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td><input id='setIsActive" + Convert.ToString(_dr["Id"]) + "' type='button' value='Mark as completed ' class='button buttonSmall' title='Save' onclick='SaveMaintananceUpdate(" + Convert.ToString(_dr["Id"]) + ",2);' style='margin-top: 10px' /></td>");
                        sb.Append("<td>&nbsp;</td>");
                        sb.Append("</tr>");
                        sb.Append("</tbody>");
                        sb.Append("</table>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");

                    }

                    sb.Append("</div>");
                }

                if (counter == 0)
                {
                    sb.Clear();

                    sb.Append("");
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("AdminCtrl", "MaintenanceRecord()", ex.Message + ex.StackTrace);
                sb.Append("Internal Execution Error");

            }

            return sb.ToString();

        }


        public static string TimeSpanCalculator(object _seconds)
        {

            string returnResult = string.Empty;

            var smTimespan = new TimeSpan();

            if (_seconds != DBNull.Value)
            {
                smTimespan = TimeSpan.FromSeconds(Math.Abs(Convert.ToDouble(_seconds)));
            }
            else
            {
                returnResult = TimeSpan.FromSeconds(0).ToString();

                smTimespan = TimeSpan.FromSeconds(0);
            }

            if (smTimespan.Days > 0)
            {

                returnResult = smTimespan.Days + " days " + smTimespan.Hours + " hrs " + smTimespan.Minutes + " mins " + smTimespan.Seconds + " secs";
            }
            else if (smTimespan.Days < 1 && smTimespan.Hours > 0)
            {
                returnResult = smTimespan.Hours + " hrs " + smTimespan.Minutes + " mins " + smTimespan.Seconds + " secs";
            }
            else if (smTimespan.Hours < 1 && smTimespan.Minutes > 0)
            {
                returnResult = smTimespan.Minutes + " mins " + smTimespan.Seconds + " secs";
            }
            else if (smTimespan.Minutes < 1 && smTimespan.Seconds > 0)
            {
                returnResult = smTimespan.Seconds + " secs";
            }
            else if (smTimespan.Seconds == 0)
            {
                returnResult = "0 secs";
            }

            return returnResult;

        }


    }
}