using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer;
using WLT.ErrorLog;
using Microsoft.AspNetCore.Http;
using WLT.EntityLayer;
using System.Threading.Tasks;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsChangePassword : IDisposable
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();

        ATPL_CRYPTO.SymmCrypto objCrypto = new ATPL_CRYPTO.SymmCrypto();

        #region Properties
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string EmailNew { get; set; }
        public string RequestId { get; set; }
        #endregion

        #region Constructors
        public clsChangePassword()
        {
            // login here
        }

        #endregion

        #region Destructor

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Methods

        public string SaveChangePassword()
        {
            string result = string.Empty;

            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 1;

                param[1] = new SqlParameter("@RequestId", SqlDbType.NVarChar);
                param[1].Value = RequestId;

                param[2] = new SqlParameter("@newpass", SqlDbType.NVarChar);
                param[2].Value = objCrypto.Encrypting(NewPassword);

                param[3] = new SqlParameter("@result", SqlDbType.VarChar, 100);
                param[3].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(AppConfiguration.Getwlt_WebAppConnectionString(), CommandType.StoredProcedure, "sp_ChangePassword", param);

                //if (Convert.ToString(param[3].Value) == "-1")
                //{
                //    result = "-1";
                //}
                //else
                //{
                result = Convert.ToString(param[3].Value);
                //}
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsChangePassword.cs", "SaveChangePassword()", ex.Message + ex.StackTrace);
                result = "Internal Execution Error :" + ex.Message;
            }

            return result;
        }

        public int CheckValidLinkPwd()
        {
            try
            {

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 3;

                param[1] = new SqlParameter("@RequestId", SqlDbType.NVarChar);
                param[1].Value = RequestId;

                param[2] = new SqlParameter("@result", SqlDbType.VarChar, 100);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_ChangePassword", param);

                return Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsChangePassword.cs", "OldPasswordExist()", ex.Message + ex.StackTrace);
                return 0;
            }
        }

        public int OldPasswordExist()
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

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_ChangePassword", param);

                return Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsChangePassword.cs", "OldPasswordExist()", ex.Message + ex.StackTrace);
                return 0;
            }
        }

        public string ChangePassword()
        {
            string result = String.Empty;
            int checkoldpass = 0;

            //if (HttpContext.Session.GetObject<clsRegistration>("clsRegistration") == null)
            //{
            //    result = "login";
            //    return result;
            //}
            //else
            //{

            try
            {
                checkoldpass = OldPasswordExist();

                if (checkoldpass == 2)
                {
                    result = SaveChangePassword();
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
                LogError.RegisterErrorInLogFile("ChangeOwercs", "ChangePassword()", ex.Message + ex.StackTrace);
                result = "Internal execution error in ChangePassword() :" + ex.Message;
            }

            return result;
            //}
        }

        public async Task<Tuple<System.Drawing.Color, string>> SendPasswordResetLink(string msgBody, string msgSubject)
        {
            System.Drawing.Color color;

            try
            {

                string ismailSent = "";
                ismailSent = await SendEmailAsync(msgBody, msgSubject);

                if (ismailSent == "false")
                {
                    color = System.Drawing.Color.Red;
                    return new Tuple<System.Drawing.Color, string>(color,"There was a problem sending emails, please try again a bit later.");
                }
                else
                {
                    if (InsertSessionId() == 3)
                    {
                        color = System.Drawing.Color.Green;                    
                     
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
                LogError.RegisterErrorInLogFile("ForgotPassword.cs", "SendPasswordResetLink()", ex.Message + ex.StackTrace);
             
                return new Tuple<System.Drawing.Color, string>(color, "There is error while proccessing your request, please try again, sometime later.");
            }
        }

        public void SendEmailPasswordConfirmation()
        {

            string msgBody = "";
            string msgSubject = "";

            msgSubject = "Change password confirmation";
            msgBody = "You have successfully changed your password. Your new password is " + NewPassword;

            SendEmailAsync(msgBody, msgSubject);
        }

        public async Task<string> SendEmailAsync(string msgBody, string msgSubject)
        {
            bool ismailSent = false;
            try
            {
                DataSet ds = new DataSet();
                ds = getSmtpsetting();

                var config = AppConfiguration.Configuration();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["SMTPServer"].ToString() != "" && ds.Tables[0].Rows[0]["SMTPPort"].ToString() != "" && ds.Tables[0].Rows[0]["AuthEmail"].ToString() != "" && ds.Tables[0].Rows[0]["AuthEmail"].ToString() != "" && (bool)ds.Tables[0].Rows[0]["IsResellerEmailEnabled"] == true)
                    {
                        //Use Reseller settings
                        ismailSent = await WLT.EmailHelper.Mailer.SendMail(ds.Tables[0].Rows[0]["SMTPServer"].ToString(), ds.Tables[0].Rows[0]["AuthEmail"].ToString(),
                          ds.Tables[0].Rows[0]["AuthPass"].ToString(), Convert.ToString(Email), Convert.ToInt32(ds.Tables[0].Rows[0]["SMTPPort"].ToString()),
                          msgSubject, msgBody, config.Ssl, true, "");

                        if (!ismailSent)
                        {
                            //Retry with WLT Settings if Reseller Config didn't work
                            ismailSent = await WLT.EmailHelper.Mailer.SendMail(config.SmtpServer, config.FromAddress,
                               config.Password, Convert.ToString(Email), config.SmtpPort,
                                msgSubject, msgBody, config.Ssl, true, "");
                        }
                    }
                    else
                    {
                        //Use WLT settings
                        ismailSent = await WLT.EmailHelper.Mailer.SendMail(config.SmtpServer, config.FromAddress,
                        config.Password, Convert.ToString(Email), config.SmtpPort,
                         msgSubject, msgBody, config.Ssl, true, "");
                    }
                }
                else
                {
                    //Use WLT settings
                    ismailSent = await WLT.EmailHelper.Mailer.SendMail(config.SmtpServer, config.FromAddress,
                         config.Password, Convert.ToString(Email), config.SmtpPort,
                          msgSubject, msgBody, config.Ssl, true, "");
                }

            }
            catch (Exception ex)
            {
                //color = System.Drawing.Color.Red;
                LogError.RegisterErrorInLogFile("ForgotPassword.cs", "SendPasswordResetLink()", ex.Message + ex.StackTrace);
                return "There is error while proccessing your request, please try again, sometime later. ";
            }
            return ismailSent.ToString();
        }

        public int InsertSessionId()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 3;

                param[1] = new SqlParameter("@email", SqlDbType.NVarChar);
                param[1].Value = Email;

                param[2] = new SqlParameter("@result", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                param[3] = new SqlParameter("@requestid", SqlDbType.NVarChar);
                param[3].Value = RequestId;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_ForgotPassword", param);

                return Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("ForgotPassword.cs", "CheckUserExist()", ex.Message + ex.StackTrace);
                return 0;
            }
        }

        public DataSet getSmtpsetting()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 4;

                param[1] = new SqlParameter("@email", SqlDbType.NVarChar);
                param[1].Value = Email; //oED.Decrypting(Email);

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_ForgotPassword", param);
                return ds;
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("ForgotPassword.cs", "CheckUserExist()", ex.Message + ex.StackTrace);
                return ds;
            }
        }

        #endregion

    }
}
