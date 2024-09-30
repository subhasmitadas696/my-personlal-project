using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.ImageList
{
    public class ImageList
    {
        public string? imageid { get; set; }
        public string? villageid { get; set; }
        public string? imagename { get; set; }
        public string? villageName { get; set; }
        public string? Status { get; set; }
        public int? MemberCount { get; set; }
        public string? startTime { get; set; }
    }
    public class ImageListmob
    {
        public string? imageid { get; set; }
        public string? villageid { get; set; }
        public string? imagename { get; set; }
        public string? villageName { get; set; }
    }
    public class VideoList
    {
        public string? videoid { get; set; }
        public string? villageid { get; set; }
        public string? videoname { get; set; }
        public string? villageName { get; set; }
        public string? Status { get; set; }
    }

}
