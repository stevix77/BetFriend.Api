namespace BetFriend.Bet.Infrastructure.Configuration.Behaviors
{
    using BetFriend.Bet.Application.Abstractions.Command;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : ICommand<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                var result = await next().ConfigureAwait(false);
                await _unitOfWork.Commit().ConfigureAwait(false);
                return result;
            }
            catch
            {
                await _unitOfWork.Rollback().ConfigureAwait(false);
                throw;
            }
        }
    }
}
