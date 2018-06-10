**RavenDB Dependency Injection for ASP Net Core 2.x**

***How to use:***

```c#
public void ConfigureServices(IServiceCollection services)
{
    // for sync module IDocumentSession 
    services.AddRavenDb(p =>
    {
        p.Urls = new[] { "http://localhost:8080" };
        p.Database = "your_database";
    });
    
    // for async module IAsyncDocumentSession 
    services.AddRavenDbAsync(p =>
    {
        p.Urls = new[] { "http://localhost:8080" };
        p.Database = "your_database";
    });
}
```

Add injection in your controller:

```c#

private readonly IDocumentSession _session;
//private readonly IAsyncDocumentSession _session;

public DemoController(IDocumentSession session)
{
   _session = session;
}
```

***More information***

> AddRavenDb contains a delegate with the main options to connect to RavenDB:

- Urls (if url is null then by default it will be "http://localhost:8080")
- Database name
- Certificate (X509Certificate2)
- Conventions (for default is new Conventions())
- Identifier

**EXTRAS**

> In this project there are some extensions methods:

- CreateDatabase / CreateDatabaseAsync
- DeleteDatabse / DeleteDatabseAsync
- CreateIndex / CreateIndexAsync
- RegisterIndexes and more...

***See the WebAPI demo project.***
