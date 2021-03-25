using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using WLT.BusinessLogic.Bal_GPSOL;
using System.Data;

namespace WLT.BusinessLogic.Bal_Reports
{
    public class ReportExtensions
    {

        public static string ChangeAssetHeaderLanguageString(string CultureID, object AssetName, object ReportID)
        {
            var _messageString = "";

            var _intVar = 0;

            var Assets = AssetName as string;

            var isInterger = int.TryParse(Assets, out _intVar);


            if (Convert.ToInt32(ReportID) == 1) //alerts
            {


                switch (CultureID)
                {
                    case "en-US":

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Specific Alerts" : "All Alerts" : Assets ?? "Alert";

                        break;

                    case "es-MX":

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Alertas especificas" : "Todas las alertas" : Assets ?? "Alerta";

                        break;

                    case "pt-PT":

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Alertas Específicos" : "Todos os alertas" : Assets ?? "Alerta";

                        break;

                    default:
                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Specific Alerts" : "All Alerts" : Assets ?? "Alert";
                        break;


                }

            }
            else if (Convert.ToInt32(ReportID) == 129) //Drivers
            {


                switch (CultureID)
                {
                    case "en-US":

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Specific Drivers" : "All Drivers" : Assets ?? "Driver";

                        break;

                    case "es-MX":

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Conductores Específicos" : "Todos los conductores" : Assets ?? "Conductores";

                        break;

                    case "pt-PT":

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Drivers específicos" : "Todos os Drivers" : Assets ?? "Driver";

                        break;

                    default:

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Specific Drivers" : "All Drivers" : Assets ?? "Driver";

                        break;
                }
            }
            else
            {

                switch (CultureID)
                {
                    case "en-US":

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Specific Assets" : "All Assets" : Assets ?? "Asset";

                        break;

                    case "es-MX":

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Activos Específicos" : "Todos los activos" : Assets ?? "activos";

                        break;

                    case "pt-PT":

                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Ativos Específicos" : "Todos os ativos" : Assets ?? "ativos";

                        break;

                    default:
                        _messageString = isInterger ? int.Parse(Assets) == 1 ? "Specific Assets" : "All Assets" : Assets ?? "Asset";
                        break;

                }



            }
            return _messageString;
        }

        public static string ChangeDateRangeFilterString(object CultureID, object RangeVariableType, object ReportID = null)
        {

            var _messageString = "";

            _messageString = int.Parse(RangeVariableType as string ?? "0") == 1 ? "Showing data within the time range" : "Showing data outside the time range";

            switch (CultureID)
            {
                case "en-US":

                    _messageString = int.Parse(RangeVariableType as string ?? "0") == 1 ? "Showing data within the time range" : "Showing data outside the time range";

                    break;

                case "es-MX":

                    _messageString = int.Parse(RangeVariableType as string ?? "0") == 1 ? "Mostrando datos dentro del rango de tiempo" : "Mostrando datos fuera del rango de tiempo";

                    break;

                case "pt-PT":

                    _messageString = int.Parse(RangeVariableType as string ?? "0") == 1 ? "Mostrando dados dentro do intervalo de tempo" : "Mostrando dados fora do intervalo de tempo";

                    break;

            }

            return _messageString;
        }

        public static string LanguageSpecifReportName(object CultureID, object RepotTypeId)
        {

            var _hsTable = new System.Collections.Hashtable();

            _hsTable.Add(131, new Hashtable()
            {

                    { "vSpanish", "Informe gráfico de Triplisting" },
                    { "vPortuguese", "Relatório de listagem de viagens gráficas" },

            }

            );





            var _result = string.Empty;

            if (CultureID != null)
                switch (CultureID as string)
                {
                    case "es-MX":

                        break;

                    case "pt-PT":

                        break;

                    default:

                        break;
                }

            return _result;
        }

        public static Hashtable EventsTranslation(object CultureID, string vEventIds)
        {
            var vLanguage = "";

            var _hsTranslatedEvents = new Hashtable();

            CultureID = CultureID ?? "";

            switch (CultureID)
            {

                case "es-MX":
                    vLanguage = "vSpanish";
                    break;

                case "pt-PT":
                    vLanguage = "vPortuguese";
                    break;

                default:
                    vLanguage = "vEnglish";
                    break;


            }


            if (vLanguage != "vEnglish")
            {
                var _ds = new clsReports_Project().GetTranslatedEvents(2, vLanguage, vEventIds);

                try
                {
                    foreach (DataTable dt in _ds.Tables)
                        foreach (DataRow dr in dt.Rows)
                            _hsTranslatedEvents.Add(Convert.ToInt32(dr["ifkEventsLookUpId"]), Convert.ToString(dr[vLanguage]));
                }
                catch (Exception ex)
                {
                    var d = ex;
                    //LogError.RegisterErrorInLogFile( "ReportLanguageExtensions.cs", "EventsTranslation()", ex.Message  + ex.StackTrace);
                }
            }
            return _hsTranslatedEvents;
        }


        public static string TranslateReportName(object CultureID, int ReportId)
        {

            string reportName = null;

            var _TranslatedReportName = new Hashtable();

            // translate amchart overspeedGraph report
            _TranslatedReportName.Add(103, new Hashtable() {
                {"es-MX","Informe sobre el gráfico de velocidad" },
                {"pt-PT","Sobre o relatório gráfico de velocidade" },

                     });


            if (_TranslatedReportName.ContainsKey(ReportId))
            {
                var reportTranslator = (Hashtable)_TranslatedReportName[ReportId];

                reportName = (string)reportTranslator[Convert.ToString(CultureID)];

            }


            return reportName;
        }

        public string GenericReportTranslation(string _rawText)
        {
            var _returnStr = "";

            var GeneralHashTable = new Hashtable();

            GeneralHashTable.Add(1, new Hashtable()
            {


            });






            return _returnStr;
        }
    }
}
