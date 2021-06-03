using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BetFriend.WebApi.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
        }
    }
}
