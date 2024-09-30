using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using CTMS.Web.Areas.AdminConsole.Interface;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class HomeController : Controller
    {
        
        public IConfiguration Configuration { get; }
        public HomeController(IUserServiceProvider userService, IConfiguration configurations)
        {
            Configuration = configurations;
        }
        public IActionResult Index()
        {
            TempData.Keep("UserName");
            TempData.Keep("Image");

            return View();
        }
       
    }
}
