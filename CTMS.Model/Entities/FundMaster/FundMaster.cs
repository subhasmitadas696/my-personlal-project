using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.FundMaster
{
    public class FundMaster
    {
        public int FundId { get; set; }
        public int DistId { get; set; }
        public int BlockId { get; set; }
        public int DeptId { get; set; }
        public int SchemeId { get; set; }
        public int EventId { get; set; }
        public string? OpeningBalance { get; set; }
        public string? AvlBalance { get; set; }
        public string? FundTransfer { get; set; }
        public int CreatedBy { get; set; }
        public int BankId { get; set; }
        public string? AccountNo { get; set; }

        public string? FundAmount { get; set; }
        public string? Date { get; set; }
        public string? RecAccNo { get; set; }
        public int RecBankId { get; set; }

    }
    public class FundMasterView
    {
        public string? distname { get; set; }
        public string? blockname { get; set; }
        public string? SchemeName { get; set; }
        public string? BANKNAME { get; set; }
        public string? AccountNo { get; set; }
        public string? OpeningBalance { get; set; }
        public string? Date { get; set; }
        public string? ReceiverBankName { get; set; }
        public string? ReceiverAccountNo { get; set; }
        public string? FundTransferAmount { get; set; }
    }
    public class Payment
    {
        public int AssignEventId { get; set; }
        public int STATUS { get; set; }
        public int DistId { get; set; }
        public int BlockId { get; set; }
        public int GpId { get; set; }
        public int TroupeId { get; set; }
        public int EventId { get; set; }
        public string? GroupName { get; set; }
        public string? PAYMENT_TYPE_MODE { get; set; }
        public string? PaymentDate { get; set; }
        public string? DistrictName { get; set; }
        public string? BlockName { get; set; }
        public string? GPName { get; set; }
        public string? ReportingTime { get; set; }
        public string? EventTitle { get; set; }
        public string? DateOfPerform { get; set; }
        public string? PaymentMode { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentAmount { get; set; }
        public string? TransctionID { get; set; }
        public IFormFile? UploadProof { get; set; }
        public string? UploadProofFileName { get; set; }
        public string? UploadFolderPath { get; set; }
    }
}
