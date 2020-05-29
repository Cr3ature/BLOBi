using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BLOBi.Core.Services
{
    public interface IBlobMetaDataService
    {
        Task<bool> AppendMetaData(string blobName, string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default);

        Task<bool> SetBlobMetaData(string blobName, string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default);

        Task<bool> UpdateBlobMetaData(string blobName, string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default);
    }
}
