using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using UniqueKey;
using System.Data;
using Newtonsoft.Json;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_Asset_ExternalSharedLink
    {

        public  string  SaveAsset_ExternalSharedLink(EL_Asset_ExternalSharedLink _EL_Asset_ExternalSharedLink , EL_AuditTrail _EL_AuditTrail, ref int successcode,String url)
        {            

            string _returnStr = string.Empty;

            var random = KeyGenerator.GetUniqueKey(10);

            _EL_AuditTrail.ifkUserId = _EL_Asset_ExternalSharedLink.iLastUpdateBy;

            _EL_AuditTrail.vIpAddress = _EL_Asset_ExternalSharedLink.ipAddress;


            _EL_Asset_ExternalSharedLink.vPublicUrl = url;


            var _DAL_Asset_ExternalSharedLink = new DAL_Asset_ExternalSharedLink();
            try
            {
                var result = _DAL_Asset_ExternalSharedLink.UpdateOrSaveorSelect_Asset_ExternalSharedLink(_EL_Asset_ExternalSharedLink, _EL_AuditTrail, ref successcode);

                _returnStr = JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("BAL_Asset_ExternalSharedLink.cs", "SaveAsset_ExternalSharedLink()", ex.Message + ex.StackTrace);
            }
           
            return _returnStr;
        }
        public  List<EL_Asset_ExternalSharedLink> GetIAsset_ExternalSharedLinktems(EL_Asset_ExternalSharedLink _EL_Asset_ExternalSharedLink, EL_AuditTrail _EL_AuditTrail, ref int successcode)
        {
            var ds = new DataSet();

            var liveLocationItems = new List<EL_Asset_ExternalSharedLink>();

            _EL_AuditTrail.ifkUserId = _EL_Asset_ExternalSharedLink.iLastUpdateBy;

            _EL_Asset_ExternalSharedLink.dExpiryDated = DateTime.UtcNow;

            var _DAL_Asset_ExternalSharedLink = new DAL_Asset_ExternalSharedLink();

            ds = _DAL_Asset_ExternalSharedLink.UpdateOrSaveorSelect_Asset_ExternalSharedLink(_EL_Asset_ExternalSharedLink, _EL_AuditTrail, ref successcode);

            foreach (DataTable dt in ds.Tables)
                foreach (DataRow row in dt.Rows)
                    liveLocationItems.Add( new EL_Asset_ExternalSharedLink {                        
                        Asset_ExternalSharedItemLinkId= Convert.ToInt32( row["id"]),
                        dExpiryDated =  UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( Convert.ToDateTime(row["dExpiryDate"]), _EL_Asset_ExternalSharedLink.vTimeZoneID),
                        //ifkClientId= Convert.ToInt32(row[""]),
                        ifkDeviceId = Convert.ToInt32(row["ifkdeviceid"]),
                        //IMEI= Convert.ToInt32(row[""]),
                        iLastUpdateBy= Convert.ToInt32(row["iLastUpdateBy"]),
                        iStatus= Convert.ToInt32(row["iStatus"]),                        
                        iUrlId = Convert.ToInt32(row["iUrlId"]),
                        vPublicUrl= Convert.ToString(row["vPublicUrl"]),
                        vRecipientMail= Convert.ToString(row["vRecipientMail"]),
                        UserName = Convert.ToString(row["vName"]),
                        UserPhoto = Convert.ToString(row["PhotoName"]),
                        dLastUpdateDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["dLastEditedDate"]), _EL_Asset_ExternalSharedLink.vTimeZoneID)
                    });




            return liveLocationItems;
        }


        public List<EL_AuditTrail> GetAsset_ExternalSharedLinkAuditTrail(EL_Asset_ExternalSharedLink _EL_Asset_ExternalSharedLink, EL_AuditTrail _EL_AuditTrail, ref int successcode)
        {
            var ds = new DataSet();

            var _EL_AuditTrailList = new  List<EL_AuditTrail>();

            _EL_Asset_ExternalSharedLink.dExpiryDated = DateTime.UtcNow;

            var _DAL_Asset_ExternalSharedLink = new DAL_Asset_ExternalSharedLink();

             ds = _DAL_Asset_ExternalSharedLink.UpdateOrSaveorSelect_Asset_ExternalSharedLink(_EL_Asset_ExternalSharedLink, _EL_AuditTrail, ref successcode);

            foreach (DataTable dt in ds.Tables)
                foreach (DataRow row in dt.Rows)
                    _EL_AuditTrailList.Add(new EL_AuditTrail
                    {

                        
                        dLoggedDateTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( Convert.ToDateTime(row["dLoggedDateTime"]), _EL_Asset_ExternalSharedLink.vTimeZoneID),
                        vAdditionalInfo = Convert.ToString(row["vAdditionalInfo"]),
                        vIpAddress = Convert.ToString(row["vIpAddress"]),
                        ifkUserId = Convert.ToInt32(row["ifkUserId"]),
                        ifkClientId = Convert.ToInt32(row["ifkClientId"]),
                        ifkCrudeId = Convert.ToInt32(row["ifkCrudeId"]),
                        UserName = Convert.ToString(row["vName"]),
                        UserPhoto = Convert.ToString(row["PhotoName"]),
                        vActivity = Convert.ToString(row["vActivity"]),


                    });

            return _EL_AuditTrailList;
        }

        public string Update_ExternalSharedLinktems(EL_Asset_ExternalSharedLink _EL_Asset_ExternalSharedLink, EL_AuditTrail _EL_AuditTrail, ref int successcode)
        {
            var ds = new DataSet();

            var liveLocationItems = new List<EL_Asset_ExternalSharedLink>();

            _EL_AuditTrail.ifkUserId = _EL_Asset_ExternalSharedLink.iLastUpdateBy;

            _EL_AuditTrail.vIpAddress = _EL_Asset_ExternalSharedLink.ipAddress;

            //_EL_Asset_ExternalSharedLink.dExpiryDated = DateTime.UtcNow;

            var _DAL_Asset_ExternalSharedLink = new DAL_Asset_ExternalSharedLink();

            ds = _DAL_Asset_ExternalSharedLink.UpdateOrSaveorSelect_Asset_ExternalSharedLink(_EL_Asset_ExternalSharedLink, _EL_AuditTrail, ref successcode);



            return string.Empty;
                 
        }


        public DataSet GetExternalResellerDetails   (EL_Asset_ExternalSharedLink _EL_Asset_ExternalSharedLink, ref int successcode)
        {
            var ds = new DataSet();

            var  _EL_AuditTrail =new EL_AuditTrail();      

            _EL_AuditTrail.ifkUserId = _EL_Asset_ExternalSharedLink.iLastUpdateBy;         

            var _DAL_Asset_ExternalSharedLink = new DAL_Asset_ExternalSharedLink();
          
            ds = _DAL_Asset_ExternalSharedLink.UpdateOrSaveorSelect_Asset_ExternalSharedLink(_EL_Asset_ExternalSharedLink, _EL_AuditTrail, ref successcode);
            
         
            return ds;

        }


        public DataSet GetExternalResellerLiveData(EL_Asset_ExternalSharedLink _EL_Asset_ExternalSharedLink, ref int successcode)
        {
            var ds = new DataSet();

            var _EL_AuditTrail = new EL_AuditTrail();

            _EL_AuditTrail.ifkUserId = _EL_Asset_ExternalSharedLink.iLastUpdateBy;

            var _DAL_Asset_ExternalSharedLink = new DAL_Asset_ExternalSharedLink();


            ds = _DAL_Asset_ExternalSharedLink.UpdateOrSaveorSelect_Asset_ExternalSharedLink(_EL_Asset_ExternalSharedLink, _EL_AuditTrail, ref successcode);



            return ds;

        }


        public EL_Asset_ExternalSharedLink Get_PublicURL_ExternalSharedLinktem(EL_Asset_ExternalSharedLink _EL_Asset_ExternalSharedLink, EL_AuditTrail _EL_AuditTrail, ref int successcode)
        {
            var ds = new DataSet();

            var result = new EL_Asset_ExternalSharedLink();

            var liveLocationItems = new List<EL_Asset_ExternalSharedLink>();

            _EL_AuditTrail.ifkUserId = _EL_Asset_ExternalSharedLink.iLastUpdateBy;

            _EL_AuditTrail.vIpAddress = _EL_Asset_ExternalSharedLink.ipAddress;           

            var _DAL_Asset_ExternalSharedLink = new DAL_Asset_ExternalSharedLink();

            ds = _DAL_Asset_ExternalSharedLink.UpdateOrSaveorSelect_Asset_ExternalSharedLink(_EL_Asset_ExternalSharedLink, _EL_AuditTrail, ref successcode);

            foreach(DataTable dt in ds.Tables)
                foreach(DataRow row in dt.Rows)
                {
                    result.vPublicUrl = Convert.ToString(row["vPublicUrl"]);
                    result.vRecipientMail = Convert.ToString(row["vRecipientMail"]);
                }

            return result;

        }


        public EL_Asset_ExternalSharedLink UpdateResendUrlTrail_ExternalSharedLinktem(EL_Asset_ExternalSharedLink _EL_Asset_ExternalSharedLink, EL_AuditTrail _EL_AuditTrail, ref int successcode)
        {
            var ds = new DataSet();

            var result = new EL_Asset_ExternalSharedLink();

            var liveLocationItems = new List<EL_Asset_ExternalSharedLink>();

            _EL_AuditTrail.ifkUserId = _EL_Asset_ExternalSharedLink.iLastUpdateBy;

            _EL_AuditTrail.vIpAddress = _EL_Asset_ExternalSharedLink.ipAddress;

            var _DAL_Asset_ExternalSharedLink = new DAL_Asset_ExternalSharedLink();

            ds = _DAL_Asset_ExternalSharedLink.UpdateOrSaveorSelect_Asset_ExternalSharedLink(_EL_Asset_ExternalSharedLink, _EL_AuditTrail, ref successcode);

            
            return result;

        }

        public EL_Asset_ExternalSharedLink Delete_ExternalSharedLink(EL_Asset_ExternalSharedLink _EL_Asset_ExternalSharedLink, EL_AuditTrail _EL_AuditTrail, ref int successcode)
        {
            var ds = new DataSet();

            var result = new EL_Asset_ExternalSharedLink();

            var liveLocationItems = new List<EL_Asset_ExternalSharedLink>();

            _EL_AuditTrail.ifkUserId = _EL_Asset_ExternalSharedLink.iLastUpdateBy;

            _EL_AuditTrail.vIpAddress = _EL_Asset_ExternalSharedLink.ipAddress;

            var _DAL_Asset_ExternalSharedLink = new DAL_Asset_ExternalSharedLink();

            ds = _DAL_Asset_ExternalSharedLink.UpdateOrSaveorSelect_Asset_ExternalSharedLink(_EL_Asset_ExternalSharedLink, _EL_AuditTrail, ref successcode);


            return result;

        }

    }
}



namespace UniqueKey
{
    public class KeyGenerator
    {
        public static string GetUniqueKey(int size)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}