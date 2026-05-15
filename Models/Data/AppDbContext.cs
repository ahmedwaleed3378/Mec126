using Mec126.Models;
using Microsoft.EntityFrameworkCore;

namespace Mec126.Models.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		public DbSet<Product> Products => Set<Product>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>(entity =>
			{
				entity.ToTable("Products");
				entity.HasKey(p => p.Id);

				entity.Property(p => p.Id)
					.ValueGeneratedOnAdd();

				entity.Property(p => p.Name)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(p => p.Price)
					.HasColumnType("decimal(18,2)");

				entity.Property(p => p.Description)
					.HasMaxLength(500);

				entity.Property(p => p.Category)
					.IsRequired()
					.HasMaxLength(50);

				//////////////////////////////
				///



				var productsSeeding = new List<Product>
{
	new Product
	{
		Id = 1,
		Name = "Gaming Laptop",
		Price = 1200.99m,
		Description = "High performance laptop for gaming and development.",
		Category = "Electronics"
	},
	new Product
	{
		Id = 2,
		Name = "Wireless Headphones",
		Price = 149.50m,
		Description = "Noise cancelling over-ear wireless headphones.",
		Category = "Electronics"
	},
	new Product
	{
		Id = 3,
		Name = "Office Chair",
		Price = 320.00m,
		Description = "Ergonomic office chair with lumbar support.",
		Category = "Furniture"
	},
	new Product
	{
		Id = 4,
		Name = "Mechanical Keyboard",
		Price = 89.99m,
		Description = "RGB mechanical keyboard with blue switches.",
		Category = "Accessories"
	},
	new Product
	{
		Id = 5,
		Name = "Smart Watch",
		Price = 210.75m,
		Description = "Fitness tracking smart watch with AMOLED display.",
		Category = "Wearables"
	},
	new Product
	{
		Id = 6,
		Name = "Coffee Maker",
		Price = 65.25m,
		Description = "Automatic coffee maker with timer feature.",
		Category = "Home Appliances"
	},
	new Product
	{
		Id = 7,
		Name = "Running Shoes",
		Price = 95.00m,
		Description = "Lightweight running shoes for daily workouts.",
		Category = "Sports"
	},
	new Product
	{
		Id = 8,
		Name = "Bluetooth Speaker",
		Price = 55.49m,
		Description = "Portable waterproof Bluetooth speaker.",
		Category = "Electronics"
	},
	new Product
	{
		Id = 9,
		Name = "Backpack",
		Price = 40.00m,
		Description = "Durable backpack suitable for travel and work.",
		Category = "Bags"
	},
	new Product
	{
		Id = 10,
		Name = "Desk Lamp",
		Price = 27.99m,
		Description = "LED desk lamp with adjustable brightness.",
		Category = "Furniture"
	}
};

				entity.HasData(
					productsSeeding.ToArray()
					);

			});
		}
	}
}
