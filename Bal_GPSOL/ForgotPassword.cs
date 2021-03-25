using System;
using System.Data;
using System.Data.SqlClient;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;
using System.Threading.Tasks;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class ForgotPassword
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        public string Email { get; set; }
        public int Operation { get; set; }
        public string RequestId { get; set; }

        private readonly ATPL_CRYPTO.SymmCrypto oED = new ATPL_CRYPTO.SymmCrypto();


        private string Password { get; set; }

        public async Task<Tuple<string, string>> SendEmailWithPasswordAsync()
        {
            string color;

            try
            {

                Password = CreateRandomPassword(8);

                string result = UpdateNewPassword();

                if (result == "22")
                {
                    color = "red";

                    return new Tuple<string, string>(color, "Sorry, this link has already been visited and has now expired");
                }
                else if (result == "-2")
                {
                    color = "red";

                    return new Tuple<string, string>(color, "There is problem while proccesing your request, please try after sometime or contact administrator");
                }
                else if (result != null || result != "")
                {
                    string msgBody = "";
                    msgBody = "<p>We've changed your password to : " + Password + " <br> Make sure you login and change your password by logging into your account, and clicking your name in the top right of the screen</p>";
                    bool ismailSent = false;

                    DataSet ds = new DataSet();

                    Email = result;

                    ds = getSmtpsetting();

                    var config = AppConfiguration.Configuration();

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["SMTPServer"].ToString() != "" && ds.Tables[0].Rows[0]["SMTPPort"].ToString() != "" && ds.Tables[0].Rows[0]["AuthEmail"].ToString() != "" && ds.Tables[0].Rows[0]["AuthPass"].ToString() != "" && (bool)ds.Tables[0].Rows[0]["IsResellerEmailEnabled"] == true)
                        {
                            ismailSent = await WLT.EmailHelper.Mailer.SendMail(ds.Tables[0].Rows[0]["SMTPServer"].ToString(), ds.Tables[0].Rows[0]["AuthEmail"].ToString(),
                              ds.Tables[0].Rows[0]["AuthPass"].ToString(), Convert.ToString(Email), Convert.ToInt32(ds.Tables[0].Rows[0]["SMTPPort"].ToString()),
                              "Password reset link", msgBody, config.Ssl, true, "");
                        }
                        else
                        {
                            ismailSent = await WLT.EmailHelper.Mailer.SendMail(config.SmtpServer, config.FromAddress,
                             config.Password, Convert.ToString(Email), config.SmtpPort,
                             "Password reset link", msgBody, config.Ssl, true, "");
                        }
                    }
                    else
                    {
                        ismailSent = await WLT.EmailHelper.Mailer.SendMail(config.SmtpServer, config.FromAddress,
                            config.Password, Convert.ToString(Email), config.SmtpPort,
                            "Password reset link", msgBody, config.Ssl, true, "");
                    }

                    if (ismailSent == true)
                    {

                        color = "green";

                        return new Tuple<string, string>(color, "We've sent your new password to your email address, <a href=\"/Account/Login\" style=\"color:#55a0ff;\">click here</a> to login");
                    }
                    else
                    {
                        color = "red";

                        return new Tuple<string, string>(color, "There is problem while sending email to you, please try after sometime or contact administrator");
                    }
                }
                else
                {
                    color = "red";

                    return new Tuple<string, string>(color, "There is problem while proccesing your request, please try after sometime or contact administrator");
                }

            }
            catch (Exception ex)
            {
                color = "red";
                LogError.RegisterErrorInLogFile("ForgotPassword.cs", "SendEmailWithPassword()", ex.Message + ex.StackTrace);

                return new Tuple<string, string>(color, "There is problem while proccesing your request, please try after sometime or contact administrator");
            }
        }

        private int UserExist()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 1;

                param[1] = new SqlParameter("@email", SqlDbType.NVarChar);
                param[1].Value = Email; //oED.Decrypting(Email);

                param[2] = new SqlParameter("@result", SqlDbType.Int);
                param[2].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_ForgotPassword", param);

                return Convert.ToInt32(param[2].Value);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("ForgotPassword.cs", "CheckUserExist()", ex.Message + ex.StackTrace);
                return 0;
            }
        }


        private DataSet getSmtpsetting()
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

        private string UpdateNewPassword()
        {
            try
            {
                string EmailID = string.Empty;

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@op", SqlDbType.Int);
                param[0].Value = 2;

                param[1] = new SqlParameter("@result", SqlDbType.Int);
                param[1].Direction = ParameterDirection.Output;

                param[2] = new SqlParameter("@requestid", SqlDbType.NVarChar);
                param[2].Value = RequestId;  //oED.Encrypting(Password);

                param[3] = new SqlParameter("@pass", SqlDbType.NVarChar);
                param[3].Value = oED.Encrypting(Password); //oED.Encrypting(Password);

                EmailID = Convert.ToString(SqlHelper.ExecuteScalar(f_strConnectionString,
                    CommandType.StoredProcedure, "sp_ForgotPassword", param));

                if (EmailID == null || EmailID == "")
                {
                    return EmailID = Convert.ToString(param[1].Value);
                }
                else
                {
                    return EmailID;
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("ForgotPassword.cs", "CheckUserExist()", ex.Message + ex.StackTrace);
                return "-2";
            }
        }


        private int InsertSessionId()
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

        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public async Task<Tuple<string, string>> SendPasswordResetLinkAsync(string url)
        {
            string color;

            try
            {


                if (UserExist() == 1)
                {
                    string msgBody = "";
                    //  msgBody = "We received a request to reset your password, please click this link to confirm you want to reset it <br>" + Convert.ToString(ConfigurationManager.AppSettings["WebsiteURL"]) + "User/ForgotPassword.aspx?requestid=" + RequestId;// +"&emailid=" + Email;
                    msgBody = "We received a request to reset your password, please click this link to confirm you want to reset it <br>" + url + "?requestid=" + RequestId;// +"&emailid=" + Email;                  

                    bool ismailSent = false;

                    DataSet ds = new DataSet();

                    ds = getSmtpsetting();

                    var config = AppConfiguration.Configuration();

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //Use Reseller Settings

                        if (ds.Tables[0].Rows[0]["SMTPServer"].ToString() != "" && ds.Tables[0].Rows[0]["SMTPPort"].ToString() != "" && ds.Tables[0].Rows[0]["AuthEmail"].ToString() != "" && ds.Tables[0].Rows[0]["AuthPass"].ToString() != "" && (bool)ds.Tables[0].Rows[0]["IsResellerEmailEnabled"] == true)
                        {
                            ismailSent = await WLT.EmailHelper.Mailer.SendMail(ds.Tables[0].Rows[0]["SMTPServer"].ToString(), ds.Tables[0].Rows[0]["AuthEmail"].ToString(),
                              ds.Tables[0].Rows[0]["AuthPass"].ToString(), Convert.ToString(Email), Convert.ToInt32(ds.Tables[0].Rows[0]["SMTPPort"].ToString()),
                              "Password reset link", msgBody, config.Ssl, true, "");

                            if (!ismailSent)
                            {
                                //If Email failed - Retry with WLT Settings
                                ismailSent = await WLT.EmailHelper.Mailer.SendMail(config.SmtpServer, config.FromAddress,
                            config.Password, Convert.ToString(Email), config.SmtpPort,
                            "Password reset link", msgBody, config.Ssl, true, "");
                            }
                        }
                        else
                        {
                            ismailSent = await WLT.EmailHelper.Mailer.SendMail(config.SmtpServer, config.FromAddress,
                          config.Password, Convert.ToString(Email), config.SmtpPort,
                          "Password reset link", msgBody, config.Ssl, true, "");
                        }
                    }
                    else
                    {
                        //No Reseller Settings - Use WLT settings
                        ismailSent = await WLT.EmailHelper.Mailer.SendMail(config.SmtpServer, config.FromAddress,
                           config.Password, Convert.ToString(Email), config.SmtpPort,
                           "Password reset link", msgBody, config.Ssl, true, "");
                    }


                    if (ismailSent == false)
                    {
                        color = "red";
                        return new Tuple<string, string>(color, "There was a problem sending emails, please try again a bit later.");
                    }
                    else
                    {
                        if (InsertSessionId() == 3)
                        {
                            color = "green";

                            //return "Reset link is sent to your inbox, please proceed further on link. ";
                            return new Tuple<string, string>(color, "We've sent the reset link to your email address, please check the email inbox or in the spam folder for further instructions. If you didn't get the email, please resend.");
                        }
                        else
                        {
                            color = color = "red";

                            return new Tuple<string, string>(color, "There is problem with email sending to you, please try again.");
                        }
                    }
                }
                else
                {
                    color = "red";

                    return new Tuple<string, string>(color, "An e-mail has been sent to the registered e-mail.");
                }
            }
            catch (Exception ex)
            {
                color = "red";

                LogError.RegisterErrorInLogFile("ForgotPassword.cs", "SendPasswordResetLink()", ex.Message + ex.StackTrace);

                return new Tuple<string, string>(color, "There is error while proccessing your request, please try again, sometime later.");
            }
        }
    }
}
