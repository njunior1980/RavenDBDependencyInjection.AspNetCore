using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using RavenDBDependencyInjection.AspNetCore;
using RavenDBDependencyInjection.DemoApi.Indexes;
using RavenDBDependencyInjection.DemoApi.Models;

namespace RavenDBDependencyInjection.DemoApi.Controllers
{
    [Route("api/[controller]")]
    public class DemoController : Controller
    {
		private readonly IDocumentStore _store;
	    private readonly IAsyncDocumentSession _session;

		public DemoController(IDocumentStore store, IAsyncDocumentSession session)
		{
			_store = store;
			_session = session;
		}

		// create a database
		[HttpPost("create-database")]
        public async Task<IActionResult> CreateDatabase([FromBody] string database)
		{
			try
			{
				await _store.CreateDatabaseAsync(database);
				return Ok($"{database} created!");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
        }

		// delete a database
	    [HttpPost("delete-database")]
	    public async Task<IActionResult> DeleteDatabase([FromBody] string database)
	    {
		    try
		    {
			    await _store.DeleteDatabaseAsync(database);
			    return Ok($"{database} deleted!");
		    }
		    catch (Exception ex)
		    {
			    return BadRequest(ex.Message);
		    }
	    }

	    // This will create a new index in current database connected
		[HttpPost("create-indexes")]
		public async Task<IActionResult> CreateIndexes()
	    {
		    try
		    {
			    var assembly = Assembly.GetExecutingAssembly();
			    await _store.RegisterIndexesAsync(assembly);
			    return Ok("Indexes created!");
		    }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	    // This will register a new product in current database connected
		[HttpPost("register-product")]
	    public async Task<IActionResult> RegisterProduct([FromBody] Product product)
	    {
		    try
		    {
			    await _session.StoreAsync(product);
			    await _session.SaveChangesAsync();
				return Ok(product);
		    }
		    catch (Exception ex)
		    {
			    return BadRequest(ex.Message);
		    }
	    }

	    // This will get all products in current database connected
		[HttpGet("products")]
	    public async Task<IActionResult> GetProducts()
	    {
		    try
		    {
			    var products = await _session.Query<Product>().ToListAsync();
			    return Ok(products);
		    }
		    catch (Exception ex)
		    {
			    return BadRequest(ex.Message);
		    }
		}
		
	    // This will get one product in current database connected
		[HttpGet("products/{id}")]
	    public async Task<IActionResult> GetProductsById(string id)
	    {
		    try
		    {
			    var products = await _session.Advanced
				    .AsyncDocumentQuery<Product, SeachProduct_ById>()
				    .WhereEquals(p => p.Id, id).FirstOrDefaultAsync();

				return Ok(products);
		    }
		    catch (Exception ex)
		    {
			    return BadRequest(ex.Message);
		    }
	    }
	}
}
