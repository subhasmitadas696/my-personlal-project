using CTMS.Core;
using CTMS.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CTMS.Web.Controllers
{
    [Authorize(Roles ="1")]
    public class JobSchedulerController : Controller
    {
        private readonly JobSchedulerHelper _scheduler;
        public JobSchedulerController(JobSchedulerHelper scheduler) { _scheduler = scheduler; }
        public IActionResult CreateRecurringJob()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult StartJob(string days, string hour, string minutes)
        {
            try
            {
                string cronExpression =CommonHelper.GenerateCronExpression(minutes,hour,days);
               _= _scheduler.HandleEventStatus(cronExpression);
                return Content(new AjaxResult {state=ResultType.success.ToString(),message="Task running" }.ToJson());
            }
            catch(Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }

    }
}
