using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CTMS.Model.Entities.Event
{
    public class AssignTroupe_SubmitResponse
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public string? Message { get; set; }
        public int? AssignTroupeStatus { get; set; }
    }
    public class EventDetails_Mob
    {
        public string? ActionCode { get; set; }
        public string? SanctionedAmount { get; set; }
        public int EventId { get; set; }
        public int Status { get; set; }
        public int DepartmentId { get; set; }
        public string? Department { get; set; }
        public string? StatusColour { get; set; }
        public int SchemeId { get; set; }
        public string? Scheme { get; set; }
        public string? Area { get; set; }
        public string? EventTitle { get; set; }
        public string? EventDescription { get; set; }
        public string? StartDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndDate { get; set; }
        public string? EndTime { get; set; }
        public string? StartDateTime { get; set; }
        public string? EndDateTime { get; set; }
        public int Distid { get; set; }
        public string? DistName { get; set; }
        public int BlockId { get; set; }
        public string? BlockName { get; set; }
        public int GPId { get; set; }
        public string? GpName { get; set; }
        public int CreatedBy { get; set; }
        public int TroupID { get; set; }
        public string? GroupName { get; set; }
        public string? ComponentXmlstring { get; set; }
        public List<ComponentXmlDetail>? ComponentXml { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? activeMembers { get; set; }
        public int? totalPerformed { get; set; }

    }
    public class ComponentXmlDetail
    {
        public string? Village { get; set; }
        public string? ImgFolderID { get; set; }
        public string? Area { get; set; }
        public string? DateofPerform { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? OfficerName { get; set; }
        public string? OfficerMobNo { get; set; }
    }
    public class EventDetails
    {
        public string? ActionCode { get; set; }
        public string? ASSIGNID { get; set; }
        public string? VillageId { get; set; }
        public string? DateOfPerform { get; set; }
        public string? SanctionedAmount { get; set; }
        public int EventId { get; set; }
        public int AssignEventId { get; set; }
        public int Status { get; set; }
        public int DepartmentId { get; set; }
        public string? Department { get; set; }
        public string? StatusColour { get; set; }
        public int SchemeId { get; set; }
        public string? Scheme { get; set; }
        public string? Area { get; set; }
        public string? EventTitle { get; set; }
        public string? EventDescription { get; set; }
        public string? StartDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndDate { get; set; }
        public string? EndTime { get; set; }
        public string? StartDateTime { get; set; }
        public string? EndDateTime { get; set; }        
        public int Distid { get; set; }
        public string? DistName { get; set; }
        public int BlockId { get; set; }
        public string? BlockName { get; set; }
        public int GPId { get; set; }
        public string? GpName { get; set; }
        public int CreatedBy { get; set; }
        public int TroupID { get; set; }
        public string? GroupName { get; set; }
        public string? VillageName { get; set; }
        public string? PlaceofPerformance { get; set; }
        public string? ImgFolderID { get; set; }
        public string? AssignOfficerName { get; set; }
        public string? AssignOfficerMobNo { get; set; }
        public XElement? ComponentXml { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? activeMembers { get; set; }
        public int? totalPerformed { get; set; }
    }
    public class EventDetails_mob
    {
        public int TroupID { get; set; }
        public int EventId { get; set; }
        public int AssignEventId { get; set; }
        public int SchemeId { get; set; }
        public string? SchemeName { get; set; }
        public int Distid { get; set; }
        public string? DistName { get; set; }
        public int BlockId { get; set; }
        public string? BlockName { get; set; }
        public int GPId { get; set; }
        public string? GpName { get; set; }
        public string? VillageId { get; set; }
        public string? VillageName { get; set; }
        public string? StartDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndDate { get; set; }
        public string? EndTime { get; set; }
        public string? DateOfPerform { get; set; }
        public string? PlaceofPerformance { get; set; }
        public string? Area { get; set; }
        public string? UploadBannerFolder { get; set; }
        public string? BannerImg { get; set; }
        public string? GroupName { get; set; }
        public string? EventTitle { get; set; }
        public string? EventDescription { get; set; }
        public string? AssignOfficerName { get; set; }
        public string? AssignOfficerMobNo { get; set; }
        public int CreatedBy { get; set; }      
        public string? ImgFolderID { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int Status { get; set; }
        public string? StatusColour { get; set; }
    }
    [Serializable]
    public class TroupeDetails
    {       
        public int? TroupeId { get; set; }
        public string? GroupName { get; set; }
        public string? Area { get; set; }
        public string? Village { get; set; }
        public string? DateofPerform { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? OfficerName { get; set; }
        public string? OfficerMobNo { get; set; }
        public string? ImgFolderID { get; set; }
        public string? AssignEventId { get; set; }
        public string? ASSIGNID { get; set; }
    }
    public class EventDetailsResponse
    {
        public string? Status { get; set; } = "";
        public string? StatusCode { get; set; }
        public string? Message { get; set; } = "";
        public List<EventDetails>? EventDetails { get; set; }
    }
    public class EventStatus
    {
        public int EventId { get; set; }
        public int Status { get; set; }
    }
}
