using Massive.Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace Massive.Client.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassiveClient(
            this IServiceCollection services,
            string apiKey,
            string baseUrl = MassiveClient.DefaultBaseUrl)
        {
            services.AddHttpClient<IMassiveClient, MassiveClient>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);

                if (apiKey.StartsWith("Bearer "))
                {
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(apiKey);
                }
                else
                {
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {apiKey}");
                }
            });

            return services;
        }
    }
}
