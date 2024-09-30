using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.PageMaster
{
    public class PageMaster
    {
        public int Id { get; set; }
        public string? PageTitle { get; set; }
        public string? PageTitleOD { get; set; }
        public string? PageDescription { get; set; }
        public string? PageDescriptionOD { get; set; }
        public int CreatedBy { get; set; }
    }
}
