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
        static string AzureAdAuthenticationUrl = "https://login.windows.net/";
        static string ClientId = "Enter Client Id";
        static string ClientSecret = "Enter secret";

        static void Main(string[] args)
        {
            var graphClient = GetGraphClient();
            Console.WriteLine("Graph client acquired...");

            DisplayUsers(graphClient).Wait();

            Console.WriteLine("Ready.");
            Console.ReadKey();
        }

        private async static Task DisplayUsers(ActiveDirectoryClient graphClient)
        {
            List<IUser> users = new List<IUser>();
            var usersPagedCollection = await graphClient.Users.ExecuteAsync();

            do
            {
                users.AddRange(usersPagedCollection.CurrentPage.ToList());
                usersPagedCollection = await usersPagedCollection.GetNextPageAsync();
            } while (usersPagedCollection != null && usersPagedCollection.MorePagesAvailable);

            foreach (var item in users)
            {
                Console.WriteLine($"User: {item.DisplayName} {item.Mail}");
            }
        }

        private static ActiveDirectoryClient GetGraphClient()
        {
            ActiveDirectoryClient graphClient = new ActiveDirectoryClient(new Uri("https://graph.windows.net/onebit101.onmicrosoft.com"), () => GetAccessToken());
            return graphClient;
        }

        private async static Task<string> GetAccessToken()
        {
            AuthenticationContext authContext = new AuthenticationContext(AzureAdAuthenticationUrl + "onebit.onmicrosoft.com", true);

            ClientCredential credentials = new ClientCredential(ClientId, ClientSecret);

            var result = await authContext.AcquireTokenAsync("https://graph.windows.net", credentials);

            return result.AccessToken;
        }
    }
}
