﻿using Microsoft.Extensions.DependencyInjection;
using Polygon.Client.Interfaces;
using System;
using System.Net.Http.Headers;

namespace Polygon.Client.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPolygonClient(this IServiceCollection services, string bearerToken)
        {
            services.AddHttpClient<IPolygonClient, PolygonClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.polygon.io");

                if (bearerToken.StartsWith("Bearer "))
                {
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(bearerToken);
                }
                else
                {
                    client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {bearerToken}");
                }
            });

            return services;
        }   
    }
}
