using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Model.DTOs
{
    public class TakeActionDto
    {
        public int Troupeid { get; set; }
        public int Status { get; set; }
        public string? Remarks { get; set; }
    }
}
