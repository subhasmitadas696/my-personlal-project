using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CTMS.Model.Entities.PaymentStructure
{
    public  class PaymentStructureMaster
    {
        public int Id { get; set; }
        public int SchemeId { get; set; }
        public int CategoryId { get; set; }
        public int NoOfArtist { get; set; }
        public int Remuneration { get; set; }
        public int Contingency { get; set; }
        public int Total { get; set; }
        public string? CreatedBy { get; set; }
        [NotMapped]

        public string? CategoryName { get; set; }
        public string? SCHEMENAME { get; set; }
        public string? CategoryIdList { get; set; }
        public string? CatIdList { get; set; }
        public XElement? PaymentStr { get; set; }
        public string? ActionCode { get; set; }
    }
}
