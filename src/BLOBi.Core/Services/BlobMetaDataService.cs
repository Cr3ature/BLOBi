using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BLOBi.Core.Internals;
using BLOBi.Core.Models;
using Microsoft.Extensions.Options;

namespace BLOBi.Core.Services
{
    internal class BlobMetaDataService : IBlobMetaDataService
    {
        private readonly AzureStorageManagement _azureStorageOptions;

        public BlobMetaDataService(IOptions<AzureStorageManagement> azureStorageOptions)
        {
            _azureStorageOptions = azureStorageOptions.Value;
        }

        public async Task<bool> AppendMetaData(string blobName, string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default)
        {
            BlobClient client =
               BlobStorageManager.GetBlobClient(
                   connectionString: _azureStorageOptions.ConnectionString,
                   containerName: containerName,
                   blobName: blobName);

            try
            {
                BlobProperties properties = await client.GetPropertiesAsync(cancellationToken: cancellationToken);

                IDictionary<string, string> existingMetaData = properties.Metadata;

                foreach (KeyValuePair<string, string> item in metaData)
                {
                    if (existingMetaData.ContainsKey(item.Key))
                        throw new InvalidOperationException($"There is allready metadata with key: {item.Key}");

                    existingMetaData.Add(item);
                }

                Azure.Response<BlobInfo> response = await client.SetMetadataAsync(metadata: existingMetaData, cancellationToken: cancellationToken);

                return response.GetRawResponse().Status == (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SetBlobMetaData(string blobName, string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default)
        {
            BlobClient client =
                 BlobStorageManager.GetBlobClient(
                     connectionString: _azureStorageOptions.ConnectionString,
                     containerName: containerName,
                     blobName: blobName);

            try
            {
                Azure.Response<BlobInfo> response = await client.SetMetadataAsync(metadata: metaData, cancellationToken: cancellationToken);
                return response.GetRawResponse().Status == (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateBlobMetaData(string blobName, string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default)
        {
            BlobClient client =
              BlobStorageManager.GetBlobClient(
                  connectionString: _azureStorageOptions.ConnectionString,
                  containerName: containerName,
                  blobName: blobName);

            try
            {
                BlobProperties properties = await client.GetPropertiesAsync(cancellationToken: cancellationToken);

                IDictionary<string, string> existingMetaData = properties.Metadata;

                foreach (KeyValuePair<string, string> item in metaData)
                {
                    if (existingMetaData.ContainsKey(item.Key))
                    {
                        existingMetaData[item.Key] = item.Value;
                    }
                    else
                    {
                        existingMetaData.Add(item);
                    }
                }

                Azure.Response<BlobInfo> response = await client.SetMetadataAsync(metadata: existingMetaData, cancellationToken: cancellationToken);

                return response.GetRawResponse().Status == (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
