namespace BetFriend.Bet.Infrastructure
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Abstractions.Command;
    using BetFriend.Bet.Application.Abstractions.Query;
    using MediatR;
    using System.Threading.Tasks;

    public class Processor : IProcessor
    {
        private readonly IMediator _mediator;

        public Processor(IMediator mediator)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task ExecuteCommandAsync<TRequest>(ICommand<TRequest> command)
        {
            await _mediator.Send(command).ConfigureAwait(false);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            return await _mediator.Send(query).ConfigureAwait(false);
        }
    }
}
