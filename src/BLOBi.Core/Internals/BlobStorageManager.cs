using Azure.Storage.Blobs;

namespace BLOBi.Core.Internals
{
    internal static class BlobStorageManager
    {
        internal static BlobContainerClient GetBlobContainerClient(string connectionString, string containerName)
            => new BlobContainerClient(connectionString: connectionString, blobContainerName: containerName);

        internal static BlobServiceClient GetBlobServiceClient(string connectionString)
            => new BlobServiceClient(connectionString);

        internal static BlobClient GetBlobClient(string connectionString, string containerName, string blobName)
            => GetBlobContainerClient(connectionString, containerName).GetBlobClient(blobName);
    }
}
