using Azure;
using Azure.Data.Tables;

namespace ABCRetailPOE.Models
{
    public class Product : ITableEntity
    {
        // Azure Table Storage properties
        public string PartitionKey { get; set; } = "PRODUCT";   
        public string RowKey { get; set; } = Guid.NewGuid().ToString(); 
        public DateTimeOffset? Timestamp { get; set; }           
        public ETag ETag { get; set; }                            

        //Product information
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public int StockQuantity { get; set; } = 0;
        public string ImageUrl { get; set; } = string.Empty;
        
    }
}
//Microsoft. (2025, 28 March). *Part 4, add a model to an ASP.NET Core MVC app*. Microsoft Learn. Retrieved [16/08/25], from https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-9.0&tabs=visual‑studio
