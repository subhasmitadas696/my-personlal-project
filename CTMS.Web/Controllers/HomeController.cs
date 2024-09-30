using CTMS.Core;
using CTMS.Model.Entities;
using CTMS.Model.Entities.ManageFAQ;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.MediaMaster;
using CTMS.Model.Entities.NewsAndUpdate;
using CTMS.Model.Entities.Notification;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repositories.Repository;
using CTMS.Web.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NEWCMS.Repository.Repositories.Repository;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace CTMS.Web.Controllers;

public class HomeController : Controller
{
    private readonly INotificationRepository _NotificationRepository;
    private readonly IBannerRepository _BannerRepository;
    public readonly IMediaRepository _MediaRepository;
    public readonly IFaqQRepository _FaqQRepository;
    private readonly IMemCache _memCache;
    private readonly IRegistraionRepository _RegistraionRepository;
    private readonly IPaymentStructureRepository _PaymentStructureRepository;
    private readonly INewsAndUpdateRepository _NewsAndUpdateRepository;
    public HomeController(INewsAndUpdateRepository NewsAndUpdateRepository, IRegistraionRepository RegistraionRepository,INotificationRepository NotificationRepository, IBannerRepository BannerRepository, IMediaRepository MediaRepository, IFaqQRepository FaqQRepository, IMemCache memCache, IPaymentStructureRepository paymentStructureRepository)
    {
        _BannerRepository = BannerRepository;
        _NotificationRepository = NotificationRepository;
        _MediaRepository = MediaRepository;
        _FaqQRepository = FaqQRepository;
        _memCache = memCache;
        _RegistraionRepository = RegistraionRepository;
        _PaymentStructureRepository = paymentStructureRepository;
        _NewsAndUpdateRepository = NewsAndUpdateRepository;
    }
    public async Task<IActionResult> Index()
    {
        try
        {
         ViewBag.CountDetails = await _BannerRepository.DashBoardCount();
         return View();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public IActionResult AboutUs()
    {
        return View();
    }
    public IActionResult Faq()
    {
        return View();
    }
    public IActionResult NotificationDetails(int id=0)
    {
        try
        {
            NotificationMaster Tbl = new NotificationMaster();
            Tbl.Id = id;
            IList<NotificationMaster> objDistlist = _NotificationRepository.ViewNotification(Tbl).Result;
            ViewBag.Data = objDistlist;
            return View();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    public IActionResult ContactUs()
    {
        return View();
    }
    public IActionResult Gallery(int Id=1)
    {
        try
        {
            MediaMaster MM = new MediaMaster();
            MM.MediaType = Id;
            IList<MediaMaster> objDistlist = _MediaRepository.GalleryBind(MM).Result;
            ViewBag.Gallery = objDistlist;
            return View();
        }
        catch(Exception ex)
        {
            throw ex;
        }
        
    }
    public IActionResult PhotoGallery(int Id=0,int MediaType=0,string GalleryName="")
    {
        try 
        {
            ViewBag.Id = Id;
            ViewBag.MediaType = MediaType;
            ViewBag.GalleryName = GalleryName;
            return View();
        }
        catch(Exception ex)
        {
            throw ex;
        }   
    }
    public IActionResult VideoGallery(int Id = 0, int MediaType = 0, string GalleryName = "")
    {
        try
        {
            ViewBag.Id = Id;
            ViewBag.MediaType = MediaType;
            ViewBag.GalleryName = GalleryName;
            return View();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public IActionResult PrivacyPolicy()
    {
        return View();
    }
    public async Task<IActionResult> ViewStatus(string? acknowledgmentNumber)
    {
        try
        {
            var res = await _RegistraionRepository.GetAllDetailsByAckNo(acknowledgmentNumber);
            if(res.Count >0)
            {
                return View(res);
            }
            else
            {
                ViewBag.NoDataFound = true;
                return View("index");
            }
            
        }
        catch(Exception ex)
        {
            throw ex;
        }
        
    }
    public IActionResult PageNotFound()
    {
        return View();
    }
    public IActionResult SessionOut()
    {
        return View();
    }
    public IActionResult CopyrightPolicy()
    {
        return View();
    }
    public IActionResult Acknowledgement(string AcknowledgementNo)
    {
        ViewBag.AcknowledgementNo = AcknowledgementNo;
        return View();
    }
    public IActionResult HyperlinkPolicy()
    {
        return View();
    }
    public IActionResult TermsCondition()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult ScreenReader()
    {
        return View();
    }
    public IActionResult SupportedDocuments()
    {
        return View();
    }
    public async Task<IActionResult> NewsAndUpdate(NewsAndUpdate TBL)
    {
        List<NewsAndUpdate> lst =await  _NewsAndUpdateRepository.ViewNewsAndUpdate(TBL);
        ViewBag.NewsAndUpdate = lst;
        return View();
    } 
    public IActionResult Disclaimer()
    {
        return View();
    }
    public IActionResult NewsAndUpdateDetails()
    {
        return View();
    }
    public IActionResult GetNotification()
    {
        try
        {
            var cacheKey = "Notification";
            List<NotificationMaster> objCacheDistlist = _memCache.GetCache<List<NotificationMaster>>(cacheKey);
            if (objCacheDistlist != null)
            {
                return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
            }
            else
            {
                IList<NotificationMaster> objDistlist = _NotificationRepository.ViewAnnoncement().Result;
                if (objDistlist != null)
                {
                    _memCache.SetCache(objDistlist, cacheKey);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                }
            }
        }
        catch (Exception ex)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
        }
    }
    public IActionResult GetBanner()
    {
        try
        {
            var cacheKey = "Banner";
            List<ManageBanner> objCacheDistlist = _memCache.GetCache<List<ManageBanner>>(cacheKey);
            if (objCacheDistlist != null)
            {
                return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
            }
            else
            {
                IList<ManageBanner> objDistlist = _BannerRepository.ContentBanner().Result;
                if (objDistlist != null)
                {
                    _memCache.SetCache(objDistlist, cacheKey);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                }
            }
        }
        catch (Exception ex)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
        }
    }
    public IActionResult GetMediaBind(MediaMaster MM)
    {
        try
        {
            IList<MediaMaster> objDistlist = _MediaRepository.MediaBind(MM).Result;
            ViewBag.Gallery = objDistlist;
            if (objDistlist != null)
            {
                    
                return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
            }
            else
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
            }
            
        }
        catch (Exception ex)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
        }
    }
    public IActionResult FaqBind()
    {
        try
        {
            var cacheKey = "FAQ";
            List<Managefaq> objCacheDistlist = _memCache.GetCache<List<Managefaq>>(cacheKey);
            if (objCacheDistlist != null)
            {
                return Content(new AjaxResult { state = ResultType.success.ToString(), data = objCacheDistlist }.ToJson());
            }
            else
            {
                IList<Managefaq> objDistlist = _FaqQRepository.BindFAQ().Result;
                if (objDistlist != null)
                {
                    _memCache.SetCache(objDistlist, cacheKey);
                    return Content(new AjaxResult { state = ResultType.success.ToString(), data = objDistlist }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.error.ToString(), message = "Error occurred while showing" }.ToJson());
                }
            }
        }
        catch (Exception ex)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    #region biligual
    public IActionResult CultureManagement(string culture, string returnUrl)
    {
        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) });

        return LocalRedirect(returnUrl);
    }
    #endregion


    public async Task<IActionResult> RateChart()
    {
        return View();
    }
    public async Task<IActionResult> RateChartDt()
    {
        try
        {

            var data = _PaymentStructureRepository.ViewPaymentStructure().Result;

            return Json(data);

        }
        catch (Exception ex)
        {
            return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
        }
    }
}