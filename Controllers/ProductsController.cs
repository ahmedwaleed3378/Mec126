using Mec126.Auth;
using Mec126.Common.Exceptions;
using Mec126.Common.Extensions;
using Mec126.Common.Responses;
using Mec126.Models;
using Mec126.Models.Data;
using Mec126.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mec126.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ProductsController : ControllerBase
	{
		private readonly AppDbContext _context;

		public ProductsController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Authorize(Roles = AuthRoles.ReadProducts)]
		public async Task<ActionResult<ApiResponse<PagedResult<ProductsDTO>>>> Get(
			[FromQuery] decimal? minPrice,
			[FromQuery] string sortBy = "id",
			[FromQuery] string sortDir = "asc",
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10)
		{
			if (page < 1) page = 1;
			if (pageSize < 1) pageSize = 10;
			if (pageSize > 100) pageSize = 100;

			IQueryable<Product> query = _context.Products;

			if (minPrice is not null && minPrice > 0)
				query = query.Where(p => p.Price >= minPrice.Value);

			// Cast price for ORDER BY — SQLite cannot sort decimal columns directly.
			query = (sortBy.ToLowerInvariant(), sortDir.ToLowerInvariant()) switch
			{
				("name", "desc") => query.OrderByDescending(p => p.Name),
				("name", _) => query.OrderBy(p => p.Name),
				("price", "desc") => query.OrderByDescending(p => (double)p.Price),
				("price", _) => query.OrderBy(p => (double)p.Price),
				("id", "desc") => query.OrderByDescending(p => p.Id),
				_ => query.OrderBy(p => p.Id),
			};

			var totalCount = await query.CountAsync();

			var items = await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(p => new ProductsDTO
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.Price,
				})
				.ToListAsync();

			var paged = new PagedResult<ProductsDTO>
			{
				Items = items,
				Page = page,
				PageSize = pageSize,
				TotalCount = totalCount,
			};

			return Ok(ApiResponse<PagedResult<ProductsDTO>>.Ok(paged));
		}

		[HttpGet("{id:int}")]
		[Authorize(Roles = AuthRoles.ReadProducts)]
		public async Task<ActionResult<ApiResponse<Product>>> GetById(int id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (product is null)
				throw new NotFoundException($"Product with id {id} was not found.");

			return Ok(ApiResponse<Product>.Ok(product));
		}

		[HttpPost]
		[Authorize(Roles = AuthRoles.Admin)]
		public async Task<ActionResult<ApiResponse<Product>>> Post([FromBody] PostProductDTO postProduct)
		{
			if (await _context.Products.AnyAsync(p => p.Name.ToLower() == postProduct.Name.ToLower()))
				throw new ConflictException("A product with the same name already exists.");

			if (!ModelState.IsValid)
				return this.ValidationFail<Product>();

			var created = new Product
			{
				Name = postProduct.Name,
				Price = postProduct.Price,
				Category = postProduct.Category ?? "",
				Description = postProduct.Description ?? "",
			};

			_context.Products.Add(created);
			await _context.SaveChangesAsync();

			return CreatedAtAction(
				nameof(GetById),
				new { id = created.Id },
				ApiResponse<Product>.Ok(created, "Product created successfully."));
		}

		[HttpPut("{id:int}")]
		[Authorize(Roles = AuthRoles.Admin)]
		public async Task<ActionResult<ApiResponse<Product>>> Update(int id, [FromBody] PostProductDTO data)
		{
			var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (existing is null)
				throw new NotFoundException($"Product with id {id} was not found.");

			if (await _context.Products.AnyAsync(p => p.Id != id && p.Name.ToLower() == data.Name.ToLower()))
				throw new ConflictException("A product with the same name already exists.");

			if (!ModelState.IsValid)
				return this.ValidationFail<Product>();

			existing.Name = data.Name;
			existing.Price = data.Price;
			existing.Category = data.Category ?? "";
			existing.Description = data.Description ?? "";

			await _context.SaveChangesAsync();

			return Ok(ApiResponse<Product>.Ok(existing, "Product updated successfully."));
		}

		[HttpDelete("{id:int}")]
		[Authorize(Roles = AuthRoles.Admin)]
		public async Task<ActionResult<ApiResponse>> Delete(int id)
		{
			var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (existing is null)
				throw new NotFoundException($"Product with id {id} was not found.");

			_context.Products.Remove(existing);
			await _context.SaveChangesAsync();

			return Ok(ApiResponse.Ok("Product deleted successfully."));
		}
	}
}
