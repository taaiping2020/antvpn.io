using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Accounting.Sqlserver
{
    public class MongoDBRepository
    {
        public MongoDBRepository() : this(null)
        {

        }
        public MongoDBRepository(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                this.client = new MongoClient("mongodb://104.160.35.172:27017");
            }
            else
            {
                this.client = new MongoClient(connectionString);
            }
            this.formatter = new JsonWriterSettings();
            this.formatter.Indent = true;
        }

        private readonly MongoClient client;
        private readonly JsonWriterSettings formatter;
        public async Task<IEnumerable<BsonDocument>> GetAllAsync()
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var list = await collection.Find(new BsonDocument("Name", "Jack")).ToListAsync();
            return list;
        }

        public void InsertData(string xml)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var bson = doc.MapToBson();
            collection.InsertOne(bson);
        }

        public string ConvertToJson(string xml)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.MapToBson().ToJson(formatter);
        }
    }
}
