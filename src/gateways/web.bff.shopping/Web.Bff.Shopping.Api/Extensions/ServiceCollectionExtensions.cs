﻿using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using GreenShop.Web.Bff.Shopping.Api.Services;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Consumers;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace GreenShop.Web.Bff.Shopping.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterHttpServices(this IServiceCollection services)
        {
            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        /// <summary>
        /// Distinct Dependency Injection Block.
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        internal static void InjectDependencies(this IServiceCollection services)
        {
            services.RegisterSingletones();
            services.RegisterScoped();
            services.RegisterTransient();
        }

        /// <summary>
        /// Method that registers all Singleton-type dependencies
        /// <para>Singleton objects are the same for every object and every request.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private static void RegisterSingletones(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }


        /// <summary>
        /// Method that registers all Scoped-type dependencies
        /// <para>Scoped objects are the same within a request, but different across different requests.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private static void RegisterScoped(this IServiceCollection services)
        {
        }

        /// <summary>
        /// Method that registers all Transient-type dependencies.
        /// <para>Transient objects are provided as a new instance to every controller and every service.</para>
        /// </summary>
        /// <param name="services">Service Collection to inject dependencies into.</param>
        private static void RegisterTransient(this IServiceCollection services)
        {
            services.AddTransient<IConsumer<Category>, CategoriesConsumer>();
            services.AddTransient<IConsumer<Product>, ProductsConsumer>();
            services.AddTransient<ICommentsConsumer, CommentsConsumer>();
            services.AddTransient<ICatalogService, CatalogService>();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}