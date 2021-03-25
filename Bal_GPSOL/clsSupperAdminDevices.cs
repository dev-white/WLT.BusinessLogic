using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class clsSupperAdminDevices : IDisposable
    {
        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();

        private int _Operation;
        private int _ipkDeviceID;
        private string _vDeviceName;
        private string _vManufacturer;
        private bool _bStatus;
        private int _iCreatedBy;
        private bool _bIsConfigurable;
        private int _iDigitalInputs;
        private int _iDigitalOutputs;
        private int _iAnalogInputs;
        private int _DeviceType;
        private bool _IsOtaConfigurable;
        private int _vPort;
        private string _ShortCode;
        private bool _bCellPhoneDevice;
        private bool _bOperatorCode;
        private bool _bUseCellNoAsIMEI;
        private int _vHardwareID;
        private string _ProductLogo;
        private int _ifkManufacturerID;
        private bool _bOneWire;
        private int _SerialInputs;
        private int _BtTemperature;
        private int _BtHumidity;
        private int _BtBattery;
        private int _OdometerUpdateType;
        private bool _ObdStatus;

        public int ipkDeviceID { get { return _ipkDeviceID; } set { _ipkDeviceID = value; } }
        public int vHardwareID { get { return _vHardwareID; } set { _vHardwareID = value; } }
        public string vDeviceName { get { return _vDeviceName; } set { _vDeviceName = value; } }
        public string vManufacturer { get { return _vManufacturer; } set { _vManufacturer = value; } }
        public bool bStatus { get { return _bStatus; } set { _bStatus = value; } }
        public int iCreatedBy { get { return _iCreatedBy; } set { _iCreatedBy = value; } }
        public int Operation { get { return _Operation; } set { _Operation = value; } }
        public bool bIsConfigurable { get { return _bIsConfigurable; } set { _bIsConfigurable = value; } }
        public bool IsOtaConfigurable { get { return _IsOtaConfigurable; } set { _IsOtaConfigurable = value; } }

        public int iDigitalInputs { get { return _iDigitalInputs; } set { _iDigitalInputs = value; } }
        public int iDigitalOutputs { get { return _iDigitalOutputs; } set { _iDigitalOutputs = value; } }
        public int iAnalogInputs { get { return _iAnalogInputs; } set { _iAnalogInputs = value; } }
        public int DeviceType { get { return _DeviceType; } set { _DeviceType = value; } }
        public int vPort { get { return _vPort; } set { _vPort = value; } }
        public string ShortCode { get { return _ShortCode; } set { _ShortCode = value; } }

        public bool bCellPhoneDevice { get { return _bCellPhoneDevice; } set { _bCellPhoneDevice = value; } }
        public bool bOperatorCode { get { return _bOperatorCode; } set { _bOperatorCode = value; } }
        public bool bUseCellNoAsIMEI { get { return _bUseCellNoAsIMEI; } set { _bUseCellNoAsIMEI = value; } }
        public string ProductLogo { get { return _ProductLogo; } set { _ProductLogo = value; } }
        public int ifkManufacturerID { get { return _ifkManufacturerID; } set { _ifkManufacturerID = value; } }
        public bool bOneWire { get { return _bOneWire; } set { _bOneWire = value; } }
        public int SerialInputs { get { return _SerialInputs; } set { _SerialInputs = value; } }

        public int BtTemperature { get { return _BtTemperature; } set { _BtTemperature = value; } }
        public int BtHumidity { get { return _BtHumidity; } set { _BtHumidity = value; } }
        public int BtBattery { get { return _BtBattery; } set { _BtBattery = value; } }
        public int OdometerUpdateType { get { return _OdometerUpdateType; } set { _OdometerUpdateType = value; } }
        public bool ObdStatus { get { return _ObdStatus; } set { _ObdStatus = value; } }

        public clsSupperAdminDevices()
        {
            // initialization constructore
        }

        public clsSupperAdminDevices(int ipkDeviceID, string vDeviceName, int ifkManufacturerID, bool bStatus, bool bIsConfigurable, int iDigitalInputs,
            int iDigitalOutputs, int iAnalogInputs, bool IsOtaConfigurable, int vPort, string ShortCode, bool bCellPhoneDevice, bool bOperatorCode,
            bool bUseCellNoAsIMEI, int vHardwareID, string ProductLogo, int DeviceType, bool bOneWire, int SerialInputs, int OdometerUpdateType)
        {
            this.ipkDeviceID = ipkDeviceID;
            this.vDeviceName = vDeviceName;
            this.ifkManufacturerID = ifkManufacturerID;
            this.bStatus = bStatus;
            this.bIsConfigurable = bIsConfigurable;
            this.iDigitalInputs = iDigitalInputs;
            this.iDigitalOutputs = iDigitalOutputs;
            this.iAnalogInputs = iAnalogInputs;
            this.IsOtaConfigurable = IsOtaConfigurable;
            this.vPort = vPort;
            this.ShortCode = ShortCode;
            this.bCellPhoneDevice = bCellPhoneDevice;
            this.bOperatorCode = bOperatorCode;
            this.bUseCellNoAsIMEI = bUseCellNoAsIMEI;
            this.vHardwareID = vHardwareID;
            this.ProductLogo = ProductLogo;
            this.DeviceType = DeviceType;
            this.bOneWire = bOneWire;
            this.SerialInputs = SerialInputs;
            this.OdometerUpdateType = OdometerUpdateType;
        }

        public string SaveSupperAdminDevice()
        {
            DataSet ds = new DataSet();
            string result = "";
            SqlParameter[] param = new SqlParameter[25];
            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                param[2] = new SqlParameter("@vDeviceName", SqlDbType.NVarChar);
                param[2].Value = vDeviceName;

                param[3] = new SqlParameter("@ifkManufacturerID", SqlDbType.Int);
                param[3].Value = ifkManufacturerID;

                param[4] = new SqlParameter("@bStatus", SqlDbType.Bit);
                param[4].Value = bStatus;

                param[5] = new SqlParameter("@iCreatedBy", SqlDbType.Int);
                param[5].Value = iCreatedBy;

                param[6] = new SqlParameter("@iDigitalInputs", SqlDbType.TinyInt);
                param[6].Value = iDigitalInputs;

                param[7] = new SqlParameter("@iDigitalOutputs", SqlDbType.TinyInt);
                param[7].Value = iDigitalOutputs;

                param[8] = new SqlParameter("@iAnalogInputs", SqlDbType.TinyInt);
                param[8].Value = iAnalogInputs;

                param[9] = new SqlParameter("@Error", SqlDbType.Int);
                param[9].Direction = ParameterDirection.Output;

                param[10] = new SqlParameter("@DeviceType", SqlDbType.TinyInt);
                param[10].Value = DeviceType;

                param[11] = new SqlParameter("@IsOtaConfigurable", SqlDbType.Bit);
                param[11].Value = IsOtaConfigurable;

                param[12] = new SqlParameter("@vPort", SqlDbType.Int);
                param[12].Value = vPort;

                param[13] = new SqlParameter("@ShortCode", SqlDbType.VarChar);
                param[13].Value = ShortCode;

                param[14] = new SqlParameter("@bCellPhoneDevice", SqlDbType.Bit);
                param[14].Value = bCellPhoneDevice;

                param[15] = new SqlParameter("@bOperatorCode", SqlDbType.Bit);
                param[15].Value = bOperatorCode;

                param[16] = new SqlParameter("@bUseCellNoAsIMEI", SqlDbType.Bit);
                param[16].Value = bUseCellNoAsIMEI;

                param[17] = new SqlParameter("@vHardwareID", SqlDbType.Int);
                param[17].Value = vHardwareID;

                param[18] = new SqlParameter("@bOneWire", SqlDbType.Bit);
                param[18].Value = bOneWire;

                param[19] = new SqlParameter("@SerialInputs", SqlDbType.Int);
                param[19].Value = SerialInputs;

                param[20] = new SqlParameter("@OdometerUpdateType", SqlDbType.Int);
                param[20].Value = OdometerUpdateType;

                param[21] = new SqlParameter("@BtTemp", SqlDbType.Int);
                param[21].Value = BtTemperature;

                param[22] = new SqlParameter("@BtHumidity", SqlDbType.Int);
                param[22].Value = BtHumidity;

                param[23] = new SqlParameter("@BtBattery", SqlDbType.Int);
                param[23].Value = BtBattery;

                param[24] = new SqlParameter("@ObdStatus", SqlDbType.Bit);
                param[24].Value = ObdStatus;

                SqlHelper.ExecuteNonQuery(f_strConnectionString, CommandType.StoredProcedure, "Newsp_SupperAdminDevices", param);
                result = param[9].Value.ToString();


                if (param[9].Value.ToString() == "-1")
                {
                    result = "-1";
                }
                else if (param[9].Value.ToString() != "-1" && param[9].Value.ToString() != "-2")
                {
                    result = param[9].Value.ToString();
                }
                else if (param[9].Value.ToString() == "-2")
                {
                    result = "-2";
                }
                else if (param[9].Value.ToString() == "-3")
                {
                    result = "-3";
                }

            }

            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsSupperAdminDevices.cs", "SaveSupperAdminDevice()", ex.Message  + ex.StackTrace);
                result = "Internal Execution Error :" + ex.Message;
            }
            return result;

        }


        public DataSet GetAdminDeviceList()
        {


            SqlParameter[] param = new SqlParameter[2];
            DataSet ds = new DataSet();

            try
            {
                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_SupperAdminDevices", param);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsSupperAdminDevices.cs", "GetAdminDeviceList()", ex.Message  + ex.StackTrace);
            }
            return ds;
        }


        public List<clsSupperAdminDevices> SelectAdminDevice()
        {
            DataSet ds = new DataSet();
            List<clsSupperAdminDevices> lstSupperAdmin = new List<clsSupperAdminDevices>();
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@Operation", SqlDbType.Int);
                param[0].Value = Operation;

                param[1] = new SqlParameter("@ipkDeviceID", SqlDbType.Int);
                param[1].Value = ipkDeviceID;

                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "Newsp_SupperAdminDevices", param);

                string uploadedLogo = "";

                if (ds.Tables.Count > 0)
                {


                    Byte[] logobytes = null;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["ProductLogo"]) != "")
                    {
                        logobytes = (Byte[])ds.Tables[0].Rows[0]["ProductLogo"];
                        string imageBase64 = Convert.ToBase64String(logobytes);
                        uploadedLogo = string.Format("data:image/gif;base64,{0}", imageBase64);

                    }

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstSupperAdmin.Add(new clsSupperAdminDevices
                        {
                            ipkDeviceID = Convert.ToInt32(row["ipkDeviceID"].ToString()),
                            vDeviceName = row["vDeviceName"].ToString(),
                            ifkManufacturerID = Convert.ToInt32(row["ifkManufacturerID"]),
                            bStatus = Convert.ToBoolean(row["bStatus"].ToString()),
                            bIsConfigurable = Convert.ToBoolean(row["bIsConfigurable"].ToString()),
                            iDigitalInputs = Convert.ToInt32(row["iDigitalInputs"].ToString()),
                            iDigitalOutputs = Convert.ToInt32(row["iDigitalOutputs"].ToString()),
                            iAnalogInputs = Convert.ToInt32(row["iAnalogInputs"].ToString()),
                            IsOtaConfigurable = Convert.ToBoolean(row["IsOtaConfigurable"].ToString()),
                            vPort = String.IsNullOrEmpty(Convert.ToString(row["vPort"].ToString())) ? 0 : Convert.ToInt32(row["vPort"].ToString()),
                            ShortCode = row["ShortCode"].ToString(),
                            bCellPhoneDevice = Convert.ToBoolean(row["isCellPhoneDevice"].ToString()),
                            bOperatorCode = Convert.ToBoolean(row["useOperatorCode"].ToString()),
                            bUseCellNoAsIMEI = Convert.ToBoolean(row["useCellNoAsIMEI"].ToString()),
                            vHardwareID = row["HardwareID"].ToString() == "" ? 0 : Convert.ToInt32(row["HardwareID"].ToString()),
                            ProductLogo = uploadedLogo,
                            DeviceType = row["DeviceType"].ToString() == "" ? 3 : Convert.ToInt32(row["DeviceType"]),
                            bOneWire = Convert.ToBoolean(row["b1Wire"].ToString()),
                            SerialInputs = Convert.ToInt32(row["SerialInputs"].ToString()),
                            OdometerUpdateType = Convert.ToInt32(row["OdometerUpdateType"]),
                            BtTemperature = Convert.ToInt32(row["BtTemperature"].ToString()),
                            BtHumidity = Convert.ToInt32(row["BtHumidity"].ToString()),
                            BtBattery = Convert.ToInt32(row["BtBattery"].ToString()),
                            ObdStatus = Convert.ToBoolean(row["ObdStatus"].ToString()),

                        });
                    }
                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsRegistration.cs", "SelectAdminDevice()", ex.Message  + ex.StackTrace);
            }

            return lstSupperAdmin;
        }

        public void Dispose()
        {
            Dispose();
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}