using Microsoft.AspNetCore.Http;
namespace CTMS.Model.Entities.Gallery
{
    public class GalleryMaster
    {
        public int? Id { get; set; }
        public string? GalleryName { get; set; }
        public string? GalleryNameOD { get; set; }
        public string? MediaType { get; set; }
        public IFormFile? ThumbnailImageFile { get; set; }
        public string? ThumbnailImg { get; set; }
        public string? Path { get; set; }
        public string? CreatedBy { get; set; }

    }
}



