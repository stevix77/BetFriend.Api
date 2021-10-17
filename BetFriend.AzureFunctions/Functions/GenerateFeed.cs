namespace BetFriend.AzureFunctions.Functions
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Usecases.GenerateFeed;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Domain.Members.Events;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Threading.Tasks;


    public class GenerateFeed
    {
        private readonly IBetModule _betModule;

        public GenerateFeed(IBetModule betModule)
        {
            _betModule = betModule;
        }

        [FunctionName("GenerateFeed")]
        public async Task Run([QueueTrigger("membercreated", Connection = "azurestorageconnectionstring")] string jsonEvent, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {jsonEvent}");
            var ev = JsonConvert.DeserializeObject<MemberCreated>(jsonEvent);
            var command = new GenerateFeedCommand(new MemberId(ev.MemberId));
            await _betModule.ExecuteCommandAsync(command).ConfigureAwait(false);
        }
    }
}
