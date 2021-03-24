using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BLOBi.Core.Models;

namespace BLOBi.Core.Internals
{
    internal static class BlobStorageManager
    {
        internal static BlobClient GetBlobClient(AzureStorageManagement azureStorageOptions, string blobContainerName, string blobName)
            => GetBlobContainerClient(azureStorageOptions, blobContainerName).GetBlobClient(blobName);

        internal static BlobClient GetBlobClient(AzureStorageManagement azureStorageOptions, string containerName, string blobName, PublicAccessType publicAccessType = PublicAccessType.None)
            => GetBlobContainerClient(azureStorageOptions, containerName, publicAccessType).GetBlobClient(blobName);

        internal static BlobContainerClient GetBlobContainerClient(AzureStorageManagement azureStorageOptions, string blobContainerName, PublicAccessType publicAccessType = PublicAccessType.None)
        {
            BlobContainerClient blobContainerClient = azureStorageOptions.UseManagedIdentity
                ? GetBlobContainerClientWithManagedIdentity(azureStorageOptions, blobContainerName)
                : GetBlobContainerClientWithConnectionOptions(azureStorageOptions, blobContainerName);

            blobContainerClient.CreateIfNotExists(publicAccessType);

            return blobContainerClient;
        }

        private static BlobContainerClient GetBlobContainerClientWithConnectionOptions(AzureStorageManagement azureStorageOptions, string blobContainerName)
            => string.IsNullOrWhiteSpace(azureStorageOptions.ConnectionString)
                ? new BlobContainerClient($"DefaultEndpointsProtocol=https;AccountName={azureStorageOptions.AccountName};AccountKey={azureStorageOptions.AccountKey};EndpointSuffix=core.windows.net", blobContainerName)
                : new BlobContainerClient(azureStorageOptions.ConnectionString, blobContainerName);

        private static BlobContainerClient GetBlobContainerClientWithManagedIdentity(AzureStorageManagement azureStorageOptions, string blobContainerName)
            => new(new Uri($"https://{azureStorageOptions.AccountName}.blob.core.windows.net/{blobContainerName}"), new Azure.Identity.DefaultAzureCredential());
    }
}
