using Azure;
using Azure.Data.Tables;

namespace ABCRetailPOE.Models
{
    public class Customer : ITableEntity
    {
        //Azure Table Storage properties
        public string PartitionKey { get; set; } = "CUSTOMER";  // Grouping key
        public string RowKey { get; set; } = Guid.NewGuid().ToString(); // Unique ID
        public DateTimeOffset? Timestamp { get; set; }         // Auto-managed timestamp
        public ETag ETag { get; set; }                          // Concurrency check

        // Customer information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
       

    
    }
}

//Microsoft. (2025, 28 March). *Part 4, add a model to an ASP.NET Core MVC app*. Microsoft Learn. Retrieved [16/08/25], from https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-9.0&tabs=visual‑studio
