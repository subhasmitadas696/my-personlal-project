using CTMS.Core;
using CTMS.Model.DTOs;
using CTMS.Model.Entities.Event;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.Registration;
using CTMS.Repository.BaseRepository;
using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace CTMS.Repository.Repository
{
#pragma warning disable
    public class RegistraionRepository : CTMSRepositoryBase, IRegistraionRepository
    {
        public RegistraionRepository(ICTMSConnectionFactory CTMSConnectionFactory) : base(CTMSConnectionFactory)
        {

        }
        public async Task<(MyProfile, List<ExecutiveMember>, List<GeneralBody>)> GetoneProfileDetails(int PID,int Blockid)
        {
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "GP");
            p.Add("@P_ID", PID);
            p.Add("@P_Blockid", Blockid);
            p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var results = await Connection.QueryMultipleAsync("USP_MY_PROFILE_DETAILS", p, commandType: CommandType.StoredProcedure);
            string returnVal = p.Get<string>("@P_MSG");
            var profileDetails = await results.ReadFirstOrDefaultAsync<MyProfile>();
            var executiveMemberDetails = await results.ReadAsync<ExecutiveMember>();
            var generalBodyDetails = await results.ReadAsync<GeneralBody>();
            return (profileDetails, executiveMemberDetails.ToList(), generalBodyDetails.ToList());
        }
        public async Task<List<MyProfile>> ViewMyProfile(MyProfile MP)
        {
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "VP");
            p.Add("@P_Blockid", MP.BLOCKID);
            p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var a=await Connection.QueryAsync<MyProfile>("USP_MY_PROFILE_DETAILS", p, commandType: CommandType.StoredProcedure);
            string returnVal = p.Get<string>("@P_MSG");
            return a.ToList();
        }
        public async Task<string> DeleteProfiledetails(MyProfile MP)
        {
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "DP");
            p.Add("@P_ID", MP.PID);
            p.Add("@P_CreatedBy", MP.createdby);
            p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var a=await Connection.QueryAsync("USP_MY_PROFILE_DETAILS", p, commandType: CommandType.StoredProcedure);
            string returnVal = p.Get<string>("@P_MSG");
            return returnVal;
        }
        public async Task<List<ExecutiveMember>> ExecutiveMemberDetails(int PID)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "EXD");
                p.Add("@P_ID", PID);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var a = await Connection.QueryAsync<ExecutiveMember>("USP_TROUPE_APPROVAL_DETAILS", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@P_MSG");
                return a.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }
        public int GetProfileCount(int BlockId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "PC");
                p.Add("@P_Blockid", BlockId);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                var a = Connection.Query("USP_MY_PROFILE_DETAILS", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@P_MSG");
                return returnVal.ToInt();
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }
        public async Task<List<GeneralBody>> GeneralMemberDetails(int PID)
        {
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", "GBD");
            p.Add("@P_ID", PID);
            p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            var a=await Connection.QueryAsync< GeneralBody>("USP_TROUPE_APPROVAL_DETAILS", p, commandType: CommandType.StoredProcedure);
            string returnVal = p.Get<string>("@P_MSG");
            return a.ToList();
        }
        public async Task<string> AddMyProfileDetails(MyProfile MP)
        {
            var p = new DynamicParameters();
            if(MP.PID!=null)
            {
            p.Add("@P_ActionCode", "UP");
            }
            else
            {
                p.Add("@P_ActionCode", "IP");
            }
            p.Add("@P_societyregcert", MP.societyregcert);
            p.Add("@P_Blockid", MP.BLOCKID);
            p.Add("@P_bylawfile", MP.bylawfile);
            p.Add("@P_bymemorandumfile", MP.bymemorandumfile);
            p.Add("@P_societyregnumber", MP.societyregnumber);
            p.Add("@P_societyregdate", MP.societyregdate);
            p.Add("@P_Bkssaccountno", MP.Bkssaccountno);
            p.Add("@P_mettingresolution", MP.mettingresolution);
            p.Add("@P_generalbodyformdate",MP.generalbodyformdate);
            p.Add("@P_vicepresidentname", MP.vicepresidentname);
            p.Add("@P_positionheldfrom", MP.positionheldfrom);
            p.Add("@P_CreatedBy", MP.createdby);
            p.Add("@P_ID", MP.PID);
            p.Add("@EXECUTIVE", MP.ExecutiveXml);
            p.Add("@GeneralBody", MP.GeneralsXml);
            p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            await Connection.ExecuteAsync("USP_MY_PROFILE_DETAILS", p, commandType: CommandType.StoredProcedure);
            string returnVal = p.Get<string>("@P_MSG");
            return returnVal;
        }
        public async Task<string> TroupeRegistration(M_Troupe_Registration P_TUS)
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
            await Connection.ExecuteAsync("USP_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
            string returnVal = p.Get<string>("@P_MSG");
            return returnVal;
        }
        public async Task<List<TroupeRegistrationView>> TroupeRegistrationView(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "V");
            objParam.Add("@P_Status", TRV.Status);
            var results = await Connection.QueryAsync<TroupeRegistrationView>("USP_TROUPE_REGISTRATION_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<ReviewerPhotolist>> ReviewersPhotoList(int assigneventid, int Troupeid)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "RV");
            objParam.Add("@P_AssignEventId", assigneventid);
            objParam.Add("@P_TroupeId", Troupeid);
            var results = await Connection.QueryAsync<ReviewerPhotolist>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<MemberDetailsView>> MemberDetails(MemberDetailsView MDV)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "M");
                p.Add("@P_Troupeid", MDV.TroupeId);
                var results = await Connection.QueryAsync<MemberDetailsView>("USP_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<EventPerformedDetails>> EventPerformedDetails(EventPerformedDetails MDV)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "EPD");
                p.Add("@P_Troupeid", MDV.TroupeId);
                var results = await Connection.QueryAsync<EventPerformedDetails>("USP_DASHBOARD_DETAILS", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<MemberDetailsView>> M_MemberDetails(string Troupeid)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "M");
                p.Add("@P_Troupeid", Troupeid);
                var results = await Connection.QueryAsync<MemberDetailsView>("USP_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<MemberDetailsView>> GetAllDetails(MemberDetailsView MDV)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "E");
                p.Add("@P_Troupeid", MDV.TroupeId);
                var results = await Connection.QueryAsync<MemberDetailsView>("USP_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TroupeRegistrationView> TroupeRegistraionBSSO(M_Troupe_Registration P_TUS)
        {
            var p = new DynamicParameters();
            p.Add("@P_ActionCode", P_TUS.Action);
            p.Add("@P_DistId", P_TUS.DistrictId);
            p.Add("@P_BlockId", P_TUS.BlockId);
            p.Add("@P_GpId", P_TUS.GpId);
            p.Add("@P_GroupName", P_TUS.vchGroupName);
            p.Add("@P_LeaderName", P_TUS.vchLeaderName);
            p.Add("@P_Address", P_TUS.vchAddress);
            p.Add("@P_MobileNo", P_TUS.vchMobileNumber);
            p.Add("@P_GrpCreatedOn", P_TUS.GrpCreatedDate);
            p.Add("@P_IsSocietyReg", P_TUS.GrpRegistered);
            p.Add("@P_NoofMembers", P_TUS.TotalMembers);
            p.Add("@P_UploadPhoto1", P_TUS.UpldPht1);
            p.Add("@P_UploadPhoto2", P_TUS.UpldPht2);
            p.Add("@P_UploadPhoto3", P_TUS.GroupPhoto);
            p.Add("@P_UploadVideo", P_TUS.UpldVdo);
            p.Add("@P_UpldRegPhtCopy", P_TUS.UpldRegPhtCopy);
            p.Add("@P_UploadFolderPath", P_TUS.UploadFolderPath);
            p.Add("@mytable", P_TUS.ComponentXml);
            p.Add("@P_CreatedBy", P_TUS.CreatedBy);
            if (P_TUS.TroupeId != 0)
            {
                p.Add("@P_TroupeId", P_TUS.TroupeId);
            }
            p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            TroupeRegistrationView? result = await Connection.QueryFirstOrDefaultAsync<TroupeRegistrationView>("USP_TROUPE_REGISTRATION_DETAILS", p, commandType: CommandType.StoredProcedure);
            result.MsgOut = Convert.ToInt32(p.Get<string>("@P_MSG"));
            return result;
        }
        public async Task<List<TroupeRegistrationView>> TroupeDetails(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "VD");
            objParam.Add("@P_Status", TRV.Status);
            objParam.Add("@P_Troupeid", TRV.TroupeId);
            var results = await Connection.QueryAsync<TroupeRegistrationView>("USP_TROUPE_REGISTRATION_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeEventDetails>> TroupeEventDetails(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "TD");
            objParam.Add("@P_DistId", TRV.DistrictId);
            objParam.Add("@P_BlockId", TRV.BlockId);
            objParam.Add("@P_GPId", TRV.GPId);
            objParam.Add("@P_FromDate", TRV.FromDate);
            objParam.Add("@P_ToDate", TRV.ToDate);
            objParam.Add("@P_Troupeid", TRV.TroupeId);
            var results = await Connection.QueryAsync<TroupeEventDetails>("USP_ASSIGN_TROUPE_EVENT_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeEventDetails>> TroupeEventDetails_Mob(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "TDM");
            objParam.Add("@P_Troupeid", TRV.TroupeId);
            var results = await Connection.QueryAsync<TroupeEventDetails>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeRegistrationView>> TotalRegsitrationDetails(TroupeRegistrationView TRV)
        {
            try
            {
                DynamicParameters objParam = new DynamicParameters();
                objParam.Add("@P_ActionCode", "TD");
                objParam.Add("@P_BlockId", TRV.BlockId);
                objParam.Add("@P_GroupName", TRV.GroupName);
                objParam.Add("@P_DistId", TRV.DistrictId);
                objParam.Add("@P_GpId", TRV.GPId);
                var results = await Connection.QueryAsync<TroupeRegistrationView>("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", objParam, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<EventDetails>> TotalEventDetails(EventDetails TRV)
        {
            try
            {
                DynamicParameters objParam = new DynamicParameters();
                objParam.Add("@P_ActionCode", "TE");
                objParam.Add("@P_BlockId", TRV.BlockId);
                var results = await Connection.QueryAsync<EventDetails>("USP_ADMIN_TROUPE_REGISTRATION_DETAILS", objParam, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TroupeRegistrationView M_TroupeDetails(string Troupeid)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "VD");
            objParam.Add("@P_Troupeid", Troupeid);
            var results = Connection.Query<TroupeRegistrationView>("USP_TROUPE_REGISTRATION_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.FirstOrDefault();
        }
        public async Task<List<ApplicationStatusByAckNo>> GetAllDetailsByAckNo(string? AckNo)
        {
            try
            {
                DynamicParameters objParam = new DynamicParameters();
                objParam.Add("@P_ActionCode", "VS");
                objParam.Add("@AckNumber", AckNo);
                var results = await Connection.QueryAsync<ApplicationStatusByAckNo>("USP_TROUPE_REGISTRATION_DETAILS", objParam, commandType: CommandType.StoredProcedure);
                return results.AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EventDetails> ViewEventDetailForTroupe(int EventId, string action, int P_DIST_BLK_ID, int TroupID)
        {
            List<EventDetails> list = null;
            var p = new DynamicParameters();
            p.Add("@ActionCode ", action);
            p.Add("@EventId", EventId);
            p.Add("@TroupeId", TroupID);
            p.Add("@P_DIST_BLK_ID", P_DIST_BLK_ID);
            p.Add("@MsgOut", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
            list = Connection.Query<EventDetails>("USP_EVENT_DETAILS", p, commandType: CommandType.StoredProcedure).ToList();
            return list;

        }
        #region MasterData_Mobile
        public List<CategoryMaster> M_Category_Master(string BlockId = "", string UserId = "")
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "CAT");
            var results = Connection.Query<CategoryMaster>("USP_MOBILE_MASTER_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        public List<SubCategoryMaster> M_SubCategory_Master(string BlockId = "", string UserId = "")
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "SCAT");
            var results = Connection.Query<SubCategoryMaster>("USP_MOBILE_MASTER_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        public List<GramPanchayatMaster> M_GP_Master(string BlockId, string UserId = "")
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "GP");
            objParam.Add("@P_BlockId", BlockId);
            var results = Connection.Query<GramPanchayatMaster>("USP_MOBILE_MASTER_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        public List<VlgMaster> M_Village_Master(string BlockId, string UserId = "")
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "VLG");
            objParam.Add("@P_BlockId", BlockId);
            var results = Connection.Query<VlgMaster>("USP_MOBILE_MASTER_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        public List<DeptMaster> M_Department_Master(string BlockId = "", string UserId = "")
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "DEPT");
            var results = Connection.Query<DeptMaster>("USP_MOBILE_MASTER_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        public List<SchemeMaster> M_Scheme_Master(string BlockId = "", string UserId = "")
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "SCHEME");
            var results = Connection.Query<SchemeMaster>("USP_MOBILE_MASTER_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        public List<TroupeMaster> M_Troupe_Master(string BlockId = "", string UserId = "")
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "TROUPE");
            var results = Connection.Query<TroupeMaster>("USP_MOBILE_MASTER_DETAILS", objParam, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
        #endregion

        #region Reporting Troupe
        public async Task<List<TroupeReportingView>> TroupeReportingDetails(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "TRD");
            objParam.Add("@P_TroupeId", TRV.TroupeId);
            objParam.Add("@P_Status", TRV.Status);
            var results = await Connection.QueryAsync<TroupeReportingView>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeReportingView>> TroupeReportingSuccess(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "SR");
            objParam.Add("@P_TroupeId", TRV.TroupeId);
            objParam.Add("@P_Status", 5);
            var results = await Connection.QueryAsync<TroupeReportingView>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeReportingView>> TroupeReportingApprovalListForBlock(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "TAL");
            objParam.Add("@P_BlockId", TRV.BlockId);
            objParam.Add("@P_Status", TRV.Status);
            objParam.Add("@FROMDATE", TRV.FromDate);
            objParam.Add("@TODATE", TRV.ToDate);
            var results = await Connection.QueryAsync<TroupeReportingView>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeReportingView>> TroupeReportingApprovalSuccess(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "SRT");
            objParam.Add("@P_BlockId", TRV.BlockId);
            objParam.Add("@P_Status", TRV.Status);
            objParam.Add("@FROMDATE", TRV.FromDate);
            objParam.Add("@TODATE", TRV.ToDate);
            var results = await Connection.QueryAsync<TroupeReportingView>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeReportingView>> TroupeReportingDetails_Payment(TroupeRegistrationView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "TRDP");
            objParam.Add("@P_TroupeId", TRV.TroupeId);
            objParam.Add("@P_Status", TRV.Status);
            var results = await Connection.QueryAsync<TroupeReportingView>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeReportingView>> TroupeReporting(TroupeReportingView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ERD");
            objParam.Add("@P_AssignEventId", TRV.AssignEventId);
            objParam.Add("@P_TroupeId", TRV.TroupeId);
            var results = await Connection.QueryAsync<TroupeReportingView>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeReportingView>> TroupeReportReverting(TroupeReportingView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "ERR");
            objParam.Add("@P_AssignEventId", TRV.AssignEventId);
            objParam.Add("@P_Status", TRV.Status);
            objParam.Add("@P_TroupeId", TRV.TroupeId);
            var results = await Connection.QueryAsync<TroupeReportingView>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<List<TroupeReportingView>> TroupeReportingForApprovalDetails(TroupeReportingView TRV)
        {
            DynamicParameters objParam = new DynamicParameters();
            objParam.Add("@P_ActionCode", "TEAD");
            objParam.Add("@@AssignEventId", TRV.AssignEventId);
            objParam.Add("@P_Status", TRV.Status);
            objParam.Add("@P_TroupeId", TRV.TroupeId);
            var results = await Connection.QueryAsync<TroupeReportingView>("USP_TROUPE_REPORTING", objParam, commandType: CommandType.StoredProcedure);
            return results.AsList();
        }
        public async Task<string> TroupeReportingToBlock(TroupeReporting P_TUS)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", P_TUS.Action);
                p.Add("@P_TroupeId", P_TUS.TroupeId);
                p.Add("@AssignEventId ", P_TUS.AssignEventId);
                p.Add("@Remarks ", P_TUS.Remark);
                p.Add("@UploadPhoto", P_TUS.ImageIdName);
                p.Add("@villageandmember", P_TUS.villageandmember);
                p.Add("@UploadVideo", P_TUS.VdoIdPath);
                p.Add("@CreatedBy", P_TUS.CreatedBy);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_TROUPE_REPORTING", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@P_MSG");
                return returnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<string> M_TroupeReportingToBlock(TroupeReporting_Mobile P_TUS)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "MTD");
                p.Add("@P_TroupeId", P_TUS.TroupeId);
                p.Add("@AssignEventId ", P_TUS.AssignEventId);
                p.Add("@Remarks ", P_TUS.Remark);
                p.Add("@UploadPhoto", P_TUS.ImgIdPath);
                p.Add("@villageandmember", P_TUS.Villageandmember);
                p.Add("@UploadVideo", P_TUS.VdoIdPath);
                p.Add("@CreatedBy", P_TUS.CreatedBy);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_TROUPE_REPORTING", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@P_MSG");
                return returnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> ReportingApproval(TroupeReporting P_TUS)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "TUD");
                p.Add("@P_TroupeId", P_TUS.TroupeId);
                p.Add("@P_Status", P_TUS.Status);
                p.Add("@P_AssignEventId", P_TUS.AssignEventId);
                p.Add("@AuthorityRemrks", P_TUS.Remark);
                p.Add("@AuthorityUploadPhoto", P_TUS.ImageIdName);
                p.Add("@AuthorityUploadVideo", P_TUS.UpldVdo);
                p.Add("@UploadVideo", P_TUS.UpldVdo);
                p.Add("@CreatedBy", P_TUS.CreatedBy);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_TROUPE_REPORTING", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@P_MSG");
                return returnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> M_ReportingApproval(TroupeReporting P_TUS)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@P_ActionCode", "MTUD");
                p.Add("@P_TroupeId", P_TUS.TroupeId);
                p.Add("@P_Status", P_TUS.Status);
                p.Add("@P_AssignEventId", P_TUS.AssignEventId);
                p.Add("@AuthorityRemrks", P_TUS.Remark);
                p.Add("@AuthorityUploadPhoto", P_TUS.ImageIdName);
                p.Add("@AuthorityUploadVideo", P_TUS.UpldVdo);
                p.Add("@UploadVideo", P_TUS.UpldVdo);
                p.Add("@CreatedBy", P_TUS.CreatedBy);
                p.Add("@P_MSG", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);
                await Connection.ExecuteAsync("USP_TROUPE_REPORTING", p, commandType: CommandType.StoredProcedure);
                string returnVal = p.Get<string>("@P_MSG");
                return returnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
