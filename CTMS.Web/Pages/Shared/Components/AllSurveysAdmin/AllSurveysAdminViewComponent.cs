
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CTMS.Web.Areas.AdminConsole;

namespace CTMS.Web.Pages.Shared.Components.AllSurveysAdmin
{
    public class AllSurveysAdminViewComponent : ViewComponent
    {
        private IAdminConsoleRepository _IAdminConsoleRepository { get; }
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;
        string ContentRootPath = "";
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public AllSurveysAdminViewComponent(IAdminConsoleRepository IAdminConsoleRepository, IConfiguration config, IHttpContextAccessor contextAccessor, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _contextAccessor = contextAccessor;
            _environment = environment;
            ContentRootPath = environment.ContentRootPath;
            _IAdminConsoleRepository = IAdminConsoleRepository;
            _config = config;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            
            var userid = UserClaimsPrincipal.FindFirst("Userid")?.Value;
            var desgid = UserClaimsPrincipal.FindFirst("RoleId")?.Value;
            var UID = UserClaimsPrincipal.FindFirst("Uid")?.Value;
            var PId = 1;
            var results = _IAdminConsoleRepository.GetLinkAccessByUserId(Convert.ToInt32(userid), PId, Convert.ToInt32(UID), Convert.ToInt32(desgid));
            var Data = results.ToList();
            bool checkSession = DoesSessionExist();


            if (HttpContext.Request.Query["Glink"].ToString() != "")
            {
                Data.ForEach(c => c.Aglinkid = Convert.ToInt32(HttpContext.Request.Query["Glink"]));
                TempData["Glink"] = HttpContext.Request.Query["Glink"].ToString();
                TempData.Keep();
            }
            if (HttpContext.Request.Query["Plink"].ToString() != "")
            {
                Data.ForEach(c => c.Aplinkid = Convert.ToInt32(HttpContext.Request.Query["Plink"]));
                TempData["Plink"] = HttpContext.Request.Query["Plink"].ToString();
                TempData.Keep();
            }
            return View("default", results);

        }

        public void StoreArrayInSession(string[] myArray)
        {
            HttpContext.Session.Set("PESC", System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(myArray));
        }
        public string[] RetrieveArrayFromSession()
        {
            byte[] byteArray = HttpContext.Session.Get("PESC");

            if (byteArray != null)
            {
                return System.Text.Json.JsonSerializer.Deserialize<string[]>(byteArray);
            }
            return null;
        }
        public bool DoesSessionExist()
        {
            return HttpContext.Session.TryGetValue("PESC", out _);
        }
    }
}


    

