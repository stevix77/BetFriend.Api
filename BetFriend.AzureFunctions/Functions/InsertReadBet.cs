namespace BetFriend.AzureFunctions.Functions
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Usecases.InsertBetQuerySide;
    using BetFriend.Bet.Domain.Bets.Events;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class InsertReadBet
    {
        private readonly IBetModule _betModule;

        public InsertReadBet(IBetModule betModule)
        {
            _betModule = betModule;
        }

        [FunctionName("InsertReadBet")]
        public async Task Run([QueueTrigger("betcreated", Connection = "azurestorageconnectionstring")] string jsonEvent, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {jsonEvent}");
            var ev = JsonConvert.DeserializeObject<BetCreated>(jsonEvent);
            var notification = new InsertBetQuerySideNotification(ev.BetId.Value, ev.CreatorId.Value);
            await _betModule.ExecuteNotificationAsync(notification).ConfigureAwait(false);
        }
    }
}
