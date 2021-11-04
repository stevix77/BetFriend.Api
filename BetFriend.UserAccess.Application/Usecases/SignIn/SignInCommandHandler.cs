namespace BetFriend.UserAccess.Application.Usecases.SignIn
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Exceptions;
    using BetFriend.UserAccess.Domain.Users;
    using System.Threading;
    using System.Threading.Tasks;


    public class SignInCommandHandler : ICommandHandler<SignInCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPassword _hashPassword;
        private readonly ITokenGenerator _tokenGenerator;

        public SignInCommandHandler(IUserRepository userRepository,
                                    IHashPassword hashPassword,
                                    ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _hashPassword = hashPassword;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var password = _hashPassword.Hash(request.Password);
            var user = await _userRepository.GetByLoginPasswordAsync(request.Login, password)
                        ?? throw new AuthenticationNotValidException("The login with password does not match");
            return await _tokenGenerator.GenerateAsync(user);
        }
    }
}
