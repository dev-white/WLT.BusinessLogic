using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    [Serializable()]
    public class clsNewRole:IDisposable
    {


        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        private int _Operation;
        private int _Profile_Id;
        private string _Profile_Name;
        private string _Description;
        private bool _IsAdministrationAccess;
        private int _iParent;
        private int _iCreatedBy;
        private int _ifkUserID;
        private int _ifkRoleID;
        private string _chkID;
        private List<clsNewRole> _chkRoleID;
        private string _chkHTML;
        private int _Id;
        private int _isReport;
        private string _adminId;
        private string _viewerId;
        private int _intUserTypeId;

        public int intUserTypeId { get { return _intUserTypeId; } set { _intUserTypeId = value; } }
        public int Id { get { return _Id; } set { _Id = value; } }
        public int isReport { get { return _isReport; } set { _isReport = value; } }
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int Profile_Id { get { return _Profile_Id; } set { _Profile_Id = value; } }
        public string Profile_Name { get { return _Profile_Name; } set { _Profile_Name = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }
        public bool IsAdministrationAccess { get { return _IsAdministrationAccess; } set { _IsAdministrationAccess = value; } }
        public int iParent { get { return _iParent; } set { _iParent = value; } }
        public int iCreatedBy { get { return _iCreatedBy; } set { _iCreatedBy = value; } }
        public int ifkUserID { get { return _ifkUserID; } set { _ifkUserID = value; } }
        public int ifkRoleID { get { return _ifkRoleID; } set { _ifkRoleID = value; } }
        public string chkID { get { return _chkID; } set { _chkID = value; } }
        public List<clsNewRole> chkRoleID { get { return _chkRoleID; } set { _chkRoleID = value; } }
        public string chkHTML { get { return _chkHTML; } set { _chkHTML = value; } }
        public string adminId { get { return _adminId; } set { _adminId = value; } }
        public string viewerId { get { return _viewerId; } set { _viewerId = value; } }

        public clsNewRole()
        {
            // initialization constructore
        }

        public clsNewRole(int Profile_Id, string Profile_Name, bool IsAdministrationAccess)
        {
            this.Profile_Id = Profile_Id;
            this.Profile_Name = Profile_Name;
            this.IsAdministrationAccess = IsAdministrationAccess;

        }

        public clsNewRole(string chkID)
        {
            this.chkID = chkID;
        
        }

        public clsNewRole(bool IsAdministrationAccess, string Profile_Name)
        {
            this.IsAdministrationAccess = IsAdministrationAccess;
            this.Profile_Name = Profile_Name;

        }

        public clsNewRole(string chkHTML, List<clsNewRole> chkRoleID, string adminId, string viewerId)
        {
            this.chkHTML = chkHTML;
            this.chkRoleID = chkRoleID;
            this.adminId = adminId;
            this.viewerId = viewerId;
        }

        public clsNewRole(string chkHTML, string chkID)
        {
            this.chkHTML = chkHTML;
            this.chkID = chkID;
        }

        public string SaveRole()
        {

            SqlParameter[] param = new SqlParameter[9];

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@Profile_Id", SqlDbType.Int);
                param[1].Value = Profile_Id;

                param[2] = new SqlParameter("@Profile_Name", SqlDbType.VarChar);
                param[2].Value = Profile_Name;

                param[3] = new SqlParameter("@Description", SqlDbType.VarChar);
                param[3].Value = Description;

                param[4] = new SqlParameter("@IsAdministrationAccess", SqlDbType.Bit);
                param[4].Value = IsAdministrationAccess;

                param[5] = new SqlParameter("@iParent", SqlDbType.Int);
                param[5].Value = iParent;

                param[6] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[6].Value = iCreatedBy;

                param[7] = new SqlParameter("@Error", SqlDbType.Int);
                param[7].Direction = ParameterDirection.Output;

                param[8] = new SqlParameter("@ifkUserTypeId", SqlDbType.Int);
                param[8].Value = intUserTypeId;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_Role", param);

                return param[7].Value.ToString();
                /*  if (param[7].Value.ToString() == "1")
                  {
                      return  "Save successful";
                  }*/
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsNewRole.cs", "SaveRole()", ex.Message  + ex.StackTrace);
                return "Internal Execution Error !";
            }
        }


        public DataSet GetUserRoleList()
        {
            SqlParameter[] param = new SqlParameter[5];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@iParent", SqlDbType.Int);
                param[1].Value = iParent;

                param[2] = new SqlParameter("@Profile_Id", SqlDbType.Int);
                param[2].Value = Profile_Id;

                param[3] = new SqlParameter("@ifkUserID", SqlDbType.Int);
                param[3].Value = ifkUserID;

                param[4] = new SqlParameter("@ifkUserTypeId", SqlDbType.Int);
                param[4].Value = intUserTypeId; 

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_Role", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsNewRole.cs", "GetUserRoleList()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetInputOutputDevices()
        {
            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@companyId", SqlDbType.Int);
                param[0].Value = iParent;

                param[1] = new SqlParameter("@userId", SqlDbType.Int);
                param[1].Value = ifkUserID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_ViewInputOutputDevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsNewRole.cs", "GetInputOutputDevices()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetOntextOfftextIODevices()
        {
            SqlParameter[] param = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@id", SqlDbType.Int);
                param[0].Value = Id;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_GetOntextOfftextIODevices", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsNewRole.cs", "GetOntextOfftextIODevices()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }


        public List<clsNewRole> SelectClientRole()
        {
            DataSet ds = new DataSet();
            List<clsNewRole> lstClientRole = new List<clsNewRole>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@Profile_Id", SqlDbType.Int);
                param[1].Value = Profile_Id;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_Role", param);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstClientRole.Add(new clsNewRole(Convert.ToInt32(row["Profile_Id"].ToString()),
                                                      row["Profile_Name"].ToString(),
                                                      Convert.ToBoolean(row["IsAdministrationAccess"].ToString())
                                                      ));
                    }
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsNewRole.cs", "SelectSupperAdmin()", ex.Message  + ex.StackTrace);
            }

            return lstClientRole;
        }
        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }


    }
}