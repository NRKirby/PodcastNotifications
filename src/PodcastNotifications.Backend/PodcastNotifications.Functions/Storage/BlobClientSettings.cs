using System;

namespace PodcastNotifications.Functions.Storage
{
    public class BlobClientSettings
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }

        public void Validate()
        {
            if (ConnectionString is null) throw new InvalidOperationException($"{nameof(ConnectionString)} was null.");
            if (ContainerName is null) throw new InvalidOperationException($"{nameof(ContainerName)} was null.");
        }
    }
}