using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace PodcastNotifications.Functions.Storage
{
    public class BlobClient
    {
        private readonly BlobContainerClient _client;

        public BlobClient(BlobClientSettings settings)
        {
            var client = new BlobServiceClient(settings.ConnectionString);
            _client = client.GetBlobContainerClient(settings.ContainerName);
        }

        public async Task Upload(string content, string fileName)
        {
            var blobClient = _client.GetBlobClient(fileName);
            var bytes = Encoding.UTF8.GetBytes(content);
            await blobClient.DeleteIfExistsAsync();
            await blobClient.UploadAsync(new MemoryStream(bytes));
        }
    }
}
