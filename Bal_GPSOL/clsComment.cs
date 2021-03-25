using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsComment:IDisposable
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        private int _Operation;
        private int _ipkCommentID;
        private int _ifkAlertCapturedID;
        private string _vComment;
        private int _ifkUserID;
        private DateTime _dEntryDate;
        private bool _bStatus;

        private bool _bIsMuted;
        private bool _bIsDismissed;

        private string _vhtmlComment;
        private string _vhtmlDetails;

        private string _strlat;
        private string _strlong;
        private string _isMute;
        private int _ifkAlertId;
        private int _ipkAssetID;
        private string _vhtmlContacts;

        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkCommentID { get { return _ipkCommentID; } set { _ipkCommentID = value; } }
        public int ifkAlertCapturedID { get { return _ifkAlertCapturedID; } set { _ifkAlertCapturedID = value; } }
        public int ifkAlertId { get { return _ifkAlertId; } set { _ifkAlertId = value; } }
        public string vComment { get { return _vComment; } set { _vComment = value; } }
        public int ifkUserID { get { return _ifkUserID; } set { _ifkUserID = value; } }
        public DateTime dEntryDate { get { return _dEntryDate; } set { _dEntryDate = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }

        public bool bIsMuted { get { return _bIsMuted; } set { _bIsMuted = value; } }
        public bool bIsDismissed { get { return _bIsDismissed; } set { _bIsDismissed = value; } }

        public string vhtmlComment { get { return _vhtmlComment; } set { _vhtmlComment = value; } }
        public string vhtmlDetails { get { return _vhtmlDetails; } set { _vhtmlDetails = value; } }

        public string strlat { get { return _strlat; } set { _strlat = value; } }
        public string strlong { get { return _strlong; } set { _strlong = value; } }
        public string isMute { get { return _isMute; } set { _isMute = value; } }

        public int ipkAssetID { get { return _ipkAssetID; } set { _ipkAssetID = value; } }
        public string vhtmlContacts { get { return _vhtmlContacts; } set { _vhtmlContacts = value; } }

        public clsComment()
        {
            // constructor
        }

        public clsComment(string vhtmlDetails, string vhtmlComment, string strlat, string strlong, string isMute)
        {
            this.vhtmlDetails = vhtmlDetails;
            this.vhtmlComment = vhtmlComment;
            this.strlat = strlat;
            this.strlong = strlong;
            this.isMute = isMute;
        }
        public clsComment(int vifkAlertId, string vhtmlDetails, string vhtmlComment, string strlat, string strlong, string isMute, string vhtmlContacts)
        {
            this.vhtmlDetails = vhtmlDetails;
            this.vhtmlComment = vhtmlComment;
            this.strlat = strlat;
            this.strlong = strlong;
            this.isMute = isMute;
            this.ifkAlertId = vifkAlertId;
            this.vhtmlContacts = vhtmlContacts;
        }
   
        public string SaveAlertComments()
        {
            string result = "";
       
            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@vComment", SqlDbType.VarChar);
                if (vComment == "")
                {
                    param[1].Value = System.DBNull.Value;
                }
                else
                {
                    param[1].Value = vComment;
                }

                param[2] = new SqlParameter("@ifkAlertCapturedID", SqlDbType.VarChar);
                param[2].Value = ifkAlertCapturedID;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter("@bIsDismissed", SqlDbType.Bit);
                param[4].Value = bIsDismissed;

                param[5] = new SqlParameter("@bIsMuted", SqlDbType.Int);
                param[5].Value = bIsMuted;
                
               SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_AlertComments", param);

               
                result = "comment added.";
               
            }
            catch (Exception ex)
            {
                
               LogError.RegisterErrorInLogFile( "clsComment.cs", "SaveComments()", ex.Message  + ex.StackTrace);
                result = "Internal execution error in SaveComments() :" + ex.Message;
            }

            return result;
        }

        public DataSet ViewAlertComments()
        {           
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[4];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ifkAlertCapturedID", SqlDbType.VarChar);
                param[1].Value = ifkAlertCapturedID;

                param[2] = new SqlParameter("@ifkAlertId", SqlDbType.VarChar);
                param[2].Value = ifkAlertId;

                param[3] = new SqlParameter("@ipkAssetID", SqlDbType.Int);
                param[3].Value = ipkAssetID;
                

                ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_AlertComments", param);
                              

            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "clsComment.cs", "ViewComments()", ex.Message  + ex.StackTrace);
                
            }

            return ds;
        }

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}