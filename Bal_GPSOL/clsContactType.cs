using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WLT.BusinessLogic.Bal_GPSOL;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsContactType : General
    {
        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();

        private int _Operation;
        private int _ipkContactTypeID;
        private string _vTypeName;
        private bool _bStatus;
        private int _ipkContactID;
        private string _vName;
        private int _ifkType;
        private string _vEmail;
        private string _vOfficeTel;
        private string _vMobile;
        private string _vNotes;
        private int _ifkAssetID;
        private char _ifkGroupID;
        private int _iParent;
        private string _vpkDeviceID;
        private string _LstOrDetails;
        private string _LstHistoryContact;

        public string LstHistoryContact
        {
            get { return _LstHistoryContact; }
            set { _LstHistoryContact = value; }
        }

        public int iParent
        {
            get { return _iParent; }
            set { _iParent = value; }
        }

        public char ifkGroupID
        {
            get { return _ifkGroupID; }
            set { _ifkGroupID = value; }
        }

        public int ifkAssetID
        {
            get { return _ifkAssetID; }
            set { _ifkAssetID = value; }
        }

        public string vNotes
        {
            get { return _vNotes; }
            set { _vNotes = value; }
        }

        public string vMobile
        {
            get { return _vMobile; }
            set { _vMobile = value; }
        }

        public string vOfficeTel
        {
            get { return _vOfficeTel; }
            set { _vOfficeTel = value; }
        }

        public string vEmail
        {
            get { return _vEmail; }
            set { _vEmail = value; }
        }

        public int ifkType
        {
            get { return _ifkType; }
            set { _ifkType = value; }
        }

        public string vName
        {
            get { return _vName; }
            set { _vName = value; }
        }

        public int ipkContactID
        {
            get { return _ipkContactID; }
            set { _ipkContactID = value; }
        }

        public bool bStatus
        {
            get { return _bStatus; }
            set { _bStatus = value; }
        }

        public int Operation
        {
            get { return _Operation; }
            set { _Operation = value; }
        }
        public int ipkContactTypeID
        {
            get { return _ipkContactTypeID; }
            set { _ipkContactTypeID = value; }
        }
        public string vTypeName
        {
            get { return _vTypeName; }
            set { _vTypeName = value; }
        }

        public string vpkDeviceID
        {
            get { return _vpkDeviceID; }
            set { _vpkDeviceID = value; }
        }

        public string LstOrDetails
        {
            get { return _LstOrDetails; }
            set { _LstOrDetails = value; }
        }

        public clsContactType()
        {
            // constructor here 

        }

        public clsContactType(string vTypeName, int ipkContactTypeID)
        {
            this.vTypeName = vTypeName;
            this.ipkContactTypeID = ipkContactTypeID;

        }

        public clsContactType(int ipkContactTypeID, int ipkContactID,int ifkType, string vName, string vTypeName, string vEmail, string vMobile, string vOfficeTel, string vNotes)
        {

            this.ipkContactTypeID = ipkContactTypeID;
            this.ifkType = ifkType;
            this.ipkContactID = ipkContactID;        
            this.vName = vName;
            this.vTypeName = vTypeName;
            this.vEmail = vEmail;
            this.vMobile = vMobile;
            this.vOfficeTel = vOfficeTel;
            this.vNotes = vNotes;

        }

        public string SaveContact()
        {
            string returnstring = "";
            SqlParameter[] param = new SqlParameter[15];

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkContactID", SqlDbType.Int);
                param[1].Value = ipkContactID;

                param[2] = new SqlParameter("@vName", SqlDbType.VarChar);
                param[2].Value = vName;

                param[3] = new SqlParameter("@vMobile", SqlDbType.VarChar);
                param[3].Value = vMobile;

                param[4] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[4].Value = bStatus;

                param[5] = new SqlParameter("@iParent", SqlDbType.Int);
                param[5].Value = iParent;

                param[6] = new SqlParameter("@ipkContactTypeID", SqlDbType.Int);
                param[6].Value = ipkContactTypeID;

                param[7] = new SqlParameter("@Error", SqlDbType.Int);
                param[7].Direction = ParameterDirection.Output;

                param[8] = new SqlParameter("@vNotes", SqlDbType.VarChar);
                param[8].Value = vNotes;

                param[9] = new SqlParameter("@vOfficeTel", SqlDbType.VarChar);
                param[9].Value = vOfficeTel;

                param[10] = new SqlParameter("@ifkType", SqlDbType.Int);
                param[10].Value = ifkType;

                param[11] = new SqlParameter("@vEmail", SqlDbType.VarChar);
                param[11].Value = vEmail;

                param[12] = new SqlParameter("@ifkAssetID", SqlDbType.Int);
                param[12].Value = ifkAssetID;

                param[13] = new SqlParameter("@ifkGroupID", SqlDbType.Int);
                param[13].Value = ifkGroupID;

                param[14] = new SqlParameter("@vTypeName", SqlDbType.VarChar);
                param[14].Value = vTypeName;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ContactType", param);

                if (param[7].Value.ToString() == "-1")
                {
                    returnstring = "-1";
                }
                else if (param[7].Value.ToString() != "-1" && param[7].Value.ToString() != "-2")
                {
                    returnstring = param[7].Value.ToString();
                }
                else if (param[7].Value.ToString() == "-2")
                {
                    returnstring = "-2";
                }
                else if (param[7].Value.ToString() == "-3")
                {
                    returnstring = "-3";
                }

            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "clsContactType.cs", "SaveContact()", ex.Message  + ex.StackTrace);
                returnstring = "Internal Execution Error";
            }

            return returnstring;
        }


        public DataSet GetContactDetails()
        {
            SqlParameter[] param = new SqlParameter[6];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;

                param[2] = new SqlParameter("@ipkContactID", SqlDbType.Int);
                param[2].Value = ipkContactID;

                param[3] = new SqlParameter("@ifkAssetID", SqlDbType.Int);  
                param[3].Value = ifkAssetID;

                param[4] = new SqlParameter("@ifkType", SqlDbType.Int);
                param[4].Value = ifkType;

                param[5] = new SqlParameter("@vpkDeviceID", SqlDbType.VarChar);
                param[5].Value = vpkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ContactType", param);
            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "clsContactType.cs", "GetContactDetails()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }


        public List<clsContactType> SelectContactInfo()
        {
            DataSet ds = new DataSet();

            List<clsContactType> lstConatctInfo = new List<clsContactType>();

            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkContactID", SqlDbType.Int);
                param[1].Value = ipkContactID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_ContactType", param);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstConatctInfo.Add(new clsContactType(
                                                              Convert.ToInt32(row["ipkContactTypeID"].ToString()),
                                                              Convert.ToInt32(row["ipkContactID"].ToString()),
                                                              Convert.ToInt32(row["ifkType"].ToString()),
                                                              row["vName"].ToString(),
                                                              row["vTypeName"].ToString(),
                                                              row["vEmail"].ToString(),                                                              
                                                              row["vMobile"].ToString(),
                                                              row["vOfficeTel"].ToString(),                                                               
                                                             row["vNotes"].ToString()));
                    }
                }



            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "clsContactType.cs", "SelectContactInfo()", ex.Message  + ex.StackTrace);
            }


            return lstConatctInfo;
        }

    }
}