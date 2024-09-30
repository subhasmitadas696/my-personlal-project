using System.Web;
using System;
using Microsoft.AspNetCore.Http;
using CTMS.Core;
using System.Xml.Serialization;
using CTMS.Model.DTOs;

namespace CTMS.Model.Entities.Registration
{
    public class TroupeRegistrationView
    {
        public int TroupeId { get; set; }

        public string? ENCTroupeId { get { return DESEncrypt.Encrypt(TroupeId.ToString()); } }
        public string? TroupeName { get; set; }
        public int Status { get; set; }
        public string? AStatus { get; set; }
        public string? DistrictName { get; set; }
        public int? DistrictId { get; set; } = 0;
        public string? BlockName { get; set; }
        public int? BlockId { get; set; } = 0;
        public string? GPName { get; set; }
        public int? GPId { get; set; } = 0;
        public string? GroupName { get; set; }
        public string? LeaderName { get; set; }
        public string? MobileNo { get; set; }
        public int MemberCount { get; set; }
        public int MsgOut { get; set; }
        public string? FolderName { get; set; }
        public int ApprovedStatus { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public int TotalPerformed { get; set; }
        public string? GrpCreatedOn { get; set; }
        public string? Address { get; set; }
        public int Pincode { get; set; }
        public string? SocReg { get; set; }
        public string? RegDocNo { get; set; }
        public string? SocietyRegDoc { get; set; }
        public string? UploadBannerFolder { get; set; }
        public string? BannerImg { get; set; }
        public string? pincode { get; set; }
        public int IsSocietyReg { get; set; }
        public string? SocietyRegDate { get; set; }
        public string? UploadVideo { get; set; }
        public string? GroupPhoto { get; set; }
        public string? FromDate { get; set; } = null;
        public string? ToDate { get; set; } = null;
        public List<MemberDetailsView>? MemberDetails { get; set; }
         

    }
    public class EventPerformedDetails
    {
        public string? ReportedDate { get; set; }
        public int StatusId { get; set; }
        public string? Status { get; set; }
        public string? EventTitle { get; set; }
        public string? DateOfPerform { get; set; }
        public string? VLGNAME { get; set; }
        public string? GPName { get; set; }
        public int EventId { get; set; }
        public int DISTID { get; set; }
        public int TroupeId { get; set; }
        public int BlockId { get; set; }
        public int GpId { get; set; }
    }
    public class MemberDetailsView
    {
        public string? AcknowledgementNo { get; set; }
        public int? MemberId { get; set; }
        public int Pincode { get; set; }
        public string? SocietyRegactNo { get; set; }
        public string? SocietyRegDate { get; set; }
        public string? Email { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int TroupeId { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? MemberFatherName { get; set; }
        public string? MemberDob { get; set; }
        public int DistId { get; set; }
        public int MemDistId { get; set; }
        public int BlockId { get; set; }
        public int MemBlockId { get; set; }
        public int GpId { get; set; }
        public int MemGpId { get; set; }
        public int NoofMembers { get; set; }
        public string? DistrictName { get; set; }
        public string? MemberDistrictName { get; set; }
        public string? BlockName { get; set; }
        public string? MemberBlockName { get; set; }
        public string? GPName { get; set; }
        public string? MemberGPName { get; set; }
        public string? MemberName { get; set; }
        public string? GroupName { get; set; }
        public string? LeaderName { get; set; }
        public string? MobileNo { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public int IsLeader { get; set; }
        public int IsSocietyReg { get; set; }
        public string? GrpCreatedOn { get; set; }
        public string? AadharNo { get; set; }
        public string? AccountNo { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? IFSC { get; set; }
        public string? SocietyRegDoc { get; set; }
        public string? UploadPhoto1 { get; set; }
        public string? UploadPhoto2 { get; set; }
        public string? UploadVideo { get; set; }
        public string? GroupPhoto { get; set; }
        public string? EmailId { get; set; }
        public string? Gender { get; set; }
        public string? BannerImg { get; set; }
        public string? RegDocNo { get; set; }
        public string? UploadBannerFolder { get; set; }
        public int ProfileStatus { get; set; }
    }
    public class TroupeReportingView
    {
        public int FOStatus { get; set; }
        public int TroupeId { get; set; }
        public string? MemberId { get; set; }
        public string? AStatus { get; set; }
        public string? MemberName { get; set; }
        public int Status { get; set; }
        public int AssignEventId { get; set; }
        public int TroupeCount { get; set; }
        public int EventId { get; set; }
        public string? DateOfPerform { get; set; }
        public string? GroupName { get; set; }
        public string? EventTitle { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? DistrictName { get; set; }
        public string? BlockName { get; set; }
        public string? GPName { get; set; }
        public string? VLGNAME { get; set; }
        public string? Area { get; set; }
        public int? DistId { get; set; }
        public int? BlockId { get; set; }
        public int? GpId { get; set; }
        public int? VillageId { get; set; }
        public string? AssignOfficerName { get; set; }
        public string? AssignOfficerMobNo { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? ReportingTime { get; set; }
        public string? TroupeRemarks { get; set; }
        public string? UploadPhoto { get; set; }
        public string? UploadVideo { get; set; }
        public string? CreatedBy { get; set; }
        public string? AuthorityRemarks { get; set; }
        public string? MemberPerformed { get; set; }
        public int MemberCount { get; set; }
        public string? ImgFolderID { get; set; }
        public string? vdoid { get; set; }
    }
    public class Block_Profile_Response
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public List<Block_Profile_Response_lIst>? Block_Profile_Response_lIst { get; set; }       
    }
    public class TroupeVwResponse
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public List<TroupeReportingView>? TroupeReportedApprovalList { get; set; }
        public List<ReviewerPhotolist>? ReviewersPhotoVideo { get; set; }
    }
    public class Block_Profile_Response_lIst
    {
        public List<TroupeRegistrationView>? TroupeRegistrationDetails { get; set; }
    }    
        
    public class TroupeEventDetails
    {
        public int EventId { get; set; }
        public int DISTID { get; set; }
        public int BlockId { get; set; }
        public int GpId { get; set; }
        public int VillageId { get; set; }
        public string? EventTitle { get; set; }
        public string? DistrictName { get; set; }
        public string? BlockName { get; set; }
        public string? GPName { get; set; }
        public string? DateOfPerform { get; set; }
        public string? SanctionedAmount { get; set; }
        public string? VLGNAME { get; set; }
        public string? EventStatus { get; set; }
        public int PaymentAmount { get; set; }


    }
    public class TroupeReporting
    {
        public int AssignEventId { get; set; }
        public int TroupeId { get; set; }
        public int Status { get; set; }
        public List<IFormFile>? ImagesFile { get; set; }
        public string? ImageIdName { get; set; }
        public string? Remark { get; set; }
        public string? UpldVdo { get; set; }
        public IFormFile? Upldvdofile { get; set; }
        public string? CreatedBy { get; set; }
        public string? ImgPath { get; set; }
        public string? Action { get; set; }
        public string? ImgIdPath { get; set; } = "";
        public string? VdoIdPath { get; set; } = "";
        public string? villageandmember { get; set; } = "";
    }
    public class TroupeReportingRequests
    {
        public int AssignEventId { get; set; }
        public string? UpldVdo { get; set; }
        public string? ImageIdName { get; set; }
        public int TroupeId { get; set; }
        public int Status { get; set; }
        public string? Remark { get; set; }
        public string? CreatedBy { get; set; }
    }
    public class TroupeReportingForEvent_Response
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public List<TroupeReportingView>? TroupeReporting { get; set; }
    }
    public class TroupeReporting_Response
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public string? TroupeReportingStatus { get; set; }
    }
    public class TroupeReporting_Mobile
    {        
        public int TroupeId { get; set; }
        public int AssignEventId { get; set; }
        public string? ImgIdPath { get; set; } 
        public string? VdoIdPath { get; set; }
        public string? Remark { get; set; }
        public string? CreatedBy { get; set; }
        public string? Villageandmember { get; set; }
        public List<TroupeReportingRequest>? TroupeReportPhotovideolist { get; set; }
    }
    public class TroupeReportingRequest
    {
        public int VillageId { get; set; }
        [XmlIgnore]
        public IFormFile? UpldVdo { get; set; }
        [XmlIgnore]
        public IFormFile? UpldImg1 { get; set; }
        [XmlIgnore]
        public IFormFile? UpldImg2 { get; set; }
        [XmlIgnore]
        public IFormFile? UpldImg3 { get; set; }
    }
    public class PerformanceApprovalResponse
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public string? PerformanceApprovalStatus { get; set; }
    }
}
