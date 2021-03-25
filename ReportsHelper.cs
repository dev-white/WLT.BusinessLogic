using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.Unused.BusinessLogic
{
    public class ReportsHelper
    {
        public ReportsHelper()
        {

        }

        public EL_Language GetReportLanguageCultureDetails(int _operation, int _userId, int _languageId)
        {
           var ds =  new DAL_Reports().GetReportLanguageCultureDetails(_operation, _userId, _languageId);

            var _clsLanguage = new EL_Language();

            foreach (DataTable tbl in ds.Tables)
                foreach(DataRow row in tbl.Rows)
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
