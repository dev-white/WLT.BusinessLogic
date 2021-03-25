using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class DynamicPaging
    {
        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();

        private int _PageSize;
        private int _PageNumber;
        private string _SearchField;
        private string _SearchText;
        private string _OrderField;
        private string _OrderText;
        private string _FieldName;
        private string _PrimryField;
        private string _TableName;
        private string _whereCondition;
        private string _AlterFieldName;

        private string _StockPrice;
        private string _SearchPrice;
        private string _SearchAvailable;
        private string _StockAvailability;

        private string _SearchComapany;
        private string _ExistComapany;


        public string SearchComapany
        {
            get { return _SearchComapany; }
            set { _SearchComapany = value; }
        }
        public string ExistComapany
        {
            get { return _ExistComapany; }
            set { _ExistComapany = value; }
        }

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        public int PageNumber
        {
            get { return _PageNumber; }
            set { _PageNumber = value; }
        }
        public string SearchField
        {
            get { return _SearchField; }
            set { _SearchField = value; }
        }
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; }
        }
        public string OrderField
        {
            get { return _OrderField; }
            set { _OrderField = value; }
        }
        public string OrderText
        {
            get { return _OrderText; }
            set { _OrderText = value; }
        }
        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }
        public string PrimryField
        {
            get { return _PrimryField; }
            set { _PrimryField = value; }
        }
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        public string whereCondition
        {
            get { return _whereCondition; }
            set { _whereCondition = value; }
        }
        public string AlterFieldName
        {
            get { return _AlterFieldName; }
            set { _AlterFieldName = value; }
        }

        public string SearchAvailable
        {
            get { return _SearchAvailable; }
            set { _SearchAvailable = value; }
        }

        public string SearchPrice
        {
            get { return _SearchPrice; }
            set { _SearchPrice = value; }
        }

        public string StockAvailability
        {
            get { return _StockAvailability; }
            set { _StockAvailability = value; }
        }

        public string StockPrice
        {
            get { return _StockPrice; }
            set { _StockPrice = value; }
        }


        public DynamicPaging()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet GetTables()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[17];

                param[0] = new SqlParameter("@PageSize", SqlDbType.Int);
                param[0].Value = PageSize;

                param[1] = new SqlParameter("@PageNumber", SqlDbType.Int);
                param[1].Value = PageNumber;

                param[2] = new SqlParameter("@SearchField", SqlDbType.VarChar, 50);
                param[2].Value = SearchField;

                param[3] = new SqlParameter("@SearchText", SqlDbType.VarChar, 50);
                param[3].Value = SearchText;

                param[4] = new SqlParameter("@OrderField", SqlDbType.VarChar, 50);
                param[4].Value = OrderField;

                param[5] = new SqlParameter("@OrderText", SqlDbType.VarChar, 50);
                param[5].Value = OrderText;

                param[6] = new SqlParameter("@FieldName", SqlDbType.VarChar, 4000);
                param[6].Value = FieldName;

                param[7] = new SqlParameter("@PrimryField", SqlDbType.VarChar, 50);
                param[7].Value = PrimryField;

                param[8] = new SqlParameter("@TableName", SqlDbType.VarChar, 200);
                param[8].Value = TableName;

                param[9] = new SqlParameter("@whereCondition", SqlDbType.VarChar, 1000);
                param[9].Value = whereCondition;

                param[10] = new SqlParameter("@AlterFieldName", SqlDbType.VarChar);
                param[10].Value = AlterFieldName;

                param[11] = new SqlParameter("@SearchAvailable", SqlDbType.VarChar);
                param[11].Value = SearchAvailable;

                param[12] = new SqlParameter("@SearchPrice", SqlDbType.VarChar);
                param[12].Value = SearchPrice;

                param[13] = new SqlParameter("@StockAvailability", SqlDbType.VarChar);
                param[13].Value = StockAvailability;

                param[14] = new SqlParameter("@StockPrice", SqlDbType.VarChar);
                param[14].Value = StockPrice;

                param[15] = new SqlParameter("@SearchCompany", SqlDbType.VarChar);
                param[15].Value = SearchComapany;
                param[16] = new SqlParameter("@ExistsCompany", SqlDbType.VarChar);
                param[16].Value = ExistComapany;


                //param[9] = new SqlParameter("@Error", SqlDbType.TinyInt);
                //param[9].Direction = ParameterDirection.Output;

                return SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "DynamicPaging_NEW", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "DynamicPaging.cs", "GetTables()", ex.Message  + ex.StackTrace);

                return null;
            }


        }

        public DataSet GetTablesWithoutPageSize()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[11];

                param[0] = new SqlParameter("@PageSize", SqlDbType.Int);
                param[0].Value = 0;

                param[1] = new SqlParameter("@PageNumber", SqlDbType.Int);
                param[1].Value = PageNumber;

                param[2] = new SqlParameter("@SearchField", SqlDbType.VarChar, 50);
                param[2].Value = SearchField;

                param[3] = new SqlParameter("@SearchText", SqlDbType.VarChar, 50);
                param[3].Value = SearchText;

                param[4] = new SqlParameter("@OrderField", SqlDbType.VarChar, 50);
                param[4].Value = OrderField;

                param[5] = new SqlParameter("@OrderText", SqlDbType.VarChar, 50);
                param[5].Value = OrderText;

                param[6] = new SqlParameter("@FieldName", SqlDbType.VarChar, 4000);
                param[6].Value = FieldName;

                param[7] = new SqlParameter("@PrimryField", SqlDbType.VarChar, 50);
                param[7].Value = PrimryField;

                param[8] = new SqlParameter("@TableName", SqlDbType.VarChar, 100);
                param[8].Value = TableName;

                param[9] = new SqlParameter("@whereCondition", SqlDbType.VarChar, 1000);
                param[9].Value = whereCondition;

                param[10] = new SqlParameter("@AlterFieldName", SqlDbType.VarChar);
                param[10].Value = AlterFieldName;

                //param[9] = new SqlParameter("@Error", SqlDbType.TinyInt);
                //param[9].Direction = ParameterDirection.Output;

                return SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "DynamicPaging", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "DynamicPaging.cs", "GetTablesWithoutPageSize()", ex.Message  + ex.StackTrace);

                return null;
            }


        }
    }
}