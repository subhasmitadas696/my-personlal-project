using CTMS.Model.DistMaster;
using CTMS.Model.Entities.ReportMaster;
using CTMS.Model.Tusers;
namespace CTMS.Repository.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<List<BlockWiseRegistration>> BlockWiseTroupeRegReport(BlockWiseRegistration Bwr);
        Task<List<ReportView>> ArtFormWiseTroupe(ReportView Bwr);
        Task<List<ReportView>> DashboardRegistration(ReportView Bwr);
        Task<List<ReportView>> DashboardPaymentReport(ReportView Bwr);
        Task<List<ReportView>> DashboardPaymentReportSearch(ReportView Bwr);
        Task<List<ReportView>> DashboardPaymentReportDetails(ReportView Bwr);
        Task<List<DistWise_TroupeReport>> Distwise_TroupeReg(DistWise_TroupeReport DT);
        Task<List<PaymentReport>> Distwise_PaymentReport(PaymentReport TRV);
        Task<List<EventReport>> EventReport(EventReport TRV);
        Task<List<EventReport>> EventReportPopUpDetails(EventReport TRV);
        Task<List<EventReport>> EventUpcomingDetails(EventReport TRV);
        Task<List<EventReport>> EventCompleteDetails(EventReport TRV);
        Task<List<EventReport>> EventRescheduleDetails(EventReport TRV);
        Task<List<EventReport>> EventCancelDetails(EventReport TRV);
        Task<List<ArtWiseReport>> TotalRegistrationpopup(ArtWiseReport TRV);
        Task<List<ArtWiseReport>> Totalmemberpopup(ArtWiseReport TRV);
        Task<List<ArtWiseReport>> TotalmemberPendingpopup(ArtWiseReport TRV);
        Task<List<ArtWiseReport>> TotalmemberApprovedpopup(ArtWiseReport TRV);
        Task<List<ArtWiseReport>> TotalRejectededpopup(ArtWiseReport TRV);
        Task<List<DistWise_TroupeReport>> DistTotalRegistrationpopup(DistWise_TroupeReport DTRR);
        Task<List<DistWise_TroupeReport>> DistWiseTotalmemberpopup(DistWise_TroupeReport DTRR);
        Task<List<DistWise_TroupeReport>> DistWisePendingmemberpopup(DistWise_TroupeReport DTRR);
        Task<List<DistWise_TroupeReport>> DistWiseApprovememberpopup(DistWise_TroupeReport DTRR);
        Task<List<DistWise_TroupeReport>> DistWiseRejectededmemberpopup(DistWise_TroupeReport DTRR);
        Task<List<PaymentReport>> DistWisePaymentTotalReportedpopup(PaymentReport PR);
        Task<List<PaymentReport>> DistWisePaymentPandingpopup(PaymentReport PR);
        Task<List<PaymentReport>> DistWisePaymentSuccesspopup(PaymentReport PR);
        Task<List<PaymentReport>> DistWisePaymentFailurepopup(PaymentReport PR);

    }
}
