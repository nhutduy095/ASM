using Application.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Data
{
    class MongoDBService
    {
        private readonly IMongoCollection<CollectionClass> _playlistCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _playlistCollection = database.GetCollection<CollectionClass>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<CollectionClass>> GetAsync() {
            return await _playlistCollection.Find(new BsonDocument()).ToListAsync();
        }
        public async Task CreateAsync(CollectionClass playlist) { }
        public async Task AddToPlaylistAsync(string id, string movieId) { }
        public async Task DeleteAsync(string id) { }
    }
}
