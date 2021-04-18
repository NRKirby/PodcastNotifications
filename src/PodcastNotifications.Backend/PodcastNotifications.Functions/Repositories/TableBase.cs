using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PodcastNotifications.Functions.Repositories
{
    public abstract class TableBase<TEntity> where TEntity : TableEntity
    {
        protected readonly CloudTable Table;
        public static string TableName => nameof(TEntity);

        protected TableBase(string connectionString, string tableName)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var tableClient = account.CreateCloudTableClient();
            Table = tableClient.GetTableReference(tableName);
        }
    }
}
