using Microsoft.AspNetCore.Http;
namespace CTMS.Model.Entities
{
	public class ManageBanner
	{
		public string? Action { get; set; }
		public int Id { get; set; }
		public string? BannerTitle { get; set; }
		public string? BannerTitleOD { get; set; }
		public string? BannerDescription { get; set; }
		public string? BannerDescriptionOD { get; set; }
		public string? BannerImage { get; set; }
		public IFormFile? BannerImageFile { get; set; }
		public string? BannerPath { get; set; }
		public string? StartDate { get; set; }
		public string? EndDate { get; set; }
		public int CreatedBy { get; set; }

    }
	public class ManageDashboardCount
	{
        public string? TroupeRegistered { get; set; }
        public string? TotalEvents { get; set; }
        public string? NoOfArtists { get; set; }
        public string? TotalArtForms { get; set; }
    }
}

