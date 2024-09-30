using Dapper;

using CTMS.Web.Areas.AdminConsole.Interface;

using System.Data;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Web.Areas.AdminConsole.Models.FunctionMaster;

namespace CTMS.Web.Areas.AdminConsole.Repository
{
    public class FuncServiceProviderNew : CTMSRepositoryBase, IFuncServiceProvider
    {

        #region Variable Declaration
        object param = new object();

        #endregion
        private readonly IConfiguration _configuration;
        public FuncServiceProviderNew(ICTMSConnectionFactory EDODISHAconnectionFactory, IConfiguration configuration) : base(EDODISHAconnectionFactory)
        {

        }
        
        public int ActiveFunction(FunctionMasterModel objFunctionMaster)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@mvarchrActionCode", objFunctionMaster.ActionCode);
                p.Add("@mvarintFunctionId", objFunctionMaster.FunctionId);
                p.Add("@mvarintCreatedBy", objFunctionMaster.CreatedBy);
                p.Add("@mvarvchOutPut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_FUNCTION_MANAGE";
                var result = Connection.Query<SuccessMessage>(query, p, commandType: CommandType.StoredProcedure);
                return result.AsList()[0].successid;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public int AddFunction(FunctionMasterModel objFunctionMaster)
        {
            try
            {
                var aParam = new DynamicParameters();
                aParam.Add("@mvarchrActionCode", objFunctionMaster.ActionCode);
                aParam.Add("@mvarvchFunctionName", objFunctionMaster.FunctionName.Trim());
                aParam.Add("@mvarvchFileName", objFunctionMaster.FileName.Trim());
                aParam.Add("@mvarvchDescription", objFunctionMaster.Description);
                aParam.Add("@mvarvchAction1", objFunctionMaster.FAdd);
                aParam.Add("@mvarvchAction2", objFunctionMaster.FView);
                aParam.Add("@mvarvchAction3", objFunctionMaster.FManage);
                aParam.Add("@mvarbitMailR", objFunctionMaster.MailR);
                aParam.Add("@mvarbitFreeBees", objFunctionMaster.FreeBees);
                aParam.Add("@mvarvchPortletFile", objFunctionMaster.PortletFile);
                aParam.Add("@mvarvchOutPut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_FUNCTION_MANAGE";
                var result = Connection.Query<SuccessMessage>(query, aParam, commandType: CommandType.StoredProcedure);
                return result.AsList()[0].successid;

            }
            catch (Exception exception)
            {
                throw new Exception("Execution Failed", exception);
            }
        }

        public int DeleteFuncImage(string actionCode, int funcId)
        {
            try
            {
                var aParam = new DynamicParameters();
                aParam.Add("mvarchrActionCode", actionCode);
                aParam.Add("mvarintFunctionId", funcId);
                aParam.Add("mvarvchOutPut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                aParam.Add("P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_FUNCTION_MANAGE";
                var results = Connection.Execute(query, aParam, commandType: CommandType.StoredProcedure);
                return results;

            }
            catch (Exception exception)
            {
                throw new Exception("Execution Failed", exception);
            }
        }

        public int EditFunction(FunctionMasterModel objFunctionMaster)
        {
            try
            {
                var aParam = new DynamicParameters();
                aParam.Add("@mvarchrActionCode", objFunctionMaster.ActionCode);
                aParam.Add("@mvarintFunctionId", objFunctionMaster.FunctionId);
                aParam.Add("@mvarvchFunctionName", objFunctionMaster.FunctionName.Trim());
                aParam.Add("@mvarvchFileName", objFunctionMaster.FileName.Trim());
                aParam.Add("@mvarvchDescription", objFunctionMaster.Description);
                aParam.Add("@mvarvchAction1", objFunctionMaster.FAdd);
                aParam.Add("@mvarvchAction2", objFunctionMaster.FView);
                aParam.Add("@mvarvchAction3", objFunctionMaster.FManage);
                aParam.Add("@mvarbitMailR", objFunctionMaster.MailR);
                aParam.Add("@mvarbitFreeBees", objFunctionMaster.FreeBees);
                aParam.Add("@mvarvchPortletFile", objFunctionMaster.PortletFile);
                aParam.Add("@mvarvchOutPut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_FUNCTION_MANAGE";
                var result = Connection.Query<SuccessMessage>(query, aParam, commandType: CommandType.StoredProcedure);
                return result.AsList()[0].successid;
            }
            catch (Exception exception)
            {
                throw new Exception("Execution Failed", exception);
            }
        }

        public IList<FunctionMasterModel> GetAllFunction(FunctionMasterModel objFunctionMaster)
        {
            List<FunctionMasterModel> list = new List<FunctionMasterModel>();
            var vParam = new DynamicParameters();
            vParam.Add("@ActionCode", objFunctionMaster.ActionCode);
            vParam.Add("@int_function_id", objFunctionMaster.FunctionId);
            vParam.Add("@int_Flag", objFunctionMaster.Flag);
            var query = "USP_ADMIN_FUNCTION_VIEW";
            var result = Connection.Query<FunctionMasterNewModel>(query, vParam, commandType: CommandType.StoredProcedure);
            var Rowno = 0;
            foreach (var funMaster in result.AsList())
            {
                Rowno = Rowno + 1;
                FunctionMasterModel item = new FunctionMasterModel
                {
                    RowNo = Rowno,// Convert.ToInt32(reader["RowNo"].ToString()),
                    FunctionId = funMaster.intFunctionId,//Convert.ToString(reader["intFunctionId"]),
                    FunctionName = funMaster.vchFunction,//Convert.ToString(reader["vchFunction"]),
                    FileName = funMaster.vchFileName,//Convert.ToString(reader["vchFileName"]),
                    Description = funMaster.vchDescription,// Convert.ToString(reader["vchDescription"]),
                    FAdd = funMaster.vchAction1,// Convert.ToString(reader["vchAction1"]),
                    FView = funMaster.vchAction2,//Convert.ToString(reader["vchAction2"]),
                    FManage = funMaster.vchAction3,//Convert.ToString(reader["vchAction3"]),
                    MailR = funMaster.bitMailSend,//Convert.ToInt32(reader["bitMailSend"]),
                    FreeBees = funMaster.bitFreebees,// Convert.ToInt32(reader["bitFreebees"]),
                    PortletFile = funMaster.vchportletfile,// Convert.ToString(reader["vchportletfile"])
                };
                list.Add(item);
            }


            return list;
        }

        public string GetFunctionData(int intFuncId)
        {
            throw new NotImplementedException();
        }
        public IList<FunctionMasterModel> GetFunctionDetails(FunctionMasterModel objFunctionMaster)
        {
            throw new NotImplementedException();
        }
        public IList<FunctionMasterModel> GetFunctionId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
