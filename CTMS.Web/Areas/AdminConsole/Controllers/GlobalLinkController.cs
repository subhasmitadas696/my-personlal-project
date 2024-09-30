using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole.Models.GlobalLink;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class GlobalLinkController : Controller
    {

        Global objGloba = new Global();

        public static string StrOL;
        private const int defaultPageSize = 10;
        private readonly IGblLinkServiceProvider _gblLinkService;
        private readonly IProjectLinkServiceProvider _projectLinkService;
        GlobalModel model = new GlobalModel();

        public GlobalLinkController(IGblLinkServiceProvider gblLinkService, IProjectLinkServiceProvider projectLinkService)
        {
            _gblLinkService = gblLinkService;
            _projectLinkService = projectLinkService;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddGlobalLink()
        {
            var s = HttpContext.Session.GetString("UserName");
                ViewGlobal g = new ViewGlobal();
                model.GlobalModelList = (await _gblLinkService.GetMaxId()).ViewGlobalLinkDetailsmodel.ToList();
                g.GMAxid = model.GlobalModelList[0].GMAxid; //_gblLinkService.GetGlobalLinkMaxId();
                g.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                g.ProjectList = (await _projectLinkService.GetAllActiveProjectLink()).ViewProjectLinkDetailsmodel.ToList();
                return View(g);
            
        }
        #region Add Global Link
        [HttpPost]
        public IActionResult AddGlobalLink(ViewGlobal objglobal)
        {
            try
            {

                var Result = _gblLinkService.AddGlobalLink(objglobal);
                return Json(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> ViewGlobalLinkActive(string Status)
        {
            try
            {
                model.GlobalModelList = (await _gblLinkService.GetAllActiveGlobalLink()).ViewGlobalLinkDetailsmodel.ToList();
                if (Status == "1")
                {
                    TempData["CommandStatus_S"] = Status;

                }
                else if (Status == "2")
                {
                    TempData["CommandStatus_U"] = Status;

                }
                else if (Status == "4")
                {
                    TempData["CommandStatus_A"] = Status;

                }
                else if (Status == "5")
                {
                    TempData["CommandStatus_AC"] = Status;

                }
                else if (Status == "6")
                {
                    TempData["CommandStatus_INA"] = Status;

                }
                else if (Status == "7")
                {
                    TempData["CommandStatus_AU"] = Status;

                }
                    
                return View(model);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ViewGlobalLinkInActive()
        {
            try
            {
                model.GlobalModelList = (await _gblLinkService.GetAllInActiveGlobalLink()).ViewGlobalLinkDetailsmodel.ToList();
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IActionResult> EditActiveGlobalLink(int id)
        {
            {
                try
                {
                    model.GlobalModelList = (await _gblLinkService.GetById(id)).ViewGlobalLinkDetailsmodel.ToList();


                    ViewGlobal m = new ViewGlobal();
                    m.ProjectList = (await _projectLinkService.GetAllActiveProjectLink()).ViewProjectLinkDetailsmodel.ToList();
                    m.intGLinkID = model.GlobalModelList[0].intGLinkID; ;
                    m.nvchGLinkName = model.GlobalModelList[0].nvchGLinkName;
                    m.VCHICONCLASS = model.GlobalModelList[0].VCHICONCLASS;
                    m.intSortNum = model.GlobalModelList[0].intSortNum;
                    m.updatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                    m.INTPROJECTLINKID = model.GlobalModelList[0].INTPROJECTLINKID;
                    return View(m);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #region Update Global Link
        [HttpPost]
        public IActionResult EditGlobalLink(ViewGlobal objglobal)
        {
            try
            {

                var Result = _gblLinkService.EditGlobalLink(objglobal);

                objglobal.resultmsg = Result;
                return Json(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Delete Global Link

        public IActionResult DeleteGlobalLink(int id)
        {
            try
            {
                var Result = _gblLinkService.DeleteGlobalLink(id, Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                return RedirectToAction("ViewGlobalLinkActive", new { Status = "Record " + Result });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Mark As InActive Global Link
        [HttpPost]
        public IActionResult MarkAsInActive(string[] chkbox, int updatedby)
        {
            try
            {
                string primarylinkId = string.Join(",", chkbox);
                var Result = String.Empty;
                foreach (string Id in chkbox)
                {
                    Result = _gblLinkService.DeleteGlobalLink(Convert.ToInt32(Id), Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                }
                return Json(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public IActionResult MarkAsActive(string[] chkbox, int updatedby)
        {
            try
            {
                string primarylinkId = string.Join(",", chkbox);
                var Result = String.Empty;
                foreach (string Id in chkbox)
                {
                    Result = _gblLinkService.MarkActiveGlobalLink(Convert.ToInt32(Id), Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                }
                return Json(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]

        public IActionResult SlnoModifyData(GlobalModel objGlobalModel)
        {
            try
            {
                var Result = String.Empty;
                string[] Globalslnolist = objGlobalModel.slnomodifyList.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in Globalslnolist)
                {
                    string[] slno = item.Split('|');
                    Result = _gblLinkService.ModifySlnoGlobalLink(Convert.ToInt32(slno[0]), Convert.ToInt32(slno[1]), Convert.ToInt32(HttpContext.Session.GetString("UserId")));

                }

                return Json(Result);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

    }
}
