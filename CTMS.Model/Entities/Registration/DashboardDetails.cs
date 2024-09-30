using CTMS.Model.Entities.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.Registration
{
    public class DashboardDetails
    {
        public int SLID { get; set; }
        public string? NOOFTROUPES { get; set; }
        public string? GPNAME { get; set; }
        public string? TOTALEVENT { get; set; }
        public int blockid { get; set; }
        public int distid { get; set; }
        public int gpid { get; set; }
        public string? fromdate { get; set; }
        public string? todate { get; set; }
        public int? userid { get; set; }
        public string? urtype { get; set; }
        public int Year { get; set; }
    }
    public class DashboardCount
    {
        public string? TOTALEVENTS { get; set; }
        public string? TOTALREGISTRATION { get; set; }
        public string? ACTIVETROUPES { get; set; }
        public string? FUNDTRANSFER { get; set; }
        public string? TOTALFUND { get; set; }
    }
    public class BlockDetailsResponse
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public DashboardCount? DashboardCount { get; set; }
        public List<DashboardDetails>? DashboardDetails { get; set; }
    }
    public class BlockPendingList_Response
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public string? Message { get; set; } 
        public string? troupeRegistrationPendingCount { get; set; }
        public string? eventReportPendingCount { get; set; }
        public string? eventReportCompletedCount { get; set; }
        public List<completedEventsList>? eventReportCompletedList { get; set; }
        public List<EventDetails>? eventReportPendingList { get; set; }
        public List<TroupeRegistrationView>? TroupeRegistrationpendingList { get; set; }
    }
    public class completedEventsList
    {
        public int eventId { get; set; }
        public int SchemeId { get; set; }
        public int Status { get; set; }
        public string? DepartmentName { get; set; }
        public string? Scheme { get; set; }
        public string? EventTitle { get; set; }
        public string? EventDescription { get; set; }
        public string? StartDateTime { get; set; }
        public string? StartDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndDate { get; set; }
        public string? EndTime { get; set; }
        public string? EndDateTime { get; set; }
        public string? Distid { get; set; }
        public string? BlockId { get; set; }
        public string? BlockName { get; set; }
        public string? DistName { get; set; }
    }
    public class pendingApprovalEventList
    {
        public int eventId { get; set; }
        public string? eventName { get; set; }
        public string? eventLocation { get; set; }
        public string? troupeBannerPath { get; set; }
        public string? eventType { get; set; }
        public string? eventStartDate { get; set; }
        public string? eventEndDate { get; set; }
        public string? eventStartTime { get; set; }
        public string? eventEndTime { get; set; }
        public string? imageUrlList { get; set; }
        public string? eventVideoPath { get; set; }
    }
    public class TroupeEventResponse
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public List<EventDetails_mob>? EventDetails_Troupe { get; set; }
        public List<TroupeReportingView>? EventsToBePerformedToTroupe { get; set; }
        public List<TroupeReportingView>? EventsPerformedSuccessful { get; set; }
        public List<TroupeEventDetails>? EventsPaymentSuccessful { get; set; }
    }
    public class DashboardTroupeDetailsCount
    {
        public string? Total_Registered { get; set; }
        public string? Total_Pending { get; set; }
        public string? Total_Approved { get; set; }
        public string? Total_Rejected { get; set; }
        public string? Total_Not_Audition { get; set; }
        public string? Percentage_Approved { get; set; }
        public string? Percentage_Rejected { get; set; }
        public string? Percentage_Pending { get; set; }
        public string? PerformanceApprovalCount { get; set; }
        public string? TroupeApprovalCount { get; set; }
        public string? TotalNotiCount { get; set; }

    }
    public class DashboardEventDetailsCount
    {
        public string? Total_Events { get; set; }
        public string? Total_Events_Upcoming { get; set; }
        public string? Total_Events_Completed { get; set; }
        public string? Percentage_Events_Completed { get; set; }
        public string? Percentage_Events_Upcoming { get; set; }
    }
    public class DashboardArtFormDetailsCount
    {
        public string? TotalTroupe { get; set; }
        public string? ActiveTroupes { get; set; }
        public string? Total_Members { get; set; }
        public string? TotalArtForm { get; set; }
    }
    public class DashboardArtFormDetailsSummary
    {
        public string? CategoryName { get; set; }
        public string? TotalTroupes { get; set; }
        public string? ActiveTroupes { get; set; }
        public string? Total_Members { get; set; }
        public string? TotalArtForm { get; set; }
        public string? event_Completed { get; set; }
        public string? event_Upcoming { get; set; }
        public string? Total_Payment_in_Lakhs { get; set; }
    }
    public class DashboardUpcommingEvent
    {
        public string? DistBlkGPName { get; set; }
        public string? UpcomingEvents { get; set; }
        public string? TotalUpcomingEvents { get; set; }
    }
    public class DashboardTroupeEventSummaryCountRes
    {
        public List<DashboardTroupeEventDistCount>? DashboardTroupeDistEvent { get; set; }
        public List<DashboardTroupeEventBlockCount>? DashboardTroupeBlockEvent { get; set; }
    }
    public class DashboardTroupeEventDistCount
    {
        public string? DISTCODE { get; set; }
        public string? DistrictName { get; set; }
        public string? TotalTroupes { get; set; }
        public string? TotalMembers { get; set; }
        public string? ActiveTroupes { get; set; }
        public string? Upcoming_Events { get; set; }
        public string? Comp_Events { get; set; }
        public string? TotalPaymentAmount { get; set; }
    }
    public class DashboardTroupeEventBlockCount
    {
        public string? DISTCODE { get; set; }
        public string? BLOCKCODE { get; set; }
        public string? BLOCKNAME { get; set; }
        public string? TotalTroupes { get; set; }
        public string? TotalMembers { get; set; }
        public string? ActiveTroupes { get; set; }
        public string? Upcoming_Events { get; set; }
        public string? Comp_Events { get; set; }
        public string? TotalPaymentAmount { get; set; }
    }
    public class DashboardEventSummary
    {
        public string? MonthName { get; set; }
        public string? EventCount { get; set; }
    }
    public class DashboardPaymentSummary
    {
        public string? FundAvailable { get; set; }
        public string? Disbursed { get; set; }
        public string? RemainingBalance { get; set; }
        public string? DisbursedPercentage { get; set; }
        public string? RemainingBalancePercentage { get; set; }
    }
}
