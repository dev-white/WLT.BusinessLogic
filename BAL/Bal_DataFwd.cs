using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_DataFwd
    {
        public string SaveDataFwdProvider(EL_DataFwd el)
        {
            string result = "";
            DAL_DataFwd dal = new DAL_DataFwd();
            result = dal.SaveDataFwdProvider(el);
            return result;
        }

        public DataSet GetDataFwdProviders(EL_DataFwd el)
        {
            DataSet ds = new DataSet();

            DAL_DataFwd dal = new DAL_DataFwd();
            ds = dal.GetDataFwdProviders(el);
            return ds;
        }

        public string DeleteDataFwdProvider(EL_DataFwd el)
        {
            string result;

            DAL_DataFwd dal = new DAL_DataFwd();
            result = dal.DeleteDataFwdProvider(el);
            return result;
        }

        public DataSet GetClients(EL_DataFwd el)
        {
            DataSet ds = new DataSet();

            DAL_DataFwd dal = new DAL_DataFwd();
            ds = dal.GetClients(el);
            return ds;
        }

        public DataSet GetFwdAssets(EL_DataFwd el)
        {
            DataSet ds = new DataSet();

            DAL_DataFwd dal = new DAL_DataFwd();
            ds = dal.GetFwdAssets(el);
            return ds;
        }
        public string SaveDataFwdDevices(EL_DataFwd el)
        {
            string result;

            DAL_DataFwd dal = new DAL_DataFwd();
            result = dal.SaveDataFwdDevices(el);
            return result;
        }
        

    }
}
