using CTMS.Core.Security.HeaderPolicy;

namespace CTMS.Web.Middleware.SecurityMiddleware
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensionsExtensions
    {
        public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app, SecurityHeadersBuilder builder)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return app.UseMiddleware<SecurityHeadersMiddleware>(builder.Build());
        }
    }
}
