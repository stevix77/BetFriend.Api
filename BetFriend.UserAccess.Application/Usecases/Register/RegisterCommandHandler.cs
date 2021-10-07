namespace BetFriend.UserAccess.Application.Usecases.Register
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.Shared.Domain;
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Exceptions;
    using BetFriend.UserAccess.Domain.Users;
    using System.Threading;
    using System.Threading.Tasks;


    public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IHashPassword _hashPassword;
        private readonly ITokenGenerator _tokenGenerator;

        public RegisterCommandHandler(IUserRepository userRepository,
                                      IDateTimeProvider dateTimeProvider,
                                      IHashPassword hashPassword,
                                      ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
            _hashPassword = hashPassword;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.IsUsernameExistsAsync(request.Username))
                throw new UsernameAlreadyExistsException();

            if (await _userRepository.IsEmailExistsAsync(request.Email))
                throw new EmailAlreadyExistsException();

            var user = new User(request.UserId,
                                request.Username,
                                request.Email,
                                _hashPassword.Hash(request.Password),
                                _dateTimeProvider.Now);
            await _userRepository.SaveAsync(user);
            return await _tokenGenerator.GenerateAsync(user);
        }
    }
}
