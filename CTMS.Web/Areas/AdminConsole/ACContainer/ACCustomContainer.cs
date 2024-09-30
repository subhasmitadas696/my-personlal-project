using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repositories.Repository;
using CTMS.Repository.Repository;
using CTMS.Web.Areas.AdminConsole;
using CTMS.Web.Areas.AdminConsole.Interface;
using CTMS.Web.Areas.AdminConsole.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NEWCMS.Repository.Repositories.Repository;

namespace CTMS.Repository.Container
{
    public static class ACCustomContainer
    {
        public static void AddACCustomContainer(this IServiceCollection services, IConfiguration configuration)
        {
            ICTMSConnectionFactory CTMSconnectionFactory = new CTMSConnectionFactory(configuration.GetConnectionString("DBconnectionCTMS"));
            services.AddSingleton<ICTMSConnectionFactory>(CTMSconnectionFactory);

            #region AdminConsole


            services.AddScoped<IHierarchyServiceProviderRepository, HierarchyServiceProviderRepository>();
            services.AddScoped<ILevelServiceProvider, LevelServiceProvider>();
            services.AddScoped<ILevelDetailsServiceProvider, LeveDetailslServiceProvider>();
            services.AddScoped<IDesignationServiceProvider, DesignationServiceProvider>();
            services.AddScoped<IFuncServiceProvider, FuncServiceProviderNew>();
            services.AddScoped<IProjectLinkServiceProvider, ProjectLinkServiceProvider>();
            services.AddScoped<IGblLinkServiceProvider, GblLinkServiceProvider>();
            services.AddScoped<IPriLinkServiceProvider, PriLinkServiceProvider>();
            services.AddScoped<ISetPermissionServiceProvider, SetPermissionServiceProvider>();
            services.AddScoped<IUserServiceProvider, UserServiceProvider>();
            services.AddScoped<IAdminConsoleRepository,AdminConsoleRepository>();


            #endregion

        }
    }
}
