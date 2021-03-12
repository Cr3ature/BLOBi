using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BLOBi.Core.Internals;
using BLOBi.Core.Models;
using Microsoft.Extensions.Options;

namespace BLOBi.Core.Services
{
    internal sealed class BlobContainerService : IBlobContainerService
    {
        private readonly AzureStorageManagement _azureStorageOptions;

        public BlobContainerService(IOptions<AzureStorageManagement> azureStorageOptions)
        {
            _azureStorageOptions = azureStorageOptions.Value;
        }

        public async Task<bool> AppendContainerMetaData(string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default)
        {
            BlobContainerClient containerClient = BlobStorageManager.GetBlobContainerClient(_azureStorageOptions, containerName);

            BlobContainerProperties blobContainerProperties = await containerClient.GetPropertiesAsync();
            IDictionary<string, string> existingMetaData = blobContainerProperties.Metadata;

            foreach (KeyValuePair<string, string> metaItem in metaData)
            {
                existingMetaData.Add(metaItem);
            }

            Response<BlobContainerInfo> result = await containerClient.SetMetadataAsync(metadata: metaData, cancellationToken: cancellationToken);

            return result.GetRawResponse().Status == (int)HttpStatusCode.OK;
        }

        public async Task<bool> CreateContainerAsync(string containerName, CancellationToken cancellationToken)
                    => await CreateContainer(containerName, default, cancellationToken);

        public async Task<bool> CreateContainerAsync(string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken)
            => await CreateContainer(containerName, metaData, cancellationToken);

        public async Task<bool> DeleteContainerAsync(string containerName, CancellationToken cancellationToken)
            => await BlobStorageManager.GetBlobContainerClient(
                azureStorageOptions: _azureStorageOptions,
                blobContainerName: containerName).DeleteIfExistsAsync(cancellationToken: cancellationToken);

        public async Task<BlobContainerProperties> GetContainerProperties(string containerName, CancellationToken cancellationToken)
            => await BlobStorageManager.GetBlobContainerClient(_azureStorageOptions, containerName).GetPropertiesAsync();

        public async Task<BlobContainerPropertiesAndBlobList> GetContainerPropertiesAndFullDetails(string containerName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient containerClient = BlobStorageManager.GetBlobContainerClient(_azureStorageOptions, containerName);

            BlobContainerProperties blobContainerProperties = await containerClient.GetPropertiesAsync();
            IEnumerable<BlobItem> blobContainerContent = containerClient.GetBlobs();

            return new BlobContainerPropertiesAndBlobList
            {
                BlobContainerProperties = blobContainerProperties,
                BlobItems = blobContainerContent.ToArray(),
            };
        }

        public async Task<BlobItem[]> ListContainerContentAsync(string containerName, CancellationToken cancellationToken = default)
        {
            IEnumerable<BlobItem> blobs = BlobStorageManager.GetBlobContainerClient(_azureStorageOptions, containerName).GetBlobs();
            return await Task.FromResult(blobs.ToArray());
        }

        public async Task<bool> SetContainerAccessType(string containerName, PublicAccessType accessType, CancellationToken cancellationToken = default)
        {
            try
            {
                BlobContainerClient containerClient = BlobStorageManager.GetBlobContainerClient(_azureStorageOptions, containerName.ToLower(), accessType);

                _ = await containerClient.SetAccessPolicyAsync(accessType: accessType);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetContainerMetaData(string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default)
        {
            BlobContainerClient containerClient = BlobStorageManager.GetBlobContainerClient(_azureStorageOptions, containerName);

            Response<BlobContainerInfo> result = await containerClient.SetMetadataAsync(metadata: metaData, cancellationToken: cancellationToken);

            return result.GetRawResponse().Status == (int)HttpStatusCode.OK;
        }

        private async Task<bool> CreateContainer(string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default)
        {
            BlobContainerClient containerClient = BlobStorageManager.GetBlobContainerClient(_azureStorageOptions, containerName.ToLower());

            return await Task.FromResult(containerClient != null);
        }
    }
}
