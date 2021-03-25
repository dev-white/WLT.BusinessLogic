using System;
using System.Collections.Generic;
using System.Text;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;
using System.Data;
using Microsoft.AspNetCore.SignalR.Client;
using WLT.EntityLayer.Utilities;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WLT.BusinessLogic.BAL
{
   public class Bal_Signal_r_installtions
    {


        public EL_AssetValidator GetValidity(EL_SignalR_Installatation_Parameters _asset)           
        {
            _asset.operation = 1;

            var ds =  DAL_SignalR_Installations.SignalR_Installations(_asset);

            var _EL_AssetValidator = new EL_AssetValidator();


            foreach ( DataTable dt in ds.Tables)     
                foreach (DataRow dr in dt.Rows)
                {
                    _EL_AssetValidator.is_asset_exists = Convert.ToBoolean(dr["asset_exists"]);
                    _EL_AssetValidator.is_device_exists = Convert.ToBoolean(dr["device_exists"]);
                    _EL_AssetValidator.is_installation_exists = Convert.ToBoolean(dr["installation_exists"]);

                    _EL_AssetValidator.EL_WebHook.vInstallationUrl = Convert.ToString(dr["vInstallationUrl"]);
                    _EL_AssetValidator.EL_WebHook.vDe_InstallationUrl = Convert.ToString(dr["vDe_InstallationUrl"]);
                    _EL_AssetValidator.EL_WebHook.ifkResellerId = Convert.ToInt32(dr["ifkResellerId"]);
                    _EL_AssetValidator.EL_WebHook.Criteria =JsonConvert.DeserializeObject<List< int >> (Convert.ToString(dr["vInstallationCriteria"]));
                }


            return _EL_AssetValidator;

        }


        public int CreateAssetSession(EL_SignalR_Installatation_Parameters _asset)
        {
            _asset.operation = 2;

             DAL_SignalR_Installations.SignalR_Installations(_asset);            

            return _asset.session_id;

        }
        public int UpdateAssetSession(EL_SignalR_Installatation_Parameters _asset)
        {
            _asset.operation = 3;

            DAL_SignalR_Installations.SignalR_Installations(_asset);           

            return _asset.AssignedID;

        }

    }

    public interface IClientSignalR
    {
        HubConnection GetConnection();
    }
    public class ClientSignalR: IClientSignalR
    {
             HubConnection connection;

             wlt_Config Configuration { get; set; }

       public ClientSignalR()
        {
            Configuration = AppConfiguration.Configuration();

            connection = new HubConnectionBuilder()
              .WithUrl(Configuration.WebHookUrl)
              .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);

                await connection.StartAsync();
            };

            connection.Reconnected += connectionId =>
            {
                if (connection.State == HubConnectionState.Connected)
                {
                    var dtring = "";
                }

                // Notify users the connection was reestablished.
                // Start dequeuing messages queued while reconnecting if any.

                return Task.CompletedTask;
            };


        }

        public HubConnection GetConnection()
        {                   
            return connection;
        }

    }
}
