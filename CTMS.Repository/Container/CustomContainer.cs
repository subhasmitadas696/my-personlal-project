using CTMS.Repository.Factory;
using CTMS.Repository.Repositories.Interfaces;
using CTMS.Repository.Repositories.Repository;
using CTMS.Repository.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NEWCMS.Repository.Repositories.Repository;

namespace CTMS.Repository.Container
{
    public static class CustomContainer
    {
        public static void AddCustomContainer(this IServiceCollection services, IConfiguration configuration)
        {
            ICTMSConnectionFactory CTMSconnectionFactory = new CTMSConnectionFactory(configuration.GetConnectionString("DBconnectionCTMS"));
            services.AddSingleton<ICTMSConnectionFactory>(CTMSconnectionFactory);

            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IMasterRepository, MasterRepository>();
            services.AddScoped<IApprovalConfigRepository, ApprovalConfigRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IRegistraionRepository, RegistraionRepository>();

            services.AddScoped<IEventRepository,EventRepository>();
            services.AddScoped<IFundManagementRepository, FundManagementRepository>();
            //Write code here
            //CMS services
            services.AddScoped<IFaqQRepository, FaqRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IGalleryRepository, GalleryRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IPageMasterRepository, PageMasterRepository>();
            services.AddScoped<IChangePasswordRepository, ChangePasswordRepository>();
            services.AddScoped<IAdminMasterRepository, AdminMasterRepository>();
            services.AddScoped<IPaymentStructureRepository, PaymentStructureRepository>();
            services.AddScoped<INewsAndUpdateRepository, NewsAndUpdateRepository>();

        }
    }
}
