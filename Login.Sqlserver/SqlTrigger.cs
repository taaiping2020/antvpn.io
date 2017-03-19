using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using Login.Sqlserver;

//example code: https://msdn.microsoft.com/en-us/library/ms131093.aspx

public partial class Triggers
{
    private readonly static LoginRepo repo = new LoginRepo("http://dc.antvpn.io:5000");

    // 为目标输入现有表或视图，并取消注释属性行
    [SqlTrigger(Name = "SqlTrigger", Target = "Logins", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void SqlTrigger()
    {
        using (var connection = new SqlConnection(@"context connection=true"))
        {
            connection.Open();

            switch (SqlContext.TriggerContext.TriggerAction)
            {
                case TriggerAction.Insert:
                    {
                        var command = new SqlCommand(@"SELECT * FROM INSERTED;", connection);
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var inserted = new Login.Sqlserver.Login(reader);
                            repo.Apply(inserted, "POST", false);
                        }

                        reader.Close();
                    }
                   
                    break;
                case TriggerAction.Update:
                    {
                        var command = new SqlCommand(@"SELECT * FROM INSERTED;", connection);
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var inserted = new Login.Sqlserver.Login(reader);
                            repo.Apply(inserted, "PUT", false);
                        }

                        reader.Close();
                    }
                    
                    break;
                case TriggerAction.Delete:
                    {
                        var commandDeleted = new SqlCommand(@"SELECT * FROM DELETED;", connection);
                        var reader = commandDeleted.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var deleted = new Login.Sqlserver.Login(reader);
                                repo.Delete(deleted, false);
                            }

                            reader.Close();
                        }
                        else
                        {
                            SqlContext.Pipe.Send("No rows affected.");
                        }
                    }

                    break;
                default:
                    break;
            }
        }

    }
}

