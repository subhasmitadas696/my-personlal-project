using CTMS.Model.DTOs;
using CTMS.Model.Entities.Event;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.Registration;
namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IRegistraionRepository
    {
        Task<List<ExecutiveMember>> ExecutiveMemberDetails(int PID);
        int GetProfileCount(int BlockId);
        Task<List<GeneralBody>> GeneralMemberDetails(int PID);
        Task<string> DeleteProfiledetails(MyProfile MP);
        Task<string> AddMyProfileDetails(MyProfile MP);
        Task<List<MyProfile>> ViewMyProfile(MyProfile MP);
        Task<(MyProfile, List<ExecutiveMember>, List<GeneralBody>)> GetoneProfileDetails(int PID,int Blockid);
        Task<List<ReviewerPhotolist>> ReviewersPhotoList(int assigneventid,int Troupeid);
        Task<string> TroupeRegistration(M_Troupe_Registration P_TUS);
        Task<TroupeRegistrationView> TroupeRegistraionBSSO(M_Troupe_Registration P_TUS);
        Task<List<TroupeRegistrationView>> TroupeRegistrationView(TroupeRegistrationView TRV);
        Task<List<TroupeRegistrationView>> TroupeDetails(TroupeRegistrationView TRV);
        Task<List<TroupeEventDetails>> TroupeEventDetails(TroupeRegistrationView TRV);
        Task<List<TroupeEventDetails>> TroupeEventDetails_Mob(TroupeRegistrationView TRV);
        Task<List<TroupeRegistrationView>> TotalRegsitrationDetails(TroupeRegistrationView TRV);
        Task<List<EventDetails>> TotalEventDetails(EventDetails TRV);
        TroupeRegistrationView M_TroupeDetails(string Troupeid);
        Task<List<MemberDetailsView>> MemberDetails(MemberDetailsView Md);
        Task<List<EventPerformedDetails>> EventPerformedDetails(EventPerformedDetails Md);
        Task<List<MemberDetailsView>> M_MemberDetails(string Troupeid);
        Task<List<MemberDetailsView>> GetAllDetails(MemberDetailsView Md);
        Task<List<ApplicationStatusByAckNo>> GetAllDetailsByAckNo(string? AckNo);
        List<CategoryMaster> M_Category_Master(string BlockId="" ,string UserId="");
        List<SubCategoryMaster> M_SubCategory_Master(string BlockId="", string UserId="");
        List<GramPanchayatMaster> M_GP_Master(string BlockId, string UserId="");
        List<VlgMaster> M_Village_Master(string BlockId, string UserId="");
        List<DeptMaster> M_Department_Master(string BlockId="", string UserId="");
        List<SchemeMaster> M_Scheme_Master(string BlockId="", string UserId="");
        List<TroupeMaster> M_Troupe_Master(string BlockId="", string UserId="");
        List<EventDetails> ViewEventDetailForTroupe(int EventId, string action, int P_DIST_BLK_ID, int TroupID);
        #region Troupe Reporting
        Task<List<TroupeReportingView>> TroupeReportingDetails(TroupeRegistrationView TRV);
        Task<List<TroupeReportingView>> TroupeReportingSuccess(TroupeRegistrationView TRV);
        Task<List<TroupeReportingView>> TroupeReportingDetails_Payment(TroupeRegistrationView TRV);
        Task<List<TroupeReportingView>> TroupeReporting(TroupeReportingView TRV);
        Task<List<TroupeReportingView>> TroupeReportReverting(TroupeReportingView TRV);
        Task<List<TroupeReportingView>> TroupeReportingForApprovalDetails(TroupeReportingView TRV);
        Task<string> TroupeReportingToBlock(TroupeReporting P_TUS);
        Task<string> M_TroupeReportingToBlock(TroupeReporting_Mobile P_TUS);
        Task<string> ReportingApproval(TroupeReporting P_TUS);
        Task<string> M_ReportingApproval(TroupeReporting P_TUS);
        Task<List<TroupeReportingView>> TroupeReportingApprovalListForBlock(TroupeRegistrationView TRV);
        Task<List<TroupeReportingView>> TroupeReportingApprovalSuccess(TroupeRegistrationView TRV);
        #endregion

    }
}
