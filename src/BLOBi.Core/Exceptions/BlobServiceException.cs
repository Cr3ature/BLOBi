using System;

namespace BLOBi.Core.Exceptions
{
    public sealed class BlobServiceException : Exception
    {
        public BlobServiceException(string message, string blobName, string containerName, string methodName, Exception innerException)
            : base(message, innerException)
        {
            BlobName = blobName;
            ContainerName = containerName;
            MethodName = methodName;

            Data.Add(nameof(BlobName), BlobName);
            Data.Add(nameof(ContainerName), ContainerName);
            Data.Add(nameof(MethodName), MethodName);
        }

        public string BlobName { get; }
        public string ContainerName { get; }
        public string MethodName { get; }
    }
}
