using System;
using Raven.Client.Documents;

namespace RavenDBDependencyInjection.AspNetCore
{
	public sealed class DocumentStoreHolder
	{
		public static Lazy<IDocumentStore> LazyDocumentStore(Action<RavenConfig> options)
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

				return store;
			});
		}
	}
}