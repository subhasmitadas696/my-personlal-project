using CTMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.Approval
{
    public class TroupeApproval
    {
        public int TroupeId { get; set; }
        public string? ENCTroupeId { get { return DESEncrypt.Encrypt(TroupeId.ToString()); } }
        public int DistId { get; set; }
        public int BlockId { get; set; }
        public string? GroupName { get; set; }
        public string? DistrictName { get; set; }
        public string? blockname { get; set; }
        public string? gpname { get; set; }
        public string? pincode { get; set; }
        public int MemberCount { get; set; }
        public string? Address { get; set; }
        public string? RegistrationDate { get; set; }
        public int ApprovedStatus { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
    public class TroupeApprovalViewReq
    {
        public int TroupeId { get; set; }
        public string? ENCTroupeId { get { return DESEncrypt.Encrypt(TroupeId.ToString()); } }
    }
    public class TroupeApprovalViewRes
    {
        public List<TroupeBasicInfo>? TroupeInfo { get; set; }
        public List<MemberInfo>? MemberInfo { get; set; }
    }
    public class TroupeBasicInfo
    {
        public int TroupeId { get; set; }
        public string? GroupName { get; set; }
        public string? TroupeCreatedon { get; set; }
        public string? DistrictName { get; set; }
        public string? BlockName { get; set; }
        public string? GPName { get; set; }
        public string? Address { get; set; }
        public int Pincode { get; set; }
        public string? GroupPhoto { get; set; }
        public string? UploadVideo { get; set; }
        public string? Registeredsociety { get; set; }
        public string? RegDocNo { get; set; }
        public string? SocietyRegDoc { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string? UploadBannerFolder { get; set; }
        public string? BannerImg { get; set; }
        public string? ApprovedStatus { get; set; }
    }
    public class MemberInfo
    {
        public int TroupeId { get; set; }
        public string? MemberName { get; set; }
        public string? RoleName { get; set; }
        public string? RoleNameOD { get; set; }
        public string? Role { get; set; }
        public int RoleId { get; set; }
        public int IsLeader { get; set; }
        public string? Gender { get; set; }
        public string? AadharNo { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public string? BankName { get; set; }
        public string? IFSC { get; set; }
        public string? BranchName { get; set; }
        public string? AccountNo { get; set; }
        public string? ApprovedStatus { get; set; }

    }
    public class ApprovalStatus
    {
        public int TroupeId { get; set; }
        public string? ApprovedStatus { get; set; }
        public string? Remark { get; set; }
        public int Uid { get; set; }
    }
    public class TroupeResponse
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public string? Message { get; set; }
        public List<TroupeApproval>? TroupeDetails { get; set; }
    }
    public class TroupeApprovalViewResponse
    {
        public string? Status { get; set; }
        public string? StatusCode { get; set; }
        public string? Message { get; set; }
        public int? ApproveStatus { get; set; }
        public TroupeApprovalViewRes? AllDetails { get; set; }
    }
}
