using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace RavenDBDependencyInjection.AspNetCore
{
	public static class RavenDocumentStoreExtensions
	{
		public static void CreateDatabase(this IDocumentStore store, string database)
		{
			store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
		}

		public static async Task CreateDatabaseAsync(this IDocumentStore store, string database)
		{
			await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(database)));
		}

		public static void DeleteDatabase(this IDocumentStore store, string database)
		{
			store.Maintenance.Server.Send(new DeleteDatabasesOperation(database, true));
		}

		public static async Task DeleteDatabaseAsync(this IDocumentStore store, string database)
		{
			await store.Maintenance.Server.SendAsync(new DeleteDatabasesOperation(database, true));
		}

		public static void RegisterIndexes(this IDocumentStore store, Assembly assemblyName)
		{
			var types = GetIndexes(assemblyName);

			foreach (var type in types)
			{
				IndexCreation.CreateIndexes(type.Assembly, store);
			}
		}

		public static async Task RegisterIndexesAsync(this IDocumentStore store, Assembly assemblyName)
		{
			var types = GetIndexes(assemblyName);

			foreach (var type in types)
			{
				await IndexCreation.CreateIndexesAsync(type.Assembly, store);
			}
		}

		public static void CreateIndex<T>(this IDocumentStore store) where T : AbstractIndexCreationTask
		{
			IndexCreation.CreateIndexes(typeof(T).Assembly, store);
		}

		public static async Task CreateIndexAsync<T>(this IDocumentStore store) where T : AbstractIndexCreationTask
		{
			await IndexCreation.CreateIndexesAsync(typeof(T).Assembly, store);
		}

		public static void DeleteIndex<T>(this IDocumentStore store) where T : AbstractIndexCreationTask
		{
			var indexName = GetIndexName<T>();
			store.Maintenance.Send(new DeleteIndexOperation(indexName));
		}

		public static async Task DeleteIndexAsync<T>(this IDocumentStore store) where T : AbstractIndexCreationTask
		{
			var indexName = GetIndexName<T>();
			await store.Maintenance.SendAsync(new DeleteIndexOperation(indexName));
		}

		public static bool CheckIfExistsIndex<T>(this IDocumentStore store) where T : AbstractIndexCreationTask
		{
			var indexName = GetIndexName<T>();
			var result = store.Maintenance.Send(new GetIndexOperation(indexName));
			return result != null;
		}
		public static async Task<bool> CheckIfExistsIndexAsync<T>(this IDocumentStore store) where T : AbstractIndexCreationTask
		{
			var indexName = GetIndexName<T>();
			var result = await store.Maintenance.SendAsync(new GetIndexOperation(indexName));
			return result != null;
		}

		public static string[] GetExistingIndexes(this IDocumentStore store, int maxIndexes = 100)
		{
			var indexes = store.Maintenance.Send(new GetIndexesOperation(0, maxIndexes));
			return indexes.Select(p => p.Name).ToArray();
		}

		public static async Task<string[]> GetExistingIndexesAsync(this IDocumentStore store, int maxIndexes = 100)
		{
			var indexes = await  store.Maintenance.SendAsync(new GetIndexesOperation(0, maxIndexes));
			return indexes.Select(p => p.Name).ToArray();
		}

		private static string GetIndexName<T>() where T : AbstractIndexCreationTask
		{
			var indexName = typeof(T).Name.Replace("_", "/");
			return indexName;
		}

		private static IEnumerable<Type> GetIndexes(Assembly assemblyName)
		{
			var types = assemblyName.GetTypes()
				.Where(p => p.IsSubclassOf(typeof(AbstractIndexCreationTask))
				            || p.IsSubclassOf(typeof(AbstractIndexCreationTask<>)));

			return types;
		}
	}
}