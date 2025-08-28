using ABCRetailPOE.Models;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailPOE.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TableClient _tableClient;

        public CustomerController(IConfiguration configuration)
        {
            
            string? storageConnectionString = configuration.GetConnectionString("AzureStorage");
            if (string.IsNullOrEmpty(storageConnectionString))
            {
                throw new InvalidOperationException("AzureStorage connection string is missing or empty.");
            }

           
            var serviceClient = new TableServiceClient(storageConnectionString);
            _tableClient = serviceClient.GetTableClient("Customers");

           
            _tableClient.CreateIfNotExists();
        }
        // GET Customer
        public IActionResult Index()
        {
            var customers = _tableClient.Query<Customer>().ToList();
            return View(customers);
        }

        // GET Customer Details
        public IActionResult Details(string partitionKey, string rowKey)
        {
            var customer = _tableClient.GetEntity<Customer>(partitionKey, rowKey).Value;
            return View(customer);
        }

        // GET Customer Create
        public IActionResult Create() => View();

        // POST Customer Create
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            customer.PartitionKey = "customers";
            customer.RowKey = Guid.NewGuid().ToString();
            

            await _tableClient.AddEntityAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        // GEt Custome Edit
        public IActionResult Edit(string partitionKey, string rowKey)
        {
            var customer = _tableClient.GetEntity<Customer>(partitionKey, rowKey).Value;
            return View(customer);
        }

        // POST Customer Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer)
        {
            customer.ETag = ETag.All;
            await _tableClient.UpdateEntityAsync(customer, customer.ETag, TableUpdateMode.Replace);
            return RedirectToAction(nameof(Index));
        }

        // GET Customer Delete
        public IActionResult Delete(string partitionKey, string rowKey)
        {
            var customer = _tableClient.GetEntity<Customer>(partitionKey, rowKey).Value;
            return View(customer);
        }

        // POST Customer DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}

/*Reference:
 * Rout, P. (no date) How to Use Bootstrap in ASP.NET Core MVC, Dot Net Tutorials [online]. Available at: https://dotnettutorials.net/lesson/how-to-use-bootstrapin-asp-net-core-mvc/ (Accessed: 27 August 2025)
 * Microsoft. (2025, 20 February). *Get started with Azure Blob Storage and .NET*. Microsoft Learn. Retrieved [22/08/25], from https://learn.microsoft.com/en‑us/azure/storage/blobs/storage‑blob‑dotnet‑get‑started?tabs=azure‑ad
*/