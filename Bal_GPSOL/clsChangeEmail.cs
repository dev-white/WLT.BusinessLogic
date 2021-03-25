using System;
using System.Data;
using System.Data.SqlClient;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;
using System.Threading.Tasks;

namespace GPSOL.User.cs
{
    public class clsChangeEmail
    {
        ATPL_CRYPTO.SymmCrypto objCrypto = new ATPL_CRYPTO.SymmCrypto();
        clsChangePassword objCp = new clsChangePassword();


        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();

        #region Properties
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string RequestId { get; set; }
        #endregion

        #region Methods
        public string SaveChangeEmail()
        {
            string result = string.Empty;

            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 1;

                param[1] = new SqlParameter("@RequestId", SqlDbType.NVarChar);
                param[1].Value = RequestId;

                param[2] = new SqlParameter("@email", SqlDbType.NVarChar);
                param[2].Value = Email;

                param[3] = new SqlParameter("@result", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_ChangeEmail", param);

                if (Convert.ToString(param[3].Value) != "-1")
                {

                    result = Convert.ToString(param[3].Value);
                }
                else
                {
                    result = "-1";
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsChangeEmail.cs", "SaveChangeEmail()", ex.Message + ex.StackTrace);
                result = "Internal Execution Error :" + ex.Message;
            }

            return result;
        }

        public int OldEmailExist()
        {
            try
            {

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 2;

                param[1] = new SqlParameter("@RequestId", SqlDbType.NVarChar);
                param[1].Value = RequestId;

                param[2] = new SqlParameter("@result", SqlDbType.VarChar, 100);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_ChangeEmail", param);

                return Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsChangeEmail.cs", "OldEmailExist()", ex.Message + ex.StackTrace);
                return 0;
            }
        }

        public string ChangeEmail()
        {
            string result = String.Empty;
            int checkoldpass = 0;
            try
            {
                checkoldpass = OldEmailExist();

                if (checkoldpass == 2)
                {
                    result = SaveChangeEmail();
                }
                else if (checkoldpass == -2)
                {
                    result = "-2";
                }
                else
                {
                    result = "-1";
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsChangeEmail.cs", "ChangeEmail()", ex.Message + ex.StackTrace);
                result = "Internal execution error in ChangePassword() :" + ex.Message;
            }

            return result;
        }

        public async Task<Tuple<System.Drawing.Color, string>> SendEmailResetLink(string msgBody, string msgSubject)
        {
            System.Drawing.Color color;

            try
            {

                string ismailSent = "";
                ismailSent = await objCp.SendEmailAsync(msgBody, msgSubject);

                if (ismailSent == "false")
                {
                    color = System.Drawing.Color.Red;
                    return new Tuple<System.Drawing.Color, string>(color, "There was a problem sending emails, please try again a bit later.");
                }
                else
                {
                    if (objCp.InsertSessionId() == 3)
                    {
                        color = System.Drawing.Color.Green;
                        //return "Reset link is sent to your inbox, please proceed further on link. ";

                        return new Tuple<System.Drawing.Color, string>(color, "We've sent the reset link to your email address, please check the email inbox or in the spam folder for further instructions. If you didn't get the email, please resend.");
                    }
                    else
                    {
                        color = System.Drawing.Color.Red;

                        return new Tuple<System.Drawing.Color, string>(color, "There is problem with email sending to you, please try again.");
                    }
                }

            }
            catch (Exception ex)
            {
                color = System.Drawing.Color.Red;
                LogError.RegisterErrorInLogFile("clsChangeEmail.cs", "SendEmailResetLink()", ex.Message + ex.StackTrace);

                return new Tuple<System.Drawing.Color, string>(color, "There is error while proccessing your request, please try again, sometime later.");
            }
        }

        public void SendEmailConfirmation(string Email, string EmailNew)
        {

            string msgBody = "";
            string msgSubject = "";

            msgSubject = "Change email confirmation";
            msgBody = "You have successfully changed your email. Your new email is " + EmailNew;

            objCp.Email = Email;
            objCp.EmailNew = EmailNew;

            objCp.SendEmailAsync(msgBody, msgSubject);
        }

        #endregion
    }
}