using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FileProvider
{
    public class AzureBlobFileProvider : IFileProvider
    {
        private CloudBlobContainer _container;

        public AzureBlobFileProvider(AzureBlobOptions azureBlobOptions)
        {
            var blobClient = new CloudBlobClient(azureBlobOptions.BaseUri, new StorageCredentials(azureBlobOptions.Token));
            _container = blobClient.GetContainerReference(azureBlobOptions.DocumentContainer);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var blob = _container.GetDirectoryReference(subpath.TrimStart('/').TrimEnd('/'));
            return new AzureBlobDirectoryContents(blob);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var blob = _container.GetBlockBlobReference(subpath.TrimStart('/').TrimEnd('/'));
            return new AzureBlobFileInfo(blob);
        }

        public IChangeToken Watch(string filter) => throw new NotImplementedException();
    }
}
