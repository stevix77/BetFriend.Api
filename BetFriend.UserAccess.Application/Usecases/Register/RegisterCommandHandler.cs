namespace BetFriend.UserAccess.Application.Usecases.Register
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.Shared.Domain;
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Users;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;


    public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IHashPassword _hashPassword;

        public RegisterCommandHandler(IUserRepository userRepository,
                                      IDateTimeProvider dateTimeProvider,
                                      IHashPassword hashPassword)
        {
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
            _hashPassword = hashPassword;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.IsUserExistsAsync(request.Username, request.Email))
            {
                await _userRepository.SaveAsync(new User(request.UserId,
                                                     request.Username,
                                                     request.Email,
                                                     _hashPassword.Hash(request.Password),
                                                     _dateTimeProvider.Now));
            }

            return Unit.Value;
        }
    }
}
