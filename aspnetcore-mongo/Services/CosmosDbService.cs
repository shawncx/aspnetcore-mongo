using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using aspnetcore_mongo.Models;
using MongoDB.Driver;

namespace aspnetcore_mongo.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private IMongoClient _client;
        private IMongoCollection<MyItem> _collection;

        public CosmosDbService(string connectionString)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            _client = new MongoClient(settings);
            _collection = _client.GetDatabase("coreDB").GetCollection<MyItem>("MyItem");
        }

        //public async Task AddItemAsync(MyItem item)
        //{
        //    await this._container.CreateItemAsync<MyItem>(item, new PartitionKey(item.Id));
        //}

        //public async Task DeleteItemAsync(string id)
        //{
        //    await this._container.DeleteItemAsync<MyItem>(id, new PartitionKey(id));
        //}

        //public async Task<MyItem> GetItemAsync(string id)
        //{
        //    try
        //    {
        //        ItemResponse<MyItem> response = await this._container.ReadItemAsync<MyItem>(id, new PartitionKey(id));
        //        return response.Resource;
        //    }
        //    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        //    {
        //        return null;
        //    }

        //}

        public async Task<IEnumerable<MyItem>> GetItemsAsync()
        {
            IAsyncCursor<MyItem> result = await _collection.FindAsync(FilterDefinition<MyItem>.Empty).ConfigureAwait(false);
            return result.ToList();
        }

        //public async Task UpdateItemAsync(string id, MyItem item)
        //{
        //    await this._container.UpsertItemAsync<MyItem>(item, new PartitionKey(id));
        //}
    }
}
