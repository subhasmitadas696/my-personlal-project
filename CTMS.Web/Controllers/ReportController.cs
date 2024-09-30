using CTMS.Core;
using CTMS.Model.BlockMaster;
using CTMS.Model.DistMaster;
using CTMS.Model.Entities.Registration;
using CTMS.Model.Entities.ReportMaster;
using CTMS.Model.GpMaster;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using CTMS.Model.Entities.ImageList;
using CTMS.Model.DTOs;
using AngleSharp.Text;
using Microsoft.AspNetCore.Authorization;
using CTMS.Model.Entities.Approval;
using CTMS.Repository.Repositories.Repository;

namespace CTMS.Web
{
    [Authorize]
    public class ReportController : Controller
    {

        public IConfiguration Configuration;
        private readonly IRegistraionRepository _RegistraionRepository;
        private readonly IMasterRepository _masterService;
        private readonly IReportRepository _ReportRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ReportController(IConfiguration configuration, IMasterRepository masterService, IRegistraionRepository RegistraionRepository, IReportRepository ReportRepository, IWebHostEnvironment hostingEnvironment)
        {
            _ReportRepository = ReportRepository;
            _masterService = masterService;
            Configuration = configuration;
            _RegistraionRepository = RegistraionRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult TroupeReportingApprovalList(string? fromDate, string? toDate)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    TroupeRegistrationView TRV = new TroupeRegistrationView();
                    var BlockId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
                    if (BlockId != 0)
                    {
                        TRV.BlockId = BlockId;
                        TRV.Status = 6;
                        TRV.FromDate = fromDate;
                        TRV.ToDate = toDate;
                        ViewBag.FromDate = fromDate; ViewBag.ToDate = toDate;
                        ViewBag.TroupeApprovalList = _RegistraionRepository.TroupeReportingApprovalSuccess(TRV).Result.ToList();
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public IActionResult TroupeReporedApprovalList(string? fromDate,string? toDate)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    TroupeRegistrationView TRV = new TroupeRegistrationView();
                    var BlockId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
                    if (BlockId != 0)
                    {
                        TRV.BlockId = BlockId;
                        TRV.Status = 5;
                        TRV.FromDate=fromDate;
                        TRV.ToDate = toDate;
                        ViewBag.FromDate=fromDate; ViewBag.ToDate=toDate;
                        ViewBag.TroupeApprovalList = _RegistraionRepository.TroupeReportingApprovalListForBlock(TRV).Result.ToList();
                        
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }

        //For Notification
        [HttpPost]
        public IActionResult Notification()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    TroupeRegistrationView TRV = new TroupeRegistrationView();
                    var BlockId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
                    if (BlockId != 0)
                    {
                        TRV.BlockId = BlockId;
                        TRV.Status = 5;
                        IList<TroupeReportingView> objlist = _RegistraionRepository.TroupeReportingApprovalListForBlock(TRV).Result.ToList();
                        if (objlist.Count != 0)
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), data = objlist }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.warning.ToString(), message = "Error occurred while showing", data = null }.ToJson());
                        }
                    }
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
        }
        public async Task<IActionResult> TroupeReportingApproval(int TroupeId, int AssignEventId, int STATUS)
        {
            try
            {

            if (User?.Identity?.IsAuthenticated == true)
            {
                if (User.FindFirst("UID")?.Value != null)
                {
                    List<ImageList> ImageList = new List<ImageList>();
                    List<VideoList> VideoList = new List<VideoList>();
                    List<ImageList> ReviewerImageList = new List<ImageList>();
                    List<VideoList> ReviewerVideoList = new List<VideoList>();
                    TroupeReportingView RV = new TroupeReportingView();
                    RV.AssignEventId = AssignEventId;
                    RV.TroupeId = TroupeId;
                    RV.Status = STATUS;
                    ViewBag.TroupeDetails = _RegistraionRepository.TroupeReportingForApprovalDetails(RV).Result.ToList();
                    if (ViewBag.TroupeDetails.Count > 0)
                    {
                        if(ViewBag.TroupeDetails[0].UploadPhoto != null && ViewBag.TroupeDetails[0].UploadPhoto != "")
                        {
                            string[] UploadphotoArray = ViewBag.TroupeDetails[0].UploadPhoto.ToString().Split('~');
                            ViewBag.count = UploadphotoArray.Length;
                            List<string> Uploadphoto = UploadphotoArray.ToList();
                            var groupedData = Uploadphoto
                            .Select(entry =>
                            {
                                var parts = entry.Split('|');
                                return new
                                {
                                    VillageId = parts[0],
                                    ImageId = parts[1]
                                };
                                
                            })
                            .GroupBy(entry => entry.VillageId)
                            .Select(group =>
                            {
                                var villageId = group.Key;
                                var imageIds = string.Join("|", group.Select(entry => entry.ImageId ));
                                return villageId + "|" + imageIds;
                            })
                            .ToList();
                            List<string> modifiedArray = new List<string>();
                            foreach (var entry in groupedData)
                            {
                                string modifiedEntry = entry.ReplaceFirst("|", "~");
                                modifiedArray.Add(modifiedEntry);
                            }
                            foreach (var item in modifiedArray)
                            {
                                var imageid = item.Split('~')[0];
                                var villageid = imageid.Split('_')[0];
                                var villageName = _masterService.GetVillageNameById(villageid);
                                var imagename = item.Split('~')[1];
                                ImageList.Add(new ImageList { imageid = imageid, imagename = imagename, villageid = villageid, villageName = villageName.ToString() });
                            }
                        }
                        if(ViewBag.TroupeDetails[0].UploadVideo!=null)
                        {
                            var UploadVideo = ViewBag.TroupeDetails[0].UploadVideo.ToString().Split('~');
                            foreach (var item in UploadVideo)
                            {
                                var videoid = item.Split('|')[0];
                                var villageid = videoid.Split('_')[0];
                                var villageName = _masterService.GetVillageNameById(villageid);
                                var videoname = item.Split('|')[1];
                                VideoList.Add(new VideoList { videoid = videoid, videoname = videoname, villageid = villageid, villageName = villageName.ToString() });
                            }
                        }
                        
                    }
                    List<ReviewerPhotolist> ReviewerPhotolist = await _RegistraionRepository.ReviewersPhotoList(AssignEventId, TroupeId);
                    if (ReviewerPhotolist != null && ReviewerPhotolist.Count>0)
                    {
                        string[] reviewersPhotoList = ReviewerPhotolist[0].ReviewersPhotolist!.Split("~");
                        var reviewersVideoList = ReviewerPhotolist[0].ReviewersVideolist!.Split("~");
                        List<string> reviewersUploadphoto = reviewersPhotoList.ToList();

                        var groupedData = reviewersUploadphoto
                        .Select(entry =>
                        {
                            var parts = entry.Split('_');
                            return new
                            {
                                VillageId = parts[0],
                                ImageId = parts[1]
                                
                            };
                        })
                        .GroupBy(entry => entry.VillageId)
                        .Select(group =>
                        {
                            var villageId = group.Key;
                            var imageIds = string.Join("|", group.Select(entry => entry.ImageId ));
                            return villageId + "_" + imageIds;
                        })
                        .ToList();
                        List<string> modifiedArray = new List<string>();
                        foreach (var entry in groupedData)
                        {
                            string modifiedEntry = entry.Replace("|" + TroupeId, "");
                            modifiedEntry = modifiedEntry.ReplaceFirst("|", "~");
                            modifiedArray.Add(modifiedEntry);
                        }
                        foreach (var item in modifiedArray)
                        {
                            var imageid = item.Split('~')[0];
                            var villageid = imageid.Split('_')[0];
                            var villageName = _masterService.GetVillageNameById(villageid);
                            var imagename = item.Split('~')[1];
                            ReviewerImageList.Add(new ImageList { imageid = imageid, imagename = imagename, villageid = villageid, villageName = villageName.ToString() });
                        }
                        foreach (var item in reviewersVideoList)
                        {
                            var videoid = item.Split('|')[0];
                            var villageid = videoid.Split('_')[0];
                            var villageName = _masterService.GetVillageNameById(villageid);
                            var videoname = item.Split('|')[1];
                            ReviewerVideoList.Add(new VideoList { videoid = videoid, videoname = videoname, villageid = villageid, villageName = villageName.ToString() });
                        }

                    }
                    ViewBag.ImageList = ImageList;
                    ViewBag.VideoList = VideoList;
                    ViewBag.ReviewerImageList = ReviewerImageList;
                    ViewBag.ReviewerVideoList = ReviewerVideoList;
                    return View();
                }
                else
                {
                    return View("UnauthorizedAccess");
                }
            }
            else
            {
                return RedirectToAction("SessionOut", "Home");
            }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> TroupeReportingApproval(TroupeReporting TR)
        {
            try { 
                if (User?.Identity?.IsAuthenticated == true)
                {
                    if (User.FindFirst("UID")?.Value != null)
                    {
                        TR.CreatedBy = User.FindFirst("UID")?.Value!;
                        string retval = await _RegistraionRepository.ReportingApproval(TR);
                        if (retval == "4")
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Report Approved Successfully", data = retval }.ToJson());
                        }
                        else if (retval == "5")
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Report Rejected Successfully", data = retval }.ToJson());
                        }
                        else if (retval == "6")
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Report Reverted Successfully", data = retval }.ToJson());
                        }
                        else
                        {
                            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "Error In Approval", data = retval }.ToJson());
                        }
                    }
                    else
                    {
                        return View("UnauthorizedAccess");
                    }
                }
                else
                {
                    return RedirectToAction("SessionOut", "Home");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult AllErrorFiles()
        {
            string[] filePathsNLog = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, "logs/"));
            string[] filePathsCH = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, "Log/ErrorLog/"));
            string[] AllLogFiles = filePathsNLog.Concat(filePathsCH).ToArray();
            List<FileModel> files = new List<FileModel>();
            foreach (string filePath in AllLogFiles)
            {
                files.Add(new FileModel { FileName = Path.GetFileName(filePath), CreatedOn = System.IO.File.GetCreationTime(filePath) });
            }
            return View(files);
        }

        public FileResult DownloadFile(string fileName)
        {
            string path = string.Empty;
            string path2 = string.Empty;
            byte[]? bytes;
            path = Path.Combine(_hostingEnvironment.WebRootPath, "logs/") + fileName;
            path2 = Path.Combine(_hostingEnvironment.WebRootPath, "Log/ErrorLog/") + fileName;
            var existsInPath1 = System.IO.File.Exists(path);
            var existsInPath2 = System.IO.File.Exists(path2);
            if (existsInPath1)
            {
                bytes = System.IO.File.ReadAllBytes(path);
            }
            else if (existsInPath2)
            {
                bytes = System.IO.File.ReadAllBytes(path2);
            }
            else
            {
                bytes = null;
            }
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
