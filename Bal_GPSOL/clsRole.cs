using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsRole
    {

        private int _Operation = 0;
        private string _Profile_Name = "";
        private string _vipkRoleIDs = "";
        private int _outID = 0;
        private string _Description;
        private string _Error;

        private int _Profile_Id;
        private int _ifkModuleID;
        private bool _bCanView = false;
        private bool _bCanAdd = false;
        private bool _bCanEdit = false;
        private bool _bCanDelete = false;






        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public string Profile_Name { get { return _Profile_Name; } set { _Profile_Name = value; } }
        private int outID { get { return _outID; } set { _outID = value; } }
        public string vipkRoleIDs { get { return _vipkRoleIDs; } set { _vipkRoleIDs = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }
        public string Error { get { return _Error; } set { _Error = value; } }

        public int Profile_Id { get { return _Profile_Id; } set { _Profile_Id = value; } }
        public int Sub_Menu_Id { get { return _ifkModuleID; } set { _ifkModuleID = value; } }
        public bool CanView { get { return _bCanView; } set { _bCanView = value; } }
        public bool bCanAdd { get { return _bCanAdd; } set { _bCanAdd = value; } }
        public bool CanEdit { get { return _bCanEdit; } set { _bCanEdit = value; } }
        public bool CanDelete { get { return _bCanDelete; } set { _bCanDelete = value; } }

        public clsRole()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public clsRole(string Error)
        {
            this.Error = Error;
            //
            // TODO: Add constructor logic here
            //
        }
        public clsRole(string Profile_Name, int Profile_Id)
        {
            this.Profile_Name = Profile_Name;
            this.Profile_Id = Profile_Id;

        }
        public DataSet FillRoleCombo(int id)
        {
            DataSet ds = new DataSet();
            try
            {

               // ds = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString().ToString(), CommandType.Text, "select * from tblRole_Master");
                if (id == 1)
                {
                    ds = SqlHelper.ExecuteDataset(AppConfiguration.Getwlt_WebAppConnectionString().ToString(), CommandType.Text, "SELECT * FROM tblRole_Master rm WHERE Profile_Id in (2,3)");
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(AppConfiguration.Getwlt_WebAppConnectionString().ToString(), CommandType.Text, "SELECT * FROM tblRole_Master rm WHERE Profile_Id = 2");
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsRole.cs", "FillRoleCombo()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }
    }
}