﻿namespace BetFriend.Infrastructure.AzureStorage
{
    using BetFriend.Domain;
    using BetFriend.Infrastructure.Configuration;
    using System.Threading.Tasks;

    public class AzureStorageDomainEventsRepository : IStorageDomainEventsRepository
    {
        public AzureStorageDomainEventsRepository(AzureStorageConfiguration azureStorageConfiguration)
        {

        }

        public Task SaveAsync(IDomainEvent item)
        {
            var r = new TableServiceClient()
            throw new System.NotImplementedException();
        }
    }
}
