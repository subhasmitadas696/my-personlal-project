using Dapper;

using CTMS.Web.Areas.AdminConsole.Interface;
using NuGet.Protocol.Plugins;
using System.Data;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Web.Areas.AdminConsole.Models.SetPermission;

namespace CTMS.Web.Areas.AdminConsole.Repository
{
    public class SetPermissionServiceProvider : CTMSRepositoryBase, ISetPermissionServiceProvider
    {

        private readonly IConfiguration _configuration;
        public SetPermissionServiceProvider(ICTMSConnectionFactory EDODISHAconnectionFactory, IConfiguration configuration) : base(EDODISHAconnectionFactory)
        {

        }

        public async Task<SetPermissionModel> GetAllPrimaryLinkByGlobalLink(SetPermissionModel objpermission)

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VGP");
                dyParam.Add("P_INTDESINATIONID", objpermission.DesignId);
                dyParam.Add("P_INTPROJECTID", objpermission.INTPROJECTLINKID);
                var query = "USP_ADMIN_SETPERMISSION_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<SetPermissionDetails>();
                SetPermissionModel viewModel = new SetPermissionModel();
                viewModel.GlobalPrimaryList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SetPermissionLink_Designation(int designationId, int Plinkid, int Intaccess, int user, int projectId)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "AED");
                dyParam.Add("P_INTDESINATIONID", designationId);
                dyParam.Add("P_intpLinkId", Plinkid);
                dyParam.Add("P_TINACCESS", Intaccess);
                dyParam.Add("P_intuser", user);
                dyParam.Add("P_INTPROJECTID", projectId);
                var query = "USP_ADMIN_SETPERMISSION_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public async Task<SetPermissionModel> GetAllPrimaryLinkByGlobalLinkUserwise(SetPermissionModel objpermission)

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VUGP");
                dyParam.Add("P_INTUSERID", objpermission.UserId);
                dyParam.Add("P_INTPROJECTID", objpermission.INTPROJECTLINKID);
                var query = "USP_ADMIN_SETPERMISSION_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<SetPermissionDetails>();
                SetPermissionModel viewModel = new SetPermissionModel();
                viewModel.GlobalPrimaryList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SetPermissionLink_User(int userId, int Plinkid, int Intaccess, int user, int projectId)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "AEU");
                dyParam.Add("P_INTUSERID", userId);
                dyParam.Add("P_intpLinkId", Plinkid);
                dyParam.Add("P_TINACCESS", Intaccess);
                dyParam.Add("P_intuser", user);
                dyParam.Add("P_INTPROJECTID", projectId);
                var query = "USP_ADMIN_SETPERMISSION_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public string DeletePermissionLink_Designation(int DesignationId, int projectId)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "DPD");
                dyParam.Add("P_INTDESINATIONID", DesignationId);
                dyParam.Add("P_INTPROJECTID", projectId);
                var query = "USP_ADMIN_SETPERMISSION_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public string DeletePermissionLink_User(int userId, int projectId)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "DPU");
                dyParam.Add("P_INTUSERID", userId);
                dyParam.Add("P_INTPROJECTID", projectId);
                var query = "USP_ADMIN_SETPERMISSION_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public async Task<SetPermissionModel> GetAllUser()

        {
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VU");
                var query = "USP_ADMIN_SETPERMISSION_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<UserDetails>();
                SetPermissionModel viewModel = new SetPermissionModel();
                viewModel.UserList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string RemovePermissionList_User(int userId, int updatedby)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "RUP");
                dyParam.Add("P_INTUSERID", userId);
                dyParam.Add("P_intuser", updatedby);
                var query = "USP_ADMIN_SETPERMISSION_MANAGE";
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
