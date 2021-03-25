using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using WLT.ErrorLog;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsDefaultIOTypes:IDisposable
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        private int _Operation;
        private int _ipkDefaultEvent;
        private string _vEventName;
        private int _iEventCode;
        private bool _bStatus;
        private string _Defaulthtml;
        private string _cmbhtml;
        private string _typeName;
        private int _iUnitID;

        private int _ipkDefaultIOData;
        private string _vName;
        private string _vCmb1Value;
        private string _vCmb2Value;

        private string _vFriendtxtValue1;
        private string _vFriendtxtValue2;

        private int _iFilterCode;
        private int _ifkDeviceID;
        private int _iParent;
        private List<clsDefaultIOTypes> _ArrayDI;
        private List<clsDefaultIOTypes> _ArrayDO;
        private List<clsDefaultIOTypes> _ArrayAI;

        private List<clsDefaultIOTypes> _lstDigitalInput;
        private List<clsDefaultIOTypes> _lstDigitalOutput;
        private List<clsDefaultIOTypes> _lstAnalogInput;

        private string _DIcmbValue1;
        private string _DIcmbValue2;

        private string _DItxtValue1;
        private string _DItxtValue2;

        private string _DOcmbValue1;
        private string _DOcmbValue2;

        private string _DOtxtValue1;
        private string _DOtxtValue2;

        private string _AIcmbValue1;
        private string _AIcmbValue2;

        private string _AItxtValue1;
        private string _AItxtValue2;

        private string _DIText;
        private string _DOText;
        private string _AIText;

        private string _dihtml;
        private string _dohtml;
        private string _aihtml; 
        private string _dino;
        private string _dono;
        private string _aino;

        private int _Action;
                
        private int _ifk_IO_Digital_Master_ID;
        private int _bInverted;
        private int _bIsDigitalInput;
        private int _bOutputPulsed;
        private string _iImeiNumber;
        private int _id;

        
        private int _ifk_IO_Analog_Master_ID;
        private float _iMinVolts;
        private float _iMaxVolts;
        private float _iMinValue;
        private float _iMaxValue;
        private string _strConnection;

        public int ifk_IO_Analog_Master_ID { get { return _ifk_IO_Analog_Master_ID; } set { _ifk_IO_Analog_Master_ID = value; } }
        public float iMinVolts { get { return _iMinVolts; } set { _iMinVolts = value; } }
        public float iMaxVolts { get { return _iMaxVolts; } set { _iMaxVolts = value; } }
        public float iMinValue { get { return _iMinValue; } set { _iMinValue = value; } }
        public float iMaxValue { get { return _iMaxValue; } set { _iMaxValue = value; } }

        public int id { get { return _id; } set { _id = value; } }
        public int ifk_IO_Digital_Master_ID { get { return _ifk_IO_Digital_Master_ID; } set { _ifk_IO_Digital_Master_ID = value; } }
        public int bInverted { get { return _bInverted; } set { _bInverted = value; } }
        public int bIsDigitalInput { get { return _bIsDigitalInput; } set { _bIsDigitalInput = value; } }
        public int bOutputPulsed { get { return _bOutputPulsed; } set { _bOutputPulsed = value; } }
        public string iImeiNumber { get { return _iImeiNumber; } set { _iImeiNumber = value; } }

        public int Action { get { return _Action; } set { _Action = value; } }
        public string dihtml { get { return _dihtml; } set { _dihtml = value; } }
        public string dohtml { get { return _dohtml; } set { _dohtml = value; } }
        public string aihtml { get { return _aihtml; } set { _aihtml = value; } }
        public string dino { get { return _dino; } set { _dino = value; } }
        public string dono { get { return _dono; } set { _dono = value; } }
        public string aino { get { return _aino; } set { _aino = value; } }


        public List<clsDefaultIOTypes> ArrayDI { get { return _ArrayDI; } set { _ArrayDI = value; } }
        public List<clsDefaultIOTypes> ArrayDO { get { return _ArrayDO; } set { _ArrayDO = value; } }
        public List<clsDefaultIOTypes> ArrayAI { get { return _ArrayAI; } set { _ArrayAI = value; } }
              
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public int ipkDefaultEvent { get { return _ipkDefaultEvent; } set { _ipkDefaultEvent = value; } }
        public string vEventName { get { return _vEventName; } set { _vEventName = value; } }
        public int iEventCode { get { return _iEventCode; } set { _iEventCode = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public string Defaulthtml { get { return _Defaulthtml; } set { _Defaulthtml = value; } }
        public string cmbhtml { get { return _cmbhtml; } set { _cmbhtml = value; } }
        public int ipkDefaultIOData { get { return _ipkDefaultIOData; } set { _ipkDefaultIOData = value; } }
        public string vName { get { return _vName; } set { _vName = value; } }
        public string vCmb1Value { get { return _vCmb1Value; } set { _vCmb1Value = value; } }
        public string vCmb2Value { get { return _vCmb2Value; } set { _vCmb2Value = value; } }

        public string vFriendtxtValue1 { get { return _vFriendtxtValue1; } set { _vFriendtxtValue1 = value; } }
        public string vFriendtxtValue2 { get { return _vFriendtxtValue2; } set { _vFriendtxtValue2 = value; } }

        public int iFilterCode { get { return _iFilterCode; } set { _iFilterCode = value; } }
        public int ifkDeviceID { get { return _ifkDeviceID; } set { _ifkDeviceID = value; } }
        public int iParent { get { return _iParent; } set { _iParent = value; } }
        public int iUnitID { get { return _iUnitID; } set { _iUnitID = value; } }

        public List<clsDefaultIOTypes> lstDigitalInput { get { return _lstDigitalInput; } set { _lstDigitalInput = value; } }
        public List<clsDefaultIOTypes> lstDigitalOutput { get { return _lstDigitalOutput; } set { _lstDigitalOutput = value; } }
        public List<clsDefaultIOTypes> lstAnalogInput { get { return _lstAnalogInput; } set { _lstAnalogInput = value; } }

        public string DIcmbValue1 { get { return _DIcmbValue1; } set { _DIcmbValue1 = value; } }
        public string DIcmbValue2 { get { return _DIcmbValue2; } set { _DIcmbValue2 = value; } }

        public string DItxtValue1 { get { return _DItxtValue1; } set { _DItxtValue1 = value; } }
        public string DItxtValue2 { get { return _DItxtValue2; } set { _DItxtValue2 = value; } }
        
        public string DOcmbValue1 { get { return _DOcmbValue1; } set { _DOcmbValue1 = value; } }
        public string DOcmbValue2 { get { return _DOcmbValue2; } set { _DOcmbValue2 = value; } }

        public string DOtxtValue1 { get { return _DOtxtValue1; } set { _DOtxtValue1 = value; } }
        public string DOtxtValue2 { get { return _DOtxtValue2; } set { _DOtxtValue2 = value; } }

        public string AIcmbValue1 { get { return _AIcmbValue1; } set { _AIcmbValue1 = value; } }
        public string AIcmbValue2 { get { return _AIcmbValue2; } set { _AIcmbValue2 = value; } }

        public string AItxtValue1 { get { return _AItxtValue1; } set { _AItxtValue1 = value; } }
        public string AItxtValue2 { get { return _AItxtValue2; } set { _AItxtValue2 = value; } }

        public string DIText { get { return _DIText; } set { _DIText = value; } }
        public string DOText { get { return _DOText; } set { _DOText = value; } }
        public string AIText { get { return _AIText; } set { _AIText = value; } }

        public string typeName { get { return _typeName; } set { _typeName = value; } }

        public string strConnection { get { return _strConnection; } set { _strConnection = value; } }
        

        public clsDefaultIOTypes()
        {
            // initialization constructore
        }

        public clsDefaultIOTypes(string Defaulthtml, string cmbhtml,List<clsDefaultIOTypes> ArrayDI, List<clsDefaultIOTypes> ArrayDO, List<clsDefaultIOTypes> ArrayAI)
        {
            this.Defaulthtml = Defaulthtml;
            this.cmbhtml = cmbhtml;
            this.ArrayDI = ArrayDI;
            this.ArrayDO = ArrayDO;
            this.ArrayAI = ArrayAI;

        }

        public clsDefaultIOTypes(string vCmb1Value, string vCmb2Value, string typeName)
        {
            this.vCmb1Value = vCmb1Value;
            this.vCmb2Value = vCmb2Value;
            this.typeName = typeName;
          
        }

        public clsDefaultIOTypes(string Defaulthtml, string cmbhtml)
        {
            this.Defaulthtml = Defaulthtml;
            this.cmbhtml = cmbhtml;

        }

        public clsDefaultIOTypes(string dihtml, string dohtml, string aihtml, string dino, string dono, string aino)
        {            
            this.dihtml = dihtml;
            this.dohtml = dohtml;
            this.aihtml = aihtml;
            this.dino = dino;
            this.dono = dono;
            this.aino = aino;
        }

        public DataSet GetDefaultIOTypes()
        {

            SqlParameter[] param = new SqlParameter[5];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDefaultEvent", SqlDbType.Int);
                param[1].Value = ipkDefaultEvent;

                param[2] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[2].Value = ifkDeviceID;

                param[3] = new SqlParameter("@iParent", SqlDbType.Int);
                param[3].Value = iParent;

                param[4] = new SqlParameter("@iFilterCode", SqlDbType.Int);
                param[4].Value = iFilterCode;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_DefaultIOTypes", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsDefaultIOTypes.cs", "GetDefaultIOTypes()", ex.Message  + ex.StackTrace);
            }
            return ds;


        }

        public DataSet GetInputOutputDevicesDropdowns()
        {

            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@ifkCompanyId", SqlDbType.BigInt);
                param[0].Value = iParent;

                param[1] = new SqlParameter("@iUnitID", SqlDbType.BigInt);
                param[1].Value = iUnitID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Get_InputOutputDevicesDropdowns", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsDefaultIOTypes.cs", "GetInputOutputDevicesDropdowns()", ex.Message  + ex.StackTrace);
            }
            return ds;


        }

        public string SaveDefaultIOTypes()
        {
            SqlParameter[] param = new SqlParameter[10];
            DataSet ds = new DataSet();
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDefaultIOData", SqlDbType.Int);
                param[1].Value = ipkDefaultIOData;

                param[2] = new SqlParameter("@iFilterCode", SqlDbType.Int);
                param[2].Value = iFilterCode;
                
                param[3] = new SqlParameter("@vCmb1Value", SqlDbType.VarChar);
                param[3].Value = vCmb1Value;
                
                param[4] = new SqlParameter("@vCmb2Value", SqlDbType.VarChar);
                param[4].Value = vCmb2Value;

                param[5] = new SqlParameter("@vName", SqlDbType.VarChar);
                param[5].Value = vName;

                param[6] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[6].Value = ifkDeviceID;

                param[7] = new SqlParameter("@iParent", SqlDbType.Int);
                param[7].Value = iParent;

                param[8] = new SqlParameter("@vFriendlyName1", SqlDbType.VarChar);
                param[8].Value = vFriendtxtValue1;

                param[9] = new SqlParameter("@vFriendlyName2", SqlDbType.VarChar);
                param[9].Value = vFriendtxtValue2;
                


                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure,"Newsp_DefaultIOTypes", param);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsDefaultIOTypes.cs", "SaveDefaultIOTypes()", ex.Message  + ex.StackTrace);
            }
            return "";
            
        }

        public string AddDigitalInputOutputDevice()
        {
            SqlParameter[] param = new SqlParameter[7];
            DataSet ds = new DataSet();
            string _strReturn = "";
            try
            {
                param[0] = new SqlParameter("@ifk_IO_Digital_Master_ID", SqlDbType.BigInt);
                param[0].Value = ifk_IO_Digital_Master_ID;

                param[1] = new SqlParameter("@bInverted", SqlDbType.Bit);
                param[1].Value = bInverted;

                param[2] = new SqlParameter("@bIsDigitalInput", SqlDbType.Bit);
                param[2].Value = bIsDigitalInput;

                param[3] = new SqlParameter("@bOutputPulsed", SqlDbType.Bit);
                param[3].Value = bOutputPulsed;

                param[4] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[4].Value = ifkDeviceID;

                param[5] = new SqlParameter("@connection", SqlDbType.VarChar, 100);
                param[5].Value = strConnection;

                param[6] = new SqlParameter("@intReturn", SqlDbType.VarChar);
                param[6].Direction = ParameterDirection.ReturnValue;

                _strReturn = Convert.ToString(SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_AddDigitalInputOutputDevice", param));
                
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsDefaultIOTypes.cs", "SaveDigitalInputOutputDevice()", ex.Message  + ex.StackTrace);
            }
            return _strReturn;

        }

        public string UpdateDigitalInputOutputDevice()
        {
            SqlParameter[] param = new SqlParameter[8];
            DataSet ds = new DataSet();
            string _strReturn = "";
            try
            {
                param[0] = new SqlParameter("@ifk_IO_Digital_Master_ID", SqlDbType.BigInt);
                param[0].Value = ifk_IO_Digital_Master_ID;

                param[1] = new SqlParameter("@bInverted", SqlDbType.Bit);
                param[1].Value = bInverted;

                param[2] = new SqlParameter("@bIsDigitalInput", SqlDbType.Bit);
                param[2].Value = bIsDigitalInput;

                param[3] = new SqlParameter("@bOutputPulsed", SqlDbType.Bit);
                param[3].Value = bOutputPulsed;

                param[4] = new SqlParameter("@ifkDeviceID", SqlDbType.Int);
                param[4].Value = ifkDeviceID;

                param[5] = new SqlParameter("@id", SqlDbType.BigInt);
                param[5].Value = id;

                param[6] = new SqlParameter("@connection", SqlDbType.VarChar, 100);
                param[6].Value = strConnection;

                param[7] = new SqlParameter("@intReturn", SqlDbType.VarChar);
                param[7].Direction = ParameterDirection.ReturnValue;

                _strReturn = Convert.ToString(SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_UpdateDigitalInputOutputDevice", param));

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsDefaultIOTypes.cs", "SaveDigitalInputOutputDevice()", ex.Message  + ex.StackTrace);
            }
            return _strReturn;

        }

        public string AddAnalogInputOutputDevice()
        {
            SqlParameter[] param = new SqlParameter[8];
            DataSet ds = new DataSet();
            string _strReturn = "";
            try
            {
                param[0] = new SqlParameter("@ifk_IO_Analog_Master_ID", SqlDbType.BigInt);
                param[0].Value = ifk_IO_Analog_Master_ID;

                param[1] = new SqlParameter("@iMinVolts", SqlDbType.Float);
                param[1].Value = iMinVolts;

                param[2] = new SqlParameter("@iMaxVolts", SqlDbType.Float);
                param[2].Value = iMaxVolts;

                param[3] = new SqlParameter("@iMinValue", SqlDbType.Float);
                param[3].Value = iMinValue;

                param[4] = new SqlParameter("@iMaxValue", SqlDbType.Float);
                param[4].Value = iMaxValue;

                param[5] = new SqlParameter("@iImeiNumber", SqlDbType.BigInt);
                param[5].Value = iImeiNumber;

                param[6] = new SqlParameter("@connection", SqlDbType.VarChar, 100);
                param[6].Value = strConnection;

                param[7] = new SqlParameter("@intReturn", SqlDbType.VarChar);
                param[7].Direction = ParameterDirection.ReturnValue;

                _strReturn = Convert.ToString(SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_AddAnalogTempInputOutputDevice", param));

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsDefaultIOTypes.cs", "SaveDigitalInputOutputDevice()", ex.Message  + ex.StackTrace);
            }
            return _strReturn;

        }

        public string UpdateAnalogInputOutputDevice()
        {
            SqlParameter[] param = new SqlParameter[9];
            DataSet ds = new DataSet();
            string _strReturn = "";
            try
            {
                param[0] = new SqlParameter("@ifk_IO_Analog_Master_ID", SqlDbType.BigInt);
                param[0].Value = ifk_IO_Analog_Master_ID;

                param[1] = new SqlParameter("@iMinVolts", SqlDbType.Float);
                param[1].Value = iMinVolts;

                param[2] = new SqlParameter("@iMaxVolts", SqlDbType.Float);
                param[2].Value = iMaxVolts;

                param[3] = new SqlParameter("@iMinValue", SqlDbType.Float);
                param[3].Value = iMinValue;

                param[4] = new SqlParameter("@iMaxValue", SqlDbType.Float);
                param[4].Value = iMaxValue;

                param[5] = new SqlParameter("@iImeiNumber", SqlDbType.BigInt);
                param[5].Value = iImeiNumber;

                param[6] = new SqlParameter("@id", SqlDbType.BigInt);
                param[6].Value = id;

                param[7] = new SqlParameter("@connection", SqlDbType.VarChar, 100);
                param[7].Value = strConnection;

                param[8] = new SqlParameter("@intReturn", SqlDbType.VarChar);
                param[8].Direction = ParameterDirection.ReturnValue;

                _strReturn = Convert.ToString(SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "sp_UpdateAnalogTempInputOutputDevice", param));

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsDefaultIOTypes.cs", "SaveDigitalInputOutputDevice()", ex.Message  + ex.StackTrace);
            }
            return _strReturn;

        }

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}
