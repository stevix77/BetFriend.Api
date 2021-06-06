namespace BetFriend.Infrastructure
{
    using BetFriend.Application.Abstractions;
    using BetFriend.Application.Abstractions.Command;
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
    }
}
