using Microsoft.AspNetCore.Http;
using System;
namespace NEWCMS.Repository.ManageBanner
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
}

