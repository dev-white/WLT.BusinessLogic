using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;
using WLT.EntityLayer.GPSOL;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_AssetSellection
    {

        string f_strConnectionString =AppConfiguration.Getwlt_WebAppConnectionString();
        public string dDeviceName { get; set; }
        public long vpkDeviceID { get; set; }
        public long ifkDeviceID { get; set; }
        public int TrackerAssetType { get; set; }
        public int iReportTypeId
        {
            get; set;
        }


        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int pkUserID { get; set; }
        public int Operation { get; set; }
        public bool isGroupRequet { get; set; }

        public int iTrackerType { get; set; }
        public int CompanyUniqueID { get; set; }
        public int ResellerID { get; set; }
        public bool isAllAssets { get; set; }

        public bool isShowOnAlert { get; set; }


        public bool isSupported { get; set; }

        public int ifkEventID { get; set; }
        
        public string CSVdeviceIDs { get; set; }
        public List<Bal_AssetSellection> Assets { get; set; }



        public string vDigitalName { get; set; }
        public int iDigitalId { get; set; }

        public string JsonDigitalData { get; set; }








        public Bal_AssetSellection()
        {


        }
        public Bal_AssetSellection(string GroupName, int GroupID, List<Bal_AssetSellection> assets)
        {
            this.GroupName = GroupName;
            this.GroupID = GroupID;
            this.Assets = assets;

        }
        public Bal_AssetSellection(List<Bal_AssetSellection> assets)
        {

            this.Assets = assets;

        }
        public Bal_AssetSellection(string GroupName, int GroupID)
        {
            this.GroupName = GroupName;
            this.GroupID = GroupID;


        }

        public string getAssets() {

            var returnstring  = "";

            var assetDs  = FetchAssetsAndGroupsAsPerClient();

            returnstring = JsonConvert.SerializeObject(assetDs, Formatting.Indented);
            return returnstring;
        }

        public string GetResellerAssets()
        {
            var returnstring = "";
            var assetDs = FetchAssetsAndGroupsAsPerReseller();

            returnstring = JsonConvert.SerializeObject(assetDs, Formatting.Indented);

            return returnstring;
        }
        public string GetResllerClients()
        {
            var returnstring = "";

            var assetDs = FetchResellerClients();

            returnstring = JsonConvert.SerializeObject(assetDs, Formatting.Indented);

            return returnstring;
        }

        public string getAssets_()
        {
            // (int Operation, int pkUserID, int CompanyUniqueID,int ResellerID, int iTrackerType )
            var assetsDS = FetchAssetsAndGroupsAsPerClient();

            var _lstObj = new Bal_AssetSellection();


            _lstObj.JsonDigitalData = EnumarateDigitalList(assetsDS.Tables[2]);



            var listofgroups = new List<Bal_AssetSellection>();
            foreach (DataRow row in assetsDS.Tables[1].Rows)
            {
                listofgroups.Add(new Bal_AssetSellection(
                    Convert.ToString(row["vpkGroupName"]),
                    Convert.ToInt32(row["ipkGroupMID"]),
                    assetEnumaration(assetsDS.Tables[0], Convert.ToInt32(row["ipkGroupMID"]))));

            }
            _lstObj.Assets = listofgroups;

            string json = JsonConvert.SerializeObject(_lstObj, Formatting.Indented);
            return json;
        }

        public string  EnumarateDigitalList(DataTable dt)
        {
            var listofDigitalInputs = new List<DigitalInputs>();

            foreach (DataRow dr in dt.Rows)
            {
                listofDigitalInputs.Add(new DigitalInputs
                {
                    vDigitalName = Convert.ToString(dr["vName"]),
                    iDigitalId = Convert.ToInt32(dr["MappingCode"])

                });

            }
            return JsonConvert.SerializeObject(listofDigitalInputs, Formatting.Indented);

        }


        private List<Bal_AssetSellection> assetEnumaration(DataTable assets, int GroupID)
        {
            List<Bal_AssetSellection> list = new List<Bal_AssetSellection>();

            var rows = assets.Select("ifkGroupMID =" + GroupID + "");

            foreach (DataRow datarow in rows)
            {

                list.Add(new Bal_AssetSellection
                {
                    dDeviceName = Convert.ToString(datarow["vDeviceName"]),
                    vpkDeviceID = Convert.ToInt64(datarow["vpkDeviceID"]),
                    ifkDeviceID = Convert.ToInt32(datarow["ifkDeviceID"]),
                    TrackerAssetType = Convert.ToInt32(datarow["iTrackerType"]),
                    iDigitalId = Convert.ToInt32(datarow["MappingCode"]),
                    isSupported = Convert.ToBoolean(datarow["isSupported"]),
                    isShowOnAlert = Convert.ToBoolean(datarow["isShowOnAlert"]),
                    ifkEventID = Convert.ToInt32(datarow["ifkEventID"]),

                });

            }

            return list;
        }
        public string getAssetGroups()
        {
            var assetsDS = FetchAssetsAndGroupsAsPerClient();
            return string.Empty;
        }




        #region  DATA CALL FROM HERE


        public DataSet FetchAssetsAndGroupsAsPerClient()
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;



                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = pkUserID;

                param[2] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[2].Value = CompanyUniqueID;

                param[3] = new SqlParameter("@iParent", SqlDbType.Int);
                param[3].Value = ResellerID;

                param[4] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[4].Value = iTrackerType;


                param[5] = new SqlParameter("@iReportTypeId", SqlDbType.Int);
                param[5].Value = iReportTypeId;

                ds =SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "NewFrontData_sp", param);

            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "registration.cs", "NewFrontData_sp()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        public DataSet FetchAssetsAndGroupsAsPerReseller()
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;



                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = pkUserID;

                param[2] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[2].Value = CompanyUniqueID;

                param[3] = new SqlParameter("@iParent", SqlDbType.Int);
                param[3].Value = ResellerID;

                param[4] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[4].Value = iTrackerType;


                param[5] = new SqlParameter("@iReportTypeId", SqlDbType.Int);
                param[5].Value = iReportTypeId;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "NewFrontData_sp", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("registration.cs", "NewFrontData_sp()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }


        public DataSet FetchResellerClients ()
        {
            DataSet ds = new DataSet();

            SqlParameter[] param = new SqlParameter[6];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;



                param[1] = new SqlParameter("@ipkUserID", SqlDbType.Int);
                param[1].Value = pkUserID;

                param[2] = new SqlParameter("@ifkCompanyID", SqlDbType.Int);
                param[2].Value = CompanyUniqueID;

                param[3] = new SqlParameter("@iParent", SqlDbType.Int);
                param[3].Value = ResellerID;

                param[4] = new SqlParameter("@iTrackerType", SqlDbType.Int);
                param[4].Value = iTrackerType;


                param[5] = new SqlParameter("@iReportTypeId", SqlDbType.Int);
                param[5].Value = iReportTypeId;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "NewFrontData_sp", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("registration.cs", "NewFrontData_sp()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }

        #endregion

    }
    class DigitalInputs
        {

        public string vDigitalName { get; set; }
        public int iDigitalId { get; set; }



    }

}
