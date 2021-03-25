using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL_Report;
using Microsoft.Extensions.Options;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic.Reports
{
    public class ReportsHelper
    {
        public static string Connectionstring { get; set; }

        private readonly wlt_Config _configurationService;
               
        public ReportsHelper()
        {
         
            _configurationService = AppConfiguration.GetAppSettings<wlt_Config>("wlt_config");

            Connectionstring = AppConfiguration.GetAppSettings<wlt_Config>("ConnectionStrings").wlt_WebAppConnectionString;
        }



        public EL_Language GetReportLanguageCultureDetails(int _operation, int _userId, int _languageId)
        {

            var report = new DAL_Reports();

           var ds = report.GetReportLanguageCultureDetails(_operation, _userId, _languageId);

            var _clsLanguage = new EL_Language();

            foreach (DataTable tbl in ds.Tables)
                foreach (DataRow row in tbl.Rows)
                {
                    _clsLanguage = new EL_Language
                    {
                        CultureID = Convert.ToString(row["vCultureInfoCode"]),
                        Language = Convert.ToString(row["vLanguage"]),
                        TwoLanguageID = Convert.ToString(row["vTwoLetterLangCode"]),
                        ThreeLanguageID = Convert.ToString(row["vThreeLetterLangCode"]),
                        Country = Convert.ToString(row["vCountry"]),
                    };
                }


            return _clsLanguage;
        }
    }
}
