using CTMS.Model.DTOs;
using CTMS.Web.Areas.AdminConsole.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class AdminConsoleLogin : Controller
    {
        public IActionResult Index()
        {
            GenerateRandom();
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginDto objlogin)
        {
            objlogin.USERNAME = Util.DecryptStringAES(objlogin.USERNAME);
            if (objlogin.USERNAME == "supadmin" && objlogin.UPASSWORD == "password" && /*objlogin.hdnCaptcha*/objlogin.captcha == objlogin.captcha)
            {
                return RedirectToAction("AddHirerarchy", "HirerachyMaster", new { area = "AdminConsole" });
            }
            else
            {
                return View("Index");
            }
        }
        public string GenerateRandom()
        {
            Random random = new Random();
            string combination = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder CSRFRandNum = new StringBuilder();
            for (int i = 0; i < 4; i++)
                CSRFRandNum.Append(combination[random.Next(combination.Length)]);
            HttpContext.Session.SetString("UNIQUERANDOM", CSRFRandNum.ToString());
            return CSRFRandNum.ToString();
        }
    }
}
