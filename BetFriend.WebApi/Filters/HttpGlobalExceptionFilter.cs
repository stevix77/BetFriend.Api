namespace BetFriend.WebApi.Filters
{
    using BetFriend.UserAccess.Domain.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using System;

    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnException(ExceptionContext context)
        {
            if (context is null)
                return;

            _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);
            BuildException(context);
        }

        private static void BuildException(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                EmailNotValidException => new ObjectResult(context.Exception.Message) { StatusCode = StatusCodes.Status400BadRequest },
                _ => new ObjectResult("An Error has occured") { StatusCode = StatusCodes.Status400BadRequest },
            };
            context.ExceptionHandled = true;
        }
    }
}
