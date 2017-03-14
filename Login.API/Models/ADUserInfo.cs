using Extensions.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Extensions;

namespace Login.API.Controllers
{
    public class ADUserInfo
    {
        public ADUserInfo(PSObject pso)
        {
            this.SID = pso.GetValue(nameof(SID));
            this.Name = pso.GetValue(nameof(Name));
            this.SamAccountName = pso.GetValue(nameof(SamAccountName));
            this.AccountExpirationDate = pso.ParseDate(nameof(AccountExpirationDate));
            this.accountExpires = pso.ParseTimeSpan(nameof(accountExpires));
            this.Created = pso.ParseDate(nameof(Created));
            this.LastLogonDate = pso.ParseDate(nameof(LastLogonDate));
            this.Modified = pso.ParseDate(nameof(Modified));
            this.msNPAllowDialin = pso.ParseBool(nameof(msNPAllowDialin));
        }
        public string SID { get; set; }
        public string Name { get; set; }
        public string SamAccountName { get; set; }
        public DateTimeOffset? AccountExpirationDate { get; set; }
        public TimeSpan? accountExpires { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? LastLogonDate { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public bool? msNPAllowDialin { get; set; }
        public IEnumerable<string> MemberOf
        {
            get
            {
                yield break;
                //using (var ps = PowerShell.Create())
                //{
                //    ps.AddCommand("Get-ADPrincipalGroupMembership");
                //    ps.AddParameter("Identity", this.Name);
                //    var psos = ps.Invoke();
                //    if (psos.IsNullOrCountEqualsZero())
                //    {
                //        yield break;
                //    }

                //    foreach (var p in psos)
                //    {
                //        yield return p.GetValue("Name");
                //    }
                //}
            }
        }
    }
}
