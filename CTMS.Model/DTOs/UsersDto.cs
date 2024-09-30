using CTMS.Model.Entities.Event;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.Registration;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CTMS.Model.DTOs
{
    public class UsersDto
    {
        public long UID { get; set; }
        public string? FULLNAME { get; set; } = "";
        public string? TROUPENAME { get; set; } = "";
        public string? MEMBERNAME { get; set; } = "";
        public string? MOBILENO { get; set; }
        public string? CategoryName { get; set; } = "";
        public string? SubCategoryName { get; set; } = "";
        public string? UserName { get; set; } = "";
        public string? Password { get; set; } = "";
        public string? Email { get; set; } = "";
        public int USERID { get; set; }
        public string? URTYPE { get; set; } = "";
        public string? USERTYPE { get; set; } = "";
        public int ROLEID { get; set; }
        public string? DISTRICTNAME { get; set; } = "";
        public string? BLOCKNAME { get; set; } = "";
        public int DISTRICTID { get; set; }
        public string? GPNAME { get; set; } = "";
        public int GPID { get; set; }
        public int BLOCKID { get; set; }
        public int FaildCount { get; set; } // Typo in original SQL?
        public bool LockoutEnable { get; set; }
        public bool ActiveStatus { get; set; }
        public int TroupeId { get; set; }
        public int IsLeader { get; set; }
    }
    public class ApiResponse
    {
        public int EventReturnValue { get; set; }
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public int? activeMembers { get; set; }
        public int? totalPerformed { get; set; }
        public List<UsersDto>? LoginResponse { get; set; }
        public List<GpDetails>? GpDetails { get; set; }
        public TroupeRegistrationView? TroupeRegistrationView { get; set; }
        public List<MemberDetailsView>? MemberDetailsView { get; set; }
        public IList<DropdownData>? GetMasterData { get; set; }
        public List<ApplicationStatusByAckNo> ?ApplicationStatusByAckNo{get;set;}
    }
    public class GpDetails
        {
        public string? GPNAME { get; set; }
        public int GPID { get; set; }
    }
    public class VerifyOfficerDetails
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public List<VOResponse>VOdetails{get;set;}

    }
    public class VOResponse
    {
        public string? MOBILENO { get; set; }
        public string? FULLNAME { get; set; } = "";
        public string? Password { get; set; } = "";
    }
    public class VerifyOtp_Reviewer
    {        
        public string? Department { get; set; }
        public string? Status { get; set; }
        public string? Scheme { get; set; } 
        public string? EventTitle { get; set; } 
        public string? EventDescription { get; set; }
        public string? StartDateTime { get; set; } 
        public string? EndDateTime { get; set; } 
        public string? DistName { get; set; } 
        public string? BlockName { get; set; } 
        public string? GPNAME { get; set; } 
        public string? TroupeName { get; set; } 
        public int ROLEID { get; set; }
        public int TroupeId { get; set; }
        public int GpId { get; set; }
        public int BlockId { get; set; }
        public int Distid { get; set; }
        public int SchemeId { get; set; }
        public int EventId { get; set; }
        public int villageid { get; set; }
        public string? villagename { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? DATEOFPERFORM { get; set; }
    }
    public class VerifyOtp_Reviewer_Response
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public string? Message { get; set; }
        public List<VerifyOtp_Reviewer>? ListEventdetails { get; set; }
    }
    public class Reviewer_SubmitRequest
    {
        public string? EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventStartDate { get; set; }
        public string? EventEndDate { get; set; }
        public string? TroupeId { get; set; }        
        public string? CreatedBy { get; set; }        
        public string? ImgPath { get; set; }        
        public string? Videopath { get; set; }
        public string? DateOfPerformance { get; set; }
        public List<eventDetails>? EventDetails { get; set; }
    }
    public class eventDetails
    {
        public string? VillageId { get; set; }        
        public string? EventStartTime { get; set; }
        public string? EventEndTime { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        [XmlIgnore]
        public IFormFile? UpldVdo { get; set; }
        [XmlIgnore]
        public IFormFile? UpldImg1 { get; set; }
        [XmlIgnore]
        public IFormFile? UpldImg2 { get; set; }
        [XmlIgnore]
        public IFormFile? UpldImg3 { get; set; }
    }
    public class Reviewer_SubmitResponse
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public string? Message { get; set; }
        public int? ReviewerSubmitStatus { get; set; }
    }
    public class ReviewerPhotolist
    {
        public int AssignEventId { get; set; }
        public string? ReviewersPhotolist { get; set; }
        public string? ReviewersVideolist { get; set; }
    }
}

