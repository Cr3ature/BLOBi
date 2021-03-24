using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using BLOBi.Core.Models;

namespace BLOBi.Core
{
    public interface IBlobContainerService
    {
        Task<bool> AppendContainerMetaData(string containerName, IDictionary<string, string> metaData, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default);

        Task<bool> CreateContainerAsync(string containerName, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default);

        Task<bool> DeleteContainerAsync(string containerName, CancellationToken cancellationToken = default);

        Task<BlobContainerProperties> GetContainerProperties(string containerName, CancellationToken cancellationToken = default);

        Task<BlobContainerPropertiesAndBlobList> GetContainerPropertiesAndBlobDetails(string containerName, CancellationToken cancellationToken = default);

        Task<BlobItem[]> ListContainerContentAsync(string containerName, CancellationToken cancellationToken = default);

        Task<bool> SetContainerAccessType(string containerName, PublicAccessType publicAccessType, CancellationToken cancellationToken = default);

        Task<bool> SetContainerMetaData(string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default);
    }
}
