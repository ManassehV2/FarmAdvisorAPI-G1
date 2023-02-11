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

        public TableStorageService(string TableName)
        {
            this.TableName = TableName;
            this.connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
            
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

        public async Task<List<T>> GetEntitiesAsync<T> () where T: class, ITableEntity, new()
        {
            var tableClient = await GetTableClient();
            var entities = tableClient.QueryAsync<T>();
            var entitiesList = new List<T>();
            await foreach (var entitiy in entities) {
                entitiesList.Add(entitiy);
            }
            
            return entitiesList;
        }
    }
}