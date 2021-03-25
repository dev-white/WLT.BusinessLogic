using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Whitelabeltracking.EntityLayer;
using Whitelabeltracking.DataAccessLayer.DAL;

namespace Whitelabeltracking.BusinessLogic.BAL
{
    public class BAL_UserLogin
    {
        public DataSet GetCompanyDetails(EL_UserLogin p_objELUserLogin)
        {
            DAL_UserLogin _objDALUserLogin = new DAL_UserLogin();
            DataSet ds = new DataSet();

            try
            {
                ds = _objDALUserLogin.GetCompanyDetails(p_objELUserLogin);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _objDALUserLogin = null;
            }

            return ds;
        }

        public EL_UserLogin CheckUserLogin(EL_UserLogin p_objELUserLogin)
        {
            DAL_UserLogin _objDALUserLogin = new DAL_UserLogin();

            try
            {
                p_objELUserLogin = _objDALUserLogin.CheckUserLogin(p_objELUserLogin);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                _objDALUserLogin = null;
            }

            return p_objELUserLogin;
        }

        public void SaveUserLoginLog(EL_UserLogin p_objELUserLogin)
        {
            DAL_UserLogin _objDALUserLogin = new DAL_UserLogin();

            try
            {
                _objDALUserLogin.SaveUserLoginLog(p_objELUserLogin);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _objDALUserLogin = null;
            }
        }
    }
}
