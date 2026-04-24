namespace Mec126.Models.DTO
{
	public class PostProductDTO
	{
		public string Name { get; init; } 
		public decimal Price { get; init; }

		public string? Description { get; init; }
		public string? Category { get; init; }
	}
}
