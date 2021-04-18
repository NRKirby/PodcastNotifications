using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace PodcastNotifications.Functions.Repositories
{
    public interface ITableRepository<TEntity> where TEntity : TableEntity
    {
        Task<IEnumerable<TEntity>> ListAsync(string partitionKey = null);
        Task InsertAsync(TEntity entity);
        Task InsertManyAsync(IEnumerable<TEntity> entities, string partitionKey);
        Task<TEntity> RetrieveAsync(string partitionKey, string rowKey);
        Task RemoveAsync(TEntity entity);
    }
}
