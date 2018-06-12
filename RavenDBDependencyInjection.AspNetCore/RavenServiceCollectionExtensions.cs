using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace RavenDBDependencyInjection.AspNetCore
{
	public static class RavenServiceCollectionExtensions
	{
		/// <summary>
		/// Add RavenDB service
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		/// <param name="options">Parameters for RavenDB</param>
		/// <param name="registerAssemblyIndexes">Assembly that contains the index objects</param>
		public static void AddRavenDb(this IServiceCollection services, Action<RavenConfig> options, Assembly registerAssemblyIndexes = null)
		{
			services.AddSingleton<IDocumentStore>(p => DocumentStoreHolder.LazyDocumentStore(options, registerAssemblyIndexes).Value);
			services.AddScoped<IDocumentSession>(p => p.GetRequiredService<IDocumentStore>().OpenSession());
		}

		/// <summary>
		/// Add RavenDB service
		/// </summary>
		/// <param name="services">IServiceCollection instance</param>
		/// <param name="options">Parameters for RavenDB</param>
		/// <param name="registerAssemblyIndexes">Assembly that contains the index objects</param>
		public static void AddRavenDbAsync(this IServiceCollection services, Action<RavenConfig> options, Assembly registerAssemblyIndexes = null)
		{
			services.AddSingleton<IDocumentStore>(p => DocumentStoreHolder.LazyDocumentStore(options, registerAssemblyIndexes).Value);
			services.AddScoped<IAsyncDocumentSession>(p => p.GetRequiredService<IDocumentStore>().OpenAsyncSession());
		}
	}
}