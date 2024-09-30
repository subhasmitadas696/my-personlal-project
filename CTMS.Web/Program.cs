using CTMS.Core;
using CTMS.Core.Security.HeaderPolicy;
using CTMS.Model.Entities.Common;
using CTMS.Repository.Container;
using CTMS.Web;
using CTMS.Web.Areas.AdminConsole;
using CTMS.Web.Filters;
using CTMS.Web.Helper;
using CTMS.Web.Middleware.SecurityMiddleware;
using CTMS.Web.Services;
using ExceptionHandling.Middlewares;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region bilingual
builder.Services.AddSingleton<CommonLocalizationService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc(options =>
{
    options.CacheProfiles.Add("Default30",
    new CacheProfile()
    {
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true
    });
}).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
builder.Services.Configure<RequestLocalizationOptions>(
    opt =>
    {
        var supportedCultures = new List<CultureInfo>
        {
                        new CultureInfo("en"),
                        new CultureInfo("or")
        };
        opt.DefaultRequestCulture = new RequestCulture("or");
        opt.SupportedCultures = supportedCultures;
        opt.SupportedUICultures = supportedCultures;
    });
#endregion
#region Hangfire
// Add services to the container.
builder.Services.AddHangfire(x => x.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            UseRecommendedIsolationLevel = true,
            PrepareSchemaIfNecessary = true,
            EnableHeavyMigrations = true,
            DisableGlobalLocks = true
        }));
builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[]
    {
    "default",Environment.MachineName.ToLower()
    };
});
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
// Add services to the container.
builder.Services.AddControllersWithViews(
    //options =>
    //{
    //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    //}
);
//builder.Services.AddControllersWithViews();
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.Configure<CustomVarModel>(builder.Configuration.GetSection("CustomVar"));
builder.Services.AddCustomContainer(builder.Configuration);
builder.Services.AddACCustomContainer(builder.Configuration);
builder.Services.AddCoreContextProvider(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddTransient<JobSchedulerHelper>();



builder.Services.UserAuthenConfig();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
});
builder.Services.AddCors(
               options => options.AddPolicy("corspolicy",
                  option => option.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                  )
               );

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCoreContextProvider();
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
app.UseDeveloperExceptionPage();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.EndsWith(".css"))
        {
            ctx.Context.Response.Headers["Content-Type"] = "text/css";
        }
    }
});
var options = new DashboardOptions
{
    Authorization = new[] { new CustomAuthorizationFilter() },
    DisplayNameFunc = (ctx, job) => job.Method.Name,
};
app.UseHangfireDashboard("/hangfire", options);
app.MapHangfireDashboard();
app.UseHttpsRedirection();
app.UseSecurityHeadersMiddleware(new SecurityHeadersBuilder().AddDefaultSecurePolicy());
app.UseSession();
app.UseCors("corspolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
