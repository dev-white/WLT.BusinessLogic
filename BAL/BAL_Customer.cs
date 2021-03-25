using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
    public class BAL_Customer
    {
        public string FindCustomer(EL_Customer el)
        {
            DataSet ds = new DataSet();
            DAL_Customer dal_Customer = new DAL_Customer();

            ds = dal_Customer.FindCustomer(el);
            
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();

            if (ds.Tables.Count > 0)
            {
                dt1 = ds.Tables[0].Copy();
                dt2 = ds.Tables[1].Copy();
                dt3 = ds.Tables[2].Copy();

                if(ds.Tables.Count == 4)
                {
                    dt4 = ds.Tables[3].Copy();
                }            

            }

            var data = new
            {
                res = dt1,
                min = dt2,
                clt = dt3,
                dt4 = dt4
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);


        }
    }
}
