using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public partial class Triggers
{        
    // 为目标输入现有表或视图，并取消注释属性行
    [SqlTrigger(Name= "SqlTrigger_Update", Target="Logins", Event="FOR UPDATE")]
    public static void SqlTrigger_Update ()
    {
        SqlConnection connection = new SqlConnection("context connection = true");
        connection.Open();
        SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * from " + "inserted";

        SqlContext.Pipe.ExecuteAndSend(command);
        // 替换为您自己的代码
        //SqlContext.Pipe.Send("触发器已激发");
    }

    [SqlTrigger(Name = "SqlTrigger_Insert", Target = "Logins", Event = "FOR INSERT")]
    public static void SqlTrigger_Insert()
    {
        SqlConnection connection = new SqlConnection("context connection = true");
        connection.Open();
        SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * from " + "inserted";

        SqlContext.Pipe.ExecuteAndSend(command);
        // 替换为您自己的代码
        //SqlContext.Pipe.Send("触发器已激发");
    }
}

