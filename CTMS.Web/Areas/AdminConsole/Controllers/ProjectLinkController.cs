using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole.Models.ProjectMaster;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class ProjectLinkController : Controller
    {



        private readonly IProjectLinkServiceProvider _projectLinkService;
        Project model = new Project();

        public ProjectLinkController(IProjectLinkServiceProvider projectLinkService)
        {
            _projectLinkService = projectLinkService;
        }
        [HttpGet]
        public async Task<IActionResult> AddProjectLink(string Status)
        {
            TempData["CommandStatus"] = Status;
            ViewBag.CREATEDBY = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            return View();

        }

        [HttpPost]
        public IActionResult AddProjectLink(Project objproject)
        {
            try
            {
                var Result = _projectLinkService.AddProjectLink(objproject);
                return Ok(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public IActionResult UpdateProjectLink(Project objproject)
        {
            try
            {
                var Result = _projectLinkService.UpdateProjectLink(objproject);
                return Ok(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region view  ProjectLink
        [HttpGet]
        public async Task<IActionResult> ViewActiveProjectLink(string Status)
        {
            try
            {
                ViewProjectLink vmodel = new ViewProjectLink();
                vmodel.ViewProjectLinkDetailsmodel = (await _projectLinkService.GetAllActiveProjectLink()).ViewProjectLinkDetailsmodel.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ViewInActiveProjectLink(string Status)
        {
            try
            {
                ViewProjectLink vmodel = new ViewProjectLink();
                vmodel.ViewProjectLinkDetailsmodel = (await _projectLinkService.GetAllInActiveProjectLink()).ViewProjectLinkDetailsmodel.ToList();
                TempData["CommandStatus"] = Status;
                return View(vmodel);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region Mark As InActive/Active Project Link
        [HttpPost]
        public IActionResult MarkAsInActive(string[] chkbox, int updatedby)
        {
            try
            {
                string primarylinkId = string.Join(",", chkbox);
                var Result = String.Empty;
                foreach (string Id in chkbox)
                {
                    Result = _projectLinkService.InactiveProjectLink(Convert.ToInt32(Id), Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public IActionResult MarkActive(string[] chkbox, int updatedby)
        {
            try
            {
                string primarylinkId = string.Join(",", chkbox);
                var Result = String.Empty;
                foreach (string Id in chkbox)
                {
                    Result = _projectLinkService.ActiveProjectLink(Convert.ToInt32(Id), Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
        public async Task<IActionResult> EditActiveProjectLink(int id)
        {

            ViewProjectLink model = new ViewProjectLink();
            try
            {
                model.ProjectLinkModelIdwise = (await _projectLinkService.GetById(id)).ProjectLinkModelIdwise.ToList();

                Project m = new Project();
                m.INTPROJECTLINKID = model.ProjectLinkModelIdwise[0].INTPROJECTLINKID; ;
                m.NVCHPROJECTLINKNAME = model.ProjectLinkModelIdwise[0].NVCHPROJECTLINKNAME;
                m.NVCHPROJECTLINKDESC = model.ProjectLinkModelIdwise[0].NVCHPROJECTLINKDESC;
                m.INTUPDATEDBY = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                return View(m);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
