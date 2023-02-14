using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Data.Tables;
using FarmAdvisor.Models.Models;

namespace FarmAdvisor.DataAccess.AzureTableStorage.services
{
    public class TableStorageService : ITableStorageService
    {

        private readonly string connectionString;
        private readonly string TableName;

        public TableStorageService(string TableName, string connectionString)
        {
            this.TableName = TableName;
            this.connectionString = connectionString;
            
        }

        private async Task<TableClient> GetTableClient() {
            var serviceClient = new TableServiceClient(connectionString);

            var tableClient = serviceClient.GetTableClient(TableName);
            await tableClient.CreateIfNotExistsAsync();

            return tableClient;
        }
        

        public async Task DeleteEntityAsync<T>(string PartitionKey, string RowKey) where T: class, ITableEntity, new()
        {
            var tableClient = await GetTableClient();

            await tableClient.DeleteEntityAsync(PartitionKey, RowKey);
        }

        public async Task<T> GetEntityAsync<T>(string PartitionKey, string RowKey) where T : class, ITableEntity, new()
        {
            var tableClient = await GetTableClient();
            
            return await tableClient.GetEntityAsync<T>(PartitionKey, RowKey);
        }

        public async Task<T> UpsertEntityAsync<T>(T entity) where T : class, ITableEntity, new()
        {
            var tableClient = await GetTableClient();

            await tableClient.UpsertEntityAsync<T>(entity);
            return entity;
        }
    }
}