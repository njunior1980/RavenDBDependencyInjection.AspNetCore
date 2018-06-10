using System;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace RavenDBDependencyInjection.AspNetCore
{
	public static class RavenServiceCollectionExtensions
	{
		public static void AddRavenDb(this IServiceCollection services, Action<RavenConfig> options)
		{
			services.AddSingleton<IDocumentStore>(p => DocumentStoreHolder.LazyDocumentStore(options).Value);
			services.AddScoped<IDocumentSession>(p => p.GetRequiredService<IDocumentStore>().OpenSession());
		}

		public static void AddRavenDbAsync(this IServiceCollection services, Action<RavenConfig> options)
		{
			services.AddSingleton<IDocumentStore>(p => DocumentStoreHolder.LazyDocumentStore(options).Value);
			services.AddScoped<IAsyncDocumentSession>(p => p.GetRequiredService<IDocumentStore>().OpenAsyncSession());
		}
	}
}