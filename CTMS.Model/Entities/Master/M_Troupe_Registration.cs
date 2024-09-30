using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CTMS.Model.Entities.Master
{
#pragma warning disable
    public class MyProfile
    {
        public string societyregcert { get; set; }
        public string MemberCount { get; set; }
        public string MemberCountss { get; set; }
        public IFormFile societyregcertfiles { get; set; }
        public string bylawfile { get; set; }
        public IFormFile Bylawfiles { get; set; }
        public IFormFile Bymemorandumfiles { get; set; }
        public string bymemorandumfile { get; set; }
        public string societyregnumber { get; set; }
        public string societyregdate { get; set; }
        public string Bkssaccountno { get; set; }
        public string mettingresolution { get; set; }
        public string generalbodyformdate { get; set; }
        public string vicepresidentname { get; set; }
        public string positionheldfrom { get; set; }
        public string createdby { get; set; }
        public string PID { get; set; }
        public string BLOCKID { get; set; }
        public List<ExecutiveMember>?Executives { get; set; }
        public List<GeneralBody>?Generals { get; set; }
        public XElement? ExecutiveXml { get; set; }
        public XElement? GeneralsXml { get; set; }
    }
    public class ExecutiveMember
    {
        public string ExecutiveMemberName { get; set; }
        public string ExecutiveMemberMobile { get; set; }
    }
    public class GeneralBody
    {
        public string GeneralBodyMemberName { get; set; }
        public string GeneralBodyMobile { get; set; }
    }
    public class M_Troupe_Registration
    {

        public XElement ComponentXml { get; set; }

        [Required(ErrorMessage = "DistrictId is required.")]
        public int DistrictId { get; set; }
        public int Pincode { get; set; }
        public string? SocietyRegactNo { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        public int TroupeId { get; set; }

        [Required(ErrorMessage = "DistrictName is required.")]
        public string? DistrictName { get; set; }

        [Required(ErrorMessage = "BlockId is required.")]
        public int? BlockId { get; set; }

        [Required(ErrorMessage = "BlockName is required.")]
        public string? BlockName { get; set; }

        [Required(ErrorMessage = "GpId is required.")]
        public int? GpId { get; set; }

        [Required(ErrorMessage = "vchGroupName is required.")]
        public string? vchGroupName { get; set; }

        [Required(ErrorMessage = "vchLeaderName is required.")]
        public string? vchLeaderName { get; set; }

        [Required(ErrorMessage = "vchMobileNumber is required.")]
        public string? vchMobileNumber { get; set; }

        [Required(ErrorMessage = "vchAddress is required.")]
        public string? vchAddress { get; set; }

        [Required(ErrorMessage = "GrpCreatedDate is required.")]
        public string? GrpCreatedDate { get; set; }

        [Required(ErrorMessage = "GrpRegistered is required.")]
        public string? GrpRegistered { get; set; }

        [Required(ErrorMessage = "TotalMembers is required.")]
        public int? TotalMembers { get; set; }
        public IFormFile UploadBanner { get; set; }
        public string UploadBannerImage { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string? UpldPht1 { get; set; }
        public IFormFile UpldPht1Img { get; set; }
        public string? UpldPht2 { get; set; }
        public IFormFile UpldPht2Img { get; set; }
        public string? GroupPhoto { get; set; }
        public IFormFile GroupPhotoImg { get; set; }
        public string? UpldVdo { get; set; }
        public IFormFile Upldvdofile { get; set; }
        public string UpldRegPhtCopyImage { get; set; }
        public IFormFile UpldRegPhtCopy { get; set; }
        public string? UploadFolderPath { get; set; }
        public int CreatedBy { get; set; }
        public int ApproveStatus { get; set;}
        public string Action { get; set; }
        public string? RegistrationDate { get; set; }

    }
    [Serializable]
    public class MemberDetails
    {
        public string MemberName { get; set; }
        public string? MemberDob { get; set; }
        public string? MemberFatherName { get; set; }
        public int? RoleId { get; set; }
        public string Role { get; set; }
        public int? MemberDistId { get; set; }
        public int? MemberBlockId { get; set; }
        public int? MemberGpId { get; set; }
        public string AadharNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string IFSCCode { get; set; }
        public string NameOfBank { get; set; }
        public string BankID { get; set; }
        public string BranchID { get; set; }
        public string BranchName { get; set; }
        public string BankAccountNo { get; set; }
        public int? IsLeader { get; set; }
    }
    public class ApplicationStatusByAckNo
    {
        public string ApprovedOnDSSO { get; set; }
        public string DSSORemarks { get; set; }
        public string ApprovedOnBSSO { get; set; }
        public string? GroupName { get; set; }
        public string? GrpCreatedOn { get; set; }
        public int ApprovedStatus { get; set; }
        public int DSSOApproveStatus { get; set; }
        public string? Remarks { get; set; }
    }
}
