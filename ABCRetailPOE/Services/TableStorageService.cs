using ABCRetailPOE.Models;
using Azure;
using Azure.Data.Tables;
using System.Text.Json;

namespace ABCRetailPOE.Services
{
    public class TableStorageService
    {
        private readonly TableServiceClient _tableServiceClient;

        public TableStorageService(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        public async Task AddEntityAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();
            await tableClient.AddEntityAsync(entity);
        }

        public async Task<T?> GetEntityAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            try
            {
                var response = await tableClient.GetEntityAsync<T>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public async Task<List<T>> GetAllEntitiesAsync<T>(string tableName) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();

            var result = new List<T>();

            await foreach (var entity in tableClient.QueryAsync<T>())
            {
                result.Add(entity);
            }

            return result;
        }

        public async Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            await tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

      
    }
}
