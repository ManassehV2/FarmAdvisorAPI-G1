using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Data.Tables;
using FarmAdvisor.Models.Models;

namespace FarmAdvisor.DataAccess.AzureTableStorage.services
{

    public interface ITableStorageService
    {
        Task<List<T>> GetEntitiesAsync<T>() where T: class, ITableEntity, new();
        Task<T> GetEntityAsync<T>(string PartitionKey, string RowKey) where T: class, ITableEntity, new();
        Task<T> UpsertEntityAsync<T>(T entity) where T: class, ITableEntity, new();
        Task DeleteEntityAsync<T>(string PartitionKey, string RowKey) where T: class, ITableEntity, new();
    }
}