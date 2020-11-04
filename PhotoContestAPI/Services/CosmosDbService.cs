using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoContestAPI.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;

namespace PhotoContestAPI.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(PhotoData photoData)
        {
            await this._container.CreateItemAsync<PhotoData>(photoData, new PartitionKey(photoData.Partition));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<PhotoData>(id, new PartitionKey());
        }

        public async Task<PhotoData> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<PhotoData> response = await this._container.ReadItemAsync<PhotoData>(id, new PartitionKey());
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<PhotoData>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<PhotoData>(new QueryDefinition(queryString));
            List<PhotoData> results = new List<PhotoData>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, PhotoData photoData)
        {
            await this._container.UpsertItemAsync<PhotoData>(photoData, new PartitionKey(photoData.Partition));
        }
    }
}
