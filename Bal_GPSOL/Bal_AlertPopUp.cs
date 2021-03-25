using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WLT.DataAccessLayer.GPSOL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class Bal_AlertPopUp
    {
        public string SetAlertsMessagePreference(EL_AlertPopUp el_AlertPopUp)
        {
            string result = "";

            DAL_AlertPopUp objPopUp = new DAL_AlertPopUp();

            result = objPopUp.SetAlertsMessagePreference(el_AlertPopUp);

            return result;
        }

        public List<EL_AlertPopUp> GetAlertPopUpPreferences(EL_AlertPopUp el_AlertPopUp)
        {
            List<EL_AlertPopUp> list = new List<EL_AlertPopUp>();

            DataSet ds = new DataSet();

            DAL_AlertPopUp objPopUp = new DAL_AlertPopUp();
            ds = objPopUp.GetAlertPopUpPreferences(el_AlertPopUp);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    list.Add(new EL_AlertPopUp { ifkUserId = Convert.ToInt32(dr["ifkUserId"]), MessageType = Convert.ToInt32(dr["MessageType"]), isDisabled = Convert.ToBoolean(dr["isDisabled"]) });
                }

            }

            return list;

        }
    }
}
