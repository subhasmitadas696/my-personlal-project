using Microsoft.AspNetCore.Http;
namespace CTMS.Model.Entities.MediaMaster
{
    public class MediaMaster
    {
        public int? Id { get; set; }
        public int GalleryId { get; set; }
        public string? GalleryName { get; set; }
        public string? ImageName { get; set; }
        public string? MediaName { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? VideoName { get; set; }
        public IFormFile? VideoFile { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }
        public string? DescriptionOd { get; set; }
        public string? GalleryNameOD { get; set; }
        public string? Thumbnail { get; set; }
        public string? CreatedBy { get; set; }
        public int MediaType { get; set; }
        public int ImageCount { get; set; }
        public int VideoCount { get; set; }
    }
}
