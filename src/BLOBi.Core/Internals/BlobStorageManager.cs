using Azure.Storage.Blobs;

namespace BLOBi.Core.Internals
{
    internal static class BlobStorageManager
    {
        internal static BlobContainerClient GetBlobContainerClient(string connectionString, string containerName)
        {
            var blobContainerClient = new BlobContainerClient(connectionString: connectionString, blobContainerName: containerName);

            blobContainerClient.CreateIfNotExists();

            return blobContainerClient;
        }

        internal static BlobServiceClient GetBlobServiceClient(string connectionString)
            => new BlobServiceClient(connectionString);

        internal static BlobClient GetBlobClient(string connectionString, string containerName, string blobName)
            => GetBlobContainerClient(connectionString, containerName).GetBlobClient(blobName);
    }
}
