using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using System.Configuration;
using WLT.ErrorLog;

namespace WLT.BusinessLogic
{
    public static class Administration
    {


        public static string CheckForDuplicateDriverId(string DriverIdNo, int DriverId)
        {

            try
            {
                DAL_Administration obj = new DAL_Administration();

                //Check if this DriverIdNo Already is active
                return obj.CheckIfDriverIdAlreadyInUse(DriverIdNo, DriverId);
            }

            catch (Exception ex)
            {               

                LogError.RegisterErrorInLogFile("Administration.cs", "CheckForDuplicateDriverId()", ex.Message  + ex.StackTrace);

                return "0";
            }

        }


        public static int DeleteEncryptedDriverIds(string ipkDriverId)
        {

            try
            {
                DAL_Administration obj = new DAL_Administration();

                //Delete all instances from the lookup
                obj.DeleteEncryptedDriverId(ipkDriverId);
               
                return 1;
            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "Administration.cs", "DeleteEncryptedDriverIds()", ex.Message  + ex.StackTrace);
                //clsError.RegisterErrorInLogFile(System. "BusinessLogic.Administration.cs", "SaveEncryptedDriverIds()", ex.Message  + ex.StackTrace);
                ////NewID = "Internal execution Error !";
                return -1;
            }

        }

        public static int SaveEncryptedDriverIds(string DriverIdNumber, string ipkDriverId )
        {

            try
            {
                DAL_Administration obj = new DAL_Administration();
                
                //AT and FM DriverID
                obj.SaveEncryptedDriverId(ipkDriverId, ConvertDriverIdToEncryptedId(DriverIdNumber, "AT"));
                
                //FM DriverID
                obj.SaveEncryptedDriverId(ipkDriverId, ConvertDriverIdToEncryptedId(DriverIdNumber, "FM"));

                //FM2 DriverID
                obj.SaveEncryptedDriverId(ipkDriverId, ConvertDriverIdToEncryptedId(DriverIdNumber, "FM2"));

                //Save the Actual Driver ID
                obj.SaveEncryptedDriverId(ipkDriverId, DriverIdNumber);

                //add new device Type here by passing in the string of device type and editing function ConvertDriverIdToEncryptedId

                return 1;
            }
            
            catch(Exception ex)
            {
                LogError.RegisterErrorInLogFile( "Administration.cs", "SaveEncryptedDriverIds()", ex.Message  + ex.StackTrace);
                //clsError.RegisterErrorInLogFile(System. "BusinessLogic.Administration.cs", "SaveEncryptedDriverIds()", ex.Message  + ex.StackTrace);
                ////NewID = "Internal execution Error !";
                return -1;
            }

        }
       
        public static String ConvertDriverIdToEncryptedId(string DriverIdNumber, string device_type)
        {
            //Pass in Driver ID as printed on iButton or tag,  and it will return the ID that is sent via the hardware (its sent in different format)
            if (DriverIdNumber != null || DriverIdNumber != "")
            {
                long number = Convert.ToInt64(DriverIdNumber, 16);
                byte[] bytes = BitConverter.GetBytes(number);
                string retval = "";

                foreach (byte b in bytes)
                {
                    retval += b.ToString("X2");
                }

                if (device_type == "AT" || device_type == "QL")
                {
                    retval = retval.Substring(2, 12);
                }

                if (device_type == "FM")
                {
                    retval = retval.Substring(0, 8);

                }

                if (device_type == "FM2")
                {
                    //do nothing
                }

                return retval;
            }
            else
            {
                return DriverIdNumber;
            }
            

        }


        public static string PayPal_Handler(string ValidationMess, double Amount, int ResellerID)
        {
            string returns = "1";
            try
            {
                DAL_Administration obj = new DAL_Administration();


                obj.PayPal_Handler(ValidationMess, Amount, ResellerID);

              
            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "Administration.cs", "PayPal_Handler()", ex.Message  + ex.StackTrace);
           
            }
            return returns;
        }
       



    }
}
