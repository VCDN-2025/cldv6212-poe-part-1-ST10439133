using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace ABCRetailPOE.Services
{
    public class FileStorageService
    {
        private readonly ShareServiceClient _shareServiceClient;

        public FileStorageService(ShareServiceClient shareServiceClient)
        {
            _shareServiceClient = shareServiceClient;
        }

        public async Task UploadFileAsync(string shareName, string directoryName, string fileName, Stream fileStream)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            await shareClient.CreateIfNotExistsAsync();

            var directoryClient = shareClient.GetDirectoryClient(directoryName);
            await directoryClient.CreateIfNotExistsAsync();

            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadRangeAsync(new HttpRange(0, fileStream.Length), fileStream);
        }

        public async Task<List<string>> ListFilesAsync(string shareName, string directoryName)
        {
            var result = new List<string>();
            try
            {
                var shareClient = _shareServiceClient.GetShareClient(shareName);
                await shareClient.CreateIfNotExistsAsync();

                var directoryClient = shareClient.GetDirectoryClient(directoryName);
                await directoryClient.CreateIfNotExistsAsync();

                await foreach (ShareFileItem item in directoryClient.GetFilesAndDirectoriesAsync())
                {
                    result.Add(item.Name);
                }
            }
            catch (RequestFailedException ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Access Error: {ex.Message}");
            }

            return result;
        }
        public async Task<Stream?> DownloadFileAsync(string shareName, string directoryName, string fileName)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            var directoryClient = shareClient.GetDirectoryClient(directoryName);
            var fileClient = directoryClient.GetFileClient(fileName);

            if (await fileClient.ExistsAsync())
            {
                var downloadInfo = await fileClient.DownloadAsync();
                return downloadInfo.Value.Content;
            }

            return null;
        }

    }
}
//Pavan Kumar (2021)Azure FILE Share Explained with DEMO Step by step Tutorial [online video], YouTube, Day Month. Available at: https://www.youtube.com/watch?v=36KZWO6cnXQ (Accessed: 22 August 2025).
