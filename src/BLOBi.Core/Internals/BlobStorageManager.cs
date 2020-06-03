using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BLOBi.Core.Internals
{
    internal static class BlobStorageManager
    {
        internal static BlobContainerClient GetBlobContainerClient(string connectionString, string containerName, PublicAccessType publicAccessType = PublicAccessType.None)
        {
            var blobContainerClient = new BlobContainerClient(connectionString: connectionString, blobContainerName: containerName);

            blobContainerClient.CreateIfNotExists(publicAccessType);

            return blobContainerClient;
        }

        internal static BlobClient GetBlobClient(string connectionString, string containerName, string blobName)
            => GetBlobContainerClient(connectionString, containerName).GetBlobClient(blobName);

        internal static BlobClient GetBlobClient(string connectionString, string containerName, string blobName, PublicAccessType publicAccessType = PublicAccessType.None)
            => GetBlobContainerClient(connectionString, containerName, publicAccessType).GetBlobClient(blobName);
    }
}
