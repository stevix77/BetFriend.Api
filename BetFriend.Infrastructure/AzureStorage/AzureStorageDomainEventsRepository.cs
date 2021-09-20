namespace BetFriend.Bet.Infrastructure.AzureStorage
{
    using Azure.Storage.Queues;
    using BetFriend.Bet.Domain;
    using BetFriend.Bet.Infrastructure.Configuration;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class AzureStorageDomainEventsRepository : IStorageDomainEventsRepository
    {
        private readonly AzureStorageConfiguration _azureStorageConfiguration;

        public AzureStorageDomainEventsRepository(AzureStorageConfiguration azureStorageConfiguration)
        {
            _azureStorageConfiguration = azureStorageConfiguration;
        }

        public async Task SaveAsync(IDomainEvent item)
        {
            var queueClient = new QueueClient(_azureStorageConfiguration.ConnectionString,
                                              item.GetType().Name.ToLower(),
                                              new QueueClientOptions()
                                              {
                                                  MessageEncoding = QueueMessageEncoding.Base64
                                              });
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(JsonConvert.SerializeObject(item));
        }
    }
}
