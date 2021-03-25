using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_Dashboard
    {



        public List<EL_Dashboard> SelectDashboard(EL_Dashboard el_Dashboards)
        {
            DAL_Dashboard dal_Dashboard = new DAL_Dashboard();
            DataSet ds = new DataSet();
            List<EL_Dashboard> list = new List<EL_Dashboard>();

            ds = dal_Dashboard.SelectDashboard(el_Dashboards);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                // list.Add(new EL_Dashboard(0, Convert.ToInt32(dr["xAxis"]), Convert.ToInt32(dr["yAxis"]), Convert.ToInt32(dr["width"]), Convert.ToInt32(dr["height"]), Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["ifkUserID"]), Convert.ToString(dr["widgetId"])));
                list.Add(new EL_Dashboard { DashboardName = Convert.ToString(dr["Dashboard_Name"]), DasboardID = Convert.ToInt32(dr["DashBoard_Id"]) });
            }

            return list;
        }
        public int SaveDashboard(EL_Dashboard el_Dashboards)
        {
            Int32 returnedValue = 0;

            DAL_Dashboard dal_Dashboard = new DAL_Dashboard();

            returnedValue = Convert.ToInt32((dal_Dashboard.SaveDashboard(el_Dashboards).Tables[0].Rows[0]["DashBoard_Id"]));
            if (el_Dashboards.DashboardType == 1)
            {
                foreach (var Widget in el_Dashboards.widgetList)
                {
                    Widget.DasboardID = returnedValue;
                    Widget.Operation = 1;
                    //Widget.width = 1;
                    //Widget.height = 5;

                }
                SaveWidget(el_Dashboards, el_Dashboards.ifkUserID);
            }
            return returnedValue;
        }
        public DataSet UpdateDashboard(EL_Dashboard el_Dashboards)
        {
            DAL_Dashboard dal_Dashboard = new DAL_Dashboard();

            var ds = new DataSet();           
           
            try
            {
                ds = dal_Dashboard.UpateDashboard(el_Dashboards);
                
            }
            catch (Exception ex)
            {
               LogError.RegisterErrorInLogFile( "bal_Dashboard.cs", "UpateDashboard()", ex.Message  + ex.StackTrace);

            }
            return ds;
        }
        public string DeleteDashboard(EL_Dashboard el_Dashboards)
        {
            DAL_Dashboard dal_Dashboard = new DAL_Dashboard();
            string str = string.Empty;
            List<EL_Dashboard> list = new List<EL_Dashboard>();
            try
            {
                str = dal_Dashboard.DeleteDashboard(el_Dashboards);
                str = " Dashboard Deteted Successfully ";
            }
            catch (Exception ex)
            {
                str = "Error Deleting Dashboard";
            }
            return str;
        }
        public string SaveWidget(EL_Dashboard el_Dashboard, int ifkUserID)
        {
        
            DAL_Dashboard dal_Dashboard = new DAL_Dashboard();

            string returnedValue = "-1";

            returnedValue = DeleteWidget(el_Dashboard);


            if (returnedValue == "1")
            {
                foreach (var widget in el_Dashboard.widgetList)
                {
                    returnedValue = dal_Dashboard.SaveWidget(widget, ifkUserID);
                }
            }
            else
            {
                returnedValue = "-1";
            }

            return returnedValue;
        }

        public List<EL_Dashboard> GetWidgets(EL_Dashboard el_Dashboard)
        {
            DAL_Dashboard dal_Dashboard = new DAL_Dashboard();
            DataSet ds = new DataSet();
            List<EL_Dashboard> list = new List<EL_Dashboard>();

            ds = dal_Dashboard.GetWidgets(el_Dashboard);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                // list.Add(new EL_Dashboard(0, Convert.ToInt32(dr["xAxis"]), Convert.ToInt32(dr["yAxis"]), Convert.ToInt32(dr["width"]), Convert.ToInt32(dr["height"]), Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["ifkUserID"]), Convert.ToString(dr["widgetId"])));
                list.Add(new EL_Dashboard { xAxis = Convert.ToInt32(dr["xAxis"]), yAxis = Convert.ToInt32(dr["yAxis"]), width = Convert.ToInt32(dr["width"]), height = Convert.ToInt32(dr["height"]), id = Convert.ToInt32(dr["id"]), ifkUserID = Convert.ToInt32(dr["ifkUserID"]), widgetId = Convert.ToString(dr["widgetId"]).Trim(), DasboardID = Convert.ToInt32(dr["DashBoard_Id"]) });
            }

            return list;
        }

        public string DeleteWidget(EL_Dashboard el_Dashboard)
        {
            DAL_Dashboard dal_Dashboard = new DAL_Dashboard();
            return dal_Dashboard.DeleteWidget(el_Dashboard);
        }

        public int? ResellerDashboardSettings(int _resellerId, bool _enable, int Operation) {
            DAL_Dashboard dal_Dashboard = new DAL_Dashboard();
            var result = dal_Dashboard.ResellerDashboardSettings(_resellerId, _enable, Operation);

            return result;

        }
        public DataSet GetResellerDashboardSettings(int Operation,int ClientCode,int UserType)
        {
            DAL_Dashboard dal_Dashboard = new DAL_Dashboard();
            var result = dal_Dashboard.GetResellerDashboardSettings( Operation, ClientCode, UserType);

            return result;

        }
        
    }
}
