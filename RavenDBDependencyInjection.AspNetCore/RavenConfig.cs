using System.Security.Cryptography.X509Certificates;
using Raven.Client.Documents.Conventions;

namespace RavenDBDependencyInjection.AspNetCore
{
    public class RavenConfig
    {
	    public string[] Urls { get; set; }
	    public string Database { get; set; }
	    public DocumentConventions Conventions { get; set; }
	    public X509Certificate2 Certificate { get; set; }
	    public string Identifier { get; set; }

	    public RavenConfig()
	    {
		    this.Urls = this.Urls ?? new[] {"http://localhost:8080"};
		    this.Database = Database;
		    this.Identifier = Identifier;
		    this.Conventions = Conventions;
		    this.Certificate = Certificate;
	    }
	}
}
