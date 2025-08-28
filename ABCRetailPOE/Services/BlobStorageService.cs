using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ABCRetailPOE.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task UploadFileAsync(string containerName, string blobName, Stream fileStream)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob); 

            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }

        public async Task<Stream?> DownloadFileAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
                return response.Value.Content;
            }

            return null;
        }

        public async Task<List<string>> ListFilesAsync(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var result = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                result.Add(blobItem.Name);
            }

            return result;
        }

        public string GetBlobUrl(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }
    }
}

//Alex The Analyst (2024) Blob Storage and Storage Accounts in Azure | Azure Fundamentals [online video], YouTube, 23 July. Available at: https://www.youtube.com/watch?v=sEImMaovc1Q (Accessed: 18 August 2025).
