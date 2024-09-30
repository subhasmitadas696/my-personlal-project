using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.Communication
{
    public class WhatsAppResponseNew
    {
        public int status { get; set; }
        public string? msg { get; set; }
        public string? request_id { get; set; }
        public decimal? responsetime { get; set; }
    }
}
