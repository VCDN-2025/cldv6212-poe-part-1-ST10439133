using ABCRetailPOE.Models;
using ABCRetailPOE.Services;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace ABCRetailPOE.Controllers
{
    public class OrderController : Controller
    {
        private readonly TableClient _tableClient;
        private readonly QueueService _queueService;
        private readonly string _tableName = "Orders";
        private readonly string _queueName = "order";

        public OrderController(IConfiguration configuration, QueueService queueService)
        {
            string? storageConnectionString = configuration.GetConnectionString("AzureStorage");
            if (string.IsNullOrEmpty(storageConnectionString))
            {
                throw new InvalidOperationException("AzureStorage connection string is missing or empty.");
            }

            var serviceClient = new TableServiceClient(storageConnectionString);
            _tableClient = serviceClient.GetTableClient(_tableName);
            _tableClient.CreateIfNotExists();

            _queueService = queueService;
        }

        // GET Order
        public IActionResult Index()
        {
            var orders = _tableClient.Query<Order>().ToList();
            return View(orders);
        }

        // GET Order Details
        public IActionResult Details(string partitionKey, string rowKey)
        {
            var order = _tableClient.GetEntity<Order>(partitionKey, rowKey).Value;
            return View(order);
        }

        // GET Order Create
        public IActionResult Create()
        {
            ViewData["StatusList"] = new SelectList(new List<string>
            {
                "Pending",
                "Processing",
                "Shipped",
                "Delivered",
                "Cancelled"
            });

            return View();
        }



        // POST Order Create
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewData["StatusList"] = new SelectList(new List<string>
                {
                    "Pending",
                    "Processing",
                    "Shipped",
                    "Delivered",
                    "Cancelled"
                }, order.Status);

                return View(order);
            }

            order.PartitionKey = "orders";
            order.RowKey = Guid.NewGuid().ToString();
            order.OrderDate = DateTime.UtcNow;
            

            await _tableClient.AddEntityAsync(order);
            
            await _queueService.SendMessageAsync(_queueName, JsonSerializer.Serialize(order));
            




            return RedirectToAction(nameof(Index));
        }

        // GET Order Edit
        public IActionResult Edit(string partitionKey, string rowKey)
        {
            var order = _tableClient.GetEntity<Order>(partitionKey, rowKey).Value;

            ViewData["StatusList"] = new SelectList(new List<string>
            {
                "Pending",
                "Processing",
                "Shipped",
                "Delivered",
                "Cancelled"
            }, order.Status);

            return View(order);
        }

        // POST Order Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewData["StatusList"] = new SelectList(new List<string>
                {
                    "Pending",
                    "Processing",
                    "Shipped",
                    "Delivered",
                    "Cancelled"
                }, order.Status);

                return View(order);
            }

            
            if (order.OrderDate.Kind == DateTimeKind.Unspecified)
            {
                order.OrderDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);
            }
            else if (order.OrderDate.Kind == DateTimeKind.Local)
            {
                order.OrderDate = order.OrderDate.ToUniversalTime();
            }

            order.ETag = ETag.All;
            await _tableClient.UpdateEntityAsync(order, order.ETag, TableUpdateMode.Replace);

            return RedirectToAction(nameof(Index));
        }

        // GET Order Delete
        public IActionResult Delete(string partitionKey, string rowKey)
        {
            var order = _tableClient.GetEntity<Order>(partitionKey, rowKey).Value;
            return View(order);
        }

        // POST Order DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}

/* References:
 * Rout, P. (no date) How to Use Bootstrap in ASP.NET Core MVC, Dot Net Tutorials [online]. Available at: https://dotnettutorials.net/lesson/how-to-use-bootstrapin-asp-net-core-mvc/ (Accessed: 24 August 2025)
 * Microsoft. (2025, 20 February). *Get started with Azure Blob Storage and .NET*. Microsoft Learn. Retrieved [22/08/25], from https://learn.microsoft.com/en‑us/azure/storage/blobs/storage‑blob‑dotnet‑get‑started?tabs=azure‑ad
*/