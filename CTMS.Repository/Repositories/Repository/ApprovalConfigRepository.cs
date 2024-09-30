using CTMS.Model.DTOs;
using CTMS.Model.Entities.Master;
using CTMS.Model.BlockMaster;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;

using System.Data;
using CTMS.Model.Entities.Registration;
using CTMS.Model.Entities.Approval;
using System.Text.RegularExpressions;
using CTMS.Core;
using static CTMS.Core.Net;
using System.Collections.Generic;
using CTMS.Model.Entities.Event;

namespace CTMS.Repository.Repository
{
    public class ApprovalConfigRepository : CTMSRepositoryBase, IApprovalConfigRepository
    {
        public ApprovalConfigRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<List<BlockMaster>> ApprovalConfiguration(BlockMaster MBL)
        {
            throw new NotImplementedException();
        }
        public async Task<int> TakeAction(TakeActionDto Obj)
        {
            
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "AP");
                p.Add("@P_Status", Obj.Status);
                p.Add("@P_TroupeId", Obj.Troupeid);
                p.Add("@P_Remarks", Obj.Remarks);    
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                var returnVal = p.Get<string>("@P_MSG");
                return Convert.ToInt32(returnVal);
            
        }
        public async Task<string> AdminTroupeRegistration(M_Troupe_Registration P_TUS)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", P_TUS.Action);
                p.Add("@P_DistId", P_TUS.DistrictId);
                p.Add("@P_BlockId", P_TUS.BlockId);
                p.Add("@P_GpId", P_TUS.GpId);
                p.Add("@P_GroupName", P_TUS.vchGroupName);
                p.Add("@P_PinCode", P_TUS.Pincode);
                p.Add("@P_Address", P_TUS.vchAddress);
                p.Add("@P_GrpCreatedOn", P_TUS.GrpCreatedDate);
                p.Add("@P_IsSocietyReg", P_TUS.GrpRegistered);
                p.Add("@P_Regdocno", P_TUS.SocietyRegactNo);
                p.Add("@P_Regdate", P_TUS.RegistrationDate);
                p.Add("@P_CategoryId", P_TUS.CategoryId);
                p.Add("@P_SubCategoryId", P_TUS.SubCategoryId);
                p.Add("@P_BannerImg", P_TUS.UploadBannerImage);
                p.Add("@P_SocietyRegDoc", P_TUS.UpldRegPhtCopyImage);
                p.Add("@P_GroupPhoto", P_TUS.GroupPhoto);
                p.Add("@P_UploadVideo", P_TUS.UpldVdo);
                p.Add("@P_UploadFolderPath", P_TUS.UploadFolderPath);
                p.Add("@mytable", P_TUS.ComponentXml);
                p.Add("@P_CreatedBy", P_TUS.CreatedBy);
                if (P_TUS.TroupeId != 0)
                {
                    p.Add("@P_TroupeId", P_TUS.TroupeId);
                }
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@P_MSG");
                return returnVal;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<TroupeRegistrationView>> ViewNewRegistration(TroupeRegistrationView TRV)
        {
            try
            {
                if(TRV.DistrictId==null)
                {
                    TRV.DistrictId = 0;
                }
                if (TRV.BlockId == null)
                {
                    TRV.BlockId = 0;
                }
                DynamicParameters objParam = new DynamicParameters();
                objParam.Add("@P_ActionCode", "V");
                objParam.Add("@P_Status", TRV.Status);
                objParam.Add("@P_DistId", TRV.DistrictId);
                objParam.Add("@P_BlockId", TRV.BlockId);
                objParam.Add("@P_GpId", TRV.GPId);
                objParam.Add("@P_GroupName", TRV.TroupeName);
                var results = await Connection.QueryAsync<TroupeRegistrationView>("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", objParam, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<MemberDetailsView>> MemberDetails(MemberDetailsView MDV)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "M");
                p.Add("@P_Troupeid", MDV.TroupeId);
                var results = await Connection.QueryAsync<MemberDetailsView>("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<completedEventsList>> completedEventsList(MemberDetailsView MDV)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@ActionCode", "C");
                p.Add("@BlockId", MDV.TroupeId);
                var results = await Connection.QueryAsync<completedEventsList>("USP_EVENT_DETAILS_MOBAPI", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<completedEventsList>> completedEventsList_MOB(string BlockId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@ActionCode", "CE");
                p.Add("@BlockId", BlockId);
                var results = await Connection.QueryAsync<completedEventsList>("USP_EVENT_DETAILS_MOBAPI", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<pendingApprovalEventList>> pendingApprovalEventList(MemberDetailsView MDV)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@ActionCode", "P");
                p.Add("@BlockId", MDV.TroupeId);
                var results = await Connection.QueryAsync<pendingApprovalEventList>("USP_EVENT_DETAILS_MOBAPI", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Approval
        public async Task<List<TroupeApproval>> ApprovalView(TroupeApproval TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "V");
            objParam.Add("@P_Status", TRV.ApprovedStatus);
            objParam.Add("@P_BlockId", TRV.BlockId);
            objParam.Add("@P_DistId", TRV.DistId);
            objParam.Add("@FROMDATE", TRV.FromDate);
            objParam.Add("@TODATE", TRV.ToDate);
            var results = await Connection.QueryAsync<TroupeApproval>("USP_TROUPE_APPROVAL_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeRegistrationView>> ApprovalView_Mob(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "MV");
            objParam.Add("@P_Status", TRV.ApprovedStatus);
            objParam.Add("@P_BlockId", TRV.BlockId);
            objParam.Add("@P_DistId", TRV.DistrictId);
            var results = await Connection.QueryAsync<TroupeRegistrationView>("USP_TROUPE_APPROVAL_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<TroupeApprovalViewRes> ApprovalTakeActionView(string TroupeId, int status)
        {
            TroupeApprovalViewRes res = new TroupeApprovalViewRes();
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "TAV");
            p.Add("@P_Status", status);
            p.Add("@P_Troupeid",Convert.ToInt32(DESEncrypt.Decrypt(TroupeId)));
            var results = await Connection.QueryMultipleAsync("USP_TROUPE_APPROVAL_DETAILS", p, commandType: CommandType.StoredProcedure);
            res.TroupeInfo = results.Read<TroupeBasicInfo>().ToList();
            res.MemberInfo = results.Read<MemberInfo>().ToList();
            return res;
        }
        public async Task<TroupeApprovalViewRes> M_ApprovalTakeActionView(string TroupeId, int status)
        {
            TroupeApprovalViewRes res = new TroupeApprovalViewRes();
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "TAV");
            p.Add("@P_Status", status);
            p.Add("@P_Troupeid",TroupeId);
            var results = await Connection.QueryMultipleAsync("USP_TROUPE_APPROVAL_DETAILS", p, commandType: CommandType.StoredProcedure);
            res.TroupeInfo = results.Read<TroupeBasicInfo>().ToList();
            res.MemberInfo = results.Read<MemberInfo>().ToList();
            return res;
        }
        #endregion
        public async Task<List<MemberDetailsView>> GetAllDetails(MemberDetailsView MDV)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "E");
                p.Add("@P_Troupeid", MDV.TroupeId);
                var results = await Connection.QueryAsync<MemberDetailsView>("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> ApprovalTakeAction(ApprovalStatus Obj)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "AP");
                p.Add("@P_Status", Obj.ApprovedStatus);
                p.Add("@P_TroupeId", Obj.TroupeId);
                p.Add("@P_Remarks", Obj.Remark);
                p.Add("@P_CreatedBy", Obj.Uid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_TROUPE_APPROVAL_DETAILS", p, commandType: CommandType.StoredProcedure);
                var returnVal = p.Get<string>("@P_MSG");
                return Convert.ToInt32(returnVal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> M_ApprovalTakeAction(ApprovalStatus Obj)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "MAP");
                p.Add("@P_Status", Obj.ApprovedStatus);
                p.Add("@P_TroupeId", Obj.TroupeId);
                p.Add("@P_Remarks", Obj.Remark);
                p.Add("@P_CreatedBy", Obj.Uid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_TROUPE_APPROVAL_DETAILS", p, commandType: CommandType.StoredProcedure);
                var returnVal = p.Get<string>("@P_MSG");
                return Convert.ToInt32(returnVal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DashboardDetails>> DashboardDetails(DashboardDetails DD)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DB");
                p.Add("@P_userid", DD.userid);
                p.Add("@P_BlockId", DD.blockid);
                p.Add("@P_DistId", DD.distid);
                p.Add("@P_GpId", DD.gpid);
                p.Add("@P_fromdate", DD.fromdate);
                p.Add("@P_todate", DD.todate);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                 var res=await Connection.QueryAsync<DashboardDetails>("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DashboardCount> Dashboardcount(string userid)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DC");
                p.Add("@P_BlockId", userid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardCount>("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DashboardTroupeDetailsCount>> DashboardTroupeDetailsCount(DashboardDetails DD)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DTD");
                p.Add("@P_BlockId", DD.userid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardTroupeDetailsCount>("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.ToList()!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DashboardEventDetailsCount>> DashboardEventDetailsCount(DashboardDetails DD)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DED");
                p.Add("@P_BlockId", DD.userid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardEventDetailsCount>("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.ToList()!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DashboardArtFormDetailsCount>> DashboardArtFormDetailsCount(DashboardDetails DD)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DAFD");
                p.Add("@P_BlockId", DD.userid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardArtFormDetailsCount>("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.ToList()!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DashboardArtFormDetailsSummary>> DashboardArtFormSummaryDetails(DashboardDetails DD)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DAFS");
                p.Add("@P_BlockId", DD.userid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardArtFormDetailsSummary>("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.ToList()!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DashboardUpcommingEvent>> DashboardUpcomingEventCount(DashboardDetails DD)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DUEC");
                p.Add("@P_BlockId", DD.userid);
                p.Add("@P_URTYPE", DD.urtype);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardUpcommingEvent>("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.ToList()!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DashboardTroupeEventSummaryCountRes> DashboardTroupeEventSummaryCount(DashboardDetails DD)
        {
            DashboardTroupeEventSummaryCountRes res = new DashboardTroupeEventSummaryCountRes();
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "DTESD");
            p.Add("@P_URTYPE", DD.urtype);
            if(DD.urtype=="DEPT")
            {
                p.Add("@P_DistId", DD.userid);
            }
            else if(DD.urtype == "DSSO")
            {
                p.Add("@P_DistId", DD.userid);
            }
            else
            {
                p.Add("@P_BlockId", DD.userid);
            }
            p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var results = await Connection.QueryMultipleAsync("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
            res.DashboardTroupeDistEvent = results.Read<DashboardTroupeEventDistCount>().ToList();
            res.DashboardTroupeBlockEvent = results.Read<DashboardTroupeEventBlockCount>().ToList();
            return res;
        }
        public async Task<List<DashboardEventSummary>> DashboardEventSummaryCount(DashboardDetails DD)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DES");
                p.Add("@P_Year", DD.Year);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardEventSummary>("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.ToList()!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DashboardPaymentSummary>> DashboardPaymentSummaryCount(DashboardDetails DD)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DPS");
                p.Add("@P_URTYPE", DD.urtype);
                p.Add("@P_BlockId", DD.userid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardPaymentSummary>("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.ToList()!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EventDetails> ViewEventDetails(string action, int Troupeid)
        {
            List<EventDetails> list = null;
            var p = new DynamicParameters();
            p.Add("@ActionCode ", action);
            p.Add("@TroupeId", Troupeid);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            list = Connection.Query<EventDetails>("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure).ToList();
            return list;
        }
        public List<EventDetails_mob> ViewEventDetails_Troupe(string action, int Troupeid)
        {
            List<EventDetails_mob> list = null;
            var p = new DynamicParameters();
            p.Add("@ActionCode ", action);
            p.Add("@TroupeId", Troupeid);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            list = Connection.Query<EventDetails_mob>("USP_EVENT_DETAILS_MOBAPI", p, commandType: CommandType.StoredProcedure).ToList();
            return list;
        }
        public List<EventDetails> Troup_Dashboard_Count(string action, int Troupeid)
        {
            List<EventDetails> list = null;
            var p = new DynamicParameters();
            p.Add("@ActionCode ", action);
            p.Add("@TroupeId", Troupeid);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            list = Connection.Query<EventDetails>("USP_EVENT_DETAILS_MOBAPI", p, commandType: CommandType.StoredProcedure).ToList();
            return list;
        }
        public List<EventDetails> ViewEventDetails_MOB(string action, int Blockid)
        {
            List<EventDetails> list = null;
            var p = new DynamicParameters();
            p.Add("@ActionCode ", action);
            p.Add("@BlockId", Blockid);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            list = Connection.Query<EventDetails>("USP_EVENT_DETAILS_MOBAPI", p, commandType: CommandType.StoredProcedure).ToList();
            return list;
        }

        public async Task<DashboardCount> DashboardDetailsByScheme(int deptid, int scehemeid, int disblockid)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "DCS");
                p.Add("@P_CategoryId", deptid);
                p.Add("@P_SubCategoryId", scehemeid);
                p.Add("@P_BlockId", disblockid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardCount>("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DashboardTroupeDetailsCount>> NotificationCount(DashboardDetails DD)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "NOTIC");
                p.Add("@P_BlockId", DD.userid);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var res = await Connection.QueryAsync<DashboardTroupeDetailsCount>("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
                return res.ToList()!;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
