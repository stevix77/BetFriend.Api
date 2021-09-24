namespace BetFriend.UserAccess.Application.Usecases.SignIn
{
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.UserAccess.Domain;
    using BetFriend.UserAccess.Domain.Exceptions;
    using BetFriend.UserAccess.Domain.Users;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;


    public class SignInCommandHandler : ICommandHandler<SignInCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPassword _hashPassword;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ISignInPresenter _presenter;

        public SignInCommandHandler(IUserRepository userRepository,
                                    IHashPassword hashPassword,
                                    ITokenGenerator tokenGenerator,
                                    ISignInPresenter presenter)
        {
            _userRepository = userRepository;
            _hashPassword = hashPassword;
            _tokenGenerator = tokenGenerator;
            _presenter = presenter;
        }

        public async Task<Unit> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var password = _hashPassword.Hash(request.Password);
            var user = await _userRepository.GetByLoginPasswordAsync(request.Login, password)
                        ?? throw new AuthenticationNotValidException();
            _presenter.Present(await _tokenGenerator.GenerateAsync(user));

            return Unit.Value;
        }
    }
}
