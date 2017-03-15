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
    [SqlTrigger(Name = "SqlTrigger", Target = "Logins", Event = "FOR INSERT, UPDATE")]
    public static void SqlTrigger()
    {
        using (var connection = new SqlConnection(@"context connection=true"))
        {
            connection.Open();
            var command = new SqlCommand(@"SELECT * FROM INSERTED;", connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var inserted = new Login.Sqlserver.Login(reader);
            reader.Close();
            
            switch (SqlContext.TriggerContext.TriggerAction)
            {
                case TriggerAction.Insert:
                    repo.Apply(inserted, "POST", false);
                    break;
                case TriggerAction.Update:
                    repo.Apply(inserted, "PUT", false);
                    break;
                case TriggerAction.Delete:
                    break;
                default:
                    break;
            }
        }

    }
}

