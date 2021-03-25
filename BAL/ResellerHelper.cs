using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic
{
    public static class ResellerHelper
    {


        public static int GetResellerIdFromClientId(int ClientId)
        {
            DAL_ResellerHelper DalReseller = new DAL_ResellerHelper();

            int ResellerId = DalReseller.GetDalResellerIdFromClientId(ClientId);
            return ResellerId;
        }

        public static bool GetClientIsTrial(int ClientId)
        {
            DAL_ResellerHelper DalClient = new DAL_ResellerHelper();

            bool isOnTrial = DalClient.GetClientIsTrial(ClientId);
            return isOnTrial;
        }

        public static string GetDevicePhoneNumber(string SIMID)
        {
            DAL_ResellerHelper DalClient = new DAL_ResellerHelper();

            string PhoneNumber = DalClient.GetDevicePhoneNumber(SIMID);
            return PhoneNumber;
        }

        public static bool HasClientExceededSmsQuota(int ClientId)
        {

            DAL_ResellerHelper DalClient = new DAL_ResellerHelper();
            bool QuotaExceeded = DalClient.HasClientExceededSmsQuota(ClientId);
            return QuotaExceeded;

        }

        public static bool IsSMSEnabled(int ResellerId)
        {

            DAL_ResellerHelper DalClient = new DAL_ResellerHelper();
            bool isSMSEnabled = DalClient.IsSMSEnabled(ResellerId);
            return isSMSEnabled;

        }
    }
}
