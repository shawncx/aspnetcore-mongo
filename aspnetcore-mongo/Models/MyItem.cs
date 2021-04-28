using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace aspnetcore_mongo.Models
{
    [BsonIgnoreExtraElements]
    public class MyItem
    {
        public ObjectId Id
        {
            get;
            set;
        }

        public string Name { get; set; }
    }
}
