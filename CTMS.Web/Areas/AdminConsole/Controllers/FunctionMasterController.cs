using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole;
using Microsoft.AspNetCore.Mvc;
using CTMS.Web.Areas.AdminConsole.Models.FunctionMaster;
using X.PagedList;
using CTMS.Core;

namespace CTMS.Web.Areas.AdminConsole.Controllers
{
    [Area("AdminConsole")]
    public class FunctionMasterController : Controller
    {     // GET: /FunctionMaster/
        int intOut;
        FunctionMasterModel ObjFun = new FunctionMasterModel();
        private const int defaultPageSize = 1000;
        private readonly IFuncServiceProvider _functionService;

        public FunctionMasterController(IFuncServiceProvider functionService)
        {

            _functionService = functionService;
        }

        public ActionResult AddFunctionMaster()
        {
            return View();
        }
        #region Add Function Master
        [HttpPost]
        public ActionResult AddFunctionMaster(FunctionMasterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.ActionCode = "A";
                    model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserId"));//Convert.ToInt16(Session["UserId"]);
                    intOut = _functionService.AddFunction(model);
                    ViewData["result"] = intOut;
                    ModelState.Clear();
                    return Json(intOut.ToString());
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }
        #endregion
        #region View Active Function Master

        public ActionResult ViewFunctionMasterActive(IFormCollection Coll, int? pages, string Status)
        {
            ViewData["result"] = Status;
            FunctionMasterModel model = new FunctionMasterModel();
            model.FunctionName = Coll["fn"];
            string Funname = model.FunctionName;
            var pageNumber = pages ?? 1;
            model.ActionCode = "V";
            model.Flag = 0;
            if (model.FunctionName == "" || model.FunctionName == null)
            {
                model.FunctionName = string.Empty;
            }
            else
            {
                model.ActionCode = "V";
            }
            var Strmodel = _functionService.GetAllFunction(model).ToPagedList(pageNumber, defaultPageSize);
            ViewData["Funname"] = Funname;
            ViewBag.Strmodel = Strmodel;
            return View(Strmodel);

        }
        #endregion
        #region View Inactive Function Master
        //[AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewFunctionMasterInActive(IFormCollection Coll, int? page)
        {
            FunctionMasterModel model = new FunctionMasterModel();
            model.ActionCode = "V";
            model.FunctionName = Coll["fn"];
            string Funname = model.FunctionName;
            model.Flag = 1;
            var posts = _functionService.GetAllFunction(model);
            var pageNumber = page ?? 1;
            if (model.FunctionName == "" || model.FunctionName == null)
            {
                model.FunctionName = "";
            }

            var Strmodel = _functionService.GetAllFunction(model).ToPagedList(pageNumber, defaultPageSize); ;//.ToPagedList(pageNumber, defaultPageSize);
            ViewData["Funname"] = Funname;
            ViewBag.Strmodel = Strmodel;
            return View(Strmodel);


        }
        #endregion
        #region InActive Function Master
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult InActiveFunctionMaster( string Resp)
        {
            try
            {
                string[] checkResp = Resp.Split(',');
                foreach (string Id in checkResp)
                {
                    FunctionMasterModel model = new FunctionMasterModel();
                    model.ActionCode = "I";
                    model.FunctionId = Id;
                    model.CreatedBy = 1; // Convert.ToInt16(Session["UserId"]);
                    intOut = _functionService.ActiveFunction(model);
                    ViewData["result"] = intOut;
                }
                return Content(new AjaxResult { status = ResultType.success.ToString(), message = "success", data = intOut }.ToJson());

            }
            catch
            {
                return View();
            }
        }

        #endregion
        #region Active Function Master
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult ActiveFunctionMaster( string Resp)
        {
            try
            {
                string[] checkResp = Resp.Split(',');
                foreach (string Id in checkResp)
                {
                    FunctionMasterModel model = new FunctionMasterModel();
                    model.ActionCode = "T";
                    model.FunctionId = Id;
                    model.CreatedBy = 1;
                    intOut = _functionService.ActiveFunction(model);
                    ViewData["result"] = intOut;
                }
                return Content(new AjaxResult { status = ResultType.success.ToString(), message = "success", data = intOut }.ToJson());
            }
            catch (Exception)
            {
                return View();
            }
        }

        #endregion

        public ActionResult EditActiveFunction(string id)
        {
            try
            {
                id = (CommonFunction.Decrypt(id));
                ViewBag.id = id;
                FunctionMasterModel model = new FunctionMasterModel();
                model.ActionCode = "E";
                model.FunctionId = id;
                model.Flag = 1;
                IList<FunctionMasterModel> objFunList = new List<FunctionMasterModel>();
                objFunList = _functionService.GetAllFunction(model);
                foreach (var i in objFunList)
                {
                    ObjFun.FunctionName = i.FunctionName;
                    ObjFun.FileName = i.FileName;
                    ViewData["Description"] = i.Description;
                    ViewData["DescriptionLen"] = 500 - Convert.ToInt16(i.Description.Length);
                    ViewData["FAdd"] = i.FAdd;
                    ViewData["FView"] = i.FView;
                    ViewData["FManage"] = i.FManage;
                    ObjFun.MailR = i.MailR;
                    ObjFun.FreeBees = i.FreeBees;
                    ObjFun.PortletFile = i.PortletFile;
                }
                return View(ObjFun);

            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditActiveFunction(FunctionMasterModel model)
        {
            try
            {
                if (model.FunctionId != null)
                {
                    model.ActionCode = "U";
                    model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserId"));// Convert.ToInt16(Session["UserId"]);
                    intOut = _functionService.EditFunction(model);
                    return Json(intOut.ToString());
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
