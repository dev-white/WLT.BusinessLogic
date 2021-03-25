using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer.API_Entities;


namespace WLT.BusinessLogic.API_BAL
{
    public class BAL_Asset
    {
        public string CreateAsset(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.CreateAsset(asset);
        }

        public string UpdateAsset(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.UpdateAsset(asset);
        }

        public string DeleteAsset(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.DeleteAsset(asset);
        }

        public string SelectOneAsset(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.SelectOneAsset(asset);
        }

        public string SelectAllAsset(EL_Asset asset)
        {
            string results = "";
            DataSet ds = new DataSet();

            DAL_Asset dAL_Asset = new DAL_Asset();

            ds = dAL_Asset.SelectAllAsset(asset);

            DataTable dt = new DataTable();

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();
            }

            var data = new
            {
                Asset = dt
            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);

            return results;
        }

        public string CreateDevice(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.CreateDevice(asset);
        }

        public string UpdateDevice(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.UpdateDevice(asset);
        }

        public string DeleteDevice(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.DeleteDevice(asset);
        }

        public string SelectOneDevice(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();
            DataSet ds = new DataSet();
            string results = "";

            ds = dAL_Asset.SelectOneDevice(asset);

            DataTable dt = new DataTable();

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();
            }

            var data = new
            {
                Asset = dt
            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);

            return results;
        }

        public string SelectAllDevice(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();
            DataSet ds = new DataSet();
            string results = "";

            ds = dAL_Asset.SelectOneDevice(asset);

            DataTable dt = new DataTable();

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();
            }

            var data = new
            {
                Asset = dt
            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);

            return results;
        }

        public string AssignAsset(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.AssignAsset(asset);
        }

        public string UnAssignAsset(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.UnAssignAsset(asset);
        }

        public string AssignSimCard(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.AssignSimCard(asset);
        }

        public string UnAssignSimCard(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.UnAssignSimCard(asset);
        }

        public string CreateSimcard(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.CreateSimcard(asset);
        }

        public string UpdateSimcard(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.UpdateSimcard(asset);
        }

        public string DeleteSimcard(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.DeleteSimcard(asset);
        }

        public string SelectOneSimcard(EL_Asset asset)
        {

            DAL_Asset dAL_Asset = new DAL_Asset();
            DataSet ds = new DataSet();
            string results = "";

            ds = dAL_Asset.SelectOneSimcard(asset);

            DataTable dt = new DataTable();

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();
            }

            var data = new
            {
                Asset = dt
            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);

            return results;
        }

        public string SelectAllSimcard(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();
            DataSet ds = new DataSet();
            string results = "";

            ds = dAL_Asset.SelectAllSimcard(asset);

            DataTable dt = new DataTable();

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();
            }

            var data = new
            {
                Asset = dt
            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);

            return results;
        }

        public string CreateSingleDevice(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.CreateSingleDevice(asset);
        }

        public string GetAllGroups(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();
            DataSet ds = new DataSet();
            string results = "";

            ds = dAL_Asset.GetAllGroups(asset);

            DataTable dt = new DataTable();

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();
            }

            var data = new
            {
                Groups = dt
            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);

            return results;
        }

        public string GetAssetGroups(EL_Asset asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();
            DataSet ds = new DataSet();
            string results = "";

            ds = dAL_Asset.GetAssetGroups(asset);

            DataTable dt = new DataTable();

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();
            }

            var data = new
            {
                Groups = dt
            };

            results = JsonConvert.SerializeObject(data, Formatting.Indented);

            return results;
        }

        public string ReAssignAssetGroups(List<EL_AssetGroup> assetGroup)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();
            string result = "";

            foreach (var asset in assetGroup)
            {
                result = dAL_Asset.ReAssignAssetGroups(asset);
            }

            return result;
        }

        public string CreateAssetGroups(EL_AssetGroup asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.CreateAssetGroups(asset);
        }
        public string UpdateAssetGroups(EL_AssetGroup asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.UpdateAssetGroups(asset);
        }

        public string DeleteAssetGroups(EL_AssetGroup asset)
        {
            DAL_Asset dAL_Asset = new DAL_Asset();

            return dAL_Asset.DeleteAssetGroups(asset);

        }
    }
}
