using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using aspnetcore_mongo.Services;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Management.CosmosDB.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace aspnetcore_mongo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=MyItem}/{action=Index}/{id?}");
            });
        }

        private static CosmosDbService InitializeCosmosClientInstanceAsync()
        {
            string subscriptionId = "937bc588-a144-4083-8612-5f9ffbbddb14";
            string resourceGroupName = "servicelinker-test-win-group";
            string accountName = "servicelinker-mongo-cosmos";

            string resourceEndpoint = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTMONGOUSERASSIGNEDIDENTITYCONNECTIONSUCCEEDED_RESOURCEENDPOINT");
            string scope = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTMONGOUSERASSIGNEDIDENTITYCONNECTIONSUCCEEDED_SCOPE");
            string tenentId = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTMONGOUSERASSIGNEDIDENTITYCONNECTIONSUCCEEDED_CLIENTID");
            string clientId = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTMONGOUSERASSIGNEDIDENTITYCONNECTIONSUCCEEDED_CLIENTID");
            string clientSecret = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTMONGOUSERASSIGNEDIDENTITYCONNECTIONSUCCEEDED_CLIENTID");

            string accessToken = GetAccessTokenByMsIdentity(scope, tenentId, clientId, clientSecret);

            string endpoint = $"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.DocumentDB/databaseAccounts/{accountName}/listConnectionStrings?api-version=2019-12-12";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage result = httpClient.PostAsync(endpoint, new StringContent("")).Result;
            DatabaseAccountListConnectionStringsResult connStrResult = result.Content.ReadAsAsync<DatabaseAccountListConnectionStringsResult>().Result;

            foreach (DatabaseAccountConnectionString connStr in connStrResult.ConnectionStrings)
            {
                if (connStr.Description.Contains("Primary") && connStr.Description.Contains("MongoDB"))
                {
                    return new CosmosDbService(connStr.ConnectionString);
                }
            }
            return null;
        }

        private static string GetAccessTokenByMsIdentity(string scope, string tenentId, string clientId, string secret)
        {
            ClientSecretCredential cred = new ClientSecretCredential(tenentId, clientId, secret);
            TokenRequestContext reqContext = new TokenRequestContext(new string[] { scope });
            AccessToken token = cred.GetTokenAsync(reqContext).Result;
            return token.Token;
        }


    }
}
