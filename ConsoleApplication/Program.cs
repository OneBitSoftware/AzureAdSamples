using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static string AzureAdAuthenticationUrl  = "https://login.windows.net/";
        static string GraphApiUrl               = "https://graph.windows.net/";
        static string TenantDomain              = "CIECOM386698.onmicrosoft.com";
        static string ClientId                  = "Enter Client ID";
        static string ClientSecret              = "Enter Client Secret";

        static void Main(string[] args)
        {
            var graphClient = GetGraphClient();
            Console.WriteLine("Graph client acquired...");

            DisplayUsers(graphClient).Wait();

            Console.WriteLine("Ready.");
            Console.ReadKey();
        }

        /// <summary>
        /// Gets users and displays them to the console 
        /// </summary>
        /// <param name="aadClient">An ActiveDirectoryClient</param>
        /// <returns></returns>
        private async static Task DisplayUsers(ActiveDirectoryClient aadClient)
        {
            List<IUser> users = new List<IUser>();
            var usersPagedCollection = await aadClient.Users.ExecuteAsync();

            do
            {
                users.AddRange(usersPagedCollection.CurrentPage.ToList());
                usersPagedCollection = await usersPagedCollection.GetNextPageAsync();
            } while (
                usersPagedCollection != null && usersPagedCollection.MorePagesAvailable
            );

            foreach (var item in users)
            {
                Console.WriteLine($"User: {item.DisplayName} Upn: {item.UserPrincipalName}");
            }
        }

        /// <summary>
        /// Returns an ActiveDirectoryClient based on the Graph URL and Tenant domain
        /// </summary>
        /// <returns></returns>
        private static ActiveDirectoryClient GetGraphClient()
        {
            ActiveDirectoryClient graphClient = 
                new ActiveDirectoryClient(
                    new Uri(GraphApiUrl + TenantDomain),
                    () => GetAccessToken());
            return graphClient;
        }

        /// <summary>
        /// Grabs an access token by using a ClientID and ClientSecret
        /// </summary>
        /// <returns></returns>
        private async static Task<string> GetAccessToken()
        {
            AuthenticationContext authContext = 
                new AuthenticationContext(AzureAdAuthenticationUrl + TenantDomain, true);

            ClientCredential credentials = new ClientCredential(ClientId, ClientSecret);

            var tokenResult = await authContext.AcquireTokenAsync(GraphApiUrl, credentials);

            return tokenResult.AccessToken;
        }
    }
}
