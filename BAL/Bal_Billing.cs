using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;
using WLT.ErrorLog;
using System.Data;
using Newtonsoft.Json;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_Billing
    {
        public string AssignAssetToAPlan(EL_Billing el_Billing)
        {
            string result = "";
            DAL_Billing dal_Billing = new DAL_Billing();

            result = dal_Billing.AssignAssetToAPlan(el_Billing);
            return result;
        }

        public string GetBillingHistory(EL_Billing el_Billing)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _BillingHistory = new DataTable();

            DAL_Billing dal_Billing = new DAL_Billing();

            ds = dal_Billing.GetBillingHistory(el_Billing);

            _BillingHistory = ds.Tables[0].Copy();

            var data = new
            {
                BillingHistory = _BillingHistory
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }


        public string MarkAsPaid(EL_Billing el_Billing, int ifkUserID)
        {

            string result = "";
            DAL_Billing dal_Billing = new DAL_Billing();

            result = dal_Billing.MarkAsPaid(el_Billing, ifkUserID);
            return result;
        }

        public string GetBillingPlanForAsset(EL_Billing el_Billing)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _BillingPlan = new DataTable();

            DAL_Billing dal_Billing = new DAL_Billing();

            ds = dal_Billing.GetBillingPlanForAsset(el_Billing);

            _BillingPlan = ds.Tables[0].Copy();

            var data = new
            {
                BillingPlan = _BillingPlan
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }

        public string GetUpcomingEmailReminderDetails(EL_Billing el_Billing)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _EmailReminders = new DataTable();

            DAL_Billing dal_Billing = new DAL_Billing();

            ds = dal_Billing.GetUpcomingEmailReminderDetails(el_Billing);

            _EmailReminders = ds.Tables[0].Copy();

            var data = new
            {
                EmailReminders = _EmailReminders
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }

        public string GetOverDueBillingDetails(EL_Billing el_Billing)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _OverDueBill = new DataTable();

            DAL_Billing dal_Billing = new DAL_Billing();

            ds = dal_Billing.GetOverDueBillingDetails(el_Billing);

            _OverDueBill = ds.Tables[0].Copy();

            var data = new
            {
                OverDueBill = _OverDueBill
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }

        public string GetUpcomingBillingDetails(EL_Billing el_Billing, int showFor)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _UpcomingBill = new DataTable();

            DAL_Billing dal_Billing = new DAL_Billing();

            ds = dal_Billing.GetUpcomingBillingDetails(el_Billing, showFor);

            _UpcomingBill = ds.Tables[0].Copy();

            var data = new
            {
                UpcomingBill = _UpcomingBill
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }

        public string GetMarkedAsPaidDetails(EL_Billing el_Billing, int showFor)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _MarkedAsPaid = new DataTable();

            DAL_Billing dal_Billing = new DAL_Billing();

            ds = dal_Billing.GetMarkedAsPaidDetails(el_Billing, showFor);

            _MarkedAsPaid = ds.Tables[0].Copy();

            var data = new
            {
                MarkedAsPaid = _MarkedAsPaid
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }

        public string GetAllResellerPlans(EL_Billing el_Billing)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _ResellerPlans = new DataTable();

            DAL_Billing dal_Billing = new DAL_Billing();

            ds = dal_Billing.GetAllResellerPlans(el_Billing);

            _ResellerPlans = ds.Tables[0].Copy();

            var data = new
            {
                ResellerPlans = _ResellerPlans
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }

        public string GetAssetsNotAssignedToAPlan(EL_Billing el_Billing)
        {
            string result = "";

            DataSet ds = new DataSet();
            DataTable _NotAssignedToAPlan = new DataTable();

            DAL_Billing dal_Billing = new DAL_Billing();

            ds = dal_Billing.GetAssetsNotAssignedToAPlan(el_Billing);

            _NotAssignedToAPlan = ds.Tables[0].Copy();

            var data = new
            {
                NotAssignedToAPlan = _NotAssignedToAPlan
            };

            result = JsonConvert.SerializeObject(data, Formatting.Indented);

            return result;
        }

        public string SaveBillingPlans(EL_Billing el_Billing)
        {
            string result = "";
            DAL_Billing dal_Billing = new DAL_Billing();

            result = dal_Billing.SaveBillingPlans(el_Billing);
            return result;
        }


        public string GetUserEmailsToNotify(EL_Billing el_Billing)
        {
            DataSet ds = new DataSet();
            string json = "";
            DataTable _Users = new DataTable();

            DAL_Billing dal_Billing = new DAL_Billing();

            ds = dal_Billing.GetUserEmailsToNotify(el_Billing);

            _Users = ds.Tables[0].Copy();

            var data = new
            {

                Users = _Users
            };

            json = JsonConvert.SerializeObject(data, Formatting.Indented);


            return json;
        }

        public string SaveBillingNotifier(EL_Billing el_Billing)
        {
            string result = "";
            DAL_Billing dal_Billing = new DAL_Billing();

            result = dal_Billing.SaveBillingNotifier(el_Billing);
            return result;
        }

        public string DeleteUpcomingEmailReminderDetails(EL_Billing el_Billing)
        {
            string result = "";
            DAL_Billing dal_Billing = new DAL_Billing();

            result = dal_Billing.DeleteUpcomingEmailReminderDetails(el_Billing);
            return result;
        }

        public string DeleteResellerPlan(EL_Billing el_Billing)
        {
            string result = "";
            DAL_Billing dal_Billing = new DAL_Billing();

            result = dal_Billing.DeleteResellerPlan(el_Billing);
            return result;
        }   
     
    }


}
