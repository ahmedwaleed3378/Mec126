using Mec126.Models;
using Mec126.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Mec126.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private static readonly List<Product> _porducts = [
				new () {Id = 1, Name = "Keyboard", Price=25.0m, Description= "This is a keyboard" , Category= "Tech"},
				new () {Id = 2, Name = "Mouse", Price=12.0m, Description= "This is a keyboard" , Category= "Tech"},
				new () {Id = 3, Name = "Monitor", Price=20.0m, Description= "This is a keyboard" , Category= "Tech"},
				
			];






		[HttpGet]
		public ActionResult<List<ProductsDTO>> Get(
				[FromQuery] decimal? minPrice
			) {
			IEnumerable<Product> query = _porducts;

			if (minPrice is not null && minPrice > 0)
			{
				query = query.Where(p => p.Price >= minPrice.Value);
			}

			// mapping is transforming db model or entity into the DTO 

			var result = query.Select(p=> new ProductsDTO
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
			}).ToList();

			return Ok(result);
		}
		
		
		//Get api/products/2
		[HttpGet("{id:int}")]
		public ActionResult<Product> GetById(int id ) {


			IEnumerable<Product> query = _porducts;

			var product = query.FirstOrDefault(p=>p.Id ==id);
				
			 return Ok(product);
		}




		[HttpPost]
		public ActionResult<List<Product>> Post([FromBody] PostProductDTO postProduct)
		{
			
			if (_porducts.Any(p => p.Name.Equals(postProduct.Name, StringComparison.OrdinalIgnoreCase)))
				ModelState.AddModelError(nameof(postProduct.Name), "A product with the same name already exists.");

			if (!ModelState.IsValid)
				return ValidationProblem(ModelState);
			//BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0)
			var created = new Product
			{
				Id =_porducts.Count == 0 ? 1 : _porducts.Max(p=> p.Id)+1 ,
				Name = postProduct.Name,
				Price = postProduct.Price,
				Category = postProduct.Category ?? "",
				Description = postProduct.Description ?? ""
			};


			_porducts.Add(created);

			return CreatedAtAction(nameof(GetById), new {id = created.Id}, created);
		}



		[HttpPut("{id:int}")]
		public ActionResult<Product> Update( int id, [FromBody] PostProductDTO data)
		{
			var existing = _porducts.FirstOrDefault(p => p.Id == id);
			if (existing is null )  
			{
				return NotFound();
			}

			if (_porducts.Any( p=> p.Id != id &&  p.Name.Equals(data.Name, StringComparison.OrdinalIgnoreCase)))
				ModelState.AddModelError(nameof(data.Name), "A product with the same name already exists.");

			if (!ModelState.IsValid)
				return ValidationProblem(ModelState);

			existing.Name = data.Name;
			existing.Price  = data.Price;
			existing.Category =data.Category;
			existing.Description =data.Description;


			return Ok (existing);

		}


		[HttpDelete("{id:int}")]
		public ActionResult<Product> Delete(int id)
		{
			var existing = _porducts.FirstOrDefault(p => p.Id == id);
			if (existing is null)
			{
				return NotFound();
			}


			_porducts.Remove(existing);
			return NoContent();
		}


		}
}
