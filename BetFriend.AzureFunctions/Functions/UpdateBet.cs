namespace BetFriend.AzureFunctions.Functions
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Usecases.UpdateBet;
    using BetFriend.Bet.Domain.Bets.Events;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Threading.Tasks;


    public class UpdateBet
    {
        private readonly IBetModule _betModule;

        public UpdateBet(IBetModule betModule)
        {
            _betModule = betModule;
        }

        [FunctionName("UpdateBet")]
        public async Task Run([QueueTrigger("betupdated", Connection = "azurestorageconnectionstring")] string jsonEvent, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {jsonEvent}");
            var ev = JsonConvert.DeserializeObject<BetUpdated>(jsonEvent);
            var command = new UpdateBetCommand(ev.BetId);
            await _betModule.ExecuteCommandAsync(command).ConfigureAwait(false);
        }
    }
}
