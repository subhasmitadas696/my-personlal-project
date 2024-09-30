using CTMS.Core;
using CTMS.Model.Entities;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CTMS.Web.Areas.CMS.Controllers
{
#pragma warning disable
    [Area("CMS")]
    [Authorize]
    public class BannerController : Controller
    {

        private readonly IBannerRepository _BannerRepository;
        private readonly IMemCache _memCache;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public BannerController(IConfiguration configuration, IBannerRepository BannerRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IMemCache memCache)
        {
            _BannerRepository = BannerRepository;
            _hostingEnvironment = hostingEnvironment;
            _memCache = memCache;
        }
        [HttpGet]
        public IActionResult ManageBanner()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ManageBanner(ManageBanner TBL)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                              .SelectMany(v => v.Errors)
                              .Select(e => e.ErrorMessage));
                return Json(new { sucess = false, responseMessage = message, responseText = "Model State is invalid", data = "" });
            }
            else
            {
                string foldername = "CMS";
                var SubFolder = "BannerImg";
                string webrootpath = _hostingEnvironment.WebRootPath;
                string MainDocPath = Path.Combine(webrootpath, foldername);
                string subDocPath = Path.Combine(MainDocPath, SubFolder);
                string BannerPath = Path.Combine(foldername, SubFolder);
                if (!Directory.Exists(MainDocPath))
                    Directory.CreateDirectory(MainDocPath);
                if (!Directory.Exists(subDocPath))
                    Directory.CreateDirectory(subDocPath);
                
                if (TBL.Id == 0 || TBL.Id == null)
                {
                    if (TBL.BannerImageFile != null)
                    {
                        var extn = "." + TBL.BannerImageFile.FileName.Split('.')[TBL.BannerImageFile.FileName.Split('.').Length - 1];
                        TBL.BannerImage = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extn;
                        TBL.BannerPath = Path.Combine(BannerPath, TBL.BannerImage);
                        using (var stream = new FileStream(Path.Combine(subDocPath, TBL.BannerImage), FileMode.Create))
                        {
                            await TBL.BannerImageFile.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        TBL.BannerImage = TBL.BannerImage;
                    }
                }
                else
                {
                    if (TBL.BannerImageFile != null)
                    {
                        string filePath = Path.Combine(subDocPath, TBL.BannerImage!);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await TBL.BannerImageFile.CopyToAsync(stream);
                            }

                        }
                        else
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await TBL.BannerImageFile.CopyToAsync(stream);
                            }
                        }
                    }
                    else
                    {
                        TBL!.BannerImage = TBL.BannerImage;
                    }
                }
                if (TBL.Id == 0)
                {
                    var data = _BannerRepository.I_ManageBanner(TBL);
                    _memCache.RemoveCache("Banner");
                    return Json(new { sucess = true, responseMessage = "Banner Saved Successfully.", responseText = "Success", data = data });

                }
                else
                {
                    var data = _BannerRepository.U_ManageBanner(TBL);
                    _memCache.RemoveCache("Banner");
                    return Json(new { sucess = true, responseMessage = "Banner Updated Successfully.", responseText = "Success", data = data });

                }
            }
        }
        [HttpGet]
        public IActionResult ViewManageBanner()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> Get_ManageBanner(ManageBanner TBL)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                return Json(new { sucess = false, responseMessage = message, responseText = "Model State is invalid", data = "" });
            }
            else
            {
                List<ManageBanner> lst = await _BannerRepository.R_ManageBanner(TBL);
                var jsonres = JsonConvert.SerializeObject(lst);

                return Json(jsonres);

            }

        }

        [HttpDelete]

        public async Task<JsonResult> D_ManageBanner(int Id)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return Json(new { sucess = false, responseMessage = message, responseText = "Model State is invalid", data = "" });
            }
            else
            {
                ManageBanner ob = new ManageBanner();
                ob.Id = Id;

                var data = await _BannerRepository.D_ManageBanner(ob);
                return Json(new { sucess = true, responseMessage = "Action taken Successfully.", responseText = "Success", data = data });
            }
        }
        [HttpGet]

        public async Task<JsonResult> RO_ManageBanner(int Id)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return Json(new { sucess = false, responseMessage = message, responseText = "Model State is invalid", data = "" });
            }
            else
            {

                ManageBanner ob = new ManageBanner();
                ob.Id = Id;
                List<ManageBanner> lst = await _BannerRepository.RO_ManageBanner(ob);
                var jsonres = JsonConvert.SerializeObject(lst?.FirstOrDefault());
                return Json(jsonres);
            }

        }
        public IActionResult DownloadFile(string fileName)
        {
            string fileDirectory = _hostingEnvironment.WebRootPath;
            string filePath = Path.Combine(fileDirectory, fileName);
            if (System.IO.File.Exists(filePath))
            {
                string contentType = GetContentType(fileName);
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, contentType, fileName);
            }
            else
            {
                return NotFound();
            }
        }
        public string GetContentType(string fileName)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string[] pdfExtensions = { ".pdf" };
            string[] videoExtensions = { ".mp4", ".avi", ".mkv" };

            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (imageExtensions.Contains(extension))
            {
                return $"image/{extension.Substring(1)}";
            }
            else if (pdfExtensions.Contains(extension))
            {
                return "application/pdf";
            }
            else if (videoExtensions.Contains(extension))
            {
                return $"video/{extension.Substring(1)}";
            }
            else
            {
                return "application/octet-stream";
            }
        }

    }
}
