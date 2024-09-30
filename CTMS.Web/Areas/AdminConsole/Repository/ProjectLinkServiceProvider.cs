using Dapper;

using CTMS.Web.Areas.AdminConsole.Interface;
using NuGet.Protocol.Plugins;
using System.Data;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Web.Areas.AdminConsole.Models.ProjectMaster;

namespace CTMS.Web.Areas.AdminConsole.Repository
{
    public class ProjectLinkServiceProvider : CTMSRepositoryBase, IProjectLinkServiceProvider
    {

        private readonly IConfiguration _configuration;
        public ProjectLinkServiceProvider(ICTMSConnectionFactory EDODISHAconnectionFactory, IConfiguration configuration) : base(EDODISHAconnectionFactory)
        {

        }

        public string AddProjectLink(Project objProjectlink)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "A");
                dyParam.Add("P_NVCHPROJECTLINKNAME", objProjectlink.NVCHPROJECTLINKNAME.Trim());
                dyParam.Add("P_NVCHPROJECTLINKDESC", objProjectlink.NVCHPROJECTLINKDESC != null ? objProjectlink.NVCHPROJECTLINKDESC.Trim() : objProjectlink.NVCHPROJECTLINKDESC);
                dyParam.Add("P_intCreatedBy", objProjectlink.INTCREATEDBY);
                var query = "USP_ADMIN_PROJECTLINK_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public async Task<ViewProjectLink> GetAllActiveProjectLink()

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VAA");
                var query = "USP_ADMIN_PROJECTLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<ViewPoject>();
                ViewProjectLink viewModel = new ViewProjectLink();
                viewModel.ViewProjectLinkDetailsmodel = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ViewProjectLink> GetAllInActiveProjectLink()

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VAI");
                var query = "USP_ADMIN_PROJECTLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<ViewPoject>();
                ViewProjectLink viewModel = new ViewProjectLink();
                viewModel.ViewProjectLinkDetailsmodel = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string InactiveProjectLink(int id, int updatedby)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "I");
                dyParam.Add("P_INTPROJECTLINKID", id);
                dyParam.Add("P_intUpdatedBy", updatedby);
                var query = "USP_ADMIN_PROJECTLINK_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public string ActiveProjectLink(int id, int updatedby)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "MA");
                dyParam.Add("P_INTPROJECTLINKID", id);
                dyParam.Add("P_intUpdatedBy", updatedby);
                var query = "USP_ADMIN_PROJECTLINK_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public async Task<ViewProjectLink> GetById(int id)

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VI");
                dyParam.Add("P_INTPROJECTLINKID", id);
                var query = "USP_ADMIN_PROJECTLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<ViewPoject>();
                ViewProjectLink viewModel = new ViewProjectLink();
                viewModel.ProjectLinkModelIdwise = t1.ToList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateProjectLink(Project objProjectlink)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "E");
                dyParam.Add("P_INTPROJECTLINKID", objProjectlink.INTPROJECTLINKID);
                dyParam.Add("P_NVCHPROJECTLINKNAME", objProjectlink.NVCHPROJECTLINKNAME.Trim());
                dyParam.Add("P_NVCHPROJECTLINKDESC", objProjectlink.NVCHPROJECTLINKDESC != null ? objProjectlink.NVCHPROJECTLINKDESC.Trim() : objProjectlink.NVCHPROJECTLINKDESC);
                dyParam.Add("P_intUpdatedBy", objProjectlink.INTUPDATEDBY);
                var query = "USP_ADMIN_PROJECTLINK_MANAGE";
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
