using System.Data.SqlClient;

namespace Accounting.Sqlserver
{
    public class Login
    {
        public Login(SqlDataReader reader)
        {
            this.UserId = (string)reader[nameof(UserId)];
            this.LoginName = (string)reader[nameof(LoginName)];
            this.NormalizedLoginName = (string)reader[nameof(NormalizedLoginName)];
            this.Password = (string)reader[nameof(Password)];
            this.AllowDialIn = (bool)reader[nameof(AllowDialIn)];
            this.Enabled = (bool)reader[nameof(Enabled)];
            this.GroupName = (string)reader[nameof(GroupName)];
        }
        public string UserId { get; set; }
        public string LoginName { get; set; }
        public string NormalizedLoginName { get; set; }
        public string Password { get; set; }
        public bool AllowDialIn { get; set; }
        public bool Enabled { get; set; }
        public string GroupName { get; set; }

        public override string ToString()
        {
            return $"UserId:{UserId} LoginName:{LoginName} NormalizedLoginName:{NormalizedLoginName} Password:{Password} AllowDialIn:{AllowDialIn} Enabled:{Enabled} GroupName:{GroupName}";
        }
    }
}
