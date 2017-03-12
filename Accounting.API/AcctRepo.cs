using Accounting.API.Models;
using Extensions;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API
{
    public class AcctRepo
    {
        public AcctRepo() : this(null)
        {

        }
        public AcctRepo(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                this.client = new MongoClient("mongodb://bosxixi.com:27017");
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
        public IEnumerable<BsonDocument> GetAll()
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var list = collection.Aggregate()
              .Group(new JsonProjectionDefinition<BsonDocument>(@"{
                          '_id': '$TimeStamp',
                          'username': { '$first': '$User_Name' }
                }"))
              .ToList();
            return list;
        }

        public IEnumerable<RemoteAccessConnectionObjectId> GetCurrent()
        {
            var database = client.GetDatabase("accountingdata");
            var meta = database.GetCollection<BsonDocument>("meta");
            var first = meta.Find(new BsonDocument() { new BsonElement("name", "currenttimestamp") }).First();
            DateTime timestamp = first["timestamp"].AsDateTime;

            var collection = database.GetCollection<RemoteAccessConnectionObjectId>("current");
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Eq("TimeStamp", timestamp);
            return collection.Find(t => t.TimeStamp == timestamp).ToList();
        }

        public IEnumerable<BsonDocument> Get(string userName, bool getStart, bool getStop)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var filterBuilder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter;
            if (getStart && getStop)
            {
                filter = filterBuilder.Eq("User_Name", userName) & (filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Start) | filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Stop));
            }
            else if (getStart)
            {
                filter = filterBuilder.Eq("User_Name", userName) & filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Start);
            }
            else if (getStop)
            {
                filter = filterBuilder.Eq("User_Name", userName) & filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Stop);
            }
            else
            {
                filter = filterBuilder.Eq("User_Name", userName);
            }
            var list = collection.Find(filter).ToList();

            return list;
        }
        public IEnumerable<string> GetAllUserNames()
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Start);
            var list = collection.Aggregate()
                .Match(filter)
                .Group(new JsonProjectionDefinition<BsonDocument>(@"{
                          '_id': '$User_Name',
                }"))
                .ToList();
            var usernames = list.Select(c => c["_id"].AsString).ToArray();
            return usernames;
        }
        public IEnumerable<Acct> GetAllAcct(IEnumerable<string> userNames)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var filterBuilder = Builders<BsonDocument>.Filter;
            //var filter = filterBuilder.Eq("User_Name", userName) & filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Start);
            var filter = filterBuilder.In("User_Name", userNames);
            var list = collection.Aggregate()
                .Match(filter)
                .Group(new JsonProjectionDefinition<BsonDocument>(@"{
                          '_id': '$MS_RAS_Correlation_ID',
                          'username': { '$first': '$User_Name' }
                }"))
                .ToList();
            var accts = list.Select(c => new Acct
            {
                MSRASCorrelationID = c["_id"].AsString,
                UserName = c["username"].AsString
            }).ToArray();
            return accts;
        }
        public IEnumerable<Acct> GetStoppedAcct(IEnumerable<string> userNames)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.In("User_Name", userNames) & filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Stop);
            var list = collection.Aggregate()
                .Match(filter)
                .Group(new JsonProjectionDefinition<BsonDocument>(@"{
                          '_id': '$MS_RAS_Correlation_ID',
                          'username': { '$first': '$User_Name' }
                }"))
                .ToList();
            var accts = list.Select(c => new Acct
            {
                MSRASCorrelationID = c["_id"].AsString,
                UserName = c["username"].AsString
            }).ToArray();
            return accts;
        }
        public IEnumerable<Acct> GetStoppedAcctWithDocs(IEnumerable<Acct> accts, DateTime? beginTime, DateTime? endTime)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var filterBuilder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = null;
            filter = filterBuilder.In("MS_RAS_Correlation_ID", accts.Select(c => c.MSRASCorrelationID));
            foreach (var acct in accts)
            {
                if (beginTime != null)
                {
                    filter = filter & filterBuilder.Gte("Event_Timestamp", beginTime.Value);
                }
                if (endTime != null)
                {
                    filter = filter & filterBuilder.Lte("Event_Timestamp", endTime.Value);
                }
            }
            filter = filter & filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Stop);
            var list = collection.Find(filter).ToList();
            var result = list.Select(c => ToAcct(c)).ToArray();
            return result;
        }
        public IEnumerable<Acct> GetStoppedAcctWithDocsByUserNames(IEnumerable<string> usernames, DateTime? beginTime, DateTime? endTime)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var filterBuilder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = null;
            filter = filterBuilder.In("User_Name", usernames);
            foreach (var acct in usernames)
            {
                if (beginTime != null)
                {
                    filter = filter & filterBuilder.Gte("Event_Timestamp", beginTime.Value);
                }
                if (endTime != null)
                {
                    filter = filter & filterBuilder.Lte("Event_Timestamp", endTime.Value);
                }
            }
            filter = filter & filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Stop);
            var list = collection.Find(filter).ToList();
            var result = list.Select(c => ToAcct(c)).ToArray();
            return result;
        }
        public IEnumerable<Acct> GetStartedAcct(IEnumerable<string> userNames)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.In("User_Name", userNames) & filterBuilder.Eq("Acct_Status_Type", AcctStatusType.Start);
            var list = collection.Aggregate()
                .Match(filter)
                .Group(new JsonProjectionDefinition<BsonDocument>(@"{
                          '_id': '$MS_RAS_Correlation_ID',
                          'username': { '$first': '$User_Name' }
                }"))
                .ToList();

            var accts = list.Select(c => new Acct
            {
                MSRASCorrelationID = c["_id"].AsString,
                UserName = c["username"].AsString
            }).ToArray();
            return accts;
        }

        public IEnumerable<Acct> GetLastedAcct(IEnumerable<string> strings)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var filterBuilder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter;
            filter = filterBuilder.In("MS_RAS_Correlation_ID", strings) & filterBuilder.Eq("Acct_Status_Type", AcctStatusType.InterimUpdate);
            var list = collection.Aggregate()
                .Match(filter)
                .Group(new JsonProjectionDefinition<BsonDocument>(@"{
                              '_id': '$MS_RAS_Correlation_ID',
                              'username': { '$first': '$User_Name' },
                              'eventTimestamp': { '$max': '$Event_Timestamp' },
                    }"))

                .ToList();
            //'docs': { '$push': '$$ROOT' },
            //'eventTimestamp': { '$max': '$Event_Timestamp' },
            var accts = list.Select(c => new Acct
            {
                MSRASCorrelationID = c["_id"].AsString,
                EventTimestamp = c["eventTimestamp"].AsDateTime,
                UserName = c["username"].AsString,
            }).ToArray();
            return accts;
        }
        public IEnumerable<Acct> GetLastedAcctWithDocs(IEnumerable<Acct> accts)
        {
            var database = client.GetDatabase("accountingdata");
            var collection = database.GetCollection<BsonDocument>("accounting");
            var filterBuilder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = null;
            foreach (var acct in accts)
            {
                if (filter == null)
                {
                    filter = filterBuilder.Eq("MS_RAS_Correlation_ID", acct.MSRASCorrelationID) & filterBuilder.Eq("Event_Timestamp", acct.EventTimestamp);
                }
                else
                {
                    filter = filter | (filterBuilder.Eq("MS_RAS_Correlation_ID", acct.MSRASCorrelationID) & filterBuilder.Eq("Event_Timestamp", acct.EventTimestamp));
                }
            }
            filter = filter & filterBuilder.Eq("Acct_Status_Type", AcctStatusType.InterimUpdate);
            var list = collection.Find(filter).ToList();
            var result = list.Select(c => ToAcct(c)).ToArray();
            return result;
        }

        public Acct ToAcct(BsonDocument c)
        {
            Acct acct = new Acct();

            acct.MSRASCorrelationID = c["MS_RAS_Correlation_ID"].AsString;
            acct.EventTimestamp = c["Event_Timestamp"].AsDateTime;
            acct.AcctStatusType = (AcctStatusType)Enum.Parse(typeof(AcctStatusType), c["Acct_Status_Type"].AsDouble.ToString());
            acct.UserName = c["User_Name"].AsString;
            acct.AcctInputOctets = c["Acct_Input_Octets"].AsDouble;
            acct.AcctOutputOctets = c["Acct_Output_Octets"].AsDouble;
            acct.AcctInputPackets = c["Acct_Input_Packets"].AsDouble;
            acct.AcctOutputPackets = c["Acct_Output_Packets"].AsDouble;
            acct.AcctSessionTime = c["Acct_Session_Time"].AsDouble;
            acct.TunnelClientEndpt = c["Tunnel_Client_Endpt"].AsString;
            acct.FramedMTU = c["Framed_MTU"].AsDouble;
            try
            {
                acct.FramedIPAddress = c["Framed_IP_Address"].AsString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            acct.ClientIPAddress = c["Client_IP_Address"].AsString;
            acct.ClientFriendlyName = c["Client_Friendly_Name"].AsString;
            acct.PacketType = (PacketType)Enum.Parse(typeof(PacketType), c["Packet_Type"].AsDouble.ToString());
            acct.TunnelType = c["Tunnel_Type"].AsDouble;

            return acct;
        }
    }
}
