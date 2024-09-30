namespace CTMS.Web.Areas.AdminConsole.Models.User
{
    public class User
    {

        public string? ActionCode { get; set; }
        public int? SLNo { get; set; }
        public int? UserId { get; set; }
        public int? intuserid { get; set; }
        public string? vchusername { get; set; }
        public string? vchfullname { get; set; }
        public int? intdesignationid { get; set; }        
        
        public string? nvchdesigname { get; set; }
        public string? vchemail { get; set; }
        public string? vchmobile { get; set; }
        public int DistorBlockId { get; set; }
        public int DistId { get; set; }
       
        public string? nvchlevelname { get; set; }
        public string? vchgender { get; set; }
        public int? levelid { get; set; }
        public int? intleveldetailid { get; set; }
        public int? L1 { get; set; }
        public int? L2 { get; set; }
        public int? L3 { get; set; }
        public int? L4 { get; set; }
        public int? hid { get; set; }

        public int? GetID { get; set; }

        public int? HierarchyId { get; set; }

        public int? LevelId { get; set; }

        public string? UserID { get; set; }

        public int? RowNo { get; set; }

        public string? FullName { get; set; }

        //public string? VCHFULLNAME { get; set; }
        public string? UserName { get; set; }

        public string? UserPasswaord { get; set; }
        public string? UPASSWORD { get; set; }
        public string? UEMAILID { get; set; }
        public string? MOBILENO { get; set; }
        public string? DESIGNAME { get; set; }

        public string? DepartmentID { get; set; }

        public string? DesignationID { get; set; }
        public string? DistorBlockID { get; set; }

        //public string? NVCHDESIGNAME { get; set; }



        public int? intDesignationID { get; set; }

        public int? AccessLevel { get; set; }


        public string? DateOfJoing { get; set; }

        public string? DateOfBirth { get; set; }

        public string? security { get; set; }

        public string? AdditionalDepartment { get; set; }


        public string? PrsentAddress { get; set; }

        public string? OfficeTelephone { get; set; }


        public string? Mobile { get; set; }


        public string? email { get; set; }

        public string? ReportDepartment { get; set; }

        public string? RepotingptAccesslable { get; set; }

        public string? ReportingUserName { get; set; }

        public int? ReportingUserId { get; set; }

        public string? TempReportingUserName { get; set; }

        public int? TempReportingUserId { get; set; }

        public string? status { get; set; }

        public string? AdminPrevilliage { get; set; }

        public int? Location { get; set; }


        public int? intLocation { get; set; }

        public string? SubAdminPrevillage { get; set; }

        public string? Secretary { get; set; }

        public string? Religion { get; set; }

        public string? Attendance { get; set; }


        public string? Gender { get; set; }

        public string? AdditionalResponsibilities { get; set; }

        public string? OptionalReportingUserName { get; set; }

        public int? OptionalReportingUserId { get; set; }

        public string? SectionID { get; set; }

        public string? Type { get; set; }

        public string? DomainUserName { get; set; }

        public string? OfficeID { get; set; }

        public string? EmployeeCode { get; set; }

        public string? ExchangeUser { get; set; }

        public string? EschangePassword { get; set; }

        public string? ReportingType { get; set; }

        public DateTime ProbationCompleteDate { get; set; }

        public string? Reignationstatus { get; set; }

        public DateTime LeavingDate { get; set; }

        public int? GradeId { get; set; }

        public string? GradeName { get; set; }

        public string? Reason { get; set; }

        public string? Payroll { get; set; }

        public int? EPF { get; set; }

        public int? ShiftID { get; set; }

        public int? CreatedBy { get; set; }

        public int? PositionId { get; set; }

        public string? LevelName { get; set; }

        public int? ParentId { get; set; }

        public int? LeveleID { get; set; }

        public string? officecordid { get; set; }

        public string? FatherNameEnglish { get; set; }

        public string? FatherNameAmharic { get; set; }

        public string? GrandFatherNameEnglish { get; set; }

        public string? GrandFatherNameAmharic { get; set; }


        public string? UserImage { get; set; }
        public string? vchuserimage { get; set; }
        public int? UserType { get; set; }
        
        public string? nvchlocation { get; set; }
        
        public string? vchmobtel { get; set; }


    }
    public class LevelDetail
    {
        public int? INTLEVELDETAILID { get; set; }
        public string? NVCHLEVELNAME { get; set; }
        public string? nvchlabel { get; set; }
    }
    public class Location
    {
        public int? INTLOCATIONID { get; set; }
        public string? NVCHLOCATION { get; set; }
    }
    public class Designation
    {
        public int? INTDESIGID { get; set; }
        public int? LEVELID { get; set; }
        public int? USERTYPE { get; set; }
        public int? BLOCKID { get; set; }
        public int? DISTID { get; set; }
        public string? NVCHDESIGNAME { get; set; }
        public string? DESIGNATIONNAME { get; set; }
        public object NVCHALIASNAME { get; internal set; }
        public object INTCREATEDBY { get; internal set; }
        public int? ROLEID { get; set; }
        public string? ROLENAME { get; set; }
    }
    public class LinkAccess
    {
        public int intaccessid { get; set; }
        public int UID { get; set; }
        public int intuserid { get; set; }
        public int intplinkid { get; set; }
        public int tinaccess { get; set; }
        public int intglinkid { get; set; }
        public string nvchglinkname { get; set; }
        public string nvchplinkname { get; set; }
        public int intfunctionid { get; set; }
        public string vchfunction { get; set; }
        public string vchfilename { get; set; }
        public string vchiconclass { get; set; }
        public int Aglinkid { get; set; }
        public int Aplinkid { get; set; }
        public int FDelete { get; set; }
    }
}
