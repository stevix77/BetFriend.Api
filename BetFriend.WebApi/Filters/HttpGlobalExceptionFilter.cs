﻿namespace BetFriend.WebApi.Filters
{
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
            context.Result = new ObjectResult("An Error has occured") { StatusCode = StatusCodes.Status400BadRequest };
            context.ExceptionHandled = true;
        }
    }
}
