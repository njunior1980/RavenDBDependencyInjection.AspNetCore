using System;
using System.Reflection;
using Raven.Client.Documents;

namespace RavenDBDependencyInjection.AspNetCore
{
	public sealed class DocumentStoreHolder
	{
		public static Lazy<IDocumentStore> LazyDocumentStore(Action<RavenConfig> options, Assembly registerAssemblyIndexes)
		{
			var settings = new RavenConfig();
			options.Invoke(settings);

			return new Lazy<IDocumentStore>(() =>
			{
				var store = new DocumentStore
				{
					Urls = settings.Urls,
					Database = settings.Database,
					Certificate = settings.Certificate,
					Identifier = settings.Identifier,
					Conventions = settings.Conventions
				};

				store.Initialize();

				if (registerAssemblyIndexes != null)
				{
					store.RegisterIndexes(registerAssemblyIndexes);
				}

				return store;
			});
		}
	}
}