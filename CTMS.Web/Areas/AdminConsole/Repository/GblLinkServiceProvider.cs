using Dapper;

using CTMS.Web.Areas.AdminConsole.Interface;

using NuGet.Protocol.Plugins;
using System.Data;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Web.Areas.AdminConsole.Models.GlobalLink;

namespace CTMS.Web.Areas.AdminConsole.Repository
{
    public class GblLinkServiceProvider : CTMSRepositoryBase, IGblLinkServiceProvider
    {

        private readonly IConfiguration _configuration;
        public GblLinkServiceProvider(ICTMSConnectionFactory EDODISHAconnectionFactory, IConfiguration configuration) : base(EDODISHAconnectionFactory)
        {

        }



        public int ActivateGlobalLink(Global objGloballink)
        {
            throw new NotImplementedException();
        }

        public string AddGlobalLink(ViewGlobal objGloballink)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "A");
                dyParam.Add("P_nvchGLinkName", objGloballink.GlobalLinkName.Trim());
                dyParam.Add("P_intCreatedBy", objGloballink.CreatedBy);
                dyParam.Add("P_intUpdatedBy", objGloballink.CreatedBy);
                dyParam.Add("P_intSortNum", objGloballink.GintSortNum);
                dyParam.Add("P_INTPROJECTID", objGloballink.INTPROJECTLINKID);
                dyParam.Add("P_VCHICONCLASS", objGloballink.VCHICONCLASS);
                var query = "USP_ADMIN_GLOBALLINK_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public string EditGlobalLink(ViewGlobal objupdateGloballink)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "E");
                dyParam.Add("P_nvchGLinkName", objupdateGloballink.GlobalLinkName.Trim());
                dyParam.Add("P_VCHICONCLASS", objupdateGloballink.VCHICONCLASS.Trim());
                dyParam.Add("P_intGLinkId", objupdateGloballink.intGLinkID);

                dyParam.Add("P_intUpdatedBy", objupdateGloballink.updatedBy);
                dyParam.Add("P_INTPROJECTID", objupdateGloballink.INTPROJECTLINKID);
                var query = "USP_ADMIN_GLOBALLINK_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public string DeleteGlobalLink(int id, int updatedby)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "D");
                dyParam.Add("P_intGLinkId", id);
                dyParam.Add("P_intUpdatedBy", updatedby);
                var query = "USP_ADMIN_GLOBALLINK_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public async Task<ViewGlobalLink> GetAllActiveGlobalLink()

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VAA");
                // dyParam.Add("P_intStatus",status);
                var query = "USP_ADMIN_GLOBALLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<ViewGlobal>();
                ViewGlobalLink viewModel = new ViewGlobalLink();
                viewModel.ViewGlobalLinkDetailsmodel = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ViewGlobalLink> GetAllInActiveGlobalLink()

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VAI");
                var query = "USP_ADMIN_GLOBALLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<ViewGlobal>();
                ViewGlobalLink viewModel = new ViewGlobalLink();
                viewModel.ViewGlobalLinkDetailsmodel = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ModifySlnoGlobalLink(int globalId, int slno, int updatedby)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "MS");
                dyParam.Add("P_intGLinkId", globalId);
                dyParam.Add("P_intSortNum", slno);
                dyParam.Add("P_intUpdatedBy", updatedby);
                var query = "USP_ADMIN_GLOBALLINK_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }
        public int GetGlobalLinkMaxId()
        {
            int GMaxId;
            IEnumerable<ViewGlobal> maxid;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "S");
                var query = "USP_ADMIN_GLOBALLINK_VIEW";
                maxid = Connection.Query<ViewGlobal>(query, dyParam, commandType: CommandType.StoredProcedure);
                GMaxId = Convert.ToInt32(maxid.AsList()[0].GMAxid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return GMaxId;
        }

        public async Task<IEnumerable<ViewGlobal>> GetGlobalLinkById(int id)
        {
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VI");
                dyParam.Add("P_intGLinkId", id);
                var query = "USP_ADMIN_GLOBALLINK_VIEW";
                var result = await Connection.QueryAsync<ViewGlobal>(query, dyParam, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ViewGlobalLink> GetById(int id)

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "VI");
                dyParam.Add("P_intGLinkId", id);
                var query = "USP_ADMIN_GLOBALLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<ViewGlobal>();
                ViewGlobalLink viewModel = new ViewGlobalLink();
                viewModel.ViewGlobalLinkDetailsmodel = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ViewGlobalLink> GetMaxId()

        {
            try
            {

                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "S");
                var query = "USP_ADMIN_GLOBALLINK_VIEW";
                var result = await Connection.QueryMultipleAsync(query, dyParam, commandType: CommandType.StoredProcedure);
                var t1 = await result.ReadAsync<ViewGlobal>();
                ViewGlobalLink viewModel = new ViewGlobalLink();
                viewModel.ViewGlobalLinkDetailsmodel = t1.AsList();
                return viewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string MarkActiveGlobalLink(int id, int updatedby)
        {
            string strOutput = "";
            IEnumerable<SuccessMessage> SuccessMessages;
            try
            {
                var dyParam = new DynamicParameters();
                dyParam.Add("P_Action", "EI");
                dyParam.Add("P_intGLinkId", id);
                dyParam.Add("P_intUpdatedBy", updatedby);
                var query = "USP_ADMIN_GLOBALLINK_MANAGE";
                SuccessMessages = Connection.Query<SuccessMessage>(query, dyParam, commandType: CommandType.StoredProcedure);
                strOutput = SuccessMessages.AsList()[0].successmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strOutput;
        }

        public IList<Global> GetAllLocation(Global objGloballink)
        {
            throw new NotImplementedException();
        }
        public IList<Global> GetGlobalLinkDetails(Global objGloballink)
        {
            throw new NotImplementedException();
        }

        public int GetMaxGlinkId()
        {
            throw new NotImplementedException();
        }

        public int InActivateGlobalLink(Global objGloballink)
        {
            throw new NotImplementedException();
        }

        public int UpdateSlno(Global objGloballink)
        {
            throw new NotImplementedException();
        }
    }
}
