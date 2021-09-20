namespace BetFriend.Bet.Infrastructure.Configuration.Behaviors
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;


    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                _logger.LogInformation($"Handling {typeof(TRequest).Name}");
                IList<PropertyInfo> props = new List<PropertyInfo>(request.GetType().GetProperties());
                var builder = new StringBuilder();
                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(request, null);
                    builder.AppendLine($"{prop.Name} : {propValue}");
                }
                _logger.LogInformation($"{builder}");
                return await next().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            finally
            {
                watch.Stop();
                _logger.LogInformation($"Time elapsed: {watch.Elapsed}");
            }
        }
    }
}
