namespace BetFriend.AzureFunctions.Functions
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;


    public class BetAnswerFunction
    {

        [FunctionName("BetAnswerFunction")]
        public void Run([QueueTrigger("betanswered", Connection = "azurestorageconnectionstring")] string jsonEvent, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {jsonEvent}");

            //send notification to bet creator for example
        }
    }
}
