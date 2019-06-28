using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVSSO.Configuration
{
    public class Config
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                //现有的可以调用的API都可以被加入,定义了哪些API可以用这个server
                //displayname is the name will be shown on the consent screen
                new ApiResource("Api1", "Api1"),
                new ApiResource("Api2","Api2")
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()

            };
        }

        public static IEnumerable<Client> Clients()
        {

            return new[]
            {
                 new Client
                {
                    ClientId = "Kong API",
                    //类似于客户端密码和id
                    ClientSecrets = new [] { new Secret("kongAPI".Sha256()) },
                    
           
                    //OwnerPassword 是通过用户密码来获得token
                   //ClientCredential允许只用clientScret来获取
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "Api1","Api2" }
                },
                new Client
                {
                    ClientId = "Baidu API",
                    //类似于客户端密码和id
                    ClientSecrets = new [] { new Secret("BaiduSecret".Sha256()) },
                    //OwnerPassword 是通过用户密码来获得token
                    //ClientCredential允许只用clientScret来获取
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "Api1","Api2" }
                },
                new Client
                {
                    ClientId = "Meitu API",
                    //类似于客户端密码和id
                    ClientSecrets = new [] { new Secret("1".Sha256()) },
                    //OwnerPassword 是通过用户密码来获得token
                    //ClientCredential允许只用clientScret来获取
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "Api1","Api2", IdentityServerConstants.StandardScopes.OpenId}
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                    RequireConsent = false,

                    ClientSecrets =
                    {
                         new Secret("secret".Sha256())
                    },

                     RedirectUris           = { "http://localhost:5002/signin-oidc" },
                     PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                     AllowedScopes =
                     {
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile
                     },

                }
            };
        }
    }
}
