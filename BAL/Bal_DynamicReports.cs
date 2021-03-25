using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_DynamicReports
    {


        public DataSet GettempTable() {

         

            var result = DAL_Reports.Go_GetdynamicFields();

            return result;
        }

    }
}
