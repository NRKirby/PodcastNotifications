using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using PodcastNotifications.Functions.Entities;

namespace PodcastNotifications.Functions.Repositories
{
    public class PodcastSubscriptionRepository : TableBase<PodcastSubscription>, ITableRepository<PodcastSubscription>
    {
        public PodcastSubscriptionRepository(string connectionString, string tableName) : base(connectionString, tableName)
        {
        }

        public Task<IEnumerable<PodcastSubscription>> ListAsync(string partitionKey = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task InsertAsync(PodcastSubscription entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);
            await Table.ExecuteAsync(operation);
        }

        public Task InsertManyAsync(IEnumerable<PodcastSubscription> entities, string partitionKey)
        {
            throw new System.NotImplementedException();
        }

        public  async Task<PodcastSubscription> RetrieveAsync(string partitionKey, string rowKey)
        {
            var tableOperation = TableOperation.Retrieve<PodcastSubscription>(partitionKey, rowKey);
            var tableResult = await Table.ExecuteAsync(tableOperation);
            return tableResult.Result as PodcastSubscription;
        }

        public async Task RemoveAsync(PodcastSubscription entity)
        {
            var tableOperation = TableOperation.Delete(entity);
            await Table.ExecuteAsync(tableOperation);
        }
    }
}
