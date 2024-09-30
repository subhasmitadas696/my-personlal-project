using CTMS.Core;
using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace CTMS.Web.Filters
{
    public class CustomAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = ((AspNetCoreDashboardContext)context).HttpContext;
            var response=httpContext.Response;
            int intRoleId = Convert.ToInt32(httpContext.User.FindFirst("RoleId")?.Value);
            //if (intRoleId == (int)CommonHelper.EnRoles.SuperAdmin)
            if(intRoleId==0)
            {
                return true;
            }
            response.Redirect("/Authentication/Forbidden");
            return false;
        }
    }

}
