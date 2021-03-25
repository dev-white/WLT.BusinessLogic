using System;
using System.Collections.Generic;
using System.Text;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.Bal_Reports
{
    public static class ReportsUtil
    {

        public static bool SetReportGeneratedForMail(El_ReportExportMarking _El_ReportExportMarking)
        {
            var _DAL_Reports = new DAL_Reports();

            _El_ReportExportMarking.operation = 1;

            return _DAL_Reports.MarkedReportForMail(_El_ReportExportMarking).success;
        }

        public static bool CheckIfGeneratedReportIsEmpty(El_ReportExportMarking _El_ReportExportMarking)
        {
            var _DAL_Reports = new DAL_Reports();

            _El_ReportExportMarking.operation = 2;

            return _DAL_Reports.MarkedReportForMail(_El_ReportExportMarking).is_empty;
        }
    }
}
