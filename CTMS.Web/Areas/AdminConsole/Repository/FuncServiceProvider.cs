using CTMS.Web.Areas.AdminConsole.Interface;
using System.Data.SqlClient;
using System.Data;
using CTMS.Web.Areas.AdminConsole.Models.FunctionMaster;

namespace CTMS.Web.Areas.AdminConsole.Repository
{
    public class FuncServiceProvider : BaseProvider, IFuncServiceProvider
    {
        #region Variable Declaration
        object param = new object();
        int intOutput = 0;

        #endregion

        public FuncServiceProvider(IDataBaseHelper dataBaseHelper) : base(dataBaseHelper)
        {

        }

        public int ActiveFunction(FunctionMasterModel objFunctionMaster)
        {
            try
            {
                object[] objArray = new object[] { "chrActionCode", objFunctionMaster.ActionCode, "intFunctionId", objFunctionMaster.FunctionId, "intCreatedBy", objFunctionMaster.CreatedBy };
                this.intOutput = Convert.ToInt32(SqlHelper.ExecuteNonQuery(DataBaseHelper.ConnectionString, "USP_ADMIN_FUNCTION_MANAGE", out param, objArray));
            }
            catch (Exception exception)
            {
                throw new Exception("Execution Failed", exception);
            }
            return Convert.ToInt32(param);
        }

        public int AddFunction(FunctionMasterModel objFunctionMaster)
        {
            try
            {
                object[] objArray = new object[] {
                                                    "chrActionCode", objFunctionMaster.ActionCode,
                                                    "intFunctionId", objFunctionMaster.FunctionId,
                                                    "vchFunctionName", objFunctionMaster.FunctionName,
                                                    "vchFileName", objFunctionMaster.FileName,
                                                    "vchDescription", objFunctionMaster.Description,
                                                    "vchAction1", objFunctionMaster.FAdd,
                                                    "vchAction2", objFunctionMaster.FView,
                                                    "vchAction3", objFunctionMaster.FManage,
                                                    "bitMailR", objFunctionMaster.MailR,
                                                    "bitFreeBees", objFunctionMaster.FreeBees,
                                                    "vchPortletFile", objFunctionMaster.PortletFile,
                                                     "intCreatedBy", objFunctionMaster.CreatedBy

                                                };
                this.intOutput = SqlHelper.ExecuteNonQuery(DataBaseHelper.ConnectionString, "USP_ADMIN_FUNCTION_MANAGE", out param, objArray);
            }
            catch (Exception exception)
            {
                throw new Exception("Execution Failed", exception);
            }
            return Convert.ToInt32(param);
        }

        public int EditFunction(FunctionMasterModel objFunctionMaster)
        {
            try
            {
                object[] objArray = new object[] {
                                                    "chrActionCode", objFunctionMaster.ActionCode,
                                                    "intFunctionId", objFunctionMaster.FunctionId,
                                                    "vchFunctionName", objFunctionMaster.FunctionName,
                                                    "vchFileName", objFunctionMaster.FileName,
                                                    "vchDescription", objFunctionMaster.Description,
                                                    "vchAction1", objFunctionMaster.FAdd,
                                                    "vchAction2", objFunctionMaster.FView,
                                                    "vchAction3", objFunctionMaster.FManage,
                                                    "bitMailR", objFunctionMaster.MailR,
                                                    "bitFreeBees", objFunctionMaster.FreeBees,
                                                    "vchPortletFile", objFunctionMaster.PortletFile,
                                                     "intCreatedBy", objFunctionMaster.CreatedBy
                                                };
                this.intOutput = SqlHelper.ExecuteNonQuery(DataBaseHelper.ConnectionString, "USP_ADMIN_FUNCTION_MANAGE", out param, objArray);
            }
            catch (Exception exception)
            {
                throw new Exception("Execution Failed", exception);
            }
            return Convert.ToInt32(param);
        }
        public int DeleteFuncImage(string actionCode, int funcId)
        {
            try
            {
                object[] objArray = new object[] {
                                                    "chrActionCode", actionCode,
                                                    "intFunctionId", funcId

                                                 };
                this.intOutput = SqlHelper.ExecuteNonQuery(DataBaseHelper.ConnectionString, "USP_ADMIN_FUNCTION_MANAGE", out param, objArray);
            }
            catch (Exception exception)
            {
                throw new Exception("Execution Failed", exception);
            }
            return Convert.ToInt32(param);
        }
        public IList<FunctionMasterModel> GetAllFunction(FunctionMasterModel objFunctionMaster)
        {
            List<FunctionMasterModel> list = new List<FunctionMasterModel>();
            object[] objArray = new object[] {
                                                "chrActionCode", objFunctionMaster.ActionCode,
                                                "intFunctionId", objFunctionMaster.FunctionId,
                                                "vchFunName",objFunctionMaster.FunctionName,
                                                "intFlag", objFunctionMaster.Flag
                                            };
            DataSet ds = SqlHelper.ExecuteDataset(DataBaseHelper.ConnectionString, "USP_ADMIN_FUNCTION_VIEW", objArray);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
            }

