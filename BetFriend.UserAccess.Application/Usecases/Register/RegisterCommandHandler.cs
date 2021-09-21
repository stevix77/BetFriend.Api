using BetFriend.UserAccess.Domain;
using BetFriend.UserAccess.Domain.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BetFriend.UserAccess.Application.Usecases.Register
{
    public class RegisterCommandHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RegisterCommandHandler(IUserRepository userRepository,
                                      IDateTimeProvider dateTimeProvider)
        {
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.SaveAsync(new User(request.UserId,
                                                     request.Username,
                                                     request.Password,
                                                     request.Email,
                                                     _dateTimeProvider.Now));

            return Unit.Value;
        }
    }
}
