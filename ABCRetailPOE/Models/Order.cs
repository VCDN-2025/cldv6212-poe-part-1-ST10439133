using Azure;
using Azure.Data.Tables;

namespace ABCRetailPOE.Models
{
    public class Order : ITableEntity
    {
        // Azure Table Storage properties
        public string PartitionKey { get; set; } = "ORDER";  
        public string RowKey { get; set; } = Guid.NewGuid().ToString(); 
        public DateTimeOffset? Timestamp { get; set; }        
        public ETag ETag { get; set; }                         

        // Order information
       public string FirstName { get; set; } = string.Empty; //Customer
        public string ProductName { get; set; } = string.Empty; //Product
        public int Quantity { get; set; } = 0;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "PENDING";       
        public string ShippingAddress { get; set; } = string.Empty;
         
        public decimal TotalPrice { get; set; }
    }
}

//Microsoft. (2025, 28 March). *Part 4, add a model to an ASP.NET Core MVC app*. Microsoft Learn. Retrieved [16/08/25], from https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-9.0&tabs=visual‑studio
