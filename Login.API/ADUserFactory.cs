using System.Management.Automation;
using Extensions.Windows;
using Login.API.Models;

namespace Login.API
{
    public static class ADUserFactory
    {
        public static ADUser Create(PSObject pso)
        {
            ADUser u = new ADUser()
            {
                Name = pso.GetValue(nameof(ADUser.Name)),
                Enabled = pso.ParseBool(nameof(ADUser.Enabled)),
                AllowDialIn = pso.ParseBool("msNPAllowDialin")
            };

            return u;
        }
    }
}