            SqlDataReader reader = SqlHelper.ExecuteReader(DataBaseHelper.ConnectionString, "USP_ADMIN_FUNCTION_VIEW", objArray);
            try
            {

                while (reader.Read())
                {
                    FunctionMasterModel item = new FunctionMasterModel
                    {
                        RowNo = Convert.ToInt32(reader["RowNo"].ToString()),
                        FunctionId = Convert.ToString(reader["intFunctionId"]),
                        FunctionName = Convert.ToString(reader["vchFunction"]),
                        FileName = Convert.ToString(reader["vchFileName"]),
                        Description = Convert.ToString(reader["vchDescription"]),
                        FAdd = Convert.ToString(reader["vchAction1"]),
                        FView = Convert.ToString(reader["vchAction2"]),
                        FManage = Convert.ToString(reader["vchAction3"]),
                        MailR = Convert.ToString(reader["bitMailSend"]),
                        FreeBees = Convert.ToInt32(reader["bitFreebees"]),
                        PortletFile = Convert.ToString(reader["vchportletfile"])
                    };
                    list.Add(item);
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list;
        }

        public IList<FunctionMasterModel> GetFunctionId(string userId)
        {
            List<FunctionMasterModel> list = new List<FunctionMasterModel>();
            string strQry = "select intFunctionId  from m_adm_function where intCreatedBy=" + int.Parse(userId);
            IDataReader reader = (IDataReader)SqlHelper.ExecuteReader(DataBaseHelper.ConnectionString, "usp_GetFunctionIdByUser", "", "userId", int.Parse(userId));
            while (reader.Read())
            {
                FunctionMasterModel item = new FunctionMasterModel
                {
                    FunctionId = reader["intFunctionId"].ToString(),
                };
                list.Add(item);
            }
            reader.Close();
            return list;

        }


        public string GetFunctionData(int intFuncId)
        {
            object[] objArray = new object[] { "chrActionCode", "E", "intFunctionId", intFuncId };
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(DataBaseHelper.ConnectionString, "USP_ADMIN_FUNCTION_VIEW", objArray);
            return ds.GetXml();
        }

        public IList<FunctionMasterModel> GetFunctionDetails(FunctionMasterModel objFunctionMaster)
        {
            List<FunctionMasterModel> list = new List<FunctionMasterModel>();
            object[] objArray = new object[] { "chrActionCode", objFunctionMaster.ActionCode, "intFunctionId", objFunctionMaster.FunctionId, "vchSearchText", objFunctionMaster.Description };
            IDataReader reader = (IDataReader)SqlHelper.ExecuteReader(DataBaseHelper.ConnectionString, "USP_ADMIN_FUNCTION_VIEW", objArray);

            try
            {
                while (reader.Read())
                {
                    FunctionMasterModel item = new FunctionMasterModel
                    {
                        FunctionId = reader["FunctionId"].ToString(),
                        FunctionName = Convert.ToString(reader["vchFunction"]),
                        FileName = Convert.ToString(reader["vchFileName"]),
                        Description = Convert.ToString(reader["vchDescription"]),
                        FAdd = Convert.ToString(reader["vchAdd"]),
                        FView = Convert.ToString(reader["vchView"]),
                        FManage = Convert.ToString(reader["vchManage"]),
                        MailR = Convert.ToString(reader["bitMailSend"]),
                        FreeBees = Convert.ToInt32(reader["bitFreebees"]),
                        PortletFile = Convert.ToString(reader["vchportletfile"])
                    };
                    list.Add(item);
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return list;
        }

    }
}
