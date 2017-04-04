// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;

namespace Identity.API.Models.AccountViewModels
{
    public class ConsentViewModel : ConsentInputModel
    {
        public ConsentViewModel(ConsentInputModel model, string returnUrl, AuthorizationRequest request, Client client, Resources scopes)
        {
            RememberConsent = model?.RememberConsent ?? true;
            ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>();

            ReturnUrl = returnUrl;

            ClientName = client.ClientName;
            ClientUrl = client.ClientUri;
            ClientLogoUrl = client.LogoUri;
            AllowRememberConsent = client.AllowRememberConsent;

            IdentityScopes = scopes.IdentityResources.Select(x => new ScopeViewModel()
            {
                Name = x.Name,
                Description = x.Description,
                DisplayName = x.DisplayName,
                Emphasize = x.Emphasize,
                Required = x.Required,
                Checked = (ScopesConsented.Contains(x.Name) || model == null) || x.Required
            }).ToArray();
            ResourceScopes = scopes.ApiResources.Select(x => new ScopeViewModel/*(ScopesConsented.Contains(x.Name) || model == null)*/
            {
                Name = x.Name,
                Description = x.Description,
                DisplayName = x.DisplayName,
                Checked = (ScopesConsented.Contains(x.Name) || model == null)
            }).ToArray();
        }

        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        public bool AllowRememberConsent { get; set; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
    }

    public class ScopeViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
    }
}
