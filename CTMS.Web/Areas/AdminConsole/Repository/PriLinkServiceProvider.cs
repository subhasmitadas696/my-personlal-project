using Dapper;

using CTMS.Web.Areas.AdminConsole.Interface;
using NuGet.Protocol.Plugins;
using System.Data;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Web.Areas.AdminConsole.Models.PrimaryLink;
using CTMS.Web.Areas.AdminConsole.Models.ProjectMaster;
using CTMS.Web.Areas.AdminConsole.Models.GlobalLink;

namespace CTMS.Web.Areas.AdminConsole.Repository
{
    public class PriLinkServiceProvider : CTMSRepositoryBase, IPriLinkServiceProvider
    {
        #region Variable Declaration        
        object param = new object();
        #endregion


        private readonly IConfiguration _configuration;
        public PriLinkServiceProvider(ICTMSConnectionFactory EDODISHAconnectionFactory, IConfiguration configuration) : base(EDODISHAconnectionFactory)
        {

        }



        public string AddPrimaryLink(PrimaryModel objPrimary)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "A");
                dyParam.Add("P_INTPROJECTID", objPrimary.INTPROJECTLINKID);
                dyParam.Add("P_nvchPLinkName", objPrimary.PlinkName);
                dyParam.Add("P_intGLinkId", objPrimary.GlinkId);
                dyParam.Add("P_intFunctionId", objPrimary.FunctionId);
                dyParam.Add("P_intCreatedBy", objPrimary.CreatedBy);
                dyParam.Add("P_intUpdatedBy", objPrimary.UpdatedBy);
                dyParam.Add("P_INTSLNO", objPrimary.INTSLNO);
                var query = "USP_PRimarylLink_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public string MarkInactivePrimaryLink(PrimaryModel objPrimary)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "I");
                dyParam.Add("P_intUpdatedBy", objPrimary.UpdatedBy);
                dyParam.Add("P_mvarintFunctionId", objPrimary.INTPLINKID);

                var query = "USP_PRimarylLink_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public async Task<PrimaryModel> GetAllFunctionMaster()

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VF");
                var query = "USP_ADMIN_PRIMARYLINK_VIEW";
                var functionList = await Connection.QueryAsync<FunctionMasterModel>(query, dyParam, commandType: CommandType.StoredProcedure);
                var viewModel = new PrimaryModel
                {
                    FunctionList = functionList.ToList()
                };
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PrimaryModel> GetAllPrimaryLink(int id)

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "V");
                dyParam.Add("P_intStatus", id);
                var query = "USP_ADMIN_PRIMARYLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<PrimaryLink>();
                PrimaryModel viewModel = new PrimaryModel();
                viewModel.PrimaryLinkList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<UpdatePrimaryLink>> GetPrimaryLinkById(int id)
        {
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VI");
                dyParam.Add("P_intPLinkId", id);
                var query = "USP_ADMIN_PRIMARYLINK_VIEW";

                var result = await Connection.QueryAsync<UpdatePrimaryLink>(query, dyParam, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string EditPrimaryLink(PrimaryModel objPrimary)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "E");
                dyParam.Add("P_mvarintFunctionId", objPrimary.INTPLINKID);
                dyParam.Add("P_nvchPLinkName", objPrimary.PlinkName);
                dyParam.Add("P_intGLinkId", objPrimary.INTGLINKID);
                dyParam.Add("P_intFunctionId", objPrimary.INTFUNCTIONID);
                dyParam.Add("P_intUpdatedBy", objPrimary.UpdatedBy);
                var query = "USP_PRimarylLink_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public string MarkActivePrimaryLink(PrimaryModel objPrimary)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "EA");
                dyParam.Add("P_intUpdatedBy", objPrimary.UpdatedBy);
                dyParam.Add("P_mvarintFunctionId", objPrimary.INTPLINKID);
                var query = "USP_PRimarylLink_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public async Task<PrimaryModel> GetAllPrimaryLinkByGlobalLink(PrimaryModel objPrimary)

        {
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VG");
                dyParam.Add("P_intGLinkId", objPrimary.GlinkId);

                var query = "USP_ADMIN_PRIMARYLINK_VIEW";
                var result = await Connection.QueryAsync<PrimaryLink>(query, dyParam, commandType: CommandType.StoredProcedure);

                PrimaryModel viewModel = new PrimaryModel();
                viewModel.PrimaryLinkList = result.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ModifySlnoPrimaryLink(int primaryId, int slno, int updatedby)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "S");
                dyParam.Add("P_intUpdatedBy", updatedby);
                dyParam.Add("P_mvarintFunctionId", primaryId);
                dyParam.Add("P_INTSLNO", slno);
                var query = "USP_PRimarylLink_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public int GetPrimaryLinkMaxId(int Glinkid)
        {
            int INTSLNO;
            IEnumerable<PrimaryModel> maxid;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "S");
                dyParam.Add("P_intGLinkId", Glinkid);
                var query = "USP_ADMIN_PRIMARYLINK_VIEW";
                maxid = Connection.Query<PrimaryModel>(query, dyParam, commandType: CommandType.StoredProcedure);
                INTSLNO = Convert.ToInt32(maxid.AsList()[0].INTSLNO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return INTSLNO;
        }
        public async Task<PrimaryModel> GetAllProjectLink()

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VP");
                var query = "USP_ADMIN_PRIMARYLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<ViewPoject>();
                PrimaryModel viewModel = new PrimaryModel();
                viewModel.ViewProjectLinkList = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PrimaryModel> GetAllActiveGlobalLinkByProjectId(int ProjectId)

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "GP");
                dyParam.Add("P_INTPROJECTID", ProjectId);
                var query = "USP_ADMIN_PRIMARYLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<ViewGlobal>();
                PrimaryModel viewModel = new PrimaryModel();
                viewModel.ViewGlobalLinkDetailsByProjectId = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
