using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Xml;
using System.IO;

namespace Accounting.Sqlserver
{
    public class StoredProcedures
    {
        //public static readonly MongoDBRepository repo = new MongoDBRepository("mongodb://bosxixi.com:27017");
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
            File.WriteAllText($"c:/logs/{DateTime.Now.ToString("yyyyMMddHHmmss")}-{DateTime.Now.Ticks}-xml-enter.txt", $"{doc.Value}");
            //repo.InsertData(doc.Value);
            if (doc == null || String.IsNullOrEmpty(doc.Value))
            {
                throw new ArgumentNullException();
            }

            try
            {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(doc.Value);
                var json = dom.MapToJsonString();
                //File.WriteAllText($"c:/logs/{DateTime.Now.ToString("yyyyMMddHHmmss")}-{DateTime.Now.Ticks}-json-enter.txt", $"{json}");
                using (SqlConnection connection = new SqlConnection("context connection=true"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("insert into eventraw (InfoXML, InfoJson) values (@xml, @json)", connection);
                    command.Parameters.AddWithValue("@xml", doc.Value);
                    command.Parameters.AddWithValue("@json", json);
                    SqlContext.Pipe.ExecuteAndSend(command);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText($"c:/logs/{DateTime.Now.ToString("yyyyMMddHHmmss")}-{DateTime.Now.Ticks}-message.txt", ex.Message);
                File.WriteAllText($"c:/logs/{DateTime.Now.ToString("yyyyMMddHHmmss")}-{DateTime.Now.Ticks}-xml.txt", doc.Value);
            }
        }
    }
}
