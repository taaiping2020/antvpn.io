using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace Accounting.Sqlserver
{
    public class StoredProcedures
    {
        public static readonly MongoDBRepository repo = new MongoDBRepository("mongodb://bosxixi.com:27017");
        [SqlProcedure]
        public static void GetVersion()
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select @@version",
                                                     connection);
                SqlContext.Pipe.ExecuteAndSend(command);
            }
        }

        [SqlProcedure]
        public static void EventReport(SqlString doc)
        {
            repo.InsertData(doc.Value);
        }
    }
}
