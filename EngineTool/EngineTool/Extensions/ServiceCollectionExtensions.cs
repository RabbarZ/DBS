using EngineTool.DataAccess.Entities;
using EngineTool.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EngineTool.Config;
using EngineTool.Interfaces;
using EngineTool.Services;
using Microsoft.Extensions.Options;

namespace EngineTool.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfiguration(this IServiceCollection serviceCollection)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            serviceCollection.AddSingleton(_ => config);

            serviceCollection.Configure<IgdbApiSettings>(config.GetRequiredSection("IgdbApi"));
        }

        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IIgdbService, IgdbService>();
            serviceCollection.AddHttpClient<IIgdbService, IgdbService>((serviceProvider, httpClient) =>
            {
                IgdbApiSettings apiSettings = serviceProvider.GetRequiredService<IOptions<IgdbApiSettings>>().Value;
                httpClient.DefaultRequestHeaders.Add("Client-ID", apiSettings.ClientId);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiSettings.BearerToken}");
            });

            serviceCollection.AddTransient<ISteamService, SteamService>();
        }
    }
}
