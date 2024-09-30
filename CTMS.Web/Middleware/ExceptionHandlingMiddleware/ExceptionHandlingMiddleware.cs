using ExceptionHandling.Models.Responses;
using NLog;
using System.Net;
using System.Text.Json;
namespace ExceptionHandling.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IConfiguration Configuration;
    private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        Configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var controller = context.Request.RouteValues["controller"]!.ToString();
        var Action = context.Request.RouteValues["action"]!.ToString();
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorResponse = new ErrorResponse
        {
            Success = false
        };
        switch (exception)
        {
            case ApplicationException ex:
                if (ex.Message.Contains("Invalid token"))
                {
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = ex.Message;
                    break;
                }
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;
            case NotImplementedException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;
            case UnauthorizedAccessException ex :
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;
            case DivideByZeroException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;
            case NullReferenceException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;
            case IndexOutOfRangeException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;
            case FormatException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;
            case TimeoutException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;
            case KeyNotFoundException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = ex.Message;
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Statuscode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "Internal Server errors. Check Logs!";
                break;
        }
        _logger.LogError(exception.Message);
        var config = LogManager.Configuration;
        bool torf = Configuration.GetValue<bool>("LogAuth:LKey");
        if (torf)
        {
            if (config.LoggingRules.Count > 2)
            {
                config.FindRuleByName("rule1").DisableLoggingForLevels(NLog.LogLevel.Trace, NLog.LogLevel.Info);
                config.RemoveRuleByName("rule1");
                config.FindRuleByName("rule2").DisableLoggingForLevels(NLog.LogLevel.Trace, NLog.LogLevel.Info);
                config.RemoveRuleByName("rule2");
                config.Install(new NLog.Config.InstallationContext());
            }
        }
        else
        {
            if (config.LoggingRules.Count > 3)
            {
                config.FindRuleByName("rule3").DisableLoggingForLevels(NLog.LogLevel.Trace, NLog.LogLevel.Info);
                config.RemoveRuleByName("rule3");
            }
        }
        LogManager.Configuration = config;
        logger.Error("| " + controller + "/" + Action + " || System.Exception: " + exception.Message + "|| Error Trace:" + exception.StackTrace);
        var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
    }
}

