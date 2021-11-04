namespace BetFriend.WebApi.Filters
{
    using BetFriend.Bet.Application.Exceptions;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.UserAccess.Domain.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;

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
                EmailNotValidException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                AnswerTooLateException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                BetUnknownException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                BetNotFoundException => BuildObjectResult(context.Exception, StatusCodes.Status404NotFound),
                MemberNotFoundException => BuildObjectResult(context.Exception, StatusCodes.Status404NotFound),
                EndDateNotValidException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                MemberAlreadyExistsException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                MemberHasNotEnoughCoinsException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                MemberUnknownException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                NotAuthenticatedException => BuildObjectResult(context.Exception, StatusCodes.Status401Unauthorized),
                AuthenticationNotValidException => BuildObjectResult(context.Exception, StatusCodes.Status403Forbidden),
                EmailAlreadyExistsException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                UserIdNotValidException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                UsernameAlreadyExistsException => BuildObjectResult(context.Exception, StatusCodes.Status400BadRequest),
                _ => new ObjectResult("An Error has occured") { StatusCode = StatusCodes.Status400BadRequest },
            };
            context.ExceptionHandled = true;
        }

        private static ObjectResult BuildObjectResult(Exception exception, int httpStatus)
        {
            return new ObjectResult(new ProblemDetails
            {
                Detail = exception.Message,
                Title = exception.GetType().Name.ToLower().Split("exception").First()
            })
            {
                StatusCode = httpStatus
            };
        }
    }
}
