using MongoDB.Bson;
using SharedProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.API.Models
{
    public class RemoteAccessConnectionObjectId : RemoteAccessConnection
    {
        public ObjectId _id { get; set; }
    }

    public class Current : RemoteAccessConnection
    {
        public int Id { get; set; }
    }
}
