using Dapper;
using CTMS.Web.Areas.AdminConsole.Interface;
using NuGet.Protocol.Plugins;
using System.Data;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Web.Areas.AdminConsole.Models.DesignationMaster;

namespace CTMS.Web.Areas.AdminConsole.Repository
{
    public class DesignationServiceProvider : CTMSRepositoryBase, IDesignationServiceProvider
    {
        private readonly IConfiguration _configuration;
        public DesignationServiceProvider(ICTMSConnectionFactory EDODISHAconnectionFactory, IConfiguration configuration) : base(EDODISHAconnectionFactory)
        {

        }
        public string AddDesignation(DesignationMasters model)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ACTION", "A");
                p.Add("@P_NVCHDESIGNAME", model.DESIGNATIONNAME);
                p.Add("@P_INTDESIGID", model.USERTYPE);
                p.Add("@P_DISTID", model.DISTID != 0 ? model.DISTID : 0);
                p.Add("@P_BLOCKID", model.BLOCKID != 0 ? model.BLOCKID : 0);
                p.Add("@P_INTCREATEDBY", model.INTCREATEDBY);
                p.Add("@P_INTUPDATEDBY", model.INTUPDATEDBY);
                p.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_MASTER_DESIGNATION_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, p, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;

        }
        public async Task<DesignationMasterModel> GetAllDesgination(int id)

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("@P_Action", "VD");
                dyParam.Add("@P_BITSTATUS", id);
                dyParam.Add("@cur", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADSMIN_DESIGNATION_MANAGE_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<DesignationMasters>();
                DesignationMasterModel viewModel = new DesignationMasterModel();
                viewModel.DesignationMasterList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
      
        public string MarkActive(DesignationMasters model)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;

            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("@P_Action", "ACTIVE");
                dyParam.Add("@P_INTUPDATEDBY", model.INTUPDATEDBY);
                dyParam.Add("@P_INTDESIGID", model.INTDESIGID);
                dyParam.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_MASTER_DESIGNATION_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;

        }
        public string MarkInactive(DesignationMasters model)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("@P_Action", "I");
                dyParam.Add("@P_INTUPDATEDBY", model.INTUPDATEDBY);
                dyParam.Add("@P_INTDESIGID", model.INTDESIGID);
                dyParam.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_MASTER_DESIGNATION_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;

        }
        public async Task<DesignationMasterModel> GetById(int id)

        {
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("@P_Action", "E");
                dyParam.Add("@P_INTDESIGID", id);
                dyParam.Add("@cur", DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADSMIN_DESIGNATION_MANAGE_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<DesignationMasters>();
                DesignationMasterModel viewModel = new DesignationMasterModel();
                viewModel.DesignationMasterList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string EditDesignation(DesignationMasters model)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("@P_ACTION", "U");
                dyParam.Add("@P_INTDESIGID", model.INTDESIGID);
                dyParam.Add("@P_NVCHDESIGNAME", model.DESIGNATIONNAME);
                dyParam.Add("@P_INTUSERTYPE", model.USERTYPE);
                dyParam.Add("@P_DISTID", model.DISTID);
                dyParam.Add("@P_BLOCKID", model.BLOCKID);
                dyParam.Add("@P_INTUPDATEDBY", model.INTUPDATEDBY);
                dyParam.Add("@P_Msg", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var query = "USP_ADMIN_MASTER_DESIGNATION_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return strOutput;

        }

    }
}
