using CTMS.Model.DTOs;
using CTMS.Model.BlockMaster;
using static CTMS.Core.Net;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.Registration;
using CTMS.Model.Entities.Approval;
using CTMS.Model.Entities.Event;

namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IApprovalConfigRepository
    {
        Task<List<BlockMaster>> ApprovalConfiguration(BlockMaster MBL);
        Task<int> TakeAction(TakeActionDto Obj);
        Task<string> AdminTroupeRegistration(M_Troupe_Registration P_TUS);        
        Task<List<TroupeRegistrationView>> ViewNewRegistration(TroupeRegistrationView TRV);
        Task<List<MemberDetailsView>> MemberDetails(MemberDetailsView Md);
        Task<List<completedEventsList>> completedEventsList(MemberDetailsView Md);
        Task<List<completedEventsList>> completedEventsList_MOB(string blockid);
        Task<List<pendingApprovalEventList>> pendingApprovalEventList(MemberDetailsView Md);
        Task<List<DashboardDetails>> DashboardDetails(DashboardDetails DD);
        Task<DashboardCount> Dashboardcount(string userid);

        #region Dashboard
        Task<List<DashboardTroupeDetailsCount>> DashboardTroupeDetailsCount(DashboardDetails DD);
        Task<List<DashboardEventDetailsCount>> DashboardEventDetailsCount(DashboardDetails DD);
        Task<List<DashboardArtFormDetailsCount>> DashboardArtFormDetailsCount(DashboardDetails DD);
        Task<List<DashboardArtFormDetailsSummary>> DashboardArtFormSummaryDetails(DashboardDetails DD);
        Task<List<DashboardUpcommingEvent>> DashboardUpcomingEventCount(DashboardDetails DD);
        Task<DashboardTroupeEventSummaryCountRes> DashboardTroupeEventSummaryCount(DashboardDetails DD);
        Task<List<DashboardEventSummary>> DashboardEventSummaryCount(DashboardDetails DD);
        Task<List<DashboardPaymentSummary>> DashboardPaymentSummaryCount(DashboardDetails DD);
        Task<List<DashboardTroupeDetailsCount>> NotificationCount(DashboardDetails DD);

        #endregion
        Task<DashboardCount> DashboardDetailsByScheme(int deptid,int scehemeid, int distblockid);
        List<EventDetails> ViewEventDetails(string action, int Troupeid);
        List<EventDetails_mob> ViewEventDetails_Troupe(string action, int Troupeid);
        List<EventDetails> Troup_Dashboard_Count(string action, int Troupeid);
        List<EventDetails> ViewEventDetails_MOB(string action, int BlockId);
        #region Approval
        Task<List<TroupeApproval>> ApprovalView(TroupeApproval TRV);
        Task<List<TroupeRegistrationView>> ApprovalView_Mob(TroupeRegistrationView TRV);
        Task<TroupeApprovalViewRes> ApprovalTakeActionView(string TroupeId,int status);
        Task<TroupeApprovalViewRes> M_ApprovalTakeActionView(string TroupeId,int status);
        Task<int> ApprovalTakeAction(ApprovalStatus Obj);
        Task<int> M_ApprovalTakeAction(ApprovalStatus Obj);
        #endregion
        Task<List<MemberDetailsView>> GetAllDetails(MemberDetailsView Md);
    }
}
