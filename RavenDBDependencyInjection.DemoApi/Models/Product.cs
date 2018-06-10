using System;

namespace RavenDBDependencyInjection.DemoApi.Models
{
	public class Product
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; }
	}
}