using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Post.Service.Base.BaseResponse;
using Post.Service.DTO.Request;

namespace Post.Service.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusCodeExceptionMiddleware> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public HttpStatusCodeExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<HttpStatusCodeExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);



            }
            catch (HttpStatusCodeException ex)
            {
                if (context.Response.HasStarted)
                {
                    // _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                // _logger.LogDebug("test");
                context.Response.Clear();
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = ex.ContentType;

                var result = new ErrorResult
                {
                    Code = ex.StatusCode,
                    Status = ex.StatusCode,
                    Message = ex.Message
                };

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };
                var content = JsonConvert.SerializeObject(result, settings);
                _logger.LogError(content);
                _logger.LogError(ex.StackTrace);
                //context.Response.ContentType = "application/json";
                if (ex.InnerException != null)
                    _logger.LogError(ex.InnerException.ToString());
                await context.Response.WriteAsync(content);

                return;
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    //  _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };
                var content = "";
                //_logger.LogDebug("test");
                context.Response.Clear();
                context.Response.ContentType = "application/json";

                if (ex.Message.Contains("isFluentError"))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    dynamic modifiedFluentErrorResponse = new System.Dynamic.ExpandoObject();
                    FluentErrorResponse fluentError = JsonConvert.DeserializeObject<FluentErrorResponse>(value: ex.Message);
                    modifiedFluentErrorResponse.Code = fluentError.Code;
                    modifiedFluentErrorResponse.Status = fluentError.Status;
                    modifiedFluentErrorResponse.Message = fluentError.Message;
                    content = JsonConvert.SerializeObject(modifiedFluentErrorResponse, settings);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    var error = new ErrorResult
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    };
                    content = JsonConvert.SerializeObject(error, settings);
                }
                _logger.LogError(content);
                _logger.LogError(ex.StackTrace);
                if (ex.InnerException != null)
                    _logger.LogError(ex.InnerException.ToString());
                await context.Response.WriteAsync(content);
                return;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpStatusCodeExceptionMiddleware>();
        }
    }
}
