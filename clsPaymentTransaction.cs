using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using WLT.BusinessLogic.BAL;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;
using System.Globalization;
using System.Data;
using WLT.ErrorLog;

namespace WLT.BusinessLogic
{
  public   class clsPaymentTransaction
    {

    
       public static DateTime TryParsePaypalDatetimeToUtc( string paypalDatetime)
       {
           DateTime paymentDate;
           DateTime retValue;

           // PayPal formats from docs
           string[] formats = new string[] { "HH:mm:ss dd MMM yyyy PDT", "HH:mm:ss dd MMM yyyy PST", 
                                      "HH:mm:ss dd MMM, yyyy PST", "HH:mm:ss dd MMM, yyyy PDT", 
                                      "HH:mm:ss MMM dd, yyyy PST", "HH:mm:ss MMM dd, yyyy PDT" };
           if (false == DateTime.TryParseExact(paypalDatetime, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out paymentDate))
           {
               retValue = DateTime.MinValue;
               return retValue;
           }
           else
           {
               retValue = TimeZoneInfo.ConvertTimeToUtc(paymentDate, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));

               return retValue;
           }
       }
      public DataSet AuthenticatePayment(string Tran_Id, int Company_id, double Amount, string DateOfPayment,string email,string Payment_Id,string PaymentStatus,double Fee)
       {
           DataSet ds = new DataSet();
 
            try
            {
                //ds = Log_PaymentClass.Go_getBilling_Data(Tran_Id, Amount, Convert.ToDateTime(DateOfPayment), Company_id, email, Payment_Id, PaymentStatus, Fee);

                ds = Log_PaymentClass.Go_getBilling_Data(Tran_Id, Amount, TryParsePaypalDatetimeToUtc(DateOfPayment), Company_id, email, Payment_Id, PaymentStatus, Fee);
                   

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsPaymentTransaction.cs", "clsPaymentTransaction()", ex.Message  + ex.StackTrace);
               
            }
            return ds;
       }
  
    }
}
