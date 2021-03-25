using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Whitelabeltracking.EntityLayer;
using Whitelabeltracking.DataAccessLayer.DAL;
using WLT.ErrorLog;

namespace Whitelabeltracking.BusinessLogic.BAL
{
    public class BAL_WLTMaster
    {
        public DataSet GetRegistrationDetails(EL_WLTMaster p_objELWLTMaster)
        {
            DAL_WLTMaster _objDALWLTMaster = new DAL_WLTMaster();
            DataSet ds = new DataSet();
            try
            {
                ds = _objDALWLTMaster.GetRegistrationDetails(p_objELWLTMaster);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "BAL_WLTMaster.cs", "GetRegistrationDetails()", ex.Message  + ex.StackTrace);
                
            }
            finally
            {
                _objDALWLTMaster = null;
            }

            return ds;
        }
    }
}
