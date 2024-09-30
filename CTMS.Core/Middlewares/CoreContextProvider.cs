using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
namespace CTMS.Core
{

    /// <summary>
    /// Core global support context
    /// Configuration, Service, HttpContext, User
    /// (It can be changed to the way of injection, in Here.
    /// MiddleWare middleware can intercept requests in the pipeline, 
    /// it can decide Yes. No. will transfer the request to the next middleware
    /// </summary>
    public class CoreContextProvider
    {
        private string LoginUserKey = ConstParameters.SysLoginUserKey;
        private string LoginProvider = ConstParameters.SysLoginProvider;

        private static IHttpContextAccessor _accessor;
        private readonly RequestDelegate _next;

        public static IConfiguration Configuration { get; set; }
        //private static IServiceCollection ServiceCollection { get; set; }
        public static IHostingEnvironment HostingEnvironment { get; set; }
        public static Microsoft.AspNetCore.Http.HttpContext HttpContext => _accessor.HttpContext;


        public CoreContextProvider(RequestDelegate next, IHttpContextAccessor accessor,
            IHostingEnvironment hostingEnvironment)
        {
            _next = next;
            _accessor = accessor;
            HostingEnvironment = hostingEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            //do somethings
            await _next(context);
        }



        /// <summary>
        /// Get MemCache context
        /// </summary>
        public static IMemCache MemCache
        {
            get
            {
                return GetService<IMemCache>();
            }
        }


        /// <summary>
        /// Get Current Login user
        /// </summary>
        public static OperatorModel CurrentSysUser
        {
            get
            {
                //HttpContext.User.Identities.Where(w => w.AuthenticationType == SysManageAuthAttribute.SysManageAuthScheme).FirstOrDefault();
                var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
                if (claimsIdentity == null || claimsIdentity.Claims.Count() == 0)
                {
                    //throw new Exception("User not Login");
                }
                var claims = claimsIdentity.Claims;
                return new OperatorModel()
                {
                    UserId = claims.Where(w => w.Type == ClaimTypes.Sid).Select(u => u.Value).FirstOrDefault(),
                    Account = claims.Where(w => w.Type == ClaimTypes.Name).Select(u => u.Value).FirstOrDefault(),
                    FullName = claims.Where(w => w.Type == ClaimTypes.GivenName).Select(u => u.Value).FirstOrDefault(),
                    OrganizeId = claims.Where(w => w.Type == ClaimTypes.PrimarySid).Select(u => u.Value).FirstOrDefault(),
                    DepartmentId = claims.Where(w => w.Type == ClaimTypes.PrimaryGroupSid).Select(u => u.Value).FirstOrDefault(),
                    RoleId = claims.Where(w => w.Type == ClaimTypes.Role).Select(u => u.Value).FirstOrDefault(),
                    LoginIPAddress = claims.Where(w => w.Type == ClaimTypes.Dns).Select(u => u.Value).FirstOrDefault(),
                    IsSystem = claims.Where(w => w.Type == ClaimTypes.IsPersistent).Select(u => u.Value).FirstOrDefault().ToBool()
                };
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILogger GetLogger(string name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return GetService<ILoggerFactory>().CreateLogger(name);
            }
            return GetService<ILogger>();
        }


        public static T GetService<T>()
        {
            return (T)HttpContext.RequestServices.GetService(typeof(T));
        }
    }


    public static class StaticCoreContextExtensions
    {

        public static void AddCoreContextProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IMemCache, MemCache>();

            //add configuration
            CoreContextProvider.Configuration = configuration;
        }

        /// <summary>
        /// Common context
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCoreContextProvider(this IApplicationBuilder app)
        {
            //var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            //var hostingEnvironment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            //CoreContextProvider.Configure(httpContextAccessor, hostingEnvironment);
            app.UseMiddleware<CoreContextProvider>();
            return app;
        }
    }
}
