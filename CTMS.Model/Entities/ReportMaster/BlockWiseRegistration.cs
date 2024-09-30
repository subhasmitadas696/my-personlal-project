using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.ReportMaster
{
    public class BlockWiseRegistration
    {
        public int? TroupeId { get; set; }
        public string? MemberName { get; set; }
        public string? TotalEvents { get; set; }
        public string? GroupName { get; set; }
        public string? GrpCreatedOn { get; set; }
        public string? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? BlockId { get; set; }
        public string? BlockName { get; set; }
        public string? GPName { get; set; }
        public string? Address { get; set; }
        public string? pincode { get; set; }
        public string? MobileNo { get; set; }
        public int? ApprovedStatus { get; set; }
        public string? TStatus { get; set; }
        public int? GPId { get; set; }
        public int? DistId { get; set; }
       
    }
    public class ReportView
    {
        public int TroupeId { get; set; }
        public int CatId { get; set; }
        public int SubCatId { get; set; }
        public int DISTCODE { get; set; }
        public int BlockId { get; set; }
        public string? DistrictName { get; set; }
        public int? TTotalReg { get; set; }
        public string? BlockName { get; set; }
        public int? TTotalMembers { get; set; }
        public int? TTotalRegPending { get; set; }
        public int? TTotalRegApproved { get; set; }
        public int? TTotalRegRejected { get; set; }
        public int DistId { get; set; }
        public int GPId { get; set; }
        public int isdist { get; set; }
        public string? UserType { get; set; }
        public int Role { get; set; }
        public int? AvailableBalance { get; set; }
        public int? FundDisburse { get; set; }
        public int? Pending { get; set; }
        public string? DistName { get; set; }
        
    }
    public class DistWise_TroupeReport
    {
        public string? UserType { get; set; }
        public int Categoryid { get; set; }
        public int isCat { get; set; }
        public string? GroupName { get; set; }
        public string? GrpCreatedOn { get; set; }
        public string? CategoryName { get; set; }
        public int SubCategoryid { get; set; }
        public int Gpid { get; set; }
        public int Distid { get; set; }
        public int Blockid { get; set; }
        public int Gpidd { get; set; }
        public string? DistrictName { get; set; }
        public string? BlockName { get; set; }
        public string? GPNAME { get; set; }
        public string? AccountNo { get; set; }
        public string? AadharNo { get; set; }
        public string? Role { get; set; }
        public string? MemberName { get; set; }
        public string? SubCategoryName { get; set; }
        public string? Toal_Reg { get; set; }
        public string? Total_Pending { get; set; }
        public string? Total_Approved { get; set; }
        public string? Total_Rejected { get; set; }
        public string? Total_Members { get; set; }
        public int? IsLeader { get; set; }
    }
    public class PaymentReport
    {
        public int Distid { get; set; }
        public int Blockid { get; set; }
        public int Gpid { get; set; }
        public string? DistrictName { get; set; }
        public string? TroupName { get; set; }
        public string? BlockName { get; set; }
        public string? GPNAME { get; set; }
        public string? villagePerform { get; set; }
        //public string? villageId { get; set; }
        public string? EventTitle { get; set; }
        public string? DateOfPerform { get; set; }
        public string? reportedon { get; set; }

        public int isdist { get; set; }
        public int Total_Reported { get; set; }
        public string? Pending_payment { get; set; }
        public string? success_payment { get; set; }
        public string? fail_payment { get; set; }
        public string? Districtname { get; set; }
        public string? UserType { get; set; }
    }
    public class EventReport
    {
        public int DistId { get; set; }
        public int URTYPE { get; set; }
        public int isdist { get; set; }
        public int EventId { get; set; }
        public string? NoOfEvents { get; set; }
        public int BlockId { get; set; }
        public int GPId { get; set; }
        public string? DistrictName { get; set; }
        public string? GPNAME { get; set; }
        public string? VLGNAME { get; set; }
        public string? EventTitle { get; set; }
        public string? CREATEDBY { get; set; }
        public string? EventDescription { get; set; }
        public string? DEPTNAME { get; set; }
        public string? SCHEMENAME { get; set; }
        public string? GroupName { get; set; }
        public string? PGroupName { get; set; }
        public string? STARTDATE { get; set; }
        public string? ENDDATE { get; set; }
        public string? BlockName { get; set; }
        public string? GPName { get; set; }
        public string? Department { get; set; }
        public string? Scheme { get; set; }
        public string? CompletedEvents { get; set; }
        public string? UpcomingEvents { get; set; }
        public string? RescheduledEvents { get; set; }
        public string? CancelledEvents { get; set; }
    }
    public class ArtWiseReport
    {
        public int DistId { get; set; }
        public int TroupeId { get; set; }
        public int isdist { get; set; }
        public int BlockId { get; set; }
        public int GPId { get; set; }
        public int CatId { get; set; }
        public int SubCatId { get; set; }
        public int IsLeader { get; set; }
        public int DISTCODE { get; set; }
        public string? DistrictName { get; set; }
        public string? AccountNo { get; set; }
        public string? AadharNo { get; set; }
        public string? GPName { get; set; }
        public string? BlockName { get; set; }
        public string? MemberName { get; set; }
        public string? RoleName { get; set; }
        public string? Role { get; set; }
        public string? RoleNameOD { get; set; }
        public string? RoleOd { get; set; }
        public string? GroupName { get; set; }
        public string? GrpCreatedOn { get; set; }
        public int? TTotalReg { get; set; }
        public string? MemberCount { get; set; }
        public int? TTotalMembers { get; set; }
        public int? TTotalRegPending { get; set; }
        public int? TTotalRegApproved { get; set; }
        public int? TTotalRegRejected { get; set; }
    }
}
