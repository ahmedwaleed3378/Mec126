using System.ComponentModel.DataAnnotations;

namespace Mec126.Models.DTO
{
	public class PostProductDTO
	{
		[Required(ErrorMessage = "Name should be written.")]
		[MaxLength(100, ErrorMessage = "Name cannot be more than 100 characters.")]
		public string Name { get; init; } = "";

		[Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
		public decimal Price { get; init; }

		[MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
		public string? Description { get; init; }



		[Required]
		[EnumDataType(typeof(Category))]
		public string? Category { get; init; }
	}



	public enum Category
	{
		Electronics, 
		CLothes,
		Food,
		Furniture
	}
}





