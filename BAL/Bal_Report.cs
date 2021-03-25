using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using WLT.DataAccessLayer.DAL;
using Newtonsoft.Json;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_Report
    {
        #region reports
        //get fuel data method 
        public string FuelInfo2(string TimeZoneID, int userid, int reportId, int operation)
        {

            List<FuelModel> fuelList = new List<FuelModel>();

            var ds = DAL_Reports.FetchfuelInfo(TimeZoneID, userid, reportId, operation);

            double MxDiff = Convert.ToDouble(ds.Tables[0].Compute("max([diff])", string.Empty));



            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var Location = string.Empty;
                var Difference = 0.0;
                if (Convert.ToInt32(row["vReportID"]) == 813 || Convert.ToInt32(row["vReportID"]) == 812)
                {
                    Location = Convert.ToString(row["txtLocation"]);
                    Difference = Convert.ToDouble(row["diff"]);
                }

                fuelList.Add(new FuelModel
                {
                    Asset = Convert.ToString(row["Asset"]),

                    Date = Convert.ToDateTime(row["dgpDatetime"]),

                    FuelData = Convert.ToDouble(row["Analog"]),

                    Description = Convert.ToString(row["Description"]),

                    Location = Location,

                    Difference = (Convert.ToDouble(row["diff"]) * 10) / MxDiff,

                    ReportID = Convert.ToInt32(row["vReportID"]),

                    vpkDeviceID = Convert.ToInt32(row["vpkDeviceID"])

                });
            }






            var json = JsonConvert.SerializeObject(fuelList, Formatting.Indented);

            return json;
        }


        public string FuelInfo3(string TimeZoneID, int userid, int reportId, int operation)
        {

            List<FuelModel> fuelList = new List<FuelModel>();

            var fuelObj = new FuelModel();

            var ds = DAL_Reports.FetchfuelInfo(TimeZoneID, userid, reportId, operation);

            double MxDiff = Convert.ToDouble(ds.Tables[0].Compute("max([diff])", string.Empty));

            var FuelDataDictionary = new Dictionary<int, FuelModel>();

            var dt = ds.Tables[0].Copy();

            int counter = 0;
            var Location = string.Empty;

            foreach (DataRow row in dt.Rows)
            {

                var color = string.Empty;
                var Difference = (Convert.ToDouble(row["diff"]) * 10) / MxDiff;
                switch (Convert.ToInt32(row["vReportID"]))

                {
                    case 812:
                        Location = fuelObj.Location;
                        color = "#3BAFDA";

                        FuelDataDictionary[counter - 1].grphColor = color;
                        FuelDataDictionary[counter - 1].Location = Convert.ToString(row["txtLocation"]);
                        FuelDataDictionary[counter - 1].Difference = 10;

                        var Avg = (FuelDataDictionary[counter - 1].FuelData + Convert.ToDouble(row["Analog"])) * .5;

                        var lblObj = new FuelModel { Difference = Convert.ToDouble(row["diff"]), grphColor = color, Date = Convert.ToDateTime(row["dgpDatetime"]), ReportID = 1, Location = "Sudden Increase ", FuelData = Avg };

                        FuelDataDictionary.Add(counter, lblObj);
                        counter++;

                        break;
                    case 813:

                        Location = fuelObj.Location;
                        color = "#DA4453";

                        FuelDataDictionary[counter - 1].grphColor = color;
                        FuelDataDictionary[counter - 1].Location = Convert.ToString(row["txtLocation"]);
                        FuelDataDictionary[counter - 1].Difference = 10;

                        var Avg1 = (FuelDataDictionary[counter - 1].FuelData + Convert.ToDouble(row["Analog"])) * .5;

                        var lblOb = new FuelModel { Difference = Convert.ToDouble(row["diff"]), Location = "Sudden Decrease ", grphColor = color, Date = Convert.ToDateTime(row["dgpDatetime"]), ReportID = 1, FuelData = Avg1 };

                        FuelDataDictionary.Add(counter, lblOb);
                        counter++;

                        break;

                    default:
                        Location = string.Empty;
                        break;
                }







                fuelObj = new FuelModel
                {
                    Asset = Convert.ToString(row["Asset"]),

                    Date = Convert.ToDateTime(row["dgpDatetime"]),

                    FuelData = Math.Round(Convert.ToDouble(row["Analog"]), 2),

                    Description = Convert.ToString(row["Description"]),

                    Location = Location,

                    grphColor = "#5CB2DA",

                    Difference = Difference,

                    ReportID = Convert.ToInt32(row["vReportID"]),

                    vpkDeviceID = Convert.ToInt32(row["vpkDeviceID"])

                };

                FuelDataDictionary.Add(counter, fuelObj);
                counter++;
            }

            fuelList = FuelDataDictionary.Values.ToList();

            var json = JsonConvert.SerializeObject(fuelList, Formatting.Indented);

            return json;
        }

        public string FuelInfo(string TimeZoneID, int userid, int reportId, int operation)
        {
            double doubleTry ;

            double _TotalDistance = 0;

            double _firstOdometerValue = 0.0;

            double _FuelUsed = 0;

            double _Consumption = 0.0;

            double _Drains = 0.0;

            double _Refills = 0.0;

            double? _firstValue = 0.0;

            Queue<double> ReadingsUnitsAveraged = new Queue<double>();

            Queue<double> IgnitionStatusQueue = new Queue<double>();

            var fuelList = new List<FuelModel>();

            var fuelObj = new FuelModel();

            var _DAL_Reports = new DAL_Reports();

            var ds = _DAL_Reports.getAnalogData_FromDb(userid, reportId, 2, 0,1);

            if (ds.Tables[0].Rows.Count>0 &&Convert.ToInt32(ds.Tables[4].Rows[0]["usesCanData"]) ==1)
            {
                foreach(DataRow row in ds.Tables[2].Rows)
                {
                    string[] fuelStr = Convert.ToString(row["vAnalog1"]).Split(' ');

                    row["vAnalog1"] = Convert.ToDouble(fuelStr[0]);
                }

            }
            var dt = ds.Tables[2].Clone();

            var rows = ds.Tables[2].Select().Where(p => (Convert.ToDouble(p["vAnalog1"]) > 0));

                if (rows.Count()>0)
                 dt = rows.CopyToDataTable();
                else
                dt = ds.Tables[2].Copy();


            if (dt.Rows.Count > 0)
            {
                double MxDiff = Convert.ToDouble(dt.Compute("max([diff])", string.Empty));

                var FuelDataDictionary = new Dictionary<int, FuelModel>();

                int counter = 0;

                var Location = string.Empty;

                double? Avg1 = 0;

               var rowCount =  dt.Rows.Count;

                for (int i=0;i<rowCount;i++ )
                {
                    DataRow row = dt.Rows[i];

                    var color = string.Empty;

                    var Difference = (Convert.ToDouble(row["diff"]) * 10) / MxDiff;

                    switch (Convert.ToInt32(row["vReportID"]))

                    {
                        case 812:   //drains 


                            var drain = Double.TryParse(Convert.ToString(row["additionalEventInfo"]), out doubleTry) ? Convert.ToDouble(row["additionalEventInfo"]) : 0;

                            _Drains += drain;

                            Location = fuelObj.Location;
                            color = "#DA4453";
                            if (FuelDataDictionary.ContainsKey(counter - 1))
                            {
                                FuelDataDictionary[counter - 1].grphColor = color;
                                FuelDataDictionary[counter - 1].Location = Convert.ToString(row["vTextMessage"]);
                                FuelDataDictionary[counter - 1].Difference = 10;
                                Avg1 = (FuelDataDictionary[counter - 1].FuelData + Convert.ToDouble(row["vAnalog1"])) * .5;
                            }

                            var lblObj = new FuelModel { Difference = Convert.ToDouble(row["diff"]), grphColor = color, Date = Convert.ToDateTime(row["dGPSDateTime"]), ReportID = 1, Location = "Sudden Decrease ", FuelData = Avg1 };

                            FuelDataDictionary.Add(counter, lblObj);
                            counter++;

                            break;
                        case 813:  //refill 

                            _Refills += Double.TryParse(Convert.ToString(row["additionalEventInfo"]), out doubleTry) ? Convert.ToDouble(row["additionalEventInfo"]) : 0;

                            Location = fuelObj.Location;
                            color = "#3BAFDA";
                            

                            if (FuelDataDictionary.ContainsKey(counter - 1))
                            {
                                FuelDataDictionary[counter - 1].grphColor = color;
                                FuelDataDictionary[counter - 1].Location = Convert.ToString(row["vTextMessage"]);
                                FuelDataDictionary[counter - 1].Difference = 10;
                                Avg1 = (FuelDataDictionary[counter - 1].FuelData + Convert.ToDouble(row["vAnalog1"])) * .5;

                            }

                            var lblOb = new FuelModel { Difference = Convert.ToDouble(row["diff"]), Location = "Sudden Increase ", grphColor = color, Date = Convert.ToDateTime(row["dGPSDateTime"]), ReportID = 1, FuelData = Avg1 };

                            FuelDataDictionary.Add(counter, lblOb);
                            counter++;

                            break;

                        default:
                            Location = string.Empty;
                            break;
                    }


                    fuelObj = new FuelModel
                    {

                        Date = Convert.ToDateTime(row["dGPSDateTime"]),

                        FuelData = Math.Round(Convert.ToDouble(row["vAnalog1"]), 2),

                        Description = Convert.ToString(row["Description"]),

                        Location = Location,

                        grphColor = "#5CB2DA",

                        Difference = Difference,

                        ReportID = Convert.ToInt32(row["vReportID"])    

                };


                    var _FuelQueueHelper = new FuelQueueHelper
                    {
                        AvgCount = 10,
                        CurrentPointValue = Convert.ToDouble(fuelObj.FuelData),
                        ReadingsUnitsAveraged = ReadingsUnitsAveraged,
                        IgnitionOnQueue = IgnitionStatusQueue,
                        ignitionQCount = 5
                    };

                     

                    fuelObj.AvgFuel = new Bal_Fuel().ConvertCurrentValueToAveragedValue(_FuelQueueHelper);

                    FuelDataDictionary.Add(counter, fuelObj);

                    counter++;


                    if (i == 0)
                    {
                        _firstValue = fuelObj.FuelData;

                        _firstOdometerValue =  Double.TryParse(Convert.ToString(row["vOdometer"]), out doubleTry) ? Convert.ToDouble(row["vOdometer"]) : 0;
                    }
                    if ((rowCount - 1) == i) // the last loop  then sum all refills and add it to the the consumption
                    {
                        var dddd = fuelObj.FuelData;

                        _FuelUsed = Convert.ToDouble((_firstValue - fuelObj.FuelData) + _Refills);

                        
                        var lastOdometerValue = Convert.ToDouble((Double.TryParse(Convert.ToString(row["vOdometer"]), out doubleTry) ? Convert.ToDouble(row["vOdometer"]) : 0));

                        _TotalDistance = lastOdometerValue - Convert.ToDouble(_firstOdometerValue);

                    }

                }

                fuelList = FuelDataDictionary.Values.ToList();

            }
            //Get logo

            string base64String = null;
            if (ds.Tables[3].Rows.Count > 0)
            {
                base64String = ds.Tables[3].Rows[0]["vLogo"].ToString();

            }

            
           
            var DateOfQuery = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, TimeZoneID).ToString("ddd dd MMMM yyyy HH:mm:ss ");
            var fp = ds.Tables[1].Rows[0];
            var detailsRow = ds.Tables[3].Rows[0];

                                   

            var fuelmodelObject = new FuelModel {
                FuelList = fuelList,
                max = Convert.ToDouble(fp["iMaxValue"]),
                min = Convert.ToDouble(fp["iMinValue"]),
                Asset = detailsRow["Asset"].ToString(),
                startDate =  Convert.ToDateTime(detailsRow["startDate"]).ToString("dd MMM yyyy   HH:mm:ss "),
                endDate =  Convert.ToDateTime(detailsRow["endDate"]).ToString("dd MMM yyyy   HH:mm:ss "),
                DateOfQuery = DateOfQuery, Logo = base64String,
                ifkDeviceId = Convert.ToInt32(detailsRow["ifkDeviceId"]),
                SensorType = Convert.ToInt32(detailsRow["SensorType"]),
                iAnalogNumber = Convert.ToInt32(detailsRow["iAnalogNumber"]),
                ifkCanBusType = Convert.ToInt32(detailsRow["ifkCanBusType"]),
                Distance= Math.Round(_TotalDistance,2),
                Consumption=0,
                FuelUsed = _FuelUsed,
                Drain =_Drains,
                Refill=_Refills
            };

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                if ( Convert.ToInt32( item["id"] )== Convert.ToInt32( ds.Tables[4].Rows[0]["AnalogSensorID"]))
                {
                    fuelmodelObject.Title = Convert.ToString(item["vName"]);
                    fuelmodelObject.analogType = Convert.ToString(item["vUnitText"]);
                    fuelmodelObject.analogType = Convert.ToString(item["vUnitText"]);
                }
                   
            }

            var json = JsonConvert.SerializeObject(fuelmodelObject, Formatting.Indented);

            return json;



        }


        public static void SaveReportsSentMailLogs(EL_ReportsEmailLog _EL_ReportsEmailLog)
        {

            DAL_Reports _dal_Reports   = new DAL_Reports();

            _dal_Reports.ReportLogs(_EL_ReportsEmailLog);
        }



        public static El_ReportStats GetReportsUsage(El_Report _El_Report )
        {
            var defaultOperation = _El_Report.Operation;

            DAL_Reports _dal_Reports = new DAL_Reports();

            var res = new El_ReportStats();

            Func<int, DataTable> fnGetStats = delegate (int operation)
            {
                var dt = new DataTable();

                _El_Report.Operation = operation;

                var res = _dal_Reports.ReportLogs(_El_Report);
                
                foreach (DataTable _dt in res.Tables)
                    dt = _dt;

                _El_Report.Operation = defaultOperation;

                return dt;
            };

            Func<DataTable, DataTable> GroupResellerReports = delegate (DataTable _dt)
            {
               var myhash = new HashSet<int>();

                var newDt = _dt.Clone();

                newDt.Columns.Add("data", typeof(string));

                foreach (DataRow _dr in _dt.Rows)
                {
                    var reseller_id = Convert.ToInt32(_dr["ResellerId"]);

                    if (!myhash.Contains(reseller_id))
                    {
                        var dataRow = newDt.NewRow();

                        myhash.Add(reseller_id);

                       var data =  _dt.AsEnumerable().Where(p => p.Field<int>("ResellerId") == reseller_id).ToList();

                        var json  = JsonConvert.SerializeObject(data.Select(r => new { report_name = r["vReportTypeName"], count = r["count"] }));

                        dataRow.ItemArray = _dr.ItemArray;

                        dataRow["data"] = json;

                        dataRow["count"] = data.Count();

                        newDt.Rows.Add(dataRow);

                    }
                }

                _El_Report.Operation = defaultOperation;

                return newDt;
            };


            if (_El_Report.Operation == 0)
            {
                res.UsageSummary = fnGetStats(1);
                res.ResellerReportConsumptionStats = GroupResellerReports( fnGetStats(2));
                res.ReportLogs =   fnGetStats(3);
            }           

           else if (_El_Report.Operation == 1)
            {
                res.UsageSummary = fnGetStats(1);
               
            }

            else if (_El_Report.Operation == 2)
            {
                res.ResellerReportConsumptionStats = GroupResellerReports(fnGetStats(2));
            }

            return res;
          }

        public static El_ReportOdometerDto GetOdometers(long vpkdeviceid, DateTime utcStartDate, DateTime utcEndDate)
        {

            var ds = DAL_Reports.GetOdometers(vpkdeviceid, utcStartDate, utcEndDate);


            foreach (DataTable dt in ds.Tables)
                foreach (DataRow dr in dt.Rows)
                {
                    return new El_ReportOdometerDto
                    {
                        InitialOdometerValue = Convert.ToDouble(dr["_initial_odo"]),
                        FinalOdometerValue = Convert.ToDouble(dr["_final_odo"]),
                    };
                }
            return null;
        }


        #endregion

    }
    //    public partial class Bal_Custom_Report
    //    {

    //        public Bal_Custom_Report()
    //        {

    //        }
    //        public DataSet SaveReport(El_Custom_Report _El_Custom_Report)
    //        {
    //            var dataset = new DataSet();

    //            try
    //            {
    //                if (_El_Custom_Report.Advanced)
    //                {

    //                    Bal_Dynamic_Report _Bal_Dynamic_Report = new Bal_Dynamic_Report();

    //                   int reportId = _Bal_Dynamic_Report.InitializeReportReducer(_El_Custom_Report);


    //                }
    //                else
    //                {
    //                    dataset = DAL_Reports.BasicCustomReport(_El_Custom_Report);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                LogError.RegisterErrorInLogFile(  "Bal_Custom_Report.cs", "SaveReport()", ex.Message  + ex.StackTrace);

    //            }

    //            return dataset;

    //        }
    //    }
    //        public partial class Bal_Dynamic_Report
    //    {
    //        public Bal_Dynamic_Report() { }

    //        public int InitializeReportReducer(El_Custom_Report _El_Dynamic_Report)
    //        {            
    //            var report = new Report();

    //            //parameter
    //            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
    //            reportParameter1.Name = "UserId";
    //            reportParameter1.Text = "UserId";

    //            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
    //            reportParameter2.Name = "ReportTypeId";
    //            reportParameter2.Text = "ReportTypeId";

    //            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
    //            reportParameter3.Name = "TimeZoneID";
    //            reportParameter3.Text = "TimeZoneID";


    //            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
    //            reportParameter4.Name = "ReportId";
    //            reportParameter4.Text = "ReportId";

    //            //page header
    //            var pageHeaderSection = new PageHeaderSection();
    //                pageHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(3.7000002861022949D);
    //                CreateControls(pageHeaderSection, report);  //to create texTBox controls

    //            //detail section 
    //            var detailSection = new Telerik.Reporting.DetailSection();
    //                detailSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.2);


    //       //group header section 
    //       var labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
    //            labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(.7);
    //            labelsGroupHeaderSection.PrintOnEveryPage = true;
    //            labelsGroupHeaderSection.Style.BorderColor.Bottom = System.Drawing.Color.Teal;
    //            labelsGroupHeaderSection.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
    //            labelsGroupHeaderSection.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);


    //            Telerik.Reporting.TextBox deviceName = new Telerik.Reporting.TextBox();
    //            deviceName.CanGrow = true;
    //            deviceName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0));
    //            deviceName.Name = "textBox3";
    //            deviceName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.3998990058898926D), Telerik.Reporting.Drawing.Unit.Cm(1D));
    //            deviceName.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
    //            deviceName.Style.Font.Name = "Microsoft Sans Serif";
    //            deviceName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
    //            deviceName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
    //            deviceName.StyleName = "Data";
    //            deviceName.Value = "=Fields.devicename";

    //            labelsGroupHeaderSection.Items.AddRange(new ReportItemBase[] { deviceName });
    //            //row group 
    //            Group vpkdeviceidgrouper = new Group();
    //            vpkdeviceidgrouper.GroupHeader = labelsGroupHeaderSection;
    //            vpkdeviceidgrouper.Groupings.Add(new Telerik.Reporting.Grouping(_El_Dynamic_Report.vpkDeviceidGrouper));
    //            vpkdeviceidgrouper.Name = "vpkDeviceIDGroup";


    //            ////row group 
    //            //Group dategrouper = new Group();
    //            //dategrouper.GroupHeader = labelsGroupHeaderSection;
    //            //dategrouper.Groupings.Add(new Telerik.Reporting.Grouping(_El_Dynamic_Report.dateGrouper));
    //            //dategrouper.Name = "dateGroup";

    //            report.Groups.AddRange(new Telerik.Reporting.Group[] { vpkdeviceidgrouper });
    //            report.PageSettings.Landscape = true;
    //            report.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25D), Telerik.Reporting.Drawing.Unit.Mm(15D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D));
    //            report.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
    //            report.Name= "Custom Report";
    //            report.Width = Telerik.Reporting.Drawing.Unit.Cm(28.599998474121094D);

    //            report.Items.AddRange(new ReportItemBase[] { pageHeaderSection,detailSection });

    //            report.ReportParameters.Add(reportParameter1);
    //            report.ReportParameters.Add(reportParameter2);
    //            report.ReportParameters.Add(reportParameter3);
    //            report.ReportParameters.Add(reportParameter4);

    //            var initialOperation = _El_Dynamic_Report.Operation;

    //            var DynamicReportDefinition = CreateDefinition(_El_Dynamic_Report);

    //            int i = 0;

    //            var length = 0.0;
    //            var previousWidth = 0.10000015050172806D;

    //            foreach (var item in DynamicReportDefinition)
    //            {

    //                var _el_Dynamic_Report = item.Value;



    //                 length += previousWidth;

    //                _el_Dynamic_Report.EntireLength = length;

    //               var txt = CreateTextBox(_el_Dynamic_Report, item.Key, labelsGroupHeaderSection);

    //                previousWidth = _el_Dynamic_Report.Width;

    //               //  Adjust(txt,item.Key);

    //                detailSection.Items.AddRange(new ReportItemBase[] { txt });

    //                i++;
    //            }



    //            Telerik.Reporting.ObjectDataSource objectDataSource1 = new Telerik.Reporting.ObjectDataSource();
    //            objectDataSource1.DataMember = "Source";
    //            objectDataSource1.DataSource = typeof(Bal_Reports_Helper);           
    //            objectDataSource1.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("UserID", typeof(int), "=Parameters.UserId"));
    //            objectDataSource1.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("ReportId", typeof(int), "=Parameters.ReportId"));
    //            objectDataSource1.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("TimeZoneID", typeof(string), "=Parameters.TimeZoneID"));
    //            objectDataSource1.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("ReportTypeId", typeof(int), "=Parameters.ReportTypeId"));
    //            report.DataSource = objectDataSource1;


    //            StringWriter sw = new StringWriter();

    //            using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(sw))
    //            {
    //                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializer =
    //                    new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();

    //                xmlSerializer.Serialize(xmlWriter, report);
    //            }

    //            _El_Dynamic_Report.Definition = sw.ToString();

    //            _El_Dynamic_Report.Operation = initialOperation;

    //            var _DynamicReport = DAL_Reports.DynamicReport(_El_Dynamic_Report);


    //            return Convert.ToInt32(_DynamicReport.Tables[0].Rows[0]["iReportTypeId"]);
    //        }

    //        public string InitializeReportReducer(El_Custom_Report _El_Dynamic_Report,int j)
    //        {
    //            var report = new Report();

    //            //parameter
    //            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
    //            reportParameter1.Name = "UserId";
    //            reportParameter1.Text = "UserId";

    //            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
    //            reportParameter2.Name = "ReportTypeId";
    //            reportParameter2.Text = "ReportTypeId";

    //            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
    //            reportParameter3.Name = "TimeZoneID";
    //            reportParameter3.Text = "TimeZoneID";


    //            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
    //            reportParameter4.Name = "ReportId";
    //            reportParameter4.Text = "ReportId";

    //            //page header
    //            var pageHeaderSection = new PageHeaderSection();
    //            pageHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(3.7000002861022949D);
    //            CreateControls(pageHeaderSection, report);  //to create texTBox controls

    //            //detail section 
    //            var detailSection = new Telerik.Reporting.DetailSection();
    //            detailSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.2);


    //            //group header section 
    //            var labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
    //            labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(.7);
    //            labelsGroupHeaderSection.PrintOnEveryPage = true;
    //            labelsGroupHeaderSection.Style.BorderColor.Bottom = System.Drawing.Color.Teal;
    //            labelsGroupHeaderSection.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
    //            labelsGroupHeaderSection.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);


    //            Telerik.Reporting.TextBox deviceName = new Telerik.Reporting.TextBox();
    //            deviceName.CanGrow = true;
    //            deviceName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0));
    //            deviceName.Name = "textBox3";
    //            deviceName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.3998990058898926D), Telerik.Reporting.Drawing.Unit.Cm(1D));
    //            deviceName.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
    //            deviceName.Style.Font.Name = "Microsoft Sans Serif";
    //            deviceName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
    //            deviceName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
    //            deviceName.StyleName = "Data";
    //            deviceName.Value = "=Fields.devicename";

    //            labelsGroupHeaderSection.Items.AddRange(new ReportItemBase[] { deviceName });
    //            //row group 
    //            Group vpkdeviceidgrouper = new Group();
    //            vpkdeviceidgrouper.GroupHeader = labelsGroupHeaderSection;
    //            vpkdeviceidgrouper.Groupings.Add(new Telerik.Reporting.Grouping(_El_Dynamic_Report.vpkDeviceidGrouper));
    //            vpkdeviceidgrouper.Name = "vpkDeviceIDGroup";



    //            report.Groups.AddRange(new Telerik.Reporting.Group[] { vpkdeviceidgrouper });
    //            report.PageSettings.Landscape = true;
    //            report.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25D), Telerik.Reporting.Drawing.Unit.Mm(15D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(10D));
    //            report.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
    //            report.Name = "Custom Report";
    //            report.Width = Telerik.Reporting.Drawing.Unit.Cm(28.599998474121094D);



    //            report.Items.AddRange(new ReportItemBase[] { pageHeaderSection, detailSection });

    //            report.ReportParameters.Add(reportParameter1);
    //            report.ReportParameters.Add(reportParameter2);
    //            report.ReportParameters.Add(reportParameter3);
    //            report.ReportParameters.Add(reportParameter4);

    //            var initialOperation = _El_Dynamic_Report.Operation;

    //            var DynamicReportDefinition = CreateDefinition(_El_Dynamic_Report);

    //            int i = 0;

    //            var length = 0.0;
    //            var previousWidth = 0.10000015050172806D;

    //            foreach (var item in DynamicReportDefinition)
    //            {

    //                var _el_Dynamic_Report = item.Value;



    //                length += previousWidth;

    //                _el_Dynamic_Report.EntireLength = length;

    //                var txt = CreateTextBox(_el_Dynamic_Report, item.Key, labelsGroupHeaderSection);

    //                previousWidth = _el_Dynamic_Report.Width;

    //                //  Adjust(txt,item.Key);

    //                detailSection.Items.AddRange(new ReportItemBase[] { txt });

    //                i++;
    //            }

    //            //var dataSource = new Telerik.Reporting.SqlDataSource();
    //            //dataSource.ConnectionString = AppConfiguration.ConnectionString();
    //            //dataSource.SelectCommand = "sp_Report_Dynamic";
    //            //dataSource.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
    //            //dataSource.Parameters.Add("@Operation", System.Data.DbType.Int32, 3);           
    //            //report.DataSource = dataSource;

    //            // UserID, int ReportId, int TimeZoneID, int ReportTypeId

    //            Telerik.Reporting.ObjectDataSource objectDataSource1 = new Telerik.Reporting.ObjectDataSource();
    //            objectDataSource1.DataMember = "Source";
    //            objectDataSource1.DataSource = typeof(Bal_Reports_Helper);
    //            objectDataSource1.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("UserID", typeof(int), "=Parameters.UserId"));
    //            objectDataSource1.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("ReportId", typeof(int), "=Parameters.ReportId"));
    //            objectDataSource1.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("TimeZoneID", typeof(string), "=Parameters.TimeZoneID"));
    //            objectDataSource1.Parameters.Add(new Telerik.Reporting.ObjectDataSourceParameter("ReportTypeId", typeof(int), "=Parameters.ReportTypeId"));
    //            report.DataSource = objectDataSource1;


    //            StringWriter sw = new StringWriter();

    //            using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(sw))
    //            {
    //                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializer =
    //                    new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();

    //                xmlSerializer.Serialize(xmlWriter, report);
    //            }

    //            _El_Dynamic_Report.Definition = sw.ToString();

    //            _El_Dynamic_Report.Operation = initialOperation;

    //         //   var _DynamicReport = ReportsDataAccess.DynamicReport(_El_Dynamic_Report);


    //            return sw.ToString();
    //        }
    //        public void CreateControls(PageHeaderSection pageHeaderSection,Report report)
    //        {
    //            var txtReportType = new Telerik.Reporting.TextBox();
    //            txtReportType.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.30000025033950806D), Telerik.Reporting.Drawing.Unit.Cm(0.30000004172325134D));
    //            txtReportType.Name = "txtReportType";
    //            txtReportType.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.5D), Telerik.Reporting.Drawing.Unit.Cm(0.5997999906539917D));
    //            txtReportType.Style.Color = System.Drawing.Color.DarkGray;
    //            txtReportType.Style.Font.Bold = true;
    //            txtReportType.Style.Font.Name = "Verdana";
    //            txtReportType.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
    //            txtReportType.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
    //            txtReportType.Value = "=Fields.reportname";

    //            PictureBox pbReportLogo = new Telerik.Reporting.PictureBox();
    //            pbReportLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.5D), Telerik.Reporting.Drawing.Unit.Cm(0.30000004172325134D));
    //            pbReportLogo.MimeType = "";
    //            pbReportLogo.Name = "pbReportLogo";
    //            pbReportLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.8083329200744629D), Telerik.Reporting.Drawing.Unit.Cm(1.9470827579498291D));
    //            pbReportLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
    //            pbReportLogo.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
    //            pbReportLogo.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
    //            pbReportLogo.Value = "=Fields.logo";

    //            var txtGeneratedOn = new Telerik.Reporting.TextBox();
    //            txtGeneratedOn.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896D));
    //            txtGeneratedOn.Name = "textBox4";
    //            txtGeneratedOn.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.299797534942627D), Telerik.Reporting.Drawing.Unit.Cm(0.44999998807907104D));
    //            txtGeneratedOn.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
    //            txtGeneratedOn.Style.Font.Bold = false;
    //            txtGeneratedOn.Style.Font.Name = "Verdana";
    //            txtGeneratedOn.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
    //            txtGeneratedOn.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
    //            txtGeneratedOn.StyleName = "PageInfo";
    //            txtGeneratedOn.Value = "Generated on:";


    //            // 
    //            // txtReportGeneratedDate
    //            // 
    //            // pageHeaderSection.
    //            var txtReportGeneratedDate = new Telerik.Reporting.TextBox();
    //            txtReportGeneratedDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.499997615814209D), Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896D));
    //            txtReportGeneratedDate.Name = "txtReportGeneratedDate";          
    //            txtReportGeneratedDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.3159375190734863D), Telerik.Reporting.Drawing.Unit.Cm(0.44999998807907104D));
    //            txtReportGeneratedDate.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
    //            txtReportGeneratedDate.Style.Font.Bold = false;
    //            txtReportGeneratedDate.Style.Font.Name = "Verdana";
    //            txtReportGeneratedDate.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
    //            txtReportGeneratedDate.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
    //            txtReportGeneratedDate.Value = "=Fields.genarateddate";


    //            // 
    //            // txtFirstDate
    //            // 
    //            var txtFirstDate = new Telerik.Reporting.TextBox();
    //            txtFirstDate.CanGrow = true;
    //            txtFirstDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.21322916448116303D), Telerik.Reporting.Drawing.Unit.Cm(2.1999998092651367D));
    //            txtFirstDate.Name = "txtFirstDate";
    //            txtFirstDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.596771240234375D), Telerik.Reporting.Drawing.Unit.Cm(0.44999998807907104D));
    //            txtFirstDate.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
    //            txtFirstDate.Style.Font.Bold = false;
    //            txtFirstDate.Style.Font.Name = "Verdana";
    //            txtFirstDate.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
    //            txtFirstDate.StyleName = "Data";
    //            txtFirstDate.Value = "=Fields.firstdate";
    //            // 
    //            // txtLastDate
    //            // 
    //            var txtLastDate = new Telerik.Reporting.TextBox();
    //            txtLastDate.CanGrow = true;
    //            txtLastDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.21322916448116303D), Telerik.Reporting.Drawing.Unit.Cm(2.7999999523162842D));
    //            txtLastDate.Name = "txtLastDate";
    //            txtLastDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.6100001335144043D), Telerik.Reporting.Drawing.Unit.Cm(0.44999998807907104D));
    //            txtLastDate.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
    //            txtLastDate.Style.Font.Bold = false;
    //            txtLastDate.Style.Font.Name = "Verdana";
    //            txtLastDate.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
    //            txtLastDate.StyleName = "Data";
    //            txtLastDate.Value = "=Fields.Lastdate";
    //            // 
    //            // txtasset
    //            // 
    //            var txtasset = new Telerik.Reporting.TextBox();
    //            txtasset.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.19999837875366211D), Telerik.Reporting.Drawing.Unit.Cm(1.6000001430511475D));
    //            txtasset.Name = "txtasset";
    //            txtasset.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.596771240234375D), Telerik.Reporting.Drawing.Unit.Cm(0.44999998807907104D));
    //            txtasset.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
    //            txtasset.Style.Font.Bold = false;
    //            txtasset.Style.Font.Name = "Verdana";
    //            txtasset.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
    //            txtasset.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
    //            txtasset.Value = "=Fields.asset";






    //            pageHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
    //            pbReportLogo,
    //            txtasset,
    //            txtLastDate,
    //            txtFirstDate,
    //            txtReportType,
    //            txtReportGeneratedDate,
    //            txtGeneratedOn
    //             });
    //           pageHeaderSection.Name = "PageHeaderSection1";
    //        }

    //        public TextBox CreateTextBox(El_Custom_Report _dim, string column,GroupHeaderSection labelsGroupHeaderSection)
    //        {
    //            //header cell 

    //            Telerik.Reporting.TextBox headerText = new Telerik.Reporting.TextBox();
    //            headerText.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(_dim.EntireLength), Telerik.Reporting.Drawing.Unit.Cm(1));
    //            headerText.Name = "HeaderTextBox" + column;
    //            headerText.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(_dim.Width), Telerik.Reporting.Drawing.Unit.Cm(0.843758761882782D));
    //            headerText.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
    //            headerText.Style.BorderColor.Default = System.Drawing.Color.Silver;
    //            headerText.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
    //            headerText.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
    //            headerText.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
    //            headerText.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
    //            headerText.Style.Font.Bold = true;
    //            headerText.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
    //            headerText.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Point(5D);
    //            headerText.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(5D);
    //            headerText.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(5D);
    //            headerText.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Point(5D);
    //            headerText.StyleName = "Normal.TableHeader";
    //            headerText.Value =column;


    //            // 
    //            // textBox3
    //            // 


    //            labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { headerText });


    //            //row cell
    //            var rowText = new TextBox();
    //            rowText.Angle = 0D;
    //            rowText.CanGrow = true;
    //            //rowText.Filters.Add(new Telerik.Reporting.Filter("=RowNumber() % 2", Telerik.Reporting.FilterOperator.Equal, "0"));

    //            Adjust(rowText, column);
    //            rowText.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
    //            rowText.ConditionalFormatting.AddRange(new Telerik.Reporting.Drawing.FormattingRule[] { });
    //            rowText.Name = "RowTextBox" + column;
    //            rowText.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(_dim.Width), Telerik.Reporting.Drawing.Unit.Cm(0.75361716747283936D));
    //            rowText.Style.BackgroundColor = System.Drawing.Color.Transparent;
    //            rowText.Style.BorderColor.Bottom = System.Drawing.Color.Teal;
    //            rowText.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
    //            rowText.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
    //            rowText.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(0.5D);
    //            rowText.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
    //            rowText.Style.Font.Bold = true;
    //            rowText.Style.Font.Name = "Verdana";
    //            rowText.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
    //            rowText.Style.LineColor = System.Drawing.Color.Black;
    //            rowText.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
    //            rowText.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
    //            rowText.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
    //            rowText.StyleName = "Data";
    //            rowText.Value = "= Fields.[" + column.Trim()+ "]";
    //            rowText.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(_dim.EntireLength), Telerik.Reporting.Drawing.Unit.Cm(0));

    //            return rowText;
    //        }

    //        public   Dictionary<string, El_Custom_Report> CreateDefinition(El_Custom_Report _El_Dynamic_Report)
    //        {                       

    //            _El_Dynamic_Report.Operation = 4;          

    //            var ds = DAL_Reports.DynamicReport(_El_Dynamic_Report,0);

    //            var dt1 = ds.Tables[0];

    //            var dt2 = ds.Tables[1];

    //             dt2.SetColumnsOrder(new string[] { "date", "time","starttime","endtime","devicename","duration", "event", "location", "from","to","startlocation","endlocation","odometer","distance","totaldistance","speed","maxspeed","roadspeed", "difference", "latitude", "longitude","battery", "driver","vpkdeviceid" });


    //            Dictionary<string, El_Custom_Report> Features = new Dictionary<string, El_Custom_Report>();

    //            foreach (System.Data.DataColumn column in dt2.Columns)
    //            {  
    //                DataRow[] rows = dt1.Select("vName ='" + column.ColumnName.Trim() + "'");

    //                dt2.Rows[0].SetField(column.ColumnName,  rows[0]["vDimension"]);

    //                var json = rows[0]["vDimension"].ToString();

    //                var dim = JsonConvert.DeserializeObject<El_Custom_Report>(json);

    //                Features.Add(rows[0]["vName"].ToString(), dim);
    //            }

    //            return Features;

    //        }

    //        public void Adjust( TextBox control,string column)
    //        {

    //            switch (column)
    //            {
    //                case "date":
    //                    control.Format = "{0:dddd  dd MMM yyyy}";

    //                    break;
    //                case "time":
    //                    control.Format = "{0:HH:mm:ss}";
    //                    break;
    //                default:
    //                    break;
    //            }



    //        }

    //        public static object LoadCustomReportData(object el_Dynamic_Report)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public void OrderColumns(List<Tuple<string, string>> userList, int templateType)
    //        {
    //            var orderedList = new string[] { "date", "time", "devicename", "event", "location", "latitude", "longitude,battery", "driver" };

    //            var remainingList = orderedList.Except(userList.Select(x => x.Item1));




    //        }Install-Package GeoCoordinate
    //    }
    //    public partial class Bal_Dynamic_Report {

    //        public DataSet LoadCustomReportFeatures(El_Custom_Report _El_Dynamic_Report)
    //        {
    //               var ds = DAL_Reports.DynamicReport(_El_Dynamic_Report);

    //            return ds;
    //        }

    //        public static DataSet  LoadCustomReportData(El_Custom_Report _El_Dynamic_Report)
    //        {
    //            var ds = DAL_Reports.DynamicReport(_El_Dynamic_Report);

    //            return ds;
    //        }
    //    }

}