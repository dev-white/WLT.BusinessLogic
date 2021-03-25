using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic.BAL
{
     public  class Bal_PTO_Report
    {

        public Bal_PTO_Report() { }


        public DataSet  GetPTO(int reportId, int UserId, int Operation, int reportTypeId) {

            var dataInfo = DAL_PTO_Report.GetPTO_fromDb(reportId,UserId,Operation,reportTypeId );

            return dataInfo;
        }  


    }
}
