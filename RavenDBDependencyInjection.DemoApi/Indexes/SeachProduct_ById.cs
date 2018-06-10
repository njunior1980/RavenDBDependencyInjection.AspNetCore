using System.Linq;
using Raven.Client.Documents.Indexes;
using RavenDBDependencyInjection.DemoApi.Models;

namespace RavenDBDependencyInjection.DemoApi.Indexes
{
	public class SeachProduct_ById : AbstractIndexCreationTask<Product>
	{
		public SeachProduct_ById()
		{
			Map = occupations => occupations.Select(p => new
			{
				p.Id,
				p.Name
			});

			Index(p => p.Id, FieldIndexing.Search);
		}
	}
}