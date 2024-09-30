using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTMS.Model.Entities.NewsAndUpdate
{
    public class NewsAndUpdate
    {

       public int? Id { get; set; }
       public string? NewsTitle { get; set; }
       public string? NewsTitleOD { get; set; }
       public string? NewsDescription { get; set; }
       public string? NewsDescriptionOD { get; set; }
       public string? NewsSource { get; set; }
       public string? NewsSourceOD { get; set; }
       public string? NewsPublishDate { get; set; }
       public string? NewsPhoto { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? NewsPhotoPath { get; set; }
       public int? CreatedBy { get; set; }
    }
}
