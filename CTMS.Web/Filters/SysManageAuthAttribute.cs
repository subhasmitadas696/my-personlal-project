using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace CTMS.Web
{
    /// <summary>
    /// System background Login authorization certification scheme
    /// </summary>
    public class SysManageAuthAttribute : AuthorizeAttribute
    {
        public const string SysManageAuthScheme = "SysManageAuthScheme";
        public SysManageAuthAttribute()
        {
            base.AuthenticationSchemes = SysManageAuthScheme;
        }
    }


    /// <summary>
    /// Foreground user Login authentication scheme
    /// </summary>
    public class MemberAuthAttribute : AuthorizeAttribute
    {
        public const string MemberAuthScheme = "MemberAuthScheme";
        public MemberAuthAttribute()
        {
            base.AuthenticationSchemes = MemberAuthScheme;
        }
    }



    /// <summary>
    /// Authorization certification
    /// </summary>
    public static class AuthenticationFactory
    {
        /// <summary>
        /// User Login configuration
        /// </summary>
        /// <param name="services"></param>
        public static void UserAuthenConfig(this IServiceCollection services)
        {
            //Multiple Login authorization methods, foreground / background reference [https://www.cnblogs.com/sky-net/p/8669892.html]
            services.AddAuthentication(SysManageAuthAttribute.SysManageAuthScheme)
            .AddCookie(SysManageAuthAttribute.SysManageAuthScheme, o =>
            {//Backstage.
                o.LoginPath = new PathString("/Authentication/LogIn");
                o.AccessDeniedPath = new PathString("/Authentication/Forbidden");
                o.LogoutPath = new PathString("/Home/SessionOut");
                o.SlidingExpiration = true;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            }).AddCookie(MemberAuthAttribute.MemberAuthScheme, o => //Front desk.
            {
                o.LoginPath = new PathString("/Authentication/TroupeLogIn");
                o.AccessDeniedPath = new PathString("/Authentication/Forbidden");
                o.LogoutPath = new PathString("/Home/SessionOut");
                o.ReturnUrlParameter = "ReturnUrl";
            });

        }
    }
}
