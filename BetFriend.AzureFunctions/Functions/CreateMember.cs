using BetFriend.Bet.Application.Abstractions;
using BetFriend.Bet.Application.Usecases.CreateMember;
using BetFriend.UserAccess.Domain.Users.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BetFriend.AzureFunctions.Functions
{
    public class CreateMember
    {
        private readonly IBetModule _betModule;

        public CreateMember(IBetModule betModule)
        {
            _betModule = betModule;
        }

        [FunctionName("CreateMember")]
        public async Task Run([QueueTrigger("userregistered", Connection = "azurestorageconnectionstring")] string jsonEvent, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {jsonEvent}");
            var ev = JsonConvert.DeserializeObject<UserRegistered>(jsonEvent);
            var command = new CreateMemberCommand(Guid.Parse(ev.Id), ev.Username);
            await _betModule.ExecuteCommandAsync(command).ConfigureAwait(false);
        }
    }
}
