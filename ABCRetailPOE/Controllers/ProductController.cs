using ABCRetailPOE.Models;
using ABCRetailPOE.Services;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailPOE.Controllers
{
    public class ProductController : Controller
    {
        private readonly TableClient _tableClient;
        private readonly BlobStorageService _blobService;
        private const string TableName = "Products";
        private const string ContainerName = "product-images";

        public ProductController(IConfiguration configuration, BlobServiceClient blobClient)
        {
            
            string? storageConnectionString = configuration.GetConnectionString("AzureStorage");
            if (string.IsNullOrEmpty(storageConnectionString))
            {
                throw new InvalidOperationException("AzureStorage connection string is missing or empty.");
            }

            var serviceClient = new TableServiceClient(storageConnectionString);
            _tableClient = serviceClient.GetTableClient("Products");
            _tableClient.CreateIfNotExists();
            
            _blobService = new BlobStorageService(blobClient);
        }
        public IActionResult Index()
        {
            var products = _tableClient.Query<Product>().ToList();
            return View(products);
        }

        public IActionResult Details(string partitionKey, string rowKey)
        {
            var product = _tableClient.GetEntity<Product>(partitionKey, rowKey).Value;
            return View(product);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Product model, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var blobName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                using var stream = imageFile.OpenReadStream();
                await _blobService.UploadFileAsync(ContainerName, blobName, stream);
                model.ImageUrl = _blobService.GetBlobUrl(ContainerName, blobName);
            }

            model.PartitionKey = "products";
            model.RowKey = Guid.NewGuid().ToString();


            await _tableClient.AddEntityAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string partitionKey, string rowKey)
        {
            var product = _tableClient.GetEntity<Product>(partitionKey, rowKey).Value;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            model.ETag = ETag.All;
            await _tableClient.UpdateEntityAsync(model, model.ETag, TableUpdateMode.Replace);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string partitionKey, string rowKey)
        {
            var product = _tableClient.GetEntity<Product>(partitionKey, rowKey).Value;
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}

// Reference: Rout, P. (no date) How to Use Bootstrap in ASP.NET Core MVC, Dot Net Tutorials [online]. Available at: https://dotnettutorials.net/lesson/how-to-use-bootstrapin-asp-net-core-mvc/ (Accessed: 27 August 2025)
// Microsoft. (2025, 20 February). *Get started with Azure Blob Storage and .NET*. Microsoft Learn. Retrieved [23/08/25], from https://learn.microsoft.com/en‑us/azure/storage/blobs/storage‑blob‑dotnet‑get‑started?tabs=azure‑ad
