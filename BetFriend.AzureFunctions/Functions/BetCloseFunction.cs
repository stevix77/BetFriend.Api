namespace BetFriend.AzureFunctions.Functions
{
    using BetFriend.Bet.Application.Abstractions;
    using BetFriend.Bet.Application.Usecases.UpdateWallet;
    using BetFriend.Bet.Domain.Bets.Events;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class BetCloseFunction
    {
        private readonly IBetModule _betModule;
        public BetCloseFunction(IBetModule betModule)
        {
            _betModule = betModule;
        }

        [FunctionName("BetCloseFunction")]
        public async Task Run([QueueTrigger("betclosed", Connection = "azurestorageconnectionstring")] string jsonEvent, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {jsonEvent}");
            var ev = JsonConvert.DeserializeObject<BetClosed>(jsonEvent);
            var command = new UpdateWalletMembersCommand(ev.BetId);
            await _betModule.ExecuteCommandAsync(command).ConfigureAwait(false);

        }
    }
}
