using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RavenDBDependencyInjection.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;

namespace RavenDBDependencyInjection.DemoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

	    private void ConfigureRavenDb(IServiceCollection services)
	    {
			// first create your database here: http://live-test.ravendb.net

			services.AddRavenDbAsync(p =>
		    {
			    p.Urls = new[] { "http://localhost:8080" };
			    p.Database = "demo"; 
			});
		}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
	            .AddJsonOptions(opt =>
	            {
		            opt.SerializerSettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
		            opt.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
		            opt.SerializerSettings.Formatting = Formatting.Indented;
		            opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		            opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
	            });

			services.AddSwaggerGen(p =>
	        {
		        p.SwaggerDoc("v1", new Info { Title = "RavenDBDependencyInjection.DemoApi", Version = "v1.0" });
	        });

	        ConfigureRavenDb(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

	        app.UseSwagger();
	        app.UseSwaggerUI(s =>
	        {
		        s.SwaggerEndpoint("/swagger/v1/swagger.json", "RavenDBDependencyInjection.DemoApi");
	        });
		}
    }
}
